namespace Paraminter.Associating.Processing;

using Moq;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Processing.Commands;

using System;

using Xunit;

public sealed class Handle
{
    [Fact]
    public void NullCommand_ThrowsArgumentNullException()
    {
        var fixture = FixtureFactory.Create<IAssociateArgumentsData>();

        var result = Record.Exception(() => Target(fixture, null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidCommand_ResetsInitiationAfter()
    {
        var fixture = FixtureFactory.Create<IAssociateArgumentsData>();

        var command = Mock.Of<IAssociateArgumentsCommand<IAssociateArgumentsData>>();

        var sequence = new MockSequence();

        fixture.DecorateeMock.InSequence(sequence).Setup((handler) => handler.Handle(command));
        fixture.InitiationResetterMock.InSequence(sequence).Setup(static (handler) => handler.Handle(It.IsAny<IResetProcessInitiationCommand>()));

        Target(fixture, command);

        fixture.DecorateeMock.Verify((handler) => handler.Handle(command), Times.Once());
        fixture.InitiationResetterMock.Verify(static (handler) => handler.Handle(It.IsAny<IResetProcessInitiationCommand>()), Times.Once());
    }

    private static void Target<TData>(
        IFixture<TData> fixture,
        IAssociateArgumentsCommand<TData> command)
        where TData : IAssociateArgumentsData
    {
        fixture.Sut.Handle(command);
    }
}

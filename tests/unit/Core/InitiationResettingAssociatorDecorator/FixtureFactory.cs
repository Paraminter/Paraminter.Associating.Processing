namespace Paraminter.Associating.Processing;

using Moq;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

internal static class FixtureFactory
{
    public static IFixture<TData> Create<TData>()
        where TData : IAssociateArgumentsData
    {
        Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> decorateeMock = new(MockBehavior.Strict);

        Mock<ICommandHandler<IResetProcessInitiationCommand>> initiationResetterMock = new(MockBehavior.Strict);

        var sut = new InitiationResettingAssociatorDecorator<TData>(decorateeMock.Object, initiationResetterMock.Object);

        return new Fixture<TData>(sut, decorateeMock, initiationResetterMock);
    }

    private sealed class Fixture<TData>
        : IFixture<TData>
        where TData : IAssociateArgumentsData
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Sut;

        private readonly Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> DecorateeMock;

        private readonly Mock<ICommandHandler<IResetProcessInitiationCommand>> InitiationResetterMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<TData>> sut,
            Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> decorateeMock,
            Mock<ICommandHandler<IResetProcessInitiationCommand>> initiationResetterMock)
        {
            Sut = sut;

            DecorateeMock = decorateeMock;

            InitiationResetterMock = initiationResetterMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<TData>> IFixture<TData>.Sut => Sut;

        Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> IFixture<TData>.DecorateeMock => DecorateeMock;

        Mock<ICommandHandler<IResetProcessInitiationCommand>> IFixture<TData>.InitiationResetterMock => InitiationResetterMock;
    }
}

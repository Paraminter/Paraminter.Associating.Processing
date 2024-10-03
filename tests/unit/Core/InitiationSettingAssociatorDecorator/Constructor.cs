namespace Paraminter.Associating.Processing;

using Moq;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullDecoratee_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target<IAssociateArgumentsData>(
            null!,
            Mock.Of<ICommandHandler<ISetProcessInitiationCommand>>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullInitiationSetter_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(
            Mock.Of<ICommandHandler<IAssociateArgumentsCommand<IAssociateArgumentsData>>>(),
            null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsHandler()
    {
        var result = Target(
            Mock.Of<ICommandHandler<IAssociateArgumentsCommand<IAssociateArgumentsData>>>(),
            Mock.Of<ICommandHandler<ISetProcessInitiationCommand>>());

        Assert.NotNull(result);
    }

    private static InitiationSettingAssociatorDecorator<TData> Target<TData>(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<ISetProcessInitiationCommand> initiationSetter)
        where TData : IAssociateArgumentsData
    {
        return new InitiationSettingAssociatorDecorator<TData>(decoratee, initiationSetter);
    }
}

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
            Mock.Of<ICommandHandler<IResetProcessCompletionCommand>>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullCompletionResetter_ThrowsArgumentNullException()
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
            Mock.Of<ICommandHandler<IResetProcessCompletionCommand>>());

        Assert.NotNull(result);
    }

    private static CompletionResettingAssociatorDecorator<TData> Target<TData>(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<IResetProcessCompletionCommand> completionResetter)
        where TData : IAssociateArgumentsData
    {
        return new CompletionResettingAssociatorDecorator<TData>(decoratee, completionResetter);
    }
}

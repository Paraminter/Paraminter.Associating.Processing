﻿namespace Paraminter.Associating.Processing;

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
            Mock.Of<ICommandHandler<ISetProcessCompletionCommand>>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullCompletionSetter_ThrowsArgumentNullException()
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
            Mock.Of<ICommandHandler<ISetProcessCompletionCommand>>());

        Assert.NotNull(result);
    }

    private static CompletionSettingAssociatorDecorator<TData> Target<TData>(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<ISetProcessCompletionCommand> completionSetter)
        where TData : IAssociateArgumentsData
    {
        return new CompletionSettingAssociatorDecorator<TData>(decoratee, completionSetter);
    }
}
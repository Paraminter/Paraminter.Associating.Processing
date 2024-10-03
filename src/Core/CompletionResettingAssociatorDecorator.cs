namespace Paraminter.Associating.Processing;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Associating.Processing.Commands;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

using System;

/// <summary>Decorates an associator by resetting the completion status before invoking the decoratee.</summary>
/// <typeparam name="TData">The type representing the data used to associate arguments with parameters.</typeparam>
public sealed class CompletionResettingAssociatorDecorator<TData>
    : ICommandHandler<IAssociateArgumentsCommand<TData>>
    where TData : IAssociateArgumentsData
{
    private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Decoratee;

    private readonly ICommandHandler<IResetProcessCompletionCommand> CompletionResetter;

    /// <summary>Instantiates a decorator of an associator, which resets the completion status before invoking the decoratee.</summary>
    /// <param name="decoratee">The decorated associator.</param>
    /// <param name="completionResetter">Resets the completion status.</param>
    public CompletionResettingAssociatorDecorator(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<IResetProcessCompletionCommand> completionResetter)
    {
        Decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));

        CompletionResetter = completionResetter ?? throw new ArgumentNullException(nameof(completionResetter));
    }

    void ICommandHandler<IAssociateArgumentsCommand<TData>>.Handle(
        IAssociateArgumentsCommand<TData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        CompletionResetter.Handle(ResetProcessCompletionCommand.Instance);

        Decoratee.Handle(command);
    }
}

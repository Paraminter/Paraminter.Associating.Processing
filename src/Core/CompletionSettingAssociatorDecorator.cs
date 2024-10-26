namespace Paraminter.Associating.Processing;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Associating.Processing.Commands;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Decorates an associator by setting the completion status after invoking the decoratee.</summary>
/// <typeparam name="TData">The type representing the data used to associate arguments with parameters.</typeparam>
public sealed class CompletionSettingAssociatorDecorator<TData>
    : ICommandHandler<IAssociateArgumentsCommand<TData>>
    where TData : IAssociateArgumentsData
{
    private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Decoratee;

    private readonly ICommandHandler<ISetProcessCompletionCommand> CompletionSetter;

    /// <summary>Instantiates a decorator of an associator, which sets the completion status after invoking the decoratee.</summary>
    /// <param name="decoratee">The decorated associator.</param>
    /// <param name="completionSetter">Sets the completion status.</param>
    public CompletionSettingAssociatorDecorator(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<ISetProcessCompletionCommand> completionSetter)
    {
        Decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));

        CompletionSetter = completionSetter ?? throw new ArgumentNullException(nameof(completionSetter));
    }

    async Task ICommandHandler<IAssociateArgumentsCommand<TData>>.Handle(
        IAssociateArgumentsCommand<TData> command,
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        await Decoratee.Handle(command, cancellationToken).ConfigureAwait(false);

        await CompletionSetter.Handle(SetProcessCompletionCommand.Instance, cancellationToken).ConfigureAwait(false);
    }
}

namespace Paraminter.Associating.Processing;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Associating.Processing.Commands;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Decorates an associator by resetting the initiation status after invoking the decoratee.</summary>
/// <typeparam name="TData">The type representing the data used to associate arguments with parameters.</typeparam>
public sealed class InitiationResettingAssociatorDecorator<TData>
    : ICommandHandler<IAssociateArgumentsCommand<TData>>
    where TData : IAssociateArgumentsData
{
    private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Decoratee;

    private readonly ICommandHandler<IResetProcessInitiationCommand> InitiationResetter;

    /// <summary>Instantiates a decorator of an associator, which resets the initiation status after invoking the decoratee.</summary>
    /// <param name="decoratee">The decorated associator.</param>
    /// <param name="initiationResetter">Resets the initiation status.</param>
    public InitiationResettingAssociatorDecorator(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<IResetProcessInitiationCommand> initiationResetter)
    {
        Decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));

        InitiationResetter = initiationResetter ?? throw new ArgumentNullException(nameof(initiationResetter));
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

        await InitiationResetter.Handle(ResetProcessInitiationCommand.Instance, cancellationToken).ConfigureAwait(false);
    }
}

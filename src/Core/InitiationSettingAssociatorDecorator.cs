﻿namespace Paraminter.Associating.Processing;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Associating.Processing.Commands;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Decorates an associator by setting the initiation status before invoking the decoratee.</summary>
/// <typeparam name="TData">The type representing the data used to associate arguments with parameters.</typeparam>
public sealed class InitiationSettingAssociatorDecorator<TData>
    : ICommandHandler<IAssociateArgumentsCommand<TData>>
    where TData : IAssociateArgumentsData
{
    private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Decoratee;

    private readonly ICommandHandler<ISetProcessInitiationCommand> InitiationSetter;

    /// <summary>Instantiates a decorator of an associator, which sets the initiation status before invoking the decoratee.</summary>
    /// <param name="decoratee">The decorated associator.</param>
    /// <param name="initiationSetter">Sets the initiation status.</param>
    public InitiationSettingAssociatorDecorator(
        ICommandHandler<IAssociateArgumentsCommand<TData>> decoratee,
        ICommandHandler<ISetProcessInitiationCommand> initiationSetter)
    {
        Decoratee = decoratee ?? throw new ArgumentNullException(nameof(decoratee));

        InitiationSetter = initiationSetter ?? throw new ArgumentNullException(nameof(initiationSetter));
    }

    async Task ICommandHandler<IAssociateArgumentsCommand<TData>>.Handle(
        IAssociateArgumentsCommand<TData> command,
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        await InitiationSetter.Handle(SetProcessInitiationCommand.Instance, cancellationToken).ConfigureAwait(false);

        await Decoratee.Handle(command, cancellationToken).ConfigureAwait(false);
    }
}

namespace Paraminter.Associating.Processing;

using Moq;

using Paraminter.Associating.Commands;
using Paraminter.Associating.Models;
using Paraminter.Cqs;
using Paraminter.Processing.Commands;

internal interface IFixture<TData>
    where TData : IAssociateArgumentsData
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<TData>> Sut { get; }

    public abstract Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> DecorateeMock { get; }

    public abstract Mock<ICommandHandler<IResetProcessCompletionCommand>> CompletionResetterMock { get; }
}

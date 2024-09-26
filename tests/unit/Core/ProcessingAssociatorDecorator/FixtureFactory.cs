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

        Mock<ICommandHandler<ISetProcessInitiationCommand>> initiationSetterMock = new(MockBehavior.Strict);
        Mock<ICommandHandler<ISetProcessCompletionCommand>> completionSetterMock = new(MockBehavior.Strict);

        var sut = new ProcessingAssociatorDecorator<TData>(decorateeMock.Object, initiationSetterMock.Object, completionSetterMock.Object);

        return new Fixture<TData>(sut, decorateeMock, initiationSetterMock, completionSetterMock);
    }

    private sealed class Fixture<TData>
        : IFixture<TData>
        where TData : IAssociateArgumentsData
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<TData>> Sut;

        private readonly Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> DecorateeMock;

        private readonly Mock<ICommandHandler<ISetProcessInitiationCommand>> InitiationSetterMock;
        private readonly Mock<ICommandHandler<ISetProcessCompletionCommand>> CompletionSetterMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<TData>> sut,
            Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> decorateeMock,
            Mock<ICommandHandler<ISetProcessInitiationCommand>> initiationSetterMock,
            Mock<ICommandHandler<ISetProcessCompletionCommand>> completionSetterMock)
        {
            Sut = sut;

            DecorateeMock = decorateeMock;

            InitiationSetterMock = initiationSetterMock;
            CompletionSetterMock = completionSetterMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<TData>> IFixture<TData>.Sut => Sut;

        Mock<ICommandHandler<IAssociateArgumentsCommand<TData>>> IFixture<TData>.DecorateeMock => DecorateeMock;

        Mock<ICommandHandler<ISetProcessInitiationCommand>> IFixture<TData>.InitiationSetterMock => InitiationSetterMock;
        Mock<ICommandHandler<ISetProcessCompletionCommand>> IFixture<TData>.CompletionSetterMock => CompletionSetterMock;
    }
}

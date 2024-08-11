using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;

namespace AutoEntityGenerator.CodeOperations
{
    internal interface IUIResultProvider
    {
        IUserInteractionResult UserInteractionResult { get; }
    }

    internal class GetUserInputOperation : CodeActionOperation, IUIResultProvider
    {
        private readonly IEntityProvider _entityProvider;
        private readonly IUserInteraction _userInteraction;

        public GetUserInputOperation(IEntityProvider entityProvider, IUserInteraction userInteraction)
        {
            _entityProvider = entityProvider;
            _userInteraction = userInteraction;
        }

        public override void Apply(Workspace workspace, CancellationToken cancellationToken)
            => UserInteractionResult = _userInteraction.ShowUIForm(_entityProvider.Entity);

        public IUserInteractionResult UserInteractionResult { get; private set; }
    }
}

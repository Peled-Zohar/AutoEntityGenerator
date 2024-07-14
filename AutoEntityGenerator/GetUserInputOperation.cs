using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;

namespace AutoEntityGenerator
{
    internal interface IUIResultProvider
    {
        IUserInteractionResult UserInteractionResult { get; }
    }

    internal class GetUserInputOperation : CodeActionOperation, IUIResultProvider
    {
        private readonly Entity _entityInfo;
        private readonly IUserInteraction _userInteraction;

        public GetUserInputOperation(Entity entityInfo, IUserInteraction userInteraction)
        {
            _entityInfo = entityInfo;
            _userInteraction = userInteraction;
        }

        public override void Apply(Workspace workspace, CancellationToken cancellationToken) 
            => UserInteractionResult = _userInteraction.ShowUIForm(_entityInfo);

        public IUserInteractionResult UserInteractionResult { get; private set; }
    }
}

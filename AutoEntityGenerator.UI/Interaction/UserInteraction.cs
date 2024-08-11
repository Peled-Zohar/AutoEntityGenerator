using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.DependencyInjection;

namespace AutoEntityGenerator.UI.Interaction
{
    internal class UserInteraction : IUserInteraction
    {
        private readonly IEntityConfigurationWindowFactory _entityConfigurationWindowFactory;

        public UserInteraction(IEntityConfigurationWindowFactory entityConfigurationWindowFactory)
        {
            _entityConfigurationWindowFactory = entityConfigurationWindowFactory;
        }

        public IUserInteractionResult ShowUIForm(Entity entity)
        {
            var window = _entityConfigurationWindowFactory.Create(entity);
            return window.ShowDialog().GetValueOrDefault()
                ? window.Result
                : new UserInteractionResult();
        }
    }
}
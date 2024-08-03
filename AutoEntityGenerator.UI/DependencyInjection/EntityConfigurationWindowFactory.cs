using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Views;

namespace AutoEntityGenerator.UI.DependencyInjection
{

    public interface IEntityConfigurationWindowFactory
    {
        IEntityConfigurationWindow Create(Entity entity);
    }

    internal class EntityConfigurationWindowFactory : IEntityConfigurationWindowFactory
    {
        public IEntityConfigurationWindow Create(Entity entity)
        {
            var viewModel = new EntityConfigurationViewModel(entoty);
            return new EntityConfigurationWindow() { DataContext = viewModel };
        }
    }

}

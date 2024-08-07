using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.ViewModels;
using AutoEntityGenerator.UI.Views;
using System.IO;

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
            var validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(entity.Project.FilePath));
            var viewModel = new EntityConfigurationViewModel(validator, entity);
            return new EntityConfigurationWindow(viewModel);
        }
    }

}

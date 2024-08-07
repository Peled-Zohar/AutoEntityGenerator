using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.Services;
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
        private readonly IDialogService _dialogService;

        public EntityConfigurationWindowFactory(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public IEntityConfigurationWindow Create(Entity entity)
        {
            var validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(entity.Project.FilePath));
            var viewModel = new EntityConfigurationViewModel(validator, _dialogService, entity);
            return new EntityConfigurationWindow(viewModel);
        }
    }

}

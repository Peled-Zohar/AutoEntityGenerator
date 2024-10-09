using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.ViewModels;
using AutoEntityGenerator.UI.Views;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EntityConfigurationViewModel> _viewModelLogger;

        public EntityConfigurationWindowFactory(IDialogService dialogService, ILogger<EntityConfigurationViewModel> viewModelLogger)
        {
            _dialogService = dialogService;
            _viewModelLogger = viewModelLogger;
        }

        public IEntityConfigurationWindow Create(Entity entity)
        {
            var validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(entity.Project.FilePath));
            var viewModel = new EntityConfigurationViewModel(_viewModelLogger, validator, _dialogService, entity);
            return new EntityConfigurationWindow(viewModel);
        }
    }

}

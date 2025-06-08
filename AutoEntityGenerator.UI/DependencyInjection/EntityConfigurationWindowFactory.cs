using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.Validators;
using AutoEntityGenerator.UI.ViewModels;
using AutoEntityGenerator.UI.Views;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace AutoEntityGenerator.UI.DependencyInjection
{

    public interface IEntityConfigurationWindowFactory
    {
        IEntityConfigurationWindow Create(Entity entity);
    }

    [ExcludeFromCodeCoverage] // There's no logic to test here...
    internal class EntityConfigurationWindowFactory : IEntityConfigurationWindowFactory
    {
        private readonly IDialogService _dialogService;
        private readonly ILogger<EntityConfigurationViewModel> _entityConfigurationLogger;
        private readonly IAppSettings _appSettings;
        private readonly SettingsViewModel _settingsViewModel;

        public EntityConfigurationWindowFactory(IDialogService dialogService, ILogger<EntityConfigurationViewModel> entityConfigurationLogger, IAppSettings appSettings, SettingsViewModel settingsViewModel)
        {
            _dialogService = dialogService;
            _entityConfigurationLogger = entityConfigurationLogger;
            _appSettings = appSettings;
            _settingsViewModel = settingsViewModel;
        }

        public IEntityConfigurationWindow Create(Entity entity)
        {
            var entityConfigurationViewModelValidator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(entity.Project.FilePath));
            var entityConfigurationViewModel = new EntityConfigurationViewModel(_appSettings, _entityConfigurationLogger, entityConfigurationViewModelValidator, _dialogService, entity);
            return new EntityConfigurationWindow(entityConfigurationViewModel, _settingsViewModel);
        }
    }

}

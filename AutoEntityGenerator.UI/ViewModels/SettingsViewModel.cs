using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class SettingsViewModel : IAppSettings, INotifyPropertyChanged
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger<SettingsViewModel> _logger;
        private readonly IValidator<IAppSettings> _settingsViewModelValidator;
        private readonly IConfigurationSaver _configurationSaver;
        private readonly IDialogService _dialogService;

        private LogLevel _selectedLogLevel;
        private string _destinationFolder;
        private string _requestSuffix;
        private string requestSuffix;

        public SettingsViewModel(
            IAppSettings appSettings,
            ILogger<SettingsViewModel> logger,
            IValidator<IAppSettings> settingsViewModelValidator,
            IConfigurationSaver configurationSaver,
            IDialogService dialogService)
        {
            _appSettings = appSettings;
            _logger = logger;
            _settingsViewModelValidator = settingsViewModelValidator;
            _configurationSaver = configurationSaver;

            LogLevels = Enum
                .GetValues(typeof(LogLevel))
                .Cast<LogLevel>()
                .Where(l => l != LogLevel.Trace)
                .ToList();

            MinimumLogLevel = _appSettings.MinimumLogLevel;
            DestinationFolder = _appSettings.DestinationFolder;
            RequestSuffix = _appSettings.RequestSuffix;
            ResponseSuffix = _appSettings.ResponseSuffix;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            _dialogService = dialogService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action RequestClose;

        public List<LogLevel> LogLevels { get; }

        public LogLevel MinimumLogLevel
        {
            get => _selectedLogLevel;
            set
            {
                if (_selectedLogLevel != value)
                {
                    _selectedLogLevel = value;
                    OnPropertyChanged(nameof(MinimumLogLevel));
                }
            }
        }

        public string DestinationFolder
        {
            get => _destinationFolder;
            set
            {
                if (_destinationFolder != value)
                {
                    _destinationFolder = value;
                    OnPropertyChanged(nameof(DestinationFolder));
                }
            }
        }

        public string RequestSuffix
        {
            get => requestSuffix;
            set
            {
                if (requestSuffix != value)
                {
                    requestSuffix = value;
                    OnPropertyChanged(nameof(_requestSuffix));
                }
            }
        }

        public string ResponseSuffix
        {
            get => _requestSuffix;
            set
            {
                if (_requestSuffix != value)
                {
                    _requestSuffix = value;
                    OnPropertyChanged(nameof(_requestSuffix));
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        private void Save()
        {
            if (!Validate())
            {
                return;
            }
            _appSettings.MinimumLogLevel = MinimumLogLevel;
            _appSettings.DestinationFolder = DestinationFolder;
            _appSettings.RequestSuffix = RequestSuffix;
            _appSettings.ResponseSuffix = ResponseSuffix;

            try
            {
                _configurationSaver.Save(_appSettings);
                OnRequestClose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save configuration.");
                _dialogService.ShowDialog("An error occured while attempting to save configuration, check log for details.", "Failed to save configuration");
            }
        }
        private void Cancel()
        {
            MinimumLogLevel = _appSettings.MinimumLogLevel;
            DestinationFolder = _appSettings.DestinationFolder;
            RequestSuffix = _appSettings.RequestSuffix;
            ResponseSuffix = _appSettings.ResponseSuffix;

            OnRequestClose();
        }

        private bool Validate()
        {
            var validationResult = _settingsViewModelValidator.Validate(this);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                _dialogService.ShowDialog(validationErrors, "Invalid input");
                _logger.LogTrace("Failed to validate user input. Validation error: {ValidationErrors}", validationErrors);
                return false;
            }
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void OnRequestClose() => RequestClose?.Invoke();
    }
}

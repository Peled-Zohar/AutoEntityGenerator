using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class EntityConfigurationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly IAppSettings _appSettings;
        private readonly ILogger<EntityConfigurationViewModel> _logger;
        private readonly IValidator<EntityConfigurationViewModel> _validator;
        private readonly IDialogService _dialogService;
        private readonly Entity _entity;
        private string _destinationFolder;
        private string _dtoName;
        private string _generatedFileName;
        private bool _openGeneratedFiles;
        private bool _allowFileNameMismatch;
        private bool _dtoNameManuallyChanged;
        private bool _destinationFolderManuallyChanged;
        private bool _openGeneratedFilesManuallyChanged;

        private MappingDirectionViewModel selectedMappingDirection;

        #endregion Fields

        public const string Extension = ".cs";

        public EntityConfigurationViewModel(
            IAppSettings appSettings,
            ILogger<EntityConfigurationViewModel> logger,
            IValidator<EntityConfigurationViewModel> validator,
            IDialogService dialogService,
            Entity entity)
        {
            _appSettings = appSettings;
            _validator = validator;
            _dialogService = dialogService;
            _entity = entity;
            _logger = logger;

            _logger.LogDebug("Initializing UI for entity {entityName}", _entity.Name);

            _allowFileNameMismatch = false;
            _dtoNameManuallyChanged = false;
            _destinationFolderManuallyChanged = false;
            _openGeneratedFilesManuallyChanged = false;

            ProjectFolder = Path.GetDirectoryName(_entity.Project.FilePath);

            SetDestinationFolder();
            SetOpenGeneratedFiles();

            MappingDirections = new[]
            {
                new MappingDirectionViewModel("From Dto To Model", MappingDirection.FromDtoToModel),
                new MappingDirectionViewModel("From Model To Dto", MappingDirection.FromModelToDto),
            };

            SelectedMappingDirection = MappingDirections[0];

            Properties = new ObservableCollection<PropertyViewModel>();
            foreach (var property in _entity.Properties)
            {
                Properties.Add(new PropertyViewModel()
                {
                    IsSelected = true,
                    Name = property.Name,
                    Type = property.Type,
                    IsReadOnly = property.IsReadonly
                });
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            BrowseCommand = new RelayCommand(Browse);
            SelectAllCommand = new RelayCommand(SelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll);
            _logger.LogDebug("UI for entity {entityName} is ready.", _entity.Name);
        }

        #region Events

        public event Action<bool?> RequestClose;
        public event Action RequestFocus;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public ObservableCollection<PropertyViewModel> Properties { get; }
        public MappingDirectionViewModel[] MappingDirections { get; }
        public MappingDirectionViewModel SelectedMappingDirection
        {
            get => selectedMappingDirection;
            set
            {
                if (selectedMappingDirection != value)
                {
                    selectedMappingDirection = value;
                    SetDtoName();
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
                    _destinationFolderManuallyChanged = true;
                    OnPropertyChanged(nameof(DestinationFolder));
                }
            }
        }

        public string DtoName
        {
            get => _dtoName;
            set
            {
                if (_dtoName != value)
                {
                    _dtoName = value;
                    _dtoNameManuallyChanged = true;
                    OnPropertyChanged(nameof(DtoName));

                    if (!_allowFileNameMismatch)
                    {
                        _generatedFileName = GenerateFileName();
                        OnPropertyChanged(nameof(GeneratedFileName));
                    }
                }
            }
        }
        public string GeneratedFileName
        {
            get => _generatedFileName;
            set
            {
                if (_generatedFileName != value)
                {
                    if (CheckFileNameMismatch(value))
                    {
                        _generatedFileName = value;
                        _allowFileNameMismatch = true;
                        OnPropertyChanged(nameof(GeneratedFileName));
                    }
                }
            }
        }
        public string ProjectFolder { get; }
        public string DestinationPath => Path.Combine(ProjectFolder, DestinationFolder.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        public IUserInteractionResult Result { get; private set; }
        public bool OpenGeneratedFiles
        {
            get => _openGeneratedFiles;
            set
            {
                if (_openGeneratedFiles != value)
                {
                    _openGeneratedFiles = value;
                    _openGeneratedFilesManuallyChanged = true;
                    OnPropertyChanged(nameof(OpenGeneratedFiles));
                }
            }
        }

        #endregion Properties

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand UnselectAllCommand { get; }
        private void Save()
        {
            if (!Validate())
            {
                return;
            }

            Result = new UserInteractionResult(
                SelectedMappingDirection.Value,
                DestinationPath,
                DtoName,
                Properties
                    .Where(p => p.IsSelected)
                    .Select(p => new Property()
                    {
                        IsReadonly = p.IsReadOnly,
                        Name = p.Name,
                        Type = p.Type
                    })
                    .ToList(),
                GeneratedFileName,
                OpenGeneratedFiles);
            OnRequestClose(true);
        }

        [ExcludeFromCodeCoverage] // There's no logic to test here...
        private void Cancel()
        {
            OnRequestClose(false);
        }
        private void SelectAll() => ToggleSelectedForAllProperties(true);
        private void UnselectAll() => ToggleSelectedForAllProperties(false);
        private void Browse()
        {
            var targetDirectory = Path.Combine(ProjectFolder, DestinationFolder);

            var initialDirectory = Directory.Exists(targetDirectory)
                    ? targetDirectory
                    : ProjectFolder;

            var (openFileResult, destinationFolder) = _dialogService.ShowFolderPickerDialog(initialDirectory);

            if (openFileResult)
            {
                if (destinationFolder != ProjectFolder && !destinationFolder.StartsWith(ProjectFolder))
                {
                    _dialogService.ShowDialog("Target folder must be a sub folder of the project folder.", "Invalid folder");
                }
                else
                {
                    DestinationFolder = destinationFolder
                        .Replace(ProjectFolder, "")
                        .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }
            }

            OnRequestFocus();
        }

        #endregion Commands

        #region Methods

        internal void SettingsChanged()
        {
            SetDtoName();
            SetDestinationFolder();
            SetOpenGeneratedFiles();
        }

        private void ToggleSelectedForAllProperties(bool selected)
        {
            foreach (var property in Properties)
            {
                property.IsSelected = selected;
            }
        }
        private bool Validate()
        {
            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                _dialogService.ShowDialog(validationErrors, "Invalid input");
                _logger.LogTrace("Failed to validate user input. Validation error: {ValidationErrors}", validationErrors);
                return false;
            }

            var fileNameMismatchResult = CheckFileNameMismatch();
            if (!fileNameMismatchResult)
            {
                _logger.LogTrace("Validation failed because file name {fileName} doesn't match entity name {entityName}.", _generatedFileName, GenerateFileName());
            }
            return fileNameMismatchResult;
        }
        private string GenerateFileName() => _dtoName + Extension;
        private bool CheckFileNameMismatch(string value = null)
        {
            value = value ?? _generatedFileName;

            if (!_allowFileNameMismatch && value != GenerateFileName())
            {
                _allowFileNameMismatch = _dialogService.ShowYesNoDialog(
                $"Generated file name doesn't match entity name.{Environment.NewLine}Is that Intended?",
                "File name and entity name mismatch");
            }
            return _allowFileNameMismatch || value == GenerateFileName();
        }
        
        private void SetDtoName()
        {
            if (!_dtoNameManuallyChanged)
            {
                var suffix = SelectedMappingDirection.Value == MappingDirection.FromDtoToModel
                    ? _appSettings.RequestSuffix
                    : _appSettings.ResponseSuffix;
                DtoName = _entity.Name + suffix;
                _dtoNameManuallyChanged = false;
            }
        }
        private void SetDestinationFolder()
        {
            if (_destinationFolderManuallyChanged)
            {
                return;
            }

            var entityDirectoryRelativeToProject = Path.GetDirectoryName(_entity.SourceFilePath).Replace(ProjectFolder, "");
            DestinationFolder = Path.Combine(entityDirectoryRelativeToProject, _appSettings.DestinationFolder);
            _destinationFolderManuallyChanged = false;
        }
        private void SetOpenGeneratedFiles()
        {
            if (!_openGeneratedFilesManuallyChanged)
            {
                OpenGeneratedFiles = _appSettings.OpenGeneratedFiles;
                _openGeneratedFilesManuallyChanged = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void OnRequestClose(bool? dialogResult) => RequestClose?.Invoke(dialogResult);
        protected virtual void OnRequestFocus() => RequestFocus?.Invoke();

        #endregion Methods
    }

}

using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Services;
using FluentValidation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class EntityConfigurationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly IValidator<EntityConfigurationViewModel> _validator;
        private readonly IDialogService _dialogService;
        private readonly Entity _entity;
        private string _destinationFolder;
        private string _dtoName;
        private string _generatedFileName;
        private bool _allowFileNameMismatch;

        #endregion Fields

        public const string Extension = ".cs";

        public EntityConfigurationViewModel(IValidator<EntityConfigurationViewModel> validator, IDialogService dialogService, Entity entity)
        {
            _validator = validator;
            _dialogService = dialogService;
            _entity = entity;

            _allowFileNameMismatch = false;
            DtoName = _entity.Name + "Request";
            ProjectFolder = Path.GetDirectoryName(_entity.Project.FilePath);
            var entityDirectory = Path.GetDirectoryName(entity.SourceFilePath).Replace(ProjectFolder, "");
            DestinationFolder = Path.Combine(entityDirectory, "Generated");

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
            BrowesCommand = new RelayCommand(Browes);
            SelectAllCommand = new RelayCommand(SelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll);
        }

        #region Events

        public event Action<bool?> RequestClose;
        public event Action RequestFocus;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public ObservableCollection<PropertyViewModel> Properties { get; }
        public MappingDirectionViewModel[] MappingDirections { get; }
        public MappingDirectionViewModel SelectedMappingDirection { get; set; }
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
        public string DtoName
        {
            get => _dtoName;
            set
            {
                if (_dtoName != value)
                {
                    _dtoName = value;
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

        public string ProjectFolder { get; private set; }
        public string DestinationPath => Path.Combine(ProjectFolder, DestinationFolder.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        public IUserInteractionResult Result { get; private set; }

        #endregion Properties

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowesCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand UnselectAllCommand { get; }

        private void Save()
        {
            if (!Validate()) { return; }

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
                GeneratedFileName);
            OnRequestClose(true);
        }
        private void Cancel()
        {
            OnRequestClose(false);
        }
        private void SelectAll() => ToggleSelectedForAllProperties(true);
        private void UnselectAll() => ToggleSelectedForAllProperties(false);
        private void Browes()
        {
            var targetDirectory = Path.Combine(ProjectFolder, DestinationFolder);

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = Directory.Exists(targetDirectory)
                    ? targetDirectory
                    : ProjectFolder;
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == CommonFileDialogResult.Ok)
                {
                    var destinationFolder = dialog.FileName;
                    if (destinationFolder != ProjectFolder && !destinationFolder.StartsWith(ProjectFolder))
                    {
                        _dialogService.ShowDialog("Target folder must be a sub folder of the project folder.", "Invalid folder");
                    }
                    else
                    {
                        DestinationFolder = destinationFolder.Replace(ProjectFolder, "");
                    }
                }
            }
            OnRequestFocus();
        }

        #endregion Commands

        #region Methods

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
                _dialogService.ShowDialog(string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage)), "Invalid input");
                return false;
            }

            return CheckFileNameMismatch();
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

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void OnRequestClose(bool? dialogResult) => RequestClose?.Invoke(dialogResult);
        protected virtual void OnRequestFocus() => RequestFocus?.Invoke();

        #endregion Methods
    }

}

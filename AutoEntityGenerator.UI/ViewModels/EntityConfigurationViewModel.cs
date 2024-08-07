using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using FluentValidation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class EntityConfigurationViewModel : INotifyPropertyChanged
    {
        private readonly IValidator<EntityConfigurationViewModel> _validator;
        private readonly Entity _entity;
        private bool _generatedFileNameWasManuallySet;
        private string _destinationFolder;
        private string _dtoName;
        private string _generatedFileName;

        public EntityConfigurationViewModel(IValidator<EntityConfigurationViewModel> validator, Entity entity)
        {
            _validator = validator;
            _entity = entity;

            _generatedFileNameWasManuallySet = false;
            DtoName = _entity.Name + "Request";
            DestinationFolder = Path.Combine(Path.GetDirectoryName(_entity.SourceFilePath), "Generated");

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

        public event Action<bool?> RequestClose;
        public event Action RequestFocus;
        public event Action<string> ValidationFailed;
        public event PropertyChangedEventHandler PropertyChanged;

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

                    if (!_generatedFileNameWasManuallySet)
                    {
                        _generatedFileName = _dtoName + ".cs";
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
                    _generatedFileName = value;
                    _generatedFileNameWasManuallySet = true;
                    OnPropertyChanged(nameof(GeneratedFileName));
                }
            }
        }
        public IUserInteractionResult Result { get; private set; }

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
                DestinationFolder,
                DtoName,
                Properties.Select(p => new Property() { IsReadonly = p.IsReadOnly, Name = p.Name, Type = p.Type }).ToList(),
                GeneratedFileName);
            OnRequestClose(true);
        }

        private bool Validate()
        {
            var validationResult = _validator.Validate(this);
            if (!validationResult.IsValid)
            {
                ValidationFailed?.Invoke(string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage)));
                return false;
            }
            return true;
        }

        private void Cancel()
        {
            Result = new UserInteractionResult();
            OnRequestClose(false);
        }
        private void SelectAll() => ToggleSelectedForAllProperties(true);
        private void UnselectAll() => ToggleSelectedForAllProperties(false);
        private void Browes()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = Path.GetDirectoryName(DestinationFolder);
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    DestinationFolder = dialog.FileName;
                }
            }
            OnRequestFocus();
        }
        private void ToggleSelectedForAllProperties(bool selected)
        {
            foreach (var property in Properties)
            {
                property.IsSelected = selected;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected virtual void OnRequestClose(bool? dialogResult) => RequestClose?.Invoke(dialogResult);
        protected virtual void OnRequestFocus() => RequestFocus?.Invoke();
    }

}

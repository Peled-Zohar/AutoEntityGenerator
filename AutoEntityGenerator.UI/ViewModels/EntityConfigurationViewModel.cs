using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class EntityConfigurationViewModel : INotifyPropertyChanged
    {
        private readonly Entity _entity;
        private string _destinationFolder;

        public event Action<bool?> RequestClose;

        public event Action RequestFocus;

        public event PropertyChangedEventHandler PropertyChanged;

        public EntityConfigurationViewModel(Entity entity)
        {
            _entity = entity;
            Properties = new ObservableCollection<PropertyViewModel>();
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand(OnCancel);
            BrowesCommand = new RelayCommand(OnBrowes);
            SelectAllCommand = new RelayCommand(OnSelectAll);
            UnselectAllCommand = new RelayCommand(OnUnselectAll);

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
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowesCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand UnselectAllCommand { get; }

        public ObservableCollection<PropertyViewModel> Properties { get; }

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

        public IUserInteractionResult Result { get; private set; }

        private void OnSave()
        {
            // TODO: Set Result property here

            OnRequestClose(true);
        }

        private bool CanSave()
        {
            // TODO: Input validation logic goes here
            return true;
        }

        private void OnCancel()
        {
            Result = new UserInteractionResult();
            OnRequestClose(false);
        }

        private void OnUnselectAll()
        {
            ToggleSelectedForAllProperties(false);
        }

        private void OnSelectAll()
        {
            ToggleSelectedForAllProperties(true);
        }

        private void OnBrowes()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRequestClose(bool? dialogResult) => RequestClose?.Invoke(dialogResult);

        protected virtual void OnRequestFocus() => RequestFocus?.Invoke();
    }
    
}

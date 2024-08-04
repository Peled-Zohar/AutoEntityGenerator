using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
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
        public event Action<bool?> RequestClose;

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

        public IUserInteractionResult Result { get; private set; }

        private void OnSave()
        {
            // TODO: Set Result property here

            RequestClose(true);
        }

        private bool CanSave()
        {
            // TODO: Input validation logic goes here
            return true;
        }

        private void OnCancel()
        {
            Result = new UserInteractionResult();
            RequestClose(false);
        }

        private void OnUnselectAll()
        {
            // TODO: Implement UnSelect all
            throw new NotImplementedException();
        }

        private void OnSelectAll()
        {
            // TODO: Implement Select all
            throw new NotImplementedException();
        }

        private void OnBrowes()
        {
            // TODO: Implement Browes
            throw new NotImplementedException();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}

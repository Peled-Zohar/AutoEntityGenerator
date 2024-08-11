using System.ComponentModel;

namespace AutoEntityGenerator.UI.ViewModels
{
    public class PropertyViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                // Normally this will be in a condition,
                // but I've noticed sometime the UI doesn't get refreshed
                // so I've removed the condition in the hope to solve this problem.
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

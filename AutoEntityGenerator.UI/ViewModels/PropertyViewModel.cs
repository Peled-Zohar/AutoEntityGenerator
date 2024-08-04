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
            get { return _isSelected; }
            set
            {
                if (IsSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using AutoEntityGenerator.Common.CodeInfo;
using System.ComponentModel;

namespace AutoEntityGenerator.UI.Winforms
{
    internal class SelectablePropertyInfo : Property, INotifyPropertyChanged
    {
        private bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value)
                {
                    return;
                }
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }
}

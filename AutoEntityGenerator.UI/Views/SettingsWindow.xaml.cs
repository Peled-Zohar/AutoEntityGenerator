using AutoEntityGenerator.UI.ViewModels;
using System.Windows;

namespace AutoEntityGenerator.UI.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _viewModel;
        public SettingsWindow(SettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.RequestClose += ViewModel_RequestClose;
        }

        private void ViewModel_RequestClose()
        {
            _viewModel.RequestClose -= ViewModel_RequestClose;
            Close();
        }
    }
}

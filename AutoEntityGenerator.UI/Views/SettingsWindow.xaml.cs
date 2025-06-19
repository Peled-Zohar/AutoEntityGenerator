using AutoEntityGenerator.UI.ViewModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AutoEntityGenerator.UI.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _viewModel;
        private readonly Action _settingsSaved;
        public SettingsWindow(SettingsViewModel viewModel, Action settingsSaved)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _settingsSaved = settingsSaved;
            DataContext = _viewModel;
            _viewModel.RequestClose += ViewModel_RequestClose;
            _viewModel.SettingsSaved += _settingsSaved;
        }

        private void ViewModel_RequestClose()
        {
            _viewModel.SettingsSaved -= _settingsSaved;
            _viewModel.RequestClose -= ViewModel_RequestClose;
            Close();
        }
    }
}

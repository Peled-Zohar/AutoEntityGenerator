using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AutoEntityGenerator.UI.Views
{
    public interface IEntityConfigurationWindow
    {
        IUserInteractionResult Result { get; }

        bool? ShowDialog();
    }

    /// <summary>
    /// Interaction logic for EntityConfigurationWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    public partial class EntityConfigurationWindow : Window, IEntityConfigurationWindow
    {
        private readonly EntityConfigurationViewModel _viewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private SettingsWindow _settingsView;

        public EntityConfigurationWindow(EntityConfigurationViewModel viewModel, SettingsViewModel settingsViewModel)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.RequestClose += ViewModel_RequestClose;
            _viewModel.RequestFocus += ViewModel_RequestFocus;
            _settingsViewModel = settingsViewModel;
        }

        private void ViewModel_RequestFocus()
        {
            SetFocus(_settingsView as Window ?? this);
        }

        private void ViewModel_RequestClose(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public IUserInteractionResult Result => _viewModel.Result;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settingsView = new SettingsWindow(_settingsViewModel, _viewModel.SettingsChanged)
            {
                Owner = this
            };
            _settingsView.ShowDialog();
            _settingsView = null;
        }

        private void SetFocus(Window window)
        {
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();
        }
    }
}

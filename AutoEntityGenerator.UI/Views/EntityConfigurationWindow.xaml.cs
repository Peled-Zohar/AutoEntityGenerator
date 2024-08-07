using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.ViewModels;
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
    public partial class EntityConfigurationWindow : Window, IEntityConfigurationWindow
    {
        private readonly EntityConfigurationViewModel _viewModel;
        public EntityConfigurationWindow(EntityConfigurationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.RequestClose += ViewModel_RequestClose;
            _viewModel.RequestFocus += ViewModel_RequestFocus;
        }

        private void ViewModel_RequestFocus()
        {
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void ViewModel_RequestClose(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public IUserInteractionResult Result => _viewModel.Result;

        private void GeneratedFileName_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.CheckFileNameMismatch();
        }
    }
}

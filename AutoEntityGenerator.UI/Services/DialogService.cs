using System.Windows;

namespace AutoEntityGenerator.UI.Services
{
    public interface IDialogService
    {
        bool ShowYesNoDialog(string message, string caption);
        void ShowDialog(string message, string caption);
    }

    internal class DialogService : IDialogService
    {
        public bool ShowYesNoDialog(string message, string caption)
            => MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        public void ShowDialog(string message, string caption)
            => MessageBox.Show(message, caption);
    }
}

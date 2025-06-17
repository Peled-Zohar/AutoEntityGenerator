using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AutoEntityGenerator.UI.Services
{
    public interface IDialogService
    {
        bool ShowYesNoDialog(string message, string caption);
        void ShowDialog(string message, string caption);

        (bool result, string folderName) ShowFolderPickerDialog(string initialDirectory);
    }

    [ExcludeFromCodeCoverage] // There's no logic to test here...
    internal class DialogService : IDialogService
    {
        public bool ShowYesNoDialog(string message, string caption)
            => MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;

        public void ShowDialog(string message, string caption)
            => MessageBox.Show(message, caption);

        public (bool result, string folderName) ShowFolderPickerDialog(string initialDirectory)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = initialDirectory;
                var dialogResult = dialog.ShowDialog();
                return (dialogResult == CommonFileDialogResult.Ok, dialog.FileName);
            }
        }
    }
}

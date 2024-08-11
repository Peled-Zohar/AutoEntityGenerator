using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            => MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes;

        public void ShowDialog(string message, string caption)
            => MessageBox.Show(message, caption);
    }
}

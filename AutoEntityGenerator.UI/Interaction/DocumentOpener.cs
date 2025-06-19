using AutoEntityGenerator.Common.Interfaces;
using Microsoft.VisualStudio.Shell;

namespace AutoEntityGenerator.UI.Interaction
{
    internal class DocumentOpener : IDocumentOpener
    {
        public void OpenDocument(string filePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            VsShellUtilities.OpenDocument(ServiceProvider.GlobalProvider, filePath);
        }
    }
}

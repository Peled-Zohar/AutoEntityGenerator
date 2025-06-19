using AutoEntityGenerator.Common.Interfaces;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;

namespace AutoEntityGenerator.UI.Interaction
{
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    internal class DocumentOpener : IDocumentOpener
    {
        public void OpenDocument(string filePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            VsShellUtilities.OpenDocument(ServiceProvider.GlobalProvider, filePath);
        }
    }
}

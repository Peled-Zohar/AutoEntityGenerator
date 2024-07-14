using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Composition;
using System.Threading.Tasks;

namespace AutoEntityGenerator
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(AutoEntityGeneratorCodeRefactoringProvider)), Shared]
    internal class AutoEntityGeneratorCodeRefactoringProvider : CodeRefactoringProvider
    {
        private readonly ILogger _logger;
        private readonly ICodeActionFactory _codeActionFactory;
        private readonly Services _services;

        public AutoEntityGeneratorCodeRefactoringProvider()
        {
            _services = new Services();

            _logger = _services.GetService<ILogger>();
            _codeActionFactory = _services.GetService<ICodeActionFactory>();
        }

        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            _logger.Debug("ComputeRefactoringsAsync called.");
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var node = root.FindNode(context.Span);

            if (!(node is ClassDeclarationSyntax || node is RecordDeclarationSyntax))
            {
                return;
            }

            _logger.Debug("About to register refactoring.");
            var action = _codeActionFactory.CreateEntityGeneratorCodeAction(context.Document, node as TypeDeclarationSyntax);
            context.RegisterRefactoring(action);
        }
    }
}

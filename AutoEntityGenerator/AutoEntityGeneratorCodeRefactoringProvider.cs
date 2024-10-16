using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;


namespace AutoEntityGenerator
{
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(AutoEntityGeneratorCodeRefactoringProvider)), Shared]
    internal class AutoEntityGeneratorCodeRefactoringProvider : CodeRefactoringProvider
    {
        private readonly ILogger<AutoEntityGeneratorCodeRefactoringProvider> _logger;
        private readonly ICodeActionFactory _codeActionFactory;
        private readonly IServices _services;

        public AutoEntityGeneratorCodeRefactoringProvider()
        {
            _services = Services.Instance;
            _logger = _services.GetService<ILogger<AutoEntityGeneratorCodeRefactoringProvider>>();
            _codeActionFactory = _services.GetService<ICodeActionFactory>();
            _logger.LogInformation("AutoEntityGenerator started.");
        }

        public sealed override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            _logger.LogDebug("ComputeRefactoringsAsync called.");
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var node = root.FindNode(context.Span);

            if (!(node is ClassDeclarationSyntax || node is RecordDeclarationSyntax))
            {
                return;
            }
            var typeDecleration = node as TypeDeclarationSyntax;
            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
            if (!(semanticModel.GetDeclaredSymbol(typeDecleration) is INamedTypeSymbol typeSymbol))
            {
                return;
            }

            var hasPublicProperties = typeSymbol.GetMembers().OfType<IPropertySymbol>().Any(p => p.DeclaredAccessibility == Accessibility.Public);

            if (!hasPublicProperties)
            {
                return;
            }

            _logger.LogDebug("attempting to register refactoring.");
            var action = _codeActionFactory.CreateEntityGeneratorCodeAction(context.Document, typeDecleration, typeSymbol);
            context.RegisterRefactoring(action);
            _logger.LogDebug("refactoring registered.");
        }
    }
}

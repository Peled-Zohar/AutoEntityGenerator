using AutoEntityGenerator.CodeGenerator;
using AutoEntityGenerator.Common;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI;
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
        private readonly IUserInteraction _userInteraction;
        private readonly ICodeFileGenerator _codeGenerator;
        private readonly IEntityGenerator _entityGenerator;
        private readonly ILogger _logger;

        public AutoEntityGeneratorCodeRefactoringProvider()
        {
            // TODO: Configure DI 

            _logger = new LoggerFactory().CreateLogger();
            _userInteraction = new UserInteraction();
            _codeGenerator = new CodeFileGenerator();
            _entityGenerator = new EntityGenerator(_logger);
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
            var action = new EntityGeneratorCodeAction(_logger, context.Document, node as TypeDeclarationSyntax, _entityGenerator, _userInteraction, _codeGenerator);
            context.RegisterRefactoring(action);
        }

    }
}

using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoEntityGenerator
{
    internal class EntityGeneratorCodeAction : CodeAction
    {
        private readonly ILogger _logger;
        private readonly Document _document;
        private readonly TypeDeclarationSyntax _typeDeclaration;
        private readonly IUserInteraction _userInteraction;
        private readonly ICodeFileGenerator _codeGenerator;
        private readonly IEntityGenerator _entityGenerator;

        public EntityGeneratorCodeAction(ILogger logger, Document document, TypeDeclarationSyntax typeDeclaration, IEntityGenerator entityGenerator, IUserInteraction userInteraction, ICodeFileGenerator codeGenerator)
        {
            _logger = logger;
            _document = document;
            _typeDeclaration = typeDeclaration;
            _entityGenerator = entityGenerator;
            _userInteraction = userInteraction;
            _codeGenerator = codeGenerator;
        }

        // TODO: consider converting the title to a LocalizedString or to a confiruated-value rather than a hardcoded one..
        public override string Title => "🔧 Generate DTO and mapping 🛠️";

        protected override async Task<IEnumerable<CodeActionOperation>> ComputeOperationsAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Attempting to gather type information.");
            // This call is here and not in a dedicated CodeActionOperation because it's an async call,
            // and CodeActionOperation's Apply method is not an async one.
            var entityInfo = await _entityGenerator.GenerateFromDocumentAsync(_document, _typeDeclaration, cancellationToken);
            var getUserInputOperation = new GetUserInputOperation(entityInfo, _userInteraction);
            var generateCodeOperation = new GenerateCodeOperation(getUserInputOperation, _entityGenerator, _codeGenerator, entityInfo, _document, _logger);

            return new CodeActionOperation[]
            {
                getUserInputOperation,
                generateCodeOperation
            };
        }

        protected override Task<IEnumerable<CodeActionOperation>> ComputePreviewOperationsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<CodeActionOperation>());
        }
    }
}

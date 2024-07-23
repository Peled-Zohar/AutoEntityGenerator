using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoEntityGenerator
{
    internal class EntityGeneratorCodeAction : CodeAction
    {
        private readonly ILogger<EntityGeneratorCodeAction> _logger;
        private readonly Document _document;
        private readonly TypeDeclarationSyntax _typeDeclaration;
        private readonly IUserInteraction _userInteraction;
        private readonly ICodeFileGenerator _codeGenerator;
        private readonly IEntityGenerator _entityGenerator;
        private readonly ICodeActionFactory _codeActionFactory;
        private readonly INamedTypeSymbol _typeSymbol;

        public EntityGeneratorCodeAction(ILogger<EntityGeneratorCodeAction> logger, Document document, TypeDeclarationSyntax typeDeclaration, IEntityGenerator entityGenerator, IUserInteraction userInteraction, ICodeFileGenerator codeGenerator, ICodeActionFactory codeActionFactory, INamedTypeSymbol typeSymbol)
        {
            _logger = logger;
            _document = document;
            _typeDeclaration = typeDeclaration;
            _entityGenerator = entityGenerator;
            _userInteraction = userInteraction;
            _codeGenerator = codeGenerator;
            _codeActionFactory = codeActionFactory;
            _typeSymbol = typeSymbol;
        }

        // TODO: consider converting the title to a LocalizedString or to a confiruated-value rather than a hardcoded one..
        public override string Title => "🔧 Generate DTO and mapping 🛠️";

        protected override async Task<IEnumerable<CodeActionOperation>> ComputeOperationsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to gather type information.");
            var getEntityInfoOperation = _codeActionFactory.CreateGetEntityInfoOperation(_document, _typeDeclaration, _typeSymbol);
            var getUserInputOperation = _codeActionFactory.CreateGetUserInputOperation(getEntityInfoOperation);
            var generateCodeOperation = _codeActionFactory.CreateGenerateCodeOperation(getUserInputOperation, getEntityInfoOperation, _document);

            return new CodeActionOperation[]
            {
                getEntityInfoOperation,
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

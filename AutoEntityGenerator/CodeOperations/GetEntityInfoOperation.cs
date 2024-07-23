using AutoEntityGenerator.Common.CodeInfo;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AutoEntityGenerator.CodeOperations
{

    public interface IEntityProvider
    {
        Entity Entity { get; }
    }

    internal class GetEntityInfoOperation : CodeActionOperation, IEntityProvider
    {
        ILogger<GetEntityInfoOperation> _logger;
        private readonly Document _document;
        private readonly TypeDeclarationSyntax _typeDeclaration;
        private readonly INamedTypeSymbol _typeSymbol;
        private readonly IEntityGenerator _entityGenerator;

        public GetEntityInfoOperation(ILogger<GetEntityInfoOperation> logger, Document document, TypeDeclarationSyntax typeDeclaration, INamedTypeSymbol typeSymbol, IEntityGenerator entityGenerator)
        {
            _logger = logger;
            _document = document;
            _typeDeclaration = typeDeclaration;
            _typeSymbol = typeSymbol;
            _entityGenerator = entityGenerator;
        }

        public override void Apply(Workspace workspace, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Attempting to generate entity from document");
            Entity = _entityGenerator.GenerateFromDocument(_document, _typeDeclaration, _typeSymbol, cancellationToken);
        }

        public Entity Entity { get; private set; }
    }
}

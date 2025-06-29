using AutoEntityGenerator.CodeOperations;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator;

internal interface ICodeActionFactory
{
    EntityGeneratorCodeAction CreateEntityGeneratorCodeAction(Document document, TypeDeclarationSyntax typeDeclaration, INamedTypeSymbol typeSymbol);
    GenerateCodeOperation CreateGenerateCodeOperation(IUIResultProvider uiResultProvider, IEntityProvider entityProvider, Document document);
    GetEntityInfoOperation CreateGetEntityInfoOperation(Document document, TypeDeclarationSyntax typeDeclaration, INamedTypeSymbol typeSymbol);
    GetUserInputOperation CreateGetUserInputOperation(IEntityProvider entityProvider);
}

internal class CodeActionFactory : ICodeActionFactory
{
    private readonly ILogger<EntityGeneratorCodeAction> _entityGeneratorCodeActionLogger;
    private readonly ILogger<GenerateCodeOperation> _generateCodeOperationLogger;
    private readonly ILogger<GetEntityInfoOperation> _getEntityInfoOperationLogger;
    private readonly IEntityGenerator _entityGenerator;
    private readonly IUserInteraction _userInteraction;
    private readonly ICodeFileGenerator _codeGenerator;
    private readonly IDocumentOpener _documentOpener;

    public CodeActionFactory(ILogger<EntityGeneratorCodeAction> entityGeneratorCodeActionLogger, ILogger<GenerateCodeOperation> generateCodeOperationLogger, ILogger<GetEntityInfoOperation> getEntityInfoOperationLogger, IEntityGenerator entityGenerator, IUserInteraction userInteraction, ICodeFileGenerator codeGenerator, IDocumentOpener documentOpener)
    {
        _entityGeneratorCodeActionLogger = entityGeneratorCodeActionLogger;
        _generateCodeOperationLogger = generateCodeOperationLogger;
        _getEntityInfoOperationLogger = getEntityInfoOperationLogger;
        _entityGenerator = entityGenerator;
        _userInteraction = userInteraction;
        _codeGenerator = codeGenerator;
        _documentOpener = documentOpener;
    }

    public EntityGeneratorCodeAction CreateEntityGeneratorCodeAction(Document document, TypeDeclarationSyntax typeDeclaration, INamedTypeSymbol typeSymbol) 
        => new(_entityGeneratorCodeActionLogger, document, typeDeclaration, this, typeSymbol);

    public GetUserInputOperation CreateGetUserInputOperation(IEntityProvider entityProvider) 
        => new(entityProvider, _userInteraction);

    public GenerateCodeOperation CreateGenerateCodeOperation(IUIResultProvider uiResultProvider, IEntityProvider entityProvider, Document document) 
        => new(
            uiResultProvider,
            _entityGenerator,
            _codeGenerator,
            entityProvider,
            document,
            _generateCodeOperationLogger,
            _documentOpener
        );

    public GetEntityInfoOperation CreateGetEntityInfoOperation(Document document, TypeDeclarationSyntax typeDeclaration, INamedTypeSymbol typeSymbol)
        => new(_getEntityInfoOperationLogger, document, typeDeclaration, typeSymbol, _entityGenerator);
}
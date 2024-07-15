using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator
{
    internal interface ICodeActionFactory
    {
        EntityGeneratorCodeAction CreateEntityGeneratorCodeAction(Document document, TypeDeclarationSyntax typeDeclaration);
        GenerateCodeOperation CreateGenerateCodeOperation(IUIResultProvider uiResultProvider, Entity entityInfo, Document document);
        GetUserInputOperation CreateGetUserInputOperation(Entity entityInfo);
    }

    internal class CodeActionFactory : ICodeActionFactory
    {
        private readonly ILogger<EntityGeneratorCodeAction> _entityGeneratorCodeActionLogger;
        private readonly ILogger<GenerateCodeOperation> _generateCodeOperationLogger;
        private readonly IEntityGenerator _entityGenerator;
        private readonly IUserInteraction _userInteraction;
        private readonly ICodeFileGenerator _codeGenerator;

        public CodeActionFactory(ILogger<EntityGeneratorCodeAction> entityGeneratorCodeActionLogger, ILogger<GenerateCodeOperation> generateCodeOperationLogger, IEntityGenerator entityGenerator, IUserInteraction userInteraction, ICodeFileGenerator codeGenerator)
        {
            _entityGeneratorCodeActionLogger = entityGeneratorCodeActionLogger;
            _generateCodeOperationLogger = generateCodeOperationLogger;
            _entityGenerator = entityGenerator;
            _userInteraction = userInteraction;
            _codeGenerator = codeGenerator;
        }

        public EntityGeneratorCodeAction CreateEntityGeneratorCodeAction(Document document, TypeDeclarationSyntax typeDeclaration) 
            => new EntityGeneratorCodeAction(_entityGeneratorCodeActionLogger, document, typeDeclaration, _entityGenerator, _userInteraction, _codeGenerator, this);

        public GetUserInputOperation CreateGetUserInputOperation(Entity entityInfo) 
            => new GetUserInputOperation(entityInfo, _userInteraction);

        public GenerateCodeOperation CreateGenerateCodeOperation(IUIResultProvider uiResultProvider, Entity entityInfo, Document document) 
            => new GenerateCodeOperation(
                uiResultProvider,
                _entityGenerator,
                _codeGenerator,
                entityInfo,
                document,
                _generateCodeOperationLogger
            );
    }
}
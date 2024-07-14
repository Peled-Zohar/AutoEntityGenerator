using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        private readonly ILogger _logger;
        private readonly IEntityGenerator _entityGenerator;
        private readonly IUserInteraction _userInteraction;
        private readonly ICodeFileGenerator _codeGenerator;

        public CodeActionFactory(ILogger logger, IEntityGenerator entityGenerator, IUserInteraction userInteraction, ICodeFileGenerator codeGenerator)
        {
            _logger = logger;
            _entityGenerator = entityGenerator;
            _userInteraction = userInteraction;
            _codeGenerator = codeGenerator;
        }

        public EntityGeneratorCodeAction CreateEntityGeneratorCodeAction(Document document, TypeDeclarationSyntax typeDeclaration) 
            => new EntityGeneratorCodeAction(_logger, document, typeDeclaration, _entityGenerator, _userInteraction, _codeGenerator, this);

        public GetUserInputOperation CreateGetUserInputOperation(Entity entityInfo) 
            => new GetUserInputOperation(entityInfo, _userInteraction);

        public GenerateCodeOperation CreateGenerateCodeOperation(IUIResultProvider uiResultProvider, Entity entityInfo, Document document) 
            => new GenerateCodeOperation(
                uiResultProvider,
                _entityGenerator,
                _codeGenerator,
                entityInfo,
                document,
                _logger
            );
    }
}
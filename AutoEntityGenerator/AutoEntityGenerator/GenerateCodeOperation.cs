﻿using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using System;
using System.IO;
using System.Threading;

namespace AutoEntityGenerator
{
    internal class GenerateCodeOperation : CodeActionOperation
    {
        private readonly ILogger _logger;
        private readonly IUIResultProvider _resultProvider;
        private readonly IEntityGenerator _entityGenerator;
        private readonly Entity _entityInfo;
        private readonly ICodeFileGenerator _codeGenerator;
        private readonly Document _document;

        public GenerateCodeOperation(IUIResultProvider getUserInputOperation, IEntityGenerator entityGenerator, ICodeFileGenerator codeGenerator, Entity entityInfo, Document document, ILogger logger)
        {
            _resultProvider = getUserInputOperation;
            _codeGenerator = codeGenerator;
            _entityInfo = entityInfo;
            _document = document;
            _entityGenerator = entityGenerator;
            _logger = logger;
        }

        public override void Apply(Workspace workspace, CancellationToken cancellationToken)
        {
            var result = _resultProvider.UserInteractionResult;
            if (!result.IsOk)
            {
                _logger.Information("The user cancled the operation.");
                return;
            }

            _logger.Information("Attempting to generate dto and mappings.");

            var dtoEntity = _entityGenerator.GenerateFromUIResult(result, _entityInfo);

            var (from, to) = result.MappingDirection == MappingDirection.FromDtoToModel
                ? (dtoEntity, _entityInfo)
                : (_entityInfo, dtoEntity);

            var dto = _codeGenerator.GenerateEntityCodeFile(dtoEntity);
            var mapping = _codeGenerator.GenerateMappingCodeFile(from, to);

            var dtoDocument = AddDocument(_document, dto.FileName, dto.Content, result.TargetDirectory);
            var mappingDocument = AddDocument(dtoDocument, mapping.FileName, mapping.Content, result.TargetDirectory);

            _logger.Debug("Attempting to save changes.");
            try
            {
                workspace.TryApplyChanges(mappingDocument.Project.Solution);

                _logger.Information($"Dto and mapping extension classes saved to {result.TargetDirectory}.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to appy changes.");
                throw;
            }
        }

        private Document AddDocument(Document document, string fileName, string code, string targetFolder)
        {
            var filePath = Path.Combine(targetFolder, fileName);
            string[] folders = targetFolder == Path.GetDirectoryName(_entityInfo.SourceFilePath)
                ? null
                : new[] { targetFolder };
            _logger.Debug($"Attempting to add document. File name: {fileName}.");
            return document.Project.AddDocument(fileName, code, folders, filePath);
        }

    }
}

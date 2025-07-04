﻿using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoEntityGenerator.CodeOperations;

internal class GenerateCodeOperation : CodeActionOperation
{
    private readonly ILogger<GenerateCodeOperation> _logger;
    private readonly IUIResultProvider _resultProvider;
    private readonly IEntityGenerator _entityGenerator;
    private readonly IEntityProvider _entityProvider;
    private readonly ICodeFileGenerator _codeGenerator;
    private readonly IDocumentOpener _documentOpener;
    private readonly Document _document;

    public GenerateCodeOperation(IUIResultProvider getUserInputOperation, IEntityGenerator entityGenerator, ICodeFileGenerator codeGenerator, IEntityProvider entityProvider, Document document, ILogger<GenerateCodeOperation> logger, IDocumentOpener documentOpener)
    {
        _resultProvider = getUserInputOperation;
        _codeGenerator = codeGenerator;
        _entityProvider = entityProvider;
        _document = document;
        _entityGenerator = entityGenerator;
        _logger = logger;
        _documentOpener = documentOpener;
    }

    public override void Apply(Workspace workspace, CancellationToken cancellationToken)
    {
        var result = _resultProvider.UserInteractionResult;
        if (!result.IsOk)
        {
            _logger.LogInformation("The user cancelled the operation.");
            return;
        }

        _logger.LogInformation("Attempting to generate dto {dtoName} and mapping extension.", result.EntityName);

        var sourceEntity = _entityProvider.Entity;
        var dtoEntity = _entityGenerator.GenerateFromUIResult(result, sourceEntity);

        var (from, to) = result.MappingDirection == MappingDirection.FromDtoToModel
            ? (dtoEntity, sourceEntity)
            : (sourceEntity, dtoEntity);

        var dto = _codeGenerator.GenerateEntityCodeFile(dtoEntity);
        var mapping = _codeGenerator.GenerateMappingCodeFile(from, to);
        _logger.LogDebug("Dto and Mapping generated.");
        var dtoDocument = AddDocument(_document, dto.FileName, dto.Content, result.TargetDirectory, sourceEntity);
        var mappingDocument = AddDocument(dtoDocument, mapping.FileName, mapping.Content, result.TargetDirectory, sourceEntity);

        _logger.LogDebug("Attempting to save changes.");

        try
        {
            if (workspace.TryApplyChanges(mappingDocument.Project.Solution))
            {
                _logger.LogInformation("Dto and mapping extension class saved to {TargetDirectory}.", result.TargetDirectory);
                if (result.OpenFiles)
                {
                    _documentOpener.OpenDocument(mappingDocument.FilePath);
                    _documentOpener.OpenDocument(dtoDocument.FilePath);
                }
            }
            else
            {
                _logger.LogWarning("TryApplyChanges returned false. Changes was not saved.");
            }
        }
        // This will be thrown if the Apply method doesn't run on the UI thread.
        catch (COMException ex) 
        {
            _logger.LogError(ex, "Failed to open documents.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply changes.");
            throw;
        }
    }

    private Document AddDocument(Document document, string fileName, string code, string targetFolder, Entity sourceEntity)
    {
        _logger.LogDebug("Attempting to add document {Filename}.", fileName);
        var filePath = Path.Combine(targetFolder, fileName);
        string[] folders = targetFolder == Path.GetDirectoryName(sourceEntity.SourceFilePath)
            ? null
            : new[] { targetFolder };

        var newDocument = document.Project.AddDocument(fileName, code, folders, filePath);
        _logger.LogDebug("document {Filename} Added.", fileName);
        return newDocument;
    }

}

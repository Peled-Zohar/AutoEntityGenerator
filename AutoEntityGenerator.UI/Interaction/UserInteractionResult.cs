﻿using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoEntityGenerator.UI.Interaction
{
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    internal class UserInteractionResult : IUserInteractionResult
    {
        internal UserInteractionResult() => IsOk = false;

        internal UserInteractionResult(MappingDirection mappingDirection, string targetDirectory, string entityName, List<Property> entityProperties, string fileName, bool openFiles)
        {
            IsOk = true;
            MappingDirection = mappingDirection;
            TargetDirectory = targetDirectory;
            EntityName = entityName;
            EntityProperties = entityProperties;
            FileName = fileName;
            OpenFiles = openFiles;
        }

        public bool IsOk { get; }
        public MappingDirection MappingDirection { get; }
        public string TargetDirectory { get; }
        public string EntityName { get; }
        public List<Property> EntityProperties { get; }
        public string FileName { get; }
        public bool OpenFiles { get; }
    }
}

using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System.Collections.Generic;

namespace AutoEntityGenerator.UI
{
    internal class UserInteractionResult : IUserInteractionResult
    {
        internal UserInteractionResult()
        {
            IsOk = false;
        }

        internal UserInteractionResult(MappingDirection mappingDirection, string targetDirectory, string entityName, List<Property> entityProperties, string fileName)
        {
            IsOk = true;
            MappingDirection = mappingDirection;
            TargetDirectory = targetDirectory;
            EntityName = entityName;
            EntityProperties = entityProperties;
            FileName = fileName;
        }

        public bool IsOk { get; }
        public MappingDirection MappingDirection { get; }
        public string TargetDirectory { get; }
        public string EntityName { get; }
        public List<Property> EntityProperties { get; }
        public string FileName { get; }
    }
}

using AutoEntityGenerator.Common.CodeInfo;
using System.Collections.Generic;

namespace AutoEntityGenerator.Common.Interfaces
{
    public interface IUserInteractionResult
    {
        bool IsOk { get; }
        MappingDirection MappingDirection { get; }
        string TargetDirectory { get; }
        string EntityName { get; }
        List<Property> EntityProperties { get; }
        string FileName { get; }
    }
}

using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.Common.Interfaces
{
    public interface ICodeFileGenerator
    {
        IGeneratedCodeFile GenerateEntityCodeFile(Entity entityInfo);
        IGeneratedCodeFile GenerateMappingCodeFile(Entity from, Entity to);
    }
}

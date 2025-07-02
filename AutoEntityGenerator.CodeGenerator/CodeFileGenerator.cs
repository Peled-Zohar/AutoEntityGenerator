using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AutoEntityGenerator.CodeGenerator.Tests")]

namespace AutoEntityGenerator.CodeGenerator;

internal class CodeFileGenerator : ICodeFileGenerator
{
    private readonly IEntityGenerator _entityGenerator;
    private readonly IMappingsClassGenerator _mappingsClassGenerator;

    public CodeFileGenerator(IEntityGenerator entityGenerator, IMappingsClassGenerator mappingsClassGenerator)
    {
        _entityGenerator = entityGenerator;
        _mappingsClassGenerator = mappingsClassGenerator;
    }

    public IGeneratedCodeFile GenerateEntityCodeFile(Entity entityInfo)
    {
        if (entityInfo is null)
        {
            return null;
        }

        var code = _entityGenerator.GenerateEntityCode(entityInfo);
        return new GeneratedCodeFile(code, Path.GetFileName(entityInfo.SourceFilePath));
    }

    public IGeneratedCodeFile GenerateMappingCodeFile(Entity from, Entity to)
    {
        if (from is null || to is null)
        {
            return null;
        }

        var code = _mappingsClassGenerator.GenerateMappingClassCode(from, to);
        return new GeneratedCodeFile(code, Path.GetFileNameWithoutExtension(from.SourceFilePath) + "MappingExtensions.cs");
    }
}

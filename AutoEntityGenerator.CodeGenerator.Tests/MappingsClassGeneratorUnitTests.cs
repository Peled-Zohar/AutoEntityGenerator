using AutoEntityGenerator.Common.CodeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoEntityGenerator.CodeGenerator.Tests;

public class MappingsClassGeneratorUnitTests
{

    private MappingsClassGenerator _mappingsClassGenerator;
    private Entity _testEntity;
    private Entity _testMappingTargetEntity;

    [SetUp]
    public void Setup()
    {
        _mappingsClassGenerator = new();

        _testEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject/TestProject.csproj", CSharpVersion = CSharpVersion.Default },
            Properties = [
                new() { IsReadonly = false, Name = "A", Type = "int" },
                new() { IsReadonly = false, Name = "B", Type = "string" },
                new() { IsReadonly = false, Name = "C", Type = "float" },
                new() { IsReadonly = true, Name = "D", Type = "string" }
            ],
            Name = "Test",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntities/TestEntity.cs",
            TypeParameters = [],
        };

        _testMappingTargetEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject/TestProject.csproj", CSharpVersion = CSharpVersion.Default },
            Properties = [
                new() { IsReadonly = false, Name = "A", Type = "int" },
                new() { IsReadonly = false, Name = "B", Type = "string" },
                new() { IsReadonly = false, Name = "C", Type = "float" },
            ],
            Name = "TestMapping",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntities/TestMapping.cs",
            TypeParameters = [],
        };
    }
}

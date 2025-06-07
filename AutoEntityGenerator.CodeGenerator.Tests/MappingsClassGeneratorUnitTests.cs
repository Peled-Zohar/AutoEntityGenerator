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

    [TestCase("", "TestMapping", "TestMapping")]
    [TestCase("TargetNamespace", "TargetClass", "TargetNamespace.TargetClass")]
    public void GenerateMappingClassCode_TargetNamespaceNameMissing_MappingMethodReturnTypeWithoutNamespace(string targetNamespaceName, string targetName, string fullTargetName)
    {
        _testMappingTargetEntity.Name = targetName;
        _testMappingTargetEntity.Namespace.Name = targetNamespaceName;

        var expectedMethodDeclarationString = $@"public static {fullTargetName} To{_testMappingTargetEntity.Name}(this {_testEntity.Name} source)";
        var result = _mappingsClassGenerator.GenerateMappingClassCode(_testEntity, _testMappingTargetEntity);
        Assert.That(result, Does.Contain(expectedMethodDeclarationString));
    }

    [TestCase("")]
    [TestCase("T")]
    [TestCase("T1", "T2")]
    public void GenerateMappingClassCode_TypeParameters_IncludeTypeParametersInMappingCode(params string[] typeParams)
    {

        _testMappingTargetEntity.TypeParameters = string.IsNullOrEmpty(typeParams[0]) ? [] : [.. typeParams];
        var typeParam = string.IsNullOrEmpty(typeParams[0])? "" : $"<{string.Join(", ", typeParams)}>";
        var expectedMethodDeclarationString = $@"public static {_testMappingTargetEntity.Namespace.Name}.{_testMappingTargetEntity.Name}{typeParam} To{_testMappingTargetEntity.Name}{typeParam}(this {_testEntity.Name}{typeParam} source)";

        var result = _mappingsClassGenerator.GenerateMappingClassCode(_testEntity, _testMappingTargetEntity);

        Assert.That(result, Does.Contain(expectedMethodDeclarationString));
    }

    [Test]
    public void GenerateMappingClassCode_GenericConstraints_IncludeGenericConstraintsInMappingCode()
    {
        _testMappingTargetEntity.TypeParameters = ["T"];
        
        _testMappingTargetEntity.GenericConstraints = ["where T : class"];

        var typeParam = "<T>";
        var genericConstraintsString = " where T : class";
        var expectedMethodDeclarationString = $@"public static {_testMappingTargetEntity.Namespace.Name}.{_testMappingTargetEntity.Name}{typeParam} To{_testMappingTargetEntity.Name}{typeParam}(this {_testEntity.Name}{typeParam} source){genericConstraintsString}";

        var result = _mappingsClassGenerator.GenerateMappingClassCode(_testEntity, _testMappingTargetEntity);

        Assert.That(result, Does.Contain(expectedMethodDeclarationString));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void GenerateMappingClassCode_DifferentScopeNamespace_ReturnsCorrectlyIndentedCode(bool isFileScoped)
    {
        _testEntity.Namespace.IsFileScoped = isFileScoped;

        var propertyListIndendaion = isFileScoped ? "\t\t\t" : "\t\t\t\t";

        var expectedNamespaceString = isFileScoped
    ? @"
namespace TestNamespace;
"
    : @"
namespace TestNamespace
{";

        var expectedClassDeclarationString = $"public static partial class {_testEntity.Name}MappingExtensions";

        var expectedMethodDeclarationString = $"    public static {_testMappingTargetEntity.Namespace.Name}.{_testMappingTargetEntity.Name} To{_testMappingTargetEntity.Name}(this {_testEntity.Name} source)";

        var expectedFirstPropertyString = $"{propertyListIndendaion}A = source.A,";

        if (!isFileScoped)
        {
            expectedClassDeclarationString = "  " + expectedClassDeclarationString;
            expectedMethodDeclarationString = " " + expectedMethodDeclarationString;
        }

        var result = _mappingsClassGenerator.GenerateMappingClassCode(_testEntity, _testMappingTargetEntity);
        Assert.That(result, Does.Contain(expectedNamespaceString));
        Assert.That(result, Does.Contain(expectedClassDeclarationString));
        Assert.That(result, Does.Contain(expectedMethodDeclarationString));
        Assert.That(result, Does.Contain(expectedFirstPropertyString));
    }
}

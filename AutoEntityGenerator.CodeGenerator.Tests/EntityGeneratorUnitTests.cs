using AutoEntityGenerator.Common.CodeInfo;
using NUnit.Framework.Legacy;
using System.Reflection.Emit;

namespace AutoEntityGenerator.CodeGenerator.Tests;

public class EntityGeneratorUnitTests
{
    Entity _testEntity;
    EntityGenerator _generator;

    [SetUp]
    public void Setup()
    {
        _generator = new EntityGenerator();

        _testEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject", CSharpVersion = CSharpVersion.Default },
            Properties = new() { new() { IsReadonly = false, Name = "TestProperty", Type = "int" } },
            Name = "Test",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntity",
            TypeParameters = [],
        };
    }

    [TestCase(CSharpVersion.CSharp9)]
    [TestCase(CSharpVersion.CSharp10)]
    [TestCase(CSharpVersion.CSharp11)]
    [TestCase(CSharpVersion.CSharp12)]
    [TestCase(CSharpVersion.Default)]
    public void GenerateEntityCode_CSharpVersion9OrHigher_GenerateRecord(CSharpVersion cSharpVersion)
    {
        _testEntity.Project.CSharpVersion = cSharpVersion;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, !Does.Contain("class"));
        Assert.That(generated, Does.Contain("record"));
    }

    [TestCase(CSharpVersion.CSharp1)]
    [TestCase(CSharpVersion.CSharp1)]
    [TestCase(CSharpVersion.CSharp3)]
    [TestCase(CSharpVersion.CSharp4)]
    [TestCase(CSharpVersion.CSharp5)]
    [TestCase(CSharpVersion.CSharp6)]
    [TestCase(CSharpVersion.CSharp7)]
    [TestCase(CSharpVersion.CSharp7_1)]
    [TestCase(CSharpVersion.CSharp7_2)]
    [TestCase(CSharpVersion.CSharp7_3)]
    [TestCase(CSharpVersion.CSharp8)]
    public void GenerateEntityCode_CSharpVersion8OrLower_GenerateClass(CSharpVersion cSharpVersion)
    {
        _testEntity.Project.CSharpVersion = cSharpVersion;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, !Does.Contain("record"));
        Assert.That(generated, Does.Contain("class"));
    }

    [Test]
    public void GenerateEntityCode_EntityWithFileScopedNamespace_ReturnFileScopedNamespace()
    {
        _testEntity.Namespace.IsFileScoped = true;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, !Does.Contain(_testEntity.Namespace.Name + Environment.NewLine + "{"));
        Assert.That(generated, Does.Contain(_testEntity.Namespace.Name + ";"));
    }

    [Test]
    public void GenerateEntityCode_EntityWithScopedNamespace_ReturnScopedNamespace()
    {
        _testEntity.Namespace.IsFileScoped = false;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, !Does.Contain(_testEntity.Namespace.Name + ";"));
        Assert.That(generated, Does.Contain(_testEntity.Namespace.Name + Environment.NewLine + "{"));
    }

    [Test]
    public void GenerateEntityCode_EntityWithTypeParameters_ReturnGenericClass()
    {
        List<string> typeParameters = ["TSource", "TTarget"];
        _testEntity.TypeParameters = typeParameters;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, Does.Contain(_testEntity.Name + $"<{string.Join(", ", typeParameters)}>"));
    }

    [Test]
    public void GenerateEntityCode_EntityWithGenericConstraints_ReturnGenericClassWithCorrectConstraints()
    {
        List<string> typeParameters = ["TSource", "TTarget"];
        List<string> genericConstraints = ["where TSource : class", "where TTarget : class, new()"];
        _testEntity.TypeParameters = typeParameters;
        _testEntity.GenericConstraints = genericConstraints;

        var generated = _generator.GenerateEntityCode(_testEntity);

        Assert.That(generated, Does.Contain(_testEntity.Name + $"<{string.Join(", ", typeParameters)}>" + " " + string.Join(" ", genericConstraints)));
    }

    [Test]
    public void GenerateEntityCode_FileScopedNamespace_PropertiesIndentedCorrectly()
    {
        _testEntity.Namespace.IsFileScoped = true;
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.Contain("\n\tpublic int TestProperty {get;set;}"));
    }

    [Test]
    public void GenerateEntityCode_ScopedNamespace_PropertiesIndentedCorrectly()
    {
        _testEntity.Namespace.IsFileScoped = false;
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.Contain("\n\t\tpublic int TestProperty {get;set;}"));
    }

    [Test]
    public void GenerateEntityCode_MultipleProperties_AllPropertiesGenerated()
    {
        _testEntity.Properties.Add(new() { IsReadonly = false, Name = "AnotherProperty", Type = "string" });
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.Contain("public int TestProperty {get;set;}"));
        Assert.That(generated, Does.Contain("public string AnotherProperty {get;set;}"));
    }

    [Test]
    public void GenerateEntityCode_DifferentPropertyTypes_GeneratedCorrectly()
    {
        _testEntity.Properties.Add(new() { Name = "StringsProperty", Type = "List<string>" });
        _testEntity.Properties.Add(new() { Name = "SomeClassProperty", Type = "SomeClass" });
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.Contain("public int TestProperty {get;set;}"));
        Assert.That(generated, Does.Contain("public List<string> StringsProperty {get;set;}"));
        Assert.That(generated, Does.Contain("public SomeClass SomeClassProperty {get;set;}"));
    }

    [Test]
    public void GenerateEntityCode_Always_ReturnsAPartialClass()
    {
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.Contain(" partial "));
    }

    [Test]
    public void GenerateEntityCode_Always_IncludesComments()
    {
        var generated = _generator.GenerateEntityCode(_testEntity);
        Assert.That(generated, Does.StartWith(_generator.Comments));
    }
}
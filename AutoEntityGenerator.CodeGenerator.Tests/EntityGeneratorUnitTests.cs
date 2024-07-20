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


}
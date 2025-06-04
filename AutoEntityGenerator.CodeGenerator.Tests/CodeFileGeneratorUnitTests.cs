using AutoEntityGenerator.Common.CodeInfo;
using FakeItEasy;

namespace AutoEntityGenerator.CodeGenerator.Tests;

class CodeFileGeneratorUnitTests
{
    private IEntityGenerator _entityGenerator;
    private IMappingsClassGenerator _mappingsClassGenerator;
    private CodeFileGenerator _codeFileGenerator;

    private Entity _testEntity;
    private Entity _testMappingTargetEntity;

    [SetUp]
    public void Setup()
    {
        _entityGenerator = A.Fake<IEntityGenerator>();
        _mappingsClassGenerator = A.Fake<IMappingsClassGenerator>();
        _codeFileGenerator = new CodeFileGenerator(_entityGenerator, _mappingsClassGenerator);

        _testEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject", CSharpVersion = CSharpVersion.Default },
            Properties = [new() { IsReadonly = false, Name = "TestProperty", Type = "int" }],
            Name = "Test",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntity",
            TypeParameters = [],
        };

        _testMappingTargetEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject", CSharpVersion = CSharpVersion.Default },
            Properties = [new() { IsReadonly = false, Name = "TestProperty", Type = "int" }],
            Name = "Target",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestMappingTargetEntity",
            TypeParameters = [],
        };
    }

    [Test]
    public void GenerateEntityCodeFileNullEntityReturnsNull()
    {
        var returnValue = _codeFileGenerator.GenerateEntityCodeFile(null);
        Assert.That(returnValue, Is.Null);
    }

    [Test]
    public void GenerateEntityCodeFileValidEntityReturnsValidResponse()
    {
        A.CallTo(() => _entityGenerator.GenerateEntityCode(A<Entity>._))
            .Returns("A non empty string value");

        var returnValue = _codeFileGenerator.GenerateEntityCodeFile(_testEntity);
        A.CallTo(() => _entityGenerator.GenerateEntityCode(_testEntity)).MustHaveHappened();
        Assert.That(returnValue, Is.Not.Null);
        Assert.That(returnValue.FileName, Is.EqualTo(Path.GetFileName(_testEntity.SourceFilePath)));
        Assert.That(returnValue.Content, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GenerateMappingCodeFileNullFromReturnsNull()
    {
        var returnValue = _codeFileGenerator.GenerateMappingCodeFile(null, _testMappingTargetEntity);
        Assert.That(returnValue, Is.Null);
    }

    [Test]
    public void GenerateMappingCodeFileNullToReturnsNull()
    {
        var returnValue = _codeFileGenerator.GenerateMappingCodeFile(_testEntity, null);
        Assert.That(returnValue, Is.Null);
    }

    [Test]
    public void GenerateMappingCodeFileValidInputReturnsValidResponse()
    {
        A.CallTo(() => _mappingsClassGenerator.GenerateMappingClassCode(A<Entity>._, A<Entity>._))
            .Returns("A non empty string value");
            
        var returnValue = _codeFileGenerator.GenerateMappingCodeFile(_testEntity, _testMappingTargetEntity);
        A.CallTo(() => _mappingsClassGenerator.GenerateMappingClassCode(_testEntity, _testMappingTargetEntity)).MustHaveHappened();
        Assert.That(returnValue, Is.Not.Null);
        Assert.That(returnValue.FileName, Is.EqualTo(Path.GetFileNameWithoutExtension(_testEntity.SourceFilePath) + "MappingExtensions.cs"));
        Assert.That(returnValue.Content, Is.Not.Null.And.Not.Empty);
    }
}

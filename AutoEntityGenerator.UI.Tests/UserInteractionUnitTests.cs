using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.DependencyInjection;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Views;
using FakeItEasy;

namespace AutoEntityGenerator.UI.Tests;

class UserInteractionUnitTests
{
    private IEntityConfigurationWindowFactory _factory;
    private UserInteraction _userInteraction;
    private Entity _testEntity;

    [SetUp]
    public void Setup()
    {
        _factory = A.Fake<IEntityConfigurationWindowFactory>();
        _userInteraction = new UserInteraction(_factory);

        _testEntity = new Entity()
        {
            Constructors = [],
            GenericConstraints = [],
            Project = new() { DefaultNamespace = "TestNamespace", FilePath = "C:/TestProject/TestProject.csproj", CSharpVersion = CSharpVersion.Default },
            Properties = [
            new() { IsReadonly = false, Name = "A", Type = "int" },
            new() { IsReadonly = false, Name = "B", Type = "string" },
            new() { IsReadonly = false, Name = "C", Type = "int" },
            new() { IsReadonly = true, Name = "D", Type = "string" }
        ],
            Name = "Test",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntities/TestEntity.cs",
            TypeParameters = [],
        };
    }

    [TestCase(null, false)]
    [TestCase(true, true)]
    [TestCase(false, false)]
    public void ShowUIForm_ReturnsIUserInteractionResult_BasedOnDialogResult(bool? showDialogResult, bool resultIsOk)
    {

        A.CallTo(() => _factory.Create(_testEntity))
            .ReturnsLazily(() =>
            {
                var window = A.Fake<IEntityConfigurationWindow>();
                A.CallTo(() => window.ShowDialog()).Returns(showDialogResult);
                A.CallTo(() => window.Result)
                    .Returns(new UserInteractionResult(
                        Common.Interfaces.MappingDirection.FromDtoToModel, 
                        "Generated", 
                        _testEntity.Name, 
                        _testEntity.Properties, 
                        _testEntity.Name + ".cs",
                        false
                    )
                );
                return window;
            });

        var result = _userInteraction.ShowUIForm(_testEntity);

        Assert.That(result.IsOk, Is.EqualTo(resultIsOk), "The result's IsOk property should match the expected value.");

    }
}

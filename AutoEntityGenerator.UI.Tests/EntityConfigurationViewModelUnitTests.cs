using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.Validators;
using AutoEntityGenerator.UI.ViewModels;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.UI.Tests;
internal class EntityConfigurationViewModelUnitTests
{
    EntityConfigurationViewModel _testViewModel;
    EntityConfigurationViewModelValidator _validator;
    IDialogService _fakeDialogService;
    ILogger<EntityConfigurationViewModel> _fakeLogger;
    IAppSettings _fakeAppSettings;

    [SetUp]
    public void Setup()
    {
        
        var testEntity = new Entity()
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
        _fakeLogger = A.Fake<ILogger<EntityConfigurationViewModel>>();
        _fakeDialogService = A.Fake<IDialogService>();
        _fakeAppSettings = A.Fake<IAppSettings>();
        _validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(testEntity.Project.FilePath));
        _testViewModel = new EntityConfigurationViewModel(_fakeAppSettings, _fakeLogger, _validator, _fakeDialogService, testEntity);
        foreach (var property in _testViewModel.Properties)
        {
            property.IsSelected = true;
        }
    }

    [TestCase(["A"])]
    [TestCase(["A", "B"])]
    [TestCase(["A", "B", "C"])]
    [TestCase(["A", "B", "C", "D"])]
    [TestCase(["A", "B", "D"])]
    [TestCase(["A", "C", "D"])]
    [TestCase(["B", "C", "D"])]
    [TestCase(["C", "D"])]
    [TestCase(["D"])]
    public void Save_SelectedProperties_ResultShowsOnlySelectedProperties(params string[] propertiesToSelect)
    {
        foreach(var property in _testViewModel.Properties)
        {
            property.IsSelected = propertiesToSelect.Contains(property.Name);
        }
        _testViewModel.SaveCommand.Execute(null);


        Assert.That(propertiesToSelect.SequenceEqual(_testViewModel.Result.EntityProperties.Select(p => p.Name)), Is.True);
            
    }


}

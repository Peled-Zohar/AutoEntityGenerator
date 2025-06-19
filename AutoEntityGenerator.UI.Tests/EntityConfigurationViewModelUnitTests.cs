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
    IAppSettings _appSettings;
    Entity _testEntity;

    [SetUp]
    public void Setup()
    {
        
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
        _fakeLogger = A.Fake<ILogger<EntityConfigurationViewModel>>();
        _fakeDialogService = A.Fake<IDialogService>();
        _appSettings = new AppSettingsImplementation
        {
            DestinationFolder = "Generated",
            MinimumLogLevel = LogLevel.Information,
            OpenGeneratedFiles = false,
            RequestSuffix = "Request",
            ResponseSuffix = "Response",
        };

        _validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(_testEntity.Project.FilePath));
        _testViewModel = new EntityConfigurationViewModel(_appSettings, _fakeLogger, _validator, _fakeDialogService, _testEntity);
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

    [Test]
    public void Save_InvalidInput_ShowValidationErrors()
    {
        _testViewModel.DtoName = "345-InvalidName";
        _testViewModel.SaveCommand.Execute(null);

        A.CallTo(() => _fakeDialogService.ShowDialog(A<string>._, A<string>._))
            .MustHaveHappened();
    }


    [Test]
    public void SelectAllCommand_Execute_AllPropertiesSelected()
    {
        _testViewModel.SelectAllCommand.Execute(null);
        Assert.That(_testViewModel.Properties.All(p => p.IsSelected), Is.True);
    }

    [Test]
    public void UnselectAllCommand_Execute_AllPropertiesNotSelected()
    {
        _testViewModel.UnselectAllCommand.Execute(null);
        Assert.That(_testViewModel.Properties.All(p => p.IsSelected), Is.False);
    }

    [Test]
    public void Browse_FolderPickerDialogCanceled_DestinationFolderUnchanged()
    {
        var initialDestinationFolder = _testViewModel.DestinationFolder;
        A.CallTo(() => _fakeDialogService.ShowFolderPickerDialog(A<string>._))
            .Returns((false, ""));

        _testViewModel.BrowseCommand.Execute(null);

        Assert.That(_testViewModel.DestinationFolder, Is.EqualTo(initialDestinationFolder));
    }

    [Test]
    public void Browse_FolderPickerDialogReturnedInvalidPath_DestinationFolderUnchanged()
    {
        var initialDestinationFolder = _testViewModel.DestinationFolder;
        A.CallTo(() => _fakeDialogService.ShowFolderPickerDialog(A<string>._))
            .Returns((true, "C:/This_path_is_not_in_the_project_path_and_therefor_invalid"));

        _testViewModel.BrowseCommand.Execute(null);
        A.CallTo(() => _fakeDialogService.ShowDialog(A<string>._, A<string>._))
            .MustHaveHappened();
        Assert.That(_testViewModel.DestinationFolder, Is.EqualTo(initialDestinationFolder));
    }

    [Test]
    public void Browse_FolderPickerDialogReturnedValidPath_DestinationFolderUpdated()
    {
        var projectPath = Path.GetDirectoryName(_testEntity.Project.FilePath)!;
        var targetDirectory = "SomeDirectory";
        var returnedPath = Path.Combine(projectPath, targetDirectory);
        var initialDestinationFolder = _testViewModel.DestinationFolder;
        A.CallTo(() => _fakeDialogService.ShowFolderPickerDialog(A<string>._))
            .Returns((true, returnedPath));

        _testViewModel.BrowseCommand.Execute(null);

        Assert.That(_testViewModel.DestinationFolder, Is.EqualTo(targetDirectory));
    }

    [Test]
    public void SettingsChanged_ValuesWhereNotChangedByTheUser_ValuesChanged()
    {
        var openGeneratedFiles = _testViewModel.OpenGeneratedFiles;
        var destinationFolder = _testViewModel.DestinationFolder;   
        _appSettings.OpenGeneratedFiles = !openGeneratedFiles;
        _appSettings.DestinationFolder += "some string";

        _testViewModel.SettingsChanged();

        Assert.That(_testViewModel.OpenGeneratedFiles, Is.Not.EqualTo(openGeneratedFiles));
        Assert.That(_testViewModel.DestinationFolder, Is.Not.EqualTo(destinationFolder));
    }

    [Test]
    public void SettingsChanged_ValuesChangedByTheUser_ValuesStayAsUserEntered()
    {
        var openGeneratedFiles = _testViewModel.OpenGeneratedFiles;
        var expectedDestinationFolder = "some different destination folder";


        _testViewModel.OpenGeneratedFiles = !openGeneratedFiles;
        _testViewModel.OpenGeneratedFiles = openGeneratedFiles;
        _testViewModel.DestinationFolder = expectedDestinationFolder;

        _appSettings.OpenGeneratedFiles = !openGeneratedFiles;
        _appSettings.DestinationFolder += "some string";

        _testViewModel.SettingsChanged();

        Assert.That(_testViewModel.OpenGeneratedFiles, Is.EqualTo(openGeneratedFiles));
        Assert.That(_testViewModel.DestinationFolder, Is.EqualTo(expectedDestinationFolder));
    }


}

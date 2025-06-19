using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.Validators;
using AutoEntityGenerator.UI.ViewModels;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.UI.Tests;

public class EntityConfigurationViewModelValidatorTests
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
                new() { IsReadonly = false, Name = "IntProperty", Type = "int" },
                new() { IsReadonly = false, Name = "StringProperty", Type = "string" }
            ],
            Name = "Test",
            Namespace = new() { Name = "TestNamespace", IsFileScoped = false },
            SourceFilePath = "C:/TestProject/TestEntities/TestEntity.cs",
            TypeParameters = [],
        };
        _fakeDialogService = A.Fake<IDialogService>();
        _fakeLogger = A.Fake<ILogger<EntityConfigurationViewModel>>();
        
        _fakeAppSettings = A.Fake<IAppSettings>();

        _validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(testEntity.Project.FilePath));
        _testViewModel = new EntityConfigurationViewModel(_fakeAppSettings, _fakeLogger, _validator, _fakeDialogService, testEntity);
        foreach (var property in _testViewModel.Properties)
        {
            property.IsSelected = true;
        }
    }


    [TestCase("", false)] 
    [TestCase("  ", false)] 
    [TestCase("4asdf", false)] 
    [TestCase("Asdf-erg", false)] 
    [TestCase("false", false)] 
    [TestCase("int", false)] 
    [TestCase("AValidTypeName", true)] 
    public void Valiidate_DtoName_ReturnExpectedResult(string dtoName, bool expected)
    {
        _testViewModel.DtoName = dtoName;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }

    [TestCase("", false)]
    [TestCase("  ", false)] 
    [TestCase("contains an invalid char (\b)", false)] 
    [TestCase("fileName", false)] 
    [TestCase("fileName.vb" ,false)] 
    [TestCase("SomeValidFileName.cs", true)] 
    public void Valiidate_GeneratedFileName_ReturnExpectedResult(string generatedFileName, bool expected)
    {
        A.CallTo(() => _fakeDialogService.ShowYesNoDialog(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        _testViewModel.GeneratedFileName = generatedFileName;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }


    [TestCase("C:/SomeAbsoulutePath", false)] 
    [TestCase("Directory with an invalid char (\t)", false)] 
    [TestCase("Directory", true)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    public void Valiidate_DestinationFolder_ReturnExpectedResult(string destinationFolder, bool expected)
    {
        _testViewModel.DestinationFolder = destinationFolder;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }


    [TestCase(false, false)] 
    [TestCase(true, true)] 
    public void Valiidate_SelectedProperties_ReturnExpectedResult(bool selectAtLeastOne, bool expected)
    {
        foreach (var property in _testViewModel.Properties)
        {
            property.IsSelected = false;
        }
        _testViewModel.Properties[0].IsSelected = selectAtLeastOne;

        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }
}
using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.ViewModels;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.UI.Tests;

public class EntityConfigurationViewModelValidatorTests
{
    EntityConfigurationViewModel _testViewModel;
    EntityConfigurationViewModelValidator _validator;
    IDialogService _dialogService;
    ILogger<EntityConfigurationViewModel> _logger;

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
        _dialogService = A.Fake<IDialogService>();
        _logger = A.Fake<ILogger<EntityConfigurationViewModel>>();
        _validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(testEntity.Project.FilePath));
        _testViewModel = new EntityConfigurationViewModel(_logger, _validator, _dialogService, testEntity);
        foreach (var property in _testViewModel.Properties)
        {
            property.IsSelected = true;
        }
    }


    [TestCase("", false)] // empty
    [TestCase("  ", false)] // white space
    [TestCase("4asdf", false)] // starts with a digit
    [TestCase("Asdf-erg", false)] // contains invalid chars
    [TestCase("false", false)] // is a keyword
    [TestCase("int", false)] // is a primitive
    [TestCase("AValidTypeName", true)] // A valid type identifier
    public void Valiidate_DtoName_ReturnExpectedResult(string dtoName, bool expected)
    {
        _testViewModel.DtoName = dtoName;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }


    // GeneratedFileName
    [TestCase("", false)] // empty
    [TestCase("  ", false)] // white space
    [TestCase("contains an invalid char (\b)", false)] // Contains and invalid char
    [TestCase("fileName", false)] // no extension
    [TestCase("fileName.vb" ,false)] // wrong extension
    [TestCase("SomeValidFileName.cs", true)] // A valid file name
    public void Valiidate_GeneratedFileName_ReturnExpectedResult(string generatedFileName, bool expected)
    {
        A.CallTo(() => _dialogService.ShowYesNoDialog(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        _testViewModel.GeneratedFileName = generatedFileName;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }


    [TestCase("C:/SomeAbsoulutePath", false)] // A rooted path
    [TestCase("Directory with an invalid char (\t)", false)] // Contains and invalid char
    [TestCase("Directory", true)] // A valid directory name
    public void Valiidate_DestinationFolder_ReturnExpectedResult(string destinationFolder, bool expected)
    {
        _testViewModel.DestinationFolder = destinationFolder;
        var validationResult = _validator.Validate(_testViewModel);

        Assert.That(expected, Is.EqualTo(validationResult.IsValid));
    }


    [TestCase(false, false)] // unselect all properties
    [TestCase(true, true)] // select at least one property
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
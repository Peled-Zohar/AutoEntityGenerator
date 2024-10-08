﻿using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.ViewModels;
using Castle.Core.Logging;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace AutoEntityGenerator.UI.Tests;
internal class EntityConfigurationViewModelUnitTests
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
        _logger = A.Fake<ILogger<EntityConfigurationViewModel>>();
        _dialogService = A.Fake<IDialogService>();
        _validator = new EntityConfigurationViewModelValidator(Path.GetDirectoryName(testEntity.Project.FilePath));
        _testViewModel = new EntityConfigurationViewModel(_logger, _validator, _dialogService, testEntity);
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

using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.ViewModels;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.UI.Tests;

class SettingsViewModelUnitTests
{
    private SettingsViewModel _settingsViewModel;
    private IAppSettings _appSettings, _updatedAppSettings;
    private IValidator<IAppSettings> _fakeValidator;
    private IConfigurationSaver _fakeConfigurationSaver;
    private IDialogService _fakeDialogService;

    [SetUp]
    public void Setup()
    {
        _appSettings = new AppSettingsImplementation
        { 
            MinimumLogLevel = LogLevel.Information,
            DestinationFolder = "DefaultDestinationFolder",
            RequestSuffix = "Request",
            ResponseSuffix = "Response"
        };
        _updatedAppSettings = new AppSettingsImplementation
        {
            MinimumLogLevel = LogLevel.Debug,
            DestinationFolder = "UpdatedDestinationFolder",
            RequestSuffix = "UpdatedRequestSuffix",
            ResponseSuffix = "UpdatedResponseSuffix"
        };


        _fakeDialogService = A.Fake<IDialogService>();
        _fakeValidator = A.Fake<IValidator<IAppSettings>>();
        _fakeConfigurationSaver = A.Fake<IConfigurationSaver>();

        _settingsViewModel = new SettingsViewModel(
            _appSettings,
            A.Fake<ILogger<SettingsViewModel>>(),
            _fakeValidator,
            _fakeConfigurationSaver,
            _fakeDialogService
        );
    }

    [Test]
    public void ExecuteCancelCommand_MustCloseViewModel()
    {
        bool isClosed = false;
        _settingsViewModel.RequestClose += () => isClosed = true;
        _settingsViewModel.CancelCommand.Execute(null);
        Assert.That(isClosed, Is.True);
    }

    [Test]
    public void ExecuteCancelCommand_MustResetProperties()
    {
        var minimumLogLevel = _appSettings.MinimumLogLevel;
        var destinationFolder = _appSettings.DestinationFolder;
        var requestSuffix = _appSettings.RequestSuffix;
        var responseSuffix = _appSettings.ResponseSuffix;

        _settingsViewModel.MinimumLogLevel = _updatedAppSettings.MinimumLogLevel;
        _updatedAppSettings.DestinationFolder = _updatedAppSettings.DestinationFolder;
        _settingsViewModel.RequestSuffix = _updatedAppSettings.RequestSuffix;
        _settingsViewModel.ResponseSuffix = _updatedAppSettings.ResponseSuffix;

        _settingsViewModel.CancelCommand.Execute(null);
        
        Assert.That(_settingsViewModel.MinimumLogLevel, Is.EqualTo(minimumLogLevel));
        Assert.That(_settingsViewModel.DestinationFolder, Is.EqualTo(destinationFolder));
        Assert.That(_settingsViewModel.RequestSuffix, Is.EqualTo(requestSuffix));
        Assert.That(_settingsViewModel.ResponseSuffix, Is.EqualTo(responseSuffix));
    }

    [Test]
    public void ExecuteSaveCommand_MustValidateSettings()
    {
        
        _settingsViewModel.SaveCommand.Execute(null);

        A.CallTo(() => _fakeValidator.Validate(A<IAppSettings>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ExecuteSaveCommand_ValidSettings_MustUpdateAppSettingsProperties()
    {

        A.CallTo(() => _fakeValidator.Validate(A<IAppSettings>.Ignored))
            .Returns(new ValidationResult());

        _settingsViewModel.MinimumLogLevel = _updatedAppSettings.MinimumLogLevel;
        _settingsViewModel.DestinationFolder = _updatedAppSettings.DestinationFolder;
        _settingsViewModel.RequestSuffix = _updatedAppSettings.RequestSuffix;
        _settingsViewModel.ResponseSuffix = _updatedAppSettings.ResponseSuffix;


        Assert.That(_appSettings.MinimumLogLevel, Is.Not.EqualTo(_updatedAppSettings.MinimumLogLevel));
        Assert.That(_appSettings.DestinationFolder, Is.Not.EqualTo(_updatedAppSettings.DestinationFolder));
        Assert.That(_appSettings.RequestSuffix, Is.Not.EqualTo(_updatedAppSettings.RequestSuffix));
        Assert.That(_appSettings.ResponseSuffix, Is.Not.EqualTo(_updatedAppSettings.ResponseSuffix));

        _settingsViewModel.SaveCommand.Execute(null);

        Assert.That(_appSettings.MinimumLogLevel, Is.EqualTo(_updatedAppSettings.MinimumLogLevel));
        Assert.That(_appSettings.DestinationFolder, Is.EqualTo(_updatedAppSettings.DestinationFolder));
        Assert.That(_appSettings.RequestSuffix, Is.EqualTo(_updatedAppSettings.RequestSuffix));
        Assert.That(_appSettings.ResponseSuffix, Is.EqualTo(_updatedAppSettings.ResponseSuffix));
    }

    [Test]
    public void ExecuteSaveCommand_ValidSettings_SavesSettings()
    {
        A.CallTo(() => _fakeValidator.Validate(A<IAppSettings>.Ignored))
            .Returns(new ValidationResult());

        _settingsViewModel.SaveCommand.Execute(null);

        A.CallTo(() => _fakeConfigurationSaver.Save(_appSettings)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ExecuteSaveCommand_InvalidSettings_MustNotSaveSettings()
    {
        A.CallTo(() => _fakeValidator.Validate(A<IAppSettings>.Ignored))
            .Returns(new ValidationResult([new ValidationFailure()]));

        _settingsViewModel.SaveCommand.Execute(null);

        A.CallTo(() => _fakeConfigurationSaver.Save(_appSettings)).MustNotHaveHappened();
    }

    [Test]
    public void ExecuteSaveCommand_ConfigurationSaverThrows_ExceptionDisplayedToUser()
    {
        A.CallTo(() => _fakeValidator.Validate(A<IAppSettings>.Ignored))
            .Returns(new ValidationResult());

        A.CallTo(() => _fakeConfigurationSaver.Save(A<IAppSettings>._))
            .Throws(new Exception("Test exception"));

        _settingsViewModel.SaveCommand.Execute(null);

        A.CallTo(() => _fakeDialogService.ShowDialog(A<string>._, A<string>._))
            .MustHaveHappened();
    }

}

internal class AppSettingsImplementation : IAppSettings
{
    public LogLevel MinimumLogLevel { get; set; }
    public required string DestinationFolder { get; set; }
    public required string RequestSuffix { get; set; }
    public required string ResponseSuffix { get; set; }
    public bool OpenGeneratedFiles { get;set; }
}

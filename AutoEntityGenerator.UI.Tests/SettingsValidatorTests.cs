using AutoEntityGenerator.UI.Validators;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.UI.Tests;

class SettingsValidatorTests
{
    SettingsValidator _validator;
    AppSettingsImplementation _defaultSettings;
    [SetUp]
    public void Setup()
    {
        _validator = new SettingsValidator();
        _defaultSettings = new AppSettingsImplementation
        {
            MinimumLogLevel = LogLevel.Information,
            DestinationFolder = "Valid/Path",
            RequestSuffix = "Request",
            ResponseSuffix = "Response"
        };
    }
    [Test]
    public void Validate_ValidSettings_ReturnsValidResult()
    {
        var validationResult = _validator.Validate(_defaultSettings);
        Assert.That(validationResult.IsValid, Is.True);
    }

    [TestCase(LogLevel.Error, true)]
    [TestCase(LogLevel.Trace, true)]
    [TestCase((LogLevel)80, false)]
    public void Validate_LogLevel_ReturnsCorrectResult(LogLevel logLevel, bool isValid)
    {
        _defaultSettings.MinimumLogLevel = logLevel;

        var validationResult = _validator.Validate(_defaultSettings);

        Assert.That(validationResult.IsValid, Is.EqualTo(isValid));

    }

    [TestCase("/Generated", true)]
    [TestCase("C:/Generated", false)]
    [TestCase("/Gener|ated", false)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    public void Validate_DestinationFolder_ReturnsCorrectResult(string destinationFolder, bool isValid)
    {
        _defaultSettings.DestinationFolder = destinationFolder;
        var validationResult = _validator.Validate(_defaultSettings);

        Assert.That(validationResult.IsValid, Is.EqualTo(isValid));
    }

    [TestCase("Request", true)]
    [TestCase("_Request", true)]
    [TestCase("-Request", false)]
    [TestCase("^Request", false)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    public void Validate_RequestSuffix_ReturnsCorrectResult(string requestSuffix, bool isValid)
    {
        _defaultSettings.RequestSuffix = requestSuffix;
        var validationResult = _validator.Validate(_defaultSettings);

        Assert.That(validationResult.IsValid, Is.EqualTo(isValid));
    }

    [TestCase("Response", true)]
    [TestCase("_Response", true)]
    [TestCase("-Response", false)]
    [TestCase("^Response", false)]
    [TestCase("", true)]
    [TestCase(" ", true)]
    public void Validate_ResponseSuffix_ReturnsCorrectResult(string responseSuffix, bool isValid)
    {
        _defaultSettings.ResponseSuffix = responseSuffix;
        var validationResult = _validator.Validate(_defaultSettings);

        Assert.That(validationResult.IsValid, Is.EqualTo(isValid));
    }
}

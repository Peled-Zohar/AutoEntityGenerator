using AutoEntityGenerator.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AutoEntityGenerator
{
    internal class ConfigurationService : IConfigurationSaver
    {
        private const string FileName = "appSettings.json";
        private readonly string _basePath;
        private readonly string _fullFilePath;
        private readonly JsonSerializerOptions _jsonOptions;
        private Exception _deferredException;

        public ConfigurationService()
        {
            _basePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Zohar Peled",
                        nameof(AutoEntityGenerator));
            _fullFilePath = Path.Combine(_basePath, FileName);
            _jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
        }

        public IAppSettings Load()
        {
            const string DefaultDestinationFolder = "Generated";
            IAppSettings settings = null;
            if (File.Exists(_fullFilePath))
            {
                try
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile(FileName)
                    .Build();
                    settings = builder.Get<AppSettings>();

                    if (!IsDestinationFolderValid(settings.DestinationFolder))
                    {
                        settings.DestinationFolder = DefaultDestinationFolder;
                    }
                }
                catch (Exception ex)
                {
                    _deferredException = ex;
                }
            }

            if (settings is null)
            {
                settings = new AppSettings()
                {
                    DestinationFolder = DefaultDestinationFolder,
                    MinimumLogLevel = LogLevel.Information,
                    RequestSuffix = "Request",
                    ResponseSuffix = "Response"
                };
            }


            return settings;
        }

        // TO Consider: Moving validation from UI to common to reduce code repitition.
        private bool IsDestinationFolderValid(string destinationFolder)
        {
            return !destinationFolder.Any(c => Path.GetInvalidPathChars().Contains(c)) &&
                   (string.IsNullOrWhiteSpace(destinationFolder) || !Path.IsPathRooted(destinationFolder.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)));
        }

        public void Save(IAppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            Directory.CreateDirectory(_basePath);
            File.WriteAllText(_fullFilePath, json);
        }

        public void LogDeferredException(ILogger<ConfigurationService> logger)
        {
            if (logger is null || _deferredException is null)
            {
                return;
            }
            logger.LogError(_deferredException, "Deffered exception");
            _deferredException = null;
        }
    }
}

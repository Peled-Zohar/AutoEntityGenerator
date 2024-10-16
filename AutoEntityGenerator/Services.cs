using AutoEntityGenerator.CodeGenerator;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoEntityGenerator
{
    public class Services : IServices
    {
        private readonly ServiceCollection _services;
        private readonly ConfigurationService _configurationService;
        private readonly IAppSettings _appSettings;

        private IServiceProvider _serviceProvider;
        private int _numberOfservices;
        private static readonly Lazy<IServices> _lazy = new Lazy<IServices>(() => new Services());

        public static IServices Instance => _lazy.Value;

        private Services()
        {
            _services = new ServiceCollection();
            _configurationService = new ConfigurationService();
            _appSettings = _configurationService.Load();
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
            _numberOfservices = _services.Count;
            ConfigureGlobalExceptionHandling();
            LogServiceConfigurationDeferredError();
        }

        private void ConfigureServices()
        {
            AddLogger()
                .AddSingleton<IConfigurationSaver>(_configurationService)
                .AddSingleton(_appSettings)
                .AddSingleton<IServices, Services>()
                .AddSingleton<IEntityGenerator, EntityGenerator>()
                .AddSingleton<ICodeActionFactory, CodeActionFactory>()
                .AddCodeGenerator()
                .AddUI();
        }

        private Services AddLogger()
        {
            const string sourceName = nameof(AutoEntityGenerator);
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            _services.AddLogging(buider => buider
                .SetMinimumLevel(_appSettings.MinimumLogLevel)
                .AddEventLog(settings =>
                {
                    settings.SourceName = sourceName;
                })
            );
            return this;
        }

        private void ConfigureGlobalExceptionHandling()
        {
            var logger = GetService<ILogger<IServices>>();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                logger.LogError(args.ExceptionObject as Exception, "Unhandled domain exception");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                logger.LogError(args.Exception, "Unobserved task exception");
                foreach (var inner in args.Exception.InnerExceptions)
                {
                    logger.LogError(inner, "inner exception");
                }
                args.SetObserved();
            };

            logger.AddUIRelatedGlobalExceptionHandling();
        }

        private void LogServiceConfigurationDeferredError()
        {
            var logger = GetService<ILogger<ConfigurationService>>();
            _configurationService.LogDeferredException(logger);
        }

        public T GetService<T>()
        {
            if (_services.Count > _numberOfservices)
            {
                _serviceProvider = _services.BuildServiceProvider();
                _numberOfservices = _services.Count;
            }
            return _serviceProvider.GetService<T>();
        }

        public IServices AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _services.AddSingleton<TService, TImplementation>();
            return this;
        }

        public IServices AddSingleton<TImplementation>() where TImplementation : class
        {
            _services.AddSingleton<TImplementation>();
            return this;
        }

        public IServices AddSingleton<TImplementation>(TImplementation instance) where TImplementation : class
        {
            _services.AddSingleton(instance);
            return this;
        }

        public IServices AddTransient<TService>() where TService : class 
        {
            _services.AddTransient<TService>();
            return this;
        }
    }
}

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
        private IServiceProvider _serviceProvider;
        private int _numberOfservices;
        public Services()
        {
            _services = new ServiceCollection();
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
            ConfigureGlobalExceptionHandling();
            _numberOfservices = _services.Count;
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

        public IServices AddTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            _services.AddTransient<TService, TImplementation>();
            return this;
        }

        private void ConfigureServices()
        {
            AddLogger()
                .AddSingleton<IServices, Services>()
                .AddSingleton<IEntityGenerator, EntityGenerator>()
                .AddSingleton<ICodeActionFactory, CodeActionFactory>()
                .AddCodeGenerator()
                .AddUI();
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

        private IServices AddLogger()
        {
            const string sourceName = nameof(AutoEntityGenerator);
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            _services.AddLogging(buider => buider
                .SetMinimumLevel(LogLevel.Information)
                .AddEventLog(settings =>
                {
                    settings.SourceName = sourceName;
                })
            );
            return this;
        }

    }
}

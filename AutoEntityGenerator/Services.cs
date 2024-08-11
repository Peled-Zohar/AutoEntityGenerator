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
        private readonly IServiceProvider _serviceProvider;

        public Services()
        {
            _services = new ServiceCollection();
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
            ConfigureGlobalExceptionHandling();
        }

        public T GetService<T>() => _serviceProvider.GetService<T>();

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
                .SetMinimumLevel(LogLevel.Trace)
                .AddEventLog(settings => {
                    settings.SourceName = sourceName;
                })
            );
            return this;
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
    }
}

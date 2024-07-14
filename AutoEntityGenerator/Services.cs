using AutoEntityGenerator.CodeGenerator;
using AutoEntityGenerator.Common;
using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        }

        public T GetService<T>() => _serviceProvider.GetService<T>();

        private void ConfigureServices()
        {
            ILogger logger = new LoggerFactory().CreateLogger();
            AddSingleton(logger)
                .AddSingleton<IServices, Services>()
                .AddSingleton<IUserInteraction, UserInteraction>()
                .AddSingleton<ICodeFileGenerator, CodeFileGenerator>()
                .AddSingleton<IEntityGenerator, EntityGenerator>()
                .AddSingleton<ICodeActionFactory, CodeActionFactory>()
                .AddSingleton<EntityConfigurationFormFactory>()
                .AddTransient<IUserInteraction, UserInteraction>()
                .AddCodeGenerator();
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

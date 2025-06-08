using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Services;
using AutoEntityGenerator.UI.Validators;
using AutoEntityGenerator.UI.ViewModels;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace AutoEntityGenerator.UI.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServices AddUI(this IServices services)
        {
            services.AddSingleton<IUserInteraction, UserInteraction>();
            services.AddSingleton<IEntityConfigurationWindowFactory, EntityConfigurationWindowFactory>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<SettingsViewModel>();
            services.AddSingleton<IValidator<IAppSettings>, AppSettingslValidator>();
            return services;
        }

        [ExcludeFromCodeCoverage] // There's no logic to test here...
        public static void AddUIRelatedGlobalExceptionHandling(this ILogger<IServices> logger)
        {
            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                logger.LogError(args.Exception, "Dispatcher unhandled exception");
                args.Handled = true;
            };
        }
    }

}

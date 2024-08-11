using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using AutoEntityGenerator.UI.Services;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace AutoEntityGenerator.UI.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServices AddUI(this IServices services)
        {
            services.AddSingleton<IUserInteraction, UserInteraction>()
                .AddSingleton<IEntityConfigurationWindowFactory, EntityConfigurationWindowFactory>()
                .AddSingleton<IDialogService, DialogService>();
            return services;
        }

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

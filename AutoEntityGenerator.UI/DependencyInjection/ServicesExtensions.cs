using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.Interaction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoEntityGenerator.UI.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServices AddUI(this IServices services)
        {
            services.AddSingleton<IUserInteraction, UserInteraction>()
                .AddSingleton<IEntityConfigurationWindowFactory, EntityConfigurationWindowFactory>();
            return services;
        }

        public static void AddUIRelatedGlobalExceptionHandling(this IServices services, ILogger<IServices> logger)
        {
            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                logger.LogError(args.Exception, "Dispatcher unhandled exception");
                args.Handled = true;
            };
        }
    }

}

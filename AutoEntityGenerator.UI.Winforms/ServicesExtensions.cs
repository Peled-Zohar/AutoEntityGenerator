using AutoEntityGenerator.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace AutoEntityGenerator.UI
{
    public static class ServicesExtensions
    {
        public static IServices AddUI(this IServices services)
        {
            services.AddSingleton<IUserInteraction, UserInteraction>()
                .AddSingleton<IEntityConfigurationFormFactory, EntityConfigurationFormFactory>();
            return services;
        }

        public static void AddUIRelatedGlobalExceptionHandling(this IServices services, ILogger<IServices> logger)
        {
            Application.ThreadException += (sender, args) =>
            {
                logger.LogError(args.Exception, "Unhandled thread exception");
            };
        }
    }
}

using AutoEntityGenerator.Common.Interfaces;

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
    }
}

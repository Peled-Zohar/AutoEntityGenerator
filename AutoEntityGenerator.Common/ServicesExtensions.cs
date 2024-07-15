using AutoEntityGenerator.Common.Interfaces;

namespace AutoEntityGenerator.Common
{
    public static class ServicesExtensions
    {
        public static IServices AddLogger(this IServices services)
        {
            ILogger logger = new LoggerFactory().CreateLogger();
            services.AddSingleton(logger);
            return services;
        }
    }
}

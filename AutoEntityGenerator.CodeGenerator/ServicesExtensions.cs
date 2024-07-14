using AutoEntityGenerator.Common.Interfaces;

namespace AutoEntityGenerator.CodeGenerator
{
    public static class ServicesExtensions
    {
        public static IServices AddCodeGenerator(this IServices services)
        {
            services.AddSingleton<IEntityGenerator, EntityGenerator>();
            services.AddSingleton<IMappingsClassGenerator, MappingsClassGenerator>();
            services.AddSingleton<ICodeFileGenerator, CodeFileGenerator>();
            return services;
        }
    }
}

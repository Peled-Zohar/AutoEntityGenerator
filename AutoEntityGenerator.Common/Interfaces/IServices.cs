namespace AutoEntityGenerator.Common.Interfaces
{
    public interface IServices
    {
        IServices AddSingleton<TImplementation>() where TImplementation : class;
        IServices AddSingleton<TImplementation>(TImplementation instance) where TImplementation : class;
        IServices AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IServices AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        T GetService<T>();
    }
}

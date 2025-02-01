namespace HunterPie.DI;

public static class DependencyContainer
{
    private static readonly object Lock = new();

    private static IDependencyRegistry? _registry;

    public static void SetRegistry(IDependencyRegistry registry)
    {
        lock (Lock)
            _registry = registry;
    }

    public static T Get<T>() where T : class => (T)Get(typeof(T));

    public static object Get(Type type)
    {
        lock (Lock)
            return _registry?.Get(type) ?? throw new NullReferenceException($"{nameof(DependencyContainer)} has not been initialized yet");
    }
}
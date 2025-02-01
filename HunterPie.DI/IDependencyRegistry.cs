namespace HunterPie.DI;

public interface IDependencyRegistry
{
    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <typeparam name="T">Type of implementation</typeparam>
    /// <returns>Implementation</returns>
    public T Get<T>() where T : class;

    /// <summary>
    /// Retrieves an implementation from the dependency registry
    /// </summary>
    /// <param name="type">Type of implementation</param>
    /// <returns>Implementation</returns>
    public object Get(Type type);

    public IDependencyRegistry WithService<T>() where T : class;
    public IDependencyRegistry WithService<T>(Func<T> builder) where T : class;

    public IDependencyRegistry WithSingle<T>() where T : class;
    public IDependencyRegistry WithSingle<T>(Func<T> builder) where T : class;
}
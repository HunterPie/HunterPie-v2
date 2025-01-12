using HunterPie.DI.Exceptions;
using System.Reflection;

namespace HunterPie.DI.Registry;

public class DependencyRegistry : IDependencyRegistry
{
    private readonly Queue<Dependency> _singletonQueue = new();
    private readonly Dictionary<Type, List<object>> _singletons = new();
    private readonly Dictionary<Type, List<Dependency>> _dependencies = new();

    public T Get<T>() where T : class
    {
        return Get(typeof(T)) as T ?? throw new Exception();
    }

    public object Get(Type type)
    {
        if (_singletons.TryGetValue(type, out List<object>? value))
            return value switch
            {
                { Count: > 1 } => throw new ManyPossibleDependenciesException(type, value.Select(it => it.GetType()).ToArray()),
                _ => value.First()
            };

        if (!_dependencies.ContainsKey(type))
            throw new DependencyNotRegisteredException(type);

        List<Dependency> instances = _dependencies[type];

        if (instances.Count > 1)
            throw new ManyPossibleDependenciesException(type, instances.Select(it => it.Type).ToArray());

        return Create(instances.First());
    }

    public object[] GetAll(Type type)
    {
        Type? elementType = type.GetElementType();

        if (!type.IsArray || elementType is not { })
            throw new ArgumentException($"Type must be receiving an array but got {type.Name} instead");

        IEnumerable<object> singletons = _singletons.GetValueOrDefault(elementType)
            ?.AsEnumerable()
            ?? Array.Empty<object>();
        IEnumerable<object> services = _dependencies.GetValueOrDefault(elementType)
            ?.Select(Create)
            ?? Array.Empty<object>();

        return services.Concat(singletons)
            .ToArray();
    }

    public IDependencyRegistry WithService<T>() where T : class
    {
        RegisterService(typeof(T));

        return this;
    }

    public IDependencyRegistry WithService<T>(Func<T> builder) where T : class
    {
        Type concreteType = typeof(T);

        RegisterService(concreteType, builder);

        return this;
    }

    public IDependencyRegistry WithSingle<T>() where T : class
    {
        _singletonQueue.Enqueue(
            item: new Dependency(
                Type: typeof(T),
                Activator: null
            )
        );

        return this;
    }

    public IDependencyRegistry WithSingle<T>(Func<T> builder) where T : class
    {
        _singletonQueue.Enqueue(
            item: new Dependency(
                Type: typeof(T),
                Activator: builder
            )
        );

        return this;
    }

    public IDependencyRegistry Build()
    {
        // TODO: Change this to build a dependency tree instead of this disgusting brute force approach
        // I actually despise this approach
        while (true)
        {
            int count = _singletonQueue.Count;

            if (count <= 0)
                return this;

            var failedDependencies = new Queue<Dependency>(count);
            var exceptions = new List<Exception>();
            while (_singletonQueue.TryDequeue(out Dependency? dependency))
                try
                {
                    RegisterSingleton(dependency);
                }
                catch (DependencyNotRegisteredException err)
                {
                    failedDependencies.Enqueue(dependency);
                    exceptions.Add(err);
                }
                catch (DependencyArgumentException err)
                {
                    failedDependencies.Enqueue(dependency);
                    exceptions.Add(err);
                }
                catch (Exception err)
                {
                    exceptions.Add(err);
                }

            if (failedDependencies.Count >= count)
                throw new SingletonsCreationException(failedDependencies.Select(it => it.Type), exceptions.ToArray());

            while (failedDependencies.TryDequeue(out Dependency? dependency))
                _singletonQueue.Enqueue(dependency);
        }
    }

    private object Create(Dependency concrete)
    {
        if (concrete.Activator is { } activator)
            return activator();

        Type type = concrete.Type;
        ConstructorInfo? constructor = type.GetConstructors()
            .MinBy(it => it.GetParameters().Length);

        if (constructor is not { })
            throw new ArgumentException($"Type {type.Name} contains no accessible constructors");

        object[] args = constructor.GetParameters()
            .Select(param =>
            {
                try
                {
                    return param.ParameterType.IsArray
                        ? GetAll(param.ParameterType)
                        : Get(param.ParameterType);
                }
                catch (Exception err)
                {
                    throw new DependencyArgumentException(type, err);
                }
            })
            .ToArray();

        object instance = constructor.Invoke(args);

        return instance;
    }

    private void RegisterSingleton(Dependency dependency)
    {
        Type[] interfaces = dependency.Type.GetInterfaces();
        object instance = Create(dependency);

        foreach (Type interfaceType in interfaces)
        {
            if (!_singletons.ContainsKey(interfaceType))
                _singletons[interfaceType] = new List<object>();

            _singletons[interfaceType].Add(instance);
        }

        if (!_singletons.ContainsKey(dependency.Type))
            _singletons[dependency.Type] = new List<object>();

        _singletons[dependency.Type].Add(instance);
    }

    private void RegisterService(Type concreteType, Func<object>? activator = null)
    {
        Type[] interfaces = concreteType.GetInterfaces();

        foreach (Type interfaceType in interfaces)
            RegisterServiceWithAbstraction(interfaceType, concreteType, activator);

        RegisterServiceWithAbstraction(concreteType, concreteType, activator);
    }

    private void RegisterServiceWithAbstraction(Type abstraction, Type concrete, Func<object>? activator)
    {
        if (!_dependencies.ContainsKey(abstraction))
            _dependencies[abstraction] = new List<Dependency>();

        _dependencies[abstraction].Add(
            item: new Dependency(
                Type: concrete,
                Activator: activator
            )
        );
    }
}
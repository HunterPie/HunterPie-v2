using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Scan.Service;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain;

public delegate Task AsyncMiddleware<in T>(T dto) where T : class;

public delegate Task AsyncDelegate();


/// <summary>
/// Implementation for a scannable entity, handles the scan and middlewares internally
/// </summary>
public abstract class Scannable : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    protected readonly IGameProcess Process;
    protected readonly IScanService ScanService;
    protected IMemoryAsync Memory => Process.Memory;
    private readonly Dictionary<Type, HashSet<Delegate>> _middlewares = new();
    private readonly List<AsyncDelegate> _scanners = new();
    private readonly Dictionary<AsyncDelegate, int> _troublesomeScannables = new();

    protected Scannable(
        IGameProcess process,
        IScanService scanService)
    {
        Process = process;
        ScanService = scanService;

        AppendScannableMethods();

        ScanService.Add(this);
    }

    private void AppendScannableMethods()
    {
        Type self = GetType();
        MethodInfo[] methods = self.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        foreach (MethodInfo method in methods)
        {

            ScannableMethod? attribute = method.GetCustomAttribute<ScannableMethod>(inherit: true);

            if (attribute is not { })
                continue;

            var func = (AsyncDelegate)Delegate.CreateDelegate(typeof(AsyncDelegate), this, method.Name);

            _ = Add(attribute.DtoType, func);
        }
    }

    /// <summary>
    /// Calls each scanning function added to the scanners list
    /// </summary>
    internal async Task ScanAsync()
    {
        foreach (AsyncDelegate scanner in _scanners)
            try
            {
                await scanner.Invoke();
            }
            catch (Exception err)
            {
                _troublesomeScannables.TryAdd(scanner, 0);
                _troublesomeScannables[scanner]++;

                if (_troublesomeScannables[scanner] >= 3)
                {
                    _logger.Warning($"Scanner: {scanner.Method.Name} had multiple exceptions. Disabling scanner for now;\n{err}");

                    _scanners.Remove(scanner);

                    _troublesomeScannables.Remove(scanner);
                }
            }
    }

    /// <summary>
    /// Calls the middlewares
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="data">DTO to pass to the middlewares</param>
    protected async Task Next<T>(T data) where T : class
    {
        foreach (Delegate middleware in _middlewares[data.GetType()])
        {
            if (middleware is not AsyncMiddleware<T> func)
                continue;

            await func(data);
        }
    }

    /// <summary>
    /// Adds a middleware for the type <typeparamref name="T"/> that will be called
    /// when a scanning function with that type runs.
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="middleware">Function to be called</param>
    /// <returns>true if the middleware was added successfully, false otherwise</returns>
    public bool MiddlewareFor<T>(AsyncMiddleware<T> middleware) where T : class
    {
        Type type = typeof(T);
        if (!_middlewares.TryGetValue(type, out HashSet<Delegate>? value))
            return false;

        value.Add(middleware);

        return true;
    }

    /// <summary>
    /// Removes a middleware of the type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of the DTO</typeparam>
    /// <param name="middleware">Function to remove</param>
    /// <returns>true if the middleware was removed successfully, false otherwise</returns>
    public bool RemoveMiddleware<T>(AsyncMiddleware<T> middleware) where T : class
    {
        Type type = typeof(T);
        if (!_middlewares.TryGetValue(type, out HashSet<Delegate>? value))
            return false;

        _ = value.Remove(middleware);

        return true;
    }

    private bool Add(Type? type, AsyncDelegate scanner)
    {
        _scanners.Add(scanner);

        if (type is null)
            return true;

        if (!_middlewares.ContainsKey(type))
            _middlewares.Add(type, new HashSet<Delegate>());

        return true;
    }

    public virtual void Dispose()
    {
        ScanService.Remove(this);
    }
}
using HunterPie.Core.Observability.Logging;
using HunterPie.DI.Module;
using HunterPie.DI.Registry;
using System;
using System.Linq;

namespace HunterPie.DI;

internal static class DependencyProvider
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly Lazy<IDependencyModule[]> Modules = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IDependencyModule).IsAssignableFrom(types) && !types.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IDependencyModule>()
            .ToArray();
    });

    public static void LoadModules()
    {
        DependencyRegistry registry = DependencyRegistryBuilder.Create();

        foreach (IDependencyModule module in Modules.Value)
        {
            module.Register(registry);
            Logger.Debug($"Loaded module {module.GetType().Name}");
        }

        DependencyContainer.SetRegistry(registry.Build());
    }
}
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal static class WidgetInitializers
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly Lazy<IWidgetInitializer[]> Initializers = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IWidgetInitializer).IsAssignableFrom(types) && !types.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IWidgetInitializer>()
            .ToArray();
    });

    public static async Task InitializeAsync(Context context)
    {
        IEnumerable<Task> tasks = Initializers.Value.Select(it => it.LoadAsync(context));

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception err)
        {
            Logger.Error(err.ToString());
        }
    }

    public static void Unload()
    {
        foreach (IWidgetInitializer initializer in Initializers.Value)
            initializer.Unload();
    }
}
using System;
using System.Linq;

namespace HunterPie.Features.Debug;

internal static class DebugWidgets
{
    private static readonly Lazy<IWidgetMocker[]> Mockers = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IWidgetMocker).IsAssignableFrom(types) && !types.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IWidgetMocker>()
            .ToArray();
    });

    public static void MockIfNeeded()
    {
        foreach (IWidgetMocker mocker in Mockers.Value)
            mocker.Mock();
    }
}
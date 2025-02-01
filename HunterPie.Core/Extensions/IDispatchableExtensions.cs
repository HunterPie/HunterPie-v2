using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Observability.Logging;
using System;

namespace HunterPie.Core.Extensions;

public static class IDispatchableExtensions
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    public static void Dispatch<T>(this IEventDispatcher self, SmartEvent<T> toDispatch, T data)
    {
        toDispatch.Invoke(self, data);
    }

    public static void Dispatch(this IEventDispatcher self, SmartEvent<EventArgs> toDispatch)
    {
        toDispatch.Invoke(self, EventArgs.Empty);
    }

    public static void Dispatch<T>(this IEventDispatcher self, EventHandler<T>? toDispatch, T data)
    {
        if (toDispatch is null)
            return;

        foreach (EventHandler<T> sub in toDispatch.GetInvocationList())
            try
            {
                sub(self, data);
            }
            catch (Exception err)
            {
                Logger.Error($"Exception in {sub.Method.Name}: {err}");
            }
    }

    public static void Dispatch(this IEventDispatcher self, EventHandler<EventArgs>? toDispatch)
    {
        if (toDispatch is null)
            return;

        foreach (EventHandler<EventArgs> sub in toDispatch.GetInvocationList())
            try
            {
                sub(self, EventArgs.Empty);
            }
            catch (Exception err)
            {
                Logger.Error($"Exception in {sub.Method.Name}: {err}");
            }
    }
}
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Logger;
using System;

namespace HunterPie.Core.Extensions;

public static class IDispatchableExtensions
{

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
                Log.Error("Exception in {0}: {1}", sub.Method.Name, err);
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
                Log.Error("Exception in {0}: {1}", sub.Method.Name, err);
            }
    }
}
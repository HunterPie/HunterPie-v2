using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace HunterPie.Core.Architecture.Events;

/// <summary>
/// SmartEvent is an wrapper for Events that works in a better way, detecting subscriber leaks and handling errors on invoke
/// without crashing the whole application.
/// </summary>
/// <typeparam name="TSource">Type of class that's going to dispatch the event</typeparam>
/// <typeparam name="TEventArgs">Type of event arg</typeparam>
public class SmartEvent<TSource, TEventArgs> : ISmartEvent
    where TSource : class
    // TODO: Make TEventArgs : EventArgs 
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly object _sync = new();
    private EventHandler<TEventArgs>? _event;

    public string Name { get; }

    public List<MethodInfo> References { get; } = new();

    public SmartEvent()
    {
        MethodBase? method = new StackTrace().GetFrame(2)?.GetMethod();
        Name = method?.ReflectedType?.Name ?? "Unknown";

        SmartEventsTracker.Track(this);
    }

    public void Hook(EventHandler<TEventArgs> handler)
    {
        lock (_sync)
        {
            _event += handler;
            References.Add(handler.Method);
        }
    }

    public void Unhook(EventHandler<TEventArgs> handler)
    {
        lock (_sync)
        {
            _event -= handler;
            References.Remove(handler.Method);
        }
    }

    public void Invoke(object? sender, TEventArgs e)
    {
        lock (_sync)
        {
            if (_event is null)
                return;

            foreach (EventHandler<TEventArgs> sub in _event.GetInvocationList())
                try
                {
                    sub(sender, e);
                }
                catch (Exception err)
                {
                    _logger.Error($"Exception in {sub.Method.Name}: {err}");
                }
        }
    }

    private void UnhookAll()
    {
        lock (_sync)
        {
            if (_event is null)
                return;

            foreach (EventHandler<TEventArgs> sub in _event.GetInvocationList())
            {
                _event -= sub;
                References.Remove(sub.Method);
            }
        }
    }

    public void Dispose()
    {
        UnhookAll();
        SmartEventsTracker.Untrack(this);
    }
}

public class SmartEvent<TEventArgs> : SmartEvent<object, TEventArgs> { }
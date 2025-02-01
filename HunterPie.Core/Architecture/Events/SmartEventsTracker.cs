using HunterPie.Core.Observability.Logging;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Architecture.Events;
#nullable enable
public class SmartEventsTracker
{
    private static readonly ILogger Logger = LoggerFactory.Create();
    private static SmartEventsTracker? _instance;
    public static SmartEventsTracker Instance => _instance ??= new SmartEventsTracker();

    public readonly HashSet<ISmartEvent> TrackedEvents = new();

    public static void Track(ISmartEvent smartEvent)
    {
        lock (Instance.TrackedEvents)
            Instance.TrackedEvents.Add(smartEvent);
    }

    public static void Untrack(ISmartEvent smartEvent)
    {
        lock (Instance.TrackedEvents)
            Instance.TrackedEvents.Remove(smartEvent);
    }

    public static int CountReferences() => Instance.TrackedEvents.Sum(se => se.References.Count);

    public static int ActiveEvents() => Instance.TrackedEvents.Count;

    public static void DisposeEvents()
    {
        foreach (ISmartEvent @event in Instance.TrackedEvents.ToArray())
        {
            if (@event.References.Any())
                @event.References.ToList().ForEach(reference =>
                        Logger.Warning(
                        $"Detected dangling event reference at {reference.DeclaringType?.FullName ?? "unknown"}::{reference.Name}"
                        )
                    );

            @event.Dispose();
        }

        lock (Instance.TrackedEvents)
            Instance.TrackedEvents.Clear();
    }
}
using System;
using System.Collections.Generic;
using System.Threading;

namespace HunterPie.UI.Architecture.Test;

public static class MockBehavior
{
    private static readonly List<Timer> timers = new();
    private static readonly Dictionary<Action, bool> runnables = new();
    public static void Run(Action runnable, float seconds = 1)
    {
        runnables[runnable] = true;
        // Yes, this is a memory leak, but will only be used for mocking purposes anyways
        var timer = new Timer((_) =>
        {
            bool completed = runnables[runnable];

            if (!completed)
                return;

            runnables[runnable] = false;
            runnable();
            runnables[runnable] = true;
        }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(seconds));
        timers.Add(timer);
    }
}
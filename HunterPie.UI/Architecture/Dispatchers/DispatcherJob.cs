using System;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture.Dispatchers;

#nullable enable
/// <summary>
/// A job that is executed in the UI thread only once after some interval
/// </summary>
public class DispatcherJob
{
    public DispatcherJob(Action callback, TimeSpan? waitTime = null)
    {
        if (waitTime is null)
            waitTime = TimeSpan.Zero;

        var timer = new DispatcherTimer(DispatcherPriority.Normal)
        {
            Interval = waitTime.Value
        };

        timer.Tick += (sender, args) =>
        {
            callback();

            if (sender is DispatcherTimer t)
                t.Stop();
        };

        timer.Start();
    }
}
#nullable restore
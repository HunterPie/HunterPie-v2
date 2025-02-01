using System;

namespace HunterPie.Core.Game.Events;

public class TimeElapsedChangeEventArgs : EventArgs
{
    public TimeElapsedChangeEventArgs(bool isTimerReset, float timeElapsed)
    {
        IsTimerReset = isTimerReset;
        TimeElapsed = timeElapsed;
    }

    /// <summary>
    /// Whether the timer has been reset (and switched) into another zero or some non-zero value.
    /// This indicates the time elapsed may experience sudden change.
    /// </summary>
    public bool IsTimerReset { get; }

    /// <summary>
    /// Time elapsed in-game
    /// </summary>
    public float TimeElapsed { get; }
}
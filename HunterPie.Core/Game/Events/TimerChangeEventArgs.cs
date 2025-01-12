using System;

namespace HunterPie.Core.Game.Events;

public class TimerChangeEventArgs : EventArgs
{
    public float Current { get; }
    public float Max { get; }

    public TimerChangeEventArgs(float current, float max)
    {
        Current = current;
        Max = max;
    }
}
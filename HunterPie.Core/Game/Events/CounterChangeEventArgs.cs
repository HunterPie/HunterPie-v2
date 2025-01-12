using System;

namespace HunterPie.Core.Game.Events;

public class CounterChangeEventArgs : EventArgs
{
    public int Current { get; }
    public int Max { get; }

    public CounterChangeEventArgs(int current, int max)
    {
        Current = current;
        Max = max;
    }
}
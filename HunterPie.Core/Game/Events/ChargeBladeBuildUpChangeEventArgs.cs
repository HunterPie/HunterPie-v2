using System;

namespace HunterPie.Core.Game.Events;

public class ChargeBladeBuildUpChangeEventArgs : EventArgs
{
    public float Current { get; }
    public float Max { get; }

    public ChargeBladeBuildUpChangeEventArgs(float current, float max)
    {
        Current = current;
        Max = max;
    }
}
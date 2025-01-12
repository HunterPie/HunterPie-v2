using System;

namespace HunterPie.Core.Game.Events;

public class BuildUpChangeEventArgs : EventArgs
{
    public float Current { get; }
    public float Max { get; }

    public BuildUpChangeEventArgs(float current, float max)
    {
        Current = current;
        Max = max;
    }
}
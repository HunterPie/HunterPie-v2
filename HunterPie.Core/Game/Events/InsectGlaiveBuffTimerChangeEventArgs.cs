using System;

namespace HunterPie.Core.Game.Events;

public class InsectGlaiveBuffTimerChangeEventArgs : EventArgs
{

    public float Timer { get; }

    public InsectGlaiveBuffTimerChangeEventArgs(float timer)
    {
        Timer = timer;
    }
}
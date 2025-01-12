using System;

namespace HunterPie.Core.Game.Events;

public class ChargeBladeBuffTimerChangeEventArgs : EventArgs
{
    public float Timer { get; }

    public ChargeBladeBuffTimerChangeEventArgs(float timer)
    {
        Timer = timer;
    }
}
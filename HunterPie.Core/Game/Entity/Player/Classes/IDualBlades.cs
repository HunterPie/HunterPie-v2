using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface IDualBlades
{
    public bool IsDemonMode { get; }
    public bool IsArchDemonMode { get; }
    public float DemonBuildUp { get; }
    public float MaxDemonBuildUp { get; }
    public float PiercingBindTimer { get; }
    public float MaxPiercingBindTimer { get; }

    public event EventHandler<StateChangeEventArgs<bool>> OnDemonModeStateChange;
    public event EventHandler<StateChangeEventArgs<bool>> OnArchDemonModeStateChange;
    public event EventHandler<BuildUpChangeEventArgs> OnDemonBuildUpChange;
    public event EventHandler<TimerChangeEventArgs> OnPiercingBindTimerChange;
}
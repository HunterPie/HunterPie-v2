using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface ILongSword : IMeleeWeapon
{
    public int SpiritLevel { get; }
    public float SpiritBuildUp { get; }
    public float MaxSpiritBuildUp { get; }
    public float SpiritRegenerationTimer { get; }
    public float MaxSpiritRegenerationTimer { get; }
    public float SpiritLevelTimer { get; }
    public float MaxSpiritLevelTimer { get; }

    public event EventHandler<SimpleValueChangeEventArgs<int>> OnSpiritLevelChange;
    public event EventHandler<BuildUpChangeEventArgs> OnSpiritBuildUpChange;
    public event EventHandler<TimerChangeEventArgs> OnSpiritRegenerationChange;
    public event EventHandler<TimerChangeEventArgs> OnSpiritLevelTimerChange;
}
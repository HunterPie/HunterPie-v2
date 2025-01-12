using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Entity.Player;

public interface IAbnormality
{
    public string Id { get; }
    public string Name { get; }
    public string Icon { get; }
    public AbnormalityType Type { get; }
    public float Timer { get; }
    public float MaxTimer { get; }
    public bool IsInfinite { get; }
    public int Level { get; }
    public bool IsBuildup { get; }

    public event EventHandler<IAbnormality> OnTimerUpdate;
}
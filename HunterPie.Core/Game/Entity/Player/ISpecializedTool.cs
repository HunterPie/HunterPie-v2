using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Entity;

public interface ISpecializedTool
{
    SpecializedToolType Id { get; }
    float Cooldown { get; }
    float MaxCooldown { get; }
    float Timer { get; }
    float MaxTimer { get; }

    event EventHandler<ISpecializedTool> OnChange;
    event EventHandler<ISpecializedTool> OnTimerUpdate;
    event EventHandler<ISpecializedTool> OnCooldownUpdate;

}
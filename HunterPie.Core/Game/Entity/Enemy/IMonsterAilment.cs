using HunterPie.Core.Game.Data.Definitions;
using System;

namespace HunterPie.Core.Game.Entity.Enemy;

public interface IMonsterAilment
{
    public AilmentDefinition Definition { get; }
    public string Id { get; }
    public int Counter { get; }
    public float Timer { get; }
    public float MaxTimer { get; }
    public float BuildUp { get; }
    public float MaxBuildUp { get; }

    public event EventHandler<IMonsterAilment> OnTimerUpdate;
    public event EventHandler<IMonsterAilment> OnBuildUpUpdate;
    public event EventHandler<IMonsterAilment> OnCounterUpdate;
}
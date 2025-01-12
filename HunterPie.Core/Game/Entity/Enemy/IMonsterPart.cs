using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Entity.Enemy;

public interface IMonsterPart
{
    public MonsterPartDefinition Definition { get; }
    public string Id { get; }
    public float Health { get; }
    public float MaxHealth { get; }
    public float Flinch { get; }
    public float MaxFlinch { get; }
    public float Sever { get; }
    public float MaxSever { get; }
    public float Tenderize { get; }
    public float MaxTenderize { get; }
    public int Count { get; }

    public PartType Type { get; }

    public event EventHandler<IMonsterPart> OnHealthUpdate;
    public event EventHandler<IMonsterPart> OnTenderizeUpdate;
    public event EventHandler<IMonsterPart> OnFlinchUpdate;
    public event EventHandler<IMonsterPart> OnSeverUpdate;
    public event EventHandler<IMonsterPart> OnBreakCountUpdate;
    public event EventHandler<IMonsterPart> OnPartTypeChange;
}
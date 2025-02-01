using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;

public sealed class MHWMonsterPart :
    CommonPart,
    IUpdatable<MHWMonsterPartStructure>,
    IUpdatable<MHWTenderizeInfoStructure>
{
    private float _flinch;
    private float _sever;
    private float _tenderize;
    private int _count;
    private readonly HashSet<uint> _tenderizeIds;

    public override string Id { get; protected set; }

    public override float Health
    {
        get => 0;
        protected set => throw new NotSupportedException();
    }

    public override float MaxHealth
    {
        get => 0;
        protected set => throw new NotSupportedException();
    }

    public override float Flinch
    {
        get => _flinch;
        protected set
        {
            if (value != _flinch)
            {
                _flinch = value;
                this.Dispatch(_onFlinchUpdate, this);
            }
        }
    }

    public override float MaxFlinch { get; protected set; }

    public override float Sever
    {
        get => _sever;
        protected set
        {
            if (value != _sever)
            {
                _sever = value;
                this.Dispatch(_onSeverUpdate, this);
            }
        }
    }

    public override float MaxSever { get; protected set; }

    public override float Tenderize
    {
        get => _tenderize;
        protected set
        {
            if (value != _tenderize)
            {
                _tenderize = value;
                this.Dispatch(_onTenderizeUpdate, this);
            }
        }
    }

    public override float MaxTenderize { get; protected set; }

    public override int Count
    {
        get => _count;
        protected set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onBreakCountUpdate, this);
            }
        }
    }
    public override PartType Type { get; protected set; }

    public MHWMonsterPart(
        MonsterPartDefinition definition
    ) : base(definition)
    {
        Id = definition.String;

        Type = definition.IsSeverable ? PartType.Severable : PartType.Flinch;
        _tenderizeIds = definition.TenderizeIds.ToHashSet();
    }

    public bool HasTenderizeId(uint id) => _tenderizeIds.Contains(id);

    public void Update(MHWMonsterPartStructure data)
    {
        switch (Type)
        {
            case PartType.Severable:
                {
                    MaxSever = data.MaxHealth;
                    Sever = data.Health;
                }

                break;
            case PartType.Flinch:
                {
                    MaxFlinch = data.MaxHealth;
                    Flinch = data.Health;
                }

                break;
            case PartType.Invalid:
            case PartType.Breakable:
            case PartType.Qurio:
            default:
                break;
        }

        Count = data.Counter;
    }

    public void Update(MHWTenderizeInfoStructure data)
    {
        Tenderize = data.Duration;
        MaxTenderize = data.MaxDuration;
    }
}
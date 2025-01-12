using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

public sealed class MHRMonsterPart : CommonPart, IUpdatable<MHRPartStructure>, IUpdatable<MHRQurioPartData>
{
    private float _qurioHealth;
    private float _health;
    private float _flinch;
    private float _sever;
    private PartType _type;
    private bool _isInQurio;

    public override string Id { get; protected set; }

    public override float Health
    {
        get => _health;
        protected set
        {
            if (value != _health)
            {
                _health = value;
                this.Dispatch(_onHealthUpdate, this);
            }
        }
    }

    public override float MaxHealth { get; protected set; }

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

    public override float Tenderize { get; protected set; }
    public override float MaxTenderize { get; protected set; }

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

    public float QurioHealth
    {
        get => _qurioHealth;
        private set
        {
            if (value != _qurioHealth)
            {
                _qurioHealth = value;
                this.Dispatch(_onQurioHealthChange, this);
            }
        }
    }

    public float QurioMaxHealth { get; private set; }

    public override PartType Type
    {
        get => _type;
        protected set
        {
            if (value != _type)
            {
                _type = value;
                this.Dispatch(_onPartTypeChange, this);
            }
        }
    }

    public override int Count { get; protected set; }

    private readonly SmartEvent<IMonsterPart> _onQurioHealthChange = new();
    public event EventHandler<IMonsterPart> OnQurioHealthChange
    {
        add => _onQurioHealthChange.Hook(value);
        remove => _onQurioHealthChange.Unhook(value);
    }

    public MHRMonsterPart(MonsterPartDefinition definition) : base(definition)
    {
        Id = definition.String;
    }

    public MHRMonsterPart(MonsterPartDefinition definition, MHRPartStructure structure) : base(definition)
    {
        Id = definition.String;

        GetCurrentType(structure);
    }

    public void Update(MHRPartStructure data)
    {
        if (Type == PartType.Qurio && !_isInQurio)
            GetCurrentType(data);

        MaxHealth = data.MaxHealth;
        Health = data.Health;
        MaxFlinch = data.MaxFlinch;
        Flinch = data.Flinch;
        MaxSever = data.MaxSever;
        Sever = data.Sever;
    }

    public void Update(MHRQurioPartData data)
    {
        switch (data.IsInQurioState)
        {
            case false when Type != PartType.Qurio:
                return;
            case false when Type == PartType.Qurio:
                _isInQurio = false;
                return;
        }

        Type = PartType.Qurio;
        QurioMaxHealth = Math.Max(data.MaxHealth, Math.Max(data.Health, QurioMaxHealth));
        QurioHealth = data.Health;
        _isInQurio = data.IsInQurioState;
    }

    private void GetCurrentType(MHRPartStructure structure)
    {
        if (structure.MaxSever > 0)
            Type = PartType.Severable;
        else if (structure.MaxHealth > 0)
            Type = PartType.Breakable;
        else if (structure.MaxFlinch > 0)
            Type = PartType.Flinch;
    }

    public override void Dispose()
    {
        _onQurioHealthChange.Dispose();
        base.Dispose();
    }
}
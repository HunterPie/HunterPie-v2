using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;

public sealed class MHWMonsterAilment : CommonAilment, IUpdatable<MHWMonsterStatusStructure>, IUpdatable<MHWMonsterAilmentStructure>
{
    private int _counter;
    private float _timer;
    private float _buildup;

    public override string Id { get; protected set; }

    public override int Counter
    {
        get => _counter;
        protected set
        {
            if (value != _counter)
            {
                _counter = value;
                this.Dispatch(_onCounterUpdate, this);
            }
        }
    }

    public override float Timer
    {
        get => _timer;
        protected set
        {
            if (value != _timer)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public override float MaxTimer { get; protected set; }
    public override float BuildUp
    {
        get => _buildup;
        protected set
        {
            if (value != _buildup)
            {
                _buildup = value;
                this.Dispatch(_onBuildUpUpdate, this);
            }
        }
    }

    public override float MaxBuildUp { get; protected set; }

    public MHWMonsterAilment(AilmentDefinition definition) : base(definition)
    {
        Id = definition.String;
    }

    public void Update(MHWMonsterStatusStructure data)
    {
        MaxTimer = data.MaxDuration;
        Timer = data.Duration > 0
            ? data.MaxDuration - data.Duration
            : 0;
        MaxBuildUp = data.MaxBuildup;
        BuildUp = data.Buildup;
        Counter = data.Counter;
    }

    public void Update(MHWMonsterAilmentStructure data)
    {
        MaxTimer = data.MaxDuration;
        Timer = data.Duration;
        MaxBuildUp = data.MaxBuildup;
        BuildUp = data.Buildup;
        Counter = data.Counter;
    }
}
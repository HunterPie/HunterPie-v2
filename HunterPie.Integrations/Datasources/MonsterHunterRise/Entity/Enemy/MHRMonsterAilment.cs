using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

public sealed class MHRMonsterAilment : CommonAilment, IUpdatable<MHRAilmentData>, IUpdatable<MHREnrageStructure>
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

    public MHRMonsterAilment(AilmentDefinition definition) : base(definition)
    {
        Id = definition.String;
    }

    public void Update(MHRAilmentData data)
    {
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxBuildUp = data.MaxBuildUp;
        BuildUp = data.BuildUp;
        Counter = data.Counter;
    }

    public void Update(MHREnrageStructure data)
    {
        MaxTimer = data.MaxTimer;
        Timer = data.Timer;
        MaxBuildUp = data.MaxBuildUp;
        BuildUp = data.BuildUp;
        Counter = data.Counter;
    }
}
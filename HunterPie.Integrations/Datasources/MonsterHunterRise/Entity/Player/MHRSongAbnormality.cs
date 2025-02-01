using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public sealed class MHRSongAbnormality : CommonAbnormality, IUpdatable<MHRHHAbnormality>
{

    private float _timer;

    public override string Id { get; protected set; }
    public override string Name { get; protected set; }
    public override string Icon { get; protected set; }

    public override AbnormalityType Type
    {
        get => AbnormalityType.Song;
        protected set => throw new NotSupportedException();
    }
    public override float Timer
    {
        get => _timer;
        protected set
        {
            if (_timer != value)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }
    public override float MaxTimer { get; protected set; }
    public override bool IsInfinite { get; protected set; }
    public override int Level { get; protected set; }
    public override bool IsBuildup { get; protected set; }

    public MHRSongAbnormality(AbnormalityDefinition schema)
    {
        Id = schema.Id;
        Name = schema.Name;
        Icon = schema.Icon;
        IsBuildup = schema.IsBuildup;
    }

    void IUpdatable<MHRHHAbnormality>.Update(MHRHHAbnormality data)
    {
        MaxTimer = Math.Max(MaxTimer, data.Timer);
        Timer = data.Timer;
    }
}
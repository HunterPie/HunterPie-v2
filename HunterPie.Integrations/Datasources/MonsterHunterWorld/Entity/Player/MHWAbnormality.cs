using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Services;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public sealed class MHWAbnormality : CommonAbnormality, IUpdatable<MHWAbnormalityStructure>
{
    private float _timer;

    public override string Id { get; protected set; }

    public override string Name { get; protected set; }

    public override string Icon { get; protected set; }

    public override AbnormalityType Type { get; protected set; }

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

    public override bool IsInfinite { get; protected set; }

    public override int Level { get; protected set; }

    public override bool IsBuildup { get; protected set; }

    public MHWAbnormality(AbnormalityDefinition schema)
    {
        Id = schema.Id;
        Name = schema.Name;
        Icon = schema.Icon;
        Type = schema.Category switch
        {
            AbnormalityGroup.CONSUMABLES => AbnormalityType.Consumable,
            AbnormalityGroup.SONGS => AbnormalityType.Song,
            AbnormalityGroup.DEBUFFS => AbnormalityType.Debuff,
            AbnormalityGroup.SKILLS => AbnormalityType.Skill,
            AbnormalityGroup.ORCHESTRA => AbnormalityType.Orchestra,
            AbnormalityGroup.GEARS => AbnormalityType.Gear,
            AbnormalityGroup.FOODS => AbnormalityType.Food,
            _ => throw new NotImplementedException("unreachable")
        };
        IsBuildup = schema.IsBuildup;
        IsInfinite = schema.IsInfinite;

        if (IsBuildup)
            MaxTimer = schema.MaxBuildup;
    }

    void IUpdatable<MHWAbnormalityStructure>.Update(MHWAbnormalityStructure data)
    {
        MaxTimer = Math.Max(MaxTimer, data.Timer);
        Timer = data.Timer;
    }
}
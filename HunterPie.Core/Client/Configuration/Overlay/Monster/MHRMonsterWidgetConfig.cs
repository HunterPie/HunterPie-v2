using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Settings.Types;
using System.Linq;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MHRMonsterWidgetConfig : MonsterWidgetConfig
{
    public override MonsterDetailsConfiguration Details { get; set; } = new MonsterDetailsConfiguration
    {
        AllowedAilments = new(MonsterAilmentRepository.FindAllBy(GameType.Rise).Select(it => it.Id))
    };
}
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration(name: "LONGSWORD_WIDGET_STRING",
    icon: "ICON_LONGSWORD",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class LongSwordWidgetConfig : ClassWidgetConfig
{

}
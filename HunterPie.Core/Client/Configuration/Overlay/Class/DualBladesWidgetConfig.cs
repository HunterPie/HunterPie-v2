using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration(name: "DUALBLADES_WIDGET_STRING",
    icon: "ICON_DUALBLADES",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class DualBladesWidgetConfig : ClassWidgetConfig
{

}
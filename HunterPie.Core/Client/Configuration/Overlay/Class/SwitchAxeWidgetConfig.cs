using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration(name: "SWITCH_AXE_WIDGET_STRING",
    icon: "ICON_SWITCHAXE",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public class SwitchAxeWidgetConfig : ClassWidgetConfig
{

}
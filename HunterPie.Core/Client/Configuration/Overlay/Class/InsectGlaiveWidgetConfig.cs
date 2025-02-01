using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration(name: "INSECT_GLAIVE_WIDGET_STRING",
    icon: "ICON_INSECTGLAIVE",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public class InsectGlaiveWidgetConfig : ClassWidgetConfig
{

}
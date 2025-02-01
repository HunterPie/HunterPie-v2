using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration(name: "CHARGE_BLADE_WIDGET_STRING",
    icon: "ICON_CHARGEBLADE",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class ChargeBladeWidgetConfig : ClassWidgetConfig
{

}
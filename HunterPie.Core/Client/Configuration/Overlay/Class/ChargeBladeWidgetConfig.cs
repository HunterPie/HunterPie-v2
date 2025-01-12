using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration("CHARGE_BLADE_WIDGET_STRING", "ICON_CHARGEBLADE", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class ChargeBladeWidgetConfig : ClassWidgetConfig
{

}
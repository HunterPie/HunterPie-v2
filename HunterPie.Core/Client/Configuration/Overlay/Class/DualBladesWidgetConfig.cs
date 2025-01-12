using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration("DUALBLADES_WIDGET_STRING", "ICON_DUALBLADES", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class DualBladesWidgetConfig : ClassWidgetConfig
{

}
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration("INSECT_GLAIVE_WIDGET_STRING", "ICON_INSECTGLAIVE", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public class InsectGlaiveWidgetConfig : ClassWidgetConfig
{

}
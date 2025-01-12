using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration("LONGSWORD_WIDGET_STRING", "ICON_LONGSWORD", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public sealed class LongSwordWidgetConfig : ClassWidgetConfig
{

}
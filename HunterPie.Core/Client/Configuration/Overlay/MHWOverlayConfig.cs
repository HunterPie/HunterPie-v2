using HunterPie.Core.Client.Configuration.Overlay.Monster;

namespace HunterPie.Core.Client.Configuration.Overlay;

public class MHWOverlayConfig : OverlayConfig
{
    public override MonsterWidgetConfig BossesWidget { get; set; } = new MHWMonsterWidgetConfig();
}
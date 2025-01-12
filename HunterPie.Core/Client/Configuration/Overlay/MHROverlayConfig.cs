using HunterPie.Core.Client.Configuration.Overlay.Monster;

namespace HunterPie.Core.Client.Configuration.Overlay;

public class MHROverlayConfig : OverlayConfig
{
    public override MonsterWidgetConfig BossesWidget { get; set; } = new MHRMonsterWidgetConfig();
}
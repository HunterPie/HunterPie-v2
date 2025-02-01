using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.Core.Client.Configuration.Games;

public sealed class MHRConfig : GameConfig
{
    public override OverlayConfig Overlay { get; set; } = new MHROverlayConfig();
}
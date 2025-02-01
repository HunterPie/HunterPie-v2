using HunterPie.Core.Client.Configuration.Integrations;

namespace HunterPie.Core.Client.Configuration.Games;

public class GameConfig
{
    public DiscordRichPresence RichPresence { get; set; } = new();
    public virtual OverlayConfig Overlay { get; set; } = new();
}
using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Configuration.Versions;

namespace HunterPie.Core.Client.Configuration;

public class Config : VersionedConfig
{
    public Config() : base(0) { }

    public ClientConfig Client { get; set; } = new ClientConfig();
    public DiscordRichPresence RichPresence { get; set; } = new DiscordRichPresence();
    public OverlayConfig Overlay { get; set; } = new OverlayConfig();
    public DevelopmentConfig Debug { get; set; } = new DevelopmentConfig();
}
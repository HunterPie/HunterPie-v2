using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.Core.Client.Configuration.Versions;

public class V4Config : VersionedConfig
{
    public V4Config() : base(3) { }

    public ClientConfig Client { get; set; } = new();

    public MHRConfig Rise { get; set; } = new();

    public MHWConfig World { get; set; } = new();

    public OverlayClientConfig Overlay { get; set; } = new();

    public DevelopmentConfig Development { get; set; } = new();
}
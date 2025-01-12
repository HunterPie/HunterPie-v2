using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Versions;

namespace HunterPie.Core.Client.Configuration;

public class V2Config : VersionedConfig
{
    public V2Config() : base(1) { }

    public ClientConfig Client { get; set; } = new();

    public MHRConfig Rise { get; set; } = new();

    public MHWConfig World { get; set; } = new();

    public OverlayClientConfig Overlay { get; set; } = new();

    public DevelopmentConfig Development { get; set; } = new();

}
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Logger;

namespace HunterPie.Core.Client;

#nullable enable
public static class ClientConfig
{
    public const string CONFIG_NAME = "config.json";

    public static V4Config Config { get; } = new();

    internal static void Initialize()
    {
        ConfigManager.Register(CONFIG_NAME, Config);

        Log.Info("Initialized HunterPie Client configuration.");
    }
}
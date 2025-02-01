using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Observability.Logging;
using System;
using System.Threading;

namespace HunterPie.Core.Client;

#nullable enable
public static class ClientConfig
{
    private static readonly ILogger Logger = LoggerFactory.Create();
    public const string CONFIG_NAME = "config.json";

    public static bool IsInitialized => LazyConfig.IsValueCreated;

    private static readonly Lazy<V4Config> LazyConfig = new(() => new V4Config(), LazyThreadSafetyMode.ExecutionAndPublication);

    public static V4Config Config => LazyConfig.Value;

    internal static void Initialize()
    {
        ConfigManager.Register(CONFIG_NAME, Config);

        Logger.Info("Initialized HunterPie Client configuration.");
    }
}
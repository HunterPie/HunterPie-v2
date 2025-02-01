using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using System;

namespace HunterPie.Internal.Migrations;

internal class V4SettingsMigrator : ISettingsMigrator
{
    public bool CanMigrate(IVersionedConfig oldSettings) => oldSettings.GetType() == typeof(V3Config);

    public Type GetRequiredType() => typeof(V3Config);

    public IVersionedConfig Migrate(IVersionedConfig oldSettings)
    {
        if (oldSettings is not V3Config config)
            throw new InvalidCastException($"old config must be of type {typeof(V3Config)}, but was {oldSettings.GetType()}");

        var newConfig = new V4Config
        {
            Client = config.Client,
            Rise = config.Rise,
            World = config.World,
            Overlay = config.Overlay,
            Development = config.Development
        };

        if (config.Client.LastConfiguredGame.Value is > GameProcessType.All or <= GameProcessType.None)
            newConfig.Client.LastConfiguredGame.Value = GameProcessType.MonsterHunterRise;

        return newConfig;
    }
}
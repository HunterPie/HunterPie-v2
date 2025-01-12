using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Migrations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Config = HunterPie.Core.Client.Configuration.Config;

namespace HunterPie.Internal.Initializers;

#nullable enable
internal class ClientConfigMigrationInitializer : IInitializer
{
    private static readonly Dictionary<int, ISettingsMigrator> _migrators = new()
    {
        { 0, new V2SettingsMigrator() },
        { 1, new V3SettingsMigrator() },
        { 2, new V4SettingsMigrator() }
    };

    public Task Init()
    {
        CreateConfigIfNeeded();

        IVersionedConfig? versionedConfig = ReadSettingsAs<VersionedConfig>();

        if (versionedConfig is null)
        {
            Log.Error("config.json was corrupted. Generating a new one...");
            versionedConfig = (IVersionedConfig?)Activator.CreateInstance(ClientConfig.Config.GetType());
        }

        if (!_migrators.ContainsKey(versionedConfig!.Version))
            return Task.CompletedTask;

        ISettingsMigrator migrator = _migrators[versionedConfig.Version];
        versionedConfig = ReadSettingsAs<IVersionedConfig>(migrator.GetRequiredType());

        while (_migrators.ContainsKey(versionedConfig!.Version))
        {
            if (!migrator.CanMigrate(versionedConfig))
                return Task.CompletedTask;

            versionedConfig = migrator.Migrate(versionedConfig);
        }

        RewriteSettings(versionedConfig);

        return Task.CompletedTask;
    }

    private static void CreateConfigIfNeeded()
    {
        string configPath = ClientInfo.GetPathFor(ClientConfig.CONFIG_NAME);

        if (!File.Exists(configPath))
            ConfigHelper.WriteObject(configPath, new Config());
    }

    private static T? ReadSettingsAs<T>() => ReadSettingsAs<T>(typeof(T));

    private static T? ReadSettingsAs<T>(Type type)
    {
        string configPath = ClientInfo.GetPathFor(ClientInfo.CONFIG_NAME);

        if (!File.Exists(configPath))
            return default;

        string rawSettings = File.ReadAllText(configPath);

        return (T)JsonConvert.DeserializeObject(rawSettings, type);
    }

    private static void RewriteSettings(IVersionedConfig newSettings)
    {
        string serializedConfig = JsonProvider.Serialize(newSettings);

        File.WriteAllText(
            ClientInfo.GetPathFor(ClientInfo.CONFIG_NAME),
            serializedConfig
        );
    }
}
using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Internal.Migrations.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Internal.Migrations;

internal class V3SettingsMigrator : ISettingsMigrator
{

    private readonly V2ToV3AbnormalityIdsCorrelation correlations = new();

    public bool CanMigrate(IVersionedConfig oldSettings) => oldSettings.GetType() == typeof(V2Config);

    public Type GetRequiredType() => typeof(V2Config);

    public IVersionedConfig Migrate(IVersionedConfig oldSettings)
    {
        if (oldSettings is V2Config config)
        {
            V3Config newConfig = new()
            {
                Client = config.Client,
                Rise = config.Rise,
                World = config.World,
                Overlay = config.Overlay,
                Development = config.Development,
            };

            for (int i = 0; i < newConfig.Rise.Overlay.AbnormalityTray.Trays.Trays.Count; i++)
            {
                AbnormalityWidgetConfig tray = newConfig.Rise.Overlay.AbnormalityTray.Trays.Trays.ElementAt(i);
                var newIds = tray.AllowedAbnormalities.Select(oldId => (oldId, suffix: oldId.Split("_").Last()))
                                                                  .Select(abnorm => correlations.GetValueOrDefault(abnorm.suffix) ?? abnorm.oldId)
                                                                  .Where(id => id != null)
                                                                  .ToHashSet();

                tray.AllowedAbnormalities = new ObservableHashSet<string>(newIds);
            }

            return newConfig;
        }

        throw new InvalidCastException($"old config must be of type {typeof(V2Config)}, but was {oldSettings.GetType()}");
    }
}
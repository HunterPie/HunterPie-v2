﻿using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Settings.Factory;

internal class SettingsFactory
{
    private readonly PoogieVersionConnector _versionConnector;
    private readonly LocalAccountConfig _localAccountConfig;
    private readonly DefaultFeatureFlags _defaultFeatureFlags;
    private readonly ConfigurationAdapter _configurationAdapter;

    public SettingsFactory(
        PoogieVersionConnector versionConnector,
        LocalAccountConfig localAccountConfig,
        DefaultFeatureFlags defaultFeatureFlags,
        ConfigurationAdapter configurationAdapter)
    {
        _versionConnector = versionConnector;
        _localAccountConfig = localAccountConfig;
        _defaultFeatureFlags = defaultFeatureFlags;
        _configurationAdapter = configurationAdapter;
    }

    public async Task<SettingsViewModel> CreateFullAsync(Observable<GameProcessType> currentGame)
    {
        ConfigurationCategory[] commonConfigurations = await BuildCommonConfigurationAsync();

        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>>
        {
            { GameProcessType.MonsterHunterRise, BuildGameConfiguration(commonConfigurations, ClientConfig.Config.Rise, GameProcessType.MonsterHunterRise) },
            { GameProcessType.MonsterHunterWorld, BuildGameConfiguration(commonConfigurations, ClientConfig.Config.World, GameProcessType.MonsterHunterWorld) }
        };
        var supportedConfigurations =
            new ObservableCollection<GameProcessType>(new List<GameProcessType>
            {
                GameProcessType.MonsterHunterRise,
                GameProcessType.MonsterHunterWorld
            });

        return new SettingsViewModel(
            configurations: configurations,
            configurableGames: supportedConfigurations,
            currentConfiguredGame: currentGame,
            connector: _versionConnector
        );
    }

    public async Task<SettingsViewModel> CreatePartialAsync(GameProcessType game)
    {
        GameConfig config = ClientConfigHelper.GetGameConfigBy(game);
        ConfigurationCategory[] commonConfigurations = await BuildCommonConfigurationAsync();

        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>>
        {
            { game, BuildGameConfiguration(commonConfigurations, config, game) }
        };
        var supportedConfigurations = new[] { game }.ToObservableCollection();

        return new SettingsViewModel(
            configurations: configurations,
            configurableGames: supportedConfigurations,
            currentConfiguredGame: game,
            connector: _versionConnector
        );
    }

    private ObservableCollection<ConfigurationCategory> BuildGameConfiguration(
        IEnumerable<ConfigurationCategory> commonConfiguration,
        GameConfig configuration,
        GameProcessType gameProcessType
    )
    {
        ObservableCollection<ConfigurationCategory> configCategory = _configurationAdapter.Adapt(configuration, gameProcessType);

        return commonConfiguration.Concat(configCategory)
            .ToObservableCollection();
    }

    private async Task<ConfigurationCategory[]> BuildCommonConfigurationAsync()
    {
        ObservableCollection<ConfigurationCategory> generalConfig = _configurationAdapter.Adapt(ClientConfig.Config);
        ObservableCollection<ConfigurationCategory> accountConfig = await _localAccountConfig.BuildAccountConfigAsync();
        ObservableCollection<ConfigurationCategory> featureFlags = ClientConfig.Config.Client.EnableFeatureFlags.Value switch
        {
            true => FeatureFlagAdapter.Adapt(_defaultFeatureFlags.Flags),
            _ => new ObservableCollection<ConfigurationCategory>()
        };

        return accountConfig
            .Concat(generalConfig)
            .Concat(featureFlags)
            .ToArray();
    }
}
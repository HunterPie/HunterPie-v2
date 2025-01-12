using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Internal.Initializers;
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

    public SettingsFactory(
        PoogieVersionConnector versionConnector,
        LocalAccountConfig localAccountConfig)
    {
        _versionConnector = versionConnector;
        _localAccountConfig = localAccountConfig;
    }

    public async Task<SettingsViewModel> CreateFullAsync(Observable<GameProcessType> currentGame)
    {
        ObservableCollection<ConfigurationCategory> generalConfig = ConfigurationAdapter.Adapt(ClientConfig.Config);
        ObservableCollection<ConfigurationCategory> accountConfig = await _localAccountConfig.BuildAccountConfigAsync();
        ObservableCollection<ConfigurationCategory> featureFlags = ClientConfig.Config.Client.EnableFeatureFlags.Value switch
        {
            true => FeatureFlagAdapter.Adapt(FeatureFlagsInitializer.Features.Flags),
            _ => new ObservableCollection<ConfigurationCategory>()
        };

        var commonConfig = accountConfig
            .Concat(generalConfig)
            .Concat(featureFlags)
            .ToObservableCollection();
        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>>
        {
            { GameProcessType.MonsterHunterRise, BuildConfiguration(commonConfig, ClientConfig.Config.Rise, GameProcessType.MonsterHunterRise) },
            { GameProcessType.MonsterHunterWorld, BuildConfiguration(commonConfig, ClientConfig.Config.World, GameProcessType.MonsterHunterWorld) }
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

        ObservableCollection<ConfigurationCategory> generalConfig = ConfigurationAdapter.Adapt(ClientConfig.Config);
        ObservableCollection<ConfigurationCategory> accountConfig = await _localAccountConfig.BuildAccountConfigAsync();
        ObservableCollection<ConfigurationCategory> gameCategories = ConfigurationAdapter.Adapt(config, game);

        var configurationCategories = accountConfig
            .Concat(generalConfig)
            .Concat(gameCategories)
            .ToObservableCollection();
        var configurations = new Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>>
        {
            { game, configurationCategories }
        };
        var supportedConfigurations = new[] { game }.ToObservableCollection();

        return new SettingsViewModel(
            configurations: configurations,
            configurableGames: supportedConfigurations,
            currentConfiguredGame: game,
            connector: _versionConnector
        );
    }

    private static ObservableCollection<ConfigurationCategory> BuildConfiguration(
        IEnumerable<ConfigurationCategory> commonConfiguration,
        GameConfig configuration,
        GameProcessType gameProcessType
    )
    {
        ObservableCollection<ConfigurationCategory> configCategory = ConfigurationAdapter.Adapt(configuration, gameProcessType);

        return commonConfiguration.Concat(configCategory)
            .ToObservableCollection();
    }
}
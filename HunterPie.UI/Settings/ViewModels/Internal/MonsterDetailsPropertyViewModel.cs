using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Settings.Monsters.Builders;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Navigation;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class MonsterDetailsPropertyViewModel : ConfigurationPropertyViewModel
{
    public required ObservableHashSet<MonsterConfiguration> Configurations { get; init; }
    public required GameProcessType Game { get; init; }

    public void ConfigureParts()
    {
        MonsterConfigurationViewModel[] monsterViewModels = MonsterPartsViewModelBuilder.Build(
            game: Game,
            configurations: Configurations
        );
        OverlayConfig configuration = ClientConfigHelper.GetOverlayConfigFrom(Game);
        MonsterGlobalConfigurationViewModel globalConfigurationViewModel =
            MonsterGlobalConfigurationViewModelBuilder.Build(
                game: Game,
                partGroups: configuration.BossesWidget.Details.AllowedPartGroups,
                allowedAilments: configuration.BossesWidget.Details.AllowedAilments
            );

        var vm = new MonsterConfigurationsViewModel(
            globalConfiguration: globalConfigurationViewModel,
            configuration: configuration.BossesWidget.Details,
            elements: monsterViewModels
        );

        Navigator.Body.Navigate(vm);
    }
}
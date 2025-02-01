using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.ViewModels;
using HunterPie.Features.Account.Views;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.Features.Patches.Views;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Features.Settings.Views;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Details.Views;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.Features.Statistics.Views;
using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using HunterPie.UI.Controls.Settings.Abnormality.Views;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Controls.Settings.Monsters.Views;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Home.Views;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Logging.Views;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationTemplatesInitializer : IInitializer
{
    public Task Init()
    {
        NavigationProvider.Bind<MainBodyView, MainBodyViewModel>();
        NavigationProvider.Bind<HomeView, HomeViewModel>();
        NavigationProvider.Bind<AccountSignFlowView, AccountSignFlowViewModel>();

        NavigationProvider.Bind<ConsoleView, ConsoleViewModel>();
        NavigationProvider.Bind<SettingsView, SettingsViewModel>();
        NavigationProvider.Bind<PatchesView, PatchesViewModel>();
        NavigationProvider.Bind<AccountPreferencesView, AccountPreferencesViewModel>();
        NavigationProvider.Bind<AbnormalityWidgetSettingsView, AbnormalityWidgetSettingsViewModel>();
        NavigationProvider.Bind<QuestStatisticsSummariesView, QuestStatisticsSummariesViewModel>();
        NavigationProvider.Bind<QuestDetailsView, QuestDetailsViewModel>();
        NavigationProvider.Bind<MonsterConfigurationsView, MonsterConfigurationsViewModel>();

        return Task.CompletedTask;
    }
}
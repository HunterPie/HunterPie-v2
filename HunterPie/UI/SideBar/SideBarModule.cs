using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Domain.Sidebar;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.UI.SideBar;

internal class SideBarModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<ConsoleSideBarViewModel>()
            .WithSingle<DiscordSideBarViewModel>()
            .WithSingle<GitHubSideBarViewModel>()
            .WithSingle<HomeSideBarViewModel>()
            .WithSingle<PatchNotesSideBarViewModel>()
            .WithSingle<PatreonSideBarViewModel>()
            .WithSingle<QuestStatisticsSideBarViewModel>()
            .WithSingle<SettingsSideBarViewModel>()
            .WithSingle(() => SideBarProvider.Get(registry))
            .WithSingle<SideBarViewModel>();
    }
}
using HunterPie.DI;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.Domain.Sidebar;

internal static class SideBarProvider
{
    public static ISideBarCollection Get(IDependencyRegistry dependency) =>
        new SimpleSideBarBuilder()
            .WithButton(dependency.Get<HomeSideBarViewModel>())
            .WithButton(dependency.Get<ConsoleSideBarViewModel>())
            .WithButton(dependency.Get<SettingsSideBarViewModel>())
            .WithButton(dependency.Get<PatchNotesSideBarViewModel>())
            .WithButton(dependency.Get<QuestStatisticsSideBarViewModel>())
            .WithButton(dependency.Get<DiscordSideBarViewModel>())
            .WithButton(dependency.Get<PatreonSideBarViewModel>())
            .WithButton(dependency.Get<GitHubSideBarViewModel>())
            .Build();
}
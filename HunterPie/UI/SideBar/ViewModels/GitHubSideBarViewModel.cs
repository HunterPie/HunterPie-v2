using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Architecture;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class GitHubSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type? Type => null;

    public string Label => "//Strings/Client/Tabs/Tab[@Id='GITHUB_STRING']";

    public string Icon => "ICON_GITHUB";

    public bool IsAvailable => true;

    public bool IsSelected { get; set; }

    public Task ExecuteAsync()
    {
        BrowserService.OpenUrl(CommonLinks.GITHUB);

        return Task.CompletedTask;
    }
}
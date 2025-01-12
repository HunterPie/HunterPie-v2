using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Architecture;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class PatreonSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type? Type => null;

    public string Label => "//Strings/Client/Tabs/Tab[@Id='PATREON_STRING']";

    public string Icon => "ICON_PATREON";

    public bool IsAvailable => true;

    public bool IsSelected { get; set; }

    public Task ExecuteAsync()
    {
        BrowserService.OpenUrl(CommonLinks.PATREON);

        return Task.CompletedTask;
    }
}
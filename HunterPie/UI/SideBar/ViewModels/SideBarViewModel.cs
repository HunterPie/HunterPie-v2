using HunterPie.Domain.Sidebar;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.SideBar.ViewModels;

internal class SideBarViewModel : ViewModel
{
    public ObservableCollection<ISideBarViewModel> Elements { get; }

    public SideBarViewModel(
        ISideBarCollection collection
    )
    {
        Elements = collection.Elements;
    }
}
using HunterPie.UI.SideBar.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.Domain.Sidebar;

public interface ISideBarCollection
{
    public ObservableCollection<ISideBarViewModel> Elements { get; }
}
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.Domain.Sidebar;

public interface ISideBarBuilder
{
    public ISideBarBuilder WithButton(ISideBarViewModel button);

    public ISideBarCollection Build();
}
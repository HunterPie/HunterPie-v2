using HunterPie.UI.SideBar.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.Domain.Sidebar;

public class SimpleSideBarBuilder : ISideBarBuilder, ISideBarCollection
{
    public ObservableCollection<ISideBarViewModel> Elements { get; } = new();

    public ISideBarBuilder WithButton(ISideBarViewModel button)
    {
        Elements.Add(button);

        return this;
    }

    public ISideBarCollection Build() => this;
}
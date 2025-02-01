using HunterPie.Core.Game.Entity.Environment;

namespace HunterPie.UI.Overlay.Widgets.Activities.Common;

public interface IActivityContextHandler : IContextHandler
{
    public IActivity ViewModel { get; }
}
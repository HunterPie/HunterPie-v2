using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

public interface IClassController<out TViewModel> : IContextHandler where TViewModel : IClassViewModel
{
    public TViewModel ViewModel { get; }
}
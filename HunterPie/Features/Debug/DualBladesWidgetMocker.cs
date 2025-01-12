using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

public class DualBladesWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockDualBladesWidget)
            return;

        var view = new ClassView();

        view.ViewModel.CurrentSettings = ClientConfig.Config.Rise.Overlay.DualBladesWidget;
        view.ViewModel.InHuntingZone = true;
        view.ViewModel.Current = MockViewModel();

        WidgetManager.Register<ClassView, ClassWidgetConfig>(view);
    }

    private static DualBladesViewModel MockViewModel()
    {
        var vm = new DualBladesViewModel();

        return vm;
    }
}
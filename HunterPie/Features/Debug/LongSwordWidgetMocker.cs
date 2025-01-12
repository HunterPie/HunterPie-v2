using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

internal class LongSwordWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockLongSwordWidget)
            return;

        var mockSettings = new ClassWidgetConfig();

        var view = new ClassView();
        view.ViewModel.Current = MockViewModel();
        view.ViewModel.InHuntingZone = true;
        view.ViewModel.CurrentSettings = mockSettings;

        WidgetManager.Register<ClassView, ClassWidgetConfig>(view);
    }

    private static LongSwordViewModel MockViewModel()
    {
        return new LongSwordViewModel();
    }
}
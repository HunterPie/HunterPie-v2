using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

internal class SwitchAxeWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockSwitchAxeWidget)
            return;

        var view = new ClassView();

        view.ViewModel.CurrentSettings = ClientConfig.Config.Rise.Overlay.SwitchAxeWidget;
        view.ViewModel.InHuntingZone = true;
        view.ViewModel.Current = new SwitchAxeViewModel
        {
            MaxChargedTimer = 100.0f,
            ChargedTimer = 50.0f
        };

        WidgetManager.Register<ClassView, ClassWidgetConfig>(view);
    }
}
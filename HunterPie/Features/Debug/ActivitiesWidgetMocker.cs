using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.Features.Debug;

internal class ActivitiesWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        Core.Client.Configuration.OverlayConfig mockConfig = ClientConfig.Config.Rise.Overlay;

        if (ClientConfig.Config.Development.MockActivitiesWidget)
        {
            _ = WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
                new ActivitiesView(mockConfig.ActivitiesWidget)
                {
                    DataContext = new MockActivitiesViewModel()
                }
            );
        }
    }
}
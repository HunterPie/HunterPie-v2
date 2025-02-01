using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using HunterPie.UI.Overlay.Widgets.Player.Views;

namespace HunterPie.Features.Debug;
internal class PlayerHudWidgetMocker : IWidgetMocker
{
    public void Mock()
    {

        if (!ClientConfig.Config.Development.MockPlayerHudWidget)
            return;

        var config = new PlayerHudWidgetConfig();

        WidgetManager.Register<PlayerHudView, PlayerHudWidgetConfig>(
            new PlayerHudView(config)
            {
                DataContext = new MockPlayerHudViewModel()
            }
        );
    }
}
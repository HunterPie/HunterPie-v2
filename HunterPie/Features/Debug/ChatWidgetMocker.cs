using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using HunterPie.UI.Overlay.Widgets.Chat.Views;

namespace HunterPie.Features.Debug;

internal class ChatWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        Core.Client.Configuration.OverlayConfig mockConfig = ClientConfig.Config.Rise.Overlay;

        if (ClientConfig.Config.Development.MockChatWidget)
        {
            _ = WidgetManager.Register<ChatView, ChatWidgetConfig>(
                new ChatView(mockConfig.ChatWidget)
                {
                    DataContext = new MockChatViewModel()
                }
            );
        }
    }
}
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;
internal class ClassWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public Task LoadAsync(IContext context)
    {
        _handler = new ClassWidgetContextHandler(context);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}
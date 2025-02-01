using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.SpecializedTools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class SpecializedToolWidgetInitializer : IWidgetInitializer
{
    private readonly List<IContextHandler> _handlers = new(2);

    public Task LoadAsync(IContext context)
    {
        if (context is not MHWContext mhwContext)
            return Task.CompletedTask;

        InitializeTool(
            context: mhwContext,
            index: 0,
            configuration: ClientConfigHelper.DeferOverlayConfig(
                game: context.Process.Type,
                deferDelegate: cfg => cfg.PrimarySpecializedToolWidget
            )
        );
        InitializeTool(
            context: mhwContext,
            index: 1,
            configuration: ClientConfigHelper.DeferOverlayConfig(
                game: context.Process.Type,
                deferDelegate: cfg => cfg.SecondarySpecializedToolWidget
            )
        );

        return Task.CompletedTask;
    }

    public void Unload()
    {
        foreach (IContextHandler handler in _handlers)
            handler.UnhookEvents();

        _handlers.Clear();
    }

    private void InitializeTool(
        MHWContext context,
        int index,
        SpecializedToolWidgetConfig configuration)
    {
        if (!configuration.Initialize)
            return;

        var player = (MHWPlayer)context.Game.Player;

        _handlers.Add(
            item: new SpecializedToolWidgetContextHandler(
                context: context,
                specializedTool: player.Tools.ElementAtOrDefault(index),
                config: configuration
            )
        );
    }
}
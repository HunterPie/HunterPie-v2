using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Wirebug;

public class WirebugWidgetContextHandler : IContextHandler
{
    private static readonly HashSet<int> UnavailableStages = new() {
        1, // Room
        3, // Gathering Hub
        4, // Hub Prep Plaza
    };
    private readonly MHRContext _context;
    private readonly WirebugsViewModel _viewModel;
    private readonly WirebugsView _view;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;

    public WirebugWidgetContextHandler(MHRContext context)
    {
        _view = new WirebugsView(ClientConfig.Config.Rise.Overlay.WirebugWidget);
        _ = WidgetManager.Register<WirebugsView, WirebugWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        Player.OnStageUpdate += OnStageUpdate;
        Player.OnWirebugsRefresh += OnWirebugsRefresh;
    }

    public void UnhookEvents()
    {
        Player.OnStageUpdate -= OnStageUpdate;
        Player.OnWirebugsRefresh -= OnWirebugsRefresh;

        foreach (WirebugViewModel vm in _viewModel.Elements)
            if (vm is WirebugContextHandler model)
                model.UnhookEvents();

        _viewModel.Elements.Clear();

        _ = WidgetManager.Unregister<WirebugsView, WirebugWidgetConfig>(_view);
    }

    private void OnStageUpdate(object sender, EventArgs e) => _viewModel.IsAvailable = !UnavailableStages.Contains(Player.StageId);

    private void OnWirebugsRefresh(object sender, MHRWirebug[] e)
    {
        _view.Dispatcher.BeginInvoke(() =>
        {
            foreach (WirebugViewModel vm in _viewModel.Elements)
                if (vm is WirebugContextHandler model)
                    model.UnhookEvents();

            _viewModel.Elements.Clear();

            foreach (MHRWirebug wirebug in e)
                _viewModel.Elements.Add(new WirebugContextHandler(wirebug));
        });
    }

    private void UpdateData()
    {
        _viewModel.IsAvailable = !UnavailableStages.Contains(Player.StageId);

        foreach (MHRWirebug wirebug in Player.Wirebugs)
            _viewModel.Elements.Add(new WirebugContextHandler(wirebug));
    }
}
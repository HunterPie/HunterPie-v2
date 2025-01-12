using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Abnormality;

internal class AbnormalityWidgetContextHandler : IContextHandler
{
    private AbnormalityWidgetConfig _config;
    public readonly IContext Context;
    public readonly AbnormalityBarViewModel ViewModel;
    private readonly AbnormalityBarView View;
    private ref AbnormalityWidgetConfig Config => ref _config;

    public AbnormalityWidgetContextHandler(IContext context, ref AbnormalityWidgetConfig config)
    {
        _config = config;
        View = new AbnormalityBarView(ref Config);
        _ = WidgetManager.Register<AbnormalityBarView, AbnormalityWidgetConfig>(View);
        Context = context;

        ViewModel = View.ViewModel;

        UpdateData();
        HookEvents();
    }

    public void HookEvents()
    {
        Context.Game.Player.OnAbnormalityStart += OnAbnormalityStart;
        Context.Game.Player.OnAbnormalityEnd += OnAbnormalityEnd;
    }

    private void OnAbnormalityEnd(object sender, IAbnormality e)
    {
        _ = Application.Current.Dispatcher.InvokeAsync(() =>
        {
            AbnormalityContextHandler handler = ViewModel.Abnormalities.Cast<AbnormalityContextHandler>()
                .FirstOrDefault(vm => vm.Context == e);

            if (handler is null)
                return;

            handler.UnhookEvents();
            _ = ViewModel.Abnormalities.Remove(handler);
        }, DispatcherPriority.Normal);
    }

    private void OnAbnormalityStart(object sender, IAbnormality e)
    {
        _ = Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (!Config.AllowedAbnormalities.Contains(e.Id))
                return;

            ViewModel.Abnormalities.Add(new AbnormalityContextHandler(e));
        }, DispatcherPriority.Normal);
    }

    public void UnhookEvents()
    {
        Context.Game.Player.OnAbnormalityStart -= OnAbnormalityStart;
        Context.Game.Player.OnAbnormalityEnd -= OnAbnormalityEnd;
        _ = WidgetManager.Unregister<AbnormalityBarView, AbnormalityWidgetConfig>(View);
    }

    private void UpdateData()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (IAbnormality abnormality in Context.Game.Player.Abnormalities)
            {
                if (!Config.AllowedAbnormalities.Contains(abnormality.Id))
                    return;

                ViewModel.Abnormalities.Add(new AbnormalityContextHandler(abnormality));
            }
        }, DispatcherPriority.Normal);
    }
}
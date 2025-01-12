using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug;

internal class WirebugContextHandler : WirebugViewModel, IContextHandler
{
    public readonly MHRWirebug Context;

    public WirebugContextHandler(MHRWirebug context)
    {
        Context = context;

        UpdateData();
        HookEvents();
    }

    public void HookEvents()
    {
        Context.OnAvailableChange += OnAvailableChange;
        Context.OnTemporaryChange += OnTemporaryChange;
        Context.OnCooldownUpdate += OnCooldownUpdate;
        Context.OnTimerUpdate += OnTimerUpdate;
        Context.OnWirebugStateChange += OnWirebugStateChange;
    }

    public void UnhookEvents()
    {
        Context.OnAvailableChange -= OnAvailableChange;
        Context.OnTemporaryChange -= OnTemporaryChange;
        Context.OnCooldownUpdate -= OnCooldownUpdate;
        Context.OnTimerUpdate -= OnTimerUpdate;
        Context.OnWirebugStateChange -= OnWirebugStateChange;
    }

    private void OnAvailableChange(object sender, MHRWirebug e) => IsAvailable = e.IsAvailable;
    private void OnTemporaryChange(object sender, MHRWirebug e) => IsTemporary = e.IsTemporary;

    private void OnCooldownUpdate(object sender, MHRWirebug e)
    {
        Cooldown = e.Cooldown;
        OnCooldown = Cooldown > 0;
        MaxCooldown = e.MaxCooldown;
    }

    private void OnTimerUpdate(object sender, MHRWirebug e)
    {
        MaxTimer = e.MaxTimer;
        Timer = e.Timer;
    }

    private void OnWirebugStateChange(object sender, MHRWirebug e) => WirebugState = e.WirebugState;

    private void UpdateData()
    {
        IsAvailable = Context.IsAvailable;
        IsTemporary = Context.IsTemporary;
        MaxCooldown = Context.MaxCooldown;
        Cooldown = Context.Cooldown;
        OnCooldown = Cooldown > 0;
        MaxTimer = Context.MaxTimer;
        Timer = Context.Timer;
        WirebugState = Context.WirebugState;
    }
}
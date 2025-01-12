using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Classes.Controllers;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;
using System;

namespace HunterPie.UI.Overlay.Widgets.Classes;

#nullable enable
public class ClassWidgetContextHandler : IContextHandler
{
    private readonly ClassViewModel _viewModel;
    private readonly ClassView _view;
    private readonly IContext _context;
    private IClassController<IClassViewModel>? _weaponController;

    public ClassWidgetContextHandler(IContext context)
    {
        _view = new ClassView();
        _ = WidgetManager.Register<ClassView, ClassWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        Update();
        HookEvents();
    }

    public void HookEvents()
    {
        _context.Game.Player.OnWeaponChange += OnWeaponChange;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
    }

    private void OnStageUpdate(object? sender, EventArgs e) => _viewModel.InHuntingZone = _context.Game.Player.InHuntingZone;

    public void UnhookEvents()
    {
        _context.Game.Player.OnWeaponChange -= OnWeaponChange;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
        _ = WidgetManager.Unregister<ClassView, ClassWidgetConfig>(_view);
    }

    private void OnWeaponChange(object? sender, WeaponChangeEventArgs e)
    {
        UpdateConfig(e.NewWeapon);
        UpdateController(e.NewWeapon);
    }

    private void UpdateConfig(IWeapon weapon)
    {
        OverlayConfig overlayConfig = ClientConfigHelper.GetOverlayConfigFrom(_context.Process.Type);
        ClassWidgetConfig? config = weapon.Id switch
        {
            Weapon.InsectGlaive => overlayConfig.InsectGlaiveWidget,
            Weapon.ChargeBlade => overlayConfig.ChargeBladeWidget,
            Weapon.DualBlades => overlayConfig.DualBladesWidget,
            Weapon.SwitchAxe => overlayConfig.SwitchAxeWidget,
            Weapon.Longsword => overlayConfig.LongSwordWidget,
            _ => null
        };
        _viewModel.CurrentSettings = config;
    }

    private void UpdateController(IWeapon weapon)
    {
        _weaponController?.UnhookEvents();

        _weaponController = weapon switch
        {
            IInsectGlaive insectGlaive => new InsectGlaiveController(_context, insectGlaive),
            IChargeBlade chargeBlade => new ChargeBladeController(_context, chargeBlade),
            IDualBlades dualBlades => new DualBladesController(_context, dualBlades),
            ISwitchAxe switchAxe => new SwitchAxeController(_context, switchAxe),
            ILongSword longSword => new LongSwordController(longSword),
            _ => null
        };
        _weaponController?.HookEvents();
        _viewModel.Current = _weaponController?.ViewModel;
    }

    private void Update()
    {
        UpdateConfig(_context.Game.Player.Weapon);
        UpdateController(_context.Game.Player.Weapon);
        _viewModel.InHuntingZone = _context.Game.Player.InHuntingZone;
    }
}
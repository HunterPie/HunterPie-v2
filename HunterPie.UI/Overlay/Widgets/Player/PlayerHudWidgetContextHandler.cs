using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using HunterPie.UI.Overlay.Widgets.Player.Views;
using System;

namespace HunterPie.UI.Overlay.Widgets.Player;
public class PlayerHudWidgetContextHandler : IContextHandler
{
    private readonly PlayerHudView _view;
    private readonly PlayerHudViewModel _viewModel;
    private readonly IContext _context;
    private IPlayer Player => _context.Game.Player;

    public PlayerHudWidgetContextHandler(IContext context)
    {
        PlayerHudWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            deferDelegate: (config) => config.PlayerHudWidget
        );

        _view = new PlayerHudView(config);
        _viewModel = _view.ViewModel;
        _context = context;
        _ = WidgetManager.Register<PlayerHudView, PlayerHudWidgetConfig>(_view);

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        Player.OnLogin += OnPlayerLogin;
        Player.OnLevelChange += OnPlayerLevelChange;
        Player.OnWeaponChange += OnPlayerWeaponChange;
        Player.OnStageUpdate += OnStageChange;
        Player.Health.OnHealthChange += OnPlayerHealthChange;
        Player.Stamina.OnStaminaChange += OnPlayerStaminaChange;
        Player.Health.OnHeal += OnHeal;
        Player.OnAbnormalityStart += OnPlayerAbnormalityStart;
        Player.OnAbnormalityEnd += OnPlayerAbnormalityEnd;

        if (Player.Weapon is IMeleeWeapon weapon)
        {
            weapon.OnSharpnessChange += OnSharpnessChange;
            weapon.OnSharpnessLevelChange += OnSharpnessLevelChange;
        }
    }

    public void UnhookEvents()
    {
        Player.OnLogin -= OnPlayerLogin;
        Player.OnLevelChange -= OnPlayerLevelChange;
        Player.OnWeaponChange -= OnPlayerWeaponChange;
        Player.OnStageUpdate -= OnStageChange;
        Player.Health.OnHealthChange -= OnPlayerHealthChange;
        Player.Stamina.OnStaminaChange -= OnPlayerStaminaChange;
        Player.Health.OnHeal -= OnHeal;
        Player.OnAbnormalityStart -= OnPlayerAbnormalityStart;
        Player.OnAbnormalityEnd -= OnPlayerAbnormalityEnd;

        if (Player.Weapon is IMeleeWeapon weapon)
        {
            weapon.OnSharpnessChange -= OnSharpnessChange;
            weapon.OnSharpnessLevelChange -= OnSharpnessLevelChange;
        }

        _ = WidgetManager.Unregister<PlayerHudView, PlayerHudWidgetConfig>(_view);
    }

    private void OnPlayerAbnormalityEnd(object sender, IAbnormality e)
    {
        AbnormalityCategory category = _context.Game.AbnormalityCategorizationService.Categorize(e);

        if (category == AbnormalityCategory.None)
            return;

        if (!_viewModel.ActiveAbnormalities.Contains(category))
            return;

        _viewModel.ActiveAbnormalities.Remove(category);
    }

    private void OnPlayerAbnormalityStart(object sender, IAbnormality e)
    {
        AbnormalityCategory category = _context.Game.AbnormalityCategorizationService.Categorize(e);

        if (category == AbnormalityCategory.None)
            return;

        _viewModel.ActiveAbnormalities.Add(category);
    }
    private void OnHeal(object sender, HealthChangeEventArgs e) => _viewModel.Heal = e.Heal;

    private void OnStageChange(object sender, EventArgs e) => _viewModel.InHuntingZone = Player.InHuntingZone;

    private void OnPlayerLevelChange(object sender, LevelChangeEventArgs e) => _viewModel.Level = Player.MasterRank;

    private void OnPlayerWeaponChange(object sender, WeaponChangeEventArgs e)
    {
        if (e.OldWeapon is IMeleeWeapon melee)
        {
            melee.OnSharpnessLevelChange -= OnSharpnessLevelChange;
            melee.OnSharpnessChange -= OnSharpnessChange;
        }

        if (e.NewWeapon is IMeleeWeapon newMelee)
        {
            newMelee.OnSharpnessLevelChange += OnSharpnessLevelChange;
            newMelee.OnSharpnessChange += OnSharpnessChange;
        }

        _viewModel.Weapon = e.NewWeapon.Id;
    }

    private void OnSharpnessChange(object sender, SharpnessEventArgs e)
    {
        _viewModel.SharpnessViewModel.MaxSharpness = e.MaxSharpness - e.Threshold;
        _viewModel.SharpnessViewModel.Sharpness = e.CurrentSharpness - e.Threshold;
    }

    private void OnSharpnessLevelChange(object sender, SharpnessEventArgs e)
    {
        _viewModel.SharpnessViewModel.SharpnessLevel = e.Sharpness;
        _viewModel.SharpnessViewModel.MaxSharpness = e.MaxSharpness - e.Threshold;
        _viewModel.SharpnessViewModel.Sharpness = e.CurrentSharpness - e.Threshold;
    }

    private void OnPlayerLogin(object sender, EventArgs e)
    {
        _viewModel.Name = Player.Name;
        _viewModel.Level = Player.MasterRank;
    }

    private void OnPlayerStaminaChange(object sender, StaminaChangeEventArgs e)
    {
        _viewModel.MaxStamina = e.MaxStamina;
        _viewModel.Stamina = e.Stamina;
        _viewModel.MaxPossibleStamina = e.MaxPossibleStamina;
        _viewModel.MaxRecoverableStamina = e.MaxRecoverableStamina;
    }

    private void OnPlayerHealthChange(object sender, HealthChangeEventArgs e)
    {
        _viewModel.MaxHealth = e.MaxHealth;
        _viewModel.MaxExtendableHealth = e.MaxPossibleHealth;
        _viewModel.Health = e.Health;
        _viewModel.RecoverableHealth = e.RecoverableHealth;
    }

    private void UpdateData()
    {
        _viewModel.MaxHealth = Player.Health.Max;
        _viewModel.MaxExtendableHealth = Player.Health.MaxPossibleHealth;
        _viewModel.Health = Player.Health.Current;
        _viewModel.RecoverableHealth = Player.Health.RecoverableHealth;
        _viewModel.MaxPossibleStamina = Player.Stamina.MaxPossibleStamina;
        _viewModel.MaxRecoverableStamina = Player.Stamina.MaxRecoverableStamina;
        _viewModel.MaxStamina = Player.Stamina.Max;
        _viewModel.Stamina = Player.Stamina.Current;
        _viewModel.Name = Player.Name;
        _viewModel.Level = Player.MasterRank;
        _viewModel.Weapon = Player.Weapon.Id;

        if (Player.Weapon is IMeleeWeapon weapon)
        {
            _viewModel.SharpnessViewModel.MaxSharpness = weapon.MaxSharpness - weapon.Threshold;
            _viewModel.SharpnessViewModel.Sharpness = weapon.CurrentSharpness - weapon.Threshold;
            _viewModel.SharpnessViewModel.SharpnessLevel = weapon.Sharpness;
        }
    }
}
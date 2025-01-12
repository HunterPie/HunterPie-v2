using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Events;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.Controllers;

/// <summary>
/// Charge Blade widget controller
/// </summary>
public class ChargeBladeController : IClassController<ChargeBladeViewModel>
{
    private readonly IContext _context;
    private readonly IChargeBlade _weapon;

    public ChargeBladeViewModel ViewModel { get; } = new();

    public ChargeBladeController(IContext context, IChargeBlade weapon)
    {
        _context = context;
        _weapon = weapon;

        Update();
    }

    public void HookEvents()
    {
        _weapon.OnShieldBuffTimerChange += OnShieldBuffTimerChange;
        _weapon.OnAxeBuffTimerChange += OnAxeBuffTimerChange;
        _weapon.OnSwordBuffTimerChange += OnSwordBuffTimerChange;
        _weapon.OnChargeBuildUpChange += OnChargeBuildUpChange;
        _weapon.OnPhialsChange += OnPhialsChange;
    }

    public void UnhookEvents()
    {
        _weapon.OnShieldBuffTimerChange -= OnShieldBuffTimerChange;
        _weapon.OnAxeBuffTimerChange -= OnAxeBuffTimerChange;
        _weapon.OnSwordBuffTimerChange -= OnSwordBuffTimerChange;
        _weapon.OnChargeBuildUpChange -= OnChargeBuildUpChange;
        _weapon.OnPhialsChange -= OnPhialsChange;
    }

    private void OnPhialsChange(object sender, ChargeBladePhialChangeEventArgs e)
    {
        ViewModel.Charge = e.Charge;

        ViewModel.UIThread.Invoke(() =>
        {
            if (ViewModel.Phials.Count != e.MaxPhials)
                for (int i = 0; i < e.MaxPhials; i++)
                {
                    int phialsCount = ViewModel.Phials.Count;

                    if (phialsCount < e.MaxPhials)
                        ViewModel.Phials.Add(new ChargeBladePhialViewModel());
                    else if (phialsCount > e.MaxPhials)
                        ViewModel.Phials.RemoveAt(phialsCount - 1);
                    else
                        break;
                }


            for (int i = 0; i < e.MaxPhials; i++)
                ViewModel.Phials[i].IsActive = i < _weapon.Phials;
        });

    }

    private void OnChargeBuildUpChange(object sender, ChargeBladeBuildUpChangeEventArgs e)
    {
        ViewModel.ChargeBuildUp = e.Current;
        ViewModel.ChargeMaxBuildUp = e.Max;
    }

    private void OnSwordBuffTimerChange(object sender, ChargeBladeBuffTimerChangeEventArgs e)
    {
        ViewModel.SwordBuff = e.Timer;
    }

    private void OnAxeBuffTimerChange(object sender, ChargeBladeBuffTimerChangeEventArgs e)
    {
        ViewModel.AxeBuff = e.Timer;
    }

    private void OnShieldBuffTimerChange(object sender, ChargeBladeBuffTimerChangeEventArgs e)
    {
        ViewModel.ShieldBuff = e.Timer;
    }

    private void Update()
    {
        ViewModel.Charge = _weapon.Charge;
        ViewModel.ShieldBuff = _weapon.ShieldBuff;
        ViewModel.AxeBuff = _weapon.AxeBuff;
        ViewModel.SwordBuff = _weapon.SwordBuff;
        ViewModel.ChargeBuildUp = _weapon.ChargeBuildUp;
        ViewModel.ChargeMaxBuildUp = _weapon.MaxChargeBuildUp;

        for (int i = 0; i < _weapon.Phials; i++)
            ViewModel.Phials.Add(new ChargeBladePhialViewModel { IsActive = i <= _weapon.Phials });
    }

}
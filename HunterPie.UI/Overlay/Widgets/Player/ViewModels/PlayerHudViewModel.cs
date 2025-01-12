using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Player.ViewModels;

public class PlayerHudViewModel : ViewModel
{

    private int _level;
    public int Level { get => _level; set => SetValue(ref _level, value); }

    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private double _health;
    public double Health { get => _health; set => SetValue(ref _health, value); }

    private double _maxHealth;
    public double MaxHealth { get => _maxHealth; set => SetValue(ref _maxHealth, value); }

    private double _maxExtendableHealth;
    public double MaxExtendableHealth { get => _maxExtendableHealth; set => SetValue(ref _maxExtendableHealth, value); }

    private double _recoverableHealth;
    public double RecoverableHealth { get => _recoverableHealth; set => SetValue(ref _recoverableHealth, value); }

    private double _heal;
    public double Heal { get => _heal; set => SetValue(ref _heal, value); }

    private double _stamina;
    public double Stamina { get => _stamina; set => SetValue(ref _stamina, value); }

    private double _maxStamina;
    public double MaxStamina { get => _maxStamina; set => SetValue(ref _maxStamina, value); }

    private double _maxPossibleStamina;
    public double MaxPossibleStamina { get => _maxPossibleStamina; set => SetValue(ref _maxPossibleStamina, value); }

    private double _maxRecoverableStamina;
    public double MaxRecoverableStamina { get => _maxRecoverableStamina; set => SetValue(ref _maxRecoverableStamina, value); }

    private bool _inHuntingZone;
    public bool InHuntingZone { get => _inHuntingZone; set => SetValue(ref _inHuntingZone, value); }

    private Weapon _weapon;
    public Weapon Weapon { get => _weapon; set => SetValue(ref _weapon, value); }

    public ThreadSafeObservableCollection<AbnormalityCategory> ActiveAbnormalities { get; } = new();

    private bool _isMoxieActive;
    public bool IsMoxieActive { get => _isMoxieActive; set => SetValue(ref _isMoxieActive, value); }

    public WeaponSharpnessViewModel SharpnessViewModel { get; } = new();
}
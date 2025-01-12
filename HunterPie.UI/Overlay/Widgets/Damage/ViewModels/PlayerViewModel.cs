using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PlayerViewModel : ViewModel
{
    private readonly DamageMeterWidgetConfig _config;
    private string _name;
    private Weapon _weapon;
    private int _damage;
    private double _dps;
    private DamageBarViewModel _bar;
    private bool _isIncreasing;
    private bool _isUser;
    private int _masterRank;

    public PlayerViewModel(DamageMeterWidgetConfig config)
    {
        _config = config;
    }

    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }
    public Weapon Weapon
    {
        get => _weapon;
        set => SetValue(ref _weapon, value);
    }
    public int Damage
    {
        get => _damage;
        set => SetValue(ref _damage, value);
    }
    public double DPS
    {
        get => _dps;
        set => SetValue(ref _dps, value);
    }
    public DamageBarViewModel Bar { get => _bar; set => SetValue(ref _bar, value); }
    public bool IsIncreasing
    {
        get => _isIncreasing;
        set => SetValue(ref _isIncreasing, value);
    }
    public bool IsUser
    {
        get => _isUser;
        set => SetValue(ref _isUser, value);
    }

    public int MasterRank { get => _masterRank; set => SetValue(ref _masterRank, value); }

    // Settings related
    public Observable<bool> ShouldHighlightMyself => _config.ShouldHighlightMyself;
    public Observable<bool> ShouldBlurNames => _config.ShouldBlurNames;
    public Observable<bool> ShouldShowDPS => _config.ShouldShowDPS;
    public Observable<bool> ShouldShowDamage => _config.ShouldShowDamage;

}
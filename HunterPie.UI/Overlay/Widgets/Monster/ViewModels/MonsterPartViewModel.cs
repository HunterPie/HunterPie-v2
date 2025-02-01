using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class MonsterPartViewModel : AutoVisibilityViewModel
{
    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private double _health;
    public double Health { get => _health; set => SetValueAndRefresh(ref _health, value); }

    private double _maxHealth;
    public double MaxHealth { get => _maxHealth; set => SetValue(ref _maxHealth, value); }

    private double _tenderize;
    public double Tenderize { get => _tenderize; set => SetValueAndRefresh(ref _tenderize, value); }

    private double _maxTenderize;
    public double MaxTenderize { get => _maxTenderize; set => SetValue(ref _maxTenderize, value); }

    private double _flinch;
    public double Flinch { get => _flinch; set => SetValueAndRefresh(ref _flinch, value); }

    private double _maxFlinch;
    public double MaxFlinch { get => _maxFlinch; set => SetValue(ref _maxFlinch, value); }

    private double _sever;
    public double Sever { get => _sever; set => SetValueAndRefresh(ref _sever, value); }

    private double _maxSever;
    public double MaxSever { get => _maxSever; set => SetValue(ref _maxSever, value); }

    private double _qurioHealth;
    public double QurioHealth { get => _qurioHealth; set => SetValueAndRefresh(ref _qurioHealth, value); }

    private double _qurioMaxHealth;
    public double QurioMaxHealth { get => _qurioMaxHealth; set => SetValue(ref _qurioMaxHealth, value); }

    private int _breaks;
    public int Breaks { get => _breaks; set => SetValueAndRefresh(ref _breaks, value); }

    private int _maxBreaks;
    public int MaxBreaks { get => _maxBreaks; set => SetValue(ref _maxBreaks, value); }

    private bool _isPartBroken;
    public bool IsPartBroken { get => _isPartBroken; set => SetValue(ref _isPartBroken, value); }

    private bool _isPartSevered;
    public bool IsPartSevered { get => _isPartSevered; set => SetValue(ref _isPartSevered, value); }

    private bool _isKnownPart;
    public bool IsKnownPart { get => _isKnownPart; set => SetValue(ref _isKnownPart, value); }

    private PartType _type;
    public PartType Type { get => _type; set => SetValue(ref _type, value); }

    public MonsterPartViewModel(MonsterWidgetConfig config) : base(config.AutoHidePartsDelay) { }
}
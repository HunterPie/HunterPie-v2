using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class AbnormalityViewModel : Bindable
{
    private string _icon;
    public string Icon
    {
        get => _icon;
        set => SetValue(ref _icon, value);
    }

    private string _id;
    public string Id
    {
        get => _id;
        set => SetValue(ref _id, value);
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    private double _timer;
    public double Timer
    {
        get => _timer;
        set => SetValue(ref _timer, value);
    }

    private double _maxTimer;
    public double MaxTimer
    {
        get => _maxTimer;
        set => SetValue(ref _maxTimer, value);
    }

    private bool _isBuff;
    public bool IsBuff
    {
        get => _isBuff;
        set => SetValue(ref _isBuff, value);
    }

    public bool _isBuildup;
    public bool IsBuildup
    {
        get => _isBuildup;
        set => SetValue(ref _isBuildup, value);
    }

    private bool _isInfinite;
    public bool IsInfinite { get => _isInfinite; set => SetValue(ref _isInfinite, value); }
}
using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class SubmarineViewModel : Bindable
{
    private int _count;
    private int _maxCount;
    private int _daysLeft;
    private bool _isActive;

    public int Count { get => _count; set => SetValue(ref _count, value); }
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }
}
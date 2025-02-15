using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarineBoostViewModel : ViewModel
{
    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    private bool _isExtraBoost;
    public bool IsExtraBoost { get => _isExtraBoost; set => SetValue(ref _isExtraBoost, value); }
}
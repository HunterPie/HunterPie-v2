using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
public class ChargeBladePhialViewModel : ViewModel
{
    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    private float _timer;
    public float Timer { get => _timer; set => SetValue(ref _timer, value); }
}
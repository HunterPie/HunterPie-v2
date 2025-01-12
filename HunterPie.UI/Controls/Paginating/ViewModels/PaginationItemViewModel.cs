using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Paginating.ViewModels;

public class PaginationItemViewModel : ViewModel
{
    private int _page;
    public int Page { get => _page; set => SetValue(ref _page, value); }

    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    private bool _isVisible;
    public bool IsVisible { get => _isVisible; set => SetValue(ref _isVisible, value); }
}
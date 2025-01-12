using HunterPie.UI.Architecture;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountSignFlowViewModel : ViewModel
{
    private readonly MainNavigator _mainNavigator;

    private int _selectedIndex;
    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }

    public AccountSignFlowViewModel(MainNavigator mainNavigator)
    {
        _mainNavigator = mainNavigator;
    }

    public void NavigateBack()
    {
        _mainNavigator.Navigate<MainBodyViewModel>();
    }
}

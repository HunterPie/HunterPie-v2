using HunterPie.UI.Architecture;

namespace HunterPie.UI.Navigation;

public interface INavigator
{
    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel;

    public void Navigate<TViewModel>() where TViewModel : ViewModel;

    public void Return();

    public void ReturnWhen<TViewModel>() where TViewModel : ViewModel;
}
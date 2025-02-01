using HunterPie.UI.Architecture;
using HunterPie.UI.Header.ViewModels;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel : ViewModel
{
    public HeaderViewModel HeaderViewModel { get; }

    private ViewModel? _contentViewModel;
    public ViewModel? ContentViewModel { get => _contentViewModel; set => SetValue(ref _contentViewModel, value); }

    public MainViewModel(HeaderViewModel headerViewModel)
    {
        HeaderViewModel = headerViewModel;
    }
}
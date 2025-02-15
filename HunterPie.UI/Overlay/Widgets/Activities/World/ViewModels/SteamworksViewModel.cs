using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class SteamworksViewModel : ViewModel
{
    private int _naturalFuel;
    public int NaturalFuel { get => _naturalFuel; set => SetValue(ref _naturalFuel, value); }

    private int _maxNaturalFuel;
    public int MaxNaturalFuel { get => _maxNaturalFuel; set => SetValue(ref _maxNaturalFuel, value); }

    private int _storedFuel;
    public int StoredFuel { get => _storedFuel; set => SetValue(ref _storedFuel, value); }
}
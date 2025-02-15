using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class HarvestBoxViewModel : ViewModel
{
    public ObservableCollection<HarvestFertilizerViewModel> Fertilizers { get; } = new();

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
}
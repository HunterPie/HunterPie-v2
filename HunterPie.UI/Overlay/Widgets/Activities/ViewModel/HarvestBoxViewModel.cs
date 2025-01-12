using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class HarvestBoxViewModel : Bindable, IActivity
{
    public ObservableCollection<HarvestFertilizerViewModel> Fertilizers { get; } = new();

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }

    public ActivityType Type => ActivityType.HarvestBox;
}
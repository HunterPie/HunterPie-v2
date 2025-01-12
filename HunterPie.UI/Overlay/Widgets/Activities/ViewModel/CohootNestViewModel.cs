using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class CohootNestViewModel : Bindable, IActivity
{
    private int _kamuraCount;
    private int _kamuraMaxCount;
    private int _elgadoCount;
    private int _elgadoMaxCount;
    private int _count;
    private int _maxCount;

    public int KamuraCount { get => _kamuraCount; set => SetValue(ref _kamuraCount, value); }
    public int KamuraMaxCount { get => _kamuraMaxCount; set => SetValue(ref _kamuraMaxCount, value); }
    public int ElgadoCount { get => _elgadoCount; set => SetValue(ref _elgadoCount, value); }
    public int ElgadoMaxCount { get => _elgadoMaxCount; set => SetValue(ref _elgadoMaxCount, value); }
    public int Count { get => _count; set => SetValue(ref _count, value); }
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }

    public ActivityType Type => ActivityType.Cohoot;
}
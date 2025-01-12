using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Overlay.Widgets.Clock.ViewModels;

public class ClockViewModel : ViewModel
{
    private TimeOnly _worldTime;
    public TimeOnly WorldTime { get => _worldTime; set => SetValue(ref _worldTime, value); }

    private TimeSpan? _questTimeLeft;
    public TimeSpan? QuestTimeLeft { get => _questTimeLeft; set => SetValue(ref _questTimeLeft, value); }
}
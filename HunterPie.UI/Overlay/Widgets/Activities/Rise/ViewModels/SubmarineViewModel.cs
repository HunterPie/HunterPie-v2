using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarineViewModel : ViewModel
{
    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }

    private int _daysLeft;
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }

    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    public ObservableCollection<SubmarineBoostViewModel> Boosts { get; } = new();

    public void SetMaxBoosts(int count)
    {
        if (Boosts.Count == count)
            return;

        for (int i = 0; i < count; i++)
            Boosts.Add(new SubmarineBoostViewModel());
    }

    public void SetBoosts(int normalCount, int extraCount)
    {
        for (int i = 0; i < Boosts.Count; i++)
        {
            SubmarineBoostViewModel viewModel = Boosts[i];
            viewModel.IsActive = i < normalCount + extraCount;
            viewModel.IsExtraBoost = viewModel.IsActive && i >= normalCount;
        }
    }
}
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class HarvestFertilizerViewModel : ViewModel
{
    private const int FERTILIZER_DAYS = 9;

    private bool _isFirst;
    public bool IsFirst { get => _isFirst; set => SetValue(ref _isFirst, value); }

    private Fertilizer _fertilizer;
    public Fertilizer Fertilizer { get => _fertilizer; set => SetValue(ref _fertilizer, value); }

    public ObservableCollection<FertilizerDayViewModel> Days { get; } = InitializeDays(FERTILIZER_DAYS);

    private bool _isExpiring;
    public bool IsExpiring { get => _isExpiring; set => SetValue(ref _isExpiring, value); }

    public HarvestFertilizerViewModel SetDays(int days)
    {
        for (int i = 0; i < Days.Count; i++)
        {
            FertilizerDayViewModel day = Days[i];
            day.IsActive = i < days;
        }

        return this;
    }

    private static ObservableCollection<FertilizerDayViewModel> InitializeDays(int days)
    {
        var viewModels = new FertilizerDayViewModel[days];

        for (int i = 0; i < viewModels.Length; i++)
            viewModels[i] = new FertilizerDayViewModel { IsActive = false };

        return viewModels.ToObservableCollection();
    }
}
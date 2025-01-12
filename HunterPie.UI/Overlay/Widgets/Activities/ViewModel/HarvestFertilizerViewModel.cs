using HunterPie.Core.Architecture;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class HarvestFertilizerViewModel : Bindable
{
    private Fertilizer _fertilizer;
    public Fertilizer Fertilizer { get => _fertilizer; set => SetValue(ref _fertilizer, value); }

    private int _amount;
    public int Amount { get => _amount; set => SetValue(ref _amount, value); }

    private int _maxAmount;
    public int MaxAmount { get => _maxAmount; set => SetValue(ref _maxAmount, value); }

    private bool _isExpiring;
    public bool IsExpiring { get => _isExpiring; set => SetValue(ref _isExpiring, value); }
}
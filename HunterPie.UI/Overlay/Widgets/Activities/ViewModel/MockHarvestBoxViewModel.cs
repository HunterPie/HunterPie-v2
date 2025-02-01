using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
internal class MockHarvestBoxViewModel : HarvestBoxViewModel
{
    public MockHarvestBoxViewModel()
    {
        Count = 25;
        MaxCount = 50;

        SetupFertilizers();
    }

    private void SetupFertilizers()
    {
        Fertilizers.Add(
            new HarvestFertilizerViewModel()
            {
                Amount = 8,
                Fertilizer = Fertilizer.FungiL,
                MaxAmount = 8,
            }
        );
        Fertilizers.Add(
            new HarvestFertilizerViewModel()
            {
                Amount = 4,
                Fertilizer = Fertilizer.GrowthL,
                MaxAmount = 8,
                IsExpiring = true,
            }
        );
        Fertilizers.Add(
            new HarvestFertilizerViewModel()
            {
                Amount = 5,
                Fertilizer = Fertilizer.HoneyL,
                MaxAmount = 8,
            }
        );
        Fertilizers.Add(
            new HarvestFertilizerViewModel()
            {
                Amount = 6,
                Fertilizer = Fertilizer.PlantL,
                MaxAmount = 8
            }
        );
    }
}
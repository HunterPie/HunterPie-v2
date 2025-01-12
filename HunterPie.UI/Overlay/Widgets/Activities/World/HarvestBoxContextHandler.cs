using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.World;
public class HarvestBoxContextHandler : IActivityContextHandler
{
    private const int MAX_FERTILIZER = 9;
    private readonly MHWContext _context;
    private readonly Dictionary<MHWFertilizer, HarvestFertilizerViewModel> _fertilizerViewModels;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;

    private readonly HarvestBoxViewModel _viewModel = new();

    public IActivity ViewModel => _viewModel;

    public HarvestBoxContextHandler(MHWContext context)
    {
        _context = context;
        _fertilizerViewModels = new(Player.HarvestBox.Fertilizers.Length);
    }

    public void HookEvents()
    {
        foreach (MHWFertilizer fertilizer in Player.HarvestBox.Fertilizers)
        {
            _fertilizerViewModels[fertilizer] = new()
            {
                Amount = fertilizer.Count,
                Fertilizer = fertilizer.Id,
                IsExpiring = fertilizer.Count <= 4,
                MaxAmount = MAX_FERTILIZER
            };

            fertilizer.OnCountChange += OnFertilizerCountChange;
            fertilizer.OnIdChange += OnFertilizerIdChange;
        }

        foreach (HarvestFertilizerViewModel vm in _fertilizerViewModels.Values)
            _viewModel.Fertilizers.Add(vm);

        Player.HarvestBox.OnItemsCountChange += OnHarvestBoxItemsCountChange;
    }

    public void UnhookEvents()
    {
        foreach (MHWFertilizer fertilizer in Player.HarvestBox.Fertilizers)
        {
            fertilizer.OnCountChange -= OnFertilizerCountChange;
            fertilizer.OnIdChange -= OnFertilizerIdChange;
        }

        _fertilizerViewModels.Clear();
        _viewModel.Fertilizers.Clear();

        Player.HarvestBox.OnItemsCountChange -= OnHarvestBoxItemsCountChange;
    }

    private void OnHarvestBoxItemsCountChange(object sender, MHWHarvestBox e)
    {
        _viewModel.Count = e.Count;
        _viewModel.MaxCount = 50;
    }

    private void OnFertilizerIdChange(object sender, MHWFertilizer e)
    {
        HarvestFertilizerViewModel vm = _fertilizerViewModels[e];

        vm.Fertilizer = e.Id;
    }

    private void OnFertilizerCountChange(object sender, MHWFertilizer e)
    {
        HarvestFertilizerViewModel vm = _fertilizerViewModels[e];

        vm.Amount = e.Count;
        vm.IsExpiring = e.Count <= 4;
        vm.MaxAmount = MAX_FERTILIZER;
    }
}
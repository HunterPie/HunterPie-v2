using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;
public class HarvestBoxController : IContextHandler
{
    private readonly MHWContext _context;
    private readonly Dictionary<MHWFertilizer, HarvestFertilizerViewModel> _fertilizerViewModels;
    private MHWPlayer Player => (MHWPlayer)_context.Game.Player;

    private readonly HarvestBoxViewModel _viewModel;

    public HarvestBoxController(
        MHWContext context,
        HarvestBoxViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;
        _fertilizerViewModels = new Dictionary<MHWFertilizer, HarvestFertilizerViewModel>(Player.HarvestBox.Fertilizers.Length);
    }

    public void HookEvents()
    {
        bool isFirst = true;
        foreach (MHWFertilizer fertilizer in Player.HarvestBox.Fertilizers)
        {
            _fertilizerViewModels[fertilizer] = new HarvestFertilizerViewModel
            {
                IsFirst = isFirst,
                Fertilizer = fertilizer.Id,
                IsExpiring = fertilizer.Count <= 4,
            };

            fertilizer.OnCountChange += OnFertilizerCountChange;
            fertilizer.OnIdChange += OnFertilizerIdChange;

            isFirst = false;
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
        vm.SetDays(e.Count);
    }

    private void OnFertilizerCountChange(object sender, MHWFertilizer e)
    {
        HarvestFertilizerViewModel vm = _fertilizerViewModels[e];

        vm.IsExpiring = e.Count <= 4;
        vm.SetDays(e.Count);
    }
}
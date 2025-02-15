using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System;

namespace HunterPie.Features.Debug;

internal class ActivitiesWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockActivitiesWidget)
            return;

        foreach (IActivitiesViewModel activities in new[] { SetupMHWorldActivities(), SetupMHRiseActivities() })
        {
            var config = new ActivitiesWidgetConfig();
            var view = new ActivitiesView(config);

            view.ViewModel.Activities = activities;
            view.ViewModel.InVisibleStage = true;

            _ = WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
                widget: view
            );
        }
    }

    #region Monster Hunter Rise

    private static IActivitiesViewModel SetupMHRiseActivities()
    {
        MHRiseActivitiesViewModel activities = DependencyContainer.Get<MHRiseActivitiesViewModel>();

        SetupSubmarines(activities.Submarines);
        SetupCohoot(activities.CohootNests);
        return activities;
    }

    private static void SetupSubmarines(SubmarinesViewModel viewModel)
    {
        viewModel.Submarines.Add(new SubmarineViewModel { Count = 15, DaysLeft = 4, IsActive = true, MaxCount = 20 });
        viewModel.Submarines.Add(new SubmarineViewModel { Count = 10, DaysLeft = 4, IsActive = true, MaxCount = 20 });
        viewModel.Submarines.Add(new SubmarineViewModel { Count = 20, DaysLeft = 4, IsActive = true, MaxCount = 20 });

        foreach (SubmarineViewModel submarine in viewModel.Submarines)
        {
            submarine.SetMaxBoosts(9);

            int count = (int)Random.Shared.NextInt64(9);
            int extras = (int)Random.Shared.NextInt64(9 - count);
            submarine.SetBoosts(count, extras);
        }
    }

    private static void SetupCohoot(CohootNestsViewModel viewModel)
    {
        var kamuraNest = new CohootNestViewModel { Name = "Kamura" };
        var elgadoNest = new CohootNestViewModel { Name = "Elgado" };
        kamuraNest.SetMaxItems(5);
        elgadoNest.SetMaxItems(5);
        kamuraNest.SetItems(3);
        elgadoNest.SetItems(5);
        viewModel.Nests.Add(kamuraNest);
        viewModel.Nests.Add(elgadoNest);
    }
    #endregion

    #region Monster Hunter World

    private static IActivitiesViewModel SetupMHWorldActivities()
    {
        MHWorldActivitiesViewModel activities = DependencyContainer.Get<MHWorldActivitiesViewModel>();

        SetupHarvestBox(activities.HarvestBox);
        SetupSteamworks(activities.Steamworks);

        return activities;
    }

    private static void SetupHarvestBox(HarvestBoxViewModel viewModel)
    {
        viewModel.Count = 20;
        viewModel.MaxCount = 50;
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.FungiL,
                IsExpiring = true,
                IsFirst = true,
            }.SetDays(3)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.GrowthL,
                IsExpiring = false,
            }.SetDays(8)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.HoneyL,
                IsExpiring = false,
            }.SetDays(9)
        );
        viewModel.Fertilizers.Add(
            item: new HarvestFertilizerViewModel
            {
                Fertilizer = Fertilizer.PlantL,
                IsExpiring = false,
            }.SetDays(7)
        );
    }

    private static void SetupSteamworks(SteamworksViewModel viewModel)
    {
        viewModel.MaxNaturalFuel = 700;
        viewModel.NaturalFuel = 584;
        viewModel.StoredFuel = 12_302;
    }

    #endregion
}
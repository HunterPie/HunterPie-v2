using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities;

internal class ActivitiesWidgetModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Monster Hunter World
        registry
            .WithService<HarvestBoxViewModel>()
            .WithService<TailraidersViewModel>()
            .WithService<SteamworksViewModel>()
            .WithService<ArgosyViewModel>()
            .WithService<MHWorldActivitiesViewModel>();

        // Monster Hunter Rise
        registry
            .WithService<CohootNestViewModel>()
            .WithService<CohootNestsViewModel>()
            .WithService<MeowcenariesViewModel>()
            .WithService<SubmarinesViewModel>()
            .WithService<TrainingDojoViewModel>()
            .WithService<MHRiseActivitiesViewModel>();
    }
}
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class MHWorldActivitiesViewModel : ViewModel, IActivitiesViewModel
{
    public Observable<bool> IsHarvestBoxEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsHarvestBoxEnabled;

    public Observable<bool> IsArgosyEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsArgosyEnabled;

    public Observable<bool> IsMeowmastersEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsMeowmastersEnabled;

    public Observable<bool> IsSteamworksEnabled { get; } =
        ClientConfig.Config.World.Overlay.ActivitiesWidget.IsSteamworksEnabled;


    public HarvestBoxViewModel HarvestBox { get; }
    public TailraidersViewModel Tailraiders { get; }
    public SteamworksViewModel Steamworks { get; }
    public ArgosyViewModel Argosy { get; }

    public MHWorldActivitiesViewModel(
        HarvestBoxViewModel harvestBox,
        TailraidersViewModel tailraiders,
        SteamworksViewModel steamworks,
        ArgosyViewModel argosy)
    {
        HarvestBox = harvestBox;
        Steamworks = steamworks;
        Argosy = argosy;
        Tailraiders = tailraiders;
    }

}
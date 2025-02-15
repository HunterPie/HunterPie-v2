using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MHRiseActivitiesViewModel : ViewModel, IActivitiesViewModel
{
    public CohootNestsViewModel CohootNests { get; }
    public MeowcenariesViewModel Meowcenaries { get; }
    public SubmarinesViewModel Submarines { get; }
    public TrainingDojoViewModel TrainingDojo { get; }

    public MHRiseActivitiesViewModel(
        CohootNestsViewModel cohootNests,
        MeowcenariesViewModel meowcenaries,
        SubmarinesViewModel submarines,
        TrainingDojoViewModel trainingDojo)
    {
        CohootNests = cohootNests;
        Meowcenaries = meowcenaries;
        Submarines = submarines;
        TrainingDojo = trainingDojo;
    }
}
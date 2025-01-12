using HunterPie.Core.Architecture;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class BooleanPropertyViewModel : ConfigurationPropertyViewModel
{
    public Observable<bool> Boolean { get; }

    public BooleanPropertyViewModel(Observable<bool> value)
    {
        Boolean = value;
    }
}
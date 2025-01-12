using HunterPie.Core.Architecture;

namespace HunterPie.UI.Settings.ViewModels.Internal;

#nullable enable
internal class StringPropertyViewModel : ConfigurationPropertyViewModel
{
    public Observable<string> String { get; }

    public StringPropertyViewModel(Observable<string> @string)
    {
        String = @string;
    }
}
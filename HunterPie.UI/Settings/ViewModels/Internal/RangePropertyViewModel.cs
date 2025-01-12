using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class RangePropertyViewModel : ConfigurationPropertyViewModel
{
    public Range Range { get; }

    public RangePropertyViewModel(Range range)
    {
        Range = range;
    }
}
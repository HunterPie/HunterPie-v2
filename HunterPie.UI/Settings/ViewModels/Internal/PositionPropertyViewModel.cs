using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class PositionPropertyViewModel : ConfigurationPropertyViewModel
{
    public Position Position { get; }

    public PositionPropertyViewModel(Position position)
    {
        Position = position;
    }
}
using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class ColorPropertyViewModel : ConfigurationPropertyViewModel
{
    public Color Color { get; }

    public ColorPropertyViewModel(Color color)
    {
        Color = color;
    }
}
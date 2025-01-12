using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class KeybindingPropertyViewModel : ConfigurationPropertyViewModel
{
    public Keybinding KeyBinding { get; }

    public KeybindingPropertyViewModel(Keybinding keyBinding)
    {
        KeyBinding = keyBinding;
    }
}
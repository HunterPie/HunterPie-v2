using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Keybinding : Bindable
{
    private string _keyCombo;

    public string KeyCombo
    {
        get => _keyCombo;
        set => SetValue(ref _keyCombo, value);
    }

    public static implicit operator string(Keybinding keybinding) => keybinding.KeyCombo;
    public static implicit operator Keybinding(string keybinding) => new() { KeyCombo = keybinding };
}
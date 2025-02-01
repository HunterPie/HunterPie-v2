using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for Keybinding.xaml
/// </summary>
public partial class Keybinding : UserControl
{
    public ObservableCollection<string> Keys { get; } = new();

    public string HotKey
    {
        get => (string)GetValue(HotKeyProperty);
        set => SetValue(HotKeyProperty, value);
    }

    public static readonly DependencyProperty HotKeyProperty =
        DependencyProperty.Register(nameof(HotKey), typeof(string), typeof(Keybinding));

    public Keybinding()
    {
        InitializeComponent();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;

        Key key = e.Key == Key.System ? e.SystemKey : e.Key;

        if (key is Key.LeftShift or Key.RightShift
            or Key.LeftCtrl or Key.RightCtrl
            or Key.LeftAlt or Key.RightAlt
            or Key.LWin or Key.RWin)
            return;

        Keys.Clear();

        if (key == Key.Delete)
        {
            Keys.Add("None");
            SetValue(HotKeyProperty, "None");
            return;
        }

        var shortcutText = new StringBuilder();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            Keys.Add("Ctrl");
            _ = shortcutText.Append("Ctrl+");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
        {
            Keys.Add("Shift");
            _ = shortcutText.Append("Shift+");
        }

        if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
        {
            Keys.Add("Alt");
            _ = shortcutText.Append("Alt+");
        }

        Keys.Add(key.ToString());
        _ = shortcutText.Append(key.ToString());
        SetValue(HotKeyProperty, shortcutText.ToString());
    }

    private void OnClick(object sender, EventArgs e) => Focus();

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Keys.Clear();

        if (HotKey is null)
            return;

        foreach (string key in HotKey.Split("+"))
            Keys.Add(key);
    }
}
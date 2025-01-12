using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for Switch.xaml
/// </summary>
public partial class Switch : ClickableControl
{
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool), typeof(Switch), new PropertyMetadata(false));

    public Switch()
    {
        InitializeComponent();
    }

    protected override void OnClickEvent()
    {
        base.OnClickEvent();

        IsActive = !IsActive;
    }
}
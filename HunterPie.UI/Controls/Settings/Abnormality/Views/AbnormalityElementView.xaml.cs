using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.UI.Controls.Settings.Abnormality.Views;
/// <summary>
/// Interaction logic for AbnormalityElementView.xaml
/// </summary>
public partial class AbnormalityElementView : ClickableControl
{
    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(AbnormalityElementView), new PropertyMetadata(false));


    public AbnormalityElementView()
    {
        InitializeComponent();
    }
}
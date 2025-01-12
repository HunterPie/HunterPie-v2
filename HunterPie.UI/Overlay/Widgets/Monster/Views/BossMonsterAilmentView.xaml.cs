using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

/// <summary>
/// Interaction logic for BossMonsterAilmentView.xaml
/// </summary>
public partial class BossMonsterAilmentView : UserControl
{
    public double Current
    {
        get => (double)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }
    public static readonly DependencyProperty CurrentProperty =
        DependencyProperty.Register("Current", typeof(double), typeof(BossMonsterAilmentView), new PropertyMetadata(0.0));

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }
    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register("Max", typeof(double), typeof(BossMonsterAilmentView), new PropertyMetadata(0.0));

    public BossMonsterAilmentView()
    {
        InitializeComponent();
    }
}
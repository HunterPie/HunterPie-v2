using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Activities.View;

/// <summary>
/// Interaction logic for TrainingDojoView.xaml
/// </summary>
public partial class TrainingDojoView : UserControl
{
    public bool ShouldShowBuddies
    {
        get => (bool)GetValue(ShouldShowBuddiesProperty);
        set => SetValue(ShouldShowBuddiesProperty, value);
    }

    public static readonly DependencyProperty ShouldShowBuddiesProperty =
        DependencyProperty.Register("ShouldShowBuddies", typeof(bool), typeof(TrainingDojoView), new PropertyMetadata(false));

    public TrainingDojoView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, EventArgs e) => ShouldShowBuddies = !ShouldShowBuddies;
}
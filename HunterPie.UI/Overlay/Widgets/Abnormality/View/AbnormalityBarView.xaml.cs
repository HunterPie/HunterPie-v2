using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.View;

/// <summary>
/// Interaction logic for AbnormalityBarView.xaml
/// </summary>
public partial class AbnormalityBarView : View<AbnormalityBarViewModel>, IWidget<AbnormalityWidgetConfig>, IWidgetWindow
{
    public string Title => Settings.Name;
    public AbnormalityWidgetConfig Settings { get; }
    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public AbnormalityBarView(ref AbnormalityWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();

    }

    private int frameCounter = 0;

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    private void OnRender(object sender, EventArgs e)
    {
        // Sort abnormalities every 60 frames
        if (frameCounter >= 60)
        {
            ViewModel.SortAbnormalities(Settings.SortByAlgorithm);
            frameCounter = 0;
        }

        frameCounter++;
    }

    private void OnLoad(object sender, RoutedEventArgs e) => CompositionTarget.Rendering += OnRender;

    private void OnUnload(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= OnRender;
}
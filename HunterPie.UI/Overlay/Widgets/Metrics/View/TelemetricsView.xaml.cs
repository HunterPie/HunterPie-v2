using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Metrics.View;

/// <summary>
/// Interaction logic for TelemetricsView.xaml
/// </summary>
public partial class TelemetricsView : View<TelemetricsViewModel>, IWidget<TelemetricsWidgetConfig>, IWidgetWindow
{
    public TelemetricsView(TelemetricsWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    public TelemetricsWidgetConfig Settings { get; }

    public string Title => "Debug Metrics";

    public WidgetType Type => WidgetType.Window;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    private void OnGCClick(object sender, EventArgs e) => ViewModel.ExecuteGarbageCollector();
}
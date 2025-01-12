using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Clock.Views;

/// <summary>
/// Interaction logic for ClockView.xaml
/// </summary>
public partial class ClockView : View<ClockViewModel>, IWidget<ClockWidgetConfig>, IWidgetWindow
{
    public readonly ClockWidgetConfig Config;

    public ClockView(ClockWidgetConfig config)
    {
        Config = config;
        InitializeComponent();
    }

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public string Title => "Clock Widget";

    public ClockWidgetConfig Settings => Config;

    public event EventHandler<WidgetType> OnWidgetTypeChange;
}
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.Views;

/// <summary>
/// Interaction logic for ActivitiesView.xaml
/// </summary>
public partial class ActivitiesView : View<ActivitiesViewModel>, IWidget<ActivitiesWidgetConfig>, IWidgetWindow
{
    public ActivitiesView(ActivitiesWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    public WidgetType Type => WidgetType.ClickThrough;

    public string Title => "Activities Widget";

    public ActivitiesWidgetConfig Settings { get; }
    IWidgetSettings IWidgetWindow.Settings => Settings;

    public event EventHandler<WidgetType> OnWidgetTypeChange;
}
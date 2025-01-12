using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.View;

/// <summary>
/// Interaction logic for ActivitiesView.xaml
/// </summary>
public partial class ActivitiesView : View<ActivitiesViewModel>, IWidget<ActivitiesWidgetConfig>, IWidgetWindow
{
    public readonly ActivitiesWidgetConfig _config;

    public ActivitiesView(ActivitiesWidgetConfig config)
    {
        _config = config;
        InitializeComponent();
    }

    public WidgetType Type => WidgetType.ClickThrough;

    public string Title => "Activities Widget";

    public ActivitiesWidgetConfig Settings => _config;
    IWidgetSettings IWidgetWindow.Settings => Settings;

    public event EventHandler<WidgetType> OnWidgetTypeChange;
}
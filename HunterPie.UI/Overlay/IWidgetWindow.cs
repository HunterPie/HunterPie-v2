using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using System;

namespace HunterPie.UI.Overlay;

#nullable enable
public interface IWidgetWindow
{
    public WidgetType Type { get; }
    public IWidgetSettings? Settings { get; }

    public event EventHandler<WidgetType> OnWidgetTypeChange;
}
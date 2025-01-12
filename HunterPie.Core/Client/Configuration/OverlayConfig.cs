using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using System;

namespace HunterPie.Core.Client.Configuration;

public class OverlayConfig : ISettings
{
    public virtual PlayerHudWidgetConfig PlayerHudWidget { get; set; } = new();
    public virtual MonsterWidgetConfig BossesWidget { get; set; } = new();
    public virtual AbnormalityTrayConfig AbnormalityTray { get; set; } = new();
    public virtual DamageMeterWidgetConfig DamageMeterWidget { get; set; } = new();
    public virtual TelemetricsWidgetConfig DebugWidget { get; set; } = new();
    public virtual WirebugWidgetConfig WirebugWidget { get; set; } = new();
    public virtual ActivitiesWidgetConfig ActivitiesWidget { get; set; } = new();
    public virtual InsectGlaiveWidgetConfig InsectGlaiveWidget { get; set; } = new();
    public virtual ChargeBladeWidgetConfig ChargeBladeWidget { get; set; } = new();
    public virtual DualBladesWidgetConfig DualBladesWidget { get; set; } = new();
    public virtual SwitchAxeWidgetConfig SwitchAxeWidget { get; set; } = new();
    public virtual LongSwordWidgetConfig LongSwordWidget { get; set; } = new();
    public virtual ChatWidgetConfig ChatWidget { get; set; } = new();
    public virtual ClockWidgetConfig ClockWidget { get; set; } = new();
    public virtual SpecializedToolWidgetConfig PrimarySpecializedToolWidget { get; set; } = new()
    {
        Position = new(950, 400)
    };
    public virtual SpecializedToolWidgetConfig SecondarySpecializedToolWidget { get; set; } = new()
    {
        Position = new(1000, 375),
        Scale = new(0.9, 2, 0.1, 0.1)
    };

    [Obsolete]
    public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";

    [Obsolete]
    public Observable<bool> HideWhenUnfocus { get; set; } = false;
}
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Animation;
using HunterPie.UI.Overlay.Controls;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ResourcesService = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.Overlay.Widgets.Player.Views;

/// <summary>
/// Interaction logic for PlayerHudView.xaml
/// </summary>
public partial class PlayerHudView : View<PlayerHudViewModel>, IWidget<PlayerHudWidgetConfig>, IWidgetWindow
{
    private readonly BrushAnimation _currentHealthBarAnimation = new BrushAnimation();
    private readonly BrushAnimation _currentStaminaBarAnimation = new BrushAnimation();
    private readonly BrushAnimation _currentHealthRecoverableAnimation = new BrushAnimation();

    private readonly Brush _defaultHealthBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FOREGROUND");
    private readonly Brush _defaultStaminaBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_STAMINA_FOREGROUND");
    private readonly Brush _defaultRecoverableBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_RECOVERABLE_FOREGROUND");

    private readonly AbnormalityCategory[] _healthCategoriesPriority = { AbnormalityCategory.Effluvia, AbnormalityCategory.Poison, AbnormalityCategory.Fire, AbnormalityCategory.Bleed };
    private readonly AbnormalityCategory[] _staminaCategoriesPriority = { AbnormalityCategory.Water, AbnormalityCategory.Ice };
    private readonly AbnormalityCategory[] _recoverableCategoriesPriority = { AbnormalityCategory.NaturalHealing };

    private readonly Dictionary<AbnormalityCategory, Brush> _abnormalityColors = new Dictionary<AbnormalityCategory, Brush>()
    {
        { AbnormalityCategory.Fire, ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FIRE_FOREGROUND") },
        { AbnormalityCategory.Poison, ResourcesService.Get<Brush>("WIDGET_PLAYER_POISON_FOREGROUND") },
        { AbnormalityCategory.Bleed, ResourcesService.Get<Brush>("WIDGET_PLAYER_BLEED_FOREGROUND") },
        { AbnormalityCategory.Effluvia, ResourcesService.Get<Brush>("WIDGET_PLAYER_EFFLUVIA_FOREGROUND") },
        { AbnormalityCategory.Ice, ResourcesService.Get<Brush>("WIDGET_PLAYER_ICE_FOREGROUND") },
        { AbnormalityCategory.Water, ResourcesService.Get<Brush>("WIDGET_PLAYER_WATER_FOREGROUND") },
        { AbnormalityCategory.NaturalHealing, ResourcesService.Get<Brush>("WIDGET_PLAYER_NATURAL_HEAL_FOREGROUND") },
    };

    public PlayerHudWidgetConfig Settings { get; }

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public string Title => "Player Widget";

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    public PlayerHudView()
    {
        InitializeComponent();
    }

    public PlayerHudView(PlayerHudWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    protected override void Initialize()
    {
        HookEvents();
    }

    private void HookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged += OnActiveAbnormalitiesCollectionChanged;
    }

    private void UnhookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged -= OnActiveAbnormalitiesCollectionChanged;
    }

    private void ResetAnimation(Bar bar, BrushAnimation animation, Brush defaultBrush) =>
        bar.BeginAnimation(Bar.ForegroundProperty, BuildBarAnimation(animation, defaultBrush), HandoffBehavior.SnapshotAndReplace);

    private void AnimateBar(Bar bar, BrushAnimation animation, Brush brush)
    {
        if (animation.To == brush)
            return;

        bar.BeginAnimation(
            Bar.ForegroundProperty,
            BuildBarAnimation(animation, brush),
            HandoffBehavior.SnapshotAndReplace
        );
    }

    private void HandleAbnormalityCategoryChange() =>
        UIThread.Invoke(() =>
        {

            var categories = ViewModel.ActiveAbnormalities.ToHashSet();

            AbnormalityCategory healthAbnormality = categories.FindPriority(_healthCategoriesPriority);
            AbnormalityCategory staminaAbnormality = categories.FindPriority(_staminaCategoriesPriority);
            AbnormalityCategory recoverableAbnormality = categories.FindPriority(_recoverableCategoriesPriority);

            if (healthAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_HealthBar, _currentHealthBarAnimation, _defaultHealthBrush);
            else
                AnimateBar(PART_HealthBar, _currentHealthBarAnimation, _abnormalityColors[healthAbnormality]);

            if (staminaAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_StaminaBar, _currentStaminaBarAnimation, _defaultStaminaBrush);
            else
                AnimateBar(PART_StaminaBar, _currentStaminaBarAnimation, _abnormalityColors[staminaAbnormality]);

            if (recoverableAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_RecoverableHealthBar, _currentHealthRecoverableAnimation, _defaultRecoverableBrush);
            else
                AnimateBar(PART_RecoverableHealthBar, _currentHealthRecoverableAnimation, _abnormalityColors[recoverableAbnormality]);

        });

    private void OnActiveAbnormalitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => HandleAbnormalityCategoryChange();

    private BrushAnimation BuildBarAnimation(BrushAnimation animation, Brush brush)
    {
        animation.To = brush;
        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

        return animation;
    }

    public override void Dispose()
    {
        UnhookEvents();

        base.Dispose();
    }
}
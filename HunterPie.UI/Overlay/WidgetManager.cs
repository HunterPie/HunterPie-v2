using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Input;
using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Components;
using System;
using System.Collections.Generic;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.UI.Overlay;

#nullable enable
public class WidgetManager : Bindable
{
    private IContext? _context;
    private bool _isDesignModeEnabled;
    private bool _isGameFocused;
    private bool _isGameHudOpen;
    private readonly Dictionary<IWidgetWindow, WidgetBase> _widgets = new();

    public bool IsDesignModeEnabled { get => _isDesignModeEnabled; private set => SetValue(ref _isDesignModeEnabled, value); }
    public bool IsGameFocused { get => _isGameFocused; private set => SetValue(ref _isGameFocused, value); }
    public bool IsGameHudOpen { get => _isGameHudOpen; private set => SetValue(ref _isGameHudOpen, value); }
    public ref readonly Dictionary<IWidgetWindow, WidgetBase> Widgets => ref _widgets;
    public OverlayClientConfig Settings => ClientConfig.Config.Overlay;

    private static WidgetManager? _instance;

    public static WidgetManager Instance
    {
        get
        {
            _instance ??= new WidgetManager();

            return _instance;
        }
    }

    private WidgetManager()
    {
        _ = Hotkey.Register(Settings.ToggleDesignMode, ToggleDesignMode);
        _ = Hotkey.Register(Settings.ToggleVisibility, ToggleVisibility);
    }

    internal static void Hook(IContext context)
    {
        Instance._context = context;
        context.Process.Focus += OnGameFocus;
        context.Process.Blur += OnGameBlur;
        context.Game.OnHudStateChange += OnHudStateChange;
    }

    private static void OnHudStateChange(object? sender, IGame e) => Instance.IsGameHudOpen = e.IsHudOpen;

    private static void OnGameBlur(object? sender, EventArgs e) => Instance.IsGameFocused = false;

    private static void OnGameFocus(object? sender, EventArgs e) => Instance.IsGameFocused = true;

    public static bool Register<T, TK>(T widget) where T : IWidgetWindow, IWidget<TK>
                                                where TK : IWidgetSettings
    {
        if (Instance.Widgets.ContainsKey(widget))
            return false;

        var wnd = new WidgetBase() { Widget = widget };
        Instance._widgets.Add(widget, wnd);
        wnd.Show();

        Log.Debug($"Added new widget: {widget.Title}");

        return true;
    }

    public static bool Unregister<T, TK>(T widget) where T : IWidgetWindow, IWidget<TK>
                                                  where TK : IWidgetSettings
    {
        if (!Instance.Widgets.ContainsKey(widget))
            return false;

        WidgetBase wnd = Instance.Widgets[widget];
        wnd.Close();

        return Instance._widgets.Remove(widget);
    }

    internal static void Dispose()
    {
        if (Instance._context is null)
            return;

        Instance._context.Process.Focus -= OnGameFocus;
        Instance._context.Process.Blur -= OnGameBlur;
        Instance._context.Game.OnHudStateChange -= OnHudStateChange;
        Instance._context = null;
    }

    private void ToggleDesignMode()
    {
        IsDesignModeEnabled = !IsDesignModeEnabled;

        foreach (WidgetBase widget in Widgets.Values)
            widget.HandleTransparencyFlag(!IsDesignModeEnabled);
    }

    private void ToggleVisibility()
    {
        Settings.IsEnabled.Value = !Settings.IsEnabled;
    }
}
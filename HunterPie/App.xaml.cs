using HunterPie.Core.Client;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Observability.Logging;
using HunterPie.DI;
using HunterPie.Features.Debug;
using HunterPie.Internal;
using HunterPie.Internal.Tray;
using HunterPie.Platforms;
using HunterPie.UI.Main.Views;
using HunterPie.Usecases;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HunterPie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private static MainView Window => DependencyContainer.Get<MainView>();
    private static MainApplication MainApplication => DependencyContainer.Get<MainApplication>();

    protected override async void OnStartup(StartupEventArgs e)
    {
        CheckForRunningInstances();

        base.OnStartup(e);

        await InitializerManager.InitializeCore();

        SupportedPlatformUseCase.Execute();

        DependencyProvider.LoadModules();
        await InitializerManager.InitializeAsync();

        ShutdownMode = ShutdownMode.OnMainWindowClose;

        CheckIfHunterPiePathIsSafe();
        SetupFrameRate();
        InitializeMainView();
        SetupTrayIcon();

        InitializerManager.InitializeGUI();
        DebugWidgets.MockIfNeeded();

        SetUiThreadPriority();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        InitializerManager.Unload();
        MainApplication.Dispose();
    }

    private void CheckIfHunterPiePathIsSafe()
    {
        bool isSafe = VerifyHunterPiePathUseCase.Invoke();

        if (isSafe)
            return;

        DialogManager.Warn(
            "Unsafe path",
            "It looks like you're executing HunterPie directly from the zip file. Please extract it first before running the client.",
            NativeDialogButtons.Accept
        );

        Shutdown();
    }

    private void SetupFrameRate()
    {
        Timeline.DesiredFrameRateProperty.OverrideMetadata(
            forType: typeof(Timeline),
            typeMetadata: new FrameworkPropertyMetadata { DefaultValue = (int)ClientConfig.Config.Client.RenderFramePerSecond.Current }
        );
    }

    private async void InitializeMainView()
    {
        _logger.Info("Initializing HunterPie client UI");

        await MainApplication.Start();

        MainWindow = Window;

        if (ClientConfig.Config.Client.EnableSeamlessStartup)
            return;

        Window.Show();
    }

    private void CheckForRunningInstances()
    {
        Process[] processes = Process.GetProcessesByName("HunterPie")
            .Where(p => p.Id != Environment.ProcessId
                    && p.MainModule?.FileName == ClientInfo.ClientFileName)
            .ToArray();

        foreach (Process process in processes)
            process.Kill();
    }

    private void SetUiThreadPriority() => Dispatcher.Thread.Priority = ThreadPriority.Highest;

    private void OnTrayShowClick(object? sender, EventArgs e)
    {
        Window.Show();
        Window.WindowState = WindowState.Normal;
        Window.Focus();
    }

    private void OnTrayClockClick(object? sender, EventArgs e)
    {
        Window.Close();
    }

    private void SetupTrayIcon()
    {
        TrayService.AddDoubleClickHandler(OnTrayShowClick);

        TrayService.AddItem("Show")
            .Click += OnTrayShowClick;

        TrayService.AddItem("Close")
            .Click += OnTrayClockClick;
    }

    private async void OnUiException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        await MainApplication.SendUiException(e.Exception);
    }

    public static async void Restart()
    {
        await Window.Dispatcher.InvokeAsync(Window.Hide);

        await MainApplication.Restart();

        Current.Shutdown();
    }
}
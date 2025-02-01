using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Internal;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Core.Observability.Logging;
using HunterPie.Platforms.Windows.Api.Kernel;
using HunterPie.Platforms.Windows.Memory;
using System.ComponentModel;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Platforms.Windows.Process;

internal class WindowsProcessWatcher : IControllableWatcherService, IEventDispatcher, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private int _isWatching;
    private Timer? _timer;
    private readonly IProcessAttachStrategy[] _strategies;
    private readonly HashSet<string> _failedProcesses = new();

    private WindowsGameProcess? _currentProcess;
    public WindowsGameProcess? CurrentProcess
    {
        get => _currentProcess;
        private set
        {
            if (value == _currentProcess)
                return;

            _currentProcess = value;

            if (value is { })
                this.Dispatch(
                    toDispatch: ProcessStart,
                    data: new ProcessEventArgs
                    {
                        Game = value
                    });
            else
                this.Dispatch(
                    toDispatch: ProcessExit,
                    data: EventArgs.Empty);
        }
    }

    public event EventHandler<ProcessEventArgs>? ProcessStart;
    public event EventHandler<EventArgs>? ProcessExit;

    public WindowsProcessWatcher(IProcessAttachStrategy[] strategies)
    {

        _strategies = strategies;
    }

    public void Start()
    {
        foreach (IProcessAttachStrategy strategy in _strategies)
        {
            _logger.Info($"Waiting for process '{strategy.Name}' to start...");
            strategy.SetStatus(ProcessStatus.Waiting);
        }

        _timer = new Timer(
            callback: Watch,
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromMilliseconds(150)
        );
    }

    private async void Watch(object? _)
    {
        if (Interlocked.Exchange(ref _isWatching, 1) == 1)
            return;

        try
        {
            if (CurrentProcess?.SystemProcess is { HasExited: true })
            {
                CurrentProcess = null;
                return;
            }

            if (CurrentProcess is { } current)
            {
                _strategies
                    .Where(it => it.Status != ProcessStatus.Hooked && it.Status != ProcessStatus.Paused)
                    .ForEach(it => it.SetStatus(ProcessStatus.Paused));

                await current.UpdateAsync();
                return;
            }

            Task[] tasks = _strategies
                .Where(strategy => !_failedProcesses.Contains(strategy.Name))
                .Select(strategy =>
                    Task.Run(() => FindAndAttach(strategy))
                ).ToArray();

            Task.WaitAll(tasks);
        }
        finally
        {
            Interlocked.Exchange(ref _isWatching, 0);
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private void FindAndAttach(IProcessAttachStrategy strategy)
    {
        strategy.SetStatus(ProcessStatus.Waiting);

        SystemProcess? process = SystemProcess.GetProcessesByName(strategy.Name)
            .FirstOrDefault(it => !string.IsNullOrEmpty(it.MainWindowTitle));

        if (process is not { })
            return;

        try
        {
            if (!strategy.CanAttach(process))
                return;

            CurrentProcess = AttachToGame(strategy, process);
        }
        catch (Exception err)
        {
            _logger.Error("Failed to open game process. Run HunterPie as Administrator!");
            _logger.Info($"Error details: {err}");

            _failedProcesses.Add(strategy.Name);
            process.Dispose();
        }
    }

    private static WindowsGameProcess AttachToGame(
        IProcessAttachStrategy strategy,
        SystemProcess process)
    {
        if (process.MainModule is null)
            throw new InvalidOperationException("Process main module is null");

        IntPtr handle = Kernel32.OpenProcess(
            dwDesiredAccess: Kernel32.PROCESS_ALL_ACCESS,
            bInheritHandle: false,
            dwProcessId: process.Id
        );

        if (handle == IntPtr.Zero)
            throw new Win32Exception("Failed to attach to process, missing permissions");

        strategy.SetStatus(ProcessStatus.Hooked);

        AddressMap.Add("BASE", process.MainModule.BaseAddress);

        return new WindowsGameProcess
        {
            SystemProcess = process,
            Handle = handle,
            Name = strategy.Name,
            Type = strategy.Game,
            Memory = new WindowsMemory(handle)
        };
    }
}
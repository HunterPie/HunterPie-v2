using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Core.Logger;
using HunterPie.Platforms.Windows.Api.Kernel;
using HunterPie.Platforms.Windows.Memory;
using System.ComponentModel;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Platforms.Windows.Process;

internal class WindowsProcessWatcher : IProcessWatcherService, IEventDispatcher, IDisposable
{
    private readonly Timer _timer;
    private readonly IProcessAttachStrategy[] _strategies;
    private readonly HashSet<string> _failedProcesses;

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
        _failedProcesses = new();
        _timer = new Timer(
            callback: Watch,
            state: null,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromMilliseconds(150)
        );
        _strategies = strategies;
    }

    public async void Watch(object? _)
    {
        if (CurrentProcess?.SystemProcess is { HasExited: true })
        {
            CurrentProcess = null;
            return;
        }

        if (CurrentProcess is { } current)
        {
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

    public void Dispose()
    {
        _timer.Dispose();
    }

    private void FindAndAttach(IProcessAttachStrategy strategy)
    {
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
            Log.Error("Failed to open game process. Run HunterPie as Administrator!");
            Log.Info("Error details: {0}", err);

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
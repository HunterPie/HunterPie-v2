using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Platforms.Windows.Api.User;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Platforms.Windows.Process;

internal class WindowsGameProcess : IGameProcess, IDisposable
{
    public required SystemProcess SystemProcess { get; init; }
    public required IntPtr Handle { get; init; }
    public required string Name { get; init; }
    public required GameProcessType Type { get; init; }
    public required IMemoryAsync Memory { get; init; }

    public event EventHandler<EventArgs>? Focus;
    public event EventHandler<EventArgs>? Blur;

    public async Task UpdateAsync()
    {
        IntPtr foregroundWindowHandle = await Task.Run(User32.GetForegroundWindow);

        EventHandler<EventArgs>? eventToDispatch = (foregroundWindowHandle == SystemProcess.MainWindowHandle) switch
        {
            true => Focus,
            _ => Blur
        };

        this.Dispatch(
            toDispatch: eventToDispatch,
            data: EventArgs.Empty
        );
    }

    public void Dispose() => SystemProcess.Dispose();
}
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Memory;
using System;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Core.Domain.Process.Entity;

public interface IGameProcess : IEventDispatcher
{
    public SystemProcess SystemProcess { get; }
    public string Name { get; }
    public GameProcessType Type { get; }
    public IMemoryAsync Memory { get; }

    public event EventHandler<EventArgs>? Focus;
    public event EventHandler<EventArgs>? Blur;
}
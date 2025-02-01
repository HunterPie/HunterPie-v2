using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Domain.Process.Service;

public interface IGameWatcher : IEventDispatcher
{
    public string Name { get; }

    public GameProcessType Game { get; }

    public ProcessStatus Status { get; }

    public event EventHandler<SimpleValueChangeEventArgs<ProcessStatus>> StatusChange;
}
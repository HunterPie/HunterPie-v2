using HunterPie.Core.Domain.Process.Events;
using System;

namespace HunterPie.Core.Domain.Process.Service;

public interface IProcessWatcherService
{
    public event EventHandler<ProcessEventArgs> ProcessStart;
    public event EventHandler<EventArgs> ProcessExit;
}
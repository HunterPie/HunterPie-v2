using HunterPie.Core.Domain.Process.Entity;
using System;

namespace HunterPie.Core.Domain.Process.Events;

public class ProcessEventArgs : EventArgs
{
    public required IGameProcess Game { get; init; }
}
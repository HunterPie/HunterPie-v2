using HunterPie.Core.Game.Entity.Enemy;
using System;

namespace HunterPie.Core.Game.Services.Monster.Events;

#nullable enable
public class InferTargetChangedEventArgs : EventArgs
{
    public IMonster? Target { get; init; }

    public InferTargetChangedEventArgs(IMonster? target)
    {
        Target = target;
    }
}
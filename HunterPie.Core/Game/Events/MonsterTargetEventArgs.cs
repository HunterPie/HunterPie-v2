using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class MonsterTargetEventArgs : EventArgs
{
    public Target LockOnTarget { get; init; }
    public Target ManualTarget { get; init; }

    public MonsterTargetEventArgs(IMonster monster)
    {
        LockOnTarget = monster.Target;
        ManualTarget = monster.ManualTarget;
    }
}
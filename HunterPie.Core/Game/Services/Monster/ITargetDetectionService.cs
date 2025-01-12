using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Services.Monster.Events;
using System;

namespace HunterPie.Core.Game.Services.Monster;

#nullable enable
public interface ITargetDetectionService
{
    public IMonster? Target { get; }

    public event EventHandler<InferTargetChangedEventArgs> OnTargetChanged;

    public Target Infer(IMonster monster);
}
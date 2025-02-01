using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player.Weapons;
public abstract class CommonMeleeWeapon : Scannable, IWeapon, IMeleeWeapon, IEventDispatcher, IDisposable
{
    public abstract Weapon Id { get; }
    public abstract Sharpness Sharpness { get; protected set; }
    public abstract int CurrentSharpness { get; protected set; }
    public abstract int MaxSharpness { get; protected set; }
    public abstract int Threshold { get; protected set; }
    public abstract int[]? SharpnessThresholds { get; protected set; }

    protected readonly SmartEvent<SharpnessEventArgs> _onSharpnessChange = new();
    public event EventHandler<SharpnessEventArgs> OnSharpnessChange
    {
        add => _onSharpnessChange.Hook(value);
        remove => _onSharpnessChange.Unhook(value);
    }

    protected readonly SmartEvent<SharpnessEventArgs> _onSharpnessLevelChange = new();
    public event EventHandler<SharpnessEventArgs> OnSharpnessLevelChange
    {
        add => _onSharpnessLevelChange.Hook(value);
        remove => _onSharpnessLevelChange.Unhook(value);
    }

    protected CommonMeleeWeapon(
        IGameProcess process,
        IScanService scanService) : base(process, scanService) { }

    public override void Dispose()
    {
        base.Dispose();
        _onSharpnessChange.Dispose();
        _onSharpnessLevelChange.Dispose();
    }

    protected static int CalculateCurrentThreshold(Sharpness currentLevel, IReadOnlyList<int> thresholds)
    {
        Sharpness previousLevel = currentLevel - 1;

        return previousLevel <= Sharpness.Broken ? 0 : thresholds[(int)previousLevel];
    }

    protected static int CalculateMaxThreshold(Sharpness currentLevel, IReadOnlyList<int> thresholds, int maxHits)
    {
        int nextLevel = (int)currentLevel + 1;

        if (nextLevel > (int)Sharpness.Purple || nextLevel >= thresholds.Count || thresholds[nextLevel] == 0)
            return maxHits;

        return thresholds[(int)currentLevel];
    }
}
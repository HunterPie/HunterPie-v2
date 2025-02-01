using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;

public class MHRLongSword : MHRMeleeWeapon, ILongSword
{
    private int _spiritLevel;
    public int SpiritLevel
    {
        get => _spiritLevel;
        private set
        {
            if (_spiritLevel == value)
                return;

            int temp = _spiritLevel;
            _spiritLevel = value;
            this.Dispatch(_onSpiritLevelChange, new SimpleValueChangeEventArgs<int>(temp, value));
        }
    }

    private float _spiritBuildUp;
    public float SpiritBuildUp
    {
        get => _spiritBuildUp;
        private set
        {
            if (_spiritBuildUp.Equals(value))
                return;

            _spiritBuildUp = value;
            this.Dispatch(_onSpiritBuildUpChange, new BuildUpChangeEventArgs(value, MaxSpiritBuildUp));
        }
    }

    public float MaxSpiritBuildUp => 100.0f;

    private float _spiritRegenerationTimer;
    public float SpiritRegenerationTimer
    {
        get => _spiritRegenerationTimer;
        private set
        {
            if (_spiritRegenerationTimer.Equals(value))
                return;

            _spiritRegenerationTimer = value;
            this.Dispatch(_onSpiritRegenerationChange, new TimerChangeEventArgs(value, MaxSpiritRegenerationTimer));
        }
    }

    public float MaxSpiritRegenerationTimer { get; private set; }

    private float _spiritLevelTimer;
    public float SpiritLevelTimer
    {
        get => _spiritLevelTimer;
        private set
        {
            if (_spiritLevelTimer.Equals(value))
                return;

            _spiritLevelTimer = value;
            this.Dispatch(_onSpiritLevelTimerChange, new TimerChangeEventArgs(value, MaxSpiritLevelTimer));
        }
    }

    public float MaxSpiritLevelTimer { get; private set; }

    #region Events
    private readonly SmartEvent<SimpleValueChangeEventArgs<int>> _onSpiritLevelChange = new();
    public event EventHandler<SimpleValueChangeEventArgs<int>> OnSpiritLevelChange
    {
        add => _onSpiritLevelChange.Hook(value);
        remove => _onSpiritLevelChange.Unhook(value);
    }

    private readonly SmartEvent<BuildUpChangeEventArgs> _onSpiritBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnSpiritBuildUpChange
    {
        add => _onSpiritBuildUpChange.Hook(value);
        remove => _onSpiritBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onSpiritRegenerationChange = new();
    public event EventHandler<TimerChangeEventArgs> OnSpiritRegenerationChange
    {
        add => _onSpiritRegenerationChange.Hook(value);
        remove => _onSpiritRegenerationChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onSpiritLevelTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnSpiritLevelTimerChange
    {
        add => _onSpiritLevelTimerChange.Hook(value);
        remove => _onSpiritLevelTimerChange.Unhook(value);
    }
    #endregion

    public MHRLongSword(
        IGameProcess process,
        IScanService scanService) : base(process, scanService, Weapon.Longsword)
    {
    }

    [ScannableMethod]
    private async Task GetData()
    {
        MHRLongSwordStructure structure = await Memory.DerefAsync<MHRLongSwordStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("CURRENT_WEAPON_OFFSETS")
        );

        float[] maxTimersByLevel = await Memory.ReadArraySafeAsync<float>(
            address: structure.LevelMaxTimersPointer,
            size: 4
        );
        float maxLevelTimer = maxTimersByLevel.ElementAtOrDefault(
            index: structure.Level
        ).ToAbnormalitySeconds();
        float currentLevelTimer = structure.LevelTimer.ToAbnormalitySeconds();
        float regenerationTimer = structure.BuildUpBuffTimer.ToAbnormalitySeconds();

        SpiritBuildUp = structure.GaugeBuildUp;
        MaxSpiritLevelTimer = maxLevelTimer;
        SpiritLevelTimer = currentLevelTimer;
        SpiritLevel = structure.Level;
        MaxSpiritRegenerationTimer = Math.Max(regenerationTimer, MaxSpiritRegenerationTimer);
        SpiritRegenerationTimer = regenerationTimer;
    }

    public override void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onSpiritLevelChange,
            _onSpiritBuildUpChange,
            _onSpiritRegenerationChange,
            _onSpiritLevelTimerChange
        );

        base.Dispose();
    }
}
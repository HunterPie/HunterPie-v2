using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;

public sealed class MHWLongSword : MHWMeleeWeapon, ILongSword
{
    private readonly ISkillService _skillService;
    private readonly float[] _maxSpiritLevelTimers = { 0.0f, 200.0f, 140.0f, 70.0f };

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

    private bool _isIaiSlashTimerActive;

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

    public MHWLongSword(
        IGameProcess process,
        ISkillService skillService) : base(process, skillService, Weapon.Longsword)
    {
        _skillService = skillService;
    }

    [ScannableMethod]
    private async Task GetData()
    {
        MHWLongSwordStructure structure = await Memory.DerefAsync<MHWLongSwordStructure>(
            address: AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_MECHANICS_OFFSETS")
        );

        if (structure.SpiritLevel is < 0 or > 3)
            return;

        float powerProlongerMultiplier = _skillService.GetPowerProlongerMultiplier(Weapon.Longsword);

        // Both Iai Slash and Helm Breaker have the same functionality in game
        // so we only track the highest timer
        bool wasIaiSlashTimerActive = _isIaiSlashTimerActive;
        _isIaiSlashTimerActive = structure.IaiSlashSpiritRegenTimer > structure.HelmBreakerSpiritRegenTimer;
        bool shouldResetMaxRegenTimer = wasIaiSlashTimerActive != _isIaiSlashTimerActive;

        float spiritRegenTimer = _isIaiSlashTimerActive switch
        {
            true => structure.IaiSlashSpiritRegenTimer,
            _ => structure.HelmBreakerSpiritRegenTimer
        };

        MaxSpiritRegenerationTimer = shouldResetMaxRegenTimer switch
        {
            true => spiritRegenTimer,
            _ => Math.Max(MaxSpiritRegenerationTimer, spiritRegenTimer)
        };
        SpiritBuildUp = structure.BuildUp * 100.0f;
        SpiritLevel = structure.SpiritLevel;
        SpiritRegenerationTimer = spiritRegenTimer;

        float maxSpiritLevelTimer = _maxSpiritLevelTimers.ElementAtOrDefault(structure.SpiritLevel) * powerProlongerMultiplier;
        MaxSpiritLevelTimer = maxSpiritLevelTimer;
        SpiritLevelTimer = structure.SpiritLevelTimer * maxSpiritLevelTimer;
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
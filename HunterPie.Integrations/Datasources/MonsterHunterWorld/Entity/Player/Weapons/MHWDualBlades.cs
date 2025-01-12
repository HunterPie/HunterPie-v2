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

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;

public sealed class MHWDualBlades : MHWMeleeWeapon, IDualBlades
{
    private bool _isDemonMode;
    private bool _isArchDemonMode;
    private float _demonBuildUp;

    public bool IsDemonMode
    {
        get => _isDemonMode;
        private set
        {
            if (value == _isDemonMode)
                return;

            _isDemonMode = value;
            this.Dispatch(_onDemonModeStateChange, new StateChangeEventArgs<bool>(value, !value));
        }
    }

    public bool IsArchDemonMode
    {
        get => _isArchDemonMode;
        private set
        {
            if (value == _isArchDemonMode)
                return;

            _isArchDemonMode = value;
            this.Dispatch(_onArchDemonModeStateChange, new StateChangeEventArgs<bool>(value, !value));
        }
    }

    public float DemonBuildUp
    {
        get => _demonBuildUp;
        private set
        {
            if (value == _demonBuildUp)
                return;

            _demonBuildUp = value;
            this.Dispatch(_onDemonBuildUpChange, new BuildUpChangeEventArgs(value, MaxDemonBuildUp));
        }
    }

    public float MaxDemonBuildUp => 100.0f;

    public float PiercingBindTimer => 0.0f;

    public float MaxPiercingBindTimer => 0.0f;

    private readonly SmartEvent<StateChangeEventArgs<bool>> _onDemonModeStateChange = new();
    public event EventHandler<StateChangeEventArgs<bool>> OnDemonModeStateChange
    {
        add => _onDemonModeStateChange.Hook(value);
        remove => _onDemonModeStateChange.Unhook(value);
    }

    private readonly SmartEvent<StateChangeEventArgs<bool>> _onArchDemonModeStateChange = new();
    public event EventHandler<StateChangeEventArgs<bool>> OnArchDemonModeStateChange
    {
        add => _onArchDemonModeStateChange.Hook(value);
        remove => _onArchDemonModeStateChange.Unhook(value);
    }

    private readonly SmartEvent<BuildUpChangeEventArgs> _onDemonBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnDemonBuildUpChange
    {
        add => _onDemonBuildUpChange.Hook(value);
        remove => _onDemonBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onPiercingBindTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnPiercingBindTimerChange
    {
        add => _onPiercingBindTimerChange.Hook(value);
        remove => _onPiercingBindTimerChange.Unhook(value);
    }

    public MHWDualBlades(
        IGameProcess process,
        ISkillService skillService) : base(process, skillService, Weapon.DualBlades) { }

    [ScannableMethod]
    private async Task GetData()
    {
        MHWDualBladesStructure structure = await Memory.DerefAsync<MHWDualBladesStructure>(
            address: AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_MECHANICS_OFFSETS")
        );

        IsDemonMode = structure.IsDemonModeActive;
        IsArchDemonMode = structure.IsArchDemonModeActive;
        DemonBuildUp = structure.DemonBuildUp * 100.0f;
    }

    public override void Dispose()
    {
        new IDisposable[]
        {
            _onDemonBuildUpChange,
            _onArchDemonModeStateChange,
            _onDemonModeStateChange,
            _onPiercingBindTimerChange
        }.DisposeAll();

        base.Dispose();
    }
}
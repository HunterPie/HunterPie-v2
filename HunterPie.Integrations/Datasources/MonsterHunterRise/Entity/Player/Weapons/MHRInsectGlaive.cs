
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
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
public sealed class MHRInsectGlaive : MHRMeleeWeapon, IInsectGlaive
{
    private KinsectBuff _primaryExtract;
    private KinsectBuff _secondaryExtract;
    private float _attackTimer;
    private float _speedTimer;
    private float _defenseTimer;
    private float _stamina;

    public KinsectBuff PrimaryExtract
    {
        get => _primaryExtract;
        private set
        {
            if (value == _primaryExtract)
                return;

            _primaryExtract = value;
            this.Dispatch(_onPrimaryExtractChange, new InsectGlaiveExtractChangeEventArgs(value));
        }
    }

    public KinsectBuff SecondaryExtract
    {
        get => _secondaryExtract;
        private set
        {
            if (value == _secondaryExtract)
                return;

            _secondaryExtract = value;
            this.Dispatch(_onSecondaryExtractChange, new InsectGlaiveExtractChangeEventArgs(value));
        }
    }

    public KinsectChargeType ChargeType => KinsectChargeType.None;

    public float AttackTimer
    {
        get => _attackTimer;
        private set
        {
            if (value == _attackTimer)
                return;

            _attackTimer = value;
            this.Dispatch(_onAttackTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    public float SpeedTimer
    {
        get => _speedTimer;
        private set
        {
            if (value == _speedTimer)
                return;

            _speedTimer = value;
            this.Dispatch(_onSpeedTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    public float DefenseTimer
    {
        get => _defenseTimer;
        private set
        {
            if (value == _defenseTimer)
                return;

            _defenseTimer = value;
            this.Dispatch(_onDefenseTimerChange, new InsectGlaiveBuffTimerChangeEventArgs(value));
        }
    }

    public float Stamina
    {
        get => _stamina;
        private set
        {
            if (value == _stamina)
                return;

            _stamina = value;
            this.Dispatch(_onKinsectStaminaChange, new KinsectStaminaChangeEventArgs(this));
        }
    }

    public float MaxStamina { get; private set; }

    public float Charge => 0.0f;

    private readonly SmartEvent<InsectGlaiveExtractChangeEventArgs> _onPrimaryExtractChange = new();
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnPrimaryExtractChange
    {
        add => _onPrimaryExtractChange.Hook(value);
        remove => _onPrimaryExtractChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveExtractChangeEventArgs> _onSecondaryExtractChange = new();
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnSecondaryExtractChange
    {
        add => _onSecondaryExtractChange.Hook(value);
        remove => _onSecondaryExtractChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onAttackTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnAttackTimerChange
    {
        add => _onAttackTimerChange.Hook(value);
        remove => _onAttackTimerChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onSpeedTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnSpeedTimerChange
    {
        add => _onSpeedTimerChange.Hook(value);
        remove => _onSpeedTimerChange.Unhook(value);
    }

    private readonly SmartEvent<InsectGlaiveBuffTimerChangeEventArgs> _onDefenseTimerChange = new();
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnDefenseTimerChange
    {
        add => _onDefenseTimerChange.Hook(value);
        remove => _onDefenseTimerChange.Unhook(value);
    }

    private readonly SmartEvent<KinsectStaminaChangeEventArgs> _onKinsectStaminaChange = new();
    public event EventHandler<KinsectStaminaChangeEventArgs> OnKinsectStaminaChange
    {
        add => _onKinsectStaminaChange.Hook(value);
        remove => _onKinsectStaminaChange.Unhook(value);
    }

    private readonly SmartEvent<KinsectChargeChangeEventArgs> _onChargeChange = new();
    public event EventHandler<KinsectChargeChangeEventArgs> OnChargeChange
    {
        add => _onChargeChange.Hook(value);
        remove => _onChargeChange.Unhook(value);
    }

    public MHRInsectGlaive(
        IGameProcess process,
        IScanService scanService) : base(process, scanService, Weapon.InsectGlaive) { }

    [ScannableMethod]
    private async Task GetKinsectData()
    {
        MHRInsectGlaiveDataStructure structure = await Memory.DerefAsync<MHRInsectGlaiveDataStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("CURRENT_WEAPON_OFFSETS")
        );
        KinsectBuff[] extracts = (await Memory.ReadArraySafeAsync<int>(structure.ExtractsArray, 2))
            .Select(it => (KinsectExtract)it)
            .Select(it => it.ToBuff())
            .ToArray();

        if (extracts.Length < 2)
            return;

        (KinsectBuff primary, KinsectBuff secondary) = (extracts.First(), extracts.Last());

        PrimaryExtract = primary;
        SecondaryExtract = secondary != KinsectBuff.None ? secondary : primary;
        AttackTimer = structure.AttackTimer.ToAbnormalitySeconds();
        SpeedTimer = structure.SpeedTimer.ToAbnormalitySeconds();
        DefenseTimer = structure.DefenseTimer.ToAbnormalitySeconds();
    }

    [ScannableMethod]
    private async Task GetKinsectStamina()
    {
        MHRKinsectStaminaStructure structure = await Memory.DerefAsync<MHRKinsectStaminaStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("KINSECT_STAMINA_OFFSETS")
        );

        MaxStamina = structure.Max;
        Stamina = structure.Current;
    }


    public override void Dispose()
    {
        new IDisposable[]
        {
            _onPrimaryExtractChange,
            _onSecondaryExtractChange,
            _onAttackTimerChange,
            _onSpeedTimerChange,
            _onDefenseTimerChange,
            _onKinsectStaminaChange,
            _onChargeChange
        }.DisposeAll();

        base.Dispose();
    }
}
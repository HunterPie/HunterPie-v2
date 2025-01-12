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

public sealed class MHWInsectGlaive : MHWMeleeWeapon, IInsectGlaive
{
    private static readonly KinsectBuff[] EmptyBuffs = { KinsectBuff.None, KinsectBuff.None };
    private KinsectBuff _primaryExtract;
    private KinsectBuff _secondaryExtract;
    private float _attackTimer;
    private float _speedTimer;
    private float _defenseTimer;
    private float _stamina;
    private KinsectChargeType _chargeType;
    private float _charge;

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

    public KinsectChargeType ChargeType
    {
        get => _chargeType;
        private set
        {
            if (value == _chargeType)
                return;

            _chargeType = value;
            this.Dispatch(_onChargeChange, new KinsectChargeChangeEventArgs(this));
        }
    }

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

    public float Charge
    {
        get => _charge;
        private set
        {
            if (value == _charge)
                return;

            _charge = value;
            this.Dispatch(_onChargeChange, new KinsectChargeChangeEventArgs(this));
        }
    }

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

    public MHWInsectGlaive(
        IGameProcess process,
        ISkillService skillService) : base(process, skillService, Weapon.InsectGlaive) { }

    [ScannableMethod]
    private async Task GetWeaponData()
    {
        MHWInsectGlaiveStructure structure = await Memory.DerefAsync<MHWInsectGlaiveStructure>(
            address: AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_MECHANICS_OFFSETS")
        );
        MHWInsectGlaiveExtraStructure extraStructure = await Memory.DerefAsync<MHWInsectGlaiveExtraStructure>(
            address: AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_EXTRA_MECHANICS_DATA_OFFSETS")
        );
        MHWKinsectStructure kinsectStructure = await Memory.ReadAsync<MHWKinsectStructure>(structure.KinsectPointer);

        KinsectBuff[] buffs = structure.QueuedBuffsCount > 0
            ? structure.BuffsQueue.TakeRolling(structure.QueuedIndex, structure.QueuedBuffsCount)
                                  .Select(it => it.ToBuff())
                                  .ToArray()
            : EmptyBuffs;

        float powerProlonger = _skillService.GetPowerProlongerMultiplier(Weapon.InsectGlaive);
        AttackTimer = Math.Max(0.0f, structure.AttackTimer * powerProlonger);
        SpeedTimer = Math.Max(0.0f, structure.SpeedTimer * powerProlonger);
        DefenseTimer = Math.Max(0.0f, structure.DefenseTimer * powerProlonger);
        PrimaryExtract = buffs.FirstOrDefault(KinsectBuff.None);
        SecondaryExtract = buffs.LastOrDefault(KinsectBuff.None);
        MaxStamina = extraStructure.MaxStamina;
        Stamina = kinsectStructure.Stamina;
        ChargeType = kinsectStructure switch
        {
            { RedCharge: > 0.0f, YellowCharge: > 0.0f } => KinsectChargeType.RedAndYellow,
            { RedCharge: > 0.0f } => KinsectChargeType.Red,
            { YellowCharge: > 0.0f } => KinsectChargeType.Yellow,
            _ => KinsectChargeType.None
        };

        float chargeTimer = Math.Max(kinsectStructure.RedCharge, kinsectStructure.YellowCharge);
        Charge = ChargeType != KinsectChargeType.None ? Math.Max(0.0f, chargeTimer * powerProlonger) : 0.0f;
    }

    public override void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onPrimaryExtractChange,
            _onSecondaryExtractChange,
            _onAttackTimerChange,
            _onSpeedTimerChange,
            _onDefenseTimerChange,
            _onKinsectStaminaChange,
            _onChargeChange
        );

        base.Dispose();
    }
}
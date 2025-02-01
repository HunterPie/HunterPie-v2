using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;

public sealed class MHWSwitchAxe : MHWMeleeWeapon, ISwitchAxe
{
    private readonly ISkillService _skillService;

    private float _buildUp;
    public float BuildUp
    {
        get => _buildUp;
        private set
        {
            if (value.Equals(_buildUp))
                return;

            _buildUp = value;
            this.Dispatch(_onBuildUpChange, new BuildUpChangeEventArgs(value, MaxBuildUp));
        }
    }

    public float MaxBuildUp => 100.0f;

    public float LowBuildUp => 30.0f;

    private float _chargeTimer;
    public float ChargeTimer
    {
        get => _chargeTimer;
        private set
        {
            if (value.Equals(_chargeTimer))
                return;

            _chargeTimer = value;
            this.Dispatch(_onChargeTimerChange, new TimerChangeEventArgs(value, MaxChargeTimer));
        }
    }
    public float MaxChargeTimer { get; private set; }

    private float _chargeBuildUp;
    public float ChargeBuildUp
    {
        get => _chargeBuildUp;
        private set
        {
            if (value.Equals(_chargeBuildUp))
                return;

            _chargeBuildUp = value;
            this.Dispatch(_onChargeBuildUpChange, new BuildUpChangeEventArgs(value, MaxChargeBuildUp));
        }
    }
    public float MaxChargeBuildUp => 100.0f;

    private float _slamBuffTimer;
    public float SlamBuffTimer
    {
        get => _slamBuffTimer;
        private set
        {
            if (value.Equals(_slamBuffTimer))
                return;

            _slamBuffTimer = value;
            this.Dispatch(_onSlamBuffTimerChange, new TimerChangeEventArgs(value, MaxSlamBuffTimer));
        }
    }
    public float MaxSlamBuffTimer { get; private set; }

    #region Events
    private readonly SmartEvent<BuildUpChangeEventArgs> _onBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnBuildUpChange
    {
        add => _onBuildUpChange.Hook(value);
        remove => _onBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onChargeTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnChargeTimerChange
    {
        add => _onChargeTimerChange.Hook(value);
        remove => _onChargeTimerChange.Unhook(value);
    }

    private readonly SmartEvent<BuildUpChangeEventArgs> _onChargeBuildUpChange = new();
    public event EventHandler<BuildUpChangeEventArgs> OnChargeBuildUpChange
    {
        add => _onChargeBuildUpChange.Hook(value);
        remove => _onChargeBuildUpChange.Unhook(value);
    }

    private readonly SmartEvent<TimerChangeEventArgs> _onSlamBuffTimerChange = new();
    public event EventHandler<TimerChangeEventArgs> OnSlamBuffTimerChange
    {
        add => _onSlamBuffTimerChange.Hook(value);
        remove => _onChargeTimerChange.Unhook(value);
    }
    #endregion

    public MHWSwitchAxe(
        IGameProcess process,
        ISkillService skillService,
        IScanService scanService) : base(process, scanService, skillService, Weapon.SwitchAxe)
    {
        _skillService = skillService;
    }

    [ScannableMethod]
    private async Task GetData()
    {
        MHWSwitchAxeStructure structure = await Memory.DerefAsync<MHWSwitchAxeStructure>(
            address: AddressMap.GetAbsolute("WEAPON_MECHANICS_ADDRESS"),
            offsets: AddressMap.GetOffsets("WEAPON_MECHANICS_OFFSETS")
        );

        float powerProlongerMultiplier = _skillService.GetPowerProlongerMultiplier(Weapon.SwitchAxe);

        BuildUp = structure.BuildUp;

        float chargeTimer = structure.ChargeTimer * powerProlongerMultiplier;
        MaxChargeTimer = Math.Max(chargeTimer, MaxChargeTimer);
        ChargeTimer = chargeTimer;

        ChargeBuildUp = structure.ChargeBuildUp;
    }

    [ScannableMethod]
    private async Task GetSlamBuff()
    {
        float powerProlongerMultiplier = _skillService.GetPowerProlongerMultiplier(Weapon.SwitchAxe);
        MHWSwitchAxeSlamStructure slamBuff = await Memory.DerefAsync<MHWSwitchAxeSlamStructure>(
            address: AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            offsets: AddressMap.GetOffsets("ABNORMALITY_OFFSETS")
        );
        float slamBuffTimer = slamBuff.IsActive ? slamBuff.Timer : 0.0f;
        float slamBuffTimerAdjusted = slamBuffTimer * powerProlongerMultiplier;

        MaxSlamBuffTimer = Math.Max(MaxSlamBuffTimer, slamBuffTimerAdjusted);
        SlamBuffTimer = slamBuffTimerAdjusted;
    }



    public override void Dispose()
    {
        var disposables = new IDisposable[]
        {
            _onBuildUpChange, _onChargeTimerChange,
            _onChargeBuildUpChange, _onSlamBuffTimerChange
        };
        disposables.DisposeAll();
        base.Dispose();
    }
}
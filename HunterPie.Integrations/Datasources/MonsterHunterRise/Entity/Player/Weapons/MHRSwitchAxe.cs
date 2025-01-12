using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;

public sealed class MHRSwitchAxe : MHRMeleeWeapon, ISwitchAxe
{
    private readonly int[] _maxChargeBuildUpOffsets = { 0x20, 0x10 };

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

    public float LowBuildUp => 37.0f;

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
    public float MaxChargeBuildUp { get; private set; }

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

    [ScannableMethod]
    private async Task GetData()
    {
        MHRSwitchAxeStructure structure = await Memory.DerefAsync<MHRSwitchAxeStructure>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("CURRENT_WEAPON_OFFSETS")
        );
        float[] maxChargeBuildUps = await Memory.ReadArraySafeAsync<float>(structure.MaxChargeBuildUpsPointer, 6);

        BuildUp = structure.BuildUp;

        await DeferMaxChargeBuildUp(
            maxChargeBuildUps: maxChargeBuildUps,
            weaponDataPtr: structure.WeaponDataPointer
        );
        ChargeBuildUp = structure.ChargeBuildUp;

        float chargeTimer = structure.ChargeTimer.ToAbnormalitySeconds();
        MaxChargeTimer = Math.Max(chargeTimer, MaxChargeTimer);
        ChargeTimer = chargeTimer;

        float slamBuffTimer = structure.SlamBuffTimer.ToAbnormalitySeconds();
        MaxSlamBuffTimer = Math.Max(slamBuffTimer, MaxSlamBuffTimer);
        SlamBuffTimer = slamBuffTimer;
    }

    private async Task DeferMaxChargeBuildUp(float[] maxChargeBuildUps, nint weaponDataPtr)
    {
        MHRSwitchAxeWeaponDataStructure weaponData = await Memory.DerefPtrAsync<MHRSwitchAxeWeaponDataStructure>(weaponDataPtr, _maxChargeBuildUpOffsets);

        // MHRise has a jump table to convert the bottle type into an index
        int buildUpIndex = (weaponData.PhialType - 1) switch
        {
            0 => 1,
            2 => 5,
            3 => 4,
            6 => 3,
            7 => 2,
            _ => 0
        };
        float maxBuildUp = maxChargeBuildUps.ElementAtOrDefault(buildUpIndex);

        MaxChargeBuildUp = maxBuildUp;
    }

    public MHRSwitchAxe(IGameProcess process) : base(process, Weapon.SwitchAxe)
    {
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
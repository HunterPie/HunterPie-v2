using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;
using HunterPie.Integrations.Datasources.Common.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Collections.Concurrent;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;

public sealed class MHRMonster : CommonMonster
{
    private readonly MonsterDefinition _definition;
    private readonly nint _address;

    private bool _isLoaded;
    private float _health = -1;
    private bool _isEnraged;
    private Target _target;
    private Target _manualTarget;
    private Crown _crown;
    private float _stamina;
    private float _captureThreshold;
    private readonly MHRMonsterAilment _enrage = new(MonsterAilmentRepository.Enrage);
    private MHRMonsterPart? _qurioThreshold;
    private readonly Dictionary<long, MHRMonsterPart> _parts = new();
    private readonly object _syncParts = new();
    private readonly ConcurrentDictionary<long, MHRMonsterAilment> _ailments = new();
    private readonly List<Element> _weaknesses = new();
    private readonly List<string> _types = new();

    public override int Id { get; protected set; }

    public override string Name => MHRContext.Strings.GetMonsterNameById(Id);

    public override float Health
    {
        get => _health;
        protected set
        {
            if (_health != value)
            {
                _health = value;
                this.Dispatch(_onHealthChange);

                if (Health <= 0)
                    this.Dispatch(_onDeath);
            }
        }
    }

    public override float MaxHealth { get; protected set; }

    public override float Stamina
    {
        get => _stamina;
        protected set
        {
            if (value != _stamina)
            {
                _stamina = value;
                this.Dispatch(_onStaminaChange);
            }
        }
    }

    public override float MaxStamina { get; protected set; }

    public override Target Target
    {
        get => _target;
        protected set
        {
            if (_target != value)
            {
                _target = value;
                this.Dispatch(_onTargetChange, new MonsterTargetEventArgs(this));
            }
        }
    }

    public override Target ManualTarget
    {
        get => _manualTarget;
        protected set
        {
            if (_manualTarget == value)
                return;

            _manualTarget = value;
            this.Dispatch(_onTargetChange, new MonsterTargetEventArgs(this));
        }
    }

    public override Crown Crown
    {
        get => _crown;
        protected set
        {
            if (_crown != value)
            {
                _crown = value;
                this.Dispatch(_onCrownChange);
            }
        }
    }

    public override IMonsterPart[] Parts
    {
        get
        {
            lock (_syncParts)
            {
                var extraParts = new List<IMonsterPart>();

                if (_qurioThreshold is not null)
                    extraParts.Add(_qurioThreshold);

                extraParts.AddRange(_parts.Values);
                return extraParts.ToArray();
            }
        }
    }

    public override IReadOnlyCollection<IMonsterAilment> Ailments => _ailments.Values.ToArray();

    public override bool IsEnraged
    {
        get => _isEnraged;
        protected set
        {
            if (value != _isEnraged)
            {
                _isEnraged = value;
                this.Dispatch(_onEnrageStateChange);
            }
        }
    }

    public override IMonsterAilment Enrage => _enrage;

    public override Element[] Weaknesses => _weaknesses.ToArray();

    public override string[] Types => _types.ToArray();

    public override float CaptureThreshold
    {
        get => _captureThreshold;
        protected set
        {
            if (value != _captureThreshold)
            {
                _captureThreshold = value;
                this.Dispatch(_onCaptureThresholdChange, this);
            }
        }
    }

    public MonsterType MonsterType { get; private set; }

    public bool IsQurioActive { get; private set; }

    public MHRMonster(
        IGameProcess process,
        nint address,
        int id
    ) : base(process)
    {
        _address = address;

        Id = id;

        _definition = MonsterRepository.FindBy(GameType.Rise, Id) ?? MonsterRepository.UnknownDefinition;

        UpdateData();

        Log.Debug($"Initialized monster at address {address:X}");
    }

    private void UpdateData()
    {
        _types.AddRange(_definition.Types);
        _weaknesses.AddRange(_definition.Weaknesses);
        this.Dispatch(_onWeaknessesChange, Weaknesses);
    }

    [ScannableMethod]
    private async Task GetMonsterType()
    {
        nint monsterTypePtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_TYPE_OFFSETS")
        );
        int monsterType = await Memory.ReadAsync<int>(monsterTypePtr + 0x5C);
        MonsterType = (MonsterType)monsterType;

        if (MonsterType != MonsterType.Qurio)
            return;

        _qurioThreshold = new MHRMonsterPart(MHRiseUtils.QurioPartDefinition);
    }

    [ScannableMethod]
    private async Task GetMonsterHealthData()
    {
        HealthData dto = new();

        nint healthComponent = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_OFFSETS")
        );
        nint currentHealthPtr = await Memory.ReadPtrAsync(
            address: healthComponent,
            offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS")
        );

        dto.Health = await Memory.ReadAsync<float>(currentHealthPtr + 0x18);
        dto.MaxHealth = await Memory.ReadAsync<float>(healthComponent + 0x18);

        nint monsterStatusPtr = await Memory.ReadAsync<nint>(_address + 0x310);
        var aliveStatus = (MonsterAliveStatus)await Memory.ReadAsync<int>(monsterStatusPtr + 0x20);

        MaxHealth = dto.MaxHealth;
        Health = dto.Health;

        if (aliveStatus == MonsterAliveStatus.Dead && Health > 0)
            this.Dispatch(_onCapture, EventArgs.Empty);
    }

    [ScannableMethod]
    private async Task GetMonsterCaptureThreshold()
    {
        if (MonsterType == MonsterType.Qurio)
        {
            CaptureThreshold = 0.0f;
            return;
        }

        if (_definition is { IsNotCapturable: true })
        {
            CaptureThreshold = 0.0f;
            return;
        }

        nint captureHealthPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_CAPTURE_HEALTH_THRESHOLD")
        );
        float captureHealth = await Memory.ReadAsync<float>(captureHealthPtr + 0x1C);

        CaptureThreshold = captureHealth / MaxHealth;
    }

    [ScannableMethod]
    private async Task GetMonsterParts()
    {
        nint qurioDataPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        bool isQurioActive = await Memory.ReadAsync<byte>(qurioDataPtr + 0x12) == 2;

        // Flinch array
        nint monsterFlinchPartsPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_FLINCH_HEALTH_COMPONENT_OFFSETS")
        );
        int monsterFlinchPartsArrayLength = await Memory.ReadAsync<int>(monsterFlinchPartsPtr + 0x1C);

        // Breakable array
        nint monsterBreakPartsArrayPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_BREAK_HEALTH_COMPONENT_OFFSETS")
        );
        int monsterBreakPartsArrayLength = await Memory.ReadAsync<int>(monsterBreakPartsArrayPtr + 0x1C);

        // Severable array
        nint monsterSeverPartsArrayPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_SEVER_HEALTH_COMPONENT_OFFSETS")
        );
        int monsterSeverPartsArrayLength = await Memory.ReadAsync<int>(monsterSeverPartsArrayPtr + 0x1C);

        // Qurio array
        nint monsterQurioPartsArrayPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_QURIO_HEALTH_COMPONENT_OFFSETS")
        );
        int monsterQurioPartsArrayLength = await Memory.ReadAsync<int>(monsterQurioPartsArrayPtr + 0x1C);

        if (monsterFlinchPartsArrayLength == monsterBreakPartsArrayLength
            && monsterFlinchPartsArrayLength == monsterSeverPartsArrayLength)
        {
            nint[] monsterFlinchArray = await Memory.ReadAsync<nint>(monsterFlinchPartsPtr + 0x20, monsterFlinchPartsArrayLength);
            nint[] monsterBreakArray = await Memory.ReadAsync<nint>(monsterBreakPartsArrayPtr + 0x20, monsterBreakPartsArrayLength);
            nint[] monsterSeverArray = await Memory.ReadAsync<nint>(monsterSeverPartsArrayPtr + 0x20, monsterSeverPartsArrayLength);
            nint[] monsterQurioArray = isQurioActive
                ? await Memory.ReadAsync<nint>(monsterQurioPartsArrayPtr + 0x20, monsterQurioPartsArrayLength)
                : Array.Empty<nint>();

            await DerefPartsAndScan(monsterFlinchArray, monsterBreakArray, monsterSeverArray, monsterQurioArray);
        }

        IsQurioActive = isQurioActive;
    }

    private async Task DerefPartsAndScan(
        IReadOnlyList<nint> flinchPointers,
        IReadOnlyList<nint> partsPointers,
        IReadOnlyList<nint> severablePointers,
        IReadOnlyList<nint> qurioPointers
    )
    {
        for (int i = 0; i < flinchPointers.Count; i++)
        {

            nint flinchPart = flinchPointers[i];
            nint breakablePart = partsPointers[i];
            nint severablePart = severablePointers[i];
            nint? qurioPart = qurioPointers.Count > 0
                ? qurioPointers[i]
                : null;

            MHRPartStructure partInfo = new()
            {
                MaxHealth = await Memory.ReadAsync<float>(breakablePart + 0x18),
                MaxFlinch = await Memory.ReadAsync<float>(flinchPart + 0x18),
                MaxSever = await Memory.ReadAsync<float>(severablePart + 0x18)
            };

            MHRQurioPartData qurioInfo = new();

            // Skip invalid parts
            if (partInfo.MaxFlinch < 0 && partInfo.MaxHealth < 0 && partInfo.MaxSever < 0)
                continue;

            nint encodedFlinchHealthPtr = await Memory.ReadPtrAsync(
                address: flinchPart,
                offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS")
            );
            nint encodedBreakableHealthPtr = await Memory.ReadPtrAsync(
                address: breakablePart,
                offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS")
            );
            nint encodedSeverableHealthPtr = await Memory.ReadPtrAsync(
                address: severablePart,
                offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS")
            );

            if (qurioPart is { } qurioPartPtr)
            {
                MHRQurioPartStructure qurioStructure = await Memory.ReadAsync<MHRQurioPartStructure>(qurioPartPtr);

                nint healthPtr = await Memory.ReadPtrAsync(
                    address: qurioStructure.HealthPtr,
                    offsets: AddressMap.Get<int[]>("MONSTER_HEALTH_COMPONENT_ENCODED_OFFSETS")
                );

                qurioInfo = new MHRQurioPartData
                {
                    MaxHealth = qurioStructure.MaxHealth,
                    Health = await Memory.ReadAsync<float>(healthPtr + 0x18),
                    IsInQurioState = qurioStructure.IsActive
                };
            }

            partInfo.Flinch = await Memory.ReadAsync<float>(encodedFlinchHealthPtr + 0x18);
            partInfo.Health = await Memory.ReadAsync<float>(encodedBreakableHealthPtr + 0x18);
            partInfo.Sever = await Memory.ReadAsync<float>(encodedSeverableHealthPtr + 0x18);

            lock (_syncParts)
            {
                if (!_parts.ContainsKey(flinchPart))
                {
                    MonsterPartDefinition definition = _definition.Parts.Length > i ? _definition.Parts[i] : MonsterRepository.UnknownPartDefinition;

                    var dummy = new MHRMonsterPart(definition, partInfo);
                    _parts.Add(flinchPart, dummy);

                    Log.Debug($"Found {definition.String} for {Name} -> Flinch: {flinchPart:X} Break: {breakablePart:X} Sever: {severablePart:X} Qurio: {qurioPart:X}");
                    this.Dispatch(_onNewPartFound, dummy);
                }

                _parts[flinchPart].Update(partInfo);
                _parts[flinchPart].Update(qurioInfo);
            }
        }
    }

    [ScannableMethod]
    private async Task GetQurioThreshold()
    {
        if (_qurioThreshold is null)
            return;

        nint qurioDataPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_QURIO_DATA")
        );
        MHRQurioThresholdStructure structure = await Memory.ReadAsync<MHRQurioThresholdStructure>(qurioDataPtr + 0x14);
        var data = new MHRQurioPartData
        {
            IsInQurioState = true,
            Health = structure.MaxThreshold - structure.Threshold,
            MaxHealth = structure.MaxThreshold
        };

        _qurioThreshold.Update(data);
    }

    [ScannableMethod]
    private async Task GetMonsterAilments()
    {
        nint ailmentsArrayPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_AILMENTS_OFFSETS")
        );

        if (ailmentsArrayPtr.IsNullPointer())
            return;

        nint[] ailmentsArray = await Memory.ReadArraySafeAsync<nint>(ailmentsArrayPtr, 17);

        await DerefAilmentsAndScanAsync(ailmentsArray);
    }

    [ScannableMethod]
    private async Task GetLockedOnMonster()
    {
        int cameraStyleType = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("LOCKON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("LOCKON_CAMERA_STYLE_OFFSETS")
        );

        nint cameraStylePtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("LOCKON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("LOCKON_OFFSETS")
        );

        cameraStylePtr += cameraStyleType * 8;

        nint targetAddress = await Memory.DerefAsync<nint>(
                cameraStylePtr,
                new[] { 0x78 }
        );

        bool isTarget = targetAddress == _address;

        Target = (isTarget, targetAddress) switch
        {
            (true, _) => Target.Self,
            (false, 0) => Target.None,
            (false, _) => Target.Another,
        };
    }

    [ScannableMethod]
    private async Task GetQuestTarget()
    {
        nint targetAddress = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("QUEST_GUI_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_AUTOMATIC_TARGET")
        );
        bool isTarget = targetAddress == _address;

        ManualTarget = (isTarget, targetAddress) switch
        {
            (true, _) => Target.Self,
            (false, 0) => Target.None,
            (false, _) => Target.Another,
        };
    }

    [ScannableMethod]
    private async Task GetMonsterCrown()
    {
        nint monsterSizePtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_CROWN_OFFSETS")
        );
        MHRSizeStructure monsterSize = await Memory.ReadAsync<MHRSizeStructure>(monsterSizePtr + 0x24);
        float monsterSizeMultiplier = monsterSize.SizeMultiplier * monsterSize.UnkMultiplier;

        MonsterSizeDefinition crown = _definition.Size;

        Crown = monsterSizeMultiplier >= crown.Gold
            ? Crown.Gold
            : monsterSizeMultiplier >= crown.Silver
                ? Crown.Silver
                : monsterSizeMultiplier <= crown.Mini
                    ? Crown.Mini
                    : Crown.None;
    }

    [ScannableMethod]
    private async Task GetMonsterEnrage()
    {
        nint enragePtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_ENRAGE_OFFSETS")
        );

        MHREnrageStructure structure = await Memory.ReadAsync<MHREnrageStructure>(enragePtr);

        structure.Timer = structure.Timer > 0 ? structure.MaxTimer - structure.Timer : 0;

        _enrage.Update(structure);

        IsEnraged = structure.Timer > 0;
    }

    [ScannableMethod]
    private async Task GetMonsterStamina()
    {
        nint staminaPtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.Get<int[]>("MONSTER_STAMINA_OFFSETS")
        );

        MHRStaminaStructure structure = await Memory.ReadAsync<MHRStaminaStructure>(staminaPtr);

        MaxStamina = structure.MaxStamina;
        Stamina = structure.Stamina;
    }

    [ScannableMethod]
    private Task FinishScan()
    {
        if (_isLoaded)
            return Task.CompletedTask;

        Log.Debug($"Initialized {Name} at address {_address:X} with id: {Id}");
        _isLoaded = true;
        this.Dispatch(_onSpawn, EventArgs.Empty);

        return Task.CompletedTask;
    }

    private async Task DerefAilmentsAndScanAsync(nint[] ailmentsPointers)
    {
        int ailmentId = 0;
        foreach (nint ailmentAddress in ailmentsPointers)
        {
            MHRMonsterAilmentStructure structure = await Memory.ReadAsync<MHRMonsterAilmentStructure>(ailmentAddress);

            int counter = await Memory.ReadAsync<int>(structure.CounterPtr + 0x20);
            float buildup = await Memory.ReadAsync<float>(structure.BuildUpPtr + 0x20);
            float maxBuildup = await Memory.ReadAsync<float>(structure.MaxBuildUpPtr + 0x20);

            if (!_ailments.ContainsKey(ailmentAddress))
            {
                AilmentDefinition? ailmentDef = MonsterAilmentRepository.FindBy(GameType.Rise, ailmentId);

                if (ailmentDef is not { } definition)
                    continue;

                MHRMonsterAilment dummy = new(definition);
                _ailments.TryAdd(ailmentAddress, dummy);

                this.Dispatch(_onNewAilmentFound, dummy);
            }

            var data = new MHRAilmentData
            {
                BuildUp = buildup,
                MaxBuildUp = maxBuildup,
                Timer = structure.Timer,
                MaxTimer = structure.MaxTimer,
                Counter = counter,
            };

            MHRMonsterAilment ailment = _ailments[ailmentAddress];
            ailment.Update(data);
            ailmentId++;
        }
    }

    public override void Dispose()
    {
        _ailments.Values.DisposeAll();
        _ailments.Clear();
        _enrage.Dispose();

        lock (_syncParts)
        {
            _qurioThreshold?.Dispose();

            _parts.Values.DisposeAll();

            _parts.Clear();
        }

        base.Dispose();
    }
}
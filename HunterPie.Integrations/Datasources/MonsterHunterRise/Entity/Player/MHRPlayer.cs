using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common.Definition;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Entities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Text;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public sealed class MHRPlayer : CommonPlayer
{
    #region Private
    private int _saveSlotId;
    private string _name = string.Empty;
    private int _stageId = -1;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    private readonly MHRParty _party = new();
    private MHRStageStructure _stageData;
    private MHRStageStructure _lastStageData;
    private readonly HealthComponent _health = new();
    private readonly StaminaComponent _stamina = new();
    private int _highRank;
    private int _masterRank;
    private IWeapon _weapon;
    private Weapon _weaponId = WeaponType.None;
    private readonly Dictionary<int, MHREquipmentSkillStructure> _armorSkills = new(46);
    private CommonConditions _commonCondition;
    private DebuffConditions _debuffCondition;
    private ActionFlags _actionFlag;
    #endregion

    public override string Name
    {
        get => _name;
        protected set
        {
            if (value == _name)
                return;

            _name = value;

            this.Dispatch(
                value != ""
                    ? _onLogin
                    : _onLogout
            );
        }
    }

    public override int HighRank
    {
        get => _highRank;
        protected set
        {
            if (value != _highRank)
            {
                _highRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }

    public override int MasterRank
    {
        get => _masterRank;
        protected set
        {
            if (value != _masterRank)
            {
                _masterRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }

    public override int StageId
    {
        get => _stageId;
        protected set
        {
            if (value != _stageId)
            {
                if (_stageData.IsVillage() && value != 5 && (_lastStageData.IsHuntingZone() || StageId == 5 || _lastStageData.IsIrrelevantStage()))
                    this.Dispatch(_onVillageEnter);
                else if (_stageData.IsHuntingZone() || value == 5)
                    this.Dispatch(_onVillageLeave);

                _stageId = value;
                this.Dispatch(_onStageUpdate);
            }
        }
    }

    public override bool InHuntingZone => _stageData.IsHuntingZone() || StageId == 5;

    public override IParty Party => _party;

    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IHealthComponent Health => _health;

    public override IStaminaComponent Stamina => _stamina;

    public MHRWirebug[] Wirebugs { get; } = { new(), new(), new(), new() };

    public MHRArgosy Argosy { get; } = new();

    public MHRTrainingDojo TrainingDojo { get; } = new();

    public MHRMeowmasters Meowmasters { get; } = new();

    public MHRCohoot Cohoot { get; } = new();

    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                IWeapon lastWeapon = _weapon;
                _weapon = value;
                this.Dispatch(_onWeaponChange, new WeaponChangeEventArgs(lastWeapon, _weapon));

                if (lastWeapon is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }

    public Scroll SwitchScroll { get; private set; }

    #region Events

    private readonly SmartEvent<MHRWirebug[]> _onWirebugsRefresh = new();
    public event EventHandler<MHRWirebug[]> OnWirebugsRefresh
    {
        add => _onWirebugsRefresh.Hook(value);
        remove => _onWirebugsRefresh.Unhook(value);
    }

    #endregion

    public MHRPlayer(
        IGameProcess process,
        IScanService scanService) : base(process, scanService)
    {
        _weapon = CreateDefaultWeapon();
    }

    private IWeapon CreateDefaultWeapon()
    {
        var weapon = new MHRMeleeWeapon(
            process: Process,
            scanService: ScanService,
            id: WeaponType.Greatsword
        );

        return weapon;
    }

    // TODO: Add DTOs for middlewares

    [ScannableMethod]
    private async Task GetStageData()
    {
        nint stageAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("STAGE_ADDRESS"),
            offsets: AddressMap.GetOffsets("STAGE_OFFSETS")
        );

        if (stageAddress.IsNullPointer())
            return;

        MHRStageStructure stageData = await Memory.ReadAsync<MHRStageStructure>(stageAddress + 0x60);

        int zoneId = stageData.IsMainMenu()
            ? -1
            : stageData.IsVillage()
            ? stageData.VillageId
            : stageData.IsLoadingScreen() ? -2 : stageData.IsSelectingCharacter() ? 199 : stageData.HuntingId + 200;
        MHRStageStructure tempStageData = _stageData;
        _stageData = stageData;
        _lastStageData = tempStageData;

        StageId = zoneId;
    }

    [ScannableMethod]
    private async Task GetPlayerSaveData()
    {
        if (_stageData.IsMainMenu())
        {
            Name = "";
            return;
        }

        nint currentPlayerSaveAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            offsets: AddressMap.GetOffsets("CHARACTER_OFFSETS")
        );

        nint namePtr = await Memory.ReadAsync<nint>(currentPlayerSaveAddress);
        int nameLength = await Memory.ReadAsync<int>(namePtr + 0x10);
        string name = await Memory.ReadAsync(
            address: namePtr + 0x14,
            length: nameLength * 2,
            encoding: Encoding.Unicode
        );

        if (name != Name)
            await FindPlayerSaveSlotAsync();

        Name = name;
    }

    private async Task FindPlayerSaveSlotAsync()
    {
        if (_stageData.IsMainMenu())
        {
            Name = "";
            _saveSlotId = -1;
            return;
        }

        nint currentPlayerSaveAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            offsets: AddressMap.GetOffsets("CHARACTER_OFFSETS")
        );
        nint namePtr = await Memory.ReadAsync<nint>(currentPlayerSaveAddress);

        nint saveAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("SAVE_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SAVE_OFFSETS")
        );

        for (int i = 0; i < 3; i++)
        {
            int[] nameOffsets = { (i * 8) + 0x20, 0x10 };

            nint saveNamePtr = await Memory.DerefAsync<nint>(saveAddress, nameOffsets);

            if (saveNamePtr != namePtr)
                continue;

            _saveSlotId = i;
        }
    }

    [ScannableMethod]
    private async Task GetPlayerLevel()
    {
        if (_saveSlotId < 0)
            return;

        nint saveAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("SAVE_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SAVE_OFFSETS")
        );

        int[] levelOffsets = { (_saveSlotId * 8) + 0x20, 0x18 };

        MHRPlayerLevelStructure level = await Memory.DerefAsync<MHRPlayerLevelStructure>(saveAddress, levelOffsets);

        HighRank = level.HighRank;
        MasterRank = level.MasterRank;
    }

    [ScannableMethod]
    private async Task GetPlayerWeaponData()
    {
        if (_stageData.IsMainMenu())
            return;

        nint weaponIdPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );

        int weaponId = await Memory.ReadAsync<int>(weaponIdPtr + 0x8C);

        WeaponType weapon = weaponId.ToWeaponId();

        if (weapon == _weaponId)
            return;

        if (Weapon is IDisposable disposable)
            disposable.Dispose();

        IWeapon? weaponInstance = weapon switch
        {
            WeaponType.ChargeBlade => new MHRChargeBlade(Process, ScanService),
            WeaponType.InsectGlaive => new MHRInsectGlaive(Process, ScanService),
            WeaponType.Bow => new MHRBow(),
            WeaponType.HeavyBowgun => new MHRHeavyBowgun(),
            WeaponType.LightBowgun => new MHRLightBowgun(),
            WeaponType.DualBlades => new MHRDualBlades(Process, ScanService),
            WeaponType.SwitchAxe => new MHRSwitchAxe(Process, ScanService),
            WeaponType.Longsword => new MHRLongSword(Process, ScanService),

            WeaponType.Greatsword
            or WeaponType.SwordAndShield
            or WeaponType.Hammer
            or WeaponType.HuntingHorn
            or WeaponType.Lance
            or WeaponType.GunLance => new MHRMeleeWeapon(Process, ScanService, weapon),

            _ => null
        };

        if (weaponInstance is not { })
            return;

        Weapon = weaponInstance;
        _weaponId = weapon;
    }

    [ScannableMethod]
    private async Task GetPlayerAbnormalitiesCleanup()
    {
        nint debuffsPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            offsets: AddressMap.GetOffsets("DEBUFF_ABNORMALITIES_OFFSETS")
        );

        if (!InHuntingZone || debuffsPtr == 0)
            ClearAbnormalities(_abnormalities);
    }

    [ScannableMethod]
    private async Task GetPlayerEquipmentSkills()
    {
        nint armorSkillsPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("PLAYER_GEAR_SKILLS_ARRAY_OFFSETS")
        );

        IAsyncEnumerable<MHREquipmentSkillStructure> armorSkills = Memory.ReadArrayOfPtrsAsync<MHREquipmentSkillStructure>(armorSkillsPtr);

        _armorSkills.Clear();

        await foreach (MHREquipmentSkillStructure skill in armorSkills)
            if (skill.Id > 0)
                _armorSkills.Add(skill.Id, skill);
    }

    [ScannableMethod]
    private async Task GetPlayerSwitchState()
    {
        int switchScroll = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("PLAYER_SWITCH_SCROLL_OFFSETS")
        );

        SwitchScroll = (Scroll)switchScroll;
    }

    [ScannableMethod]
    private async Task GetConsumableAbnormalities()
    {
        if (!InHuntingZone)
            return;

        nint consumableBuffs = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            offsets: AddressMap.Get<int[]>("CONS_ABNORMALITIES_OFFSETS")
        );

        if (consumableBuffs.IsNullPointer())
            return;

        AbnormalityDefinition[] consumableSchemas =
            AbnormalityRepository.FindAllAbnormalitiesBy(GameType.Rise, AbnormalityGroup.CONSUMABLES);

        foreach (AbnormalityDefinition schema in consumableSchemas)
        {
            int abnormSubId = schema.DependsOn switch
            {
                0 => 0,
                _ => await Memory.ReadAsync<int>(consumableBuffs + schema.DependsOn)
            };

            bool isConditionValid = schema.FlagType switch
            {
                AbnormalityFlagType.RiseCommon => _commonCondition.HasFlag(schema.GetFlagAs<CommonConditions>()),
                AbnormalityFlagType.RiseDebuff => _debuffCondition.HasFlag(schema.GetFlagAs<DebuffConditions>()),
                AbnormalityFlagType.RiseAction => _actionFlag.HasFlag(schema.GetFlagAs<ActionFlags>()),
                _ => true
            } && abnormSubId == schema.WithValue;

            MHRAbnormalityData abnormality = new();

            if (isConditionValid)
            {
                if (schema.IsInfinite)
                    abnormality.Timer = 1;
                else
                {
                    MHRAbnormalityStructure abnormalityStructure = await Memory.ReadAsync<MHRAbnormalityStructure>(consumableBuffs + schema.Offset);
                    abnormality = MHRAbnormalityAdapter.Convert(schema, abnormalityStructure);

                    if (!schema.IsInteger && !schema.IsBuildup)
                        abnormality.Timer = abnormality.Timer.ToAbnormalitySeconds();

                    if (schema.MaxTimer > 0)
                        abnormality.Timer = Math.Max(0.0f, schema.MaxTimer - abnormality.Timer);
                }
            }

            HandleAbnormality<MHRConsumableAbnormality, MHRAbnormalityData>(
                _abnormalities,
                schema,
                abnormality.Timer,
                abnormality
            );
        }
    }

    [ScannableMethod]
    private async Task GetPlayerDebuffAbnormalities()
    {
        if (!InHuntingZone)
            return;

        nint debuffsPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            offsets: AddressMap.Get<int[]>("DEBUFF_ABNORMALITIES_OFFSETS")
        );

        if (debuffsPtr.IsNullPointer())
            return;

        AbnormalityDefinition[] debuffSchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.Rise, AbnormalityGroup.DEBUFFS);

        foreach (AbnormalityDefinition schema in debuffSchemas)
        {
            int abnormSubId = schema.DependsOn switch
            {
                0 => 0,
                _ => await Memory.ReadAsync<int>(debuffsPtr + schema.DependsOn)
            };

            bool isConditionValid = schema.FlagType switch
            {
                AbnormalityFlagType.RiseCommon => _commonCondition.HasFlag(schema.GetFlagAs<CommonConditions>()),
                AbnormalityFlagType.RiseDebuff => _debuffCondition.HasFlag(schema.GetFlagAs<DebuffConditions>()),
                AbnormalityFlagType.RiseAction => _actionFlag.HasFlag(schema.GetFlagAs<ActionFlags>()),
                _ => true
            } && abnormSubId == schema.WithValue;

            MHRAbnormalityData abnormality = new();

            if (isConditionValid)
            {
                if (schema.IsInfinite)
                    abnormality.Timer = 1;
                else
                {
                    MHRAbnormalityStructure abnormalityStructure = await Memory.ReadAsync<MHRAbnormalityStructure>(debuffsPtr + schema.Offset);
                    abnormality = MHRAbnormalityAdapter.Convert(schema, abnormalityStructure);

                    if (!schema.IsInteger && !schema.IsBuildup)
                        abnormality.Timer = abnormality.Timer.ToAbnormalitySeconds();

                    if (schema.MaxTimer > 0)
                        abnormality.Timer = Math.Max(0.0f, schema.MaxTimer - abnormality.Timer);
                }
            }

            HandleAbnormality<MHRDebuffAbnormality, MHRAbnormalityData>(
                _abnormalities,
                schema,
                abnormality.Timer,
                abnormality
            );
        }
    }

    [ScannableMethod]
    private async Task GetSessionPlayers()
    {
        int questState = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("QUEST_ADDRESS"),
            offsets: AddressMap.Get<int[]>("QUEST_STATUS_OFFSETS")
        );

        if (questState.IsQuestFinished())
            return;

        // Only scan party members when the player is not in the quest end screen
        if (!questState.IsInQuest() && !StageId.IsTrainingRoom())
        {
            _party.Clear();
            return;
        }

        nint playersArrayPtr = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SOS_SESSION_PLAYER_OFFSETS")
        );

        nint playersWeaponPtr = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("SESSION_PLAYERS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SESSION_PLAYER_OFFSETS")
        );

        PartyMemberMetadata[] sessionPlayersArray = (await Memory.ReadAsync<nint>(playersArrayPtr + 0x20, 4))
            .Select(pointer => (isValid: pointer != 0, pointer))
            .Select(async (player, index) => new PartyMemberMetadata(index, player.isValid, await Memory.ReadAsync<MHRCharacterData>(player.pointer)))
            .Select(it => it.Result)
            .ToArray();

        bool isSos = sessionPlayersArray.Any(player => player.IsValid);

        if (!isSos)
        {
            playersArrayPtr = await Memory.DerefAsync<nint>(
                address: AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
                offsets: AddressMap.Get<int[]>("CHARACTER_INFO_OFFSETS")
            );

            sessionPlayersArray = (await Memory.ReadAsync<nint>(playersArrayPtr + 0x20, 6))
                .Select(pointer => (isValid: pointer != 0, pointer))
                .Select(async (player, index) => new PartyMemberMetadata(index, player.isValid, await Memory.ReadAsync<MHRCharacterData>(player.pointer)))
                .Select(it => it.Result)
                .ToArray();
        }

        bool isOnlineSession = sessionPlayersArray[..4].Any(player => player.IsValid);

        nint[] playerWeaponsPtr = await Memory.ReadAsync<nint>(playersWeaponPtr + 0x20, 6);
        PartyMemberMetadata[] servantsData = await GetServantsDataAsync();

        if (!isSos && servantsData.Any())
            Array.Copy(servantsData, 0, sessionPlayersArray, 4, 2);

        // In case player DC'd
        if (!isOnlineSession)
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _party.Update(new MHRPartyMemberData
            {
                Index = 0,
                Name = Name,
                HighRank = HighRank,
                MasterRank = MasterRank,
                WeaponId = _weaponId,
                IsMyself = true
            });

            if (!servantsData.Any())
                return;
        }

        foreach ((int index, bool isValid, MHRCharacterData data) in sessionPlayersArray)
        {
            nint weaponPtr = playerWeaponsPtr[index];
            string name = await Memory.ReadAsync(data.NamePointer + 0x14, 32, Encoding.Unicode);

            if (!isValid || weaponPtr.IsNullPointer())
            {
                if (isOnlineSession)
                    _party.Remove(index);

                continue;
            }

            Weapon weapon = (await Memory.ReadAsync<int>(weaponPtr + 0x134)).ToWeaponId();

            var memberData = new MHRPartyMemberData
            {
                Index = index,
                Name = name,
                HighRank = data.HighRank,
                MasterRank = data.MasterRank,
                WeaponId = weapon,
                IsMyself = name == Name
            };

            _party.Update(memberData);
        }
    }

    private async Task<PartyMemberMetadata[]> GetServantsDataAsync()
    {
        nint servantsArrayPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("SERVANTS_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SERVANTS_DATA_ARRAY_OFFSETS")
        );

        nint[] servantsArray = await Memory.ReadArrayAsync<nint>(servantsArrayPtr);

        if (!servantsArray.Any())
            return Array.Empty<PartyMemberMetadata>();

        var servants = new PartyMemberMetadata[servantsArray.Length];

        nint[] servantsNamePtrs = servantsArray.Select(async pointer => await Memory.ReadPtrAsync(
            address: pointer,
            offsets: AddressMap.Get<int[]>("SERVANT_NAME_OFFSETS")
        )).Select(it => it.Result)
            .ToArray();

        for (int i = 0; i < servants.Length; i++)
            servants[i] = new PartyMemberMetadata(
                Index: i + 4,
                IsValid: !servantsArray[i].IsNullPointer(),
                Data: new MHRCharacterData { NamePointer = servantsNamePtrs.ElementAtOrDefault(i), HighRank = HighRank, MasterRank = MasterRank }
            );

        return servants;
    }

    [ScannableMethod]
    private async Task GetPlayerSongAbnormalities()
    {
        if (!InHuntingZone)
            return;

        nint songBuffsPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            offsets: AddressMap.Get<int[]>("HH_ABNORMALITIES_OFFSETS")
        );

        if (songBuffsPtr.IsNullPointer())
            return;

        nint[] songBuffPtrs = await Memory.ReadArrayAsync<nint>(songBuffsPtr);

        await DerefSongBuffsAsync(songBuffPtrs);
    }

    [ScannableMethod]
    private async Task GetPlayerStatus()
    {
        if (!InHuntingZone)
            return;

        nint playerHudPtr = await Memory.ReadAsync(
            AddressMap.GetAbsolute("UI_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_HUD_OFFSETS")
        );

        if (playerHudPtr.IsNullPointer())
            return;

        MHRPlayerHudStructure playerHud = await Memory.ReadAsync<MHRPlayerHudStructure>(playerHudPtr);

        MHRPetalaceStatsStructure? petalace = await GetEquippedPetalaceStatsAsync();

        if (petalace is null)
            return;

        var healthData = new HealthData
        {
            MaxHealth = playerHud.MaxHealth,
            Health = playerHud.Health,
            RecoverableHealth = playerHud.RecoverableHealth,
            MaxPossibleHealth = petalace.Value.CalculateMaxPlayerHealth(),
            Heal = playerHud.Heal
        };

        // For when Berserk skill is active
        if (_armorSkills.ContainsKey(137) && SwitchScroll == Scroll.Blue)
            healthData = healthData with
            {
                Health = 0,
                RecoverableHealth = healthData.Health,
                Heal = healthData.Heal > healthData.Health ? healthData.Heal : 0
            };

        _health.Update(healthData);

        var staminaData = new StaminaData
        {
            MaxStamina = playerHud.MaxStamina,
            Stamina = playerHud.Stamina,
            MaxRecoverableStamina = playerHud.MaxExtendableStamina,
            MaxPossibleStamina = petalace.Value.CalculateMaxPlayerStamina(),
        };

        _stamina.Update(staminaData);
    }

    [ScannableMethod(typeof(MHRWirebugData))]
    private async Task GetPlayerWirebugs()
    {
        MHRWirebugCountStructure wirebugCount = await Memory.DerefAsync<MHRWirebugCountStructure>(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("WIREBUG_COUNT_OFFSETS")
        );

        nint wirebugsArrayPtr = await Memory.ReadAsync(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("WIREBUG_DATA_OFFSETS")
        );

        if (wirebugCount.Total() is 0 || wirebugsArrayPtr.IsNullPointer())
        {
            this.Dispatch(_onWirebugsRefresh, Array.Empty<MHRWirebug>());
            return;
        }

        bool isBlocked = await Memory.DerefAsync<int>(
            AddressMap.GetAbsolute("UI_ADDRESS"),
            AddressMap.Get<int[]>("IS_WIREBUG_BLOCKED_OFFSETS")
        ) != 0;

        WirebugState wirebugState = isBlocked ? WirebugState.Blocked :
            _debuffCondition.HasFlag(DebuffConditions.IceBlight) ? WirebugState.IceBlight :
            _commonCondition.HasFlag(CommonConditions.WindMantle) ? WirebugState.WindMantle :
            _commonCondition.HasFlag(CommonConditions.RubyWirebug) ? WirebugState.RubyWirebug :
            _commonCondition.HasFlag(CommonConditions.GoldWirebug) ? WirebugState.GoldWirebug :
            WirebugState.None;

        nint[] wirebugsPtrs = await Memory.ReadArraySafeAsync<nint>(wirebugsArrayPtr, Wirebugs.Length);

        bool shouldDispatchEvent = false;
        for (int i = 0; i < wirebugsPtrs.Length; i++)
        {
            MHRWirebug wirebug = Wirebugs[i];
            nint wirebugPtr = wirebugsPtrs[i];
            WirebugType wirebugType = wirebugCount.ToType(i);

            var data = new MHRWirebugData
            {
                IsAvailable = wirebugType != WirebugType.None,
                IsTemporary = wirebugType is WirebugType.Environment or WirebugType.Skill,
                WirebugState = wirebugState,
                Structure = await Memory.ReadAsync<MHRWirebugStructure>(wirebugPtr)
            };

            data.Structure.Cooldown = data.Structure.Cooldown.ToAbnormalitySeconds();
            data.Structure.MaxCooldown = data.Structure.MaxCooldown.ToAbnormalitySeconds();
            data.Structure.ExtraCooldown = data.Structure.ExtraCooldown.ToAbnormalitySeconds();

            if (wirebugPtr != wirebug.Address)
            {
                shouldDispatchEvent = true;
                wirebug.Address = wirebugPtr;
            }

            if (data.IsTemporary)
            {
                int[] wirebugExtraOffsets = wirebugType switch
                {
                    WirebugType.Environment => AddressMap.Get<int[]>("WIREBUG_EXTRA_DATA_OFFSETS"),
                    _ => AddressMap.Get<int[]>("WIREBUG_EXTRA_DATA_FROM_SKILL_OFFSETS")
                };
                MHRWirebugExtrasStructure temporaryData = await Memory.DerefAsync<MHRWirebugExtrasStructure>(
                    AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
                    wirebugExtraOffsets
                );
                temporaryData.Timer = temporaryData.Timer.ToAbnormalitySeconds();
                wirebug.Update(temporaryData);
            }

            wirebug.Update(data);
        }

        if (shouldDispatchEvent)
            this.Dispatch(_onWirebugsRefresh, Wirebugs);
    }

    [ScannableMethod]
    private async Task GetPlayerConditions()
    {
        if (!InHuntingZone)
        {
            _commonCondition = CommonConditions.None;
            _debuffCondition = DebuffConditions.None;
            return;
        }

        nint conditionPtr = await Memory.ReadAsync(
            AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_CONDITION_OFFSETS")
        );

        if (conditionPtr.IsNullPointer())
        {
            _commonCondition = CommonConditions.None;
            _debuffCondition = DebuffConditions.None;
            return;
        }

        _commonCondition = (CommonConditions)await Memory.ReadAsync<ulong>(conditionPtr + 0x10);
        _debuffCondition = (DebuffConditions)await Memory.ReadAsync<ulong>(conditionPtr + 0x38);
    }

    [ScannableMethod]
    private async Task GetPlayerActionArgs()
    {
        if (!InHuntingZone)
        {
            _actionFlag = ActionFlags.None;
            return;
        }

        nint actionFlagArray = await Memory.ReadAsync(
            AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_ACTIONFLAG_OFFSETS")
        );

        if (actionFlagArray.IsNullPointer())
        {
            _actionFlag = ActionFlags.None;
            return;
        }

        _actionFlag = (ActionFlags)await Memory.ReadAsync<uint>(actionFlagArray + 0x20);
    }

    [ScannableMethod(typeof(MHRSubmarineData))]
    private async Task GetArgosy()
    {
        int maxSubmarinesCount = Argosy.Submarines.Length;
        nint argosyAddress = await Memory.ReadAsync(
            AddressMap.GetAbsolute("ARGOSY_ADDRESS"),
            AddressMap.Get<int[]>("ARGOSY_OFFSETS")
        );

        if (argosyAddress.IsNullPointer())
            return;

        List<MHRSubmarineStructure> submarineStructs = Memory.ReadArrayOfPtrsSafeAsync<MHRSubmarineStructure>(argosyAddress, maxSubmarinesCount)
            .Collect();
        var submarines = new MHRSubmarineData[submarineStructs.Count];

        for (int i = 0; i < submarines.Length; i++)
        {
            MHRSubmarineStructure structure = submarineStructs[i];

            submarines[i].Data = submarineStructs[i];
            submarines[i].Items = Memory.ReadArrayOfPtrsSafeAsync<MHRSubmarineItemEntryStructure>(structure.ItemArrayPtr, 20)
                .ToBlockingEnumerable()
                .Select(async it => await Memory.ReadAsync<MHRSubmarineItemStructure>(it.ItemPointer))
                .Select(it => it.Result)
                .ToArray();
        }

        for (int i = 0; i < submarines.Length; i++)
            Argosy.Submarines[i].Update(submarines[i]);
    }

    [ScannableMethod]
    private async Task GetCohoot()
    {
        MHRCohootStructure cohoot = await Memory.DerefAsync<MHRCohootStructure>(
            address: AddressMap.GetAbsolute("COHOOT_ADDRESS"),
            offsets: AddressMap.Get<int[]>("COHOOT_COUNT_OFFSETS")
        );

        Cohoot.Update(cohoot);
    }

    [ScannableMethod]
    private async Task GetTrainingDojo()
    {
        int[] staticTrainingData = await Memory.ReadAsync<int>(
            address: AddressMap.GetAbsolute("DATA_TRAINING_DOJO_ROUNDS_LEFT"),
            count: 5
        );
        MHRTrainingDojoData data = new()
        {
            Rounds = staticTrainingData[0],
            MaxRounds = staticTrainingData[1],
            Boosts = staticTrainingData[3],
            MaxBoosts = staticTrainingData[4],
            Buddies = new MHRBuddyData[6]
        };

        nint trainingDojo = await Memory.ReadAsync(
            AddressMap.GetAbsolute("TRAINING_DOJO_ADDRESS"),
            AddressMap.Get<int[]>("TRAINING_DOJO_OFFSETS")
        );

        data.BuddiesCount = await Memory.ReadAsync<int>(trainingDojo + 0x18);

        nint trainingDojoBuddyArray = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("TRAINING_DOJO_ADDRESS"),
            offsets: AddressMap.Get<int[]>("TRAINING_DOJO_BUDDY_ARRAY_OFFSETS")
        );

        nint[] buddyPtrs = await Memory.ReadAsync<nint>(trainingDojoBuddyArray + 0x20, 6);

        for (int i = 0; i < data.BuddiesCount; i++)
            data.Buddies[i] = await DerefBuddyDataAsync(buddyPtrs[i]);

        TrainingDojo.Update(data);
    }

    [ScannableMethod]
    private async Task GetMeowcenaries()
    {
        nint meowmastersAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("MEOWMASTERS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("MEOWMASTERS_OFFSETS")
        );

        MHRMeowmasterStructure structure = await Memory.ReadAsync<MHRMeowmasterStructure>(meowmastersAddress);
        MHRMeowmasterData data = new()
        {
            IsDeployed = structure.IsDeployed,
            IsLagniappleActive = structure.IsLagniappleActive,
            BuddiesCount = await Memory.ReadAsync<int>(structure.BuddiesPointer + 0x18),
            CurrentStep = structure.CurrentStep,
            MaxStep = 5
        };

        Meowmasters.Update(data);
    }

    [ScannableMethod]
    private async Task GetPartyData()
    {
        nint partyArrayPtr = await Memory.ReadAsync(
            AddressMap.GetAbsolute("SESSION_PLAYERS_ADDRESS"),
            AddressMap.Get<int[]>("SESSION_PLAYERS_ARRAY_OFFSETS")
        );

        nint[] playerAddresses = await Memory.ReadAsync<nint>(partyArrayPtr + 0x20, _party.MaxSize);

        int membersCount = playerAddresses.Count(address => address != 0x0);

        _party.SetSize(membersCount);
    }

    private async Task<MHRBuddyData> DerefBuddyDataAsync(nint buddyPtr)
    {
        nint namePtr = await Memory.ReadPtrAsync(buddyPtr, AddressMap.Get<int[]>("BUDDY_NAME_OFFSETS"));
        nint levelPtr = await Memory.ReadPtrAsync(buddyPtr, AddressMap.Get<int[]>("BUDDY_LEVEL_OFFSETS"));
        int nameLength = await Memory.ReadAsync<int>(namePtr + 0x10);

        MHRBuddyData data = new()
        {
            Name = await Memory.ReadAsync(
                address: namePtr + 0x14,
                length: nameLength * 2,
                encoding: Encoding.Unicode
            ),
            Level = await Memory.ReadAsync<int>(levelPtr + 0x24)
        };

        return data;
    }

    private async Task DerefSongBuffsAsync(nint[] buffs)
    {
        int id = 0;
        AbnormalityDefinition[] schemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.Rise, AbnormalityGroup.SONGS);
        foreach (nint buffPtr in buffs)
        {
            MHRHHAbnormality abnormality = await Memory.ReadAsync<MHRHHAbnormality>(buffPtr);
            abnormality.Timer = abnormality.Timer.ToAbnormalitySeconds();

            AbnormalityDefinition schema = schemas[id];

            HandleAbnormality<MHRSongAbnormality, MHRHHAbnormality>(
                abnormalities: _abnormalities,
                schema: schema,
                timer: abnormality.Timer,
                newData: abnormality
            );

            id++;
        }
    }

    private async Task<MHRPetalaceStatsStructure?> GetEquippedPetalaceStatsAsync()
    {
        nint petalaceArray = await Memory.ReadAsync(
            AddressMap.GetAbsolute("GEAR_ADDRESS"),
            AddressMap.Get<int[]>("PETALACES_ARRAY_OFFSETS")
        );

        if (petalaceArray == 0)
            return null;

        int selectedPetalaceId = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("PLAYER_GEAR_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SELECTED_PETALACE_OFFSETS")
        ) & 0x0000FFFF;

        nint[] petalacePtrs = await Memory.ReadArrayAsync<nint>(petalaceArray);

        MHRPetalaceStructure structure = await Memory.ReadAsync<MHRPetalaceStructure>(
            petalacePtrs[selectedPetalaceId % petalacePtrs.Length]
        );

        MHRPetalaceDataStructure data = await Memory.ReadAsync<MHRPetalaceDataStructure>(structure.Data);

        return await Memory.ReadAsync<MHRPetalaceStatsStructure>(data.Stats);
    }

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        foreach (EntityDamageData entity in entities)
            _party.Update(entity);
    }

    public override void Dispose()
    {
        Wirebugs.DisposeAll();
        Argosy.Dispose();
        TrainingDojo.Dispose();
        Meowmasters.Dispose();
        Cohoot.Dispose();
        _onWirebugsRefresh.Dispose();
        base.Dispose();
    }
}
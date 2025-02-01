using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
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
using HunterPie.Integrations.Datasources.Common.Definition;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;
using HealthData = HunterPie.Integrations.Datasources.Common.Definition.HealthData;
using SpecializedTool = HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.MHWSpecializedTool;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public sealed class MHWPlayer : CommonPlayer
{
    #region consts
    private static readonly Stage[] PeaceZones =
    {
        Stage.Astera,
        Stage.AsteraGatheringHub,
        Stage.ResearchBase,
        Stage.Seliana,
        Stage.SelianaGatheringHub,
        Stage.LivingQuarters,
        Stage.PrivateQuarters,
        Stage.PrivateSuite,
        Stage.SelianaRoom
    };
    #endregion

    #region Private fields

    private nint _playerSaveAddress;
    private nint _localPlayerAddress;
    private Stage _zoneId;
    private Weapon _weaponId = WeaponType.None;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    private readonly MHWParty _party = new();
    private IWeapon _weapon;
    private int _highRank;
    private int _masterRank;
    private readonly HealthComponent _health = new();
    private readonly StaminaComponent _stamina = new();
    private readonly MHWSkillService _skillService;

    #endregion

    #region Public Properties

    public nint PlayerSaveAddress
    {
        get => _playerSaveAddress;
        private set
        {
            if (value != _playerSaveAddress)
            {
                _playerSaveAddress = value;

                this.Dispatch(
                    value != 0
                    ? _onLogin
                    : _onLogout
                );

            }
        }
    }

    public override string Name { get; protected set; } = string.Empty;

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

    public int PlayTime { get; private set; }

    public Stage ZoneId
    {
        get => _zoneId;
        set
        {
            if (value != _zoneId)
            {
                if (PeaceZones.Contains(value) && !PeaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageEnter);
                else if (!PeaceZones.Contains(value) && PeaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageLeave);

                _zoneId = value;
                this.Dispatch(_onStageUpdate);
            }
        }
    }

    public SpecializedTool[] Tools { get; } = { new(), new() };

    public bool IsLoggedOn => _playerSaveAddress != 0;

    public override int StageId
    {
        get => (int)ZoneId;
        protected set => throw new NotSupportedException();
    }

    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IParty Party => _party;

    public override bool InHuntingZone => ZoneId != Stage.MainMenu
                                 && !PeaceZones.Contains(_zoneId);

    public override IHealthComponent Health => _health;

    public override IStaminaComponent Stamina => _stamina;

    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                IWeapon temp = _weapon;
                _weapon = value;
                this.Dispatch(_onWeaponChange, new WeaponChangeEventArgs(temp, _weapon));

                if (temp is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }

    public MHWHarvestBox HarvestBox { get; } = new();

    public MHWSteamworks Steamworks { get; } = new();

    public MHWArgosy Argosy { get; } = new();

    public MHWTailraiders Tailraiders { get; } = new();
    #endregion

    internal MHWPlayer(
        IGameProcess process,
        IScanService scanService) : base(process, scanService)
    {
        _skillService = new MHWSkillService(process, scanService);
        _weapon = CreateDefaultWeapon();
    }

    private IWeapon CreateDefaultWeapon()
    {
        var weapon = new MHWMeleeWeapon(
            process: Process,
            scanService: ScanService,
            skillService: _skillService,
            id: WeaponType.Greatsword
        );
        return weapon;
    }

    [ScannableMethod]
    private async Task GetZoneData()
    {
        nint zoneAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ZONE_OFFSET"),
            offsets: AddressMap.Get<int[]>("ZoneOffsets")
        );

        ZoneId = (Stage)await Memory.ReadAsync<int>(zoneAddress);
    }

    [ScannableMethod]
    private async Task GetBasicData()
    {
        if (ZoneId == Stage.MainMenu)
        {
            PlayerSaveAddress = 0;
            return;
        }

        nint firstSaveAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("LEVEL_OFFSET"),
            offsets: AddressMap.Get<int[]>("LevelOffsets")
        );

        uint currentSaveSlot = await Memory.ReadAsync<uint>(firstSaveAddress + 0x44);
        const nint nextPlayerSave = 0x26CC00;
        nint currentPlayerSaveHeader = await Memory.ReadAsync<nint>(firstSaveAddress) + (nextPlayerSave * (nint)currentSaveSlot);

        if (currentPlayerSaveHeader == _playerSaveAddress)
            return;

        Name = await Memory.ReadAsync(currentPlayerSaveHeader + 0x50, 32);
        HighRank = await Memory.ReadAsync<short>(currentPlayerSaveHeader + 0x90);
        MasterRank = await Memory.ReadAsync<short>(currentPlayerSaveHeader + 0xD4);
        PlayTime = await Memory.ReadAsync<int>(currentPlayerSaveHeader + 0xA0);
        PlayerSaveAddress = currentPlayerSaveHeader;
    }



    [ScannableMethod]
    private async Task GetPlayerHudData()
    {
        nint basicPlayerDataPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            offsets: AddressMap.Get<int[]>("PLAYER_BASIC_INFORMATION_OFFSETS")
        );

        MHWHudStructure hudStructure = await Memory.ReadAsync<MHWHudStructure>(basicPlayerDataPtr);

        MHWHealingStructure totalHealing = (await Memory.ReadAsync<MHWHealingStructure>(hudStructure.HealingArrayPointer + 0xEBB0, 4))
                                                           .ToTotal();

        var healthData = new HealthData
        {
            MaxHealth = hudStructure.MaxHealth,
            Health = hudStructure.Health,
            RecoverableHealth = hudStructure.RecoverableHealth,
            Heal = hudStructure.Health + totalHealing.MaxHeal - totalHealing.Heal,
            MaxPossibleHealth = Math.Max(_skillService.ToMaximumHealthPossible(), hudStructure.MaxHealth),
        };

        _health.Update(healthData);

        var staminaData = new StaminaData
        {
            MaxStamina = hudStructure.MaxStamina,
            Stamina = hudStructure.Stamina,
            MaxPossibleStamina = Math.Max(_skillService.ToMaximumStaminaPossible(), hudStructure.MaxStamina)
        };

        _stamina.Update(staminaData);
    }

    [ScannableMethod]
    private async Task GetWeaponData()
    {
        PlayerEquipmentData data = new();

        if (!IsLoggedOn)
            return;

        nint address = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );

        data.WeaponType = (WeaponType)await Memory.ReadAsync<byte>(address);

        if (data.WeaponType == _weaponId)
            return;

        if (Weapon is IDisposable disposable)
            disposable.Dispose();

        IWeapon? weaponInstance = data.WeaponType switch
        {
            WeaponType.ChargeBlade => new MHWChargeBlade(Process, _skillService, ScanService),
            WeaponType.InsectGlaive => new MHWInsectGlaive(Process, _skillService, ScanService),
            WeaponType.Bow => new MHWBow(),
            WeaponType.HeavyBowgun => new MHWHeavyBowgun(),
            WeaponType.LightBowgun => new MHWLightBowgun(),
            WeaponType.DualBlades => new MHWDualBlades(Process, _skillService, ScanService),
            WeaponType.SwitchAxe => new MHWSwitchAxe(Process, _skillService, ScanService),
            WeaponType.Longsword => new MHWLongSword(Process, _skillService, ScanService),
            WeaponType.Greatsword
                or WeaponType.SwordAndShield
                or WeaponType.Hammer
                or WeaponType.HuntingHorn
                or WeaponType.Lance
                or WeaponType.GunLance
                or WeaponType.GunLance => new MHWMeleeWeapon(Process, ScanService, _skillService, data.WeaponType),
            _ => null
        };

        if (weaponInstance is not { })
            return;

        Weapon = weaponInstance;
        _weaponId = data.WeaponType;
    }

    [ScannableMethod]
    private async Task GetParty()
    {
        MHWQuestStructure quest = await Memory.DerefAsync<MHWQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_DATA_OFFSETS")
        );

        if (quest.State.IsQuestOver() || quest.Id <= 0)
            return;

        nint partyInformationPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("PARTY_ADDRESS"),
            offsets: AddressMap.Get<int[]>("PARTY_OFFSETS")
        );

        nint damageInformation = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("DAMAGE_ADDRESS"),
            offsets: AddressMap.Get<int[]>("DAMAGE_OFFSETS")
        );

        int partySize = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("SESSION_OFFSET"),
            offsets: AddressMap.Get<int[]>("SESSION_PARTY_OFFSETS")
        );

        if (partySize is 0)
        {
            _party.ClearExcept(0);
            _localPlayerAddress = 0;
            _party.Update(0, new MHWPartyMemberData
            {
                Name = Name,
                Weapon = _weaponId,
                Damage = 0,
                Slot = 0,
                IsMyself = true,
                MasterRank = MasterRank
            });

            return;
        }

        _party.Remove(0);

        MHWPartyMemberStructure[] partyMembers = await Memory.ReadAsync<MHWPartyMemberStructure>(partyInformationPtr, 4);

        nint localPlayerReference = 0;
        int index = -1;
        foreach (MHWPartyMemberStructure partyMember in partyMembers)
        {
            index++;

            string name = await Memory.ReadAsync(partyMember.Address + 0x49, 32);

            bool isInParty = !string.IsNullOrEmpty(name);

            if (!isInParty)
            {
                _party.Remove(partyMember.Address);
                continue;
            }

            MHWPartyMemberLevelStructure levels = await Memory.ReadAsync<MHWPartyMemberLevelStructure>(partyMember.Address + 0x70);

            bool isLocalPlayer = name == Name;

            if (isLocalPlayer)
                localPlayerReference = partyMember.Address;

            var data = new MHWPartyMemberData
            {
                Name = name,
                Weapon = isLocalPlayer ? _weaponId : (WeaponType)await Memory.ReadAsync<byte>(partyMember.Address + 0x7C),
                Damage = await Memory.ReadAsync<int>(damageInformation + (index * 0x2A0)),
                Slot = index,
                IsMyself = isLocalPlayer,
                MasterRank = levels.MasterRank
            };

            _party.Update(partyMember.Address, data);
        }

        _localPlayerAddress = localPlayerReference;
    }

    [ScannableMethod]
    private async Task GetHarvestBoxData()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        nint harvestBoxPtr = PlayerSaveAddress + 0x103068;

        MHWFertilizerStructure[] fertilizers = await Memory.ReadAsync<MHWFertilizerStructure>(harvestBoxPtr, 4);
        MHWItemStructure[] harvestBoxItems = await Memory.ReadAsync<MHWItemStructure>(harvestBoxPtr + 0x50, 50);

        var data = new MHWHarvestBoxData(
            Items: harvestBoxItems,
            Fertilizers: fertilizers
        );

        HarvestBox.Update(data);
    }

    [ScannableMethod]
    private async Task GetSteamFuel()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        MHWSteamFuelStructure structure = await Memory.ReadAsync<MHWSteamFuelStructure>(PlayerSaveAddress + 0x102FDC);

        var data = new MHWSteamFuelData(
            NaturalFuel: structure.NaturalFuel,
            StoredFuel: structure.StoredFuel
        );

        Steamworks.Update(data);
    }

    [ScannableMethod]
    private async Task GetArgosy()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        nint argosyPtr = PlayerSaveAddress + 0x1034C0;

        byte argosyDays = await Memory.ReadAsync<byte>(argosyPtr);
        bool isInTown = argosyDays < 250;

        if (!isInTown)
            argosyDays = (byte)(byte.MaxValue - argosyDays + 1);

        var data = new MHWArgosyData(
            DaysLeft: argosyDays,
            IsInTown: isInTown
        );

        Argosy.Update(data);
    }

    [ScannableMethod]
    private async Task GetTailraiders()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        nint tailraidersPtr = PlayerSaveAddress + 0x1034DC;

        byte tailraidersQuests = await Memory.ReadAsync<byte>(tailraidersPtr);
        bool isDeployed = tailraidersQuests != byte.MaxValue;
        int questsLeft = isDeployed
            ? Tailraiders.MaxDays - tailraidersQuests
            : 0;

        var data = new MHWTailraidersData(
            QuestsLeft: questsLeft,
            IsDeployed: isDeployed
        );

        Tailraiders.Update(data);
    }

    [ScannableMethod]
    private async Task GetMantlesData()
    {
        nint address = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );
        SpecializedToolType[] ids = (await Process.Memory.ReadAsync<int>(address + 0x34, 2))
            .Select(e => (SpecializedToolType)e)
            .ToArray();

        nint equipmentAddress = await Memory.ReadAsync(
            AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            AddressMap.Get<int[]>("EQUIPMENT_OFFSETS")
        );

        const int specializedTools = 20;
        float[] cooldowns = await Memory.ReadAsync<float>(equipmentAddress + 0x99C, specializedTools * 2);
        float[] timers = await Memory.ReadAsync<float>(equipmentAddress + 0xA8C, specializedTools * 2);

        for (int i = 0; i < Tools.Length; i++)
        {
            int id = (int)ids[i];
            var tool = new MHWSpecializedToolStructure
            {
                Id = ids[i],
                Timer = timers.ElementAtOrDefault(id),
                MaxTimer = timers.ElementAtOrDefault(id + specializedTools),
                Cooldown = cooldowns.ElementAtOrDefault(id),
                MaxCooldown = cooldowns.ElementAtOrDefault(id + specializedTools),
            };
            IUpdatable<MHWSpecializedToolStructure> mhwTool = Tools[i];
            mhwTool.Update(tool);
        }
    }

    [ScannableMethod]
    private async Task GetAbnormalitiesCleanup()
    {
        nint abnormalityBaseAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            offsets: AddressMap.Get<int[]>("ABNORMALITY_OFFSETS")
        );

        if (!InHuntingZone || abnormalityBaseAddress == 0)
            ClearAbnormalities(_abnormalities);
    }

    [ScannableMethod]
    private async Task GetAbnormalities()
    {
        if (!InHuntingZone)
            return;

        nint abnormalityBaseAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            offsets: AddressMap.Get<int[]>("ABNORMALITY_OFFSETS")
        );

        MHWAbnormalityStructure[] abnormalities = await Memory.ReadAsync<MHWAbnormalityStructure>(abnormalityBaseAddress + 0x38, 75);

        GetHuntingHornAbnormalities(abnormalities);
        GetOrchestraAbnormalities(abnormalities);

        Task.WaitAll(
            GetDebuffAbnormalitiesAsync(abnormalityBaseAddress),
            GetConsumableAbnormalitiesAsync(abnormalityBaseAddress),
            GetSkillAbnormalitiesAsync(abnormalityBaseAddress),
            GetFoodAbnormalitiesAsync(),
            GetGearAbnormalitiesAsync()
        );
    }

    private void GetHuntingHornAbnormalities(MHWAbnormalityStructure[] buffs)
    {
        AbnormalityDefinition[] abnormalitySchemas =
            AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.SONGS);
        int offsetFirstAbnormality = abnormalitySchemas[0].Offset;

        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            // We can calculate the index of the abnormality based on their offset and the size of a float
            int index = (abnormalitySchema.Offset - offsetFirstAbnormality) / sizeof(float);
            MHWAbnormalityStructure structure = buffs[index];

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private void GetOrchestraAbnormalities(MHWAbnormalityStructure[] buffs)
    {
        AbnormalityDefinition[] abnormalitySchemas =
            AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.ORCHESTRA);
        int offsetFirstAbnormality = abnormalitySchemas[0].Offset;
        int indexFirstOrchestraAbnormality = (offsetFirstAbnormality - 0x38) / sizeof(float);

        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            int index = ((abnormalitySchema.Offset - offsetFirstAbnormality) / sizeof(float)) + indexFirstOrchestraAbnormality;
            MHWAbnormalityStructure structure = buffs[index];

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private async Task GetDebuffAbnormalitiesAsync(nint baseAddress)
    {
        AbnormalityDefinition[] abnormalitySchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.DEBUFFS);

        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = new();

            int abnormSubId = abnormalitySchema.DependsOn switch
            {
                0 => 0,
                _ => await Memory.ReadAsync<int>(baseAddress + abnormalitySchema.DependsOn)
            };

            if (abnormSubId == abnormalitySchema.WithValue)
                structure = await Memory.ReadAsync<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private async Task GetConsumableAbnormalitiesAsync(nint baseAddress)
    {
        AbnormalityDefinition[] abnormalitySchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.CONSUMABLES);

        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = new();

            int abnormSubId = abnormalitySchema.DependsOn switch
            {
                0 => 0,
                _ => await Memory.ReadAsync<int>(baseAddress + abnormalitySchema.DependsOn)
            };

            if (abnormSubId == abnormalitySchema.WithValue)
            {
                structure = await Memory.ReadAsync<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

                if (abnormalitySchema.IsInfinite)
                    structure.Timer = 1;
            }

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private async Task GetSkillAbnormalitiesAsync(nint baseAddress)
    {
        AbnormalityDefinition[] abnormalitySchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.SKILLS);

        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = await Memory.ReadAsync<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private async Task GetFoodAbnormalitiesAsync()
    {
        nint canteenAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("CANTEEN_ADDRESS"),
            offsets: AddressMap.Get<int[]>("ABNORMALITY_CANTEEN_OFFSETS")
        );

        AbnormalityDefinition[] abnormalitySchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.FOODS);
        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = await Memory.ReadAsync<MHWAbnormalityStructure>(canteenAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private async Task GetGearAbnormalitiesAsync()
    {
        nint gearAddress = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            offsets: AddressMap.Get<int[]>("ABNORMALITY_GEAR_OFFSETS")
        );

        AbnormalityDefinition[] abnormalitySchemas = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.GEARS);
        foreach (AbnormalityDefinition abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = await Memory.ReadAsync<MHWAbnormalityStructure>(gearAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        // Only update damage for index 0 since it is only possible to track the local player's damage
        entities.Where(it => it.Entity.Index == 0)
            .ForEach(it => _party.Update(_localPlayerAddress, it));
    }

    public override void Dispose()
    {
        _skillService.Dispose();
        Tools.DisposeAll();
        HarvestBox.Dispose();
        Steamworks.Dispose();
        base.Dispose();
    }
}
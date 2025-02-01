using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;
using System.Diagnostics;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;

public sealed class MHWGame : CommonGame
{
    private readonly MHWPlayer _player;
    private readonly Dictionary<nint, IMonster> _monsters = new();
    private readonly Dictionary<nint, EntityDamageData[]> _damageDone = new();
    private bool _isMouseVisible;
    private readonly Stopwatch _localTimerStopwatch = new();
    private readonly Stopwatch _damageUpdateThrottleStopwatch = new();

    public override IPlayer Player => _player;

    public override List<IMonster> Monsters { get; } = new();

    public override IChat? Chat => null;

    public override bool IsHudOpen
    {
        get => _isMouseVisible;
        protected set
        {
            if (value != _isMouseVisible)
            {
                _isMouseVisible = value;
                this.Dispatch(_onHudStateChange, this);
            }
        }
    }

    public override float TimeElapsed { get; protected set; }

    private MHWQuest? _quest;
    public override IQuest? Quest => _quest;

    public override IAbnormalityCategorizationService AbnormalityCategorizationService { get; } = new MHWAbnormalityCategorizatonService();

    public MHWGame(
        IGameProcess process,
        IScanService scanService) : base(process, scanService)
    {
        _player = new MHWPlayer(process, scanService);
        DamageMessageHandler.OnReceived += OnReceivePlayersDamage;
        _player.OnStageUpdate += OnPlayerStageUpdate;
    }

    [ScannableMethod]
    private async Task GetHudVisibility()
    {
        bool isMouseVisible = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("HUD_MENU_ADDRESS"),
            offsets: AddressMap.Get<int[]>("HUD_MENU_OPEN_OFFSETS")
        ) == 1;

        IsHudOpen = isMouseVisible;
    }

    [ScannableMethod]
    private async Task GetWorldData()
    {
        MHWWorldDataStructure worldData = await Memory.DerefAsync<MHWWorldDataStructure>(
            address: AddressMap.GetAbsolute("WORLD_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("WORLD_DATA_OFFSETS")
        );

        float hour = Math.Abs(worldData.WorldTime);
        float minute = Math.Abs(60 * (worldData.WorldTime % 1));
        WorldTime = new TimeOnly((int)hour, (int)minute);
    }

    [ScannableMethod]
    private async Task GetTimeElapsed()
    {
        nint questEndTimerPtrs = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("QUEST_TIMER_OFFSETS")
        );
        ulong timer = await Memory.ReadAsync<ulong>(questEndTimerPtrs);
        uint questMaxTimerRaw = await Process.Memory.ReadAsync<uint>(questEndTimerPtrs + 0x10);

        float elapsed = MHWCrypto.LiterallyWhyCapcom(timer);

        MHWQuestStructure quest = await Memory.DerefAsync<MHWQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_DATA_OFFSETS")
        );

        if (Quest is null && !quest.State.IsQuestOver())
        {
            if (!Player.InHuntingZone)
            {
                SetTimeElapsed(0, false);
                return;
            }

            if (!_localTimerStopwatch.IsRunning)
            {
                _localTimerStopwatch.Start();
                SetTimeElapsed(0, true);
                return;
            }

            SetTimeElapsed(_localTimerStopwatch.ElapsedMilliseconds / 1000.0f, false);

            return;
        }

        float questMaxTimer = questMaxTimerRaw
                .ApproximateHigh(MHWGameUtils.MaxQuestTimers)
                .ToSeconds();

        float timeElapsed = Math.Max(0, questMaxTimer - elapsed);

        if (!_localTimerStopwatch.IsRunning)
        {
            SetTimeElapsed(timeElapsed, TimeElapsed == 0);
            return;
        }

        _localTimerStopwatch.Reset();
        SetTimeElapsed(timeElapsed, true);
    }

    [ScannableMethod]
    private async Task GetQuest()
    {
        MHWQuestStructure quest = await Memory.DerefAsync<MHWQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_DATA_OFFSETS")
        );

        bool hasQuestStarted = quest.State == QuestState.InQuest;
        bool isQuestOver = quest.State.IsQuestOver() || !hasQuestStarted;
        bool isQuestInvalid = quest.Id <= 0;

        if (_quest is not null
            && (isQuestOver || isQuestInvalid))
        {
            this.Dispatch(_onQuestEnd, new QuestEndEventArgs(_quest, quest.State.ToStatus(), TimeElapsed));
            _quest.Dispose();
            _quest = null;
        }

        if (_quest is null
            && !isQuestOver
            && !isQuestInvalid)
        {
            var questType = quest.Category.ToQuestType();

            _quest = new MHWQuest(
                process: Process,
                scanService: ScanService,
                id: quest.Id,
                stars: quest.Stars,
                questType: questType
            );

            this.Dispatch(_onQuestStart, _quest);
        }

    }

    [ScannableMethod]
    private async Task GetPartyMembersDamage()
    {
        if (_damageUpdateThrottleStopwatch.IsRunning && _damageUpdateThrottleStopwatch.ElapsedMilliseconds < 100)
            return;

        _damageUpdateThrottleStopwatch.Restart();

        if (!Player.InHuntingZone)
            return;

        await DamageMessageHandler.RequestHuntStatisticsAsync(CommonConstants.AllTargets);
    }

    [ScannableMethod]
    private async Task GetMonsters()
    {
        nint monsterComponentsPointer = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("MONSTER_LIST_ADDRESS"),
            offsets: AddressMap.GetOffsets("MONSTER_LIST_OFFSETS")
        );
        (nint monsterPtr, string em)[] bigMonsters = (await Memory.ReadAsync<nint>(monsterComponentsPointer, 128))
            .AsParallel()
            .Where(component => !component.IsNullPointer())
            .Select(async component => await Memory.ReadAsync<nint>(component + 0x138))
            .AwaitResults()
            .Select(async monsterPtr => (monsterPtr, em: await Memory.ReadStringAsync(monsterPtr + 0x2A0, 64)))
            .AwaitResults()
            .Where(it => it.em.StartsWith("em\\em") && !it.em.StartsWith("em\\ems"))
            .ToArray();

        foreach ((nint monsterPtr, string em) in bigMonsters)
            await HandleMonsterSpawnAsync(monsterPtr, em);

        nint[] monsterPtrs = bigMonsters.Select(it => it.monsterPtr)
            .ToArray();

        _monsters.Keys.Where(monsterPtr => !monsterPtrs.Contains(monsterPtr))
            .ForEach(HandleMonsterDespawn);
    }

    private async Task HandleMonsterSpawnAsync(nint address, string em)
    {
        if (_monsters.ContainsKey(address))
            return;

        int id = await Memory.ReadAsync<int>(address + 0x12280);

        var monster = new MHWMonster(
            process: Process,
            scanService: ScanService,
            address: address,
            id: id,
            em: em
        );
        _monsters.Add(address, monster);
        Monsters.Add(monster);

        this.Dispatch(_onMonsterSpawn, monster);
    }

    private void HandleMonsterDespawn(nint address)
    {
        if (_monsters[address] is not MHWMonster monster)
            return;

        _ = _monsters.Remove(address);
        _ = Monsters.Remove(monster);

        this.Dispatch(_onMonsterDespawn, monster);

        monster.Dispose();
    }

    public override void Dispose()
    {
        DamageMessageHandler.OnReceived -= OnReceivePlayersDamage;
        _player.OnStageUpdate -= OnPlayerStageUpdate;
        base.Dispose();
    }

    private void SetTimeElapsed(float value, bool isReset)
    {
        if (value == TimeElapsed)
            return;

        TimeElapsed = value;
        this.Dispatch(_onTimeElapsedChange, new TimeElapsedChangeEventArgs(isReset, value));
    }

    #region Damage helpers

    private async void OnPlayerStageUpdate(object? sender, EventArgs e)
    {
        if (!Player.InHuntingZone)
            // When back from hunt, manually clear damage data in case player enters Training Area (data won't be reset)
            foreach (MHWPartyMember member in Player.Party.Members.Cast<MHWPartyMember>())
                member.ResetDamage();

        await DamageMessageHandler.ClearAllHuntStatisticsExceptAsync(Array.Empty<nint>());
        await DamageMessageHandler.RequestHuntStatisticsAsync(CommonConstants.AllTargets);
        _damageUpdateThrottleStopwatch.Reset();
        _localTimerStopwatch.Reset();
    }

    private void OnReceivePlayersDamage(object? sender, ResponseDamageMessage e)
    {
        nint target = e.Target;

        _damageDone[target] = e.Entities;

        EntityDamageData[] damages = _damageDone.Values.SelectMany(entity => entity)
            .GroupBy(entity => entity.Entity.Index)
            .Select(group =>
            {
                EntityDamageData entity = group.ElementAt(0);

                return entity with
                {
                    RawDamage = group.Sum(it => it.RawDamage),
                    ElementalDamage = group.Sum(it => it.ElementalDamage)
                };
            })
            .ToArray();

        _player.UpdatePartyMembersDamage(damages);
    }
    #endregion
}
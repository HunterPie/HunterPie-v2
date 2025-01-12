
using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.World;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Chat;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Text;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;

public sealed class MHRGame : CommonGame
{
    public const int MAXIMUM_MONSTER_ARRAY_SIZE = 5;
    public const int TRAINING_ROOM_ID = 5;

    private readonly MHRChat _chat = new();
    private readonly MHRPlayer _player;
    private float _timeElapsed;
    private (int, DateTime) _lastTeleport = (0, DateTime.Now);
    private bool _isHudOpen;
    private DateTime _lastDamageUpdate = DateTime.MinValue;
    private readonly Dictionary<IntPtr, IMonster> _monsters = new();
    private readonly Dictionary<IntPtr, EntityDamageData[]> _damageDone = new();

    public override IPlayer Player => _player;
    public override List<IMonster> Monsters { get; } = new();

    public override IChat Chat => _chat;

    public override bool IsHudOpen
    {
        get => _isHudOpen;
        protected set
        {
            if (value != _isHudOpen)
            {
                _isHudOpen = value;
                this.Dispatch(_onHudStateChange, this);
            }
        }
    }

    public override float TimeElapsed
    {
        get => _timeElapsed;
        protected set
        {
            if (value != _timeElapsed)
            {
                bool hasReset = value - _timeElapsed > 5;

                _timeElapsed = value;
                this.Dispatch(_onTimeElapsedChange, new TimeElapsedChangeEventArgs(hasReset, value));
            }
        }
    }

    private MHRQuest? _quest;
    public override IQuest? Quest => _quest;

    public override IAbnormalityCategorizationService AbnormalityCategorizationService { get; } = new MHRAbnormalityCategorizationService();

    public MHRGame(IGameProcess process) : base(process)
    {
        _player = new MHRPlayer(process);

        ScanManager.Add(
            this,
            _player
        );

        HookEvents();
    }

    private void HookEvents()
    {
        DamageMessageHandler.OnReceived += OnReceivePlayersDamage;
        _player.OnStageUpdate += OnPlayerStageUpdate;
    }

    public override void Dispose()
    {
        DamageMessageHandler.OnReceived -= OnReceivePlayersDamage;
        _player.OnStageUpdate -= OnPlayerStageUpdate;
        base.Dispose();
    }

    [ScannableMethod]
    private async Task ScanChat()
    {
        IntPtr chatArrayPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("CHAT_ADDRESS"),
            offsets: AddressMap.GetOffsets("CHAT_OFFSETS")
        );
        IntPtr chatArray = await Memory.ReadAsync<IntPtr>(chatArrayPtr);
        int chatCount = await Memory.ReadAsync<int>(chatArrayPtr + 0x8);

        if (chatCount <= 0)
            return;

        IntPtr[] chatMessagePtrs = await Memory.ReadAsync<IntPtr>(chatArray + 0x20, chatCount);

        bool isChatOpen = false;

        for (int i = 0; i < chatCount; i++)
        {
            IntPtr messagePtr = chatMessagePtrs[i];

            MHRChatMessageStructure message = await Memory.ReadAsync<MHRChatMessageStructure>(messagePtr);

            if (message.Type is not 0 and not 1)
                continue;

            if (!isChatOpen)
                isChatOpen |= message.Visibility == 2;

            if (_chat.ContainsMessage(messagePtr))
                continue;

            MHRChatMessage messageData = await DerefChatMessageAsync(message);

            _chat.AddMessage(messagePtr, messageData);
        }

        if (!isChatOpen)
            isChatOpen |= await Memory.DerefAsync<byte>(
                address: AddressMap.GetAbsolute("CHAT_UI_ADDRESS"),
                offsets: AddressMap.GetOffsets("CHAT_UI_OFFSETS")
            ) == 1;

        _chat.SetChatState(isChatOpen);
    }

    [ScannableMethod]
    private async Task GetElapsedTime()
    {
        float elapsedTime = await Memory.DerefAsync<float>(
            address: AddressMap.GetAbsolute("QUEST_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_TIMER_OFFSETS")
        );

        TimeElapsed = elapsedTime > 0
            ? elapsedTime
            : (float)(DateTime.Now - _lastTeleport.Item2).TotalSeconds;

        if (Player.StageId != _lastTeleport.Item1)
            _lastTeleport = (Player.StageId, DateTime.Now);

    }

    [ScannableMethod]
    private async Task GetWorldData()
    {
        IntPtr timersArrayPtr = await Memory.DerefAsync<IntPtr>(
            address: AddressMap.GetAbsolute("STAGE_MANAGER_ADDRESS"),
            offsets: AddressMap.GetOffsets("WORLD_TIME_OFFSETS")
        );
        List<MHRWorldTimeStructure> timers = Memory.ReadListOfPtrsSafeAsync<MHRWorldTimeStructure>(
           address: timersArrayPtr,
           size: 3
        ).Collect();
        MHRWorldTimeStructure lastTimer = timers.LastOrDefault(default(MHRWorldTimeStructure));

        if (lastTimer is not { Hours: <= 24 and >= 0, Minutes: <= 60 and >= 0, Seconds: <= 60 and >= 0 })
            return;

        WorldTime = new TimeOnly(lastTimer.Hours, lastTimer.Minutes, lastTimer.Seconds);
    }

    [ScannableMethod]
    private async Task GetQuest()
    {
        MHRQuestStructure questStructure = await Memory.DerefAsync<MHRQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_OFFSETS")
        );

        MHRQuestDataStructure questData = await Memory.ReadAsync<MHRQuestDataStructure>(questStructure.QuestDataPointer);
        MHRQuestData? currentQuest = await questData.GetCurrentQuestAsync(Memory);

        var questType = questStructure.Type.ToQuestType();
        bool hasQuestStarted = questStructure.State == QuestState.InQuest;
        bool isQuestOver = questStructure.State.IsQuestOver() || !hasQuestStarted;
        bool isQuestInvalid = (currentQuest?.Id ?? 0) <= 0 || questType is null;

        if (_quest is not null
            && (isQuestOver || isQuestInvalid))
        {
            this.Dispatch(_onQuestEnd, new QuestEndEventArgs(_quest, questStructure.State.ToQuestStatus(), TimeElapsed));
            ScanManager.Remove(_quest);
            _quest = null;
        }

        if (_quest is null
            && !isQuestOver
            && !isQuestInvalid
            && currentQuest is { } quest)
        {
            _quest = new MHRQuest(
                process: Process,
                id: quest.Id,
                type: questType!.Value,
                level: quest.Level,
                stars: quest.Stars
            );

            ScanManager.Add(_quest);
            this.Dispatch(_onQuestStart, _quest);
        }
    }

    [ScannableMethod]
    private async Task GetPartyMembersDamage()
    {
        if ((DateTime.Now - _lastDamageUpdate).TotalMilliseconds < 100)
            return;

        _lastDamageUpdate = DateTime.Now;

        if (!Player.InHuntingZone)
            return;

        await DamageMessageHandler.RequestHuntStatisticsAsync(CommonConstants.AllTargets);
    }

    [ScannableMethod]
    private async Task GetUiState()
    {
        byte isHudOpen = await Memory.DerefAsync<byte>(
            address: AddressMap.GetAbsolute("MOUSE_ADDRESS"),
            offsets: AddressMap.GetOffsets("MOUSE_OFFSETS")
        );

        byte isCutsceneActive = await Memory.DerefAsync<byte>(
            address: AddressMap.GetAbsolute("EVENTCAMERA_ADDRESS"),
            offsets: AddressMap.GetOffsets("CUTSCENE_STATE_OFFSETS")
        );

        IsHudOpen = isHudOpen == 1 || isCutsceneActive != 0;
    }

    [ScannableMethod]
    private async Task GetMonstersArray()
    {
        // Only scans for monsters in hunting areas
        if (!Player.InHuntingZone && Player.StageId != TRAINING_ROOM_ID)
        {
            if (_monsters.Keys.Count <= 0)
                return;

            foreach (IntPtr mAddress in _monsters.Keys)
                HandleMonsterDespawn(mAddress);

            return;
        }

        IntPtr address = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("MONSTERS_ADDRESS"),
            offsets: AddressMap.GetOffsets("MONSTER_LIST_OFFSETS")
        );

        var monsterAddresses = (await Memory.ReadArraySafeAsync<IntPtr>(address, MAXIMUM_MONSTER_ARRAY_SIZE))
            .Where(mAddress => mAddress != IntPtr.Zero)
            .ToHashSet();

        IntPtr[] toDespawn = _monsters.Keys.Where(it => !monsterAddresses.Contains(it))
            .ToArray();

        foreach (IntPtr mAddress in toDespawn)
            HandleMonsterDespawn(mAddress);

        IntPtr[] toSpawn = monsterAddresses.Where(it => !_monsters.ContainsKey(it))
            .ToArray();

        foreach (IntPtr mAddress in toSpawn)
            await HandleMonsterSpawn(mAddress);

    }

    private async Task HandleMonsterSpawn(IntPtr monsterAddress)
    {
        if (monsterAddress.IsNullPointer() || _monsters.ContainsKey(monsterAddress))
            return;

        int monsterId = await Memory.ReadAsync<int>(monsterAddress + 0x2D4);

        var monster = new MHRMonster(
            process: Process,
            address: monsterAddress,
            id: monsterId
        );
        _monsters.Add(monsterAddress, monster);
        Monsters.Add(monster);
        ScanManager.Add(monster);

        this.Dispatch(_onMonsterSpawn, monster);
    }

    private void HandleMonsterDespawn(IntPtr address)
    {
        if (_monsters[address] is not MHRMonster monster)
            return;

        _monsters.Remove(address);
        _damageDone.Remove(address);
        Monsters.Remove(monster);
        ScanManager.Remove(monster);

        this.Dispatch(_onMonsterDespawn, monster);

        monster.Dispose();
    }

    #region Damage helpers

    private static async void OnPlayerStageUpdate(object? sender, EventArgs e)
    {
        await DamageMessageHandler.ClearAllHuntStatisticsExceptAsync(Array.Empty<IntPtr>());
        await DamageMessageHandler.RequestHuntStatisticsAsync(CommonConstants.AllTargets);
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
                    RawDamage = group.Sum(damage => damage.RawDamage),
                    ElementalDamage = group.Sum(damage => damage.ElementalDamage)
                };
            })
            .ToArray();

        _player.UpdatePartyMembersDamage(damages);
    }

    #endregion

    #region Chat helpers
    private async Task<MHRChatMessage> DerefChatMessageAsync(MHRChatMessageStructure message)
    {
        return message.Type switch
        {
            0x0 => await DerefNormalChatMessageAsync(message),
            _ => DerefUnknownTypeMessage()
        };
    }

    private async Task<MHRChatMessage> DerefNormalChatMessageAsync(MHRChatMessageStructure message)
    {
        int messageStringLength = await Memory.ReadAsync<int>(message.Message + 0x10);
        int messageAuthorLength = await Memory.ReadAsync<int>(message.Author + 0x10);

        string messageString = await Memory.ReadAsync(message.Message + 0x14, messageStringLength * 2, Encoding.Unicode);
        string messageAuthor = await Memory.ReadAsync(message.Author + 0x14, messageAuthorLength * 2, Encoding.Unicode);

        return new MHRChatMessage
        {
            Message = messageString,
            Author = messageAuthor,
            Type = AuthorType.Player,
            PlayerSlot = message.PlayerSlot,
        };
    }

    private static MHRChatMessage DerefUnknownTypeMessage() => new() { Type = AuthorType.None };
    #endregion
}

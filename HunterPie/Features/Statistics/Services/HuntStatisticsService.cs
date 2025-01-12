using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Crypto;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Statistics.Services;

internal class HuntStatisticsService : IHuntStatisticsService<HuntStatisticsModel>
{
    private readonly IContext _context;
    private readonly DateTime _startedAt = DateTime.UtcNow;
    private readonly List<IHuntStatisticsService<MonsterModel>> _monsterStatisticsServices = new();
    private readonly List<IHuntStatisticsService<PartyMemberModel>> _partyMembersStatisticsServices = new();

    public HuntStatisticsService(IContext context)
    {
        _context = context;

        HookEvents();
    }

    public HuntStatisticsModel Export()
    {
        QuestDetailsModel? questModel = _context.Game.Quest switch
        {
            { } quest => new QuestDetailsModel(
                Id: quest.Id,
                Type: quest.Type,
                Deaths: quest.Deaths,
                MaxDeaths: quest.MaxDeaths,
                Level: quest.Level,
                Stars: quest.Stars
            ),
            _ => null
        };

        var players = _partyMembersStatisticsServices.Select(s => s.Export())
            .ToList();

        var monsters = _monsterStatisticsServices.Select(s => s.Export())
            .ToList();

        return new HuntStatisticsModel(
            Game: GetGameType(),
            Quest: questModel,
            Players: players,
            Monsters: monsters,
            StartedAt: _startedAt,
            FinishedAt: DateTime.UtcNow,
            UploadedAt: DateTime.UtcNow,
            Hash: GenerateHuntHash(players, monsters)
        );
    }

    private void HookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin += OnPartyMemberJoin;
        _context.Game.OnMonsterSpawn += OnMonsterSpawn;


        foreach (IPartyMember player in _context.Game.Player.Party.Members)
            HandlePartyMember(player);

        foreach (IMonster monster in _context.Game.Monsters)
            HandleMonster(monster);
    }

    private void UnhookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin -= OnPartyMemberJoin;
        _context.Game.OnMonsterSpawn -= OnMonsterSpawn;
    }

    private void OnMonsterSpawn(object? sender, IMonster e) => HandleMonster(e);

    private void OnPartyMemberJoin(object? sender, IPartyMember e) => HandlePartyMember(e);

    private void HandleMonster(IMonster e)
    {
        _monsterStatisticsServices.Add(new MonsterStatisticsService(_context, e));
    }

    private void HandlePartyMember(IPartyMember member)
    {
        if (member.Type != MemberType.Player)
            return;

        _partyMembersStatisticsServices.Add(new PartyMemberStatisticsService(_context, member));
    }

    private static string GenerateHuntHash(List<PartyMemberModel> players, List<MonsterModel> monsters)
    {
        string[] data = players.Select(p => p.Name)
            .Concat(monsters.Select(m => m.Id.ToString()))
            .Where(e => !string.IsNullOrEmpty(e))
            .OrderBy(e => e.Length)
            .ToArray();

        string uniqueString = string.Join('|', data);

        return HashService.Hash(uniqueString);
    }

    public void Dispose()
    {
        UnhookEvents();

        _monsterStatisticsServices.DisposeAll();
        _monsterStatisticsServices.Clear();

        _partyMembersStatisticsServices.DisposeAll();
        _partyMembersStatisticsServices.Clear();
    }

    private GameType GetGameType()
    {
        return (_context.Game) switch
        {
            MHRGame => GameType.Rise,
            MHWGame => GameType.World,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
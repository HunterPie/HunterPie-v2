using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Game.Data.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Data.Repository;

public static class MonsterRepository
{
    private const string RISE_MONSTERS_FILE = "Game/Rise/Data/MonsterData.xml";
    private const string WORLD_MONSTERS_FILE = "Game/World/Data/MonsterData.xml";

    private static readonly Lazy<Dictionary<int, MonsterDefinition>> LazyRiseMonstersDataSource = new(() => LoadMonsters(RISE_MONSTERS_FILE));
    private static readonly Lazy<Dictionary<int, MonsterDefinition>> LazyWorldMonstersDataSource = new(() => LoadMonsters(WORLD_MONSTERS_FILE));

    public static MonsterDefinition UnknownDefinition = new();

    public static MonsterPartDefinition UnknownPartDefinition = new MonsterPartDefinition { String = "PART_UNKNOWN" };

    /// <summary>
    /// Finds a monster schema based on the game and their internal Id
    /// </summary>
    /// <param name="game">Game</param>
    /// <param name="id">Id of the monsters</param>
    /// <returns>Monster data</returns>
    public static MonsterDefinition? FindBy(GameType game, int id)
    {
        Dictionary<int, MonsterDefinition> dataSource = GetMonsterDataSourceBy(game);

        return dataSource.ContainsKey(id) ? dataSource[id] : null;
    }

    /// <summary>
    /// Returns all monsters based on game
    /// </summary>
    /// <param name="game">Game</param>
    /// <returns>Array of monsters</returns>
    public static MonsterDefinition[] FindAllBy(GameType game)
    {
        Dictionary<int, MonsterDefinition> dataSource = GetMonsterDataSourceBy(game);

        return dataSource.Values.ToArray();
    }

    private static Dictionary<int, MonsterDefinition> GetMonsterDataSourceBy(GameType game)
    {
        Lazy<Dictionary<int, MonsterDefinition>> dataSource = game switch
        {
            GameType.Rise => LazyRiseMonstersDataSource,
            GameType.World => LazyWorldMonstersDataSource,
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };

        return dataSource.Value;
    }

    private static Dictionary<int, MonsterDefinition> LoadMonsters(string file)
    {
        var document = new XmlDocument();
        document.Load(ClientInfo.GetPathFor(file));
        return LoadMonsterFromDocument(document);
    }

    private static Dictionary<int, MonsterDefinition> LoadMonsterFromDocument(XmlDocument document)
    {
        XmlNodeList? monsters = document.SelectNodes("//GameData/Monsters/Monster");

        if (monsters is null)
            return new Dictionary<int, MonsterDefinition>();

        return monsters.Cast<XmlNode>()
            .Select(MapFactory.Map<XmlNode, MonsterDefinition>)
            .ToDictionary(it => it.Id);
    }
}
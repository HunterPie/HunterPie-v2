using System;
using GameTypeEntity = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Integrations.Poogie.Common.Models;
internal enum GameType
{
    MHR,
    MHW
}

internal static class GameTypeExtensions
{
    public static GameTypeEntity ToEntity(this GameType type)
    {
        return (type) switch
        {
            GameType.MHR => GameTypeEntity.Rise,
            GameType.MHW => GameTypeEntity.World,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static GameType ToApiModel(this GameTypeEntity type)
    {
        return (type) switch
        {
            GameTypeEntity.Rise => GameType.MHR,
            GameTypeEntity.World => GameType.MHW,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
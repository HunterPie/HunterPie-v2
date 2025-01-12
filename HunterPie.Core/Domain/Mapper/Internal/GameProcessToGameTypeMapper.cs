using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;
using System;

namespace HunterPie.Core.Domain.Mapper.Internal;

#nullable enable
internal class GameProcessToGameTypeMapper : IMapper<GameProcessType, GameType?>
{
    public GameType? Map(GameProcessType data)
    {
        return data switch
        {
            GameProcessType.None => null,
            GameProcessType.MonsterHunterRise => GameType.Rise,
            GameProcessType.MonsterHunterWorld => GameType.World,
            GameProcessType.All => null,
            _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
        };
    }
}
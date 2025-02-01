using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Domain.Mapper.Internal;

internal class GameTypeToGameProcessMapper : IMapper<GameType, GameProcessType>
{
    public GameProcessType Map(GameType data)
    {
        return data switch
        {
            GameType.Rise => GameProcessType.MonsterHunterRise,
            GameType.World => GameProcessType.MonsterHunterWorld,
            _ => GameProcessType.None
        };
    }
}
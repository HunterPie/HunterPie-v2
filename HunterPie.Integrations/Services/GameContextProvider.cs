using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Services.Exceptions;

namespace HunterPie.Integrations.Services;

internal class GameContextProvider : IGameContextService
{
    public Context Get(IGameProcess game)
    {
        return game.Type switch
        {
            GameProcessType.MonsterHunterRise => new MHRContext(game),

            GameProcessType.MonsterHunterWorld => new MHWContext(game),

            GameProcessType.None or
            GameProcessType.All or
            _ => throw new UnsupportedGameException(game.Name)
        };
    }
}
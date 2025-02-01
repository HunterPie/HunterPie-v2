using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Services.Exceptions;

namespace HunterPie.Integrations.Services;

internal class GameContextProvider : IGameContextService
{
    private readonly IScanService _scanService;

    public GameContextProvider(IScanService scanService)
    {
        _scanService = scanService;
    }

    public Context Get(IGameProcess game)
    {
        return game.Type switch
        {
            GameProcessType.MonsterHunterRise => new MHRContext(game, _scanService),

            GameProcessType.MonsterHunterWorld => new MHWContext(game, _scanService),

            GameProcessType.None or
            GameProcessType.All or
            _ => throw new UnsupportedGameException(game.Name)
        };
    }
}
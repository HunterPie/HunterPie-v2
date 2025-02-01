using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Game.Service;

namespace HunterPie.Features.Game;

internal class GameModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<GameContextController>();
    }
}
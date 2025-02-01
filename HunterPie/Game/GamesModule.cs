using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Game.Rise;
using HunterPie.Game.World;

namespace HunterPie.Game;

internal class GamesModule : IDependencyModule
{
    // TODO: Move this to their respective modules in HunterPie.Integrations.dll
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<MHWContextInitializer>()
            .WithSingle<MHRContextInitializer>();
    }
}
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Services;

namespace HunterPie.Integrations;

internal class IntegrationsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<GameContextProvider>();
    }
}
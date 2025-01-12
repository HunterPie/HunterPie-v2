using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld;

internal class MHWGameModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<MHWProcessAttachStrategy>();
    }
}
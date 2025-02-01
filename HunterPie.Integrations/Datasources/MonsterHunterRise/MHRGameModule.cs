using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise;

internal class MHRGameModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<MHRProcessAttachStrategy>();
    }
}
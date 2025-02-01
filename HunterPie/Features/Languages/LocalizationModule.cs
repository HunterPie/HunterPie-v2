using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Languages.Repository;

namespace HunterPie.Features.Languages;

internal class LocalizationModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<LocalizationRepository>();
    }
}
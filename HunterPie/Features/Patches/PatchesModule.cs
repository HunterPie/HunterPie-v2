using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Patches.ViewModels;

namespace HunterPie.Features.Patches;

internal class PatchesModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<PatchesViewModel>();
    }
}
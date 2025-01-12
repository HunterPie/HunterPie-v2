using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Update.Gateway;
using HunterPie.Update.Service;

namespace HunterPie.Update;

internal class UpdateModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<UpdateCleanUpService>()
            .WithService<UpdateService>()
            .WithService<LocalizationUpdateService>()
            .WithService<ChecksumService>()
            .WithService<UpdateGateway>();
    }
}
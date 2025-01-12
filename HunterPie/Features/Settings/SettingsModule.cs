using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Settings.Factory;

namespace HunterPie.Features.Settings;

internal class SettingsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<SettingsFactory>();
    }
}
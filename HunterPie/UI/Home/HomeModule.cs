using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Home.Services;

namespace HunterPie.UI.Home;

internal class HomeModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<HomeCallToActionsService>()
            .WithService<HomeService>();
    }
}
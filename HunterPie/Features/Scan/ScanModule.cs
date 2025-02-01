using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Scan.Service;

namespace HunterPie.Features.Scan;

internal class ScanModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<ScanService>();
    }
}
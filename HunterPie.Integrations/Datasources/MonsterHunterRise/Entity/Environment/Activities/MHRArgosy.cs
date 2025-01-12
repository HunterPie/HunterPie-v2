using HunterPie.Core.Extensions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;

public class MHRArgosy : IDisposable
{
    public MHRSubmarine[] Submarines { get; } = {
        new(), new(), new()
    };

    public void Dispose()
    {
        Submarines.DisposeAll();
    }
}
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise;

public sealed class MHRContext : Context
{
    public static MHRStrings Strings { get; private set; }

    internal MHRContext(
        IGameProcess process,
        IScanService scanService) : base(new MHRGame(process, scanService), process)
    {
        Strings = new MHRStrings();
    }
}
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld;

public sealed class MHWContext : Context
{
    public static MHWStrings Strings { get; private set; }

    internal MHWContext(
        IGameProcess process,
        IScanService scanService) : base(new MHWGame(process, scanService), process)
    {
        Strings = new MHWStrings();
    }
}
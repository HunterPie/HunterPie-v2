using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld;

public sealed class MHWContext : Context
{
    public static MHWStrings Strings { get; private set; }

    internal MHWContext(IGameProcess process) : base(new MHWGame(process), process)
    {
        Strings = new MHWStrings();
    }
}
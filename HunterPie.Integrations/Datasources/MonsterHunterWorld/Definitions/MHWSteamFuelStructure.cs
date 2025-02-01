using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWSteamFuelStructure
{
    public int NaturalFuel;
    public int StoredFuel;
}
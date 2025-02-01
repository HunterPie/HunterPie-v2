using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRPlayerLevelStructure
{
    public int HighRank;
    public int MasterRank;
}
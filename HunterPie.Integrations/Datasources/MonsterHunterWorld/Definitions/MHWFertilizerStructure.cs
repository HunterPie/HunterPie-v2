using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWFertilizerStructure
{
    public long Reference;
    public int Id;
    public int Amount;
}
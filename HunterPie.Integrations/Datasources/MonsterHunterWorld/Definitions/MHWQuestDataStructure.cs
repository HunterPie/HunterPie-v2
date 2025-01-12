using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWQuestDataStructure
{
    [FieldOffset(0x0)] public int MaxDeaths;
    [FieldOffset(0x4)] public int Deaths;
}
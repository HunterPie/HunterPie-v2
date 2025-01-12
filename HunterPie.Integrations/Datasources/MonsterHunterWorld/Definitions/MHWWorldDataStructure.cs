using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWWorldDataStructure
{
    [FieldOffset(0x38)] public float WorldTime;
}
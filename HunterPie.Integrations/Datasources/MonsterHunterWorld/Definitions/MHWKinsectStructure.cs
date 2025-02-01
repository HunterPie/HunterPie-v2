using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWKinsectStructure
{
    [FieldOffset(0x0A8C)] public float Stamina;
    [FieldOffset(0x1BA8)] public float RedCharge;
    [FieldOffset(0x1BAC)] public float YellowCharge;
}
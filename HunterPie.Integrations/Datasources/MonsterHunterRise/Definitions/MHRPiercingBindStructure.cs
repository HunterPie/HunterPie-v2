using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRPiercingBindStructure
{
    [FieldOffset(0x1F0)] public float Timer;
}
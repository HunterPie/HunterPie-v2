using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSubmarineItemStructure
{
    [FieldOffset(0x14)]
    public int Amount;

    public bool IsNotEmpty() => Amount > 0;
}
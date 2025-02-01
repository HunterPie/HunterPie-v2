using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSubmarineStructure
{
    [FieldOffset(0x18)]
    public long Buddy;

    [FieldOffset(0x28)]
    public int Item;

    [FieldOffset(0x2C)]
    public int BargainSkill;

    [FieldOffset(0x30)]
    public int DaysLeft;

    [FieldOffset(0x40)]
    public nint ItemArrayPtr;
}
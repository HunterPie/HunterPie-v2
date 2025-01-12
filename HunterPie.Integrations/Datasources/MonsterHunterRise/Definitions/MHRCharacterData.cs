using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRCharacterData
{
    [FieldOffset(0x18)]
    public nint NamePointer;

    [FieldOffset(0x38)]
    public int HighRank;

    [FieldOffset(0x90)]
    public int MasterRank;
}
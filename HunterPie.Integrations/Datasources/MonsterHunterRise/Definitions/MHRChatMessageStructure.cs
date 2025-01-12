using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRChatMessageStructure
{
    [FieldOffset(0x8)]
    public int Visibility;

    [FieldOffset(0x10)]
    public int Type;

    [FieldOffset(0x38)]
    public nint Author;

    [FieldOffset(0x50)]
    public int PlayerSlot;

    [FieldOffset(0x68)]
    public nint Message;
}
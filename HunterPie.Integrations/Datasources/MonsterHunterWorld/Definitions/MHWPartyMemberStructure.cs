using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWPartyMemberStructure
{
    public nint Address;
    public long Unk0;
    public long Ref;
    public long Unk1;
    public long Unk2;
    public long Unk3;
    public long Unk4;
    public long Unk5;
    public long Unk6;
    public int Unk7;
    public int Unk8;
    public int Unk9;
    public int Unk10;
}
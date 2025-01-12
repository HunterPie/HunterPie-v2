using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWTenderizeInfoStructure
{
    public long Address;
    public float Duration;
    public float MaxDuration;
    public int Unk0;
    public int Unk1;
    public long Unk2;
    public float ExtraDuration;
    public float MaxExtraDuration;
    public int Unk3;
    public int Unk4;
    public uint PartId;
    public int TenderizedCounter;
    public int Unk6;
    public int Unk7;
}
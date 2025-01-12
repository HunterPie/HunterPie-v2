using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWMonsterStatusStructure
{
    public long Reference;
    public long Unk0;
    public int Unk1;
    public int IsActive;
    public float Buildup;
    public float DamageDone;
    public float Unk3;
    public float Duration;
    public float MaxDuration;
    public int Unk4;
    public int Unk5;
    public int Counter;
    public int unk6;
    public float MaxBuildup;
    public float Unk8; // 10 for legiana
    public float Unk9; // 10 for legiana
}
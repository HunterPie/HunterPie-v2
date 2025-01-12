using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWMonsterPartStructure : IEquatable<MHWMonsterPartStructure>
{
    public long Reference;
    public float Unk0;
    public float MaxHealth;
    public float Health;
    public int Unk1;
    public int Counter;
    public int Unk2;
    public float ExtraMaxHealth;
    public float ExtraHealth;
    public int Unk3;
    public int Unk4;
    public int Unk5;
    public int Unk6;
    public int Unk7;
    public int Unk8;

    // Extras
    public int Unk9;
    public int Unk10;
    public long Unk11;
    public long Unk12;
    public long Unk13;
    public int Unk14;
    public int Unk15;
    public int Unk16;
    public uint Index;

    public bool Equals(MHWMonsterPartStructure other)
    {
        return MaxHealth == other.MaxHealth
            && Health == other.Health
            && Counter == other.Counter
            && ExtraHealth == other.ExtraHealth
            && ExtraMaxHealth == other.ExtraMaxHealth
            && Index == other.Index;
    }
}
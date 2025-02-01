using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHWHealingStructure
{
    public long Ref1;
    public long Ref2;
    public float Heal;
    public float OldMaxHeal;
    public float MaxHeal;
    public float HealSpeed;
    public float MaxHealSpeed;
    public float Unk;
    public float Unk1;
    public int Stage; // Healing stage; 0 = not healing; 1 = first stage; 2 = second stage
    public int Unk2;
    public int Unk3;
    public int Unk4;
    public int Unk5;
}
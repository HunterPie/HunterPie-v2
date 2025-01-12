using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRPetalaceStatsStructure
{
    public long Reference;
    public int Unk;
    public int Unk1;
    public long Unk2;
    public int Unk3;
    public int Unk4;
    public int HealthUp;
    public int StaminaUp;
    public int AttackUp;
    public int DefenseUp;
}
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MHWGearSkill
{
    public long Ref;
    public byte LevelGear;
    public byte LevelMantle;
    public byte Unk1;
    public byte Unk2;
    public int Unk3;
    public double Unk4;
}
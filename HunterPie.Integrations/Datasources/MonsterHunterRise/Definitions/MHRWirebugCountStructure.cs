using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRWirebugCountStructure
{
    public int Default;
    public int Environment;
    public int Skill;

    public int Total() => Math.Max(0, Default + Environment + Skill);

    public bool AnyEnvironment() => Environment > 0;

    public bool AnySkill() => Skill > 0;
}
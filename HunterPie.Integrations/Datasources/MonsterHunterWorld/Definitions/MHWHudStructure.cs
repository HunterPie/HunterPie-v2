using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWHudStructure
{
    [FieldOffset(0x30)]
    public nint HealingArrayPointer;

    [FieldOffset(0x60)]
    public float MaxHealth;

    [FieldOffset(0x64)]
    public float Health;

    [FieldOffset(0x12c)]
    public float Stamina;

    [FieldOffset(0x130)]
    public float MaxStamina;

    [FieldOffset(0x2DD4)]
    public float RecoverableHealth;
}
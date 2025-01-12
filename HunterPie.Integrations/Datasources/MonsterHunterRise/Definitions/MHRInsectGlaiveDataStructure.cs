using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRInsectGlaiveDataStructure
{
    [FieldOffset(0xA08)] public nint ExtractsArray;
    [FieldOffset(0xA44)] public float AttackTimer;
    [FieldOffset(0xA48)] public float SpeedTimer;
    [FieldOffset(0xA4C)] public float DefenseTimer;
}
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWInsectGlaiveExtraStructure
{
    [FieldOffset(0x12B4)] public float MaxStamina;
}
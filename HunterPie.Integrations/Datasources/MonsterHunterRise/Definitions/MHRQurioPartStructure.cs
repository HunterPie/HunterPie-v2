using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRQurioPartStructure
{
    [FieldOffset(0x10)] public bool IsActive;
    [FieldOffset(0x18)] public nint HealthPtr;
    [FieldOffset(0x38)] public float MaxHealth;
}
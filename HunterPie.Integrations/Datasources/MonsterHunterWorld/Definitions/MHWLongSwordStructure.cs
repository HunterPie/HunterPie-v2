using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWLongSwordStructure
{
    [FieldOffset(0x2368)] public float BuildUp;
    [FieldOffset(0x2370)] public int SpiritLevel;
    [FieldOffset(0x2374)] public float SpiritLevelTimer;
    [FieldOffset(0x2378)] public float IaiSlashSpiritRegenTimer;
    [FieldOffset(0x2388)] public float HelmBreakerSpiritRegenTimer;
}
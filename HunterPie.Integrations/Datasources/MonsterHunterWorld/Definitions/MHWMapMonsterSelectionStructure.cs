using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWMapMonsterSelectionStructure
{
    [FieldOffset(0x128)] public nint MapInsectsRef;
    [FieldOffset(0x148)] public nint SelectedMonster;
    [FieldOffset(0x160)] public nint GuiRadarRef;
}
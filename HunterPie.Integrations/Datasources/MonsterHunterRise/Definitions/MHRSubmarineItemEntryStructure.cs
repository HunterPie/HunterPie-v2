using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSubmarineItemEntryStructure
{
    /// <summary>
    /// Pointer to <see cref="MHRSubmarineItemStructure"/>
    /// </summary>
    [FieldOffset(0x20)] public nint ItemPointer;
}
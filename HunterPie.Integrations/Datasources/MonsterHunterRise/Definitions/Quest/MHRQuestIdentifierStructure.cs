using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRQuestIdentifierStructure
{
    [FieldOffset(0x18)] public int UniqueIdentifier;
}
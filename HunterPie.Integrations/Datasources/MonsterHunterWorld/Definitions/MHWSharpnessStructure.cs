using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;


[StructLayout(LayoutKind.Explicit)]
public struct MHWSharpnessStructure
{
    [FieldOffset(0x1D10)] public int MaxLevel;
    [FieldOffset(0x20F8)] public int Sharpness;
    [FieldOffset(0x20FC)] public Sharpness Level;
}
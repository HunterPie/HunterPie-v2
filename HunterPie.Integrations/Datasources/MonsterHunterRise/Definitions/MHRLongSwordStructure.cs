using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRLongSwordStructure
{
    [FieldOffset(0x9F0)] public float LevelTimer;
    [FieldOffset(0x9F4)] public float GaugeBuildUp;
    [FieldOffset(0x9F8)] public int Level;
    /// <summary>
    /// float[] pointer, contains the maximum timer for each level. Can be indexed by <see cref="Level"/>
    /// </summary>
    [FieldOffset(0xA00)] public nint LevelMaxTimersPointer;
    [FieldOffset(0xA08)] public float BuildUpBuffTimer;
}
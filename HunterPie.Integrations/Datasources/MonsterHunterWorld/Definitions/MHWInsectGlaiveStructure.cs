using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWInsectGlaiveStructure
{
    /// <summary>
    /// Pointer to <see cref="MHWKinsectStructure"/>
    /// </summary>
    [FieldOffset(0x2350)] public nint KinsectPointer;
    [FieldOffset(0x2368)] public float AttackTimer;
    [FieldOffset(0x236C)] public float SpeedTimer;
    [FieldOffset(0x2370)] public float DefenseTimer;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    [FieldOffset(0x2378)] public KinsectBuffType[] BuffsQueue;
    [FieldOffset(0x2388)] public int QueuedIndex;
    [FieldOffset(0x2390)] public int QueuedBuffsCount;
}
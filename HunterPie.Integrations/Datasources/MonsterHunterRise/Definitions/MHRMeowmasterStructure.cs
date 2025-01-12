using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRMeowmasterStructure
{
    [FieldOffset(0x78)]
    [MarshalAs(UnmanagedType.U1)]
    public bool IsDeployed;

    [FieldOffset(0x79)]
    [MarshalAs(UnmanagedType.U1)]
    public bool IsLagniappleActive;

    [FieldOffset(0x88)]
    public nint BuddiesPointer;

    [FieldOffset(0xC4)]
    public int CurrentStep;
}
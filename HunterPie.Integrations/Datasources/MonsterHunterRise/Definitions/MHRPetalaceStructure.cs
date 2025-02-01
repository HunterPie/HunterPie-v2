using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRPetalaceStructure
{
    public nint Reference;
    public int Unk;
    public int Unk1;
    public int Unk2;
    public int Id;
    public nint Data;
}
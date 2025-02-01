using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MHRCryptoFloatStructure
{
    public uint Key;
    public uint Index;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] Values;

    public uint GetValue()
    {
        return Values[Index & 3];
    }
}
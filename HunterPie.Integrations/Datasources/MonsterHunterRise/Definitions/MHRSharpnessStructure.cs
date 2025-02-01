using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRSharpnessStructure
{
    public Sharpness Level;
    public int Hits;
    public int MaxHits;
}
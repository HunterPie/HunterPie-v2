using System.Drawing;

namespace HunterPie.Core.Extensions;

public static class ColorExtensions
{
    public static string ToHexString(this Color self) => $"#{self.A:X2}{self.R:X2}{self.G:X2}{self.B:X2}";
}
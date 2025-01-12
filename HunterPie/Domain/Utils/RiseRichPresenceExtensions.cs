namespace HunterPie.Domain.Utils;

internal static class RiseRichPresenceExtensions
{
    public static string ToImageKey(this int self) => $"rise-stage-{self}";
}
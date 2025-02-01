namespace HunterPie.Core.Extensions;

public static class StructExtensions
{
    /// <summary>
    /// Creates a shallow copy of a structure
    /// </summary>
    /// <typeparam name="T">Type of structure</typeparam>
    /// <param name="structure">Structure to copy</param>
    /// <returns>Copied structure</returns>
    public static T Copy<T>(this T structure) where T : struct => structure;
}
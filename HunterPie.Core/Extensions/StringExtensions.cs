using System;
using System.Text.RegularExpressions;

namespace HunterPie.Core.Extensions;

#nullable enable
public static class StringExtensions
{
    /// <summary>
    /// Removes specific characters from a string
    /// </summary>
    /// <param name="value">string</param>
    /// <param name="chars">characters to remove (default is ' ', '\x0A', '\x0B', '\x0C', '\x0D')</param>
    /// <returns>Pretty string</returns>
    public static string RemoveChars(this string value, char[]? chars = null)
    {
        chars ??= new char[] { ' ', '\x0A', '\x0B', '\x0C', '\x0D' };

        // Apparently this is faster than using Regex to replace
        string[] temp = value.Split(chars, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(" ", temp);
    }

    /// <summary>
    /// Filters style from a GMD string
    /// </summary>
    /// <param name="value">Text</param>
    /// <returns>Filtered Text</returns>
    public static string FilterStyles(this string value)
    {
        // STYL tag
        var tags = new Regex("<[(/\\w )]+>");
        return tags.Replace(value, string.Empty);
    }

    /// <summary>
    /// Allows case insensitive checks
    /// </summary>
    public static bool Contains(this string source, string toCheck, StringComparison comp) => source.IndexOf(toCheck, comp) >= 0;

    /// <summary>
    /// Checks if a string contains any of the substrings
    /// </summary>
    /// <param name="source">String to be compared</param>
    /// <param name="substrings">Substrings</param>
    /// <param name="comp">Comparision strategy</param>
    /// <returns>True if source contains any of the substrings, false otherwise</returns>
    public static bool ContainsAny(this string source, string[] substrings, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
    {
        for (int i = 0; i < substrings.Length; i++)
            if (source.Contains(substrings[i], comp))
                return true;

        return false;
    }

    /// <summary>
    /// Formats a string with given arguments
    /// </summary>
    /// <param name="source">String to be used as base</param>
    /// <param name="args">Arguments to format the string</param>
    /// <returns>Formatted string</returns>
    public static string Format(this string source, params object?[] args)
    {
        return string.Format(source, args);
    }
}
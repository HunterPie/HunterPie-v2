using System;

namespace HunterPie.Core.Search;

public static class SearchEngine
{
    public static bool IsMatch(string text, string query)
    {
        return text.Contains(query, StringComparison.InvariantCultureIgnoreCase);
    }
}
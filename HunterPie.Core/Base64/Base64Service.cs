using System;
using System.Text;

namespace HunterPie.Core.Base64;

public static class Base64Service
{

    public static string Encode(string input)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(buffer);
    }

    public static string Decode(string input)
    {
        byte[] buffer = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(buffer);
    }

}
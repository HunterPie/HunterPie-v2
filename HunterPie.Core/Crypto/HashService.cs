using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Crypto;

public static class HashService
{

    public static async Task<string> HashAsync(string value)
    {
        using var sha256 = SHA256.Create();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        byte[] buffer = await sha256.ComputeHashAsync(stream);

        StringBuilder builder = new(32);

        foreach (byte @byte in buffer)
            builder.Append(@byte.ToString("X2"));

        return builder.ToString();
    }

    public static string Hash(string value)
    {
        using var sha256 = SHA256.Create();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        byte[] buffer = sha256.ComputeHash(stream);

        StringBuilder builder = new(32);

        foreach (byte @byte in buffer)
            builder.Append(@byte.ToString("X2"));

        return builder.ToString();
    }

    public static async Task<string> ChecksumAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        using FileStream stream = File.OpenRead(filePath);
        byte[] buffer = await sha256.ComputeHashAsync(stream);

        StringBuilder builder = new(32);

        foreach (byte @byte in buffer)
            builder.Append(@byte.ToString("X2"));

        return builder.ToString();
    }
}
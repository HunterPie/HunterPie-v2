using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Text;

namespace HunterPie.Core.Client;

public static class ConfigHelper
{
    private static readonly ILogger Logger = LoggerFactory.Create();
    private static readonly object Sync = new();

    public static string ReadObject(string path)
    {
        lock (Sync)
        {
            using FileStream stream = File.OpenRead(path);
            byte[] buffer = new byte[stream.Length];
            _ = stream.Read(buffer, 0, buffer.Length);

            string str = Encoding.UTF8.GetString(buffer);

            if (string.IsNullOrEmpty(str)
                || str[0] == '\x00'
                || str == "null")
                throw new Exception("Configuration file was empty");

            return str;
        }
    }

    public static void WriteObject(string path, object obj)
    {
        lock (Sync)
            try
            {
                string serialized = JsonProvider.Serialize(obj);
                ReadOnlySpan<byte> buffer = Encoding.UTF8.GetBytes(serialized);
                using FileStream stream = File.OpenWrite(path);
                stream.SetLength(0);
                stream.Write(buffer);
            }
            catch (Exception err)
            {
                Logger.Error(err.ToString());
            }
    }

    public static string GetFullPath(string path)
    {
        if (!Path.IsPathFullyQualified(path))
            path = Path.Combine(ClientInfo.ClientPath, path);

        return path;
    }
}
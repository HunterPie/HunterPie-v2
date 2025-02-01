using System.IO;
using System.IO.Compression;

namespace HunterPie.Core.Zip;
public static class CompressionProvider
{
    public static string Compress(string zipPath, params string[] files)
    {
        using ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);

        foreach (string file in files)
            zip.CreateEntryFromFile(file, Path.GetFileName(file));

        return zipPath;
    }
}
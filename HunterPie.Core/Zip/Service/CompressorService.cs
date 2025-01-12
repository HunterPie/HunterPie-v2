using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace HunterPie.Core.Zip.Service;

internal class CompressorService : ICompressor
{
    public Task<bool> Compress(string path, IEnumerable<string> files)
    {
        using ZipArchive zipFile = ZipFile.Open(path, ZipArchiveMode.Create);

        foreach (string file in files)
            zipFile.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.SmallestSize);

        return Task.FromResult(true);
    }
}
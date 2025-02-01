using HunterPie.Core.Client;
using HunterPie.Core.Zip;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.Strategies;

internal class MHWBackupStrategy : IBackupStrategy
{
    private const string SAVE_FOLDER = "582010/remote";
    private readonly string[] _patterns = { "*.dat", "SAVEDATA*" };

    private readonly ICompressor _compressor;

    public MHWBackupStrategy(ICompressor compressor)
    {
        _compressor = compressor;
    }

    public GameType Type => GameType.MHW;

    public async Task<string> PackFilesAsync(string steamPath)
    {
        string path = Path.Combine(steamPath, SAVE_FOLDER);

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException("Could not find steam folder for monster hunter world");

        string temporaryZip = ClientInfo.GetRandomTempFile();
        IEnumerable<string> files = _patterns.SelectMany(pattern => Directory.EnumerateFiles(path, pattern));

        bool didCompress = await _compressor.Compress(
            path: temporaryZip,
            files: files
        );

        if (!didCompress)
            throw new Exception("Failed to backup save files");

        return temporaryZip;
    }
}
using HunterPie.Core.Client;
using HunterPie.Core.Zip;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Backup.Strategies;

internal class MHRBackupStrategy : IBackupStrategy
{
    private const string SAVE_FILE_PATTERN = "*.bin";
    private const string RISE_SAVE_FOLDER = "1446780/remote/win64_save";
    private readonly byte[] _magicBytes = { 0x44, 0x53, 0x53, 0x53 };

    private readonly ICompressor _compressor;

    public MHRBackupStrategy(ICompressor compressor)
    {
        _compressor = compressor;
    }

    public GameType Type => GameType.MHR;


    public async Task<string> PackFilesAsync(string steamPath)
    {
        string path = Path.Combine(steamPath, RISE_SAVE_FOLDER);

        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException("Could not find steam folder for monster hunter rise");

        string temporaryZip = ClientInfo.GetRandomTempFile();
        IEnumerable<string> filesToBackup = await FilterCorrectFilesAsync(
            files: Directory.EnumerateFiles(path, SAVE_FILE_PATTERN)
        );

        bool hasCompressed = await _compressor.Compress(
            path: temporaryZip,
            files: filesToBackup
        );

        if (!hasCompressed)
            throw new Exception("Failed to backup save files");

        return temporaryZip;
    }

    private async Task<IEnumerable<string>> FilterCorrectFilesAsync(IEnumerable<string> files)
    {
        var results = new List<string>();
        byte[] buffer = new byte[_magicBytes.Length];

        foreach (string file in files)
        {
            await using FileStream stream = File.OpenRead(file);
            await stream.ReadExactlyAsync(buffer, 0, buffer.Length);

            if (!_magicBytes.SequenceEqual(buffer))
                continue;

            results.Add(file);
        }

        return results;
    }
}
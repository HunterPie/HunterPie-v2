using HunterPie.Core.Crypto;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Update.Service;

internal class ChecksumService
{
    public async Task<Dictionary<string, string>> IndexAsync(
        string basePath,
        string relativePath,
        HashSet<string>? ignorableFiles = null)
    {
        Dictionary<string, string> files = new();

        await IndexRecursivelyAsync(
            basePath: basePath,
            relativePath: relativePath,
            files: files,
            ignorableFiles: ignorableFiles
        );

        return files;
    }

    private async Task IndexRecursivelyAsync(
        string basePath,
        string relativePath,
        Dictionary<string, string> files,
        HashSet<string>? ignorableFiles = null)
    {
        foreach (string entry in Directory.GetFileSystemEntries(basePath))
        {
            if (ignorableFiles?.Contains(entry) == true)
                continue;

            FileAttributes attributes = File.GetAttributes(entry);
            string absolute = Path.Combine(basePath, entry);
            string relative = Path.Combine(
                path1: relativePath,
                path2: Path.GetRelativePath(
                    relativeTo: basePath,
                    path: entry
                )
            );

            if (!attributes.HasFlag(FileAttributes.Directory))
            {
                files.Add(relative, await HashService.ChecksumAsync(absolute));
                continue;
            }

            await IndexRecursivelyAsync(
                basePath: absolute,
                relativePath: relative,
                files: files,
                ignorableFiles: ignorableFiles
            );
        }
    }
}
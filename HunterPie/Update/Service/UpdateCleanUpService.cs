using HunterPie.Core.Client;
using HunterPie.Core.Observability.Logging;
using HunterPie.Update.UseCase;
using System;
using System.Collections.Generic;
using System.IO;

namespace HunterPie.Update.Service;

internal class UpdateCleanUpService : IUpdateCleanUpUseCase
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public void Invoke()
    {
        Stack<string> directories = new();

        directories.Push(ClientInfo.ClientPath);

        while (directories.Count > 0)
            foreach (string entry in Directory.GetFileSystemEntries(directories.Pop()))
            {
                FileAttributes attributes = File.GetAttributes(entry);

                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    directories.Push(entry);
                    continue;
                }

                if (!entry.EndsWith(".old"))
                    continue;

                try
                {
                    File.Delete(entry);
                }
                catch (Exception err)
                {
                    _logger.Error($"Failed to update HunterPie: {err}");
                }
            }
    }
}
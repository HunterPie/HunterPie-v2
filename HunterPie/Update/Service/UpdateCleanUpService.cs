using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Update.UseCase;
using System;
using System.Collections.Generic;
using System.IO;

namespace HunterPie.Update.Service;

internal class UpdateCleanUpService : IUpdateCleanUpUseCase
{
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
                    Log.Error("Failed to delete file: {0}", err.ToString());
                }
            }
    }
}
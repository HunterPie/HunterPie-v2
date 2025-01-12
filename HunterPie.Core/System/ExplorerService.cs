using HunterPie.Core.Logger;
using System;
using System.IO;

namespace HunterPie.Core.System;

public static class ExplorerService
{

    public static void Delete(string path, bool recursively)
    {
        try
        {
            Directory.Delete(path, recursively);
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }
    }

    public static void DeleteFile(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }
    }
}
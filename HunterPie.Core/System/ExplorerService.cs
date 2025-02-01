using HunterPie.Core.Observability.Logging;
using System;
using System.IO;

namespace HunterPie.Core.System;

public static class ExplorerService
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    public static void Delete(string path, bool recursively)
    {
        try
        {
            Directory.Delete(path, recursively);
        }
        catch (Exception err)
        {
            Logger.Error(err.ToString());
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
            Logger.Error(err.ToString());
        }
    }
}
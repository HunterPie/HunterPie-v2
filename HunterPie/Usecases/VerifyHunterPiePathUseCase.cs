using HunterPie.Core.Client;
using System.Linq;

namespace HunterPie.Usecases;
internal class VerifyHunterPiePathUseCase
{

    public static bool Invoke()
    {
        string executablePath = ClientInfo.ClientPath;
        return !IsTemporaryPath(executablePath);
    }

    private static bool IsTemporaryPath(string path)
    {
        string[] tempIndicators = { "temp", "tmp" };
        string lowerPath = path.ToLowerInvariant();

        return tempIndicators.Any(lowerPath.Contains);
    }
}
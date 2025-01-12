using HunterPie.Core.System.Common.Exceptions;

namespace HunterPie.Platforms;

internal static class SupportedPlatformUseCase
{
    public static void Execute()
    {
        if (!OperatingSystem.IsWindows())
            throw new UnsupportedPlatformException();
    }
}
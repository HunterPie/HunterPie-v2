using HunterPie.Core.Native.IPC;
using HunterPie.Core.Observability.Logging;
using System.Threading.Tasks;

namespace HunterPie.Features.Native;

internal static class NativeIPCInitializer
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    public static async Task WaitForIPCInitialization()
    {
        if (await IPCService.Initialize())
            return;
        for (int i = 0; i <= 10; i++)
        {
            Logger.Debug($"Retrying to connect: Attempt {i}...");
            await Task.Delay(i * 100);
            if (await IPCService.Initialize())
                return;
        }
    }
}
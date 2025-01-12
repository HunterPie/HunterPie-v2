using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC;
using System.Threading.Tasks;

namespace HunterPie.Features.Native;

internal static class NativeIPCInitializer
{

    public static async Task WaitForIPCInitialization()
    {
        if (await IPCService.Initialize())
            return;
        for (int i = 0; i <= 10; i++)
        {
            Log.Debug($"Retrying to connect: Attempt {i}...");
            await Task.Delay(i * 100);
            if (await IPCService.Initialize())
                return;
        }
    }
}
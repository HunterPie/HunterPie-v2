using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Native;

internal static class IPCInjectorInitializer
{
    private const string NATIVE_NAME = "HunterPie.Native.dll";
    private const string NATIVE_PATH = "libs/" + NATIVE_NAME;

    public static async Task<bool> InjectNativeModuleAsync(IContext context)
    {
        try
        {
            string native = ClientInfo.GetPathFor(NATIVE_PATH);

            if (IsAlreadyInjected(context))
            {
                Log.Native("HunterPie Native Interface is already running");
                return false;
            }

            await context.Process.Memory.InjectAsync(native);

            Log.Native("HunterPie Native Interface injected successfully!");

            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Failed to inject HunterPie Native Interface. {0}", ex);
            return false;
        }
    }

    private static bool IsAlreadyInjected(IContext context)
    {
        return context.Process.SystemProcess.Modules.Cast<ProcessModule>()
            .Any(module => module.ModuleName == NATIVE_NAME);
    }
}
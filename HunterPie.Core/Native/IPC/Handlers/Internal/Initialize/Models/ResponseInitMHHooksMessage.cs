using System.Runtime.InteropServices;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;

[StructLayout(LayoutKind.Sequential)]
public struct ResponseInitMHHooksMessage
{
    public HookStatus Status;
}
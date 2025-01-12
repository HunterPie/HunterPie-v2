using HunterPie.Core.Native.IPC.Models;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;

[StructLayout(LayoutKind.Sequential)]
public struct RequestIPCInitializationMessage
{
    public IPCMessage Header;

    public IPCInitializationHostType HostType;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public nint[] Addresses;
}
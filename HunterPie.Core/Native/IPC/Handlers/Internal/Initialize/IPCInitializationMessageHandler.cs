using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using HunterPie.Core.Observability.Logging;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;

internal class IPCInitializationMessageHandler : IMessageHandler
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private const string ERROR_DIALOG_MESSAGE = "HunterPie has detected wrong version of HunterPie Native Interface currently in the game.\nYou must restart your game for it to work properly";
    public int Version => 2;

    public IPCMessageType Type => IPCMessageType.INIT_IPC_MEMORY_ADDRESSES;

    public void Handle(byte[] message)
    {
        ResponseIPCInitializationMessage response = MessageHelper.Deserialize<ResponseIPCInitializationMessage>(message);

        if (response.Header.Version != Version)
        {
            _logger.Warning(ERROR_DIALOG_MESSAGE);
            return;
        }

        // Not throwing Exception here since stack trace won't be helpful.
        Exception ex = Marshal.GetExceptionForHR(response.HResult);
        if (ex != null)
        {
            _logger.Error($"Failed to initialize IPC: {ex}");
            _logger.Warning(ERROR_DIALOG_MESSAGE);
            return;
        }

        IPCHookInitializationMessageHandler.RequestInitMHHooks();
    }

    public static async Task RequestIPCInitializationAsync(IPCInitializationHostType hostType, nint[] addresses)
    {
        nint[] buffer = new nint[256];

        Buffer.BlockCopy(addresses, 0, buffer, 0, addresses.Length * Marshal.SizeOf<nint>());

        var request = new RequestIPCInitializationMessage()
        {
            Header = new IPCMessage
            {
                Type = IPCMessageType.INIT_IPC_MEMORY_ADDRESSES,
                Version = 1,
            },
            HostType = hostType,
            Addresses = buffer,
        };

        await IPCService.Send(request);
    }
}
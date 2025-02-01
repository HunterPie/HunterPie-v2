using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using HunterPie.Core.Observability.Logging;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;

internal class IPCHookInitializationMessageHandler : IMessageHandler
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public int Version => 1;

    public IPCMessageType Type => IPCMessageType.INIT_MH_HOOKS;

    public void Handle(byte[] message)
    {
        ResponseInitMHHooksMessage response = MessageHelper.Deserialize<ResponseInitMHHooksMessage>(message);

        if (response.Status > HookStatus.AlreadyInitialized)
        {
            _logger.Error($"Failed to initialize HunterPie Native Interface hooks. Error code: {response.Status}");
            return;
        }

        _logger.Native("Successfully initialized HunterPie Native Interface hooks!");
    }

    public static async void RequestInitMHHooks()
    {
        var request = new IPCMessage
        {
            Type = IPCMessageType.INIT_MH_HOOKS,
            Version = 1
        };

        _ = await IPCService.Send(request);
    }
}
using HunterPie.Core.Native.IPC.Handlers;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using HunterPie.Core.Observability.Logging;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Native.IPC;

public class IPCService
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private const string IPC_DEFAULT_ADDRESS = "127.0.0.1";
    private const short IPC_DEFAULT_PORT = 22002;
    private const int IPC_DEFAULT_BUFFER_SIZE = 8192;

    private static IPCService? _instance;
    private TcpClient? _client;
    private NetworkStream? Stream => _client?.GetStream();
    private bool IsConnected => _client?.Connected ?? false;
    private static readonly SemaphoreSlim _semaphoreSlim = new(1);

    public static IPCService Instance
    {
        get
        {
            if (_instance == null)
                _instance = new IPCService();

            return _instance;
        }
    }

    public static async Task<bool> Send<T>(T data) where T : struct => await Instance.SendAsync(data);

    internal static async Task<bool> Initialize() => await Instance.Connect();

    private async Task<bool> Connect()
    {
        if (IsConnected)
            return true;

        _client = new();

        try
        {
            await _client.ConnectAsync(IPC_DEFAULT_ADDRESS, IPC_DEFAULT_PORT);
        }
        catch (Exception err)
        {
            _logger.Error(err.ToString());
            return false;
        }

        if (!IsConnected)
            return false;

        _logger.Native("Connected to HunterPie Native Interface");

        Listen();

        return IsConnected;
    }

    private void HandleMessage(byte[] rawData)
    {
        IPCMessage message = MessageHelper.Deserialize<IPCMessage>(rawData);

        MessageHandlerManager.Dispatch(message.Type, rawData);
    }

    private void Listen()
    {
        _ = Task.Factory.StartNew(async () =>
        {
            byte[] buffer = new byte[IPC_DEFAULT_BUFFER_SIZE];

            while (IsConnected)
            {
                int dataSize = await Stream?.ReadAsync(buffer, 0, buffer.Length);
                byte[] dataCopy = new byte[dataSize];

                Buffer.BlockCopy(buffer, 0, dataCopy, 0, dataSize);

                HandleMessage(dataCopy);
            }

            HandleDisconnect();
        });
    }

    private void HandleDisconnect()
    {
        _client?.Dispose();
        _logger.Native("HunterPie was disconnected from Native Interface.");

        Reconnect();
    }

    private async void Reconnect()
    {
        for (int i = 0; i < 10; i++)
        {
            if (IsConnected)
                break;

            _logger.Debug("Attempting to reconnect to Native Interface");

            _ = await Connect();

            await Task.Delay(i * 100);
        }
    }

    private async Task<bool> SendAsync<T>(T message) where T : struct
    {
        byte[] raw = MessageHelper.Serialize(message);
        return await SendRawAsync(raw);
    }

    private async Task<bool> SendRawAsync(byte[] raw)
    {
        if (!IsConnected)
            return false;

        try
        {
            await _semaphoreSlim.WaitAsync();

            if (Stream is null)
                return false;

            await Stream.WriteAsync(raw, 0, raw.Length);
            return true;
        }
        catch (Exception err)
        {
            _logger.Error(err.ToString());
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return false;
    }
}
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Logger;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Internal.Logger;

internal class FileStreamLogger : ILogger, IDisposable
{
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);
    private static readonly FileStream Stream = File.OpenWrite(ClientInfo.GetPathFor("HunterPie_Log.txt"));

    public Task Debug(string message) => WriteToBuffer(LogLevel.Debug, message);

    public Task Debug(string format, params object[] args) => WriteToBuffer(LogLevel.Debug, string.Format(format, args));

    public Task Info(string message) => WriteToBuffer(LogLevel.Info, message);

    public Task Info(string format, params object[] args) => WriteToBuffer(LogLevel.Info, string.Format(format, args));

    public Task Warn(string message) => WriteToBuffer(LogLevel.Warn, message);

    public Task Warn(string format, params object[] args) => WriteToBuffer(LogLevel.Warn, string.Format(format, args));

    public Task Native(string message) => WriteToBuffer(LogLevel.Info, message);

    public Task Native(string format, params object[] args) => WriteToBuffer(LogLevel.Info, string.Format(format, args));

    public Task Error(string message) => WriteToBuffer(LogLevel.Error, message);

    public Task Error(string format, params object[] args) => WriteToBuffer(LogLevel.Error, string.Format(format, args));

    public Task Benchmark(string message) => WriteToBuffer(LogLevel.Info, message);

    public Task Benchmark(string format, params object[] args) => WriteToBuffer(LogLevel.Info, string.Format(format, args));


    private static async Task WriteToBuffer(LogLevel level, string message)
    {
        try
        {
            await Semaphore.WaitAsync();
            await Stream.WriteAsync(Encoding.UTF8.GetBytes($"[{DateTime.Now.ToLongTimeString()}][{level}] {message}\n"));
            await Stream.FlushAsync();
        }
        catch { }
        finally
        {
            Semaphore.Release();
        }
    }

    public void Dispose()
    {
        try
        {
            Semaphore.Wait();
            Stream.Dispose();
        }
        catch { }
        finally
        {
            Semaphore.Release();
        }
    }
}
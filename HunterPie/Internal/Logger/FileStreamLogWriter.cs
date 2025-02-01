using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Text;

namespace HunterPie.Internal.Logger;

internal class FileStreamLogWriter : ILogWriter
{
    private static readonly FileStream Stream = File.OpenWrite(ClientInfo.GetPathFor("HunterPie_Log.txt"));

    public void Debug(string message) =>
        WriteToBuffer(LogLevel.Debug, message);

    public void Info(string message) =>
        WriteToBuffer(LogLevel.Info, message);

    public void Warning(string message) =>
        WriteToBuffer(LogLevel.Warn, message);

    public void Native(string message) =>
        WriteToBuffer(LogLevel.Info, message);

    public void Error(string message) =>
        WriteToBuffer(LogLevel.Error, message);

    public void Benchmark(string message) =>
        WriteToBuffer(LogLevel.Info, message);


    private static void WriteToBuffer(LogLevel level, string message)
    {
        string now = DateTime.Now.ToLongTimeString();
        byte[] buffer = Encoding.UTF8.GetBytes($"[{now}][{level}] {message}\n");
        Stream.Write(buffer, 0, buffer.Length);
        Stream.Flush();
    }

    public void Dispose()
    {
        Stream.Dispose();
    }
}
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain;

public static class ScanManager
{
    private static Thread _thread;
    private static CancellationTokenSource _token = new();
    private static readonly HashSet<Scannable> Scannables = new();

    // Metrics

    public static readonly Observable<long> ScanTime = 0;

    internal static void Start()
    {
        if (_thread is not null)
            return;

        _thread = new Thread(() =>
        {
            do
                try
                {
                    var sw = Stopwatch.StartNew();
                    Scan();
                    sw.Stop();
                    ScanTime.Value = sw.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);

                    if (_token.IsCancellationRequested)
                        break;

                    Thread.Sleep((int)ClientConfig.Config.Client.PollingRate.Current);
                }
                catch (Exception err)
                {
                    // Logs the error if it came from a generic exception instead of a
                    // cancel request
                    Log.Error(err.ToString());
                }
            while (true);

            _token = new();
        })
        {
            Name = "ScanManager",
            IsBackground = true,
            Priority = ThreadPriority.AboveNormal
        };
        _thread.Start();
    }

    internal static void Stop()
    {
        if (_thread is null)
            return;

        lock (Scannables)
        {
            Scannables.Clear();
            _token.Cancel();
            _thread = null;
        }
    }

    private static void Scan()
    {

        Scannable[] readOnlyScannables = Scannables.ToArray();
        var tasks = new Task[readOnlyScannables.Length];

        for (int i = 0; i < readOnlyScannables.Length; i++)
            tasks[i] = readOnlyScannables[i].Scan();

        Task.WaitAll(tasks);
    }

    public static void Add(params Scannable[] scannableList)
    {
        foreach (Scannable scannable in scannableList)
            Add(scannable);
    }

    public static void Add(Scannable scannable)
    {
        lock (Scannables)
        {
            if (Scannables.Contains(scannable))
                return;

            _ = Scannables.Add(scannable);
        }
    }

    public static void Remove(Scannable scannable)
    {
        lock (Scannables)
        {
            if (!Scannables.Contains(scannable))
                return;

            _ = Scannables.Remove(scannable);
        }
    }
}
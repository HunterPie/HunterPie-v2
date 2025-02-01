using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Domain;
using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Features.Scan.Service;

internal class ScanService : IControllableScanService, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private Task? _task;
    private readonly HashSet<Scannable> _scannables = new();
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public Observable<long> ElapsedTime { get; } = 0;

    public void Start(CancellationToken cancellationToken)
    {
        _task = Task.Factory.StartNew(
            action: () => ExecuteScanPolling(cancellationToken),
            cancellationToken: cancellationToken,
            creationOptions: TaskCreationOptions.LongRunning,
            scheduler: TaskScheduler.Default
        );
    }

    private void ExecuteScanPolling(CancellationToken cancellationToken)
    {
        while (true)
            try
            {
                _stopwatch.Restart();
                Scan(cancellationToken);
                _stopwatch.Stop();
                ElapsedTime.Value = _stopwatch.ElapsedTicks / TimeSpan.TicksPerMicrosecond;

                Thread.Sleep(
                    timeout: TimeSpan.FromMilliseconds(
                        value: ClientConfig.Config.Client.PollingRate.Current
                    )
                );
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
    }

    private void Scan(CancellationToken cancellationToken)
    {
        _lock.EnterReadLock();
        var readOnlyScannables = _scannables.ToImmutableArray();
        _lock.ExitReadLock();

        Task[] tasks = readOnlyScannables.Select(async it => await it.ScanAsync())
            .ToArray();

        Task.WaitAll(tasks, cancellationToken);
    }

    public void Add(Scannable scannable)
    {
        try
        {
            _lock.EnterWriteLock();

            if (_scannables.Contains(scannable))
                return;

            _scannables.Add(scannable);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Remove(Scannable scannable)
    {
        try
        {
            _lock.EnterWriteLock();

            _scannables.Remove(scannable);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Dispose()
    {
        _lock.Dispose();
        _task?.Dispose();
    }
}
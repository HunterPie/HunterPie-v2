using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class FileStreamLoggerInitializer : IInitializer, IDisposable
{
    private readonly FileStreamLogWriter _logWriter;

    public FileStreamLoggerInitializer(FileStreamLogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public Task Init()
    {
        LoggerFactory.Add(_logWriter);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _logWriter.Dispose();
    }
}
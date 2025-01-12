using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;
internal class FileStreamLoggerInitializer : IInitializer
{
    public Task Init()
    {
        ILogger logger = new FileStreamLogger();
        Log.Add(logger);

        return Task.CompletedTask;
    }
}
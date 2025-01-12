using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Logger;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class HunterPieLoggerInitializer : IInitializer
{
    public Task Init()
    {
        ILogger logger = new HunterPieLogger();
        Log.Add(logger);

        Log.Info("Initialized HunterPie logger");

        return Task.CompletedTask;
    }
}
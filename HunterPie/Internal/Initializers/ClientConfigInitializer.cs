using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ClientConfigInitializer : IInitializer
{
    public Task Init()
    {
        ClientConfig.Initialize();

        return Task.CompletedTask;
    }
}
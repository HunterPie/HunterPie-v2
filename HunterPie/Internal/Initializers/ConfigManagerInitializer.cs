using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ConfigManagerInitializer : IInitializer
{
    public Task Init()
    {
        ConfigManager.Initialize();

        return Task.CompletedTask;
    }
}
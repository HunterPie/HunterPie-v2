using HunterPie.Core.Client.Localization;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ClientLocalizationInitializer : IInitializer
{
    public Task Init()
    {
        _ = Localization.Instance;

        return Task.CompletedTask;
    }
}
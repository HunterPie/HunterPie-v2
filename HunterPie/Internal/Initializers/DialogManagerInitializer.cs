using HunterPie.Core.Domain.Dialog;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Dialog;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class DialogManagerInitializer : IInitializer
{
    public Task Init()
    {
        INativeDialogFactory factory = new UIDialogFactory();
        _ = new DialogManager(factory);

        return Task.CompletedTask;
    }
}
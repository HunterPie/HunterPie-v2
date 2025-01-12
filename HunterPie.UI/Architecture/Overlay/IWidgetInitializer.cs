using HunterPie.Core.Game;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Overlay;

internal interface IWidgetInitializer
{
    public Task LoadAsync(IContext context);
    public void Unload();
}
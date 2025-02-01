using HunterPie.Core.Game;
using System.Threading.Tasks;

namespace HunterPie.Domain.Interfaces;

internal interface IContextInitializer
{
    /// <summary>
    /// Performs the initialization operation.
    /// </summary>
    /// <remarks>
    /// The implementation of this function should not assume it is called on the main thread.
    /// To make sure certain operations are taken place on the main thread (e.g., direct UI object model manipulation), the implementation
    /// should use <c>DispatcherObject.Dispatcher.DispatchAsync</c>.
    /// </remarks>
    public Task InitializeAsync(IContext context);
}
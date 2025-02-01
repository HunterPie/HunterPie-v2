using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Logging.Entity;
using HunterPie.UI.Logging.Services;
using HunterPie.UI.Logging.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Logging;

internal class LoggingModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle(() => new ObservableCollection<LogString>())
            .WithSingle<ConsoleViewModel>()
            .WithSingle<HunterPieLogWriter>();
    }
}
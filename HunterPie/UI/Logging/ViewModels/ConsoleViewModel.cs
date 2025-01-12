using HunterPie.UI.Architecture;
using HunterPie.UI.Logger;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Logging.ViewModels;

internal class ConsoleViewModel : ViewModel
{
    public ObservableCollection<LogString> Logs { get; }

    public ConsoleViewModel(ObservableCollection<LogString> logs)
    {
        Logs = logs;
    }
}
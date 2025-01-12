using HunterPie.Core.Extensions;
using HunterPie.Core.Settings.Types;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class FileSelectorPropertyViewModel : ConfigurationPropertyViewModel
{
    public IFileSelector FileSelector { get; }

    public ObservableCollection<string> Elements { get; }

    public FileSelectorPropertyViewModel(IFileSelector fileSelector)
    {
        FileSelector = fileSelector;
        Elements = fileSelector.GetElements().ToObservableCollection();
    }
}
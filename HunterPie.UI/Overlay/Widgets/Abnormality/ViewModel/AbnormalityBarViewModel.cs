using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class AbnormalityBarViewModel : Bindable
{

    private ObservableCollection<AbnormalityViewModel> _abnormalities = new();
    public ObservableCollection<AbnormalityViewModel> Abnormalities { get => _abnormalities; set => SetValue(ref _abnormalities, value); }

    public void SortAbnormalities(SortBy sortBy)
    {
        Abnormalities = sortBy switch
        {
            SortBy.Lowest => new(_abnormalities.OrderBy(e => e.Timer)),
            SortBy.Highest => new(_abnormalities.OrderByDescending(e => e.Timer)),
            SortBy.Off => _abnormalities,
            _ => throw new NotImplementedException("unreachable"),
        };
    }
}
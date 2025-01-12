using LiveCharts.Wpf;
using System.Collections.Generic;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public interface ISectionControllable
{
    public bool IsToggled { get; set; }

    public List<AxisSection> Activations { get; }
}
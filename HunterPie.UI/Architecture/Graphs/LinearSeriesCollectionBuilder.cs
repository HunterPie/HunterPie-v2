using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Graphs;

public class LinearSeriesCollectionBuilder
{
    private readonly SeriesCollection _instance = new();

    public LinearSeriesCollectionBuilder AddSeries(ChartValues<ObservablePoint> points, string title, Color color)
    {
        var series = new LineSeries
        {
            Title = title,
            Stroke = new SolidColorBrush(color),
            Fill = ColorFadeGradient.FromColor(color),
            PointGeometrySize = 0,
            StrokeThickness = 2,
            LineSmoothness = 0.7,
            Values = points
        };

        _instance.Add(series);

        return this;
    }

    public SeriesCollection Build() => _instance;

}
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HunterPie.UI.Overlay.Controls;

public class Arc : Shape
{

    /*
        Credits: https://stackoverflow.com/questions/36752183/wpf-doughnut-progressbar
    */

    public double StartAngle
    {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new PropertyMetadata(90.0, AnglesChanged));

    public bool Reverse
    {
        get => (bool)GetValue(ReverseProperty);
        set => SetValue(ReverseProperty, value);
    }

    // Using a DependencyProperty as the backing store for Reverse.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ReverseProperty =
        DependencyProperty.Register("Reverse", typeof(bool), typeof(Arc), new PropertyMetadata(false));

    public double Percentage
    {
        get => (double)GetValue(PercentageProperty);
        set => SetValue(PercentageProperty, value);
    }

    // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PercentageProperty =
        DependencyProperty.Register("Percentage", typeof(double), typeof(Arc), new PropertyMetadata(0.0, AnglesChanged));

    public double EndAngle
    {
        get => (double)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EndAngleProperty =
        DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new PropertyMetadata(0.0, AnglesChanged));

    protected override Geometry DefiningGeometry => GetArcGeometry();

    private static void AnglesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Arc arc)
            arc.InvalidateVisual();
    }

    private Geometry GetArcGeometry()
    {
        double end = ConvertPercentageIntoAngle(Reverse, Percentage);

        Point startPoint = PointAtAngle(Math.Min(StartAngle, end));
        Point endPoint = PointAtAngle(Math.Max(StartAngle, end));

        var arcSize = new Size(Math.Max(0, (RenderSize.Width - StrokeThickness) / 2),
        Math.Max(0, (RenderSize.Height - StrokeThickness) / 2));
        bool isLargeArc = Math.Abs(end - StartAngle) > 180;
        var geom = new StreamGeometry();
        using (StreamGeometryContext context = geom.Open())
        {
            context.BeginFigure(startPoint, false, false);
            context.ArcTo(endPoint, arcSize, 0, isLargeArc,
            SweepDirection.Counterclockwise, true, false);
        }

        geom.Transform = new TranslateTransform(StrokeThickness / 2, StrokeThickness / 2);
        return geom;
    }

    private Point PointAtAngle(double angle)
    {
        double radAngle = angle * (Math.PI / 180);
        double xRadius = (RenderSize.Width - StrokeThickness) / 2;
        double yRadius = (RenderSize.Height - StrokeThickness) / 2;
        double x = xRadius + (xRadius * Math.Cos(radAngle));
        double y = yRadius - (yRadius * Math.Sin(radAngle));
        return new Point(x, y);
    }

    public static double ConvertPercentageIntoAngle(bool reverse, double percentage)
    {
        double angle = reverse switch
        {
            true => -90.0 + (360.0 * percentage),
            false => 90.0 - (360.0 * percentage),
        };
        double cap = -269.999;
        return Math.Max(angle, cap);
    }
}
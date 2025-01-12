using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Position : Bindable
{
    private double _x;
    private double _y;

    public double X
    {
        get => _x;
        set => SetValue(ref _x, value);
    }
    public double Y
    {
        get => _y;
        set => SetValue(ref _y, value);
    }

    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Position other) => X == other.X && Y == other.Y;
}
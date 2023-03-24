using System.Collections;


namespace NelderMeadMethod;

public class Point : IEnumerable<double>, ICloneable
{
    public int Dimension { get; }

    private readonly double[] _coordinates;

    public double this[int index]
    {
        get => _coordinates[index];
        set => _coordinates[index] = value;
    }
    
    public Point(IEnumerable<double> coordinates)
    {
        _coordinates = (double[])coordinates.ToArray().Clone();
        Dimension = _coordinates.Length;
    }

    public Point(params double[] coordinates) : this((IEnumerable<double>)coordinates) { }

    public Point(int dimension) : this(new double[dimension]) { }

    public static Point operator -(Point point1) =>
        new(point1._coordinates.Select(coord => -coord));

    public static Point operator +(Point point1, Point point2)
    {
        if (point1.Dimension != point2.Dimension)
            throw new ArgumentException("Dimension of points must be equal");

        return new Point(
            point1._coordinates.Zip(point2._coordinates, (a, b) => a + b)
            );
    }

    public static Point operator -(Point point1, Point point2) => point1 + (-point2);

    public static Point operator /(Point point1, double number)
    {
        if (number == 0)
            throw new ArgumentException("Number can't be 0");

        return new Point(point1._coordinates.Select(coord => coord / number));
    }

    public static Point operator *(double number, Point point1) =>
        new(point1._coordinates.Select(coord => coord * number));

    public static Point operator *(Point point1, Point point2) =>
        new(point1._coordinates.Zip(point2._coordinates, (a, b) => a * b));

    public IEnumerator<double> GetEnumerator() =>
        ((IEnumerable<double>)_coordinates).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public object Clone() => new Point(_coordinates);

    public override string ToString() => $"({string.Join(", ", _coordinates)})";

    
}

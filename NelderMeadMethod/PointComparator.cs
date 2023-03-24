namespace NelderMeadMethod;

public class PointComparator : IComparer<Point>
{
    private readonly IFunction _function;

    public PointComparator(IFunction function) => _function = function;

    public int Compare(Point? x, Point? y)
    {
        if (x is null || y is null)
            throw new ArgumentNullException("Incorrect parameter value");

        return _function.Calculate(x).CompareTo(_function.Calculate(y));
    }
}

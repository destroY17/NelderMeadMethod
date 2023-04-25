namespace NelderMeadMethod;

public class NelderMead
{
    private readonly double _reflection;
    private readonly double _contraction;
    private readonly double _expansion;

    public Simplex? CurrentSimplex { get; private set; }
    public IFunction? CurrentFunction { get; private set; }

    public delegate void DrawEachIteration(NelderMead algorithm);

    public NelderMead(double reflection = 1, double contraction = 0.5, double expansion = 2)
    {
        if (reflection <= 0 || contraction <= 0 || expansion <= 0)
            throw new ArgumentOutOfRangeException("Coefficients must be > 0");

        _reflection = reflection;
        _contraction = contraction;
        _expansion = expansion;
    }

    public Point Run(IFunction function, Point? initialPoint = null, int maxIteration = 100, 
        double accuracy = 0.001, DrawEachIteration draw = null)
    {
        if (function is null) throw new ArgumentNullException("Function can't be null");

        if (initialPoint is not null && function.Dimension != initialPoint.Dimension)
            throw new ArgumentException("Dimension of function and points must be equal");

        int iterationCount = 0;

        CurrentSimplex = new Simplex(function.Dimension, initialPoint);
        CurrentFunction = function;

        while (iterationCount < maxIteration && !AccuraccyIsReached(CurrentSimplex, accuracy))
        {
            iterationCount++;

            CurrentSimplex = Simplex.Sort(CurrentSimplex, new PointComparator(function));

            Point bestPoint = CurrentSimplex[0];
            double bestValue = function.Calculate(bestPoint);

            Point goodPoint = CurrentSimplex[^2];
            double goodValue = function.Calculate(goodPoint);

            Point worstPoint = CurrentSimplex[^1];
            double worstValue = function.Calculate(worstPoint);

            if (draw is not null)
                draw(this);

            var centroid = GetCentroid(CurrentSimplex.Points, worstPoint);

            Point reflectedPoint = GetReflectedPoint(centroid, worstPoint);
            double reflectedValue = function.Calculate(reflectedPoint);

            if (reflectedValue < bestValue)
            {
                Point expandedPoint = GetExpandedPoint(centroid, reflectedPoint);
                double expandedValue = function.Calculate(expandedPoint);

                if (expandedValue < reflectedValue)
                    CurrentSimplex[^1] = expandedPoint;
                else
                    CurrentSimplex[^1] = reflectedPoint;

                continue;
            }
            else if (bestValue < reflectedValue && reflectedValue < goodValue)
            {
                CurrentSimplex[^1] = reflectedPoint;
                continue;
            }
            else if (goodValue < reflectedValue && reflectedValue < worstValue)
            {
                CurrentSimplex[^1] = reflectedPoint;
                worstPoint = reflectedPoint;
            }

            Point shrinkedPoint = GetContractedPoint(centroid, worstPoint);
            double shrinkedValue = function.Calculate(shrinkedPoint);

            if (shrinkedValue < worstValue)
                CurrentSimplex[^1] = shrinkedPoint;
            else
                CurrentSimplex = Simplex.GlobalCompression(CurrentSimplex, bestPoint);
        }
        return CurrentSimplex[0];
    }

    private Point GetCentroid(List<Point> points, Point? excludedPoint = null)
    {
        int dimension = points.Count - 1;
        Point centroid = new(dimension);

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i] != excludedPoint)
                centroid += points[i];
        }
        return excludedPoint is null ? centroid / (dimension + 1) : centroid / dimension;
    }

    private Point GetReflectedPoint(Point centroid, Point worst) =>
        (1 + _reflection) * centroid - _reflection * worst;

    private Point GetExpandedPoint(Point centroid, Point reflected) =>
        (1 - _expansion) * centroid + _expansion * reflected;

    private Point GetContractedPoint(Point ceontroid, Point worst) =>
        (1 - _contraction) * ceontroid + _contraction * worst;

    private bool AccuraccyIsReached(Simplex simplex, double accuracy)
    {
        Point averagePoint = GetCentroid(simplex.Points);
        Point dispersion = new(averagePoint.Dimension);

        for (int i = 0; i < simplex.Count; i++)
        {
            dispersion += (simplex[i] - averagePoint) * (simplex[i] - averagePoint); 
        }
        dispersion /= simplex.Count;

        foreach(var coordinate in dispersion)
        {
            if (Math.Sqrt(Math.Abs(coordinate)) > accuracy)
                return false;
        }
        return true;
    }
}

using static System.Math;

namespace NelderMeadMethod;

public class HimmelblausFunction : IFunction
{
    public int Dimension => 2;

    public double Calculate(Point p)
    {
        if (p.Dimension != Dimension)
            throw new ArgumentException("Dimension must be equal to " + Dimension);

        return Pow(p[0] * p[0] + p[1] - 11, 2) + Pow(p[0] + p[1] * p[1] - 7, 2);
    }
}

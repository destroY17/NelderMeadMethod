namespace NelderMeadMethod;

public class SinFunction : IFunction
{
    public int Dimension => 1;

    public double Calculate(Point p) =>
        5 * Math.Sin(2 * p[0]) + Math.Pow(p[0], 2);
}

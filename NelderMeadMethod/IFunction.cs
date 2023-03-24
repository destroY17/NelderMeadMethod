namespace NelderMeadMethod;

public interface IFunction
{
    int Dimension { get; }
    double Calculate(Point p);
}

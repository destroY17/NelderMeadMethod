using NelderMeadMethod;

namespace NelderMeadTest;

[TestClass]
public class TestAlgorithm
{
    private NelderMead _algorithm = new NelderMead();
    private IFunction _rosenbrock = new RosenbrockFunction();
    private IFunction _himmelblaus = new HimmelblausFunction();
    private IFunction _sinFunc = new SinFunction();

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IncorrectCoefficients() => new NelderMead(-5);

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void IncorrectRunParameters()
    {
        var initialPoint = new Point(1, 2, 3);
        _algorithm.Run(_rosenbrock, initialPoint);
    }

    [TestMethod]
    public void FindOptimumRosenbrock()
    {
        var answer = _rosenbrock.Calculate(_algorithm.Run(_rosenbrock));
        var expected = _rosenbrock.Calculate(new Point(1, 1));

        Assert.IsTrue((answer - expected) < 0.001);
    }

    [TestMethod]
    public void FindOptimumHimmelblaus()
    {
        var answer = _himmelblaus.Calculate(_algorithm.Run(_himmelblaus));
        var expected = _himmelblaus.Calculate(new Point(3, 2));

        Assert.IsTrue((answer - expected) < 0.001);
    }

    [TestMethod]
    public void FindOptimumWithInitialPoint()
    {
        var initialPoint = new Point(-5, 10);
        var answer = _himmelblaus.Calculate(_algorithm.Run(_himmelblaus, initialPoint));
        var expected = _himmelblaus.Calculate(new Point(3, 2));

        Assert.IsTrue((answer - expected) < 0.001);
    }

    [TestMethod]
    public void FindLocalOptimum()
    {
        var initialPoint = new Point(new double[] { 3 });
        var answer = _sinFunc.Calculate(_algorithm.Run(_sinFunc, initialPoint));
        var expected = _sinFunc.Calculate(new Point(new double[] { 2.1355 }));

        Assert.IsTrue((answer - expected) < 0.001);
    }


}
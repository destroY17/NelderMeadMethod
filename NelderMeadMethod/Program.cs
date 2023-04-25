using NelderMeadMethod;

var alg = new NelderMead();

void Draw(NelderMead algoritm)
{
    var simplex = algoritm.CurrentSimplex;
    var function = algoritm.CurrentFunction;

    Console.WriteLine(
    $"Лучшая точка: {simplex[0]} Значение функции: {function.Calculate(simplex[0])}\n" +
    $"Хорошая точка: {simplex[^2]}, Значение функции: {function.Calculate(simplex[^2])}\n" +
    $"Худшая точка: {simplex[^1]}, Значение функции: {function.Calculate(simplex[^1])}\n"
    );
}

Console.WriteLine("Функция Розенброка\n");
IFunction func = new RosenbrockFunction();
alg.Run(func, new Point(5,5), 100, 0.001, Draw);

Console.WriteLine("Функция Химмельблау\n");
func = new HimmelblausFunction();
alg.Run(func, new Point(-3,-3), 100, 0.001, Draw);
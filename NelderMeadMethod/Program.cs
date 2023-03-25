using NelderMeadMethod;

var alg = new NelderMead();

Console.WriteLine("Функция Розенброка\n");
IFunction func = new RosenbrockFunction();
alg.Run(func, new Point(5,5), 100, 0.001, true);

Console.WriteLine("Функция Химмельблау\n");
func = new HimmelblausFunction();
alg.Run(func, new Point(-3,-3), 100, 0.001, true);
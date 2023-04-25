namespace NelderMeadMethod
{
    public class Simplex : ICloneable
    {
        public List<Point> Points { get; }
        public int Count { get; }

        private static readonly Random _random = new Random();

        public Point this[int index]
        {
            get => Points[index];
            set => Points[index] = value;
        }

        public Simplex(List<Point> simplex)
        {
            Points = GetPointsClone(simplex);
            Count = Points.Count;
        }

        public Simplex(int dimension, Point? initialPoint)
        {
            Points = CreateSimplex(dimension, initialPoint);
            Count = Points.Count;
        }
            
        private List<Point> CreateSimplex(int dimension, Point? initialPoint)
        {
            initialPoint ??= new Point(dimension);

            var simplex = new List<Point>(initialPoint.Dimension + 1);
            simplex.Add(initialPoint);

            double offset1 = _random.NextDouble();
            double offset2 = offset1 * _random.NextDouble();

            Point prevPoint = initialPoint;

            for (int i = 0; i < initialPoint.Dimension; i++)
            {
                Point generatedPoint = (Point)prevPoint.Clone();

                for (int j = 0; j < generatedPoint.Dimension; j++)
                {
                    if (i == j)
                        generatedPoint[j] += offset1;
                    else
                        generatedPoint[j] += offset2;
                }
                simplex.Add(generatedPoint);
                prevPoint = generatedPoint;
            }
            return simplex;
        }

        public static Simplex GlobalCompression(Simplex simplex, Point best)
        {
            var simplexComrpression = simplex.Clone() as Simplex;

            for (int i = 0; i < simplexComrpression.Count; i++)
            {
                if (simplexComrpression[i] != best)
                {
                    simplexComrpression[i] = best + (simplexComrpression[i] - best) / 2;
                }
            }
            return simplexComrpression;
        }

        public static Simplex Sort(Simplex simplex, IComparer<Point> comparer)
        {
            var copySimplex = simplex.Clone() as Simplex;
            copySimplex.Points.Sort(comparer);
            return copySimplex;
        }

        public object Clone() => new Simplex(GetPointsClone(Points));

        private List<Point> GetPointsClone(List<Point> points)
        {
            var clone = new List<Point>();

            foreach (var point in points)
                clone.Add((Point)point.Clone());
            return clone;
        }
    }
}

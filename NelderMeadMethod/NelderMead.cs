using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace NelderMeadMethod
{
    public class NelderMead
    {
        private static readonly Random _random = new Random();

        private readonly double _reflection;
        private readonly double _contraction;
        private readonly double _expansion;

        public NelderMead(double reflection = 1, double contraction = 0.5, double expansion = 2)
        {
            if (reflection <= 0 || contraction <= 0 || expansion <= 0)
                throw new ArgumentOutOfRangeException("Coefficients must be > 0");

            _reflection = reflection;
            _contraction = contraction;
            _expansion = expansion;
        }

        public Point Run(IFunction function, Point? initialPoint = null, int maxIteration = 100, double accuracy = 0.001)
        {
            if (function is null) throw new ArgumentNullException("Function can't be null");

            if (initialPoint is not null && function.Dimension != initialPoint.Dimension)
                throw new ArgumentException("Dimension of function and points must be equal");

            List<Point> simplex = CreateSimplex(function.Dimension, initialPoint);
            int iterationCount = 0;

            while (iterationCount < maxIteration && !AccuraccyIsReached(simplex, accuracy))
            {
                simplex.Sort(new PointComparator(function));

                Point bestPoint = simplex[0];
                double bestValue = function.Calculate(bestPoint);

                Point goodPoint = simplex[^2];
                double goodValue = function.Calculate(goodPoint);

                Point worstPoint = simplex[^1];
                double worstValue = function.Calculate(worstPoint);

                var centroid = GetCentroid(simplex, worstPoint);

                Point reflectedPoint = GetReflectedPoint(centroid, worstPoint);
                double reflectedValue = function.Calculate(reflectedPoint);

                if (reflectedValue < bestValue)
                {
                    Point expandedPoint = GetExpandedPoint(centroid, reflectedPoint);
                    double expandedValue = function.Calculate(expandedPoint);

                    if (expandedValue < reflectedValue)
                        simplex[^1] = expandedPoint;
                    else
                        simplex[^1] = reflectedPoint;

                    continue;
                }
                else if (bestValue < reflectedValue && reflectedValue < goodValue)
                {
                    simplex[^1] = reflectedPoint;
                    continue;
                }
                else if (goodValue < reflectedValue && reflectedValue < worstValue)
                {
                    simplex[^1] = reflectedPoint;
                    worstPoint = reflectedPoint;
                }

                Point shrinkedPoint = GetContractedPoint(centroid, worstPoint);
                double shrinkedValue = function.Calculate(shrinkedPoint);

                if (shrinkedValue < worstValue)
                    simplex[^1] = shrinkedPoint;
                else
                    GlobalCompression(simplex, bestPoint);

                iterationCount++;
            }
            return simplex[0];
        }


        private List<Point> CreateSimplex(int dimension, Point? initialPoint)
        {
            initialPoint ??= new Point(dimension);

            var simplex = new List<Point>(initialPoint.Dimension + 1);
            simplex.Add(initialPoint);

            double offset1 = 0.05;
            double offset2 = 0.00025;

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

        private void GlobalCompression(List<Point> simplex, Point best)
        {
            for (int i = 0; i < simplex.Count; i++)
            {
                if (simplex[i] != best)
                {
                    simplex[i] = best + (simplex[i] - best) / 2;
                }
            }
        }

        private bool AccuraccyIsReached(List<Point> simplex, double accuracy)
        {
            Point averagePoint = GetCentroid(simplex);
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
}

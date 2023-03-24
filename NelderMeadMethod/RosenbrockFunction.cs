using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelderMeadMethod
{
    public class RosenbrockFunction : IFunction
    {
        public int Dimension => 2;

        public double Calculate(Point p)
        {
            return Math.Pow(1 - p[0], 2) + 100 * Math.Pow(p[1] - p[0] * p[0], 2);
        }
    }
}

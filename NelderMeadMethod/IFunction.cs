using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelderMeadMethod
{
    public interface IFunction
    {
        int Dimension { get; }
        double Calculate(Point p);
    }
}

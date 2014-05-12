using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace JHLib.QuantLIB
{
    public static class VectorExtensions
    {
        public static double StdDev(this DenseVector vector)
        {
            double average = vector.Average();
            return Math.Sqrt(vector * vector / vector.Count - average*average);
        }
    }
}

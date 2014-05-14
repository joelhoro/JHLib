using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JHLib.QuantLIB
{
    public static class Context
    {
        public static Date TODAY;
        public static void Initialize() {
            TODAY = new Date(DateTime.Now);
        }
    }

    //[DebuggerDisplay( Name = "this (equity object)")]
    public class Equity
    {
        public double spot;
        public double sigma;

        public double Variance(Date maturity)
        {
            return sigma * sigma * ( maturity -Context.TODAY).TotalDays/365;
        }

        public override string ToString()
        {
            return "Equity[" + spot + "]";
        }

        public static Equity Lookup(string ticker)
        {
            return SecurityStore.GetEquity(ticker);
        }
    }


    public class EquityModel
    {
        public Equity equity;
        public List<Date> diffusiondates;
        public int N;

        private List<DenseVector> pathsCache;

        private List<double> Variances()
        {
            var variances = new List<double>();
            double previousVariance = 0;
            foreach(var date in diffusiondates)
            {
                double variance = equity.Variance(date);
                variances.Add(variance - previousVariance);
                previousVariance = variance;
            }
            return variances;
        }

        private IEnumerable<DenseVector> LogReturns(Normal normal)
        {
            var variances = Variances();

            foreach(var variance in variances)
            {
                var stdev = Math.Sqrt(variance);
                var drift = -0.5 * variance;
                var returns = new DenseVector(N);
                for (int i = 0; i < N; i++)
                    returns[i] = Math.Exp( normal.Sample() * stdev + drift );
                returns /= returns.Average();
                yield return returns;
            }
        }

        private IEnumerable<DenseVector> Spots(Normal normal)
        {
            var spots = DenseVector.Create(N, (_) => equity.spot);
            foreach( var returns in LogReturns(normal) )
            {
                spots = spots.PointwiseMultiply(returns) as DenseVector;
                yield return spots;
            }
        }

        public void ComputePaths(Normal normal)
        {
            pathsCache = Spots(normal).ToList();
        }

        public DenseVector PathsOnDate(Date date)
        {
            int i = diffusiondates.IndexOf(date);
            if (i == -1)
                throw new Exception("Can't find " + date + " in diffusion dates");
            else
                return pathsCache[i];
        }
    }

    public class EquityMultiModel
    {
        public List<Equity> equities;
        public DenseMatrix correlations;
        public List<Date> diffusiondates;
        public int N;

        private List<DenseVector> pathsCache;

        private DenseMatrix _cholesky;

        public void Initialize()
        {
            var size = equities.Count;
            _cholesky = new DenseMatrix(size, size);
            double rho = correlations[0, 1];
            _cholesky[0, 0] = 1;
            _cholesky[0, 1] = 0;
            _cholesky[1, 0] = rho;
            _cholesky[1, 1] = Math.Sqrt(1 - rho * rho);
        }

        private DenseMatrix Variances()
        {
            var variances = new DenseMatrix(diffusiondates.Count, equities.Count);

            for (int i = 0; i < equities.Count; i++)
            {
                double previousVariance = 0;
                for (int j = 0; j < diffusiondates.Count;j++ )
                {
                    Date date = diffusiondates[j];
                    double variance = equities[i].Variance(date);
                    variances[j,i] = variance - previousVariance;
                    previousVariance = variance;
                }
            }
            return variances;
        }

        //private IEnumerable<DenseVector> LogReturns(Normal normal)
        //{
        //    var variances = Variances();

        //    foreach (var variance in variances)
        //    {
        //        var stdev = Math.Sqrt(variance);
        //        var drift = -0.5 * variance;
        //        var returns = new DenseVector(N);
        //        for (int i = 0; i < N; i++)
        //            returns[i] = Math.Exp(normal.Sample() * stdev + drift);
        //        returns /= returns.Average();
        //        yield return returns;
        //    }
        //}

        //private IEnumerable<DenseVector> Spots(Normal normal)
        //{
        //    var spots = DenseVector.Create(N, (_) => equities.spot);
        //    foreach (var returns in LogReturns(normal))
        //    {
        //        spots = spots.PointwiseMultiply(returns) as DenseVector;
        //        yield return spots;
        //    }
        //}

        //public void ComputePaths(Normal normal)
        //{
        //    pathsCache = Spots(normal).ToList();
        //}

        //public DenseVector PathsOnDate(Date date)
        //{
        //    int i = diffusiondates.IndexOf(date);
        //    if (i == -1)
        //        throw new Exception("Can't find " + date + " in diffusion dates");
        //    else
        //        return pathsCache[i];
        //}
    }

}

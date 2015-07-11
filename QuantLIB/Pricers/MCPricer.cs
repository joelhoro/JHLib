using JHLib.QuantLIB.Core;
using JHLib.QuantLIB.Model;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHLib.QuantLIB.Pricers
{
    public class MCPricer
    {
        public Func<double, double> Payoff;
        public Equity equity;
        public Date maturity;
        public int NumberOfPaths = 10000;

        public double Price()
        {
            var diffusiondates = new List<Date>() { maturity };
            var model = new EquityModel(equity, diffusiondates, NumberOfPaths);
            var normal = new Normal();
            model.ComputePaths(normal);
            var payoffs = model.PathsOnIndex(0)
                .Select(Payoff);
            var avg   = payoffs.Average();
            var avgSQ = payoffs.Average(p => p * p);
            Console.WriteLine("Average: {0}", Math.Sqrt((avgSQ-avg*avg)/NumberOfPaths));
            return avg;
        }
    }
}

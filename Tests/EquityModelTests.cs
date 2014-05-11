using JHLib.QuantLIB;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class EquityModelTests
    {
        const double TOLERANCE = 1e-6;

        /// <summary>
        /// Check that the MonteCarlo price of a call matches the analytical value
        /// </summary>
        [TestMethod]
        public void TestEquityModel()
        {
            const string ticker = "MSFT";
            const string NEWLINE = "\n";
            const int N = 10000000;
            var equity = Equity.Lookup(ticker);
            double K = equity.spot;

            Context.Initialize();
            var today = Context.TODAY;
            var year = new Frequency(365);
            var midDate = today + year;
            var endDate = midDate + year;

            var dates = new List<Date> { midDate, endDate };
            var model = new EquityModel { equity = equity, diffusiondates = dates, N = N };
            var normal = new Normal();

            var t = DateTime.Now;

            model.ComputePaths(normal);

            foreach( var evaluationDate in dates )
            {
                var values = model.PathsOnDate(evaluationDate);

                // apply call option payoff
                for (int i = 0; i < values.Count; i++)
                    values[i] = Math.Max(values[i]-K, 0);

                var MCPrice = values.Average();
                var stdev = values.StdDev() / Math.Sqrt(model.N);

                double maturity = (evaluationDate-today).Days / 365;
                var BSPrice = BlackScholes.Price(OptionType.Call, equity.spot, K, maturity, 0, equity.sigma);

                double tolerance = Math.Min(stdev, 1e-2);
                Assert.AreEqual(BSPrice,MCPrice, tolerance);

                string output = "Date: " + evaluationDate.ToString() + NEWLINE;
                output += String.Format("{0} +- {1}", MCPrice, stdev) + NEWLINE;
                output += "Time = " + (DateTime.Now - t).TotalSeconds + NEWLINE;
                output += String.Format("BS price: {0}", BSPrice) + NEWLINE;
                Console.WriteLine(output);
            }
        }
    }
}

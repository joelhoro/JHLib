using JHLib.QuantLIB;
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Tests
{
    [TestClass]
    public class EquityModelTests
    {
        const double TOLERANCE = 1e-6;

        private EquityModel GetModel() 
        {
            const string ticker = "MSFT";
            const int N = 10000000;
            var equity = Equity.Lookup(ticker);

            Context.Initialize();
            var today = Context.TODAY;
            var year = new Frequency(365);
            var midDate = today + year;
            var endDate = midDate + year;

            var dates = new List<Date> { midDate, endDate };
            var model = new EquityModel { equity = equity, diffusiondates = dates, N = N };
            var normal = new Normal();

            model.ComputePaths(normal);
            return model;
        }

        /// <summary>
        /// Check that the MonteCarlo price of a call matches the analytical value
        /// </summary>
        [TestMethod]
        public void TestEquityModel()
        {
            const string NEWLINE = "\n";
            var model = GetModel();
            var equity = model.equity;
            var dates = model.diffusiondates;
            var today = Context.TODAY;

            double K = equity.spot;
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

                double tolerance = Math.Min(2*stdev, 1e-2);
                Assert.AreEqual(BSPrice,MCPrice, tolerance);

                string output = "Date: " + evaluationDate.ToString() + NEWLINE;
                output += String.Format("{0} +- {1}", MCPrice, stdev) + NEWLINE;
                output += String.Format("BS price: {0}", BSPrice) + NEWLINE;
                Console.WriteLine(output);
            }
        }

        /// <summary>
        /// Check that the MonteCarlo price of a forward starting call matches the analytical value
        /// </summary>
        [TestMethod]
        public void TestEquityModelForward()
        {
            const string NEWLINE = "\n";
            var model = GetModel();
            var equity = model.equity;
            var dates = model.diffusiondates;
            var today = Context.TODAY;

            double K = 1;

            var values = model.PathsOnDate(dates[1]).PointwiseDivide(
                         model.PathsOnDate(dates[0])
                            ) as DenseVector;

            // apply call option payoff
            for (int i = 0; i < values.Count; i++)
                values[i] = Math.Max(values[i] - K, 0);

            var MCPrice = values.Average();
            var stdev = values.StdDev() / Math.Sqrt(model.N);

            double maturity = (dates[1]-dates[0]).Days / 365;
            var BSPrice = BlackScholes.Price(OptionType.Call, 1, K, maturity, 0, equity.sigma);

            double tolerance = Math.Min(stdev, 1e-2);
            Assert.AreEqual(BSPrice, MCPrice, tolerance);

            string output = "Date: " + dates[1].ToString() + NEWLINE;
            output += String.Format("{0} +- {1}", MCPrice, stdev) + NEWLINE;
            output += String.Format("BS price: {0}", BSPrice) + NEWLINE;
            Console.WriteLine(output);
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JHLib.QuantLIB;
using JHLib.QuantLIB.Core;
using JHLib.QuantLIB.Model;

namespace Tests
{
    [TestClass]
    public class QuantLIBTests
    {
        [TestMethod]
        public void IncreasingVariance()
        {
            var equity = new Equity(100, 0.2);
            Context.Initialize();
            Date today = Context.TODAY;

            double previousVariance = 0;
            for (Date date = today; date < today + new Frequency(3650); date = date + new Frequency(30) )
            {
                double newVariance = equity.Variance(date);
                Assert.IsTrue(newVariance >= previousVariance);
                Console.WriteLine("{0,10}: {1:F4}", date, newVariance);
                previousVariance = newVariance;
            }
        }
    }
}

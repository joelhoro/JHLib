using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JHLib.QuantLIB.Pricers;
using JHLib.QuantLIB.Core;
using JHLib.QuantLIB.Model;
using JHLib.QuantLIB;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class MCPricerTests
    {
        [TestMethod,TestCategory("Slow")]
        public void TestMCPricerForward()
        {
            string ticker = "MSFT";
            var equity = Equity.Lookup(ticker);

            Date date;
            Context.Initialize();
            date = Context.TODAY;

            var strike = 100;
            var maturity = date + Frequency.y1;
            var T = (maturity - Context.TODAY).TotalDays / 365;
            var r = 0;

            Func<double, double> payoff = x => Math.Max(x - strike, 0) + Math.Max(strike - x, 0);
            var mcPricer = new MCPricer { equity = equity, maturity = maturity, Payoff = payoff, NumberOfPaths = 1000000 };
            var mcPrice = mcPricer.Price();

            var blackScholesprice = new List<OptionType>() { OptionType.Call, OptionType.Put }
                .Sum(optionType => BlackScholes.Price(optionType, equity, strike, T, r));

            var tolerance = 1e-1;
            Assert.AreEqual(blackScholesprice, mcPrice, tolerance);
        }
    }
}

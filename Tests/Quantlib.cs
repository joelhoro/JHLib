using Microsoft.VisualStudio.TestTools.UnitTesting;
using JHLib.QuantLIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tests
{
    [TestClass]
    public class QuantlibTests
    {
        const double TOLERANCE = 1e-6;

        [TestMethod]
        public void CumulativeDistributionTests()
        {
            var cnd = BlackScholes.CND2(0);
            Assert.AreEqual(0.5, cnd);

            cnd = BlackScholes.CND2(1);
            var expected = 0.84134474606854293;
            Assert.AreEqual(expected, cnd, TOLERANCE);
        }

        [TestMethod]
        public void ImpliedVolTest()
        {
            var fwd = 105;
            var K = 100;
            var r = 0.04;
            var T = 4;
            var optionType = OptionType.Call;

            var volInitial = 0.2;

            var price = BlackScholes.Price(optionType, fwd, K, T, r, volInitial);

            var vol = BlackScholes.ImpliedVol(optionType, price, fwd, K, T, r);

            Assert.AreEqual(volInitial, vol, TOLERANCE);
        }
    }
}

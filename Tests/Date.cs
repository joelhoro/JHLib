using JHLib.QuantLIB.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{

    [TestClass]
    public class DateTests
    {
        [TestMethod]
        public void TestComparison()
        {
            Assert.IsTrue(Date.Tomorrow > Date.Today);
            Assert.IsFalse(Date.Tomorrow < Date.Today);
        }

        [TestMethod]
        public void TestOperators()
        {
            Assert.AreEqual((Date.Today + Frequency.d1) - Frequency.d1, Date.Today);
            Assert.AreEqual((Date.Today - Frequency.d1) + Frequency.d1, Date.Today);
            Assert.AreEqual(Date.Tomorrow - Date.Today, TimeSpan.FromDays(1));
            Assert.AreEqual(Date.Today + Frequency.d2 - Date.Today, TimeSpan.FromDays(2));
            Assert.AreEqual(Date.Today + Frequency.w - Date.Today, TimeSpan.FromDays(7));
            Assert.AreNotEqual(Date.Today, 0);
            Assert.AreEqual("7/14/2015",new Date(new DateTime(2015,7,14)).ToString());
        }
    }
}

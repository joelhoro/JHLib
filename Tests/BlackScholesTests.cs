using Microsoft.VisualStudio.TestTools.UnitTesting;
using JHLib.QuantLIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using JHLib.QuantLIB.Utils;

namespace Tests
{
    [TestClass]
    public class BlackScholesTests
    {
        const double TOLERANCE = 1e-6;

        [TestMethod]
        public void BlackScholesPrice()
        {
            double S = 105;
            double K = 100;
            double T = 2;
            double r = 0.02;
            double vol = 0.2;
            OptionType optionType = OptionType.Call;
    
            double price = BlackScholes.Price(optionType, S, K, T, r, vol);
            double priceActual = 13.640652756637222;
            Assert.AreEqual(price, priceActual,TOLERANCE);
            Console.WriteLine("Testing");
//            Console.WriteLine(@"##teamcity[setParameter name='ddd' value='fff']");
            var number = new Random().Next(10) + 25;
            TeamCity.Log("mymetric", number);
            
            //Debugger.Launch();
            File.WriteAllText("artifact.txt", "Testing at " + DateTime.Now.ToString());

        }

        [TestMethod]
        public void BlackSholesImpliedVol()
        {
            double S = 105;
            double K = 100;
            double T = 2;
            double r = 0.02;
            OptionType optionType = OptionType.Call;

            File.WriteAllText("artifact2.txt", "Testing");

            var rnd = new Random(0);
            for(int i = 0; i < 5; i++)
            {
                double marketPrice = 5 + rnd.NextDouble() * 20;
                double vol = BlackScholes.ImpliedVol(optionType, marketPrice, S, K, T, r);
                double price = BlackScholes.Price(optionType, S, K, T, r, vol);
                Console.WriteLine("{0} vs. {0}", price, marketPrice);
                Assert.AreEqual(price, marketPrice, TOLERANCE);
            }
        }

        [TestMethod]
        public void BlackScholesSpeed()
        {
            DateTime t = DateTime.Now;
            double N = 100000;
            double total = 0;
            for(int i = 0; i < N; i++)
                total += BlackScholes.Price(OptionType.Call,100,100,3,0.03, 0.2);

            double elapsedμs = (DateTime.Now - t).TotalMilliseconds / N * 1000;
            const double MAXTIME = 0.8; // μ-seconds
            Console.WriteLine("Average over {0} runs: {1} μs elapsed", N, elapsedμs);
            Assert.IsTrue(elapsedμs < MAXTIME);
            TeamCity.Log("QL - Average Blackscholes calc time (ns)", Math.Floor(elapsedμs*1000));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{

    class LinqExamples
    {
        struct CashFlow
        {
            public int date; 
            public double amount;
            public override string ToString()
            {
                return String.Format("{0} on {1}", amount, date);
            }
        }

        struct accumulator { public double total, lowerLimit; }

        public static double Integral(Func<double,double> f, double a, double b, int N)
        {
            if (a > b) return -Integral(f, b, a, N);
            if (a == b) return 0;

            double dx = (b-a) / N;

            var points = Enumerable
                        .Range(1,N)
                        .Select( idx => a + idx*dx )
                        .Aggregate( new accumulator { total = 0, lowerLimit = f(a) }, 
                            (accu,x) =>
                            {
                                double upperLimit = f(x);
                                return new accumulator
                                {
                                    total = accu.total + (accu.lowerLimit + upperLimit) / 2 * dx,
                                    lowerLimit = upperLimit
                                };
                            } );

            //Console.WriteLine(points);
            return 0;
        }

        public static void CalculateIntegral()
        {
            Func<double, double> f = x => x*x;
            double y = Integral(f, 0, 3,100 );

        }

        public static void Recovery()
        {
            var cashFlows = new List<CashFlow>();
            cashFlows.Add(new CashFlow { date = 1, amount = 10 });
            cashFlows.Add(new CashFlow { date = 2, amount = 10 });
            cashFlows.Add(new CashFlow { date = 3, amount = 10 });
            cashFlows.Add(new CashFlow { date = 4, amount = 10 });
            cashFlows.Add(new CashFlow { date = 5, amount = 10 });
            cashFlows.Add(new CashFlow { date = 6, amount = 10 });
            cashFlows.Add(new CashFlow { date = 6, amount = 100 });

            foreach (var recovery in new double[] { 0, 0.5, 0.7, 0.9, 0.95, 0.99, 0.999, 0.9999999 })
            {
                double r = 0.1;
                double cs = 0.005;
                //double recovery = 0.4;

                Func<double, double> DF = (date) => Math.Exp(-r * date);
                Func<double, double> SurvivalProb = (date) => Math.Exp(-cs / (1 - recovery) * date);

                var total =
                    from cashFlow in cashFlows
                    select new
                    {
                        amount = cashFlow.amount,
                        pv = cashFlow.amount * DF(cashFlow.date) * SurvivalProb(cashFlow.date)
                    };

                var npv = cashFlows.Select(cashFlow =>
                {
                    double pv = cashFlow.amount * DF(cashFlow.date) * SurvivalProb(cashFlow.date);
                    double amount = cashFlow.amount;
                    return new { amount = amount, pv = pv };
                }).Sum(x => x.pv);

                var npv2 = cashFlows.Aggregate(0.0, ( runningtotal, cashFlow ) =>
                        runningtotal + cashFlow.amount * DF(cashFlow.date) * SurvivalProb(cashFlow.date)
                    );

                int maturity = cashFlows[cashFlows.Count - 1].date;
                double step = 0.1;
                var dates = new List<double>();
                for (double t = 0; t < maturity; t += step)
                    dates.Add(t);

                double previousSurvival = 1;
                var recoveryvalue = dates.Sum(date =>
                {
                    double survival = SurvivalProb(date);
                    double value = DF(date) * (previousSurvival - survival) * recovery * 100;
                    previousSurvival = survival;
                    return value;
                });

                Console.WriteLine("{3,8:F6}: {0,8:F3}+{1,8:F3}={2,8:F3}", npv, recoveryvalue, npv + recoveryvalue, recovery);
            }
        }

        public static void Pairs()
        {
            int[] A = { 0, 1, 2 };
            string[] B = { "a",  "c" };
            bool[] C = { true, false };

            var pairs = from a in A from b in B from c in C select new { x=a*a, b, c };

            foreach(var p in pairs)
            {
                Console.WriteLine(p);
            }
        }

        public static void Index()
        {
            string[] B1 = { "a", "b", "c", "d" };
            string[] B2 = { "A", "B", "C", "D" };
            var list = B1
                        .Select((x, i) => new { x,i })
                        .Zip(B2, (x, y) => new { x, y });

            foreach(var y in list)
                Console.WriteLine(y);
        }

        public static void TestRange()
        {
            //int N = 10;
            var a = new string[] {"A","B","C"};
            var b = new int[] { 11, 22, 33 };
            int N = a.Count();
            var x = Enumerable
                        .Range(0,N)
                        .Select( i => new { i, a = a[i], b = b[i] });

            //var z = a.Zip(b,)
            var y = 1;

            


        }

    }
}

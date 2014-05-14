using JHLib.XLFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleApplication
{
    using JHLib.QuantLIB;
    using MathNet.Numerics.Distributions;
    using System.Diagnostics;
    using System.IO;
    using System.Web.Script.Serialization;


    class Program
    {
        static void TestHandle()
        {
            Dictionary<string, object> handlestore = new Dictionary<string, object>();

            handlestore["test1"] = new bool[5];
            handlestore["test2"] = "brolmiche";
            handlestore["test3"] = new double[3];
            handlestore["test4"] = 4543.123;

            HandleViewer handleviewer = new HandleViewer(handlestore);
            

            Application.Run(handleviewer);
        }

        static void EquityModel()
        {
            string ticker = "MSFT";
            var equity = Equity.Lookup(ticker);

            Date date;
            Context.Initialize();
            date = Context.TODAY;

            var frequency = new Frequency(180);
            List<Date> dates = Date.GenerateSchedule(date, frequency, 10).ToList();

            var model = new EquityModel { equity = equity, diffusiondates = dates, N = 1000000 };
            var normal = new Normal();
            var output = "";
            const string NEWLINE = "\n";

            //DumpToFile(model);
            DumpToFile(new Date(DateTime.Now));
            

            var t = DateTime.Now;
            int seeds = 10;
            //Debugger.Launch();

            for (int i = 0; i < seeds; i++ )
            {   
                model.ComputePaths(normal);
                //Console.WriteLine(model.PathsOnDate(date));
                var values = model.PathsOnDate(dates[dates.Count-1]);
                var mean = values.Average() - 100;
                var stdev = Math.Sqrt((values.PointwiseMultiply(values)).Average() - mean * mean) / Math.Sqrt(model.N);
                var value = String.Format("{0} +- {1}", mean, stdev);
                output += value + NEWLINE;
            }
            output += NEWLINE + "Time = " + (DateTime.Now - t).TotalSeconds / seeds;

            MessageBox.Show(output);

        }

        private static void DumpToFile(object obj)
        {
            string filename = @"c:\temp\test.txt";
            var file = new StreamWriter(filename);
            //ObjectDumper.Dumper.Dump(model, "Model", file);
            var json = new JavaScriptSerializer().Serialize(obj);
            file.Write(json);
            file.Close();
            Process.Start(filename);
        }

        public static void BlackScholesTest(int N)
        {
            double x = 0;
            for (int i = 0; i < N; i++)
                x += BlackScholes.ImpliedVol(OptionType.Call, 7.5, 100, 100, 1, 0, 0.2);
                //x += BlackSholes.BlackScholes(OptionType.Call, 100, 100, 1, 0, 0.2);

            double v = BlackScholes.ImpliedVol(OptionType.Call, 7.5, 100, 100, 1, 0, accuracy: 1e-3);
            double p = BlackScholes.Price(OptionType.Call, 100, 100, 1, 0, v);
            Console.WriteLine(p);
        }

        public static void Main()
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("============================");
            LinqExamples.TestRange();
            Console.WriteLine("============================");
        }
    }

    class A
    {
        const double PI = 3.141592;

        double CalculatePaintNeeded(double paintPerUnit, double radius)
        {
            // Select any of the following:
            // 1. The entire next line of code.
            // 2. The right-hand side of the next line of code.
            // 3. Just "PI *" of the right-hand side of the next line
            //    of code (to see the prompt for selection expansion).
            // 4.  All code within the method body.
            // ...Then invoke Extract Method.

            double area = PI * radius * radius;

            return area / paintPerUnit;
        }
    }
}

//using JHLib.XLFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleApplication
{
    using JHLib.QuantLIB;
    using JHLib.QuantLIB.Core;
    using JHLib.QuantLIB.Graph;
    using JHLib.QuantLIB.Model;
    using JHLib.QuantLIB.Pricers;
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

            //HandleViewer handleviewer = new HandleViewer(handlestore);
            

            //Application.Run(handleviewer);
        }

        static void EquityModel()
        {
            string ticker = "MSFT";
            var equity = Equity.Lookup(ticker);

            Date date;
            Context.Initialize();
            date = Context.TODAY;

            Func<double, double> payoff = x => Math.Max(x - 100, 0)+Math.Max(100-x,0);
            var mcPricer = new MCPricer { equity = equity, maturity = date + new Frequency(360), Payoff = payoff };
            var p = mcPricer.Price();
            return;
            var frequency = new Frequency(180);
            List<Date> dates = Date.GenerateSchedule(date, frequency, 10).ToList();

            var model = new EquityModel(equity, dates, 1000000);
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

        public void TestCPP()
        {

        }


        public static void Main()
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("============================");

            EquityModel();
            return;


            var a = new GraphNodelet<double>(1);
            var b = new GraphNodelet<double>(2);
            var c = new GraphNodelet<double>(() => a.V + b.V);
            var d = new GraphNodelet<double>(() => b.V + c.V);
            var e = new GraphNodelet<double>(() => b.V + d.V);
            
            var x = c.V;
            var y = e.V;
            c.DebugMode = true;
            c.Invalidate();
            x = e.V;

            Console.WriteLine("============================");
        }
    }

}

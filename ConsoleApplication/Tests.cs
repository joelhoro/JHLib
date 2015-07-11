using JHLib.QuantLIB;
using JHLib.QuantLIB.Core;
using JHLib.QuantLIB.Graph;
using JHLib.QuantLIB.Model;
using JHLib.XLFunctions;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ConsoleApplication
{
    public enum HobbyFrequency { Daily, Weekly, Monthly };

    public struct Hobby
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public HobbyFrequency Frequency;
    }

    public struct Employee
    {
        [XmlAttribute]
        public string Name;
        //        [XmlAttribute]
        public List<Hobby> Hobbies;
        [XmlAttribute]
        public int Age;
    }

    public class Tests
    {
        static void TestHandle()
        {
            Dictionary<string, object> handlestore = new Dictionary<string, object>();

            handlestore["test1"] = new bool[5];
            handlestore["test2"] = "brolmiche";
            handlestore["test3"] = new double[3];
            handlestore["test4"] = 4543.123;

            HandleViewer handleviewer = new HandleViewer();//handlestore);
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

            var model = new EquityModel { equity = equity, diffusiondates = dates, N = 100000 };
            var normal = new Normal();
            var output = "";
            const string NEWLINE = "\n";

            //DumpToFile(model);
            //DumpToFile(new Date(DateTime.Now));


            var t = DateTime.Now;
            int seeds = 10;
            //Debugger.Launch();

            for (int i = 0; i < seeds; i++)
            {
                model.ComputePaths(normal);
                //Console.WriteLine(model.PathsOnDate(date));
                var values = model.PathsOnDate(dates[dates.Count - 1]);
                var mean = values.Average() - 100;
                var stdev = Math.Sqrt((values.PointwiseMultiply(values)).Average() - mean * mean) / Math.Sqrt(model.N);
                var value = String.Format("{0} +- {1}", mean, stdev);
                output += value + NEWLINE;
            }
            output += NEWLINE + "Time = " + (DateTime.Now - t).TotalSeconds / seeds;

            MessageBox.Show(output);

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


        public static List<T> Test<T>(T x)
        {
            return new List<T>() { x, x, x };
        }

        public static void GraphTest()
        {

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

        public static void TestReflection()
        {

            //            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //          var assembly = Assembly.GetAssembly(typeof(BlackScholes));

            var assemblyName = "QL";
            var objectName = "JHLib.QuantLIB.BlackScholes";
            var method = "CND2";
            var assembly = Assembly.Load(assemblyName);
            var objType = assembly.GetType(objectName);
            var args = new object[] { 0.5 };
            //var type = typeof();
            //var type = Type.GetType("BlackScholes", false, true);
            //var type2 = typeof(BlackScholes).GetType();
            var pi = objType.GetMethod(method);
            Type returnType = pi.ReturnType;
            var genmethod = typeof(Program).GetMethod("Test");
            var generic = genmethod.MakeGenericMethod(returnType);
            var t = generic.Invoke(null, new object[] { 12 });
            var ret = (objType.InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                null, null, args));

            var i = 123;

            //EquityModel();
            return;
        }

    }
}

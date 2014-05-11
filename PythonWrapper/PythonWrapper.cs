using IronPython.Hosting;
using MathNet.Numerics.Distributions;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;

namespace JHLib.PythonWrapper
{
    public class MCModel
    {
        public static IEnumerable<double> Paths(double S, double sigma, double maturity, double N)
        {
            double var = sigma*sigma*maturity;

            Normal n = new Normal();
            for (int i = 0; i < N; i++ )
            {
                double normalsample = n.Sample();
                double Sf = S * Math.Exp(-0.5 * var + Math.Sqrt(var) * normalsample);
                yield return Sf;
            }
        }

    }

    public class Wrapper
    {
        public static ScriptScope LoadFile(string filename)
        {
            filename = @"c:\Users\joel\Documents\Visual Studio 2013\Projects\JHLib\Ironpython\Ironpython\test.py";
            var ipy = Python.CreateRuntime();
            ScriptScope file = ipy.UseFile(filename);
            return file;
        }

        public static ScriptScope LoadString(string code)
        {
            var engine = Python.CreateEngine();
            var calculator = engine.CreateScope();
            engine.Execute(code, calculator);

            return calculator;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Wrapper.LoadFile("");
        }
    }
}

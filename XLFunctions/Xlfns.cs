using ExcelDna.Integration;
using JHLib.PythonWrapper;
using Microsoft.Scripting.Hosting;
using System;

namespace JHLib.XLFunctions
{
    public class Addins: IExcelAddIn
    {
        const string FNS_HANDLES = "Custom functions (handles)";
        const string FNS_PYTHON = "Custom functions (Python)";
        const string FNS_ANALYTIC = "Custom functions (Analytic)";

        static private ExcelDNALogForm form;

        public void AutoOpen()
        {
            form = new ExcelDNALogForm(); 
            form.Show();
            form.Print("Loaded add-in");
        }

        public void AutoClose() 
        { 
            form.Close();  
        }

        public static void Log(string output)
        {
            form.Print(output);
        }

        #region Handles

        private static string CreateHandle(object obj)
        {
            return CreateHandle(new object[1,1] { { obj } } );
        }

        [ExcelFunction(Description = "Create a handle to store an object", Category = FNS_HANDLES )]
        public static string CreateHandle(object[,] obj)
        {
            Log("Creating handle for " + obj.ToString());
            ExcelReference reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
            string tag = String.Format("{0}/{1}", reference.ColumnFirst, reference.RowFirst);
            return Handles.Create(obj, tag);
        }

        [ExcelFunction(Description = "Retrieve a handle", Category = FNS_HANDLES)]
        public static object GetHandle(string handlename)
        {
            Log("Retrieving handle for  " + handlename);            
            return Handles.Get(handlename);
        }

        #endregion

        #region Python functions
        [ExcelFunction(Description = "Creates a Python fn out of code", Category = FNS_PYTHON)]
        public static string PythonFn(object[] lines)
        {
            string code = String.Join("\n", lines);
            ScriptScope scope = Wrapper.LoadString(code);
            string fnname = null;
            foreach( string name in scope.GetVariableNames() )
            {
                if (!name.StartsWith("__"))
                    fnname = name;
            }

            if (fnname != null)
                return CreateHandle(scope.GetVariable(fnname));
            else
                return null;
        }

        [ExcelFunction(Description = "Loads a Python script", Category = FNS_PYTHON)]
        public static string LoadPythonScript(string filename)
        {
            dynamic file = Wrapper.LoadFile(filename as string);
            return CreateHandle(file.fn);
        }

        [ExcelFunction(Description = "Extract a fn from a Python file (handle)", Category = FNS_PYTHON)]
        public static string ExtractFn(string filehandle, string functionName)
        {
            dynamic file = GetHandle(filehandle);
            var fn = file[0,0].GetVariable(functionName);
            return CreateHandle(fn);
        }

        [ExcelFunction(Description = "Apply a Python fn ", Category = FNS_PYTHON)]
        public static object ApplyFn(string functionhandle, object value)
        {
            dynamic fn = GetHandle(functionhandle);
            return fn[0,0](value);
        }


        public static double[,] MCPrice(string payoffhandle,double maturity, double N)
        {
            dynamic payoff = GetHandle(payoffhandle);
            double S = 100;
            double sigma = 0.2;

            double total = 0;
            double totalvar = 0;
            foreach( double Sf in MCModel.Paths(S,sigma,maturity,N) )
            {
                double value = payoff[0, 0](Sf);
                total += value;
                totalvar += value * value;
            }
            double average = total / N;
            double stdev = Math.Sqrt((totalvar / N - average * average) / N);
            return new double[2,1] {{ average },{ stdev } };
        }

        #endregion



    }
}

using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
//using JHLib.PythonWrapper;
//using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using JHLib.QuantLIB;

namespace JHLib.XLFunctions
{
    [ComVisible(true)]
    public class MyRibbon : ExcelRibbon
    {

        public void RefreshCmd(IRibbonControl control)
        {
            //Years.Instance.Year(2012).Set_Trial_Balance_Data();
            //Excel.Application xlApp = (Excel.Application)ExcelDnaUtil.Application;
            MessageBox.Show("Refresh Done!");

        }

        public void MenuChoice(IRibbonControl control)
        {
            MessageBox.Show("Menu!");
        }
    }

    [ComVisible(true)]
    public class Addins : IExcelAddIn
    {
        const string FNS_HANDLES = "Custom functions (handles)";
        const string FNS_PYTHON = "Custom functions (Python)";
        const string FNS_ANALYTIC = "Custom functions (Analytic)";

        static private ExcelDNALogForm form;

        struct Menu {
            public string Name;
            public List<MenuItem> Items;
            public Menu(string name, List<MenuItem> items)
            {
                Name = name;
                Items = items;
            }

            public void AddToXL()
            {
                var obj = new object[Items.Count + 1, 5];
                obj[0, 0] = Name;
                int i = 1;
                foreach (var item in Items)
                {
                    obj[i, 0] = item.Name;
                    obj[i++, 1] = item.Command;
                }
                XlCall.Excel(XlCall.xlfAddMenu, 1, obj, "Help");
            }

        }

        struct MenuItem {
            public string Name;
            public string Command;
            public MenuItem(string name, string command)
            {
                Name = name;
                Command = command;
            }
        }

        public void AutoOpen()
        {
            form = new ExcelDNALogForm();
            //ToggleForm();
            form.Print("Loaded add-in");

            var menus = new List<Menu>() {
                new Menu( "Main menu", new List<MenuItem>() {
                    new MenuItem("Ring bell", "RingBell"),
                    new MenuItem("Toggle", "ToggleForm"),
                }),
            };

            foreach (var menu in menus)
                menu.AddToXL();
        }

        private void command()
        {

        }

        public void AutoClose() 
        { 
            form.Close();
            XlCall.Excel(XlCall.xlfDeleteMenu,
            1.0 /* Worksheet and macro sheet menu bar*/,
            "Testing");
        }

        public static void RingBell()
        {
            System.Console.Beep();
        }

        public static bool LogMessage(string output)
        {
            form.Print(output);
            return true;
        }

        public static bool _formVisibility = false;
        public static void ToggleForm()
        {
            _formVisibility = !_formVisibility;
            SetFormVisibility(_formVisibility);
        }

        public static bool SetFormVisibility(bool status)
        {
            if (status) {
                form.Show();
                form.Activate();
            }
            else
                form.Hide();
            return status;
        }

        #region Handles

        private static string CreateHandle(object obj)
        {
            return CreateHandle(new object[1,1] { { obj } } );
        }

        public static T EnumParse<T>(string typeName) 
        {
            return (T)Enum.Parse(typeof(T), typeName);
        }
        public static double BS(string type, double fwd, double strike, double rate, double vol, double T, string returnType)
        {
            var optionType = EnumParse<OptionType>(type);
            PriceType priceType;
            switch (returnType)
            {
                case "":
                case "P":
                    priceType = PriceType.Price;
                    break;
                case "D":
                    priceType = PriceType.Δ;
                    break;
                case "G":
                    priceType = PriceType.Γ;
                    break;
                case "V":
                    priceType = PriceType.Vega;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return BlackScholes.Price(optionType, fwd, strike, T, rate, vol, priceType);
        }

        [ExcelFunction(Description = "Create a handle to store an object", Category = FNS_HANDLES)]
        public static string CreateHandle(object[,] obj)
        {
            LogMessage("Creating handle for " + obj.ToString());
            ExcelReference reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
            string tag = String.Format("{0}/{1}", reference.ColumnFirst, reference.RowFirst);
            return Handles.Create(obj, tag);
        }

        [ExcelFunction(Description = "Retrieve a handle", Category = FNS_HANDLES)]
        public static object GetHandle(string handlename)
        {
            LogMessage("Retrieving handle for  " + handlename);            
            return Handles.Get(handlename);
        }

        [ExcelFunction(Description = "Retrieve a handle", Category = FNS_HANDLES)]
        public static object[,] GetArray(string handlename)
        {
            ExcelReference reference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
            var result = GetHandle(handlename) as object[,];
            if (result.GetLength(0) > reference.RowLast - reference.RowFirst + 1 ||
                result.GetLength(1) > reference.ColumnLast - reference.ColumnFirst + 1)
                return new object[,] { { "Area is to small" } };
            return result;
        }



        #endregion

        #region Python functions
        //[ExcelFunction(Description = "Creates a Python fn out of code", Category = FNS_PYTHON)]
        //public static string PythonFn(object[] lines)
        //{
        //    string code = String.Join("\n", lines);
        //    ScriptScope scope = Wrapper.LoadString(code);
        //    string fnname = null;
        //    foreach( string name in scope.GetVariableNames() )
        //    {
        //        if (!name.StartsWith("__"))
        //            fnname = name;
        //    }

        //    if (fnname != null)
        //        return CreateHandle(scope.GetVariable(fnname));
        //    else
        //        return null;
        //}

        //[ExcelFunction(Description = "Loads a Python script", Category = FNS_PYTHON)]
        //public static string LoadPythonScript(string filename)
        //{
        //    dynamic file = Wrapper.LoadFile(filename as string);
        //    return CreateHandle(file.fn);
        //}


        internal interface IVariable {
            string GetVariable(string name);
        }
        private interface PythonFn
        {

            IVariable this[int k, int v] { get;  }
        }

        [ExcelFunction(Description = "Extract a fn from a Python file (handle)", Category = FNS_PYTHON)]
        public static string ExtractFn(string filehandle, string functionName)
        {
            var file = GetHandle(filehandle) as PythonFn;
            var fn = file[0,0].GetVariable(functionName);
            return CreateHandle(fn);
        }

        public static bool RefreshPivot()
        {
            //ExcelDnaUtil.Application.
            return true;
        }

        public static bool RefreshPivot2()
        {
            //ExcelDnaUtil.Application.
            return true;
        }

        public static string BuildQuery(object obj)
        {
            var conditions = obj as object[,];
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < conditions.GetLength(0); i++)
            {
                if (conditions[i, 1] is ExcelDna.Integration.ExcelEmpty)
                    continue;
                var value = conditions[i, 1].ToString();
                dict[conditions[i, 0].ToString()] = value;
            }
            return BuildQueryFromDictionary(dict);
        }

        /// <summary>
        /// Given a list of fields and conditions, build the corresponding SQL predicate
        /// Fund => BMEA,BMCA
        /// Ticker => %SPX,%VIX
        /// will return "Fund IN ('BMEA','BMCA') AND ( Ticker LIKE '%SPX' OR Ticker LIKE '%VIX')"
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        static string BuildQueryFromDictionary(Dictionary<string, string> conditions)
        {
            if (!conditions.Any())
                return "1=1";
            return string.Join(" AND ", conditions
                .Select(kvp => { 
                    var transformValues = kvp.Value.Split(',').Select(r => "'" + r + "'");
                    if (transformValues.Any(v => v.Contains("%")))
                        return "(" + string.Join(" OR ", transformValues.Select(v => kvp.Key + " LIKE " + v)) + ")";
                    return string.Format("{0} in ({1})", kvp.Key, string.Join(",", transformValues));
                })
               );
        }


        [ExcelFunction(Description = "Apply a Python fn ", Category = FNS_PYTHON)]
        public static object ApplyFn(string functionhandle, object value)
        {
            dynamic fn = GetHandle(functionhandle);
            return fn[0,0](value);
        }


        [ExcelFunction(Description = "Apply a Python fn ", Category = FNS_PYTHON)]
        public static object MultiplyBy2(double value)
        {
            return value * 4;
        }


        public static double Rename2(string from, string to)
        {
            Handles.Rename(Handles.GetHandleName(from), to);
            return 0;
        }

        //public static double[,] MCPrice(string payoffhandle,double maturity, double N)
        //{
        //    dynamic payoff = GetHandle(payoffhandle);
        //    double S = 100;
        //    double sigma = 0.2;

        //    double total = 0;
        //    double totalvar = 0;
        //    foreach( double Sf in MCModel.Paths(S,sigma,maturity,N) )
        //    {
        //        double value = payoff[0, 0](Sf);
        //        total += value;
        //        totalvar += value * value;
        //    }
        //    double average = total / N;
        //    double stdev = Math.Sqrt((totalvar / N - average * average) / N);
        //    return new double[2,1] {{ average },{ stdev } };
        //}

        #endregion

//           Dim myCommand() As Object = New Object() {"My Menu Item", 
//"MyExposedFunction"} 



//Registered function... 

//    Public Shared Sub MyExposedFunction() 
//        MsgBox("Hello World") 
//    End Sub 

    }
}

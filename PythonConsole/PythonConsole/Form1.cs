using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonConsole
{
    public partial class Form1 : Form
    {
        [DllImport("Kernel32.dll")]
        static extern Boolean AllocConsole();
        [DllImport("Kernel32.dll")]
        static extern Boolean FreeConsole();

        [DllImport("Kernel32.dll")]
        static extern void SetConsoleTitle(string title);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public Thread consoleThread;
        public class TestObject
        {
            public int factor;
            public int Multiplier(int x)
            {
                return factor*x;
            }
        }

        private void OpenConsole()
        {
            var variables = new Dictionary<string,object>();
            variables["A"] = 123;
            variables["B"] = new List<string>() { "AAA", "BBB", "CCC" };
            variables["C"] = new TestObject { factor = 10 };

            var ipy = Python.CreateRuntime();
            dynamic Python_Script = ipy.UseFile(@"..\..\shell.py");
            consoleThread = new Thread( () => Python_Script.startconsole(variables) );
            consoleThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( !AllocConsole() )
                    MessageBox.Show("Could not open console...");
            SetConsoleTitle("Python console");
            OpenConsole();
        }
    }
}

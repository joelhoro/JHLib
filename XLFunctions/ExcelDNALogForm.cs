using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JHLib.XLFunctions
{
    public partial class ExcelDNALogForm : Form
    {
        public ExcelDNALogForm()
        {
            InitializeComponent();
        }

        public void Print(string message)
        {
            textBox1.Text += message + "\r\n";
         //   textBox1.ScrollToCaret();
            Refresh();
        }
    }
}

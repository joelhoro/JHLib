using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JHLib.XLFunctions
{
    public partial class HandleViewer : Form
    {
        Dictionary<string, object> handles;

        public HandleViewer(Dictionary<string,object> handles)
        {
            this.handles = handles;

            InitializeComponent();
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (string key in handles.Keys)
                dataGridView1.Rows.Add(new string[] { key, handles[key].ToString() });
        }

        private void SwitchActiveHandle(object sender, EventArgs e)
        {
            label1.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }
    }
}

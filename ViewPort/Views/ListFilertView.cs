using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using System.IO.Compression;
using System.IO;

namespace ViewPort.Views
{
    public partial class ListFilertView : Form
    {
        FormViewPort Main;
        List<string> list_filter = new List<string>();
        public List<string> List_Filter { get => list_filter; set => list_filter = value; }

        public ListFilertView(FormViewPort parent)
        {
            InitializeComponent();
            Main = parent;
            DataTable dt = new DataTable();
            dt.Columns.Add("List Name");
            //Main.fil

            dt.PrimaryKey = new DataColumn[] { dt.Columns["List Name"] };


            dataGridView1.DataSource = dt;
        }

        private void 복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List_Filter.Clear();
            char[] rowSplitter = { '\r', '\n' };
            char[] columnSplitter = { '\t' };

            IDataObject datainClip = Clipboard.GetDataObject();

            if (datainClip == null)
                return;
            string stringinClip = (string)datainClip.GetData(DataFormats.Text);


            string[] rowsInClipboard = stringinClip.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);

            int r = dataGridView1.SelectedCells[0].RowIndex;
            int c = dataGridView1.SelectedCells[0].ColumnIndex;


            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();

            DataTable Dt = new DataTable();
            Dt.Columns.Add("List Name");
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns["List Name"] };

            for (int i = 0; i < rowsInClipboard.Length; i++)
            {
                if(rowsInClipboard[0].Length >7)
                {
                    Dt.Rows.Add(rowsInClipboard[i]);
                    List_Filter.Add(rowsInClipboard[i].Substring(0, 12));
                }
                else
                {
                    Dt.Rows.Add(rowsInClipboard[i]);
                    List_Filter.Add(rowsInClipboard[i]);
                }
            }

            dataGridView1.DataSource = Dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.List_Filter_Main = List_Filter;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

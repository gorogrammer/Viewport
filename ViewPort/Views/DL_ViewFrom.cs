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
    public partial class DL_ViewFrom : Form
    {

        FormViewPort Main;
        public DL_ViewFrom(FormViewPort parent)
        {
            InitializeComponent();
            Main = parent;
            
            DataTable dt = new DataTable();
            dt.Columns.Add("SDIP Code");
            dt.Columns.Add("SDIP 불량명");
            dt.Columns.Add("자동불량수(투입수)");
            dt.Columns.Add("자동불량율(%)");
            dt.Columns.Add("Limit(%)");

            dataGridView1.DataSource = dt;

        }

        public void Dl_LIst_ADD(List<string> list)
        {
            DataTable Dt = (DataTable)dataGridView1.DataSource;

            Dt.Rows.Clear();
            textBox1.Text = string.Empty;

            if (list.Count >0)
            {
                list.RemoveAt(0);
                list.RemoveAt(0);

                DataTable dt = new DataTable();
                dt.Columns.Add("SDIP Code");
                dt.Columns.Add("SDIP 불량명");
                dt.Columns.Add("자동불량수(투입수)");
                dt.Columns.Add("자동불량율(%)");
                dt.Columns.Add("Limit(%)");

                foreach (string dl in list)
                {
                    if(dl.Split(',').Length == 2)
                    {
                        textBox1.Text = dl;
                    }
                    else
                    {
                        string[] dl_text = dl.Split(',');
                        string[] col_2_3 = dl_text[1].Split(':');
                        dt.Rows.Add(dl_text[0], col_2_3[0], col_2_3[1], dl_text[2], dl_text[3]);

                    }

                    dataGridView1.DataSource = dt;
                }
            }
            else
            {

            }
           

          
        }
    }
}

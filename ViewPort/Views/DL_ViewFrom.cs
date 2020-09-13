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
        }

        public void Dl_LIst_ADD(List<string> list)
        {
            for(int i =0; i < list.Count; i++)
            {
                listBox1.Items.Add(list[i]);
            }
        }
    }
}

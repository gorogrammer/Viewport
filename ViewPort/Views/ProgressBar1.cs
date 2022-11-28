using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPort.Views
{
    public partial class ProgressBar1 : Form
    {
        public ProgressBar1()
        {
            InitializeComponent();
        }
        public void valuePlus()
        {
            progressBar2.Value++;
        }
        public void initBar()
        {
            progressBar2.Value = 0;
        }
        public void valueChange(int Plus)
        {
            this.Activate();
            progressBar2.Value = Plus;
        }
        public void MaxValue(int max)
        {
            progressBar2.Maximum = max;
        }

    }
}

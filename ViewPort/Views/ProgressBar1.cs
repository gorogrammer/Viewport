using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
            progressBar2.BringToFront();
        }
        public void valueChange(int Plus)
        {
            progressBar2.BringToFront();
            while (progressBar2.Value != Plus)
            {
                progressBar2.Value = progressBar2.Value + 1;
                Thread.Sleep(1);
            }
        }
        public void MaxValue(int max)
        {
            progressBar2.BringToFront();
            progressBar2.Maximum = max;
        }

    }
}

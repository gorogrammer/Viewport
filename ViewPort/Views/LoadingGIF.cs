using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using System.IO.Compression;
using System.IO;


namespace ViewPort.Views
{
    public partial class LoadingGIF : Form
    {
        public Action Function { get; set; }
        public LoadingGIF()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Activate();
        }

        public LoadingGIF(Form parent)
        {
            InitializeComponent();
            if (parent != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(parent.Location.X + parent.Width / 2 - this.Width / 2, parent.Location.Y + parent.Height / 2 - this.Height / 2);
                this.Activate();
            }
            else
                this.StartPosition = FormStartPosition.CenterParent;
        }

        public void CloseWaitForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            if(pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

        }
    
    }
}

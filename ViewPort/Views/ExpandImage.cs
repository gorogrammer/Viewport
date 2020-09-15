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
    public partial class ExpandImage : Form
    {
        FormViewPort Main;

        ImageViewer open;
        public ExpandImage(ImageViewer img)
        {
            InitializeComponent();
            open = img;
        }

        public void Set_Expand_Img(Image img)
        {
            pictureBox1.Size = new Size(1000, 600);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = img;
        }
    }
}

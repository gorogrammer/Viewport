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
        Dictionary<string, ImageInfo> expand_ImgInfo = new Dictionary<string, ImageInfo>();

        public Dictionary<string, ImageInfo> Expand_ImgInfo
        {
            get { return expand_ImgInfo; }
            set { expand_ImgInfo = value; }
        }
        public ExpandImage(ImageViewer img)
        {
            InitializeComponent();
            open = img;
        }

        public void Set_Expand_Img(Image img)
        {
          
            this.Text = Expand_ImgInfo[Expand_ImgInfo.Keys.ElementAt(0)].Imagename;
            if(img.Width == img.Height)
            {
                pictureBox1.Size = new Size(img.Width*2 , img.Height*2);
                this.Size = new Size(img.Width*2, img.Height*2);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.Size = new Size(img.Width * 3, img.Height * 3);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            
            //pictureBox1.Size = new Size(img.Width*3, img.Height*3);
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = img;
            
        }

        public void Del_Expand_Pic()
        {
            Expand_ImgInfo.Clear();
        }

        private void ExpandImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Del_Expand_Pic();
        }

        private void ExpandImage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}

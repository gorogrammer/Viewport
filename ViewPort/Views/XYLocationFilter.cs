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
    
    public partial class XYLocationFilter : Form
    {
        ImageViewer Open;

        Dictionary<string, ImageInfo> dicInfo_XY_Filter = new Dictionary<string, ImageInfo>();

        public Dictionary<string, ImageInfo> DicInfo_XY_Filter { get => dicInfo_XY_Filter; set => dicInfo_XY_Filter = value; }
        public XYLocationFilter(ImageViewer parent)
        {
            InitializeComponent();
            Open = parent;
        }

        public void XY_Filter()
        {
            if (Open.OpenViewType == "FrameSetView" || Open.OpenViewType == "DLFrameSetView")
            {
              
            }
            else
            {
              
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("좌표 Filter를 진행 하시겠습니까?", "좌표 FIlter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

            }
                
        }
    }
}

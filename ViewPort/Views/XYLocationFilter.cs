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

        Dictionary<string, ImageInfo> xy_Location = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_XY_filter = new Dictionary<string, ImageInfo>();
        public Dictionary<string, ImageInfo> XY_Location { get => xy_Location; set => xy_Location = value; }
        public Dictionary<string, ImageInfo> DicInfo_XY_filter { get => dicInfo_XY_filter; set => dicInfo_XY_filter = value; }
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

        public void Set_XY_TB()
        {
            X_TB.Text = XY_Location[XY_Location.ElementAt(0).Key].X_Location;
            Y_TB.Text = XY_Location[XY_Location.ElementAt(0).Key].Y_Location;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("좌표 Filter를 진행 하시겠습니까?", "좌표 FIlter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach(string id in DicInfo_XY_filter.Keys.ToList())
                {
                    if (int.Parse(X_TB.Text) - int.Parse(Xfilter_TB.Text) <= int.Parse(DicInfo_XY_filter[id].X_Location) && int.Parse(DicInfo_XY_filter[id].X_Location) <= int.Parse(X_TB.Text) + int.Parse(Xfilter_TB.Text))
                        continue;
                    else
                        DicInfo_XY_filter.Remove(id);
                }

                X_TB.Text = string.Empty;
                Y_TB.Text = string.Empty;

                this.Close();
                Open.XY_Filter_Set(DicInfo_XY_filter);
                
            }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            X_TB.Text = string.Empty;
            Y_TB.Text = string.Empty;

            this.Close();

        }

        private void XYLocationFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            XY_Location.Clear();
        }
    }
}

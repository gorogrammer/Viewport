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
    public partial class Change_Code : Form
    {
        ImageViewer Open;
        List<string> Select_Pic_change = new List<string>();
        Dictionary<string, ImageInfo> dicInfo_Code_Change = new Dictionary<string, ImageInfo>();

        public Dictionary<string, ImageInfo> DicInfo_Code_Change { get => dicInfo_Code_Change; set => dicInfo_Code_Change = value; }
        public Change_Code(ImageViewer parent)
        {
            InitializeComponent();
            Open = parent;
        }

        public void Change_Sdip()
        {
            string code = null;
            Select_Pic_change = Open.Select_Pic_List;


            if (Open.OpenViewType == "FrameSetView" || Open.OpenViewType == "DLFrameSetView")
            {
                DicInfo_Code_Change = Open.Frame_dicInfo_Filter;


                code = Code_Change_TB.Text;
                for (int i = 0; i < Select_Pic_change.Count; i++)
                {
                    if (Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
                    {
                        Open.Frame_dicInfo_Filter[Select_Pic_change[i]].sdip_no = code;
                        Open.Frame_dicInfo_Filter[Select_Pic_change[i]].Change_Code = "Change";
                    }
                }
            }
            else if(Open.OpenViewType == "Code_200_SetView")
            {
                DicInfo_Code_Change = new Dictionary<string, ImageInfo>(Open.DicInfo_Filtered);


                code = Code_Change_TB.Text;


                for(int i =0; i < Select_Pic_change.Count; i++)
                {
                    if(Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
                    {
                        Open.DicInfo_Filtered[Select_Pic_change[i]].sdip_no = code;
                        Open.DicInfo_Filtered[Select_Pic_change[i]].Change_Code = "Change";
                    }
                }
            
            }
            else
            {
                DicInfo_Code_Change = new Dictionary<string, ImageInfo>(Open.DicInfo_Filtered);

                code = Code_Change_TB.Text;

                for (int i = 0; i < Select_Pic_change.Count; i++)
                {
                    if (Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
                    {
                        Open.DicInfo_Filtered[Select_Pic_change[i]].sdip_no = code;
                        Open.DicInfo_Filtered[Select_Pic_change[i]].Change_Code = "Change";
                    }
                }
            }

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(" SDIP 코드를 " + Code_Change_TB.Text+"로 변경하시겠습니까?", "코드 변경",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Change_Sdip();
                this.Close();
            }
        }
    }
}

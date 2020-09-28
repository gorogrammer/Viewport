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

            DicInfo_Code_Change = Open.DicInfo_Filtered;
            code = Code_Change_TB.Text;

            foreach(KeyValuePair<string, ImageInfo> pair in Open.DicInfo_Filtered)
            {
                
                    Open.DicInfo_Filtered[pair.Key].sdip_no = code;
                    Open.DicInfo_Filtered[pair.Key].Change_Code = "Change";
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(" SDIP 코드를 " + Code_Change_TB.Text+"로 변경하시겠습니까?", "코드 변경",MessageBoxButtons.YesNo) == DialogResult.Yes)
                Change_Sdip();


        }
    }
}

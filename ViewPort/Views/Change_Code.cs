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
        FormViewPort Main;
        List<string> Select_Pic_change = new List<string>();
        Dictionary<string, ImageInfo> dicInfo_Code_Change = new Dictionary<string, ImageInfo>();

        public Dictionary<string, ImageInfo> DicInfo_Code_Change { get => dicInfo_Code_Change; set => dicInfo_Code_Change = value; }
        public Change_Code(FormViewPort parent)
        {
            InitializeComponent();
            Main = parent;
        }

        public void Change_Sdip()
        {
            string code = null;
            Select_Pic_change = Main.selected_Pic;
            code = Code_Change_TB.Text;
       

     
            if (Main.ViewType == "Code_200_SetView")
            {
                
                for (int i = 0; i < Select_Pic_change.Count; i++)
                {
                    if (Main.Sdip_200_code_dicInfo.ContainsKey(Select_Pic_change[i]))
                    {
                        Main.DicInfo.Add(Select_Pic_change[i], Main.Sdip_200_code_dicInfo[Select_Pic_change[i]]);
                        Main.Sdip_200_code_dicInfo.Remove(Select_Pic_change[i]);
                        
                        Main.DicInfo[Select_Pic_change[i]].sdip_no = code;
                        Main.DicInfo[Select_Pic_change[i]].Change_Code = "Change";
                    }
                }
            }
            else
            {
               
                for (int i = 0; i < Select_Pic_change.Count; i++)
                {
                    if (Main.DicInfo.ContainsKey(Select_Pic_change[i]))
                    {
                        Main.DicInfo[Select_Pic_change[i]].sdip_no = code;
                        Main.DicInfo[Select_Pic_change[i]].Change_Code = "Change";
                    }
                }
            }

            //if (Open.OpenViewType == "FrameSetView" || Open.OpenViewType == "DLFrameSetView")
            //{
            //    DicInfo_Code_Change = Open.Frame_dicInfo_Filter;


            //    code = Code_Change_TB.Text;
            //    for (int i = 0; i < Select_Pic_change.Count; i++)
            //    {
            //        if (Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
            //        {
            //            Open.Frame_dicInfo_Filter[Select_Pic_change[i]].sdip_no = code;
            //            Open.Frame_dicInfo_Filter[Select_Pic_change[i]].Change_Code = "Change";
            //        }
            //    }
            //}
            //else if(Open.OpenViewType == "Code_200_SetView")
            //{
            //    DicInfo_Code_Change = new Dictionary<string, ImageInfo>(Open.DicInfo_Filtered);


            //    code = Code_Change_TB.Text;


            //    for(int i =0; i < Select_Pic_change.Count; i++)
            //    {
            //        if(Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
            //        {
            //            Open.DicInfo_Filtered[Select_Pic_change[i]].sdip_no = code;
            //            Open.DicInfo_Filtered[Select_Pic_change[i]].Change_Code = "Change";
            //        }
            //    }

            //}
            //else
            //{
            //    DicInfo_Code_Change = new Dictionary<string, ImageInfo>(Open.DicInfo_Filtered);

            //    code = Code_Change_TB.Text;

            //    for (int i = 0; i < Select_Pic_change.Count; i++)
            //    {
            //        if (Open.DicInfo_Filtered.ContainsKey(Select_Pic_change[i]))
            //        {
            //            Open.DicInfo_Filtered[Select_Pic_change[i]].sdip_no = code;
            //            Open.DicInfo_Filtered[Select_Pic_change[i]].Change_Code = "Change";
            //        }
            //    }
            //}
            
        
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (int.Parse(Code_Change_TB.Text) == 1)
            {
                MessageBox.Show("1번 코드로 변경되지 않습니다.");
                Code_Change_TB.Text = "";

                return;
            }
            else if (int.Parse(Code_Change_TB.Text[0].ToString()) == 2)
            {
                MessageBox.Show("200번 대 코드로 변경되지 않습니다.");
                Code_Change_TB.Text = "";

                return;
            }


            if (MessageBox.Show(" SDIP 코드를 " + Code_Change_TB.Text+"로 변경하시겠습니까?", "코드 변경",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                Change_Sdip();
                this.Close();
            }
        }

        private void Code_Change_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(int.Parse(Code_Change_TB.Text) == 1)
                {
                    MessageBox.Show("1번 코드로 변경되지 않습니다.");
                    Code_Change_TB.Text = "";

                    return;
                }
                else if(int.Parse(Code_Change_TB.Text[0].ToString())==2)
                {
                    MessageBox.Show("200번 대 코드로 변경되지 않습니다.");
                    Code_Change_TB.Text = "";

                    return;
                }


                if (MessageBox.Show(" SDIP 코드를 " + Code_Change_TB.Text + "로 변경하시겠습니까?", "코드 변경", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                    Change_Sdip();
                    this.Close();
                }
            }
        }
    }
}

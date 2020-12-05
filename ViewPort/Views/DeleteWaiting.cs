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
using ViewPort.Functions;
using System.IO.Compression;
using System.IO;
using MetroFramework.Forms;

namespace ViewPort.Views
{
    public partial class DeleteWaiting : MetroForm
    {


        FormViewPort Main;
            
        ImageViewer open = new ImageViewer();
        LoadingGIF_Func waitform = new LoadingGIF_Func();
        int Frame_Filter_check = 0;
        int Camera_Filter_check = 0;
        int EQ_Filter_check = 0;
        List<PictureBox> PictureData = new List<PictureBox>();
        List<PictureBox> Picture_Glass = new List<PictureBox>();
        List<string> Wait_del_List = new List<string>();
        Label[] DefectState, ImageNameLB, ImageNameEQ;
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        List<string> Select_Pic = new List<string>();
        List<string> Selected_Equipment_DF_List = new List<string>();
        List<int> Frame_List_Img = new List<int>();
        Dictionary<string, ImageInfo> dicInfo_Filter_Del = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Delete_Sel = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Delete_Return = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> framesort_dic =new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Camera_dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Waiting_Del_DLView = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Eq_CB_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Main_Dic = new Dictionary<string, ImageInfo>();
        int cols, rows, width, height;
        int Current_PageNum, Total_PageNum;
        Point src_Mouse_XY, dst_Mouse_XY;
        List<Tuple<string, int>> all_Equipment_DF_List = new List<Tuple<string, int>>();
        struct BoxRange { public int left, top, width, height; }
        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        int Last_Picture_Selected_Index;
        int EachPage_ImageNum;
        List<string> Print_Frame = new List<string>();
        ZipArchive zip = null;
        string zipFilePath = null;
        List<string> Change_state_List = new List<string>();

        public List<string> Return_Img { get => Select_Pic; set => Select_Pic = value; }

        public List<Tuple<string, int>> All_Equipment_DF_List { get => all_Equipment_DF_List; set => all_Equipment_DF_List = value; }
        public Dictionary<string, ImageInfo> Waiting_Img { get => dicInfo_Filter_Del; set => dicInfo_Filter_Del = value; }
        public string ZipFilePath { get => zipFilePath; set => zipFilePath = value; }

        public DeleteWaiting(FormViewPort parent) 
        {
            InitializeComponent();
            //Del_img_list.Select();

            Main = parent;
           
        }
        public void Set_EQ()
        {
            All_Equipment_DF_List.Clear();
            Equipment_DF_CLB.Items.Clear();
            int x = 1;
            int index = 0;
            foreach (string pair in Waiting_Img.Keys.ToList())
            {
                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Main_Dic[pair].EquipmentDefectName)) == -1)
                    All_Equipment_DF_List.Add(new Tuple<string, int>(Main_Dic[pair].EquipmentDefectName, 1));
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Main_Dic[pair].EquipmentDefectName));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(Main_Dic[pair].EquipmentDefectName, ++x);

                }
            }

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            Select_All_BTN_Click(null, null);
        }

        public void Set_View_Del()
        {
            if(Main_Dic.Count == 0)
            {
                Main_Dic = new Dictionary<string, ImageInfo>(dicInfo_Filter_Del);
            }
            this.KeyPreview = true;

            splitContainer1.Panel2.Controls.Clear();
            PictureData.Clear();

            if (Frame_Filter_check == 1)
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(framesort_dic);
            }
            else if (Camera_Filter_check ==1 )
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(framesort_dic);
            }
            else if(EQ_Filter_check==1)
            {

            }
            else
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(Main_Dic);
            }
            cols = 7;
            rows = 3;
            width = 120;
            height = 120;

            Current_PageNum = 1;

            S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter_Del.Count - 1) / (cols * rows)) + 1;
            E_Page_TB.Text = Total_PageNum.ToString();


            Set_PictureBox();
            Set_Image();
            Del_img_list.Text = String.Format("{0:#,##0}", dicInfo_Filter_Del.Count);
            Last_Picture_Selected_Index = -1;
            this.Focus();
        }

        private void Set_PictureBox()
        {
            int pre_cols, pre_rows;
            PictureBox temp_PB;
            Label temp_LB;
            Refresh();


            pre_cols = cols;
            pre_rows = rows;

           

            ImageRangeInfo.Clear();

            BoxRange t_RangeInfo = new BoxRange();
            for (int i = 0; i < (cols * rows); i++)
            {
                t_RangeInfo.left = (width * (i % (cols))) + ((i % (cols)) * 16) + 16;// + 375;
                t_RangeInfo.top = (height * (i / cols)) + ((i / cols) * 16) + 16;// + 35;
                t_RangeInfo.width = width;
                t_RangeInfo.height = height;

                ImageRangeInfo.Add(t_RangeInfo);
            }

            for (int i = (PictureData.Count - 1); i >= 0; i--)
            {
                splitContainer1.Panel2.Controls.Remove(PictureData.ElementAt(i));
                PictureData.ElementAt(i).Dispose();
                PictureData.ElementAt(i).Image = null;
            }
            PictureData.Clear();


            for (int i = (Picture_Glass.Count - 1); i >= 0; i--)
            {   splitContainer1.Panel2.Controls.Remove(Picture_Glass.ElementAt(i));
                Picture_Glass.ElementAt(i).Dispose();
                Picture_Glass.ElementAt(i).Image = null;
            }
            Picture_Glass.Clear();

            if (DefectState != null)
            {
                for (int i = (DefectState.Length - 1); i >= 0; i--)
                {
                    Controls.Remove(DefectState[i]);
                    DefectState[i] = null;
                }
                DefectState = null;
            }

            if (ImageNameLB != null)
            {
                for (int i = (ImageNameLB.Length - 1); i >= 0; i--)
                {
                    Controls.Remove(ImageNameLB[i]);
                    ImageNameLB[i] = null;
                }
                ImageNameLB = null;
            }
            if (ImageNameEQ != null)
            {
                for (int i = (ImageNameEQ.Length - 1); i >= 0; i--)
                {
                    Controls.Remove(ImageNameEQ[i]);
                    ImageNameEQ[i] = null;
                }
                ImageNameEQ = null;
            }
            for (int i = 0; i < (cols * rows); i++)
            {
                temp_PB = new PictureBox();
                temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.Location = new Point(ImageRangeInfo.ElementAt(i).left, ImageRangeInfo.ElementAt(i).top);
                temp_PB.Size = new Size(ImageRangeInfo.ElementAt(i).width, ImageRangeInfo.ElementAt(i).height);
                temp_PB.SizeMode = PictureBoxSizeMode.StretchImage;

                PictureData.Add(temp_PB);
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                temp_PB = new PictureBox();
                temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.MouseDown += new MouseEventHandler(PictureBox_MouseDown);
                temp_PB.MouseMove += new MouseEventHandler(PictureBox_MouseMove);
                temp_PB.MouseUp += new MouseEventHandler(PictureBox_MouseUp);


                temp_PB.Location = new Point(0, 0);
                temp_PB.Size = new Size(ImageRangeInfo.ElementAt(i).width, ImageRangeInfo.ElementAt(i).height);
                temp_PB.SizeMode = PictureBoxSizeMode.StretchImage;
                temp_PB.Image = new Bitmap(width, height);

                Picture_Glass.Add(temp_PB);

                Picture_Glass.ElementAt(i).BackColor = Color.Transparent;
                Picture_Glass.ElementAt(i).Parent = PictureData.ElementAt(i);

            }

            DefectState = new Label[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {
                temp_LB = new Label();
                temp_LB.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, 3);
                temp_LB.Parent = Picture_Glass.ElementAt(i);
                DefectState[i] = temp_LB;
            }

            ImageNameLB = new Label[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {


                temp_LB = new Label();
                temp_LB.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, height - 15);
                temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameLB[i] = temp_LB;
            }


            ImageNameEQ = new Label[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {
                temp_LB = new Label();
                temp_LB.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, height - 15);
                temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameEQ[i] = temp_LB;
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                this.splitContainer1.Panel2.Controls.Add(PictureData.ElementAt(i));

            }

            Last_Picture_Selected_Index = -1;
            Current_PageNum = (pre_cols * pre_rows * (Current_PageNum - 1)) / (cols * rows) + 1;
            Total_PageNum = ((dicInfo_Filter_Del.Count - 1) / (cols * rows)) + 1;

        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            src_Mouse_XY.X = e.X;
            src_Mouse_XY.Y = e.Y;

            if (Draged_PB != null)
            {
                src_Mouse_XY.X += Draged_PB.Location.X;
                src_Mouse_XY.Y += Draged_PB.Location.Y;
            }

        }

        private void splitContainer1_Panel2_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            int tmp_XY;

            dst_Mouse_XY.X = e.X;
            dst_Mouse_XY.Y = e.Y;

            if (Draged_PB != null)
            {
                dst_Mouse_XY.X += Draged_PB.Location.X;
                dst_Mouse_XY.Y += Draged_PB.Location.Y;
            }

            if (src_Mouse_XY.X > dst_Mouse_XY.X)
            {
                tmp_XY = src_Mouse_XY.X;
                src_Mouse_XY.X = dst_Mouse_XY.X;
                dst_Mouse_XY.X = tmp_XY;
            }
            if (src_Mouse_XY.Y > dst_Mouse_XY.Y)
            {
                tmp_XY = src_Mouse_XY.Y;
                src_Mouse_XY.Y = dst_Mouse_XY.Y;
                dst_Mouse_XY.Y = tmp_XY;
            }

            Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);
            change_Glass();


            src_Mouse_XY.X = -1;
            src_Mouse_XY.Y = -1;
            dst_Mouse_XY.X = -1;
            dst_Mouse_XY.Y = -1;
            this.Focus();
        }
        private void Find_Contain_PB(Point Src, Point Dst)
        {
            List<string> Change_Data = new List<string>();
            Rectangle PB_Area, Drag_Area;
            int index = ((Current_PageNum - 1) * (cols * rows));
            string result = "";
            Change_state_List.Clear();

            if (Selected_Picture_Index.Count > 0)
                Selected_Picture_Index.Clear();

            for (int i = 0; i < PictureData.Count; i++)
            {
                PB_Area = new Rectangle(PictureData.ElementAt(i).Left, PictureData.ElementAt(i).Top, PictureData.ElementAt(i).Width, PictureData.ElementAt(i).Height);
                Drag_Area = new Rectangle(Src.X, Src.Y, Dst.X - Src.X, Dst.Y - Src.Y);

                if (PB_Area.IntersectsWith(Drag_Area))
                {
                    if (dicInfo_Filter_Del.Count > (index + i))
                    {
                        Selected_Picture_Index.Add(index + i);
                        Select_Pic.Add(dicInfo_Filter_Del.Keys.ElementAt(index + i));
                    }
                }


            }
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                Last_Picture_Selected_Index = Selected_Picture_Index.ElementAt(i);

                if (dicInfo_Filter_Del.Count <= Last_Picture_Selected_Index)
                {
                    //Last_Picture_Selected_Index = -1;
                    return;
                }

                if (dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(Last_Picture_Selected_Index)].DeleteCheck.Equals("삭제대기"))
                    result = "0";
                else
                {
                    result = "삭제대기";
                    for (int p = 0; p < Select_Pic.Count; p++)
                    {
                        if (Select_Pic[p].Equals(dicInfo_Filter_Del.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic.RemoveAt(p);
                            p--;
                        }
                    }

                }


                dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(Last_Picture_Selected_Index)].DeleteCheck = result;
                Change_state_List.Add(dicInfo_Filter_Del.Keys.ElementAt(Last_Picture_Selected_Index));

            }



        }

        private void DeleteWaiting_KeyDown(object sender, KeyEventArgs e)
        {
            //Del_img_list.Select();
            this.KeyPreview = true;
            if (e.Control)
            {
                if (S_Page_TB.Text == "" || int.Parse(S_Page_TB.Text) <= 1)
                {
                    MessageBox.Show("첫 페이지 입니다.");
                }
                else
                {
                    Last_Picture_Selected_Index = -1;
                    Current_PageNum = int.Parse(this.S_Page_TB.Text) - 1;
                    S_Page_TB.Text = Current_PageNum.ToString();
                    Set_PictureBox();
                    Set_Image();


                }
            }

            else if (e.Alt)
            {
                e.Handled = true;
                if (S_Page_TB.Text == "" || int.Parse(S_Page_TB.Text) >= int.Parse(E_Page_TB.Text))
                {
                    MessageBox.Show("마지막 페이지 입니다.");
                }
                else
                {
                    Last_Picture_Selected_Index = -1;
                    Current_PageNum = int.Parse(this.S_Page_TB.Text) + 1;
                    S_Page_TB.Text = Current_PageNum.ToString();
                    Set_PictureBox();
                    Set_Image();


                }
            }
            else if (e.Shift && e.KeyCode == Keys.A)
            {
                
                try
                {
                    waitform.Show();
             
                        foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter_Del)
                        {
                            pair.Value.DeleteCheck = "0";
                        }
                     
                        Select_Pic = dicInfo_Filter_Del.Keys.ToList();
                    Set_PictureBox();
                    Set_Image();



                    waitform.Close();
                }
                catch { }

            }
            else if (e.KeyCode == Keys.A)
            {

                Select_Pic.Clear();

                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();


                    for (int i = 0; i < (cols * rows); i++)
                    {
                        if ((index + i) >= dicInfo_Filter_Del.Count)
                            break;
                        Selected_Picture_Index.Add(index + i);
                    }

                    for (int p = 0; p < Selected_Picture_Index.Count; p++)
                    {
                        dicInfo_Filter_Del[dicInfo_Filter_Del.ElementAt(Selected_Picture_Index[p]).Key].DeleteCheck = "0";
                        Select_Pic.Add(dicInfo_Filter_Del.ElementAt(Selected_Picture_Index[p]).Key);
                        Change_state_List.Add(dicInfo_Filter_Del.ElementAt(Selected_Picture_Index[p]).Key);
                    }
                Set_PictureBox();
                Set_Image();




            }

            else if (e.Shift && e.KeyCode == Keys.G)
            {
                Select_Pic.Clear();

                foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter_Del)
                {
                    pair.Value.DeleteCheck = "삭제대기";
                }


                Set_PictureBox();
                Set_Image();


            }

            else if (e.KeyCode == Keys.G)
            {
                Select_Pic.Clear();

                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();

                    for (int i = 0; i < (cols * rows); i++)
                    {
                        if ((index + i) >= dicInfo_Filter_Del.Count)
                            break;
                        Selected_Picture_Index.Add(index + i);
                    }

                    for (int p = 0; p < Selected_Picture_Index.Count; p++)
                    {
                        dicInfo_Filter_Del[dicInfo_Filter_Del.ElementAt(Selected_Picture_Index[p]).Key].DeleteCheck = "삭제대기";

                        Change_state_List.Add(dicInfo_Filter_Del.ElementAt(Selected_Picture_Index[p]).Key);
                    }

                Set_PictureBox();
                Set_Image();


            }
            //this.splitContainer1.Panel2.Focus();
        }

        private void Set_Image()
        {
            Bitmap tmp_Img = null;
            List<string> compare_zip = new List<string>();
            Dictionary<string, ImageInfo> Compare_Dicinfo = new Dictionary<string, ImageInfo>();
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;

            if (Frame_Filter_check == 1)
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(framesort_dic);
            }
            else if (Camera_Filter_check == 1)
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(framesort_dic);
            }
            else if (EQ_Filter_check == 1)
            {

            }
            else
            {
                dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(Main_Dic);
            }
            //Sorted_dic = dicInfo_Filter_Del.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //dicInfo_Filter_Del = Sorted_dic;

            if (dicInfo_Filter_Del.Count <= 0)
            {
                for (int i = 0; i < PictureData.Count; i++)
                {
                    if (PictureData.ElementAt(i).Image != null)
                    {
                        PictureData.ElementAt(i).Image.Dispose();
                        PictureData.ElementAt(i).Image = null;
                    }
                }
            }

            if (dicInfo_Filter_Del.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += dicInfo_Filter_Del.Count - ((cols * rows) * Current_PageNum);
            int idex = 0;

            zip = ZipFile.Open(ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
            for (int x = 0; x < zip.Entries.Count; x++)
            {

                compare_zip.Add(zip.Entries[x].Name.ToString());
            }

            for(int i = S_ImageIndex; i < S_ImageIndex+EachPage_ImageNum; i++)
            {
                Compare_Dicinfo.Add(dicInfo_Filter_Del.Keys.ToList()[i], dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ToList()[i]]);

            }

            
            
            if (ZipFilePath != "")
            {
                foreach(string img_names in Compare_Dicinfo.Keys.ToList())
                {
                    if(S_ImageIndex > 0)
                    {

                    }
                    
                    MemoryStream subEntryMS = new MemoryStream();
                    if(compare_zip.FindIndex(x => x.Equals(Compare_Dicinfo[img_names].Imagename.Substring(1, 5) + ".zip")) == -1)
                    {
                        zip.Entries[compare_zip.FindIndex(x => x.Equals(Compare_Dicinfo[img_names].Imagename.Substring(1, 5) + ".Zip"))].Open().CopyTo(subEntryMS);
                    }
                    else
                    {
                        zip.Entries[compare_zip.FindIndex(x => x.Equals(Compare_Dicinfo[img_names].Imagename.Substring(1, 5) + ".zip"))].Open().CopyTo(subEntryMS);

                    }
                    

                    ZipArchive subZip = new ZipArchive(subEntryMS);

                    var sub =
                                    from ent in subZip.Entries
                                    orderby ent.Name
                                    select ent;
                    //if (compare_zip.FindIndex(x => x.Equals(dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename.Substring(1, 5) + ".zip")) != -1)
                    //{
                    //       idex = zip.Entries.ToList().FindIndex(x => x.Equals(dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename.Substring(1, 5) + ".zip"));
                    //}
                    foreach (ZipArchiveEntry subEntry in sub)
                    {
                        if (Current_Index >= EachPage_ImageNum)
                            break;
                        if (Current_Index >= dicInfo_Filter_Del.Count)
                            break;
                        if (S_ImageIndex + Current_Index >= dicInfo_Filter_Del.Count)
                            break;

                        if (subEntry.Name.Equals(Compare_Dicinfo[img_names].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                        {
                            tmp_Img = new Bitmap(subEntry.Open());

                            //방향
                            tmp_Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            PictureData.ElementAt(Current_Index).Image = tmp_Img;
                            //PictureData.ElementAt(Current_Index).Name = dicInfo_Filter_Del.Keys.ElementAt(S_ImageIndex + Current_Index);

                            Current_Index++;
                            break;
                        }
                    }
                }


                zip.Dispose();
             
                for (int i = EachPage_ImageNum; i < (cols * rows); i++)
                {
                    try
                    {
                       
                        //PictureData.ElementAt(i).Image.Dispose();
                        PictureData.ElementAt(i).Image = null;
                        ImageNameLB[i] = null;
                        ImageNameEQ[i] = null;
                    }
                    catch (Exception)
                    {
                    }
                }

            }
            change_Glass();

        }
        public void change_Glass()
        {

            Rectangle regSelection = new Rectangle();
            Graphics gPic;
            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                if (i >= dicInfo_Filter_Del.Count)
                    break;

                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

                if (index >= dicInfo_Filter_Del.Count)
                    break;

                string temp = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].DeleteCheck;

                Pen pen;

                if (temp.Equals("삭제대기") || temp.Equals("*"))
                {
                    Picture_Glass.ElementAt(i).Image.Dispose();
                    Picture_Glass.ElementAt(i).Image = new Bitmap(width, height);

                    pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                    regSelection.Location = new Point(0, 0);
                    regSelection.Size = new Size(Picture_Glass.ElementAt(i).Image.Width - 1, Picture_Glass.ElementAt(i).Image.Height - 1);

                }

                else
                {
                    Picture_Glass.ElementAt(i).Image.Dispose();
                    Picture_Glass.ElementAt(i).Image = new Bitmap(width, height);

                    pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
                    regSelection.Location = new Point(1, 1);
                    regSelection.Size = new Size(Picture_Glass.ElementAt(i).Image.Width - 3, Picture_Glass.ElementAt(i).Image.Height - 3);


                    //Select_Pic.Add(Picture_Glass.ElementAt(i).Parent.Name);

                }

                gPic = Graphics.FromImage(Picture_Glass.ElementAt(i).Image);
                gPic.DrawRectangle(pen, regSelection);
            }

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].ReviewDefectName;
                int length = 0;
                if (temp.Equals("삭제대기") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Yellow;
                    ImageNameEQ[i].BackColor = Color.Black;

               
                    if (Print_Image_Name.Checked && !Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Print_Image_Name.Checked && Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }


                    PictureData.ElementAt(i).Tag = Color.Yellow;

                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Red;
                    ImageNameEQ[i].BackColor = Color.Black;

                 

                    if (Print_Image_Name.Checked && !Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Print_Image_Name.Checked && Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }


                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }


            if (EachPage_ImageNum < 0)
                EachPage_ImageNum = 0;

            for (int i = EachPage_ImageNum; i < cols * rows; i++)
            {
                Picture_Glass.ElementAt(i).Image.Dispose();
                Picture_Glass.ElementAt(i).Image = new Bitmap(width, height);

                Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                regSelection.Location = new Point(0, 0);
                regSelection.Size = new Size(Picture_Glass.ElementAt(i).Image.Width - 1, Picture_Glass.ElementAt(i).Image.Height - 1);


                PictureData.ElementAt(i).Tag = Color.Black;
            }

            Cheked_State_DF();
        }
        private void Page_Filter_Dl(int page)
        {
            Last_Picture_Selected_Index = -1;
            Current_PageNum = page;
            S_Page_TB.Text = Current_PageNum.ToString();

            Set_PictureBox();
            Set_Image();
            this.Focus();
        }
        private void DeleteWaiting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           



        }

        private void Delete_wait_img_bt_Click(object sender, EventArgs e)
        {
            Get_Delete_IMG();
            
            
            if(Main.ViewType == "Code_200_SetView")
            {
                for (int i = 0; i < Select_Pic.Count; i++)
                {
                    if (dicInfo_Filter_Del.ContainsKey(Select_Pic[i]))
                    {
                        dicInfo_Filter_Del[Select_Pic[i]].ReviewDefectName = "양품";
                        dicInfo_Filter_Del[Select_Pic[i]].DeleteCheck = "0";
                        Main.Sdip_200_code_dicInfo[Select_Pic[i]] = dicInfo_Filter_Del[Select_Pic[i]];

                        //Main.selected_Pic.Add(Select_Pic[i]);
                        dicInfo_Filter_Del.Remove(Select_Pic[i]);
                        Main_Dic.Remove(Select_Pic[i]);
                    }


                }

            }
            else
            {
                for (int i = 0; i < Select_Pic.Count; i++)
                {
                    if (Main_Dic.ContainsKey(Select_Pic[i]))
                    {
                        Main_Dic[Select_Pic[i]].ReviewDefectName = "양품";
                        Main_Dic[Select_Pic[i]].DeleteCheck = "0";
                        Main.DicInfo[Select_Pic[i]] = Main_Dic[Select_Pic[i]];

                        //Main.selected_Pic.Add(Select_Pic[i]);
                        if(dicInfo_Filter_Del.ContainsKey(Select_Pic[i]))
                        {
                            dicInfo_Filter_Del.Remove(Select_Pic[i]);
                        }
                     
                        Main_Dic.Remove(Select_Pic[i]);
                        Main.Waiting_Del.Remove(Select_Pic[i]);
                    }


                }
            }
                        
           
            Set_View_Del();
            Set_EQ();
            Select_Pic.Clear();
            Main.selected_Pic = dicInfo_Delete_Sel.Keys.ToList();


            
            Main.Dl_Wait_Del_Print_List();
            Main.Return_Img_Print();
            dicInfo_Delete_Sel.Clear();
        }

        private void Delete_Img_In_ZIp_Click(object sender, EventArgs e)
        {
            try
            {
                Waiting_Del_DLView = Main.Waiting_Del;
                if (Waiting_Del_DLView.Count > 0)
                {
                    if (MessageBox.Show("" + Waiting_Del_DLView.Count + "개의 이미지를 삭제하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        waitform.Show();
                        Main.Delete_ZipImg();
                        Dispose(true);
                    }
                    else
                    {

                        return;
                    }


                }
                waitform.Close();
                Main.mAPTXTUpdateToolStripMenuItem1_Click(null, null);
            }
            catch(Exception ex)
            {
                waitform.Close();
                MessageBox.Show(ex.ToString());
            }
           
        }
        public void Cheked_State_DF()
        {
            int length = 0;
            Sorted_dic = dicInfo_Filter_Del.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            dicInfo_Filter_Del = Sorted_dic;

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Yellow;
                    ImageNameEQ[i].BackColor = Color.Black;

            

                    if (Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {

                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }


                    PictureData.ElementAt(i).Tag = Color.Yellow;
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Red;
                    ImageNameEQ[i].BackColor = Color.Black;

                   

                    if (Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Print_Image_Name.Checked && Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter_Del.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
        }

        private void S_Page_TB_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                Page_Filter_Dl(int.Parse(S_Page_TB.Text));
            }
        }

        private void Print_Image_Name_CheckedChanged(object sender, EventArgs e)
        {
            Cheked_State_DF();
        }

        private void Frame_S_TB_KeyDown(object sender, KeyEventArgs e)
        {
            int Num;
            List<int> Frame_filter_List = new List<int>();
            
            if (e.KeyCode == Keys.Enter)
            {
                if (Frame_S_TB.Text == "")
                {
                    framesort_dic = new Dictionary<string, ImageInfo>(Main_Dic);
                    Set_View_Del();
                  
                    return;

                }
                else if (!int.TryParse(Frame_S_TB.Text, out Num))
                {
                    MessageBox.Show("입력을 숫자로 부탁드립니다.");
                    Frame_S_TB.Text = "";
                    return;
                }
                Set_Frame_Filter();

                if (Frame_List_Img.Contains(int.Parse(Frame_S_TB.Text)))
                {
                    Frame_Filter(int.Parse(Frame_S_TB.Text));
                }
                else
                {

                    MessageBox.Show("해당 프레임은 존재하지 않습니다.");
                    Frame_S_TB.Text = "";
                }

            }
            Frame_List_Img.Clear();
        }
        public void Frame_Filter(int Frame)
        {
            framesort_dic.Clear();
            foreach (string no in Main_Dic.Keys.ToList())
            {
                if (Main_Dic[no].FrameNo == Frame)
                {
                    framesort_dic.Add(no, Main_Dic[no]);
                }
                else
                {
                    //DicInfo_Filtered.Remove(no);
                }
            }
            Frame_Filter_check = 1;
            Set_View_Del();
            
        }
        public void Set_Frame_Filter()
        {
            Frame_List_Img.Clear();
            foreach (string pair in Main_Dic.Keys.ToList())
            {
                if (Frame_List_Img.Contains(Main_Dic[pair].FrameNo))
                {

                }
                else
                {
                    Frame_List_Img.Add(Main_Dic[pair].FrameNo);

                }
            }
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Frame_S_TB.Text = "";
            Camera_NO_Filter_TB.Text = "";
            Frame_Filter_check = 0;
            Camera_Filter_check = 0;
            Set_View_Del();
        }

        private void Camera_NO_Filter_TB_KeyDown(object sender, KeyEventArgs e)
        {
            int Num;

            framesort_dic.Clear();
            if (e.KeyCode == Keys.Enter)
            {
                string[] Split_String = null;
                Split_String = Camera_NO_Filter_TB.Text.Split(',');

                if (Camera_NO_Filter_TB.Text == "")
                {
                    
                    Set_View_Del();
                   
                    Camera_NO_Filter_TB.Text = "";
                    return;

                }
               


                if (int.Parse(Split_String[0]) > 0)
                {
                    Camera_Filter_check = 1;

                        foreach (string No in Main_Dic.Keys.ToList())
                        {
                            if (Main_Dic.ContainsKey(No))
                            {
                                if (Split_String.Contains(Main_Dic[No].CameraNo.ToString()))
                                {
                                framesort_dic.Add(No, Main_Dic[No]);
                                }

                            }
                        }

                        if (framesort_dic.Count == 0)
                        {
                            dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(Main_Dic);
                            MessageBox.Show("해당 카메라 이미지가 없습니다.");
                            Camera_NO_Filter_TB.Text = string.Empty;
                        }
                        Set_View_Del();
                       
                        framesort_dic.Clear();
                   

                    
                }
            
            }
        }

        private void Select_All_BTN_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
                Equipment_DF_CLB.SetItemCheckState(i, CheckState.Checked);

            Equipment_DF_CLB_SelectedValueChanged(null, null);
        }

        private void Equipment_DF_CLB_SelectedValueChanged(object sender, EventArgs e)
        {

            Selected_Equipment_DF_List.Clear();

            foreach (int index in Equipment_DF_CLB.CheckedIndices)
                Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0]);
        }

        private void Select_Empty_BTN_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
                Equipment_DF_CLB.SetItemCheckState(i, CheckState.Unchecked);

            Equipment_DF_CLB_SelectedValueChanged(null, null);
        }

        private void _filterAct_bt_Click(object sender, EventArgs e)
        {
            EQ_Filter_check = 1;
            Initial_Equipment_DF_FilterList();
            dicInfo_Filter_Del = new Dictionary<string, ImageInfo>(Eq_CB_dicInfo);
          
            PictureData.Clear();

            Set_View_Del();
            EQ_Filter_check = 0;
        }

        private void Initial_Equipment_DF_FilterList()
        {
            Eq_CB_dicInfo.Clear();

            for (int i = 0; i < Selected_Equipment_DF_List.Count; i++)
            {
                foreach (KeyValuePair<string, ImageInfo> pair in Main_Dic)
                {

                    if (pair.Value.EquipmentDefectName == Selected_Equipment_DF_List[i])
                    {
                        if (Eq_CB_dicInfo.ContainsKey(pair.Key) == false)
                            Eq_CB_dicInfo.Add(pair.Key, pair.Value);
                    }

                }

            }
           

        }
        private void Print_Image_EQ_CheckedChanged(object sender, EventArgs e)
        {
            Cheked_State_DF();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            this.Focus();
            MouseEventArgs MouseEvent = (MouseEventArgs)e;
            PictureBox PB = (PictureBox)sender;

            int index = ((Current_PageNum - 1) * (cols * rows));
            int EachPage_ImageNum = cols * rows;

            if (dicInfo_Filter_Del.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += dicInfo_Filter_Del.Count - ((cols * rows) * Current_PageNum);

            switch (MouseEvent.Button)
            {
                case MouseButtons.Left:
                    {
                        break;
                    }
                case MouseButtons.Right:
                    {
                        for (int i = 0; i < EachPage_ImageNum; i++)
                        {
                            //if (PB.Image == PictureData.ElementAt(i).Image)
                            if (PB.Image == Picture_Glass.ElementAt(i).Image)
                            {
                                Last_Picture_Selected_Index = index + i;
                                Selected_Picture_Index.Clear();
                                Selected_Picture_Index.Add(Last_Picture_Selected_Index);
                                break;
                            }
                        }
                        break;
                    }
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            PictureBox PB = (PictureBox)sender;

            for (int i = 0; i < Picture_Glass.Count; i++)
            {
                if (PB.Image == Picture_Glass.ElementAt(i).Image)
                {
                    Draged_PB = PictureData.ElementAt(i);
                    break;
                }
            }
            ImageViewer_PL_MouseDown(sender, e);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            this.Focus();
            splitContainer1_Panel2_MouseMove(sender, e);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.Focus();
            splitContainer1_Panel2_MouseUp(sender, e);
            Draged_PB = null;
        }
        private void ImageViewer_PL_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            src_Mouse_XY.X = e.X;
            src_Mouse_XY.Y = e.Y;

            if (Draged_PB != null)
            {
                src_Mouse_XY.X += Draged_PB.Location.X;
                src_Mouse_XY.Y += Draged_PB.Location.Y;
            }
        }
        public void Get_Delete_IMG()
        {
            Select_Pic.Clear();
            foreach (string name in dicInfo_Filter_Del.Keys.ToList())
            {
                if (dicInfo_Filter_Del[name].DeleteCheck == "0")
                {
                    Select_Pic.Add(name);
                }

            }
            for (int p = 0; p < Select_Pic.Count; p++)
            {
                if (Main_Dic.ContainsKey(Select_Pic[p]))
                {

                    dicInfo_Delete_Sel.Add(Select_Pic[p], Main_Dic[Select_Pic[p]]);

                    dicInfo_Delete_Sel[Select_Pic[p]].DeleteCheck = "삭제대기";
                }
            }
         
        }
    }


}

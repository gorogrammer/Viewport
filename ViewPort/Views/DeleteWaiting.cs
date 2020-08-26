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
    public partial class DeleteWaiting : Form
    {
        

        FormViewPort Main = new FormViewPort();
        

        List<PictureBox> PictureData = new List<PictureBox>();
        List<PictureBox> Picture_Glass = new List<PictureBox>();
        List<string> Wait_del_List = new List<string>();
        Label[] DefectState, ImageNameLB;
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        List<string> Select_Pic = new List<string>();
        Dictionary<string, ImageInfo> dicInfo_Filter_Del = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Delete_Sel = new Dictionary<string, ImageInfo>();
        int cols, rows, width, height;
        int Current_PageNum, Total_PageNum;
        Point src_Mouse_XY, dst_Mouse_XY;
        struct BoxRange { public int left, top, width, height; }
        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        int Last_Picture_Selected_Index;
        int EachPage_ImageNum;
        List<string> Print_Frame = new List<string>();
        ZipArchive zip = null;
        string zipFilePath = null;
        List<string> Change_state_List = new List<string>();

        public Dictionary<string, ImageInfo> Waiting_Img { get => dicInfo_Filter_Del; set => dicInfo_Filter_Del = value; }
        public string ZipFilePath { get => zipFilePath; set => zipFilePath = value; }

        public DeleteWaiting() 
        {
            InitializeComponent();
            
        }
        public void Set_View_Del()
        {
            splitContainer1.Panel2.Controls.Clear();
            PictureData.Clear();


            

            cols = 8;
            rows = 5;
            width = 120;
            height = 120;

            Current_PageNum = 1;

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter_Del.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();


            Set_PictureBox();
            Set_Image();
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

            if (ImageNameLB != null)
            {
                for (int i = (ImageNameLB.Length - 1); i >= 0; i--)
                {
                    Controls.Remove(ImageNameLB[i]);
                    ImageNameLB[i] = null;
                }
                ImageNameLB = null;
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
            Set_Image();


            src_Mouse_XY.X = -1;
            src_Mouse_XY.Y = -1;
            dst_Mouse_XY.X = -1;
            dst_Mouse_XY.Y = -1;
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
            if (e.Control)
            {
                if (Main.S_Page_TB.Text == "" || int.Parse(Main.S_Page_TB.Text) <= 1)
                {
                    MessageBox.Show("첫 페이지 입니다.");
                }
                else
                {
                    Last_Picture_Selected_Index = -1;
                    Current_PageNum = int.Parse(Main.S_Page_TB.Text) - 1;
                    Main.S_Page_TB.Text = Current_PageNum.ToString();
                    Set_Image();


                }
            }

            else if (e.Alt)
            {
                e.Handled = true;
                if (Main.S_Page_TB.Text == "" || int.Parse(Main.S_Page_TB.Text) >= int.Parse(Main.E_Page_TB.Text))
                {
                    MessageBox.Show("마지막 페이지 입니다.");
                }
                else
                {
                    Last_Picture_Selected_Index = -1;
                    Current_PageNum = int.Parse(Main.S_Page_TB.Text) + 1;
                    Main.S_Page_TB.Text = Current_PageNum.ToString();
                    Set_Image();


                }
            }

            else if (e.KeyCode == Keys.Delete)
            {
                Get_Delete_IMG();

                for (int i = 0; i < Select_Pic.Count; i++)
                {
                    if (dicInfo_Filter_Del.ContainsKey(Select_Pic[i]))
                    {
                        dicInfo_Filter_Del.Remove(Select_Pic[i]);
                    }

                }

                Main.Waiting_Del = dicInfo_Filter_Del;
                Select_Pic.Clear();
            }
        }

        private void Set_Image()
        {
            Bitmap tmp_Img = null;

            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;

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


            if (ZipFilePath != "")
            {
                zip = ZipFile.Open(ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
                string Open_ZipName;

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    Open_ZipName = entry.Name.Split('.')[0];
                    if (Open_ZipName[0].Equals('R'))
                        Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);

                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1)
                    {
                        MemoryStream subEntryMS = new MemoryStream();           // 2중 압축파일을 MemoryStream으로 읽는다.
                        entry.Open().CopyTo(subEntryMS);

                        ZipArchive subZip = new ZipArchive(subEntryMS);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.
                        foreach (ZipArchiveEntry subEntry in subZip.Entries)       // 2중 압축파일 내에 있는 파일을 탐색
                        {
                            if (Current_Index >= EachPage_ImageNum)
                                break;
                            if (Current_Index >= dicInfo_Filter_Del.Count)
                                break;
                            if (subEntry.Name.Equals(dicInfo_Filter_Del[dicInfo_Filter_Del.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename+".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                            {
                                tmp_Img = new Bitmap(subEntry.Open());

                                //방향

                                PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                PictureData.ElementAt(Current_Index).Name = dicInfo_Filter_Del.Keys.ElementAt(S_ImageIndex + Current_Index);

                                Current_Index++;
                            }

                            if (Current_Index >= EachPage_ImageNum)
                                break;
                           
                        }
                        subZip.Dispose();
                    }
                    if (Current_Index >= EachPage_ImageNum || Print_Frame.Count <= PF_index)
                        break;
                }
                zip.Dispose();

                for (int i = EachPage_ImageNum; i < (cols * rows); i++)
                {
                    try
                    {
                        PictureData.ElementAt(i).Image = null;
                    }
                    catch (Exception)
                    {
                    }
                }

            }

            Rectangle regSelection = new Rectangle();
            Graphics gPic;
            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                if (i >= dicInfo_Filter_Del.Count)
                    break;

                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

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
                    pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
                    regSelection.Location = new Point(1, 1);
                    regSelection.Size = new Size(Picture_Glass.ElementAt(i).Image.Width - 3, Picture_Glass.ElementAt(i).Image.Height - 3);


                    //Select_Pic.Add(Picture_Glass.ElementAt(i).Parent.Name);

                }

                gPic = Graphics.FromImage(Picture_Glass.ElementAt(i).Image);
                gPic.DrawRectangle(pen, regSelection);
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

                DefectState[i].Text = "";
                ImageNameLB[i].Text = "";
                PictureData.ElementAt(i).Tag = Color.Black;
            }
          
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
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
            splitContainer1_Panel2_MouseMove(sender, e);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            splitContainer1_Panel2_MouseUp(sender, e);
            Draged_PB = null;
        }
        private void ImageViewer_PL_MouseDown(object sender, MouseEventArgs e)
        {
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
            for (int p = 0; p < Select_Pic.Count; p++)
            {
                if (dicInfo_Filter_Del.ContainsKey(Select_Pic[p]))
                {

                    dicInfo_Delete_Sel.Add(Select_Pic[p], dicInfo_Filter_Del[Select_Pic[p]]);

                    dicInfo_Delete_Sel[Select_Pic[p]].DeleteCheck = "삭제대기";
                }
            }

        }
    }


}

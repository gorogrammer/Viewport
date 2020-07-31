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
    public partial class ImageViewer : UserControl
    {

        public FormViewPort Main;



        struct BoxRange { public int left, top, width, height; }

        List<PictureBox> PictureData = new List<PictureBox>();
        List<PictureBox> Picture_Glass = new List<PictureBox>();
        List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        List<string> Print_Frame = new List<string>();
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        Label[] DefectState, ImageNameLB;
        int Current_PageNum, Total_PageNum;
        int Last_Picture_Selected_Index;        // -3 : 리스트 전체 선택,  -2 : 다중 선택, -1 : 미선택, 그 외, 선택한 Image Index
        int EachPage_ImageNum;

        Point src_Mouse_XY, dst_Mouse_XY;
        //bool isMousePressed = false, isMouseDraged = false;

        string Input_Defect_Code;

        List<Tuple<string, int>> Defect_Code_Info = new List<Tuple<string, int>>();
        List<ImageListInfo> FilteredList = new List<ImageListInfo>();
        TextBox[] Defect_Code_Name = new TextBox[41];
        TextBox[] Defect_Code_NumName = new TextBox[41];
        bool CodeModifyMode = false;
        int cols, rows, width, height;

        ZipArchive zip = null;

        private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
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
        }

        public ImageViewer(FormViewPort mainForm)
        {
            InitializeComponent();
        }

        public ImageViewer()
        {
            InitializeComponent();
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {

        }

        public void Set_View()
        {
            Main.GetFilterList(FilteredList);

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;


            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((FilteredList.Count - 1) / (cols * rows)) + 1;
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

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

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
                Controls.Remove(PictureData.ElementAt(i));
                PictureData.ElementAt(i).Dispose();
                PictureData.ElementAt(i).Image = null;
            }
            PictureData.Clear();


            for (int i = (Picture_Glass.Count - 1); i >= 0; i--)
            {
                Controls.Remove(Picture_Glass.ElementAt(i));
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

            for (int i = 0; i < (cols * rows); i++)
            {
                temp_PB = new PictureBox();
                //temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.Location = new Point(ImageRangeInfo.ElementAt(i).left, ImageRangeInfo.ElementAt(i).top);
                temp_PB.Size = new Size(ImageRangeInfo.ElementAt(i).width, ImageRangeInfo.ElementAt(i).height);
                temp_PB.SizeMode = PictureBoxSizeMode.StretchImage;

                PictureData.Add(temp_PB);
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                temp_PB = new PictureBox();
                //temp_PB.Click += new EventHandler(PictureBox_Click);
                //temp_PB.MouseDown += new MouseEventHandler(PictureBox_MouseDown);
                //temp_PB.MouseMove += new MouseEventHandler(PictureBox_MouseMove);
                //temp_PB.MouseUp += new MouseEventHandler(PictureBox_MouseUp);

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
                //double tmp_Size = 9;

                //tmp_Size *= width / (double)200;

                temp_LB = new Label();
                temp_LB.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, height - 20);
                temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameLB[i] = temp_LB;
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                this.Controls.Add(PictureData.ElementAt(i));
                //this.Controls.Add(Picture_Glass.ElementAt(i));
            }

            Last_Picture_Selected_Index = -1;
            Current_PageNum = (pre_cols * pre_rows * (Current_PageNum - 1)) / (cols * rows) + 1;
            Total_PageNum = ((FilteredList.Count - 1) / (cols * rows)) + 1;

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            //Changed_Setting = false;
        }

        private void Set_Image()
        {
            Bitmap tmp_Img = null;
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;

            if (FilteredList.Count <= 0)
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

            if (FilteredList.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += FilteredList.Count - ((cols * rows) * Current_PageNum);

            if (Print_Frame.Count > 0)
            {
                Print_Frame.Clear();
                Print_Frame = new List<string>();
            }

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                Current_ImageFrame = FilteredList.ElementAt(S_ImageIndex + i).Imagename.Substring(1, 5);

                if (!Print_Frame.Contains(Current_ImageFrame))
                    Print_Frame.Add(Current_ImageFrame);
            }
            Print_Frame.Sort();

            if (Main.GetLoad_State() == 0)
            {
                for (int i = 0; i < EachPage_ImageNum; i++)
                {
                    tmp_Img = new Bitmap(Path.Combine(FilteredList.ElementAt(S_ImageIndex + i).FilePath, FilteredList.ElementAt(S_ImageIndex + i).Imagename + ".jpg"));
                    ///방향 전환

                    PictureData.ElementAt(Current_Index + i).Image = tmp_Img;
                }
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
            else
            {
                if (Main.GetZipFilePath() != "")
                {
                    zip = ZipFile.Open(Main.GetZipFilePath(), ZipArchiveMode.Read);       // Zip파일(Lot) Load
                    string Open_ZipName;

                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        Open_ZipName = entry.Name.Split('.')[0];
                        if (Open_ZipName[0].Equals('R'))
                            Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);

                        if (Print_Frame.Count > PF_index && Open_ZipName.Equals(Print_Frame.ElementAt(PF_index)) && entry.Name.ToUpper().IndexOf(".ZIP") != -1)
                        {
                            MemoryStream subEntryMS = new MemoryStream();           // 2중 압축파일을 MemoryStream으로 읽는다.
                            entry.Open().CopyTo(subEntryMS);

                            ZipArchive subZip = new ZipArchive(subEntryMS);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.
                            foreach (ZipArchiveEntry subEntry in subZip.Entries)       // 2중 압축파일 내에 있는 파일을 탐색
                            {
                                if (Current_Index >= EachPage_ImageNum)
                                    break;
                                if (subEntry.Name.Equals(FilteredList.ElementAt(S_ImageIndex + Current_Index).Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                                {
                                    tmp_Img = new Bitmap(subEntry.Open());

                                    //방향

                                    PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                    Current_Index++;
                                }

                                if (Current_Index >= EachPage_ImageNum)
                                    break;
                                if (!FilteredList.ElementAt(S_ImageIndex + Current_Index).Imagename.Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
                                {
                                    PF_index++;
                                    break;
                                }
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
            }

            Rectangle regSelection = new Rectangle();
            Graphics gPic;

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = FilteredList.ElementAt(index).ReviewDefectName;

                Pen pen;

                if (temp.Equals("양품") || temp.Equals("*"))
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

    }
}

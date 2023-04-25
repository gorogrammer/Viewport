using ViewPort.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using ViewPort.Functions;
using System.IO.Compression;
using System.IO;
using System.Diagnostics;

namespace ViewPort.Views
{
    public partial class ImageViewer : UserControl
    {
        public void Normal_Data()
        {
            if (Main.XY_BT.Checked)
            {



                Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                int maxY = DicInfo_Filtered.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;

                foreach (KeyValuePair<string, ImageInfo> pair in DicInfo_Filtered)

                {

                    int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                    int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                    int SortedXY = y * (maxY * 10) + x;

                    pair.Value.SortedXY = SortedXY;


                }
                var keyValues = DicInfo_Filtered.OrderBy(x => x.Value.SortedXY);
                foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                {

                    SortXY_DIC_Load.Add(pair.Key, pair.Value);


                }
                Main.Eng_dicinfo = SortXY_DIC_Load;
            }
            else if (Main.Frame_BT.Checked)
            {
                Dictionary<string, ImageInfo> SortFrame_DIC_Load = new Dictionary<string, ImageInfo>();

                SortFrame_DIC_Load = DicInfo_Filtered.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                Main.Eng_dicinfo = SortFrame_DIC_Load;

            }
            Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();
            Main.Eng_Print_List();
            //Main.update_Equipment_DF_CLB(DicInfo_Filtered.Keys.ToList());
        }
        public void Set_EngData(string StateNum, string Cols, string Rows, string Width, string Height)
        {
            Main.Cols_TB.Text = Cols;
            Main.Rows_TB.Text = Rows;
            Main.Width_TB.Text = Width;
            Main.Height_TB.Text = Height;
            Dictionary<string, ImageInfo> Data = new Dictionary<string, ImageInfo>();
            List<int> FramNoData = new List<int>();

            if (Main.Eng_dicinfo.Count > 0)
            {
                Main.Eng_dicinfo.Clear();
            }
            if (!StateNum.Equals(EQ_STR.DEFAULT))
            {
                normalCheck = false;
                foreach (KeyValuePair<string, ImageInfo> valuePair in DicInfo_Filtered)
                {
                    if (valuePair.Value.EquipmentDefectName.Contains(StateNum) && !Main.Eng_dicinfo.ContainsKey(valuePair.Key))
                        Main.Eng_dicinfo.Add(valuePair.Key, valuePair.Value);


                }

                if(Main.Eng_dicinfo.Count == 0)
                {
                    return;
                }

                if (Main.Frame_BT.Checked)
                {
                  
                    Data = Main.Eng_dicinfo.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    Main.Eng_dicinfo = Data;
                }
                else
                {
                    Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                    int maxY = Main.Eng_dicinfo.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;
                    foreach (KeyValuePair<string, ImageInfo> pair in Main.Eng_dicinfo)
                    {

                        int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                        int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                        int SortedXY = y * (maxY * 10) + x;

                        pair.Value.SortedXY = SortedXY;


                    }
                    var keyValues = Main.Eng_dicinfo.OrderBy(x => x.Value.SortedXY);
                    foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                    {

                        SortXY_DIC_Load.Add(pair.Key, pair.Value);


                    }
                    Main.Eng_dicinfo = SortXY_DIC_Load;
                }
                

                Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
                Main.E_Page_TB.Text = Total_PageNum.ToString();
                Set_PictureBox();
                Set_Image_Eng();
                Main.Eng_Print_List();
                Main.Eng_EQ_Setting();
                // Main.update_Equipment_DF_CLB(Main.Eng_dicinfo.Keys.ToList());
            }
            else
            {
                normalCheck = true;
                // Main.Eng_dicinfo = DicInfo_Filtered;
                if (Main.Frame_BT.Checked)
                {

                    Dictionary<string, ImageInfo> SortFrame_DIC_Load = new Dictionary<string, ImageInfo>();
                    SortFrame_DIC_Load = Main.DicInfo.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    DicInfo_Filtered = SortFrame_DIC_Load;

                }
                else
                {
                    Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>(Main.DicInfo);
                    int maxY = SortXY_DIC_Load.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;
                    foreach (KeyValuePair<string, ImageInfo> pair in SortXY_DIC_Load)
                    {

                        int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                        int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                        int SortedXY = y * (maxY * 10) + x;

                        pair.Value.SortedXY = SortedXY;


                    }
                    var keyValues = SortXY_DIC_Load.OrderBy(x => x.Value.SortedXY);
                    foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                    {

                        Main.Eng_dicinfo.Add(pair.Key, pair.Value);


                    }
                   
                }

               
                
                Main.Eng_dicinfo = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
                Main.E_Page_TB.Text = Total_PageNum.ToString();
                Set_PictureBox();
                Set_Image_Eng();
                Main.Eng_EQ_Setting();
                Main.Eng_Print_List();
                //Main.update_Equipment_DF_CLB(Main.Eng_dicinfo.Keys.ToList());
            }

        }
        public void Set_MultiCheck_EngData(string StateNum, string Cols, string Rows, string Width, string Height)
        {

            Dictionary<string, ImageInfo> Data = new Dictionary<string, ImageInfo>();
            List<int> FramNoData = new List<int>();
            Main.Cols_TB.Text = Cols;
            Main.Rows_TB.Text = Rows;
            Main.Width_TB.Text = Width;
            Main.Height_TB.Text = Height;


            foreach (KeyValuePair<string, ImageInfo> valuePair in DicInfo_Filtered)
            {
                if (valuePair.Value.EquipmentDefectName.Contains(StateNum) && !Main.Eng_dicinfo.ContainsKey(valuePair.Key))
                    Main.Eng_dicinfo.Add(valuePair.Key, valuePair.Value);


            }
            if (Main.Frame_BT.Checked)
            {

                Data = Main.Eng_dicinfo.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                Main.Eng_dicinfo = Data;
            }
            else
            {
                Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                int maxY = Main.Eng_dicinfo.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;
                foreach (KeyValuePair<string, ImageInfo> pair in Main.Eng_dicinfo)
                {

                    int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                    int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                    int SortedXY = y * (maxY * 10) + x;

                    pair.Value.SortedXY = SortedXY;


                }
                var keyValues = Main.Eng_dicinfo.OrderBy(x => x.Value.SortedXY);
                foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                {

                    SortXY_DIC_Load.Add(pair.Key, pair.Value);


                }
                Main.Eng_dicinfo = SortXY_DIC_Load;
            }

            Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();
            Set_PictureBox();
            Set_Image_Eng();
            Main.Eng_Print_List();
            Main.Eng_EQ_Setting();
        }
        public void Del_EngData(string StateNum, int check)
        {
            Dictionary<string, ImageInfo> Data, RemoveList = new Dictionary<string, ImageInfo>();
            List<string> FramNoData = new List<string>();
            //Manager = manager;
            RemoveList = Main.Eng_dicinfo;
            int Count = Main.Eng_dicinfo.Count;
            foreach (KeyValuePair<string, ImageInfo> valuePair in Main.Eng_dicinfo)
            {
                if (valuePair.Value.EquipmentDefectName.Contains(StateNum) && !FramNoData.Contains(valuePair.Key))
                    FramNoData.Add(valuePair.Key);


            }
            foreach (string number in FramNoData)
            {
                Main.Eng_dicinfo.Remove(number);
            }

            if (Main.Eng_dicinfo.Count > 0)
            {
                if (Main.Frame_BT.Checked)
                {

                    Data = Main.Eng_dicinfo.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    Main.Eng_dicinfo = Data;
                }
                else
                {
                    Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                    int maxY = Main.Eng_dicinfo.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;
                    foreach (KeyValuePair<string, ImageInfo> pair in Main.Eng_dicinfo)
                    {

                        int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                        int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                        int SortedXY = y * (maxY * 10) + x;

                        pair.Value.SortedXY = SortedXY;


                    }
                    var keyValues = Main.Eng_dicinfo.OrderBy(x => x.Value.SortedXY);
                    foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                    {

                        SortXY_DIC_Load.Add(pair.Key, pair.Value);


                    }
                    Main.Eng_dicinfo = SortXY_DIC_Load;
                }
            }
            if (check == 2)
            {
                string SingData = Manager.Del_Single_Check();
                string[] Single = SingData.Split(',');

                Set_EngData(Single[0], Single[1], Single[2], Single[3], Single[4]);

            }
            else
            {
                Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
                Main.E_Page_TB.Text = Total_PageNum.ToString();

                Set_PictureBox();
                Set_Image_Eng();
                Main.Print_List();
                Main.Eng_EQ_Setting();

            }
        }
        public void Set_Image_Eng()
        {
            //Stopwatch st = new Stopwatch();
            //st.Start();

            Total_Frame_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
            Bitmap tmp_Img = null;
            int Size = (cols * rows);
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = (cols * rows);

            //Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //DicInfo_Filtered = Sorted_dic;

            var list = Main.Eng_dicinfo.Keys.ToList();
            list.OrderBy(x => x);

            if (imglist.Count > 0)
                imglist.Clear();



            //if (dicInfo_Filter.Count <= 0)
            //{
            //    for (int i = 0; i < PictureData.Count; i++)
            //    {
            //        if (PictureData.ElementAt(i).Image != null)
            //        {
            //            PictureData.ElementAt(i).Image.Dispose();
            //            PictureData.ElementAt(i).Image = null;
            //        }
            //    }
            //}

            if (list.Count <= 0)
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

            //if (dicInfo_Filter.Count - ((cols * rows) * Current_PageNum) < 0)
            //    EachPage_ImageNum += dicInfo_Filter.Count - ((cols * rows) * Current_PageNum);

            if (list.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += list.Count - ((cols * rows) * Current_PageNum);

            if (Print_Frame.Count > 0)
            {
                Print_Frame.Clear();
                Print_Frame = new List<string>();
            }

            //for (int i = 0; i < EachPage_ImageNum; i++)
            //{
            //    Current_ImageFrame = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + i).Substring(1, 5);

            //    if (!Print_Frame.Contains(Current_ImageFrame))
            //        Print_Frame.Add(Current_ImageFrame);
            //}
            for (int i = 0; i < EachPage_ImageNum; i++)
            {

                Current_ImageFrame = list[S_ImageIndex + i].Substring(1, 5);


                if (!Print_Frame.Contains(Current_ImageFrame))
                    Print_Frame.Add(Current_ImageFrame);
            }

            Print_Frame.Sort();


            if (Main.ZipFilePath != null)
            {
                zip = ZipFile.Open(Main.ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
                string Open_ZipName;
                ZipArchiveEntry sortzip;
                if (Main.Frame_BT.Checked)
                {
                    if (Print_Frame.Count > 0)
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                        {
                            Open_ZipName = entry.Name.Split('.')[0];
                            if (Open_ZipName[0].Equals('R'))
                                Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);

                            if (Print_Frame.Count >= PF_index && Open_ZipName.Equals(Print_Frame.ElementAt(PF_index)) && entry.Name.ToUpper().IndexOf(".ZIP") != -1)
                            {
                                MemoryStream subEntryMS = new MemoryStream();           // 2중 압축파일을 MemoryStream으로 읽는다.
                                entry.Open().CopyTo(subEntryMS);

                                ZipArchive subZip = new ZipArchive(subEntryMS);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.


                                var sub =
                                                    from ent in subZip.Entries
                                                    orderby ent.Name
                                                    select ent;


                                foreach (ZipArchiveEntry subEntry in sub)       // 2중 압축파일 내에 있는 파일을 탐색
                                {
                                    //if (dicInfo_Filter.ContainsKey(subEntry.Name.Substring(0, 12)))
                                    //{

                                    //}


                                    if (Current_Index >= EachPage_ImageNum)
                                        break;
                                    //if (subEntry.Name.Equals(dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                                    if (subEntry.Name.Contains(list[S_ImageIndex + Current_Index]))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                                    {
                                        tmp_Img = new Bitmap(subEntry.Open());

                                        switch (Main.Rotate_CLB.SelectedIndex)
                                        {
                                            case 0:
                                                {
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    tmp_Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    tmp_Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                                    break;
                                                }
                                            case 3:
                                                {
                                                    tmp_Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                                    break;
                                                }
                                            default:
                                                {
                                                    MessageBox.Show("이미지 회전 오류");
                                                    return;
                                                }
                                        }
                                        // PictureData.ElementAt(Current_Index).LoadAsync();

                                        PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                        //PictureData.ElementAt(Current_Index).Update();
                                        //PictureData.ElementAt(Current_Index).Name = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);
                                        PictureData.ElementAt(Current_Index).Name = list[S_ImageIndex + Current_Index];

                                        Current_Index++;
                                    }

                                    if (Current_Index >= EachPage_ImageNum)
                                        break;
                                    //if (!dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index).Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
                                    //{
                                    //    PF_index++;
                                    //    break;
                                    //}
                                    if (!list[S_ImageIndex + Current_Index].Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
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
                    }
                }
                else
                {

                    for (int i = 0; i < EachPage_ImageNum; i++)
                    {

                        Open_ZipName = list[S_ImageIndex + i].Substring(1, 5);
                        ZipArchiveEntry entry = zip.GetEntry(Open_ZipName + ".zip");
                        if (Open_ZipName[0].Equals('R'))
                            Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);


                        MemoryStream subEntryMS = new MemoryStream();           // 2중 압축파일을 MemoryStream으로 읽는다.
                        entry.Open().CopyTo(subEntryMS);

                        ZipArchive subZip = new ZipArchive(subEntryMS);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.


                        var sub =
                                            from ent in subZip.Entries
                                            orderby ent.Name
                                            select ent;


                        foreach (ZipArchiveEntry subEntry in sub)       // 2중 압축파일 내에 있는 파일을 탐색
                        {
                            //if (dicInfo_Filter.ContainsKey(subEntry.Name.Substring(0, 12)))
                            //{

                            //}


                            if (Current_Index >= EachPage_ImageNum)
                                break;
                            //if (subEntry.Name.Equals(dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                            if (subEntry.Name.Contains(list[S_ImageIndex + Current_Index]))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                            {
                                tmp_Img = new Bitmap(subEntry.Open());

                                switch (Main.Rotate_CLB.SelectedIndex)
                                {
                                    case 0:
                                        {
                                            break;
                                        }
                                    case 1:
                                        {
                                            tmp_Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                            break;
                                        }
                                    case 2:
                                        {
                                            tmp_Img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                            break;
                                        }
                                    case 3:
                                        {
                                            tmp_Img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                            break;
                                        }
                                    default:
                                        {
                                            MessageBox.Show("이미지 회전 오류");
                                            return;
                                        }
                                }

                                PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                //PictureData.ElementAt(Current_Index).Update();
                                //PictureData.ElementAt(Current_Index).Name = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);
                                PictureData.ElementAt(Current_Index).Name = list[S_ImageIndex + Current_Index];

                                Current_Index++;
                            }

                            if (Current_Index >= EachPage_ImageNum)
                                break;
                            //if (!dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index).Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
                            //{
                            //    PF_index++;
                            //    break;
                            //}

                        }

                        subZip.Dispose();
                        if (Current_Index >= EachPage_ImageNum || Print_Frame.Count <= PF_index)
                            break;
                    }

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


            //PictureData.ElementAt(Current_Index).Update();
            EngMode_change_Glass();
            this.Focus();
            Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Eng_dicinfo.Count);
            // st.Stop();

            // MessageBox.Show(st.ElapsedMilliseconds.ToString());
        }
        public void EngMode_change_Glass()
        {
            Rectangle regSelection = new Rectangle();
            Graphics gPic;

            if (Main.Eng_dicinfo.Count <= 0)
                return;

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

                string temp = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].ReviewDefectName;

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
                int length = 0;
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Yellow;
                    ImageNameEQ[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = Main.Eng_dicinfo.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Main.Eng_dicinfo.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Red;
                    ImageNameEQ[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = Main.Eng_dicinfo.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Main.Eng_dicinfo.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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

                DefectState[i].Text = "";
                ImageNameLB[i].Text = "";
                ImageNameEQ[i].Text = "";
                PictureData.ElementAt(i).Tag = Color.Black;
            }
            this.Focus();
        }
        public void Eng_Cheked_State_DF()
        {
            int length = 0;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            dicInfo_Filter = Sorted_dic;

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;
                    ImageNameEQ[i].ForeColor = Color.Yellow;
                    ImageNameEQ[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {

                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";



                    if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
        }
        public void Eng_Set_View()
        {

            if (Main == null)
            {
                // MessageBox.Show("Load된 Image가 없습니다.");

                return;
            }
            Main.ViewType = "SetView";
            OpenViewType = "SetView";

            this.Controls.Clear();
            PictureData.Clear();






            //if (Main.Camera_NO_Filter_TB.Text != "")
            //{
            //    string[] Split_String = null;
            //    Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
            //    bool Target = false;
            //    DicInfo_Filtered.Clear();
            //    foreach (string No in Main.DicInfo.Keys.ToList())
            //    {
            //        if (Main.DicInfo.ContainsKey(No))
            //        {
            //            if (Split_String.Contains(Main.DicInfo[No].CameraNo.ToString()))
            //            {
            //                DicInfo_Filtered.Add(No, Main.DicInfo[No]);
            //            }

            //        }

            //    }
            //    if (DicInfo_Filtered.Count == 0)
            //    {
            //        DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
            //        MessageBox.Show("해당 카메라 이미지가 없습니다.");
            //        Main.Camera_NO_Filter_TB.Text = string.Empty;
            //    }

            //}

            if (Main.Waiting_Del.Count > 0)
            {
                foreach (string pair in Main.Eng_dicinfo.Keys.ToList())
                {
                    if (Main.Waiting_Del.ContainsKey(pair))
                        Main.Eng_dicinfo.Remove(pair);
                }
            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;
            Current_Frame_PageNum = 1;



            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();


            Set_Image_Eng();

            Last_Picture_Selected_Index = -1;

            Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Eng_dicinfo.Count);
            Main.Eng_Print_List();

            //Main.Print_List();
            this.Focus();
            //Filter_NO_Set();

        }
    }
}
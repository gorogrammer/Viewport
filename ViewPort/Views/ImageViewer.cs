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
        
        public FormViewPort Main;
        public ManagerForm Manager;
        LoadingGIF_Func waitform = new LoadingGIF_Func();
        public SplitterPanel split;
        string openViewType = string.Empty;
        string openFilterType = string.Empty;
        bool normalCheck = false;
        struct BoxRange { public int left, top, width, height; }
        List<string> Display_Id = new List<string>();
        List<string> Change_state_List = new List<string>();
        public List<PictureBox> PictureData = new List<PictureBox>();
        List<PictureBox> Picture_Glass = new List<PictureBox>();
        List<string> Select_Pic = new List<string>();
        List<string> Eq_cb_need_del = new List<string>();
        List<int> frame_List_Img = new List<int>();
        Dictionary<string, ImageInfo> dicInfo_Filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Delete = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> sorted_dic_Eng = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> frame_dicInfo_Filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> expand_ImgInfo = new Dictionary<string, ImageInfo>();
        List<string> f12_del_list = new List<string>();
        Dictionary<string, ImageInfo> Before_No1_Filter_dicInfo = new Dictionary<string, ImageInfo>();
        int setting = 0;
        List<int> apply_List_opne = new List<int>(); 
        List<int> Notapply_List_opne = new List<int>();
        int befroe_X = 0;
        int before_Y = 0;    
        public int Filter_NO_1 = 0;
        public int Filter_F9 = 0;
        public int Filter_F10 = 0;
        public int Filter_F5 = 0;
        public int Filter_F = 0;
        public int viewMode_PSW_Check = 0;
        public int shift_del = 0;
        int only_del = 0;
        Image expand_img = null;
        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        List<string> Print_Frame = new List<string>();
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        Label[] DefectState, ImageNameLB, ImageNameEQ;
        List<Label> ImageNameLBList = new List<Label>();
        List<Label> ImageNameEQList = new List<Label>();
        public int Current_PageNum, Total_PageNum;
        int Current_Frame_PageNum, Total_Frame_PageNum;
        int Last_Picture_Selected_Index;
        int EachPage_ImageNum;
        List<string> imglist = new List<string>();
        int Frame_Filter_check = 0;
        Point src_Mouse_XY, dst_Mouse_XY;
        Point A_Mouse_XY, B_Mouse_XY;

        public int cols, rows, width, height;

        ZipArchive zip = null;
        public Dictionary<string, ImageInfo> DicInfo_Filtered
        {
            get { return dicInfo_Filter; }
            set { dicInfo_Filter = value; }
        }

        public Dictionary<string, ImageInfo> DicInfo_Delete
        {
            get { return dicInfo_Delete; }
            set { dicInfo_Delete = value; }
        }

        public List<string> Select_Pic_List
        {
            get { return Select_Pic; }
            set { Select_Pic = value; }
        }

        public List<string> Change_state
        {
            get { return Change_state_List; }
            set { Change_state_List = value; }
        }

        public List<int> Frame_List_Img { get => frame_List_Img; set => frame_List_Img = value; }

        public int Setting { get => setting; set => setting = value; }

        public int Befroe_X { get => befroe_X; set => befroe_X = value; }
        public int Before_Y { get => before_Y; set => before_Y = value; }
        public int ViewMode_PSW_Check { get => viewMode_PSW_Check; set => viewMode_PSW_Check = value; }

        public int Shift_del { get => shift_del; set => shift_del = value; }
        public Dictionary<string, ImageInfo> sorted_dic { get => Sorted_dic; set => Sorted_dic = value; }

        public int Only_del { get => only_del; set => only_del = value; }
        public string OpenViewType { get => openViewType; set => openViewType = value; }
        public string OpenFilterType { get => openFilterType; set => openFilterType = value; }
        public Dictionary<string, ImageInfo> Frame_dicInfo_Filter { get => frame_dicInfo_Filter; set => frame_dicInfo_Filter = value; }
        public Dictionary<string, ImageInfo> Sorted_dic_Eng { get => sorted_dic_Eng; set => sorted_dic_Eng = value; }
        public List<string> F12_del_list { get => f12_del_list; set => f12_del_list = value; }
        private void FPaint(object sender, PaintEventArgs e)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new PaintEventHandler(this.FPaint), new object[] { sender, e });
                return;
            }


            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.ClientRectangle))
            {
                bufferedgraphic.Graphics.Clear(Color.Silver);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p = new Pen(Color.FromArgb(111, 91, 160), 3);
                bufferedgraphic.Graphics.DrawLine(p, 0, 0, 100, 100);
                p.Dispose();

                bufferedgraphic.Render(e.Graphics);
            }



        }
        public void SelectGrid_Img_View(string id)
        {
            Rectangle regSelection = new Rectangle();
            Graphics gPic;
            Pen pen;
            timer1.Interval = 1500;
            pen = new System.Drawing.Pen(System.Drawing.Color.Orange, 10);
            List<string> dic_index_List = new List<string>();
            if (Main.EngrMode)
                dic_index_List = Main.Eng_dicinfo.Keys.ToList();
            else if (OpenFilterType == "Filter")
                dic_index_List = Main.Filter_CheckEQ_Dic.Keys.ToList();
            else
                dic_index_List = dicInfo_Filter.Keys.ToList();

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);

            int index = dic_index_List.IndexOf(id);
            double quotient = (index / (cols * rows));
            int view_page = Convert.ToInt32(System.Math.Truncate(quotient));

            Last_Picture_Selected_Index = -1;
            Current_PageNum = view_page + 1;
            Main.S_Page_TB.Text = Current_PageNum.ToString();
            if (Main.Eng_dicinfo.Count > 0)
            {
                Set_Image_Eng();
            }
            else if (Main.Frame_View_CB.Checked)
            {
                Frame_Set_Image();

                
            }
            else
            {
                Set_Image();

               
            }
            for (int i = 0; i < PictureData.Count; i++)
            {
                if (PictureData.ElementAt(i).Name == id)
                {
                    if (PictureData.ElementAt(i).Name == id)
                    {
                        regSelection.Location = new Point(4, 4);
                        regSelection.Size = new Size(PictureData.ElementAt(i).Image.Width - 10, PictureData.ElementAt(i).Image.Height - 10);

                        gPic = Graphics.FromImage(PictureData.ElementAt(i).Image);
                        gPic.DrawRectangle(pen, regSelection);
                        timer1.Start();
                    }

                }
            }
            



        }
        
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

       
        public void Key_only_del()
        {
            Main.InputKey += 1;
            
                ProgressBar1 progressBar = new ProgressBar1();
                Select_Pic_List.Clear();
                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();
                try
                {
                    
                    progressBar.Text = "Delete Wait..";
                    progressBar.Show();
                    progressBar.SetProgressBarMaxSafe(100);


                    if (Main.Frame_View_CB.Checked)
                    {
                        for (int i = 0; i < (cols * rows); i++)
                        {
                            if ((index + i) >= frame_dicInfo_Filter.Count)
                                break;
                            Selected_Picture_Index.Add(index + i);
                        }
                        for (int p = 0; p < Selected_Picture_Index.Count; p++)
                        {
                            if (frame_dicInfo_Filter[frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName == "선택")
                            {
                                Select_Pic_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                            }
                        }

                        Get_Delete_IMG();

                        for (int i = 0; i < Select_Pic_List.Count; i++)
                        {
                            if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                            {
                                frame_dicInfo_Filter.Remove(Select_Pic_List[i]);
                            }
                            if (Main.Eng_dicinfo.Count > 1)
                            {
                                if (Main.Eng_dicinfo.ContainsKey(Select_Pic_List[i]))
                                {
                                    Main.Eng_dicinfo.Remove(Select_Pic_List[i]);
                                }
                            }
                        }
                        if (Filter_NO_1 == 1)
                        {
                            Main.No1_Dl_PrintList();
                            Filter_NO_1 = 0;
                        }
                        else if(Main.Eng_dicinfo.Count > 1)
                        {
                            Main.Eng_Print_List();
                        }
                        else
                            Main.Dl_PrintList();


                        Eq_cb_need_del = new List<string>(Select_Pic_List);
                        if (Main.Eng_dicinfo.Count > 1)
                        {
                            Set_Image_Eng();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Eng_dicinfo.Count);
                        }
                        else
                        {
                            DL_Frame_Set_View();


                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                           
                        }
                        Main.Wait_Del_Print_List();
                    }
                    else
                    {
                    if (Main.EngrMode)
                    {
                        foreach (string pair in Main.Eng_dicinfo.Keys.ToList())
                        {
                            if (Main.Eng_dicinfo[pair].ReviewDefectName == "선택")
                            {
                                Select_Pic_List.Add(pair);
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        
                        
                        if (openFilterType == "Filter")
                        {
                            for (int i = 0; i < (cols * rows); i++)
                            {
                                if ((index + i) >= Main.Filter_CheckEQ_Dic.Count)
                                    break;
                                Selected_Picture_Index.Add(index + i);
                            }
                            for (int p = 0; p < Selected_Picture_Index.Count; p++)
                            {
                                if (Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(Main.Filter_CheckEQ_Dic.ElementAt(Selected_Picture_Index[p]).Key);
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < (cols * rows); i++)
                            {
                                if ((index + i) >= dicInfo_Filter.Count)
                                    break;
                                Selected_Picture_Index.Add(index + i);
                            }
                            for (int p = 0; p < Selected_Picture_Index.Count; p++)
                            {
                                if (dicInfo_Filter[dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                                }

                            }
                        }
                    }

                    Get_Delete_IMG();
                        progressBar.tabProgressBarSafe(50);
                        for (int i = 0; i < Select_Pic_List.Count; i++)
                        {
                        if (OpenFilterType == "Filter" && Main.Filter_CheckEQ_Dic.ContainsKey(Select_Pic_List[i]))
                            Main.Filter_CheckEQ_Dic.Remove(Select_Pic_List[i]);

                            if (dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                            {
                                dicInfo_Filter.Remove(Select_Pic_List[i]);
                            }
                            if (Main.EngrMode)
                            {
                                if (Main.Eng_dicinfo.ContainsKey(Select_Pic_List[i]))
                                {
                                    Main.Eng_dicinfo.Remove(Select_Pic_List[i]);
                                    sorted_dic_Eng.Remove(Select_Pic_List[i]);
                                }
                            }
                        }

                        Main.Dl_PrintList();

                        if (Main.Eng_dicinfo.Count > 1)
                        {
                            //Main.Dl_PrintList();
                         //   Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                            //Main.FI_RE_B.Enabled = false;
                            Main.EQ_Data_Update_eng(sorted_dic_Eng);
                          //  Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                            Main.Eng_Print_List();
                        }
                        else
                        {
                            //Main.Dl_PrintList();
                           // Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                          //  ///Main.FI_RE_B.Enabled = false;
                            Main.EQ_Data_Updaten(Main.DicInfo);
                          //  Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                            Main.Print_List();
                           // Main.Return_update_Equipment_DF_CLB();
                        }
                        if (Main.Eng_dicinfo.Count > 1)
                        {
                            Set_Image_Eng();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Eng_dicinfo.Count);
                        }
                        else
                        {
                            Eq_cb_need_del = new List<string>(Select_Pic);
                            Del_Set_View();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);
                        }
                            Main.Wait_Del_Print_List();


                        progressBar.tabProgressBarSafe(50);
                    }
                   // Main.filterMode = Enums.FILTERTYPE.NULL;
                    Main.delete_W = Main.Waiting_Del.Count;
                    Main.InfoListCount = Main.InfoListCount - Select_Pic_List.Count;
                    Main.UpdateDeleteText();
                    Select_Pic_List.Clear();
                    progressBar.ExitProgressBarSafe();
                    //Eq_cb_need_del.Clear();
                }
                catch (Exception ex)
                {
                    progressBar.ExitProgressBarSafe();
                    MessageBox.Show(ex.ToString());

                }
           

        }
        public void Key_shift_del()
        {
            Main.InputKey += 1;
           
                ProgressBar1 progressBar = new ProgressBar1();
                Select_Pic_List.Clear();
                try
                {
                   
                    progressBar.Text = "Delete Wait..";
                    progressBar.Show();
                    progressBar.SetProgressBarMaxSafe(100);


                    if (Main.Frame_View_CB.Checked)
                    {

                        foreach (string pair in frame_dicInfo_Filter.Keys.ToList())
                        {
                            if (frame_dicInfo_Filter[pair].ReviewDefectName == "선택")
                            {
                                Select_Pic_List.Add(pair);
                            }
                            else
                            {

                            }
                        }
                        Get_Delete_IMG();

                        for (int i = 0; i < Select_Pic_List.Count; i++)
                        {
                            if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                            {
                                frame_dicInfo_Filter.Remove(Select_Pic_List[i]);
                            }

                        }
                        //if (Filter_NO_1 == 1)
                        //{
                        //    Main.No1_Dl_PrintList();
                        //    Filter_NO_1 = 0;
                        //}
                        //else
                        Main.Dl_PrintList();

                        Eq_cb_need_del = new List<string>(Select_Pic_List);
                        DL_Frame_Set_View();


                        Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                        Main.Wait_Del_Print_List();

                    }
                    else
                    {
                        if (Main.EngrMode)
                        {
                            foreach (string pair in Main.Eng_dicinfo.Keys.ToList())
                            {
                                if (Main.Eng_dicinfo[pair].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(pair);
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {
                            foreach (string pair in dicInfo_Filter.Keys.ToList())
                            {
                                if (dicInfo_Filter[pair].ReviewDefectName == "선택")
                                {
                                    Select_Pic_List.Add(pair);
                                }
                                else
                                {

                                }
                            }
                        }
                        Get_Delete_IMG();

                        for (int i = 0; i < Select_Pic_List.Count; i++)
                        {
                            if (dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                            {
                                dicInfo_Filter.Remove(Select_Pic_List[i]);
                            }
                            if (Main.EngrMode)
                            {
                                if (Main.Eng_dicinfo.ContainsKey(Select_Pic_List[i]))
                                {
                                    Main.Eng_dicinfo.Remove(Select_Pic_List[i]);
                                    sorted_dic_Eng.Remove(Select_Pic_List[i]);
                                }
                            }

                        }

                        //if (Filter_NO_1 == 1)
                        //{
                        //    Main.No1_Dl_PrintList();
                        //    Filter_NO_1 = 0;
                        //}
                        //else
                        Main.Dl_PrintList();


                        Eq_cb_need_del = new List<string>(Select_Pic);
                        if (Main.Eng_dicinfo.Count > 1)
                        {
                            Eng_Set_View();
                          //  Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                          //  Main.FI_RE_B.Enabled = false;
                            Main.EQ_Data_Update_eng(Main.Eng_dicinfo);
                          //  Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                            Main.Eng_Print_List();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Eng_dicinfo.Count);
                        }
                        else
                        {
                           // Main.Equipment_DF_CLB.SelectedValueChanged -= Main.Equipment_DF_CLB_ItemCheck;
                           // Main.FI_RE_B.Enabled = false;
                            Main.EQ_Data_Updaten(Main.DicInfo);
                          //  Main.Equipment_DF_CLB.SelectedValueChanged += Main.Equipment_DF_CLB_ItemCheck;
                          //  Main.Print_List();
                            //Del_Set_View();
                            Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);
                        }
                      //  Main.filterMode = Enums.FILTERTYPE.NULL;
                        Main.delete_W = Main.Waiting_Del.Count;
                        Main.InfoListCount = Main.InfoListCount - Select_Pic_List.Count;
                        Main.UpdateDeleteText();

                        Main.Wait_Del_Print_List();
                       

                        progressBar.tabProgressBarSafe(100);
                    }

                    Select_Pic_List.Clear();
                    progressBar.ExitProgressBarSafe();
                    //Eq_cb_need_del.Clear();
                }
                catch (Exception ex)
                {
                    progressBar.ExitProgressBarSafe();
                    MessageBox.Show(ex.ToString());

                }


        }
  
        public ImageViewer(FormViewPort mainForm)
        {
            InitializeComponent();
            this.Paint += FPaint;
        }

        public void Load_Del()
        {
            Select_Pic_List.Clear();

            foreach (var key in dicInfo_Delete.Keys.ToList())
            {
                dicInfo_Delete[key].DeleteCheck = "삭제대기";

                if (dicInfo_Filter.ContainsKey(key))
                {
                    Select_Pic_List.Add(key);
                    dicInfo_Filter.Remove(key);
                }
                else if(Main.Real_SDIP_200_DIc.Keys.ToList().Contains(key))
                {
                    if(Main.Sdip_200_code_dicInfo.Keys.ToList().Contains(key))
                    {
                        Main.Sdip_200_code_dicInfo.Remove(key);
                    }
                }
                else if(Main.F5_code_dicInfo.Keys.ToList().Contains(key))
                {

                }
                
            }

            if(Main.F12_del_list_main.Count>0)
            {
                foreach(string pair in Main.F12_del_list_main)
                {
                    if (dicInfo_Filter.ContainsKey(pair))
                    {
                        Select_Pic_List.Add(pair);
                        dicInfo_Filter.Remove(pair);
                    }
                        
                }
               
            }
            Del_Set_View();

        }

        public void XY_Filter_Set(Dictionary<string, ImageInfo> dic)
        {
            if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
            {
                Frame_dicInfo_Filter = dic;
                Frame_Set_View();
                Main.Frame_Print_List();
            }
            else
            {
                DicInfo_Filtered = dic;
                Set_View();
                Main.Print_List();
            }


        }
        public void Get_Delete_IMG()
        {
            dicInfo_Delete.Clear();
            if (Main.Frame_View_CB.Checked)
            {
                for (int p = 0; p < Select_Pic_List.Count; p++)
                {
                    if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[p]))
                    {

                        dicInfo_Delete.Add(Select_Pic_List[p], frame_dicInfo_Filter[Select_Pic_List[p]]);

                        dicInfo_Delete[Select_Pic_List[p]].DeleteCheck = "삭제대기";
                    }
                }
            }
            else
            {
                for (int p = 0; p < Select_Pic_List.Count; p++)
                {
                    if (dicInfo_Filter.ContainsKey(Select_Pic_List[p]))
                    {

                        dicInfo_Delete.Add(Select_Pic_List[p], dicInfo_Filter[Select_Pic_List[p]]);

                        dicInfo_Delete[Select_Pic_List[p]].DeleteCheck = "삭제대기";
                    }
                }
            }

            foreach(string pair in dicInfo_Delete.Keys.ToList())
            {
                if(Main.Waiting_Del.ContainsKey(pair))
                { }
                else
                {
                    Main.Waiting_Del.Add(pair, dicInfo_Delete[pair]);
                }
            }

            
        }

        private void ImageViewer_PL_MouseDown(object sender, MouseEventArgs e)
        {

            this.Focus();
            src_Mouse_XY.X = e.Location.X;
            src_Mouse_XY.Y = e.Location.Y;

            if (Draged_PB != null)
            {
                src_Mouse_XY.X += Draged_PB.Location.X;
                src_Mouse_XY.Y += Draged_PB.Location.Y;
            }
        }
        private void CopyToClipboard()
        {
            //Copy to clipboard
            DataObject dataObj = Main.dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void ImageViewer_Load(object sender, EventArgs e)
        {

        }

        public void Filter_NO_Set()
        {
            Filter_NO_1 = 0;
            Filter_F9 = 0;
            Filter_F10 = 0;
            Filter_F5 = 0;
            Filter_F = 0;
            Frame_Filter_check = 0;
            ViewMode_PSW_Check = 0;
        }
        public void Set_View()
        {

            if (Main == null)
            {
               // MessageBox.Show("Load된 Image가 없습니다.");

                return;
            }
            Main.ViewType = "SetView";
            OpenViewType = "SetView";

            if (Main.Fixed_CB.Checked == true)
            {
                Main.Height_TB.Text = Main.Width_TB.Text;
            }
            this.Controls.Clear();
            PictureData.Clear();

            if (!Main.Exceed_CB.Checked && Filter_NO_1 != 1 && Filter_F9 != 1 && Filter_F10 != 1 && Filter_F5 != 1 && Filter_F != 1 && Main.List_filter != 1 && Main.State_Filter != 1 && Frame_Filter_check!=1 && Main.ImageSize_Filter_NO !=1 && OpenFilterType == "NoneFilter" && Main.Filter_CheckEQ_Dic.Count ==0)
            {
                dicInfo_Filter = Main.DicInfo;

            }
            else if (Main.Exceed_CB.Checked)
            {
                dicInfo_Filter = Main.Exceed_filter;

            }
            if (dicInfo_Filter.Count != 0 && OpenFilterType == "NoneFilter")
            {
                if (Main.Frame_BT.Checked)
                {
                    Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                    DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
                }
                else if (Main.XY_BT.Checked)
                {

                    Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                    int maxY = dicInfo_Filter.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;

                    foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)

                    {

                        int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                        int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                        int SortedXY = y * (maxY * 10) + x;

                        pair.Value.SortedXY = SortedXY;


                    }
                    var keyValues = dicInfo_Filter.OrderBy(x => x.Value.SortedXY);
                    foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                    {

                        SortXY_DIC_Load.Add(pair.Key, pair.Value);


                    }
                    DicInfo_Filtered = SortXY_DIC_Load;
                }
            }
            else
            {
                if (DicInfo_Filtered.Count != 0 && OpenFilterType == "NoneFilter")
                {
                    if (Main.Frame_BT.Checked)
                    {
                        Sorted_dic = DicInfo_Filtered.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                        DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
                    }
                    else if (Main.XY_BT.Checked)
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
                        DicInfo_Filtered = SortXY_DIC_Load;
                    }
                }

                else if(OpenFilterType == "Filter")
                {
                    if (Main.Frame_BT.Checked)
                    {
                        Sorted_dic = Main.Filter_CheckEQ_Dic.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                        Main.Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(Sorted_dic);
                    }
                    else if (Main.XY_BT.Checked)
                    {

                        Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                        int maxY = Main.Filter_CheckEQ_Dic.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;

                        foreach (KeyValuePair<string, ImageInfo> pair in Main.Filter_CheckEQ_Dic)

                        {

                            int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                            int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                            int SortedXY = y * (maxY * 10) + x;

                            pair.Value.SortedXY = SortedXY;


                        }
                        var keyValues = Main.Filter_CheckEQ_Dic.OrderBy(x => x.Value.SortedXY);
                        foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                        {

                            SortXY_DIC_Load.Add(pair.Key, pair.Value);


                        }
                        Main.Filter_CheckEQ_Dic = SortXY_DIC_Load;
                    }
                }
            }
            
            
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
                foreach (string pair in DicInfo_Filtered.Keys.ToList())
                {
                    if (Main.Waiting_Del.ContainsKey(pair))
                        DicInfo_Filtered.Remove(pair);
                }
            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;
            Current_Frame_PageNum = 1;



            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();

            if (Main.Frame_View_CB.Checked)
            {
                Main.Frame_S_Page_TB.Text = Current_PageNum.ToString();
                Total_Frame_PageNum = frame_List_Img.Count;
                Main.Frame_E_Page_TB.Text = Total_Frame_PageNum.ToString();


                Frame_Set_Image();
            }
            else
                Set_Image();

            Last_Picture_Selected_Index = -1;

            //Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);


            //Main.Print_List();
            this.Focus();
            //Filter_NO_Set();

        }
        
        public void Code_200_Set_View()
        {
            Main.ViewType = "Code_200_SetView";
            OpenViewType = "Code_200_SetView";

            this.Controls.Clear();
            PictureData.Clear();

            dicInfo_Filter = Main.Filter_200_dic_Main;

            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            DicInfo_Filtered = Sorted_dic;

            if (Main.Camera_NO_Filter_TB.Text != "")
            {
                string[] Split_String = null;
                Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
                bool Target = false;

                foreach (string No in DicInfo_Filtered.Keys.ToList())
                {
                    if (DicInfo_Filtered.ContainsKey(No))
                    {
                        if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
                        {
                            continue;
                        }
                        else
                            DicInfo_Filtered.Remove(No);
                    }

                }

            }

            if (Main.Waiting_Del.Count > 0)
            {
                foreach (string pair in DicInfo_Filtered.Keys.ToList())
                {
                    if (Main.Waiting_Del.ContainsKey(pair))
                        DicInfo_Filtered.Remove(pair);
                }
            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;
            Current_PageNum = 1;
            Current_Frame_PageNum = 1;



            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();

            if (Main.Frame_View_CB.Checked)
            {
                Main.Frame_S_Page_TB.Text = Current_PageNum.ToString();
                Total_Frame_PageNum = frame_List_Img.Count;
                Main.Frame_E_Page_TB.Text = Total_Frame_PageNum.ToString();



                Frame_Set_Image();
            }
            else
                Set_Image();

            Last_Picture_Selected_Index = -1;

            Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count); ;
            Main.Print_List();
            this.Focus();
            Filter_NO_Set();

        }

        
        public bool Frame_Set_View()
        {
            if (Main == null)
            {
                MessageBox.Show("Load된 Image가 없습니다.");

                return false;
            }

            Main.ViewType = "FrameSetView";
            OpenViewType = "FrameSetView";

            this.Controls.Clear();
            PictureData.Clear();

            if (Main.Exceed_CB.Checked)
            {
                Frame_List_Img = Main.Exceed_List;
            }
            else if (Main.ListFiler == "FrameList_FIlter" || Main.ListFiler == "List_FIlter")
            {
                Frame_List_Img.Clear();
                foreach (string pair in DicInfo_Filtered.Keys.ToList())
                {
                    if (Frame_List_Img.Contains(DicInfo_Filtered[pair].FrameNo))
                    {

                    }
                    else
                    {
                        Frame_List_Img.Add(DicInfo_Filtered[pair].FrameNo);

                    }
                }
            }
            else
            {
                Frame_List_Img.Clear();
                foreach (string pair in DicInfo_Filtered.Keys.ToList())
                {
                    if (Frame_List_Img.Contains(DicInfo_Filtered[pair].FrameNo))
                    {

                    }
                    else
                    {
                        Frame_List_Img.Add(DicInfo_Filtered[pair].FrameNo);

                    }
                }
            }
            //if (Main.Eq_CB_dicInfo.Count > 0)
            //{
            //    dicInfo_Filter = Main.Eq_CB_dicInfo;
            //    Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            //    DicInfo_Filtered = Sorted_dic;
            //    Frame_List_Img.Clear();
            //    foreach (KeyValuePair<string, ImageInfo> pair in DicInfo_Filtered)
            //    {
            //        if (Frame_List_Img.Contains(pair.Value.FrameNo))
            //        {
            //        }
            //        else
            //        {
            //            Frame_List_Img.Add(pair.Value.FrameNo);
            //        }
            //    }
            //}


            if (Filter_NO_1 == 1)
            {
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                DicInfo_Filtered = Sorted_dic;
            }
            else if (Filter_F == 1)
            {
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                DicInfo_Filtered = Sorted_dic;
            }
            else if (Main.ViewType == "FrameList_FIlter")
            {
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                DicInfo_Filtered = Sorted_dic;
            }
            else
            {
                //dicInfo_Filter = Main.DicInfo;
                //Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                //DicInfo_Filtered = Sorted_dic;
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                DicInfo_Filtered = Sorted_dic;
            }




            if (Main.Camera_NO_Filter_TB.Text != "")
            {
                string[] Split_String = null;
                Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');


                foreach (string No in DicInfo_Filtered.Keys.ToList())
                {
                    if (DicInfo_Filtered.ContainsKey(No))
                    {
                        if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
                        {
                            continue;
                        }
                        else
                            DicInfo_Filtered.Remove(No);
                    }

                }

            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);


            Current_PageNum = 1;
            Current_Frame_PageNum = 1;


            Set_PictureBox();
            Frame_Set_Image();

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((frame_dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();


            Main.Frame_S_Page_TB.Text = Current_PageNum.ToString();
            Total_Frame_PageNum = frame_List_Img.Count;
            Main.Frame_E_Page_TB.Text = Total_Frame_PageNum.ToString();


            Last_Picture_Selected_Index = -1;
            //Main.Print_List();
            this.Focus();
            Filter_NO_Set();
            return true;
        }
        public void Set_Frame_Filter()
        {
            Frame_List_Img.Clear();
            foreach (string pair in Main.DicInfo.Keys.ToList())
            {
                if (Frame_List_Img.Contains(Main.DicInfo[pair].FrameNo))
                {

                }
                else
                {
                    Frame_List_Img.Add(Main.DicInfo[pair].FrameNo);

                }
            }
        }
        public void DL_Frame_Set_View()
        {
            Main.ViewType = "DLFrameSetView";
            OpenViewType = "DLFrameSetView";

            this.Controls.Clear();
            PictureData.Clear();

            //frame_List_Img = Main.Frame_List_Main;
            //dicInfo_Filter = Main.DicInfo;

            DicInfo_Filtered = Main.DicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;

            if (Main.Camera_NO_Filter_TB.Text != "")
            {
                string[] Split_String = null;
                Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');


                foreach (string No in DicInfo_Filtered.Keys.ToList())
                {
                    if (DicInfo_Filtered.ContainsKey(No))
                    {
                        if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
                        {
                            continue;
                        }
                        else
                            DicInfo_Filtered.Remove(No);
                    }

                }

            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            if (frame_dicInfo_Filter.Count == 0)
            {
                frame_List_Img.RemoveAt(Current_Frame_PageNum - 1);
                Current_Frame_PageNum = Current_Frame_PageNum - 1;
            }

            if (((frame_dicInfo_Filter.Count - 1) / (cols * rows)) + 1 < Current_PageNum)
                Current_PageNum = ((frame_dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            else
                Current_PageNum = int.Parse(Main.S_Page_TB.Text);

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            if (frame_List_Img.Count <= Current_Frame_PageNum)
                Current_Frame_PageNum = frame_List_Img.Count;
            else
                Current_Frame_PageNum = int.Parse(Main.Frame_S_Page_TB.Text);

            Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();
            Total_Frame_PageNum = frame_List_Img.Count;
            Main.Frame_E_Page_TB.Text = Total_Frame_PageNum.ToString();

            Set_PictureBox();
            Frame_Set_Image();
            Last_Picture_Selected_Index = -1;
            this.Focus();
        }
        public void Eq_CB_Set_View()
        {
            Main.ViewType = "EQCBSetView";
            OpenViewType = "EQCBSetView";

            this.Controls.Clear();
            PictureData.Clear();



            dicInfo_Filter = Main.Eq_CB_dicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;

            for (int i = 0; i < Eq_cb_need_del.Count; i++)
            {
                DicInfo_Filtered.Remove(Eq_cb_need_del[i]);
            }

            //if (Main.Camera_NO_Filter_TB.Text != "")
            //{
            //    string[] Split_String = null;
            //    Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
            //    bool Target = false;

            //    foreach (string No in DicInfo_Filtered.Keys.ToList())
            //    {
            //        if (DicInfo_Filtered.ContainsKey(No))
            //        {
            //            if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
            //            {
            //                continue;
            //            }
            //            else
            //                DicInfo_Filtered.Remove(No);
            //        }

            //    }

            //}

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;


            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();
            Set_Image();
            Last_Picture_Selected_Index = -1;

            Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);
            this.Focus();
        }

        public void Eq_CB_Set_View_ING()
        {

            Main.ViewType = "EQCBSetView_ING";
            OpenViewType = "EQCBSetView_ING";

            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = new Dictionary<string, ImageInfo>(Main.Return_dicInfo);

            
            
            
            if (Main.Frame_BT.Checked)
            {
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                DicInfo_Filtered = Sorted_dic;
            }
            else if (Main.XY_BT.Checked)
            {
                Dictionary<string, ImageInfo> SortXY_DIC_Load = new Dictionary<string, ImageInfo>();
                int maxY = dicInfo_Filter.Max(x => Int32.Parse(x.Value.Y_Location)) / Main.Px;

                foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)

                {

                    int x = Int32.Parse(pair.Value.X_Location) / Main.Px;
                    int y = Int32.Parse(pair.Value.Y_Location) / Main.Px;

                    int SortedXY = y * (maxY * 10) + x;

                    pair.Value.SortedXY = SortedXY;


                }
                var keyValues = dicInfo_Filter.OrderBy(x => x.Value.SortedXY);
                foreach (KeyValuePair<string, ImageInfo> pair in keyValues)
                {

                    SortXY_DIC_Load.Add(pair.Key, pair.Value);


                }
                DicInfo_Filtered = SortXY_DIC_Load;
            }
            Eq_cb_need_del = Main.selected_Pic;
            //if (Main.Camera_NO_Filter_TB.Text != "")
            //{
            //    string[] Split_String = null;
            //    Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
            //    bool Target = false;

            //    foreach (string No in DicInfo_Filtered.Keys.ToList())
            //    {
            //        if (DicInfo_Filtered.ContainsKey(No))
            //        {
            //            if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
            //            {
            //                continue;
            //            }
            //            else
            //                DicInfo_Filtered.Remove(No);
            //        }

            //    }

            //}
            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;


            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();
           
            Set_Image();
            
            Last_Picture_Selected_Index = -1;
            Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);
            //Main.Return_dicInfo.Clear();
            this.Focus();
        }
        public void Filter_CB_after_Set_View()
        {

            Main.ViewType = "FilterCBafterSetView";
            OpenViewType = "FilterCBafterSetView";

            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = Main.DicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;

            if (Main.Camera_NO_Filter_TB.Text != "")
            {
                string[] Split_String = null;
                Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
                bool Target = false;

                foreach (string No in DicInfo_Filtered.Keys.ToList())
                {
                    if (DicInfo_Filtered.ContainsKey(No))
                    {
                        if (Split_String.Contains(DicInfo_Filtered[No].CameraNo.ToString()))
                        {
                            continue;
                        }
                        else
                            DicInfo_Filtered.Remove(No);
                    }

                }

            }

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;


            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();
            Set_Image();
            Last_Picture_Selected_Index = -1;
            this.Focus();
        }

        private void ImageViewer_PL_MouseMove(object sender, MouseEventArgs e)
        {
            if (Setting == 1)
            {
                //Main.Activate();
                A_Mouse_XY = this.PointToClient(Cursor.Position);
            }

        }

        private void ImageViewer_PL_MouseUp(object sender, MouseEventArgs e)
        {
            if (Setting == 1)
            {
               
                this.Focus();
                //Main.Activate();
                int tmp_XY;

                dst_Mouse_XY.X = e.Location.X;
                dst_Mouse_XY.Y = e.Location.Y;

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
                if (!Main.EngrMode)
                {
                    if (Main.Frame_View_CB.Checked)
                        Frame_Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);
                    else if(OpenFilterType =="Filter")
                        Find_Contain_PB_Filter(src_Mouse_XY, dst_Mouse_XY);
                    else
                        Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);
                }
                else
                    Eng_Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);



                if (!Main.EngrMode)
                {
                    if (Main.Frame_View_CB.Checked)
                        Frame_change_Glass();
                    else if (OpenFilterType == "Filter")
                        Filter_change_Glass();
                    else
                        Selected_change_Glass();
                }
                else
                    EngMode_change_Glass();

                if (Change_state_List.Count > 0)
                    Main.Changeed_State();

                src_Mouse_XY.X = -1;
                src_Mouse_XY.Y = -1;
                dst_Mouse_XY.X = -1;
                dst_Mouse_XY.Y = -1;

                //src_Mouse_XY = Point.Empty;
                //dst_Mouse_XY = Point.Empty;
                
            }

        }
        private void Find_Contain_PB_Filter(Point Src, Point Dst)
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
                    if (Main.Filter_CheckEQ_Dic.Count > (index + i))
                    {
                        Selected_Picture_Index.Add(index + i);

                        Select_Pic_List.Add(Main.Filter_CheckEQ_Dic.Keys.ElementAt(index + i));

                    }
                }


            }
            Select_Pic_List = Select_Pic_List.Distinct().ToList();
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                Last_Picture_Selected_Index = Selected_Picture_Index.ElementAt(i);

                if (Main.Filter_CheckEQ_Dic.Count <= Last_Picture_Selected_Index)
                {
                    //Last_Picture_Selected_Index = -1;
                    return;
                }

                if (Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName.Equals("양품"))
                    result = "선택";
                else
                {
                    result = "양품";

                    Select_Pic_List.Remove(Main.Filter_CheckEQ_Dic.Keys.ElementAt(Last_Picture_Selected_Index));
                    /*
                    for (int p = 0; p < Select_Pic_List.Count; p++)
                    {
                        if (Select_Pic_List[p].Equals(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic_List.RemoveAt(p);
                            p--;
                        }
                    }*/

                }


                Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName = result;
                Change_state_List.Add(Main.Filter_CheckEQ_Dic.Keys.ElementAt(Last_Picture_Selected_Index));

            }


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
                    if (dicInfo_Filter.Count > (index + i))
                    {
                        Selected_Picture_Index.Add(index + i);
                        
                        Select_Pic_List.Add(dicInfo_Filter.Keys.ElementAt(index + i));

                    }
                }
                

            }
            Select_Pic_List = Select_Pic_List.Distinct().ToList();
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                Last_Picture_Selected_Index = Selected_Picture_Index.ElementAt(i);

                if (dicInfo_Filter.Count <= Last_Picture_Selected_Index)
                {
                    //Last_Picture_Selected_Index = -1;
                    return;
                }

                if (dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName.Equals("양품"))
                    result = "선택";
                else
                {
                    result = "양품";

                    Select_Pic_List.Remove(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index));
                    /*
                    for (int p = 0; p < Select_Pic_List.Count; p++)
                    {
                        if (Select_Pic_List[p].Equals(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic_List.RemoveAt(p);
                            p--;
                        }
                    }*/

                }


                dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName = result;
                Change_state_List.Add(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index));

            }


        }
        private void Eng_Find_Contain_PB(Point Src, Point Dst)
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
                    if (Main.Eng_dicinfo.Count > (index + i))
                    {
                        Selected_Picture_Index.Add(index + i);
                        Select_Pic_List.Add(Main.Eng_dicinfo.Keys.ElementAt(index + i));
                    }
                }


            }
            Select_Pic_List = Select_Pic_List.Distinct().ToList();
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                Last_Picture_Selected_Index = Selected_Picture_Index.ElementAt(i);

                if (Main.Eng_dicinfo.Count <= Last_Picture_Selected_Index)
                {
                    //Last_Picture_Selected_Index = -1;
                    return;
                }

                if (Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName.Equals("양품"))
                    result = "선택";
                else
                {
                    result = "양품";
                    Select_Pic_List.Remove(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index));

                }


                Main.Eng_dicinfo[Main.Eng_dicinfo.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName = result;
                Change_state_List.Add(Main.Eng_dicinfo.Keys.ElementAt(Last_Picture_Selected_Index));

            }


        }
        private void Expand_Find_Contain_PB(Point Src, Point Dst)
        {
            Rectangle PB_Area, Drag_Area;

            int index =  cols * rows;

            expand_ImgInfo.Clear();
            expand_img = null;
            if (Selected_Picture_Index.Count > 0)
                Selected_Picture_Index.Clear();

            for (int i = 0; i < PictureData.Count; i++)
            {
                PB_Area = new Rectangle(PictureData.ElementAt(i).Left, PictureData.ElementAt(i).Top, PictureData.ElementAt(i).Width, PictureData.ElementAt(i).Height);
                Drag_Area = new Rectangle(Src.X, Src.Y, Dst.X - Src.X, Dst.Y - Src.Y);


                if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                {
                    if (PB_Area.IntersectsWith(Drag_Area))
                    {
                        if (frame_dicInfo_Filter.Count > ( i))
                        {
                            expand_img = PictureData[ i].Image;
                            expand_ImgInfo.Add(PictureData[ i].Name, frame_dicInfo_Filter[PictureData[i].Name]);

                        }
                        else
                        {
                            //MessageBox.Show("이미지를 다시 선택해주세요.");

                        }
                    }
                  
                }
                else
                {
                    if (PB_Area.IntersectsWith(Drag_Area))
                    {
                        if (dicInfo_Filter.Count > (i))
                        {
                            expand_img = PictureData[i].Image;
                            expand_ImgInfo.Add(PictureData[i].Name, dicInfo_Filter[PictureData[i].Name]);

                        }
                        else
                        {
                           // MessageBox.Show("이미지를 다시 선택해주세요.");
                        }
                    }
                }

            }


        }
        public void Clipboard_IMG()
        {

        }

        private void Frame_Find_Contain_PB(Point Src, Point Dst)
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
                    if (frame_dicInfo_Filter.Count > (index + i))
                    {
                        Selected_Picture_Index.Add(index + i);
                        Select_Pic_List.Add(frame_dicInfo_Filter.Keys.ElementAt(index + i));
                    }
                }


            }
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                Last_Picture_Selected_Index = Selected_Picture_Index.ElementAt(i);

                if (frame_dicInfo_Filter.Count <= Last_Picture_Selected_Index)
                {
                    //Last_Picture_Selected_Index = -1;
                    return;
                }

                if (frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName.Equals("양품"))
                    result = "선택";
                else
                {
                    result = "양품";
                    for (int p = 0; p < Select_Pic.Count; p++)
                    {
                        if (Select_Pic_List[p].Equals(frame_dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic_List.RemoveAt(p);
                            p--;
                        }
                    }

                }


                frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName = result;
                Change_state_List.Add(frame_dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index));

            }


        }

        public void Del_Set_View()
        {
            this.Controls.Clear();
            PictureData.Clear();

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);



            if (((dicInfo_Filter.Count - 1) / (cols * rows)) + 1 < int.Parse(Main.S_Page_TB.Text))
                Current_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            else
                Current_PageNum = int.Parse(Main.S_Page_TB.Text);



            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();
            Set_Image();
            Last_Picture_Selected_Index = -1;
            this.Focus();
        }

        public void Set_PictureBox()
        {
            int pre_cols, pre_rows;
            PictureBox temp_PB;
           //mageBox temp_IB;
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
                t_RangeInfo.left = (width * (i % (cols))) + ((i % (cols)) * 8) + 8;// + 375;
                t_RangeInfo.top = (height * (i / cols)) + ((i / cols) * 8) + 8;// + 35;
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
                temp_PB = new DoubleBufferPicture();

                temp_PB.CreateGraphics();
                temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.MouseHover += PictureBox_MouseHover;
                temp_PB.Location = new Point(ImageRangeInfo.ElementAt(i).left, ImageRangeInfo.ElementAt(i).top);
                temp_PB.Size = new Size(ImageRangeInfo.ElementAt(i).width, ImageRangeInfo.ElementAt(i).height);
                temp_PB.SizeMode = PictureBoxSizeMode.StretchImage;
                PictureData.Add(temp_PB);
                temp_PB.Parent = Main.splitContainer1.Panel2;
                
                
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                temp_PB = new DoubleBufferPicture();
                temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.MouseDown += new MouseEventHandler(PictureBox_MouseDown);
                temp_PB.MouseMove += new MouseEventHandler(PictureBox_MouseMove);
                temp_PB.MouseUp += new MouseEventHandler(PictureBox_MouseUp);
                temp_PB.MouseHover += PictureBox_MouseHover;

                temp_PB.Location = new Point(0, 0);
                temp_PB.Size = new Size(ImageRangeInfo.ElementAt(i).width, ImageRangeInfo.ElementAt(i).height);
                temp_PB.SizeMode = PictureBoxSizeMode.StretchImage;
                temp_PB.Image = new Bitmap(width, height);
                Picture_Glass.Add(temp_PB);

                Picture_Glass.ElementAt(i).BackColor = Color.Transparent;
                Picture_Glass.ElementAt(i).Parent = PictureData.ElementAt(i);
                
            }

            DefectState = new DoubleBufferLabel[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {
                temp_LB = new DoubleBufferLabel();
                temp_LB.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, 3);
               // temp_LB.Parent = Picture_Glass.ElementAt(i);
               
                DefectState[i] = temp_LB;
                Picture_Glass[i].Controls.Add(DefectState[i]);
            }

            ImageNameLB = new DoubleBufferLabel[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {


                temp_LB = new DoubleBufferLabel();
                temp_LB.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, height - 15);
               // temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameLB[i] = temp_LB;
                Picture_Glass[i].Controls.Add(ImageNameLB[i]);
            }


            ImageNameEQ = new DoubleBufferLabel[(cols * rows)];
            for (int i = 0; i < (cols * rows); i++)
            {
                temp_LB = new DoubleBufferLabel();
                temp_LB.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
                temp_LB.ForeColor = Color.Red;
                temp_LB.BackColor = Color.Transparent;
                temp_LB.AutoSize = true;
                temp_LB.Location = new Point(4, height - 15);
                //temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameEQ[i] = temp_LB;
                Picture_Glass[i].Controls.Add(ImageNameEQ[i]);
            }

            for (int i = 0; i < (cols * rows); i++)
            {
                this.Controls.Add(PictureData.ElementAt(i));
            }

            Last_Picture_Selected_Index = -1;
            Current_PageNum = (pre_cols * pre_rows * (Current_PageNum - 1)) / (cols * rows) + 1;
            if(Main.Eng_dicinfo.Count > 0)
                Total_PageNum = ((Main.Eng_dicinfo.Count - 1) / (cols * rows)) + 1;
            else
                Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Main.E_Page_TB.Text = Total_PageNum.ToString();


        }

        public void Set_Image()
        {
            if(Main == null)
            {
                return;
            }
            //Stopwatch st = new Stopwatch();
            //st.Start();
            Bitmap tmp_Img = null;
            int Size = (cols * rows);
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = (cols * rows);
            List<string> list = new List<string>();
            //Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //DicInfo_Filtered = Sorted_dic;
            if (OpenFilterType == "Filter")
                list = Main.Filter_CheckEQ_Dic.Keys.ToList();
            else
                list = DicInfo_Filtered.Keys.ToList();

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
            if (Main.Frame_BT.Checked)
                Print_Frame.Sort();

            //Application.DoEvents();

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
            if (OpenFilterType == "Filter")
                Filter_change_Glass();
            else    
                change_Glass();
            this.Focus();
            if(OpenFilterType == "Filter")
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Filter_CheckEQ_Dic.Count);
            else
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);
           // st.Stop();

           // MessageBox.Show(st.ElapsedMilliseconds.ToString());
        }

        public void Faster_Set_Image()
        {
            if (Main == null)
            {
                return;
            }
            //Stopwatch st = new Stopwatch();
            //st.Start();
            Bitmap tmp_Img = null;
            int Size = (cols * rows);
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = (cols * rows);
            List<string> list = new List<string>();
            //Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //DicInfo_Filtered = Sorted_dic;
            if (OpenFilterType == "Filter")
                list = Main.Filter_CheckEQ_Dic.Keys.ToList();
            else
                list = DicInfo_Filtered.Keys.ToList();

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
            if (Main.Frame_BT.Checked)
                Print_Frame.Sort();

            //Application.DoEvents();

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
                                        using (var srce = new Bitmap(subEntry.Open()))
                                        {
                                            switch (Main.Rotate_CLB.SelectedIndex)
                                            {
                                                case 0:
                                                    {
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        srce.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        srce.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        srce.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        MessageBox.Show("이미지 회전 오류");
                                                        return;
                                                    }
                                            }
                                            var dest = new Bitmap(PictureData.ElementAt(Current_Index).Width, PictureData.ElementAt(Current_Index).Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                                            using (var gr = Graphics.FromImage(dest))
                                            {
                                                gr.DrawImage(srce, new Rectangle(Point.Empty, dest.Size));
                                            }
                                            if (PictureData.ElementAt(Current_Index).Image != null) PictureData.ElementAt(Current_Index).Image.Dispose();
                                            PictureData.ElementAt(Current_Index).Image = dest;
                                            PictureData.ElementAt(Current_Index).Name = list[S_ImageIndex + Current_Index];
                                            srce.Dispose();
                                        }
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
                                using (var srce = new Bitmap(subEntry.Open()))
                                {
                                    switch (Main.Rotate_CLB.SelectedIndex)
                                    {
                                        case 0:
                                            {
                                                break;
                                            }
                                        case 1:
                                            {
                                                srce.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                                break;
                                            }
                                        case 2:
                                            {
                                                srce.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                                break;
                                            }
                                        case 3:
                                            {
                                                srce.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                                break;
                                            }
                                        default:
                                            {
                                                MessageBox.Show("이미지 회전 오류");
                                                return;
                                            }
                                    }
                                    var dest = new Bitmap(PictureData.ElementAt(Current_Index).Width, PictureData.ElementAt(Current_Index).Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                                    using (var gr = Graphics.FromImage(dest))
                                    {
                                        gr.DrawImage(srce, new Rectangle(Point.Empty, dest.Size));
                                    }
                                    if (PictureData.ElementAt(Current_Index).Image != null) PictureData.ElementAt(Current_Index).Image.Dispose();
                                    PictureData.ElementAt(Current_Index).Image = dest;
                                    PictureData.ElementAt(Current_Index).Name = list[S_ImageIndex + Current_Index];
                                }

                                //tmp_Img = new Bitmap(subEntry.Open());



                                //PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                //PictureData.ElementAt(Current_Index).Update();
                                //PictureData.ElementAt(Current_Index).Name = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);
                                // PictureData.ElementAt(Current_Index).Name = list[S_ImageIndex + Current_Index];

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
            if (OpenFilterType == "Filter")
                Filter_change_Glass();
            else
                change_Glass();
            this.Focus();
            if (OpenFilterType == "Filter")
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", Main.Filter_CheckEQ_Dic.Count);
            else
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);
            // st.Stop();

            // MessageBox.Show(st.ElapsedMilliseconds.ToString());
        }
        public void Filter_Frame_Set_IMG()
        {

            Current_Frame_PageNum = int.Parse(Main.Frame_S_TB.Text);
            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            List<string> Frame_Del_Range = new List<string>();
            int Frame_List_Index = Current_Frame_PageNum - 1;

            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;

            if (Main.Frame_Interval_CB.Checked || Main.Exceed_CB.Checked || Main.Eq_filter == 1)
            {
                Frame_List_Img.Clear();
                foreach (string pair in Main.DicInfo.Keys.ToList())
                {
                    if (Frame_List_Img.Contains(DicInfo_Filtered[pair].FrameNo))
                    {

                    }
                    else
                    {
                        Frame_List_Img.Add(Main.DicInfo[pair].FrameNo);

                    }
                }
                //Main.Eq_filter = 0;
            }
            if (Frame_Filter_check == 1 || Main.State_Filter ==1)
            {
                Main.Frame_S_TB.Text = Main.Frame_S_TB.Text;
                Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
                Frame_List_Index = frame_List_Img.FindIndex(r => r.Equals(int.Parse(Main.Frame_S_TB.Text)));
                //Frame_Filter_check = 0;

            }
            else
            {
                Main.Frame_S_TB.Text = frame_List_Img[Frame_List_Index].ToString();
                Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
            }

            frame_dicInfo_Filter.Clear();

            foreach (KeyValuePair<string, ImageInfo> kvp in Main.DicInfo)
            {
                if(Frame_List_Index != -1)
                {
                    if (kvp.Value.FrameNo == frame_List_Img[Frame_List_Index])
                        if (frame_dicInfo_Filter.ContainsKey(kvp.Key))
                            continue;
                        else
                            frame_dicInfo_Filter.Add(kvp.Key, kvp.Value);
                }
              

            }

            foreach (string pair in frame_dicInfo_Filter.Keys.ToList())
            {
                if (frame_dicInfo_Filter[pair].FrameNo == frame_List_Img[Frame_List_Index])
                    continue;
                else
                    frame_dicInfo_Filter.Remove(pair);
            }
            if (Main.Frame_View_CB.Checked)
            {

                if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) >= int.Parse(Main.Frame_E_Page_TB.Text))
                {
                    //MessageBox.Show("마지막 페이지 입니다.");
                }
                else
                {

                    Current_PageNum = 1;
                    Last_Picture_Selected_Index = -1;
                    Current_Frame_PageNum = int.Parse(Main.Frame_S_TB.Text);
                    Main.Frame_S_Page_TB.Text = Convert.ToString(frame_List_Img.FindIndex(r => r.Equals(Current_Frame_PageNum)) + 1);

                 
                }
                
                //Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
            }

        }
        public void Frame_Set_Image()
        {
            Bitmap tmp_Img = null;

            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            List<string> Frame_Del_Range = new List<string>();
            int Frame_List_Index = Current_Frame_PageNum - 1;

            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;
            
            if(Main.Frame_Interval_CB.Checked || Main.Exceed_CB.Checked || Main.Eq_filter == 1)
            {
                Frame_List_Img.Clear();
                foreach (string pair in Main.DicInfo.Keys.ToList())
                {
                    if (Frame_List_Img.Contains(DicInfo_Filtered[pair].FrameNo))
                    {

                    }
                    else
                    {
                        Frame_List_Img.Add(Main.DicInfo[pair].FrameNo);

                    }
                }
                //Main.Eq_filter = 0;
            }
            //else
            //{
            //    Set_Frame_Filter();
            //}

            
            

            if (Frame_Filter_check == 1 || Main.State_Filter ==1)
            {  
                    Main.Frame_S_TB.Text = Main.Frame_S_TB.Text;
                    Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
                    Frame_List_Index = frame_List_Img.FindIndex(r => r.Equals(int.Parse(Main.Frame_S_TB.Text)));
                    Frame_Filter_check = 0;

            }
            else
            {
                Main.Frame_S_TB.Text = frame_List_Img[Frame_List_Index].ToString();
                Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
            }


           

            if(Main.State_Filter != 1)
            {
                frame_dicInfo_Filter.Clear();

                foreach (KeyValuePair<string, ImageInfo> kvp in Main.DicInfo)
                {
                    if (kvp.Value.FrameNo == frame_List_Img[Frame_List_Index])
                        if (frame_dicInfo_Filter.ContainsKey(kvp.Key))
                            continue;
                        else
                            frame_dicInfo_Filter.Add(kvp.Key, kvp.Value);

                }

                foreach (string pair in frame_dicInfo_Filter.Keys.ToList())
                {
                    if (frame_dicInfo_Filter[pair].FrameNo == frame_List_Img[Frame_List_Index])
                        continue;
                    else
                        frame_dicInfo_Filter.Remove(pair);
                }
            }
            else
            {

            }
        






            if (DicInfo_Delete.Count > 0)
            {
                foreach (string pair in frame_dicInfo_Filter.Keys.ToList())
                {
                    if (DicInfo_Delete.ContainsKey(pair))
                    {
                        frame_dicInfo_Filter.Remove(pair);
                    }
                }
            }




            Frame_Del_Range.Clear();




            if (frame_dicInfo_Filter.Count <= 0)
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

            //Sorted_dic = Frame_dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //Frame_dicInfo_Filter = Sorted_dic;

            //var Sorted_frame_dic =
            //                                    from ent in frame_dicInfo_Filter
            //                                    orderby ent.Key
            //                                    select ent;

            //frame_dicInfo_Filter = Sorted_frame_dic.ToDictionary(pair => pair.Key, pair => pair.Value);

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((frame_dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();


            if (frame_dicInfo_Filter.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += frame_dicInfo_Filter.Count - ((cols * rows) * Current_PageNum);

            ////if (Main.ZipFilePath != "")
            {
                zip = ZipFile.Open(Main.ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
                string Open_ZipName;

                foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                {
                    Open_ZipName = entry.Name.Split('.')[0];
                    if (Open_ZipName[0].Equals('R'))
                        Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);

                    if (Main.Frame_View_CB.Checked)
                    {
                        if (entry.Name.ToUpper().IndexOf(".ZIP") != -1 && int.Parse(entry.Name.Split('.')[0]).Equals(frame_List_Img[Frame_List_Index]))
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

                                if (Current_Index >= EachPage_ImageNum)
                                    break;
                                if (subEntry.Name.Equals(frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
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
                                                MessageBox.Show("이건 무슨 경우? 님 해커임?");
                                                return;
                                            }
                                    }
                                    PictureData.ElementAt(Current_Index).Image = tmp_Img;
                                    PictureData.ElementAt(Current_Index).Name = frame_dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);

                                    Current_Index++;
                                }

                                if (Current_Index >= EachPage_ImageNum)
                                    break;

                            }
                            subZip.Dispose();
                        }
                    }
                    else
                    {
                        if (entry.Name.ToUpper().IndexOf(".ZIP") != -1 && entry.Name.Split('.')[0].Equals(frame_List_Img[Frame_List_Index]))
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

                                if (Current_Index >= EachPage_ImageNum)
                                    break;
                                if (subEntry.Name.Equals(frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
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
                                    PictureData.ElementAt(Current_Index).Name = frame_dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);

                                    Current_Index++;
                                }

                                if (Current_Index >= EachPage_ImageNum)
                                    break;
                                if (!frame_dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index).Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
                                {
                                    PF_index++;
                                    break;
                                }
                            }
                            subZip.Dispose();
                        }
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
            Frame_change_Glass();
            Main.Frame_Print_List();
            Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);

        }
        public void Filter_change_Glass()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            BufferedGraphics graphics; //
            Rectangle regSelection = new Rectangle();
            Graphics gPic;



            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

                string temp = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].ReviewDefectName;

                Pen pen;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    Picture_Glass.ElementAt(i).Image.Dispose();
                    Picture_Glass.ElementAt(i).Image = new Bitmap(width, height);
                    pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                    graphics = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Picture_Glass.ElementAt(i).Image.Width - 1, Picture_Glass.ElementAt(i).Image.Height - 1));
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
                gPic.SmoothingMode = SmoothingMode.HighSpeed;
                gPic.DrawRectangle(pen, regSelection);
            }

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].ReviewDefectName;
                int length = 0;
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

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Main.Filter_CheckEQ_Dic.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }

                    else if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = Main.Filter_CheckEQ_Dic.Keys.ElementAt(index);
                    }


                    else if (Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                    {
                        ImageNameEQ[i].Text = "";
                        ImageNameLB[i].Text = "";
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

                    if (Main.Print_Image_Name.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = Main.Filter_CheckEQ_Dic.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Main.Filter_CheckEQ_Dic.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Main.Filter_CheckEQ_Dic[Main.Filter_CheckEQ_Dic.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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
        public void change_Glass()
        {
            BufferedGraphicsContext context= BufferedGraphicsManager.Current;
            BufferedGraphics graphics; //
            Rectangle regSelection = new Rectangle();
            Graphics gPic;

            

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                Pen pen;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    Picture_Glass.ElementAt(i).Image.Dispose();
                    Picture_Glass.ElementAt(i).Image = new Bitmap(width, height);
                    pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                    graphics = context.Allocate(CreateGraphics(), new Rectangle(0,0, Picture_Glass.ElementAt(i).Image.Width - 1, Picture_Glass.ElementAt(i).Image.Height - 1));
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
                gPic.SmoothingMode = SmoothingMode.HighSpeed;
                gPic.DrawRectangle(pen, regSelection);
            }

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;
                int length = 0;
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

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }

                    else if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    }


                    else if (Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                }
                    else
                    {
                        ImageNameEQ[i].Text = "";
                        ImageNameLB[i].Text = "";
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

                    if (Main.Print_Image_Name.Checked)
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

        public void Selected_change_Glass()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            BufferedGraphics graphics; //
            Rectangle regSelection = new Rectangle();
            Graphics gPic;


            
            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                int index =  Selected_Picture_Index[i];
                int pbindex = Selected_Picture_Index[i] - ((cols * rows) * (Current_PageNum - 1));
                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                Pen pen;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    Picture_Glass.ElementAt(pbindex).Image.Dispose();
                    Picture_Glass.ElementAt(pbindex).Image = new Bitmap(width, height);
                    pen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
                    graphics = context.Allocate(CreateGraphics(), new Rectangle(0, 0, Picture_Glass.ElementAt(pbindex).Image.Width - 1, Picture_Glass.ElementAt(pbindex).Image.Height - 1));
                    regSelection.Location = new Point(0, 0);
                    regSelection.Size = new Size(Picture_Glass.ElementAt(pbindex).Image.Width - 1, Picture_Glass.ElementAt(pbindex).Image.Height - 1);
                }

                else
                {
                    Picture_Glass.ElementAt(pbindex).Image.Dispose();
                    Picture_Glass.ElementAt(pbindex).Image = new Bitmap(width, height);

                    pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
                    regSelection.Location = new Point(1, 1);
                    regSelection.Size = new Size(Picture_Glass.ElementAt(pbindex).Image.Width - 3, Picture_Glass.ElementAt(pbindex).Image.Height - 3);


                    //Select_Pic.Add(Picture_Glass.ElementAt(i).Parent.Name);

                }

                gPic = Graphics.FromImage(Picture_Glass.ElementAt(pbindex).Image);
                gPic.SmoothingMode = SmoothingMode.HighSpeed;
                gPic.DrawRectangle(pen, regSelection);
            }

            for (int i = 0; i < Selected_Picture_Index.Count; i++)
            {
                int index = Selected_Picture_Index[i];
                int pbindex = Selected_Picture_Index[i] - ((cols * rows) * (Current_PageNum - 1));
                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;
                int length = 0;
                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[pbindex].ForeColor = Color.Yellow;
                    ImageNameLB[pbindex].ForeColor = Color.Yellow;
                    ImageNameLB[pbindex].BackColor = Color.Black;
                    ImageNameEQ[pbindex].ForeColor = Color.Yellow;
                    ImageNameEQ[pbindex].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[pbindex].Text = temp;
                    else
                        DefectState[pbindex].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[pbindex].Text = dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[pbindex].Location = new Point(4, height - 30);
                        ImageNameEQ[pbindex].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }

                    else if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[pbindex].Location = new Point(4, height - 15);
                        ImageNameLB[pbindex].Text = dicInfo_Filter.Keys.ElementAt(index);
                    }


                    else if (Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[pbindex].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                    {
                        ImageNameEQ[pbindex].Text = "";
                        ImageNameLB[pbindex].Text = "";
                    }


                    PictureData.ElementAt(pbindex).Tag = Color.Yellow;

                }
                else
                {

                    DefectState[pbindex].ForeColor = Color.Red;
                    ImageNameLB[pbindex].ForeColor = Color.Red;
                    ImageNameLB[pbindex].BackColor = Color.Black;
                    ImageNameEQ[pbindex].ForeColor = Color.Red;
                    ImageNameEQ[pbindex].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[pbindex].Text = temp;
                    else
                        DefectState[pbindex].Text = "";

                    if (Main.Print_Image_Name.Checked)
                    {
                        ImageNameLB[pbindex].Location = new Point(4, height - 15);
                        ImageNameLB[pbindex].Text = dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[pbindex].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[pbindex].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[pbindex].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[pbindex].Text = dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[pbindex].Location = new Point(4, height - 30);
                        ImageNameEQ[pbindex].Text = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }


                    PictureData.ElementAt(pbindex).Tag = Color.Red;
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
           // this.Refresh();
        }

        public void ViewMode_Del()
        {
            
            try
            {
                
                
                Get_Delete_IMG();

                if (Main.Frame_View_CB.Checked)
                {
                    for (int i = 0; i < Select_Pic_List.Count; i++)
                    {
                        if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                        {
                            frame_dicInfo_Filter.Remove(Select_Pic_List[i]);
                        }

                    }
                    if (Filter_NO_1 == 1)
                    {
                        Main.No1_Dl_PrintList();
                        Filter_NO_1 = 0;
                    }
                    else
                        Main.Dl_PrintList();

                    Eq_cb_need_del = new List<string>(Select_Pic_List);
                    DL_Frame_Set_View();


                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                    Main.Wait_Del_Print_List();

                }
                else
                {
                    for (int i = 0; i < Select_Pic_List.Count; i++)
                    {
                        if (dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                        {
                            dicInfo_Filter.Remove(Select_Pic_List[i]);
                        }

                    }

                    if (Filter_NO_1 == 1)
                    {
                        Main.No1_Dl_PrintList();
                        Filter_NO_1 = 0;
                    }
                    else
                        Main.Dl_PrintList();


                    Eq_cb_need_del = new List<string>(Select_Pic_List);
                    Del_Set_View();


                    Main.Wait_Del_Print_List();
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);


                }

                Select_Pic_List.Clear();
               
                //Eq_cb_need_del.Clear();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());


            }
        }
       

      /*
        public void ViewMode_Del()
        {
            try
            {
                waitform.Show();
                Get_Delete_IMG();

                if (Main.Frame_View_CB.Checked)
                {
                    for (int i = 0; i < Select_Pic_List.Count; i++)
                    {
                        if (frame_dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                        {
                            frame_dicInfo_Filter.Remove(Select_Pic_List[i]);
                        }

                    }
                    if (Filter_NO_1 == 1)
                    {
                        Main.No1_Dl_PrintList();
                        Filter_NO_1 = 0;
                    }
                    else
                        Main.Dl_PrintList();

                    Eq_cb_need_del = new List<string>(Select_Pic_List);
                    DL_Frame_Set_View();


                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                    Main.Wait_Del_Print_List();

                }
                else
                {
                    for (int i = 0; i < Select_Pic_List.Count; i++)
                    {
                        if (dicInfo_Filter.ContainsKey(Select_Pic_List[i]))
                        {
                            dicInfo_Filter.Remove(Select_Pic_List[i]);
                        }

                    }

                    if (Filter_NO_1 == 1)
                    {
                        Main.No1_Dl_PrintList();
                        Filter_NO_1 = 0;
                    }
                    else
                        Main.Dl_PrintList();


                    Eq_cb_need_del = new List<string>(Select_Pic_List);
                    Del_Set_View();


                    Main.Wait_Del_Print_List();
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);


                }

                Select_Pic_List.Clear();
                waitform.Close();
                //Eq_cb_need_del.Clear();
            }
            catch (Exception ex)
            {
                waitform.Close();
                MessageBox.Show(ex.ToString());


            }
        }
        */
        public void Frame_change_Glass()
        {
            Rectangle regSelection = new Rectangle();
            Graphics gPic;



            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;

                string temp = frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

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
                string temp = frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

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
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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
       
        public void Page_Filter(int page)
        {
            Last_Picture_Selected_Index = -1;
            Current_PageNum = page;
            Main.S_Page_TB.Text = Current_PageNum.ToString();

            //Set_PictureBox();

            if (Main.Frame_View_CB.Checked)
                Frame_Set_Image();
            else
                Set_Image();
        }

        private void ImageViewer_MouseHover(object sender, EventArgs e)
        {
            //Main.Activate();
        }

        public void Frame_Filter(int Frame)
        {

            if (Main.Frame_View_CB.Checked)
            {

                if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) >= int.Parse(Main.Frame_E_Page_TB.Text))
                {
                    MessageBox.Show("마지막 페이지 입니다.");
                }
                else
                {

                    Current_PageNum = 1;
                    Last_Picture_Selected_Index = -1;
                    Current_Frame_PageNum = Frame;
                    Main.Frame_S_Page_TB.Text = Convert.ToString(frame_List_Img.FindIndex(r => r.Equals(Frame)) + 1);

                    Frame_Filter_check = 1;

                    Frame_Set_Image();



                }
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
            }
            else
            {
                if (Main.Frame_Interval_CB.Checked)
                {
                    if (Main.FrameFilterReseve.Checked)
                    {
                        foreach (string No in Sorted_dic.Keys.ToList())
                        {
                            if (Sorted_dic.ContainsKey(No))
                            {
                                if (Sorted_dic[No].FrameNo >= Frame && Sorted_dic[No].FrameNo <= int.Parse(Main.Frame_E_TB.Text))
                                {
                                    Sorted_dic.Remove(No);
                                }
                                else
                                {
                                   
                                }

                            }
                        }
                    }
                    else
                    {
                        foreach (string No in Sorted_dic.Keys.ToList())
                        {
                            if (Sorted_dic.ContainsKey(No))
                            {
                                if (Sorted_dic[No].FrameNo >= Frame && Sorted_dic[No].FrameNo <= int.Parse(Main.Frame_E_TB.Text))
                                {

                                }
                                else
                                {
                                    Sorted_dic.Remove(No);
                                }

                            }
                        }
                    }
                    Main.Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(Sorted_dic);
                    //DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
                }
                else
                {
                    if (Main.Eq_CB_dicInfo.Count > 0)
                    {
                        Sorted_dic = new Dictionary<string, ImageInfo>(Main.Filter_CheckEQ_Dic);
                        if (Main.FrameFilterReseve.Checked)
                        {
                            foreach (string No in Sorted_dic.Keys.ToList())
                            {
                                if (Sorted_dic.ContainsKey(No))
                                {
                                    if (Sorted_dic[No].FrameNo == Frame)
                                    {
                                        Sorted_dic.Remove(No);
                                    }
                                    else
                                    {
                                        
                                    }

                                }
                            }
                        }
                        else
                        {
                            foreach (string No in Sorted_dic.Keys.ToList())
                            {
                                if (Sorted_dic.ContainsKey(No))
                                {
                                    if (Sorted_dic[No].FrameNo == Frame)
                                    {

                                    }
                                    else
                                    {
                                        Sorted_dic.Remove(No);
                                    }

                                }
                            }
                        }
                        Main.Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(Sorted_dic);
                        //DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
                    }
                    else
                    {
                        if (Main.FrameFilterReseve.Checked)
                        {
                            foreach (string no in Main.Filter_CheckEQ_Dic.Keys.ToList())
                            {
                                if (Main.Filter_CheckEQ_Dic[no].FrameNo == Frame)
                                {
                                    Main.Filter_CheckEQ_Dic.Remove(no);
                                }
                                else
                                {
                                    
                                }
                            }
                        }
                        else
                        {

                            foreach (string no in Main.Filter_CheckEQ_Dic.Keys.ToList())
                            {
                                if (Main.Filter_CheckEQ_Dic[no].FrameNo == Frame)
                                {
                                    // FrameDic.Add(no, Main.DicInfo[no]);
                                }
                                else
                                {
                                    Main.Filter_CheckEQ_Dic.Remove(no);
                                }
                            }
                        }
                       // Main.Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(FrameDic);
                    }
                }
                

               // Frame_Filter_check = 1;

                ///Set_View();
                //Main.Print_List();
            }
            //Frame_Filter_check = 0;
        }

        public void Frame_Interval_Filter(List<int> inter)
        {
                
                Dictionary<string, ImageInfo> interval_dic = new Dictionary<string, ImageInfo>(Main.DicInfo);
      
                foreach (string no in interval_dic.Keys.ToList())
                {
                    if (inter.Contains(interval_dic[no].FrameNo))
                    {

                    }
                    else
                    {
                        interval_dic.Remove(no);
                    }
                }
                DicInfo_Filtered = new Dictionary<string, ImageInfo>(interval_dic);
                Frame_Filter_check = 1;

                Set_View();
                Main.Print_List();
                interval_dic.Clear();
                Frame_Filter_check = 0;
        }


        public void No_Frmae_Filter(int Frame)
        {
            if (Main.Frame_View_CB.Checked)
            {

               

                    Current_PageNum = 1;
                    Last_Picture_Selected_Index = -1;
                    Current_Frame_PageNum = Frame;
                    Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();

                    Frame_Set_Image();



                
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
            }
            else
                MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
        }

        public void Cheked_State_DF()
        {
            int length = 0;
            if (Main.Frame_BT.Checked)
            {
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                dicInfo_Filter = Sorted_dic;
            }
            else
            {
                
            }
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
      
        public void Frame_Cheked_State_DF()
        {
            //if(Main.Return_dicInfo.Count > 0)
            //{
            //    Frame_dicInfo_Filter = new Dictionary<string, ImageInfo>(Main.Return_dicInfo);
            //    Main.Return_dicInfo.Clear();
            //}

            Sorted_dic = Frame_dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Frame_dicInfo_Filter = Sorted_dic;

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;
                int length = 0;
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
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

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

                    if (Main.Print_Image_Name.Checked && Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                        ImageNameLB[i].Location = new Point(4, height - 30);
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    if (Main.Print_Image_Name.Checked && !Main.Print_Image_EQ.Checked)
                    {
                        ImageNameLB[i].Location = new Point(4, height - 15);
                        ImageNameLB[i].Text = Frame_dicInfo_Filter.Keys.ElementAt(index);
                    }
                    else
                    {
                        ImageNameLB[i].Text = "";

                    }

                    if (Main.Print_Image_EQ.Checked)
                    {
                        length = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Length;
                        ImageNameEQ[i].Text = Frame_dicInfo_Filter[Frame_dicInfo_Filter.Keys.ElementAt(index)].Imagename.Substring(13, length - 13);

                    }
                    else
                        ImageNameEQ[i].Text = "";

                    

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Main.Eng_dicinfo.Count > 0)
            {
                Set_Image_Eng();
            }
            else if (Main.Frame_View_CB.Checked)
                Frame_Set_Image();
            else
                Set_Image();

            timer1.Stop();
        }

        private void PictureBox_MouseHover(object sender, EventArgs e)
        {
            Main.Activate();
            try
            {
                Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                Dictionary<string, ImageInfo> xy_Location = new Dictionary<string, ImageInfo>();
                if (expand_ImgInfo.Count > 0)
                {
                    xy_Location.Add(expand_ImgInfo.Keys.ElementAt(0), expand_ImgInfo[expand_ImgInfo.Keys.ElementAt(0)]);
                    Main.MouseXY_FT_X.Text = xy_Location[xy_Location.ElementAt(0).Key].X_Location;
                    Main.MouseXY_FT_Y.Text = xy_Location[xy_Location.ElementAt(0).Key].Y_Location;
                    Main.M_TB.Text = xy_Location[xy_Location.ElementAt(0).Key].Master_NO;
                }
            }
            catch { }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            Main.Activate();
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
            ImageViewer_PL_MouseMove(sender, e);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {

            ImageViewer_PL_MouseUp(sender, e);
            Draged_PB = null;
        }

        public void ImageViewer_Clear()
        {
            Eq_cb_need_del.Clear();
            F12_del_list.Clear();
        }

        public ImageViewer()
        {
            InitializeComponent();
            
        }
        public class DoubleBufferPicture : PictureBox
        {
            public DoubleBufferPicture()
            {
               // this.FunctionalMode = FunctionalModeOption.Minimum;
                this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
                this.UpdateStyles();
             //   this.Paint += FPaint;


            }
            private void FPaint(object sender, PaintEventArgs e)
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new PaintEventHandler(this.FPaint), new object[] { sender, e });
                    return;
                }


                using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.ClientRectangle))
                {
                    bufferedgraphic.Graphics.Clear(Color.Silver);
                    bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollOffset.X, this.AutoScrollOffset.Y);

                    Pen p = new Pen(Color.FromArgb(111, 91, 160), 3);
                    bufferedgraphic.Graphics.DrawLine(p, 0, 0, 100, 100);
                    p.Dispose();

                    bufferedgraphic.Render(e.Graphics);
                }



            }
        }
        public void EngrMode_Data()
        {
            
        }
        public class DoubleBufferLabel : Label
        {
            public DoubleBufferLabel()
            {
                
                this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
                this.UpdateStyles();
                this.Paint += FPaint;
            }
            private void FPaint(object sender, PaintEventArgs e)
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new PaintEventHandler(this.FPaint), new object[] { sender, e });
                    return;
                }


                



            }
        }
        
    }

}

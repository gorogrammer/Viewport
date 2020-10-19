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

namespace ViewPort.Views
{
    public partial class ImageViewer : UserControl
    {
        public FormViewPort Main;
        LoadingGIF_Func waitform = new LoadingGIF_Func();

        string openViewType = string.Empty; 
        struct BoxRange { public int left, top, width, height; }
        List<string> Display_Id = new List<string>();
        List<string> Change_state_List = new List<string>();
        List<PictureBox> PictureData = new List<PictureBox>();
        List<PictureBox> Picture_Glass = new List<PictureBox>();
        List<string> Select_Pic = new List<string>();
        List<string> Eq_cb_need_del = new List<string>();
        List<int> frame_List_Img = new List<int>();
        Dictionary<string, ImageInfo> dicInfo_Filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Delete = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> frame_dicInfo_Filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> expand_ImgInfo = new Dictionary<string, ImageInfo>();

        Dictionary<string, ImageInfo> Before_No1_Filter_dicInfo = new Dictionary<string, ImageInfo>();
        int setting = 0;
        List<int> apply_List_opne = new List<int>();
        List<int> Notapply_List_opne = new List<int>();

        int Filter_NO_1 = 0;
        int Filter_F9 = 0;
        int Filter_F10 = 0;
        int Filter_F5 = 0;
        int Filter_F = 0;
        Image expand_img = null;
        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        List<string> Print_Frame = new List<string>();
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        Label[] DefectState, ImageNameLB, ImageNameEQ;
        int Current_PageNum, Total_PageNum;
        int Current_Frame_PageNum, Total_Frame_PageNum;
        int Last_Picture_Selected_Index;       
        int EachPage_ImageNum;
        List<string> imglist = new List<string>();
        int Frame_Filter_check = 0;
        Point src_Mouse_XY, dst_Mouse_XY;
        Point A_Mouse_XY, B_Mouse_XY;

        int cols, rows, width, height;

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
        public string OpenViewType { get => openViewType; set => openViewType = value; }
        public Dictionary<string, ImageInfo> Frame_dicInfo_Filter { get => frame_dicInfo_Filter; set => frame_dicInfo_Filter = value; }

        public void SelectGrid_Img_View(string id)
        {
            List<string> dic_index_List = dicInfo_Filter.Keys.ToList();

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);

            int index = dic_index_List.IndexOf(id);
            double quotient = (index / (cols * rows));
            int view_page = Convert.ToInt32(System.Math.Truncate(quotient));

            Last_Picture_Selected_Index = -1;
            Current_PageNum = view_page + 1;
            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Set_Image();
        }
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

                   

                    if (Main.Frame_View_CB.Checked)
                        Frame_Set_Image();
                    else
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

                    //Set_PictureBox();

                    if (Main.Frame_View_CB.Checked)
                        Frame_Set_Image();
                    else
                        Set_Image();

                }
            }

            else if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    waitform.Show();
                    Get_Delete_IMG();

                    if (Main.Frame_View_CB.Checked)
                    {
                        for (int i = 0; i < Select_Pic.Count; i++)
                        {
                            if (frame_dicInfo_Filter.ContainsKey(Select_Pic[i]))
                            {
                                frame_dicInfo_Filter.Remove(Select_Pic[i]);
                            }

                        }
                        if (Filter_NO_1 == 1)
                        {
                            Main.No1_Dl_PrintList();
                            Filter_NO_1 = 0;
                        }
                        else
                            Main.Dl_PrintList();

                        Eq_cb_need_del = new List<string>(Select_Pic);
                        DL_Frame_Set_View();


                        Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                        Main.Wait_Del_Print_List();

                    }
                    else
                    {
                        for (int i = 0; i < Select_Pic.Count; i++)
                        {
                            if (dicInfo_Filter.ContainsKey(Select_Pic[i]))
                            {
                                dicInfo_Filter.Remove(Select_Pic[i]);
                            }

                        }

                        if (Filter_NO_1 == 1)
                        {
                            Main.No1_Dl_PrintList();
                            Filter_NO_1 = 0;
                        }
                        else
                            Main.Dl_PrintList();


                        Eq_cb_need_del = new List<string>(Select_Pic);
                        Del_Set_View();


                        Main.Wait_Del_Print_List();
                        Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);


                    }

                    Select_Pic.Clear();
                    waitform.Close();
                    //Eq_cb_need_del.Clear();
                }
                catch
                {

                }

               
            }

            else if (e.KeyCode == Keys.Z)
            {
                if (Main.Frame_View_CB.Checked)
                {
                    e.Handled = true;
                    if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) <= 1)
                    {
                        MessageBox.Show("첫 페이지 입니다.");
                    }
                    else
                    {


                        Last_Picture_Selected_Index = -1;
                        Current_Frame_PageNum = int.Parse(Main.Frame_S_Page_TB.Text) - 1;
                        Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();
                       
                        Frame_Set_Image();



                    }

                }
                else
                    MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
            }

            else if (e.KeyCode == Keys.X)
            {
                if (Main.Frame_View_CB.Checked)
                {
                    e.Handled = true;
                    if (Main.Frame_S_Page_TB.Text == "" || int.Parse(Main.Frame_S_Page_TB.Text) >= int.Parse(Main.Frame_E_Page_TB.Text))
                    {
                        MessageBox.Show("마지막 페이지 입니다.");
                    }
                    else
                    {

                        Current_PageNum = 1;
                        Last_Picture_Selected_Index = -1;
                        Current_Frame_PageNum = int.Parse(Main.Frame_S_Page_TB.Text) + 1;
                        Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();
                       
                        Frame_Set_Image();



                    }
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
                }
                else
                    MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");

            }

            else if (e.KeyCode == Keys.L)
            {
                DL_ViewFrom DL = new DL_ViewFrom(Main);
                DL.Dl_LIst_ADD(Main.Dl_List_Main);
                DL.Show();


            }

            else if (e.KeyCode == Keys.F11)
            {
                try
                {
                    waitform.Show();
                    Before_No1_Filter_dicInfo = new Dictionary<string, ImageInfo>(DicInfo_Filtered);
                    DicInfo_Filtered = Main.Sdip_NO1_dicInfo;
              

                    Filter_NO_1 = 1;
                    Set_View();
                    Main.Print_List();
                    waitform.Close();
                }
                catch { }
         
                

            }



            else if (e.Shift && e.KeyCode == Keys.A)
            {
                try
                {
                    waitform.Show();
                    if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                    {
                        foreach (KeyValuePair<string, ImageInfo> pair in Frame_dicInfo_Filter)
                        {
                            pair.Value.ReviewDefectName = "선택";
                        }
                        Select_Pic_List = Frame_dicInfo_Filter.Keys.ToList();


                        Frame_Set_View();
                    }
                    else
                    {
                        foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)
                        {
                            pair.Value.ReviewDefectName = "선택";
                        }
                        Select_Pic_List = dicInfo_Filter.Keys.ToList();

                        Set_Image();
                    }

                    Main.ALL_Changeed_State();
                    waitform.Close();
                }
                catch { }
                
            }

            else if (e.KeyCode == Keys.A)
            {

                Select_Pic_List.Clear();

                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();


                if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                {
                    for (int i = 0; i < (cols * rows); i++)
                    {
                        if ((index + i) >= frame_dicInfo_Filter.Count)
                            break;
                        Selected_Picture_Index.Add(index + i);
                    }

                    for (int p = 0; p < Selected_Picture_Index.Count; p++)
                    {
                        frame_dicInfo_Filter[frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "선택";
                        Select_Pic_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                        Change_state_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                    }
                    Frame_Set_View();
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
                        dicInfo_Filter[dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "선택";
                        Select_Pic_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                        Change_state_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                    }

                    Set_Image();
                }

                Main.Changeed_State();


            }

            else if (e.Shift && e.KeyCode == Keys.G)
            {
                Select_Pic_List.Clear();

                foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)
                {
                    pair.Value.ReviewDefectName = "양품";
                }


                Set_Image();
                Main.ALL_Changeed_State();


            }

            else if (e.KeyCode == Keys.G)
            {
                Select_Pic_List.Clear();

                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();


                
                if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                {
                    for (int i = 0; i < (cols * rows); i++)
                    {
                        if ((index + i) >= frame_dicInfo_Filter.Count)
                            break;
                        Selected_Picture_Index.Add(index + i);
                    }

                    for (int p = 0; p < Selected_Picture_Index.Count; p++)
                    {
                        frame_dicInfo_Filter[frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "양품";

                        Change_state_List.Add(frame_dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                    }
                    Frame_Set_View();
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
                        dicInfo_Filter[dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key].ReviewDefectName = "양품";

                        Change_state_List.Add(dicInfo_Filter.ElementAt(Selected_Picture_Index[p]).Key);
                    }

                    Set_Image();
                }


                Main.Changeed_State();


            }



            else if (e.KeyCode == Keys.F9)
            {
                if (Main.F9_code_dicInfo.Count > 0)
                {
                    DicInfo_Filtered = Main.F9_code_dicInfo;

                    Filter_F9 = 1;
                    Set_View();
                    Main.Print_List();
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count); 
                    
                }
                else
                {
                    MessageBox.Show("Limit 아래 부품이 없습니다.");
                }



            }

            else if (e.KeyCode == Keys.F10)
            {

                if (Main.F10_code_dicInfo.Count > 0)
                {
                    DicInfo_Filtered = Main.F10_code_dicInfo;

                    Filter_F10 = 1;
                    Set_View();
                    Main.Print_List();
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);

                }
                else
                    MessageBox.Show("Limit 초과 부품이 없습니다.");

            }

            else if (e.KeyCode == Keys.R)
            {

                //A_Mouse_XY = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));

                ExpandImage expand = new ExpandImage(this);
                Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                expand.Expand_ImgInfo.Add(expand_ImgInfo.Keys.ElementAt(0), expand_ImgInfo[expand_ImgInfo.Keys.ElementAt(0)]);
                expand.Set_Expand_Img(expand_img);


                expand.ShowDialog();

            }

            else if (e.KeyCode == Keys.F)
            {
                Filter_F = 1;

                XYLocationFilter xyFilter = new XYLocationFilter(this);
                Expand_Find_Contain_PB(A_Mouse_XY, A_Mouse_XY);
                xyFilter.XY_Location.Add(expand_ImgInfo.Keys.ElementAt(0), expand_ImgInfo[expand_ImgInfo.Keys.ElementAt(0)]);

                if (Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                {
                    xyFilter.DicInfo_XY_filter = frame_dicInfo_Filter;
                }
                else
                {
                    xyFilter.DicInfo_XY_filter = DicInfo_Filtered;
                }

                xyFilter.Set_XY_TB();
                xyFilter.ShowDialog();
                
            }

            else if (e.KeyCode == Keys.F5)
            {

                if (Main.F5_code_dicInfo.Count > 0)
                {
                    DicInfo_Filtered = Main.F5_code_dicInfo;

                    Filter_F5 = 1;
                    Set_View();
                    Main.Print_List();
                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);

                }
                else
                    MessageBox.Show("221, 222 code 부품이 없습니다.");

            }


            else if (e.KeyCode == Keys.F12)
            {
                //Get_Delete_IMG();

                if (Main.Frame_View_CB.Checked)
                {
                    for (int i = 0; i < Select_Pic.Count; i++)
                    {
                        if (frame_dicInfo_Filter.ContainsKey(Select_Pic[i]))
                        {
                            frame_dicInfo_Filter.Remove(Select_Pic[i]);
                        }

                    }
                    Main.Dl_PrintList();
                    DL_Frame_Set_View();


                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);

                    Eq_cb_need_del = new List<string>(Select_Pic);
                }
                else
                {
                    for (int i = 0; i < Select_Pic.Count; i++)
                    {
                        if (dicInfo_Filter.ContainsKey(Select_Pic[i]))
                        {
                            dicInfo_Filter.Remove(Select_Pic[i]);
                        }

                    }

                    Main.Dl_PrintList();
                    Del_Set_View();



                    Main.List_Count_TB.Text = String.Format("{0:#,##0}", dicInfo_Filter.Count);

                    Eq_cb_need_del = new List<string>(Select_Pic);
                }





                Select_Pic.Clear();
            }
            else if (e.KeyCode == Keys.F5)
            {

            }

        }



        public ImageViewer(FormViewPort mainForm)
        {
            InitializeComponent();

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
            }
            Del_Set_View();

        }

        public void XY_Filter_Set(Dictionary<string,ImageInfo> dic)
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
            if (Main.Frame_View_CB.Checked)
            {
                for (int p = 0; p < Select_Pic.Count; p++)
                {
                    if (frame_dicInfo_Filter.ContainsKey(Select_Pic[p]))
                    {

                        dicInfo_Delete.Add(Select_Pic[p], frame_dicInfo_Filter[Select_Pic[p]]);

                        dicInfo_Delete[Select_Pic[p]].DeleteCheck = "삭제대기";
                    }
                }
            }
            else
            {
                for (int p = 0; p < Select_Pic.Count; p++)
                {
                    if (dicInfo_Filter.ContainsKey(Select_Pic[p]))
                    {

                        dicInfo_Delete.Add(Select_Pic[p], dicInfo_Filter[Select_Pic[p]]);

                        dicInfo_Delete[Select_Pic[p]].DeleteCheck = "삭제대기";
                    }
                }
            }

            Main.Waiting_Del = dicInfo_Delete;
        }

        private void ImageViewer_PL_MouseDown(object sender, MouseEventArgs e)
        {
            //Main.SetFocus();
            src_Mouse_XY.X = e.X;
            src_Mouse_XY.Y = e.Y;

            if (Draged_PB != null)
            {
                src_Mouse_XY.X += Draged_PB.Location.X;
                src_Mouse_XY.Y += Draged_PB.Location.Y;
            }
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
        }
        public void Set_View()
        {
            Main.ViewType = "SetView";
            OpenViewType = "SetView";

            this.Controls.Clear();
            PictureData.Clear();

            if (!Main.Exceed_CB.Checked &&Filter_NO_1 != 1 && Filter_F9 != 1 && Filter_F10 != 1 && Filter_F5 != 1 && Filter_F !=1)
            {
                //frame_List_Img = Main.Frame_List_Main;
                dicInfo_Filter = Main.DicInfo;
                
            }
            else if(Main.Exceed_CB.Checked)
            {
                dicInfo_Filter = Main.Exceed_filter;

            }
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            DicInfo_Filtered = Sorted_dic;

            if (Main.Camera_NO_Filter_TB.Text != "")
            {
                string[] Split_String = null;
                Split_String = Main.Camera_NO_Filter_TB.Text.Split(',');
                bool Target = false;
                
                foreach (string No in DicInfo_Filtered.Keys.ToList())
                {
                    if(DicInfo_Filtered.ContainsKey(No))
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

            if(Main.Waiting_Del.Count > 0)
            {
                foreach(string pair in DicInfo_Filtered.Keys.ToList())
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

            Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);

            
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
        public void Frame_Set_View()
        {

            Main.ViewType = "FrameSetView";
            OpenViewType = "FrameSetView";

            this.Controls.Clear();
            PictureData.Clear();

            if(Main.Exceed_CB.Checked)
            {
                Frame_List_Img = Main.Exceed_List;
            }
            else
            {
                if (Main.mAP_LIST.Count > 0)
                    Frame_List_Img = Main.mAP_LIST;
                else
                    Frame_List_Img = Main.Frame_List_Main;
            }
           


            if (Main.Eq_CB_dicInfo.Count > 0)
            {
                dicInfo_Filter = Main.Eq_CB_dicInfo;
                Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

                DicInfo_Filtered = Sorted_dic;
                Frame_List_Img.Clear();
                foreach (KeyValuePair<string,ImageInfo> pair in DicInfo_Filtered)
                {
                    

                    if(Frame_List_Img.Contains(pair.Value.FrameNo))
                    {
                    }
                    else
                    {
                        Frame_List_Img.Add(pair.Value.FrameNo);
                    }
                }
            }
            else if(Filter_F ==1)
            {

            }
            else
            {
                dicInfo_Filter = Main.DicInfo;
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
                frame_List_Img.RemoveAt(Current_Frame_PageNum-1);
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
            
            for(int i = 0;  i < Eq_cb_need_del.Count; i++)
            {
                DicInfo_Filtered.Remove(Eq_cb_need_del[i]);
            }

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

            Main.List_Count_TB.Text = String.Format("{0:#,##0}", DicInfo_Filtered.Count);
            this.Focus();
        }

        public void Eq_CB_Set_View_ING()
        {

            Main.ViewType = "EQCBSetView_ING";
            OpenViewType = "EQCBSetView_ING";

            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = Main.Return_dicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;
            Eq_cb_need_del = Main.selected_Pic;

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
            A_Mouse_XY = this.PointToClient(Cursor.Position);
        }

        private void ImageViewer_PL_MouseUp(object sender, MouseEventArgs e)
        {
            if(Setting ==1)
            {
                Main.SetFocus();
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

                if (Main.Frame_View_CB.Checked)
                    Frame_Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);
                else
                    Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);






                if (Main.Frame_View_CB.Checked)
                    Frame_change_Glass();
                else
                    change_Glass();

                if (Change_state_List.Count > 0)
                    Main.Changeed_State();

                //src_Mouse_XY.X = -1;
                //src_Mouse_XY.Y = -1;
                //dst_Mouse_XY.X = -1;
                //dst_Mouse_XY.Y = -1;

                src_Mouse_XY = Point.Empty;
                dst_Mouse_XY = Point.Empty;
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
                        Select_Pic.Add(dicInfo_Filter.Keys.ElementAt(index + i));
                    }
                }
                    
                        
            }
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
                    for(int p = 0; p < Select_Pic.Count; p++)
                    {
                        if (Select_Pic[p].Equals(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic.RemoveAt(p);
                            p--;
                        }
                    }
                    
                }
                    

                dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)].ReviewDefectName = result;
                Change_state_List.Add(dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index));

            }

            
        }
        private void Expand_Find_Contain_PB(Point Src, Point Dst)
        {
            Rectangle PB_Area, Drag_Area;
            
            int index = ((Current_PageNum - 1) * (cols * rows));

            expand_ImgInfo.Clear();
            expand_img = null;
            if (Selected_Picture_Index.Count > 0)
                Selected_Picture_Index.Clear();

            for (int i = 0; i < PictureData.Count; i++)
            {
                PB_Area = new Rectangle(PictureData.ElementAt(i).Left, PictureData.ElementAt(i).Top, PictureData.ElementAt(i).Width, PictureData.ElementAt(i).Height);
                Drag_Area = new Rectangle(Src.X, Src.Y, Dst.X - Src.X, Dst.Y - Src.Y);
                

                if(Main.ViewType == "FrameSetView" || Main.ViewType == "DLFrameSetView")
                {
                    if (PB_Area.IntersectsWith(Drag_Area))
                    {
                        if (frame_dicInfo_Filter.Count > (index + i))
                        {
                            expand_img = PictureData[index + i].Image;
                            expand_ImgInfo.Add(PictureData[index + i].Name, frame_dicInfo_Filter[PictureData[index + i].Name]);

                        }
                    }
                }
                else
                {
                    if (PB_Area.IntersectsWith(Drag_Area))
                    {
                        if (dicInfo_Filter.Count > (index + i))
                        {
                            expand_img = PictureData[index + i].Image;
                            expand_ImgInfo.Add(PictureData[index + i].Name, dicInfo_Filter[PictureData[index + i].Name]);

                        }
                    }
                }
                                
            }
            

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
                        Select_Pic.Add(frame_dicInfo_Filter.Keys.ElementAt(index + i));
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
                        if (Select_Pic[p].Equals(frame_dicInfo_Filter.Keys.ElementAt(Last_Picture_Selected_Index)))
                        {
                            Select_Pic.RemoveAt(p);
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
                temp_PB = new PictureBox();
                temp_PB.Click += new EventHandler(PictureBox_Click);
                temp_PB.MouseHover += PictureBox_MouseHover;
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
                temp_PB.MouseHover += PictureBox_MouseHover;

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
                this.Controls.Add(PictureData.ElementAt(i));

            }

            Last_Picture_Selected_Index = -1;
            Current_PageNum = (pre_cols * pre_rows * (Current_PageNum - 1)) / (cols * rows) + 1;
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Main.E_Page_TB.Text = Total_PageNum.ToString();


        }

        public void Set_Image()
        {
            Bitmap tmp_Img = null;

            string Current_ImageFrame = "";
            int S_ImageIndex = (cols * rows) * (Current_PageNum - 1);
            int PF_index = 0, Current_Index = 0;
            EachPage_ImageNum = cols * rows;

            if (imglist.Count > 0)
                imglist.Clear();

            imglist = dicInfo_Filter.Keys.ToList();

            if (dicInfo_Filter.Count <= 0)
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

            if (dicInfo_Filter.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += dicInfo_Filter.Count - ((cols * rows) * Current_PageNum);

            if (Print_Frame.Count > 0)
            {
                Print_Frame.Clear();
                Print_Frame = new List<string>();
            }

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                Current_ImageFrame = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + i).Substring(1, 5);

                if (!Print_Frame.Contains(Current_ImageFrame))
                    Print_Frame.Add(Current_ImageFrame);
            }
            Print_Frame.Sort();


            if (Main.ZipFilePath != "")
            {
                zip = ZipFile.Open(Main.ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
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


                        var sub =
                                            from ent in subZip.Entries
                                            orderby ent.Name
                                            select ent;


                        foreach (ZipArchiveEntry subEntry in sub)       // 2중 압축파일 내에 있는 파일을 탐색
                        {
                            if (dicInfo_Filter.ContainsKey(subEntry.Name.Substring(0, 12)))
                            {

                            }
                            if (Current_Index >= EachPage_ImageNum)
                                break;
                            if (subEntry.Name.Equals(dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index)].Imagename + ".jpg"))  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
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
                                PictureData.ElementAt(Current_Index).Name = dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index);

                                Current_Index++;
                            }

                            if (Current_Index >= EachPage_ImageNum)
                                break;
                            if (!dicInfo_Filter.Keys.ElementAt(S_ImageIndex + Current_Index).Substring(1, 5).Equals(Print_Frame.ElementAt(PF_index)))
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



            change_Glass();


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

           if(Frame_Filter_check ==1)
            {
                Main.Frame_S_TB.Text = Main.Frame_S_TB.Text;
                Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
                Frame_List_Index = frame_List_Img.FindIndex(r=>r.Equals(int.Parse(Main.Frame_S_TB.Text)));
                Frame_Filter_check = 0;
            }
           else
            {
                Main.Frame_S_TB.Text = frame_List_Img[Frame_List_Index].ToString();
                Main.Frame_E_TB.Text = Main.Frame_S_TB.Text;
            }
            

            frame_dicInfo_Filter.Clear();

            foreach (KeyValuePair<string, ImageInfo> kvp in dicInfo_Filter)
            {
                if (kvp.Value.FrameNo == frame_List_Img[Frame_List_Index])
                    if (frame_dicInfo_Filter.ContainsKey(kvp.Key))
                        continue;
                    else
                        frame_dicInfo_Filter.Add(kvp.Key, kvp.Value);
               
            }

            foreach(string pair in frame_dicInfo_Filter.Keys.ToList())
            {
                if (frame_dicInfo_Filter[pair].FrameNo == frame_List_Img[Frame_List_Index])
                    continue;
                else
                    frame_dicInfo_Filter.Remove(pair);
            }
            if(DicInfo_Delete.Count > 0)
            {
                foreach(string pair in frame_dicInfo_Filter.Keys.ToList())
                {
                    if(DicInfo_Delete.ContainsKey(pair))
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

            var Sorted_frame_dic =
                                                from ent in frame_dicInfo_Filter
                                                orderby ent.Key
                                                select ent;

            frame_dicInfo_Filter = Sorted_frame_dic.ToDictionary(pair=>pair.Key, pair => pair.Value);

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((frame_dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();


            if (frame_dicInfo_Filter.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += frame_dicInfo_Filter.Count - ((cols * rows) * Current_PageNum);

            if (Main.ZipFilePath != "")
            {
                zip = ZipFile.Open(Main.ZipFilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load
                string Open_ZipName;

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    Open_ZipName = entry.Name.Split('.')[0];
                    if (Open_ZipName[0].Equals('R'))
                        Open_ZipName = Open_ZipName.Substring(1, Open_ZipName.Length - 1);
                    
                    if(Main.Frame_View_CB.Checked)
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
  
        public void change_Glass()
        {
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
                    Main.Frame_S_Page_TB.Text = Current_Frame_PageNum.ToString();

                    Frame_Filter_check = 1;

                    Frame_Set_Image();

                   

                }
                Main.List_Count_TB.Text = String.Format("{0:#,##0}", frame_dicInfo_Filter.Count);
            }
            else
                MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
        }

        public void Cheked_State_DF()
        {
            int length = 0;
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
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            this.Focus();
            MouseEventArgs MouseEvent = (MouseEventArgs)e;
            PictureBox PB = (PictureBox)sender;

            int index = ((Current_PageNum - 1) * (cols * rows));
            int EachPage_ImageNum = cols * rows;

            if (dicInfo_Filter.Count - ((cols * rows) * Current_PageNum) < 0)
                EachPage_ImageNum += dicInfo_Filter.Count - ((cols * rows) * Current_PageNum);

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

        private void PictureBox_MouseHover(object sender, EventArgs e)
        {
            Main.Activate();
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
            ImageViewer_PL_MouseMove(sender, e);
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            
            ImageViewer_PL_MouseUp(sender, e);
            Draged_PB = null;
        }

        public ImageViewer()
        {
            InitializeComponent();
            
        }
    }
}

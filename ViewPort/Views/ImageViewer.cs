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
        int Filter_NO_1 = 0;

        List<BoxRange> ImageRangeInfo = new List<BoxRange>();
        List<string> Print_Frame = new List<string>();
        List<int> Selected_Picture_Index = new List<int>();
        PictureBox Draged_PB;
        Label[] DefectState, ImageNameLB;
        int Current_PageNum, Total_PageNum;
        int Current_Frame_PageNum, Total_Frame_PageNum;
        int Last_Picture_Selected_Index;        // -3 : 리스트 전체 선택,  -2 : 다중 선택, -1 : 미선택, 그 외, 선택한 Image Index
        int EachPage_ImageNum;
        List<string> imglist = new List<string>();

        Point src_Mouse_XY, dst_Mouse_XY;

     
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

                    Set_PictureBox();

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
                    
                    Set_PictureBox();

                    if (Main.Frame_View_CB.Checked)
                        Frame_Set_Image();
                    else
                        Set_Image();

                   


                }
            }

            else if (e.KeyCode == Keys.Delete)
            {
                Get_Delete_IMG();

                for (int i = 0; i < Select_Pic.Count; i++)
                {
                    if (dicInfo_Filter.ContainsKey(Select_Pic[i]))
                    {
                        dicInfo_Filter.Remove(Select_Pic[i]);
                    }

                }
               
                Del_Set_View();
                Main.Dl_PrintList();
                Main.Wait_Del_Print_List();
                Eq_cb_need_del = new List<string>(Select_Pic);
                
                Select_Pic.Clear();
                Main.List_Count_TB.Text = dicInfo_Filter.Count.ToString();
                //Eq_cb_need_del.Clear();
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
                        Set_PictureBox();
                        Frame_Set_Image();



                    }

                }
                else
                    MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
                Main.List_Count_TB.Text = frame_dicInfo_Filter.Count.ToString();
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
                        Set_PictureBox();
                        Frame_Set_Image(); 
                        


                    }

                }
                else
                    MessageBox.Show("Frame 별 체크 후에 사용 부탁드립니다.");
                Main.List_Count_TB.Text = frame_dicInfo_Filter.Count.ToString();
            }

            else if (e.KeyCode == Keys.L)
            {
                DL_ViewFrom DL = new DL_ViewFrom(Main);
                DL.Dl_LIst_ADD(Main.Dl_List_Main);
                DL.Show();
               

            }

            else if (e.KeyCode == Keys.F11)
            {
                foreach(string NO_1 in dicInfo_Filter.Keys.ToList())
                {
                    if (dicInfo_Filter[NO_1].sdip_no == "1")
                        continue;
                    else
                    {
                        dicInfo_Filter.Remove(NO_1);
                        Select_Pic_List.Add(NO_1);
                    }
                        
                    
                }

                Filter_NO_1 = 1;
                Set_View();
                Main.Filter_NO_1_PrintList();
            }

        

            else if (e.Shift && e.KeyCode == Keys.A)
            {
                foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Filter)
                {
                    pair.Value.ReviewDefectName = "불량";
                }
                Select_Pic_List = dicInfo_Filter.Keys.ToList();
                
                Set_Image();
                Main.ALL_Changeed_State();
            }

            else if ( e.KeyCode == Keys.A)
            {

                Select_Pic_List.Clear();

                int index = ((Current_PageNum - 1) * (cols * rows));
                Selected_Picture_Index.Clear();

                for(int i =0; i < (cols * rows); i++)
                {
                    if ((index + i) >= dicInfo_Filter.Count)
                        break;
                    Selected_Picture_Index.Add(index + i);
                }
                
                for(int p =0; p < Selected_Picture_Index.Count; p++)
                {
                    dicInfo_Filter[dicInfo_Filter.ElementAt(p).Key].ReviewDefectName =  "불량";
                    Select_Pic_List.Add(dicInfo_Filter.ElementAt(p).Key);
                    Change_state_List.Add(dicInfo_Filter.ElementAt(p).Key);
                }

                Set_Image();
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

                for (int i = 0; i < (cols * rows); i++)
                {
                    if ((index + i) >= dicInfo_Filter.Count)
                        break;
                    Selected_Picture_Index.Add(index + i);
                }

                for (int p = 0; p < Selected_Picture_Index.Count; p++)
                {
                    dicInfo_Filter[dicInfo_Filter.ElementAt(p).Key].ReviewDefectName = "양품";
                    
                    Change_state_List.Add(dicInfo_Filter.ElementAt(p).Key);
                }

                Set_Image();
                Main.Changeed_State();


            }

            

            else if (e.KeyCode == Keys.F9)
            {
                

                if (Main.Dl_Apply_List_Main.Count > 0)
                {
                    dicInfo_Filter.Clear();
                    Select_Pic_List.Clear();

                    foreach (KeyValuePair<string, ImageInfo> pair in Main.DicInfo_Copy)
                    {

                        for (int i = 0; i < Main.Dl_Apply_List_Main.Count; i++)
                        {
                            if (pair.Value.sdip_no == Main.Dl_Apply_List_Main[i])
                            {
                                dicInfo_Filter.Add(pair.Key, pair.Value);
                                //Select_Pic_List.Add(pair.Key);
                            }

                        }

                    }
                    Set_PictureBox();

                    Set_Image();
                    Main.Print_List();
                    Main.List_Count_TB.Text = dicInfo_Filter.Count.ToString();
                }
                else
                    MessageBox.Show("Limit 아래 부품이 없습니다.");
                    
            }

            else if (e.KeyCode == Keys.F10)
            {
                

                if (Main.Dl_NotApply_List_Main.Count > 0)
                {
                    dicInfo_Filter.Clear();
                    Select_Pic_List.Clear();

                    foreach (KeyValuePair<string, ImageInfo> pair in Main.DicInfo_Copy)
                    {

                        for (int i = 0; i < Main.Dl_NotApply_List_Main.Count; i++)
                        {
                            if (pair.Value.sdip_no == Main.Dl_NotApply_List_Main[i])
                            {
                                dicInfo_Filter.Add(pair.Key, pair.Value);
                                //Select_Pic_List.Add(pair.Key);
                            }

                        }

                    }
                    Set_PictureBox();

                    Set_Image();
                    Main.Print_List();
                    Main.List_Count_TB.Text = dicInfo_Filter.Count.ToString();
                }
                else
                    MessageBox.Show("Limit 초과 부품이 없습니다.");

            }

        }
       

        public ImageViewer(FormViewPort mainForm)
        {
            InitializeComponent();
            
        }

        public void Load_Del()
        {
            foreach(var key in dicInfo_Delete.Keys.ToList())
            {
                dicInfo_Delete[key].DeleteCheck = "삭제대기";

                if (dicInfo_Filter.ContainsKey(key))
                {
                    dicInfo_Filter.Remove(key);
                }
            }
            Del_Set_View();
            
        }

        public void Get_Delete_IMG()
        {
            
            for(int p = 0; p < Select_Pic.Count; p++)
            {
                if(dicInfo_Filter.ContainsKey(Select_Pic[p]))
                {
                    
                    dicInfo_Delete.Add(Select_Pic[p], dicInfo_Filter[Select_Pic[p]]);

                    dicInfo_Delete[Select_Pic[p]].DeleteCheck = "삭제대기";
                }
            }
            Main.Waiting_Del = dicInfo_Delete;
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

        private void ImageViewer_Load(object sender, EventArgs e)
        {

        }

        public void Set_View()
        {
            this.Controls.Clear();
            PictureData.Clear();
            
            if(Filter_NO_1 != 1)
            {
                frame_List_Img = Main.Frame_List_Main;
                dicInfo_Filter = Main.DicInfo;
                
            }
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            DicInfo_Filtered = Sorted_dic;

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

            Main.List_Count_TB.Text = DicInfo_Filtered.Count.ToString();
            this.Focus();
        }

        public void Frame_Set_View()
        {
            this.Controls.Clear();
            PictureData.Clear();

            frame_List_Img = Main.Frame_List_Main;
            dicInfo_Filter = Main.DicInfo;


            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;

            cols = int.Parse(Main.Cols_TB.Text);
            rows = int.Parse(Main.Rows_TB.Text);
            width = int.Parse(Main.Width_TB.Text);
            height = int.Parse(Main.Height_TB.Text);

            Current_PageNum = 1;
            Current_Frame_PageNum = 1;

            Main.S_Page_TB.Text = Current_PageNum.ToString();
            Total_PageNum = ((dicInfo_Filter.Count - 1) / (cols * rows)) + 1;
            Main.E_Page_TB.Text = Total_PageNum.ToString();


            Main.Frame_S_Page_TB.Text = Current_PageNum.ToString();
            Total_Frame_PageNum = frame_List_Img.Count;
            Main.Frame_E_Page_TB.Text = Total_PageNum.ToString();

            Set_PictureBox();
            Frame_Set_Image();
            Last_Picture_Selected_Index = -1;
            this.Focus();
        }
        public void Eq_CB_Set_View()
        {
            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = Main.Eq_CB_dicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;
            
            for(int i = 0;  i < Eq_cb_need_del.Count; i++)
            {
                dicInfo_Filter.Remove(Eq_cb_need_del[i]);
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

            Main.List_Count_TB.Text = DicInfo_Filtered.Count.ToString();
            this.Focus();
        }

        public void Eq_CB_Set_View_ING()
        {
            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = Main.Return_dicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;
            Eq_cb_need_del = Main.selected_Pic;
           

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
            this.Controls.Clear();
            PictureData.Clear();


            dicInfo_Filter = Main.DicInfo;
            Sorted_dic = dicInfo_Filter.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            DicInfo_Filtered = Sorted_dic;

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

        }

        private void ImageViewer_PL_MouseUp(object sender, MouseEventArgs e)
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

            if(Main.Frame_View_CB.Checked)
                Frame_Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);
            else
                Find_Contain_PB(src_Mouse_XY, dst_Mouse_XY);






            if (Main.Frame_View_CB.Checked)
                Frame_change_Glass();
            else
                change_Glass();

            if (Change_state_List.Count > 0)
                Main.Changeed_State();

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
                    result = "불량";
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
                    result = "불량";
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

            Current_PageNum = 1;

            if(Main.Frame_View_CB.Checked)
            {
                Frame_Set_View();
            }

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
                temp_LB.Location = new Point(4, height - 20);
                temp_LB.Parent = Picture_Glass.ElementAt(i);
                ImageNameLB[i] = temp_LB;
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

                                //방향

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
            
            //if (Frame_List_Index != 0)
            //{
            //    Frame_Del_Range = frame_dicInfo_Filter.Keys.ToList();
            //}


            foreach (KeyValuePair<string, ImageInfo> kvp in dicInfo_Filter)
            {
                if (kvp.Value.FrameNo == frame_List_Img[Frame_List_Index])
                    if (frame_dicInfo_Filter.ContainsKey(kvp.Key))
                        break;
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

            //if (Frame_List_Index != 0)
            //{
            //    for(int i =0; i < Frame_Del_Range.Count; i++)
            //    {
            //        frame_dicInfo_Filter.Remove(Frame_Del_Range[i]);
                    
            //    }
            //}

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

                                    //방향

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

                                    //방향

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

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)

                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Yellow;
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
            if (EachPage_ImageNum < 0)
                EachPage_ImageNum = 0;

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
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)

                        ImageNameLB[i].Text = frame_dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Yellow;
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = frame_dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
            if (EachPage_ImageNum < 0)
                EachPage_ImageNum = 0;

            this.Focus();
        }
        public void Cheked_State_DF()
        {

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = dicInfo_Filter[dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Yellow;
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Red;
                }
            }
        }
        public void Frame_Cheked_State_DF()
        {

            for (int i = 0; i < EachPage_ImageNum; i++)
            {
                int index = ((Current_PageNum - 1) * (cols * rows)) + i;
                string temp = frame_dicInfo_Filter[frame_dicInfo_Filter.Keys.ElementAt(index)].ReviewDefectName;

                if (temp.Equals("양품") || temp.Equals("*"))
                {
                    DefectState[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].ForeColor = Color.Yellow;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = frame_dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

                    PictureData.ElementAt(i).Tag = Color.Yellow;
                }
                else
                {

                    DefectState[i].ForeColor = Color.Red;
                    ImageNameLB[i].ForeColor = Color.Red;
                    ImageNameLB[i].BackColor = Color.Black;

                    if (Main.Print_Image_State.Checked)
                        DefectState[i].Text = temp;
                    else
                        DefectState[i].Text = "";

                    if (Main.Print_Image_Name.Checked)
                        ImageNameLB[i].Text = frame_dicInfo_Filter.Keys.ElementAt(index);
                    else
                        ImageNameLB[i].Text = "";

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
        {this.Focus();
            ImageViewer_PL_MouseUp(sender, e);
            Draged_PB = null;
        }

        public ImageViewer()
        {
            InitializeComponent();
        }
    }
}

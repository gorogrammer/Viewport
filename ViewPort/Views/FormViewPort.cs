using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Functions;
using ViewPort.Models;
using ViewPort.Views;
using ViewPortNetwork;
using MetroFramework.Forms;
using System.Reflection;
using CredentialManagement;


namespace ViewPort
{


    public partial class FormViewPort : MetroForm
    {
        #region MEMBER VARIABLES

        bool Zip_Error = false;
        ImageViewer open = new ImageViewer();
        LoadingGIF_Func waitform = new LoadingGIF_Func();
        Dictionary<string, ImageInfo> eq_CB_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Copy = new Dictionary<string, ImageInfo>();
        Dictionary<string, txtInfo> dicTxt_info = new Dictionary<string, txtInfo>();
        Dictionary<string, ImageInfo> dicInfo_Waiting_Del = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> return_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> sdip_NO1_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> real_SDIP_200_DIc = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> f9_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> f10_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> f5_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> sdip_200_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> filter_200_dic_Main = new Dictionary<string, ImageInfo>();
        Dictionary<int, int> exceed_Count = new Dictionary<int, int>();
        Dictionary<string, ImageInfo> exceed_filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> set_filter = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> set_filter_frame_dicinfo = new Dictionary<string, ImageInfo>();
        
        List<int> set_filter_frame = new List<int>();
        List<int> final_Frame_List = new List<int>();
        List<int> sdip_200_frame = new List<int>();
        List<string> f12_del_list_main = new List<string>();
        List<string> overlap_key_Main = new List<string>();
        string between = string.Empty;
        int state_filter = 0;
        int curruent_viewtype = 0;
        int mode_change = 0;
        int eq_filter = 0;
        int list_filter = 0;
        int imageSize_Filter = 0;
        List<int> exceed_List = new List<int>();
        int setting = 0;
        string[] dic_ready = null;
        int arg = 0;
        string final_text_main = string.Empty;
        string viewType = null;
        int state_Filter = 0;
        string listFiler = string.Empty;
        string path_check = string.Empty;
        int Rotate_Option = 1;
        //List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        //List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> list_Filter_Main = new List<string>();

        List<string> All_LotID_List = new List<string>();
        Dictionary<string, string> sdip_result_dic = new Dictionary<string, string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();
        List<Tuple<string, int>> All_Equipment_DF_List_DEL = new List<Tuple<string, int>>();
        List<Tuple<string, int>> code_200_List = new List<Tuple<string, int>>();
        List<int> MAP_LIST = new List<int>();
        List<int> frame_List_main = new List<int>();
        List<int> f9_Frame_List_Main = new List<int>();
        List<int> f10_Frame_List_Main = new List<int>();
        List<string> Eq_Filter_Select_Key_List = new List<string>();
        List<int> exception_Frame = new List<int>();

        List<string> f5_Img_KeyList = new List<string>();
        List<string> accu_wait_Del_Img_List = new List<string>();
        List<string> Wait_Del_Img_List = new List<string>();
        List<string> Selected_Equipment_DF_List = new List<string>();
        List<string> ImageSizeList = new List<string>();
        List<string> Selected_Pic = new List<string>();
        List<string> Change_state_List = new List<string>();
        List<string> dl_List_Main = new List<string>();
        List<int> contain_200_Frame_Main = new List<int>();
        List<string> dl_Apply_List_Main = new List<string>();
        List<string> dl_NotApply_List_Main = new List<string>();
        Dictionary<int, int> map_List_Dic_main = new Dictionary<int, int>();
        Dictionary<int, int> map_List_Dic_Compare_main = new Dictionary<int, int>();

        Dictionary<int, int> Map_TXT_NO_Counting = new Dictionary<int, int>();
        List<Tuple<int, int>> Map_TXT_NO_Counting_Compare = new List<Tuple<int, int>>();

        int btnColumnIdx;

        Dictionary<string, ImageInfo> checkEQ_Dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> Sorted_dic_GRID = new Dictionary<string, ImageInfo>();
        Dictionary<int, int> Sorted_Map_dic = new Dictionary<int, int>();
        private int _load_State;
        private string dirPath;
        private string zipFilePath;
        private string ref_DirPath;
        FormLoading formLoading_1 = new FormLoading();

        public Dictionary<string, ImageInfo> DicInfo
        {
            get { return dicInfo; }
            set { dicInfo = value; }
        }
        public string ViewType { get => viewType; set => viewType = value; }
        public string ListFiler { get => listFiler; set => listFiler = value; }
        public string Between { get => between; set => between = value; }
        public List<Tuple<string, int>> CODE_200_List { get => code_200_List; set => code_200_List = value; }
        public int Setting { get => setting; set => setting = value; }
        public int State_Filter { get => state_filter; set => state_filter = value; }
        public int ImageSize_Filter_NO { get => imageSize_Filter; set => imageSize_Filter = value; }
    
        public int List_filter { get => list_filter; set => list_filter = value; }
        public List<int> Exception_Frame { get => exception_Frame; set => exception_Frame = value; }
        public string Final_text_main { get => final_text_main; set => final_text_main = value; }
        public Dictionary<string, ImageInfo> Eq_CB_dicInfo { get => eq_CB_dicInfo; set => eq_CB_dicInfo = value; }

        public Dictionary<int, int> Map_List_Dic_main { get => map_List_Dic_main; set => map_List_Dic_main = value; }
        public Dictionary<int, int> Map_List_Dic_Compare_main { get => map_List_Dic_Compare_main; set => map_List_Dic_Compare_main = value; }

        public Dictionary<string, ImageInfo> Filter_200_dic_Main { get => filter_200_dic_Main; set => filter_200_dic_Main = value; }
        public Dictionary<string, ImageInfo> Real_SDIP_200_DIc { get => real_SDIP_200_DIc; set => real_SDIP_200_DIc = value; }
        public Dictionary<string, ImageInfo> Exceed_filter { get => exceed_filter; set => exceed_filter = value; }
        public Dictionary<string, ImageInfo> Sdip_200_code_dicInfo { get => sdip_200_code_dicInfo; set => sdip_200_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> Filter_CheckEQ_Dic { get => filter_CheckEQ_Dic; set => filter_CheckEQ_Dic = value; }
        public Dictionary<string, ImageInfo> F9_code_dicInfo { get => f9_code_dicInfo; set => f9_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> F10_code_dicInfo { get => f10_code_dicInfo; set => f10_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> CheckEQ_Dic { get => checkEQ_Dic; set => checkEQ_Dic = value; }
        public Dictionary<string, ImageInfo> F5_code_dicInfo { get => f5_code_dicInfo; set => f5_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> Sdip_NO1_dicInfo { get => sdip_NO1_dicInfo; set => sdip_NO1_dicInfo = value; }
        public Dictionary<string, ImageInfo> DicInfo_Copy { get => dicInfo_Copy; set => dicInfo_Copy = value; }
        public Dictionary<string, ImageInfo> Return_dicInfo { get => return_dicInfo; set => return_dicInfo = value; }
        public Dictionary<string, ImageInfo> Waiting_Del { get => dicInfo_Waiting_Del; set => dicInfo_Waiting_Del = value; }

        public List<int> Frame_List_Main { get => frame_List_main; set => frame_List_main = value; }

        public Dictionary<string, string> Sdip_result_dic { get => sdip_result_dic; set => sdip_result_dic = value; }

        public List<int> F9_Frame_List_Main { get => f9_Frame_List_Main; set => f9_Frame_List_Main = value; }

        public List<int> Exceed_List { get => exceed_List; set => exceed_List = value; }
        public List<int> F10_Frame_List_Main { get => f10_Frame_List_Main; set => f10_Frame_List_Main = value; }

        public List<string> F5_Img_KeyList_Main { get => f5_Img_KeyList; set => f5_Img_KeyList = value; }

        public List<string> Overlap_key_Main { get => overlap_key_Main; set => overlap_key_Main = value; }


        public List<int> mAP_LIST { get => MAP_LIST; set => MAP_LIST = value; }
        public List<int> Contain_200_Frame_Main { get => contain_200_Frame_Main; set => contain_200_Frame_Main = value; }
        public List<string> selected_Pic { get => Selected_Pic; set => Selected_Pic = value; }
        public List<string> List_Filter_Main { get => list_Filter_Main; set => list_Filter_Main = value; }
        public List<string> Dl_Apply_List_Main { get => dl_Apply_List_Main; set => dl_Apply_List_Main = value; }
        public List<string> F12_del_list_main { get => f12_del_list_main; set => f12_del_list_main = value; }
        public List<string> Dl_NotApply_List_Main { get => dl_NotApply_List_Main; set => dl_NotApply_List_Main = value; }
        public List<string> Dl_List_Main { get => dl_List_Main; set => dl_List_Main = value; }

        public List<string> Selected_Equipment_DF_List_Main { get => Selected_Equipment_DF_List; set => Selected_Equipment_DF_List = value; }


        public string ZipFilePath { get => zipFilePath; set => zipFilePath = value; }
        public string REF_DirPath { get => ref_DirPath; set => ref_DirPath = value; }
        public string DirPath { get => dirPath; set => dirPath = value; }
        public int Load_State { get => _load_State; set => _load_State = value; }

        public int Eq_filter { get => eq_filter; set => eq_filter = value; }
        public int GetLoad_State()
        {
            return Load_State;
        }


        #endregion

        #region Initialize CODE
        public FormViewPort()
        {


            InitializeComponent();

            dataGridView1.DoubleBuffered(true);
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView2.DoubleBuffered(true);
            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

            Init();
            InitialData();
            Assembly assemObj = Assembly.GetExecutingAssembly();
            Version v = assemObj.GetName().Version; // 현재 실행되는 어셈블리..dll의 버전 가져오기

            int majorV = v.Major; // 주버전
            int minorV = v.Minor; // 부버전
            int buildV = v.Build; // 빌드번호
            int revisionV = v.Revision; // 수정번호

            versionNo.Text = "Version" + " "+ revisionV.ToString();

        }

        public void Init()
        {
            splitContainer1.Panel2.Controls.Add(open);
            open.Dock = DockStyle.Fill;

            DataTable dt = new DataTable();
            dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            dt.Columns.Add(COLUMN_STR.GRID_STATE);

            dt.PrimaryKey = new DataColumn[] { dt.Columns[COLUMN_STR.GRID_IMGNAME] };


            dataGridView1.DataSource = dt;


            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = COLUMN_STR.GRID_SELECT;
            btnColumn.Name = "buttonColumn";
            btnColumnIdx = dataGridView1.Columns.Add(btnColumn);

            dataGridView1.Columns[0].Width = 230;
            dataGridView1.Columns[1].Width = 55;
            dataGridView1.Columns[2].Width = 55;




            DataTable dt_del = new DataTable();
            dt_del.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            dt_del.Columns.Add(COLUMN_STR.GRID_STATE);
            dt_del.PrimaryKey = new DataColumn[] { dt_del.Columns[COLUMN_STR.GRID_IMGNAME] };
            dataGridView2.DataSource = dt_del;

            Rotate_CLB.Items.Add("0˚");
            Rotate_CLB.Items.Add("90˚");
            Rotate_CLB.Items.Add("180˚");
            Rotate_CLB.Items.Add("270˚");
            Rotate_CLB.SelectedIndex = 1;

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView2, new object[] { true });
        }
        private void InitialData()
        {
            Load_State = -1;
            DirPath = string.Empty;
            REF_DirPath = string.Empty;

        }
        #endregion

        #region UI CODE
        private void Initial_Equipment_DF_List()
        {

            All_Equipment_DF_List = All_Equipment_DF_List.OrderBy(s => s.Item2).ThenByDescending(s => s.Item2).ToList();
            All_Equipment_DF_List.Reverse();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;


        }

        public int GetRotation_Option()
        {
            return Rotate_Option;
        }

        public void SetRotation_Option(int Option)
        {
            Rotate_Option = Option;
        }

        public void Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();


            //foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
            //    dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);


            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };



            foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            dataGridView1.DataSource = Dt;

        }

        public void First_Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();


            foreach (KeyValuePair<string, ImageInfo> kvp in DicInfo)
                dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

        }
        public void Frame_Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();

            //foreach (KeyValuePair<string, ImageInfo> kvp in open.Frame_dicInfo_Filter)
            //    dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };



            foreach (KeyValuePair<string, ImageInfo> kvp in open.Frame_dicInfo_Filter)
                Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            dataGridView1.DataSource = Dt;
        }

        private void Eq_Filter_after_Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();


            //foreach (KeyValuePair<string, ImageInfo> kvp in eq_CB_dicInfo)
            //    dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };



            foreach (KeyValuePair<string, ImageInfo> kvp in eq_CB_dicInfo)
                Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            dataGridView1.DataSource = Dt;

        }
        public void Return_Img_Print()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            Return_dicInfo.Clear();

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {
                Return_dicInfo = open.Frame_dicInfo_Filter;
            }
            else
            {
                Return_dicInfo = open.DicInfo_Filtered;
            }


            if (ViewType == "Code_200_SetView")
            {
                for (int i = 0; i < selected_Pic.Count; i++)
                {
                    Return_dicInfo.Add(selected_Pic[i], Sdip_200_code_dicInfo[selected_Pic[i]]);

                    if (dt.Rows.Contains(selected_Pic[i]))
                        continue;
                    else
                        dt.Rows.Add(Sdip_200_code_dicInfo[selected_Pic[i]].Imagename, Sdip_200_code_dicInfo[selected_Pic[i]].ReviewDefectName);
                }
            }
            else
            {
                for (int i = 0; i < selected_Pic.Count; i++)
                {
                    if(DicInfo.Keys.ToList().Contains(selected_Pic[i]))
                    {
                        Return_dicInfo.Add(selected_Pic[i], dicInfo[selected_Pic[i]]);

                        if (dt.Rows.Contains(selected_Pic[i]))
                            continue;
                        else
                            dt.Rows.Add(dicInfo[selected_Pic[i]].Imagename, dicInfo[selected_Pic[i]].ReviewDefectName);
                    }
                    else if(F5_code_dicInfo.Keys.ToList().Contains(selected_Pic[i]))
                    {
                        Return_dicInfo.Add(selected_Pic[i], F5_code_dicInfo[selected_Pic[i]]);

                        if (dt.Rows.Contains(selected_Pic[i]))
                            continue;
                        else
                            dt.Rows.Add(F5_code_dicInfo[selected_Pic[i]].Imagename, F5_code_dicInfo[selected_Pic[i]].ReviewDefectName);
                    }
                    

                
                }

            }



            dt.DefaultView.Sort = "이름";


            Sorted_dic = Return_dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Return_dicInfo = Sorted_dic;



            //open.DicInfo_Filtered = Return_dicInfo;
            List_Count_TB.Text = String.Format("{0:#,##0}", Return_dicInfo.Count);


            if (Selected_Equipment_DF_List.Count > 0 && ViewType != "Code_200_SetView")
            {
                open.Eq_CB_Set_View_ING();
            }  
            else
            {
                open.Set_Image();
            }
               



            selected_Pic.Clear();


        }
        public void Wait_Del_Print_List()
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataTable dt_del = (DataTable)dataGridView2.DataSource;
            dt_del.Rows.Clear();
            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };


            foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
            {
                Dt.Rows.Add(kvp.Key, kvp.Value.DeleteCheck);
            }


            dataGridView2.DataSource = Dt;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        public void Img_txt_Info_Combine()
        {

            foreach (KeyValuePair<string, Models.txtInfo> kvp in dicTxt_info)
            {
                if (dicInfo.ContainsKey(kvp.Key))
                {
                    dicInfo[kvp.Key].sdip_no = kvp.Value.SDIP_No;
                    dicInfo[kvp.Key].sdip_result = kvp.Value.SDIP_Result;
                    dicInfo[kvp.Key].X_Location = kvp.Value.X_Location;
                    dicInfo[kvp.Key].Y_Location = kvp.Value.Y_Location;
                    dicInfo[kvp.Key].Master_NO = kvp.Value.Master_No;

                    if (dicInfo[kvp.Key].sdip_no == "1")
                    {
                        Sdip_NO1_dicInfo.Add(kvp.Key, dicInfo[kvp.Key]);
                    }

                }

            }
        }

        public void Dl_Wait_Del_Print_List()
        {

            int index = 0;

            DataTable dt_del = (DataTable)dataGridView2.DataSource;
            dt_del.PrimaryKey = new DataColumn[] { dt_del.Columns[COLUMN_STR.GRID_IMGNAME] };

            if (ViewType == "Code_200_SetView")
            {
                Return_Update_Code_No200_List(Selected_Pic);
                for (int i = 0; i < Selected_Pic.Count; i++)
                {
                    DataRow dr = dt_del.NewRow();
                    dr = dt_del.Rows.Find(Selected_Pic[i]);
                    index = dt_del.Rows.IndexOf(dr);
                    dt_del.Rows[index].Delete();
                    //dt_del.AcceptChanges();

                }
                Code_200_Set();
            }
            else
            {
                Return_update_Equipment_DF_CLB(Selected_Pic);

                for (int i = 0; i < Selected_Pic.Count; i++)
                {
                    DataRow dr = dt_del.NewRow();
                    dr = dt_del.Rows.Find(Selected_Pic[i]);
                    index = dt_del.Rows.IndexOf(dr);
                    dt_del.Rows[index].Delete();
                    //dt_del.AcceptChanges();

                }
            }

            for (int i = 0; i < Selected_Pic.Count; i++)
            {
                if (Waiting_Del.Keys.ToList().Contains(Selected_Pic[i]))
                {
                    Waiting_Del.Remove(Selected_Pic[i]);
                }

            }
            

        }
        public void Dl_PrintList()
        {
            int index = 0;

            Selected_Pic = new List<string>(open.Select_Pic_List);
            int dd = Selected_Equipment_DF_List.Count;
            DataTable dt = (DataTable)dataGridView1.DataSource;

            

            //dt.Rows.Clear();

            if (ViewType == "Code_200_SetView")
            {
                Update_Code_No200_List(Selected_Pic);

                foreach (string name in Selected_Pic)
                {
                    DataRow dr = dt.NewRow();
                    dr = dt.Rows.Find(Sdip_200_code_dicInfo[name].Imagename);
                    index = dt.Rows.IndexOf(dr);
                    dt.Rows[index].Delete();
                }
                dt.AcceptChanges();

                for (int i = 0; i < Selected_Pic.Count; i++)
                {
                    Sdip_200_code_dicInfo.Remove(Selected_Pic[i]);

                }
                Code_200_Set();
                //DataTable Dt = new DataTable();
                //Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
                //Dt.Columns.Add(COLUMN_STR.GRID_STATE);
                //Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };



                //foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                //    Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

                //dataGridView1.DataSource = Dt;

                if (Eq_CB_dicInfo.Count > 0)
                {

                }
                else
                    Select_All_BTN_Click(null, null);
            }
            else
            {
                update_Equipment_DF_CLB(Selected_Pic);


                foreach (string name in Selected_Pic)
                {
                    if(F5_code_dicInfo.ContainsKey(name))
                    {
                        DataRow dr = dt.NewRow();
                        dr = dt.Rows.Find(F5_code_dicInfo[name].Imagename);
                        index = dt.Rows.IndexOf(dr);
                        dt.Rows[index].Delete();
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr = dt.Rows.Find(DicInfo[name].Imagename);
                        index = dt.Rows.IndexOf(dr);
                        dt.Rows[index].Delete();

                    }
                    
                }
                dt.AcceptChanges();


                for (int i = 0; i < Selected_Pic.Count; i++)
                {

                    DicInfo.Remove(Selected_Pic[i]);
                    if (Eq_CB_dicInfo.Count > 0)
                    {
                        if (Eq_CB_dicInfo.ContainsKey(Selected_Pic[i]))
                            Eq_CB_dicInfo.Remove(Selected_Pic[i]);
                    }

                }

             
                //DataTable Dt = new DataTable();
                //Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
                //Dt.Columns.Add(COLUMN_STR.GRID_STATE);
                //Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };

                

                //foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                //    Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

                //dataGridView1.DataSource = Dt;

                

            //    if (Eq_CB_dicInfo.Count > 0)
            //    {
            //        Select_All_BTN_Click(null, null);
            //    }
            //    else
            //        Select_All_BTN_Click(null, null);
            }


            Selected_Pic.Clear();
        }

        public void Set_Dl_PrintList()
        {
            int index = 0;

            DataTable dt = (DataTable)dataGridView1.DataSource;



            for (int i = 0; i < Selected_Pic.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(DicInfo_Copy[Selected_Pic[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                dt.Rows[index].Delete();

            }


            Selected_Pic.Clear();
        }

        public void Load_After_Dl_PrintList()
        {
            int index = 0;

            Selected_Pic = open.Select_Pic_List;

            DataTable dt = (DataTable)dataGridView1.DataSource;

            update_Equipment_DF_CLB(Selected_Pic);

            for (int i = 0; i < Selected_Pic.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(DicInfo_Copy[Selected_Pic[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                dt.Rows[index].Delete();

                DicInfo.Remove(Selected_Pic[i]);
                if (Eq_CB_dicInfo.Count > 0)
                {
                    if (Eq_CB_dicInfo.ContainsKey(Selected_Pic[i]))
                        Eq_CB_dicInfo.Remove(Selected_Pic[i]);
                }
                //dt.AcceptChanges();
            }

            if (Eq_CB_dicInfo.Count > 0)
            {

            }
            else
                Select_All_BTN_Click(null, null);
        }
        public void No1_Dl_PrintList()
        {
            int index = 0;
            Dictionary<string, ImageInfo> Dl_DicInfo = new Dictionary<string, ImageInfo>(DicInfo);

            Selected_Pic = open.Select_Pic_List;


            DataTable dt = (DataTable)dataGridView1.DataSource;

            update_Equipment_DF_CLB(Selected_Pic);
            //DicInfo.Clear();



            for (int i = 0; i <= Selected_Pic.Count - 1; i++)
            {
                if (DicInfo.ContainsKey(Selected_Pic[i]))
                    DicInfo.Remove(Selected_Pic[i]);
            }



            dt.Rows.Clear();
            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };

            foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);


            dataGridView1.DataSource = Dt;
            Select_All_BTN_Click(null, null);
        }
        public void Filter_NO_1_PrintList()
        {
            int index = 0;

            Selected_Pic = open.Select_Pic_List;

            DataTable dt = (DataTable)dataGridView1.DataSource;



            for (int i = 0; i < Selected_Pic.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(dicInfo[Selected_Pic[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                dt.Rows[index].Delete();


                //dt.AcceptChanges();
            }
            Selected_Pic.Clear();
        }


        public void Changeed_State()
        {
            int index = 0;
            Change_state_List = open.Change_state;

            DataTable dt = (DataTable)dataGridView1.DataSource;


            for (int i = 0; i < Change_state_List.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(dicInfo_Copy[Change_state_List[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                if (dicInfo_Copy.ContainsKey(Change_state_List[i]))
                    dt.Rows[index][1] = dicInfo_Copy[Change_state_List[i]].ReviewDefectName;


            }

        }

        public void ALL_Changeed_State()
        {
            int index = 0;


            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            DataTable Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { Dt.Columns[COLUMN_STR.GRID_IMGNAME] };
            //dt.Rows.Clear();

            //if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dataGridView1.Rows[i].Cells[2].Value = open.Frame_dicInfo_Filter[dataGridView1.Rows[2].Cells[1].Value.ToString().Substring(0, 12)].ReviewDefectName;
            //    }
            //}
            //else
            //{

            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dataGridView1.Rows[i].Cells[2].Value = open.DicInfo_Filtered[dataGridView1.Rows[2].Cells[1].Value.ToString().Substring(0, 12)].ReviewDefectName;
            //    }
            //}

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in open.Frame_dicInfo_Filter)
                    Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);
            }
            else
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                    Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

            }

            dataGridView1.DataSource = Dt;
        }

        public void Counting_IMG_inZip(string ZipFilePath)
        {
            Exception_Frame.Clear();
            ZipArchive subZip;
            Stream subEntryMS;
            int arg = 0;

            ZipArchive zip = ZipFile.Open(ZipFilePath, ZipArchiveMode.Update);   // Zip파일(Lot) Load
            {

                foreach (ZipArchiveEntry entry in zip.Entries.ToList())
                {
                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1)
                    {
                        if(!final_Frame_List.Contains(int.Parse(entry.Name.Substring(0, 5))) && !sdip_200_frame.Contains(int.Parse(entry.Name.Substring(0, 5))))
                        {
                            subEntryMS = entry.Open();
                            subZip = new ZipArchive(subEntryMS);
                            ZipArchiveEntry ImgEntry = zip.GetEntry(entry.Name);
                            if (subZip.Entries.Count == 0)
                            {
                                arg = 1;
                                Exception_Frame.Add(int.Parse(entry.Name.Substring(0, 5)));

                            }
                            else
                                arg = 0;

                            subZip.Dispose();

                            if (arg == 1)
                            {
                                ImgEntry.Delete();
                            }
                        }
                    }

                }

                zip.Dispose();
            }

        }
        private void _filterAct_bt_Click(object sender, EventArgs e)
        {
            try
            {
                waitform.Show(this);
                Eq_CB_dicInfo.Clear();
                //Camera_NO_Filter_TB.Text = "";
                textBox4.Text = "";
                Frame_S_TB.Text = "";
                checkBox1.Checked = false;
                ImageSize_CB.Items.Clear();
                ImageSize_CB.Text = "";
                Initial_Equipment_DF_FilterList();
                Exceed_CB.CheckState = CheckState.Unchecked;
                if (Frame_View_CB.Checked)
                {
                    Frame_View_CB.Checked = false;
                    Frame_S_Page_TB.Text = "";
                    Frame_E_Page_TB.Text = "";
                }

                if(Frame_Interval_CB.Checked)
                {
                    Frame_E_TB.Enabled = true;
                    Frame_E_TB.ReadOnly = true;
                    Frame_E_TB.Text = "";
                    Frame_Interval_CB.CheckState = CheckState.Unchecked;

                }
                //eq_CB_dicInfo = new Dictionary<string, ImageInfo>(dicInfo);

                if (Waiting_Del.Count > 0)
                {
                    foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
                        eq_CB_dicInfo.Remove(kvp.Key);

                }
                if (Camera_NO_Filter_TB.Text != "")
                {
                    string[] Split_String = null;
                    Split_String = Camera_NO_Filter_TB.Text.Split(',');


                    foreach (string No in eq_CB_dicInfo.Keys.ToList())
                    {
                        if (eq_CB_dicInfo.ContainsKey(No))
                        {
                            if (Split_String.Contains(eq_CB_dicInfo[No].CameraNo.ToString()))
                            {
                                continue;
                            }
                            else
                                eq_CB_dicInfo.Remove(No);
                        }

                    }

                }

                if (textBox4.Text != "")
                {
                    if (textBox4.Text == "양품" || textBox4.Text == "선택")
                    {


                        foreach (string pair in eq_CB_dicInfo.Keys.ToList())
                        {
                            if (eq_CB_dicInfo[pair].ReviewDefectName == textBox4.Text)
                            {

                            }
                            else
                            {
                                eq_CB_dicInfo.Remove(pair);
                            }
                        }



                    }
                    else
                    {
                        textBox4.Text = string.Empty;
                        MessageBox.Show("양품 or 선택으로 입력.");
                        State_Filter = 0;
                      
                    }
                }

                Sorted_dic = eq_CB_dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                Eq_CB_dicInfo = Sorted_dic;

                Eq_Filter_after_Print_List();
                open.Main = this;
                open.Eq_CB_Set_View();
                Equipment_DF_CLB.SelectedIndex = Equipment_DF_CLB.Items.IndexOf(Equipment_DF_CLB.CheckedItems[0].ToString());


                waitform.Close();
            }
            catch { }


        }

        private void Set_EQ_Dic()
        {
            Initial_Equipment_DF_FilterList();

            if (Waiting_Del.Count > 0)
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
                    eq_CB_dicInfo.Remove(kvp.Key);

            }
            CheckEQ_Dic = new Dictionary<string, ImageInfo>(eq_CB_dicInfo);
            Sorted_dic = CheckEQ_Dic.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            CheckEQ_Dic = Sorted_dic;
        }

        private void Initial_Equipment_DF_FilterList()
        {

            for (int i = 0; i < Selected_Equipment_DF_List.Count; i++)
            {
                foreach (KeyValuePair<string, ImageInfo> pair in DicInfo)
                {

                    if (pair.Value.EquipmentDefectName == Selected_Equipment_DF_List[i])
                    {
                        if (Eq_CB_dicInfo.ContainsKey(pair.Key) == false)
                            Eq_CB_dicInfo.Add(pair.Key, pair.Value);
                    }

                }

            }


        }

        private void Equipment_DF_CLB_SelectedValueChanged(object sender, EventArgs e)
        {
            Selected_Equipment_DF_List.Clear();

            foreach (int index in Equipment_DF_CLB.CheckedIndices)
            {
                if(Equipment_DF_CLB.Items[index].ToString().Split('-').Length >2)
                {
                    Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0]+"-"+ Equipment_DF_CLB.Items[index].ToString().Split('-')[1]);
                }
                else
                {
                    Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0]);
                }
                    
            }
                

      
        }

        public Dictionary<string, ImageInfo> Get_selected_item_inEQ()
        {
            Selected_Equipment_DF_List.Clear();

            foreach (int index in Equipment_DF_CLB.CheckedIndices)
            {
                if (Equipment_DF_CLB.Items[index].ToString().Split('-').Length > 2)
                {
                    Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0] + "-" + Equipment_DF_CLB.Items[index].ToString().Split('-')[1]);
                }
                else
                {
                    Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0]);
                }

            }

            Initial_Equipment_DF_FilterList();
            Sorted_dic = eq_CB_dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Eq_CB_dicInfo = Sorted_dic;

            if (Waiting_Del.Count > 0)
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
                    Eq_CB_dicInfo.Remove(kvp.Key);

            }

            return Eq_CB_dicInfo;
        }

        private void Select_All_BTN_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
                Equipment_DF_CLB.SetItemCheckState(i, CheckState.Checked);

            Equipment_DF_CLB_SelectedValueChanged(null, null);
        }

        private void Select_Empty_BTN_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
                Equipment_DF_CLB.SetItemCheckState(i, CheckState.Unchecked);

            Equipment_DF_CLB_SelectedValueChanged(null, null);
        }

        public void ZipLoadFile_Viewmode()
        {
            open.Filter_NO_Set();
            Load_State = 1;
            string path = Util.OpenFileDlg(ZIP_STR.EXETENSION);
            string FileName = string.Empty;

            if (string.IsNullOrEmpty(path) == false)
            {
                FileName = Util.GetFileName();
                DirPath = Directory.GetParent(path).ToString();
                ZipFilePath = path;
                Equipment_DF_CLB.Items.Clear();
                if (MessageBox.Show("" + FileName + "로트 파일을 로드 하시겠습니까?", "프로그램 로드", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    FormLoading formLoading = new FormLoading(path, this);
                    formLoading.Opacity = 0.9;
                    formLoading.ShowDialog();
                    if (formLoading.Zip_Error)
                    {
                        Zip_Error = formLoading.Zip_Error;
                        return;
                    }

                    DicInfo = formLoading.Dic_Load;
                    Dictionary<string, ImageInfo> sort_dic = new Dictionary<string, ImageInfo>(DicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value));
                    
                    DicInfo = sort_dic;
                    f9_Frame_List_Main = formLoading.F9_Frame_List;
                    f10_Frame_List_Main = formLoading.F10_Frame_List;
                    F5_Img_KeyList_Main = formLoading.F5_dic_Load;

                    Contain_200_Frame_Main = formLoading.Contain_200_Frame_List;

                    MAP_LIST = formLoading.Map_List;

                    Dl_List_Main = formLoading.Dl_List;

                    Frame_List_Main = formLoading.Frame_List;

                    for (int i = 0; i < MAP_LIST.Count; i++)
                    {
                        if (Frame_List_Main.Contains(MAP_LIST[i]))
                            continue;
                        else
                        {
                            MAP_LIST.RemoveAt(i);
                            i--;
                        }
                    }




                    dicTxt_info = formLoading.DicTxt_info;

                    All_Equipment_DF_List = formLoading.All_Equipment_DF_List;
                    All_LotID_List = formLoading.All_LotID_List;
                    Dl_Apply_List_Main = formLoading.Dl_Apply_List;
                    Dl_NotApply_List_Main = formLoading.Dl_NOt_Apply_List;
                    ImageSizeList = formLoading.ImageSizeList;

                    Sdip_NO1_dicInfo = formLoading.Sdip_no1_dicload;

                    dataGridView1.DataSource = formLoading.Dt;





                    formLoading.Dispose();
                }
                else
                {
                  
                    ZipFilePath = "";
                }

            }
        }
        private void ZipLoadFile_Async()
        {
            //if (path == "" && curruent_viewtype == 1)
            //{
            //    if (DicInfo.Count > 0 && View_Mode_RB.Checked == true)
            //    {
            //        Manual_Mode_RB.Checked = true;
            //        View_Mode_RB.Checked = false;

            //    }
            //    else if (DicInfo.Count > 0 && Manual_Mode_RB.Checked == true)
            //    {
            //        Manual_Mode_RB.Checked = true;
            //        View_Mode_RB.Checked = false;

            //    }
            //    mode_change = 0;
            //    return;
            //}
            //else if (path == "" && curruent_viewtype == 0)
            //{
            //    if (DicInfo.Count > 0 && View_Mode_RB.Checked == true)
            //    {
            //        Manual_Mode_RB.Checked = false;
            //        View_Mode_RB.Checked = true;

            //    }
            //    else if (DicInfo.Count > 0 && Manual_Mode_RB.Checked == true)
            //    {
            //        Manual_Mode_RB.Checked = false;
            //        View_Mode_RB.Checked = true;

            //    }
            //    mode_change = 0;
            //    return;
            //}
            string path = Util.OpenFileDlg(ZIP_STR.EXETENSION);
            if (path !="")
            {
                open.Filter_NO_Set();
                open.ImageViewer_Clear();
                Load_State = 1;
            }
          
       
           
            path_check = path;
            if(path == "" && DicInfo.Count==0)
            {
                return;
            }
            else if(path == "" && curruent_viewtype == 1)
            {
               
                    Manual_Mode_RB.Checked = true;
                    View_Mode_RB.Checked = false;

             
                mode_change = 0;
                return;
            }
            else if (path == "" && curruent_viewtype == 0)
            {
               
                
                    Manual_Mode_RB.Checked = false;
                    View_Mode_RB.Checked = true;

                
                mode_change = 0;
                return;
            }
            else if (path == "")
            {
                mode_change = 0;
                return;
            }
                

                string FileName = string.Empty;
                if (string.IsNullOrEmpty(path) == false)
                {
                All_Clear();
                InitialData();
                FileName = Util.GetFileName();
                    DirPath = Directory.GetParent(path).ToString();
                    ZipFilePath = path;
                    Equipment_DF_CLB.Items.Clear();
                    if (MessageBox.Show("" + FileName + "로트 파일을 로드 하시겠습니까?", "프로그램 로드", MessageBoxButtons.YesNo) ==System.Windows.Forms.DialogResult.Yes)
                    {
                        FormLoading formLoading = new FormLoading(path, this);
                        formLoading.Opacity = 0.9;
                        formLoading.ShowDialog();
                    if(formLoading.Zip_Error)
                    {
                        Zip_Error = formLoading.Zip_Error;
                        return;
                    }

                                         


                        DicInfo = formLoading.Dic_Load;
                        f9_Frame_List_Main = formLoading.F9_Frame_List;
                        f10_Frame_List_Main = formLoading.F10_Frame_List;
                        F5_Img_KeyList_Main = formLoading.F5_dic_Load;

                        Contain_200_Frame_Main = formLoading.Contain_200_Frame_List;

                        MAP_LIST = formLoading.Map_List;

                        Dl_List_Main = formLoading.Dl_List;

                        Frame_List_Main = formLoading.Frame_List;
                    
                    List<int> Map_Last_Frame = Map_List_Dic_main.Keys.ToList();
       
                    for (int i = 0; i < Frame_List_Main.Count; i++)
                    {
                        if (Map_List_Dic_main.Keys.ToList().Contains(Frame_List_Main[i]))
                            continue;
                        else if (Frame_List_Main[i] != 1 || Frame_List_Main[i] != Map_Last_Frame[Map_List_Dic_main.Count - 1])
                        {
                            MessageBox.Show("MAP TXT에는 " + Frame_List_Main[i] + "프레임이 있지만 해당 프레임 이미지가 없습니다.");
                            ZipFilePath = "";
                            return;
                        }
                    }


                    dicTxt_info = formLoading.DicTxt_info;

                        All_Equipment_DF_List = formLoading.All_Equipment_DF_List;
                        All_LotID_List = formLoading.All_LotID_List;
                        Dl_Apply_List_Main = formLoading.Dl_Apply_List;
                        Dl_NotApply_List_Main = formLoading.Dl_NOt_Apply_List;
                        ImageSizeList = formLoading.ImageSizeList;
                        Overlap_key_Main = formLoading.Overlap_key;

                        Sdip_NO1_dicInfo = formLoading.Sdip_no1_dicload;

                        dataGridView1.DataSource = formLoading.Dt;







                        formLoading.Dispose();
                    }
                    else
                    {
                        ZipFilePath = "";
                    }

                }
            




        }
        public Dictionary<string, ImageInfo> F5_Find_in_Dicinfo()
        {
            F9_code_dicInfo.Clear();
            F10_code_dicInfo.Clear();
            foreach (KeyValuePair<string, ImageInfo> pair in DicInfo)
            {
                if (F9_Frame_List_Main.Contains(pair.Value.FrameNo))
                {
                    if (!F9_code_dicInfo.ContainsKey(pair.Key))
                        F9_code_dicInfo.Add(pair.Key, pair.Value);


                }
                else if (F10_Frame_List_Main.Contains(pair.Value.FrameNo))
                {
                    if (!F10_code_dicInfo.ContainsKey(pair.Key))
                        F10_code_dicInfo.Add(pair.Key, pair.Value);


                }

            }
            return F9_code_dicInfo;
        }
        public Dictionary<string, ImageInfo> F9_Find_in_Dicinfo()
        {
            F9_code_dicInfo.Clear();
            F10_code_dicInfo.Clear();
            foreach (KeyValuePair<string, ImageInfo> pair in DicInfo)
            {
                if (F9_Frame_List_Main.Contains(pair.Value.FrameNo))
                {
                    if(!F9_code_dicInfo.ContainsKey(pair.Key))
                        F9_code_dicInfo.Add(pair.Key, pair.Value);


                }
                else if (F10_Frame_List_Main.Contains(pair.Value.FrameNo))
                {
                    if (!F10_code_dicInfo.ContainsKey(pair.Key))
                        F10_code_dicInfo.Add(pair.Key, pair.Value);


                }

            }
            return F9_code_dicInfo;
        }

        public Dictionary<string, ImageInfo> F10_Find_in_Dicinfo()
        {
            F9_code_dicInfo.Clear();
            F10_code_dicInfo.Clear();
            foreach (KeyValuePair<string, ImageInfo> pair in DicInfo)
            {
                if (F9_Frame_List_Main.Contains(pair.Value.FrameNo))
                {

                    F9_code_dicInfo.Add(pair.Key, pair.Value);


                }
                else if (F10_Frame_List_Main.Contains(pair.Value.FrameNo))
                {
                    F10_code_dicInfo.Add(pair.Key, pair.Value);


                }

            }
            return F10_code_dicInfo;
        }
        public void zipLoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //All_Clear();
            //InitialData();

            ZipLoadFile_Async();
            
            if(Zip_Error)
            {
                this.Close();
                return;
            }
            int x = 1;
            int index = 0;
            if (ZipFilePath != "")
            {
                //Img_txt_Info_Combine();

                
                if (path_check != "")
                {
                    dicInfo_Copy = new Dictionary<string, ImageInfo>(DicInfo);
                    sdip_200_frame.Clear();
                    Sdip_200_code_dicInfo.Clear();
                    Selected_Pic.Clear();

                    if (Manual_Mode_RB.Checked)
                    {
                        curruent_viewtype = 1;
                        //SDIP 코드 211~230 제외
                        foreach (string pair in dicInfo.Keys.ToList())
                        {
                            if (F5_Img_KeyList_Main.Contains(pair))
                            {
                                F5_code_dicInfo.Add(pair, dicInfo[pair]);
                            }


                            if (dicInfo[pair].sdip_no != "-" && 200 <= int.Parse(dicInfo[pair].sdip_no) && int.Parse(dicInfo[pair].sdip_no) <= 299)
                            {
                                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[pair].EquipmentDefectName)) == -1)
                                    continue;
                                else
                                {
                                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[pair].EquipmentDefectName));
                                    x = All_Equipment_DF_List[index].Item2;
                                    All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[pair].EquipmentDefectName, --x);

                                    if (x == 0)
                                    {
                                        All_Equipment_DF_List.RemoveAt(index);

                                    }

                                }
                                Sdip_200_code_dicInfo.Add(pair, DicInfo[pair]);
                                //Selected_Pic.Add(pair);
                                
                                if(sdip_200_frame.Contains(dicInfo[pair].FrameNo))
                                {

                                }
                                else
                                {
                                    sdip_200_frame.Add(dicInfo[pair].FrameNo);
                                }
                                dicInfo.Remove(pair);
                            }

                        }
                    }
                    else if(View_Mode_RB.Checked)
                    {
                        curruent_viewtype = 1;
                        //SDIP 코드 211~230 제외
                        foreach (string pair in dicInfo.Keys.ToList())
                        {
                            if (F5_Img_KeyList_Main.Contains(pair))
                            {
                                F5_code_dicInfo.Add(pair, dicInfo[pair]);
                            }

                    



                            if (dicInfo[pair].sdip_no != "-" && 200 <= int.Parse(dicInfo[pair].sdip_no) && int.Parse(dicInfo[pair].sdip_no) <= 299)
                            {
                                Sdip_200_code_dicInfo.Add(pair, DicInfo[pair]);
                                //Selected_Pic.Add(pair);

                                if (sdip_200_frame.Contains(dicInfo[pair].FrameNo))
                                {

                                }
                                else
                                {
                                    sdip_200_frame.Add(dicInfo[pair].FrameNo);
                                }
                            }
                             
                               
                          

                        }
                    }
                    else
                    {
                        curruent_viewtype = 0;
                    }
                }

                if(Sdip_200_code_dicInfo.Count>0)
                {
                    Real_SDIP_200_DIc = new Dictionary<string, ImageInfo>(Sdip_200_code_dicInfo);
                }
                    
                if(path_check != "")
                {

                    Sorted_dic_GRID = DicInfo.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    DicInfo = Sorted_dic_GRID;

                    Set_Dl_PrintList();
                    //First_Print_List();

                    All_LotID_List.Sort();
                    Initial_Equipment_DF_List();
                    Code_200_Set();

                    if (checkBox1.Checked)
                    {
                        ImageSize_CB.Items.Clear();
                        ImageSize_CB.Items.Add("");
                        for (int i = 0; i < ImageSizeList.Count; i++)
                            ImageSize_CB.Items.Add(ImageSizeList.ElementAt(i));

                        //ImageSize_CB.SelectedIndex = 0;
                    }


                    for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                        Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);


                    Select_All_BTN_Click(null, null);
                    MessageBox.Show(MSG_STR.SUCCESS);

                    if (Overlap_key_Main.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach(string key in Overlap_key_Main)
                        {
                            sb.Append(key).Append("\n");
                        }
                        Clipboard.SetText(sb.ToString());
                        MessageBox.Show("아래의 키는 중복되어 리스트에 제외 되었습니다" +"\n" + "\n" +sb.ToString()+"\n"+"클립보드에 저장되었습니다.");

                        

                    }

                    open.Main = this;
                    open.Set_View();
                    Setting = 1;
                    open.Setting = Setting;
                }

       
            }
            else
            {

            }

        }
        private void Code_200_Set()
        {
            int x = 1;
            int index = 0;
            CODE_200_List.Clear();
            Sdip_result_dic.Clear();
            foreach (KeyValuePair<string, ImageInfo> pair in Sdip_200_code_dicInfo)
            {
                if (CODE_200_List.FindIndex(s => s.Item1.Equals(pair.Value.sdip_no)) == -1)
                {
                    x = 1;
                    CODE_200_List.Add(new Tuple<string, int>(pair.Value.sdip_no, x));
                    Sdip_result_dic.Add(pair.Value.sdip_no, pair.Value.EquipmentDefectName);
                }
                else
                {
                    index = CODE_200_List.FindIndex(s => s.Item1.Equals(pair.Value.sdip_no));
                    x = CODE_200_List[index].Item2;
                    CODE_200_List[index] = new Tuple<string, int>(pair.Value.sdip_no, ++x);

                }

            }

            CODE_200_List = CODE_200_List.OrderBy(s => s.Item1).ThenBy(s => s.Item1).ToList();


        }
        public void Code_200_Filter()
        {
            open.Code_200_Set_View();

        }
        private void update_Equipment_DF_CLB(List<string> deleted_pic)
        {
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = { '-' };
            
            foreach(string id in deleted_pic)
            {
                if(F5_code_dicInfo.ContainsKey(id))
                {
                    return;
                
                }
            }

            for (int i = 0; i < deleted_pic.Count; i++)
            {
                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName)) == -1)
                {
                    continue;
                }
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[deleted_pic[i]].EquipmentDefectName, --x);

                    if (x == 0)
                    {
                        All_Equipment_DF_List.RemoveAt(index);

                    }

                }

            }
            Equipment_DF_CLB.Items.Clear();
            Initial_Equipment_DF_List();
            List<string> checked_name = new List<string>(Selected_Equipment_DF_List);

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            if (EQ_Search_TB.Text != "")
            {
                EQ_Search();

            }


            for (int p = 0; p < Equipment_DF_CLB.CheckedItems.Count; p++)
            {
                changed_eq.Add(Equipment_DF_CLB.CheckedItems[p].ToString());
            }


            for (int p = 0; p < changed_eq.Count; p++)
            {
                if (Equipment_DF_CLB.Items.Contains(changed_eq[p]))
                {
                    index = Equipment_DF_CLB.Items.IndexOf(changed_eq[p]);

                    string[] rep = changed_eq[p].Split(sd);
                    int index2 = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(rep[0]));

                    Equipment_DF_CLB.Items[index] = All_Equipment_DF_List.ElementAt(index2).Item1 + "-" + All_Equipment_DF_List.ElementAt(index2).Item2;
                }

            }

            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
            {
                if (checked_name.Contains(Equipment_DF_CLB.Items[i].ToString().Split('-')[0]))
                {
                    Equipment_DF_CLB.SetItemChecked(i, true);
                }
                else
                    Equipment_DF_CLB.SetItemChecked(i, false);
            }

            
            if (checked_name.Count > 0)
            {

                for (int i = 0; i < checked_name.Count; i++)
                {
                    bool contain_ch = All_Equipment_DF_List.Any(c => c.Item1.Equals(checked_name[i]));
                    
                    if (contain_ch)
                    {
                        Equipment_DF_CLB.SelectedIndex = Equipment_DF_CLB.Items.IndexOf(Equipment_DF_CLB.CheckedItems[0].ToString());

                        return;
                    }
                    else
                    {

                    }
                }
                
            }
                
        }

        public void Update_Code_No200_List(List<string> deleted_pic)
        {
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = { '-' };

            for (int i = 0; i < deleted_pic.Count; i++)
            {
                if (CODE_200_List.FindIndex(s => s.Item1.Equals(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName)) == -1)
                    continue;
                else
                {
                    index = CODE_200_List.FindIndex(s => s.Item1.Equals(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = CODE_200_List[index].Item2;
                    CODE_200_List[index] = new Tuple<string, int>(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName, --x);

                    if (x == 0)
                    {
                        CODE_200_List.RemoveAt(index);

                    }

                }

            }


        }

        public void Return_Update_Code_No200_List(List<string> deleted_pic)
        {
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = { '-' };

            for (int i = 0; i < deleted_pic.Count; i++)
            {
                if (CODE_200_List.FindIndex(s => s.Item1.Equals(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName)) == -1)
                    continue;
                else
                {
                    index = CODE_200_List.FindIndex(s => s.Item1.Equals(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = CODE_200_List[index].Item2;
                    CODE_200_List[index] = new Tuple<string, int>(Sdip_200_code_dicInfo[deleted_pic[i]].EquipmentDefectName, ++x);

                    if (x == 0)
                    {
                        CODE_200_List.RemoveAt(index);

                    }

                }

            }


        }

        private void Return_update_Equipment_DF_CLB(List<string> deleted_pic)
        {
            Initial_Equipment_DF_List();
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = { '-' };

            foreach (string id in deleted_pic)
            {
                if (F5_code_dicInfo.ContainsKey(id))
                {
                    return;

                }
            }

            for (int i = 0; i < deleted_pic.Count; i++)
            {

                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName)) == -1)
                    All_Equipment_DF_List.Add(new Tuple<string, int>(dicInfo[deleted_pic[i]].EquipmentDefectName, x));
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[deleted_pic[i]].EquipmentDefectName, ++x);

                }

            }

            Equipment_DF_CLB.Items.Clear();
            Initial_Equipment_DF_List();

            List<string> checked_name = new List<string>(Selected_Equipment_DF_List);

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            if (EQ_Search_TB != null)
            {
                EQ_Search();

            }
            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
            {
                if (checked_name.Contains(Equipment_DF_CLB.Items[i].ToString().Split('-')[0]))
                {
                    Equipment_DF_CLB.SetItemChecked(i, true);
                }
                else
                    Equipment_DF_CLB.SetItemChecked(i, false);
            }

        }

        private void Return_update_Code200_List(List<string> deleted_pic)
        {

        }
        private void Width_TB_TextChanged(object sender, EventArgs e)
        {
            Height_TB.Text = Width_TB.Text;

            switch (ViewType)
            {
                case "SetView":
                    open.Set_View();
                    break;

                case "FrameSetView":
                    open.Frame_Set_View();
                    break;

                case "DLFrameSetView":
                    open.DL_Frame_Set_View();
                    break;

                case "EQCBSetView":
                    open.Eq_CB_Set_View();
                    break;

                case "EQCBSetView_ING":
                    open.Eq_CB_Set_View_ING();
                    break;

                case "FilterCBafterSetView":
                    open.Filter_CB_after_Set_View();
                    break;

                case "Code_200_SetView":
                    open.Code_200_Set_View();
                    break;


            }
        }

        private void Width_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(Fixed_CB.Checked == true)
                {
                    Height_TB.Text = Width_TB.Text;
                }
                else
                {

                }
                
                {
                    switch (ViewType)
                    {
                        case "SetView":
                            open.Set_View();
                            break;

                        case "FrameSetView":
                            open.Frame_Set_View();
                            break;

                        case "DLFrameSetView":
                            open.DL_Frame_Set_View();
                            break;

                        case "EQCBSetView":
                            open.Eq_CB_Set_View();
                            break;

                        case "EQCBSetView_ING":
                            open.Eq_CB_Set_View_ING();
                            break;

                        case "FilterCBafterSetView":
                            open.Filter_CB_after_Set_View();
                            break;

                        case "Code_200_SetView":
                            open.Code_200_Set_View();
                            break;

                    }

                }
            }
        }

        private void Rows_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                switch (ViewType)
                {
                    case "SetView":
                        open.Set_View();
                        break;

                    case "FrameSetView":
                        open.Frame_Set_View();
                        break;

                    case "DLFrameSetView":
                        open.DL_Frame_Set_View();
                        break;

                    case "EQCBSetView":
                        open.Eq_CB_Set_View();
                        break;

                    case "EQCBSetView_ING":
                        open.Eq_CB_Set_View_ING();
                        break;

                    case "FilterCBafterSetView":
                        open.Filter_CB_after_Set_View();
                        break;

                }

            }


        }

        private void Print_Image_State_CheckedChanged(object sender, EventArgs e)
        {
            if (Frame_View_CB.Checked)
                open.Frame_Cheked_State_DF();
            else
                open.Cheked_State_DF();
        }

        private void Print_Image_Name_CheckedChanged(object sender, EventArgs e)
        {

            if (Frame_View_CB.Checked)
                open.Frame_Cheked_State_DF();
            else
                open.Cheked_State_DF();


        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //All_Equipment_DF_List_DEL.Clear();
            //int x = 1;
            //int index = 0;
            Wait_Del_Img_List = open.DicInfo_Delete.Keys.ToList();
            DeleteWaiting deleteWaiting = new DeleteWaiting(this);
            //foreach(string pair in Waiting_Del.Keys.ToList())
            //{
            //    if (All_Equipment_DF_List_DEL.FindIndex(s => s.Item1.Equals(Waiting_Del[pair].EquipmentDefectName)) == -1)
            //        All_Equipment_DF_List_DEL.Add(new Tuple<string, int>(Waiting_Del[pair].EquipmentDefectName, 1));
            //    else
            //    {
            //        index = All_Equipment_DF_List_DEL.FindIndex(s => s.Item1.Equals(Waiting_Del[pair].EquipmentDefectName));
            //        x = All_Equipment_DF_List_DEL[index].Item2;
            //        All_Equipment_DF_List_DEL[index] = new Tuple<string, int>(Waiting_Del[pair].EquipmentDefectName, ++x);

            //    }
            //}
            //deleteWaiting.All_Equipment_DF_List = All_Equipment_DF_List_DEL;
            deleteWaiting.Waiting_Img = new Dictionary<string,ImageInfo>(Waiting_Del);
            deleteWaiting.ZipFilePath = zipFilePath;
            deleteWaiting.Set_View_Del();
            deleteWaiting.Set_EQ();
            
            deleteWaiting.ShowDialog();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 0)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells["이름"].Value.ToString().Substring(0, 12);

                open.SelectGrid_Img_View(id);

            }


        }
        public void SetFocus()
        {
            open.Focus();
        }
        public void Delete_ZipImg()
        {

            Func.DeleteJPG_inZIP(zipFilePath, dicInfo_Waiting_Del);

            foreach (KeyValuePair<string, ImageInfo> pair in dicInfo_Waiting_Del)
            {
                if (DicInfo.ContainsKey(pair.Key))
                    DicInfo.Remove(pair.Key);
            }


            dicInfo_Waiting_Del.Clear();
            ((DataTable)dataGridView2.DataSource).Rows.Clear();

            int ci = open.DicInfo_Delete.Count;

        }
        private void All_Clear()
        {
            if(Frame_View_CB.Checked || Frame_Interval_CB.Checked || Exceed_CB.Checked)
            {
                Frame_View_CB.Checked = false;
                Frame_Interval_CB.Checked = false;
                Exceed_CB.Checked = false;
            }
            Eq_filter = 0;
            textBox4.Text = "";
            EQ_Search_TB.Text = "";
            Frame_S_TB.Text = "";
            Frame_E_TB.Text = "";
            Frame_S_Page_TB.Text = "";
            Frame_E_Page_TB.Text = "";
            Camera_NO_Filter_TB.Text = "";
            CODE_200_List.Clear();
            Setting = 0;
            Exception_Frame.Clear();
            Final_text_main = string.Empty;
            Eq_CB_dicInfo.Clear();
            Map_List_Dic_main.Clear();
            Map_List_Dic_Compare_main.Clear();
            Filter_200_dic_Main.Clear();
            Exceed_filter.Clear();
            Sdip_200_code_dicInfo.Clear();
            F9_code_dicInfo.Clear();
            F10_code_dicInfo.Clear();

            F5_code_dicInfo.Clear();
            Sdip_NO1_dicInfo.Clear();
            DicInfo_Copy.Clear();
            Return_dicInfo.Clear();
            Waiting_Del.Clear();

            Frame_List_Main.Clear();
            Sdip_result_dic.Clear();
            F9_Frame_List_Main.Clear();

            Exceed_List.Clear();
            F10_Frame_List_Main.Clear();

            F5_Img_KeyList_Main.Clear();


            mAP_LIST.Clear();
            Contain_200_Frame_Main.Clear();
            selected_Pic.Clear();

            Dl_Apply_List_Main.Clear();

            Dl_NotApply_List_Main.Clear();
            Dl_List_Main.Clear();

            ZipFilePath = string.Empty;
            REF_DirPath = string.Empty;
            DirPath = string.Empty;
            F12_del_list_main.Clear();

            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();

            DataTable dt2 = (DataTable)dataGridView2.DataSource;

            dt2.Rows.Clear();




        }

        private void FormViewPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Zip_Error)
            {

            }
            else
            {
                if (Waiting_Del.Count > 0)
                {
                    if (MessageBox.Show("" + Waiting_Del.Count + "개의 이미지를 삭제하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Delete_ZipImg();
                        Dispose(true);
                    }
                    else if (MessageBox.Show("프로그램을 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Dispose(true);

                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }


                }

                else if (MessageBox.Show("프로그램을 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Dispose(true);

                }
                else
                {
                    e.Cancel = true;
                    return;
                }

            }



        }

        private void 중간저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("현재 삭제대기 정보를 저장하시겠습니까? \r 주의: Zip 파일의 이미지를 삭제 한 후에는 저장이 되지 않습니다", "알림", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                F12_del_list_main = new List<string>(open.F12_del_list);
                Func.SaveDelFileID(Waiting_Del, F12_del_list_main);
                
            }
            else
            {

            }
            

        }

        private void 저장불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Func.LoadDelFileID(this, Waiting_Del, dicInfo_Copy);

        }


        public void Load_saveFile()
        {
            open.DicInfo_Delete = Waiting_Del;
            open.Load_Del();
            Dl_PrintList();
            Wait_Del_Print_List();
            List_Count_TB.Text = String.Format("{0:#,##0}", open.DicInfo_Filtered.Count);

        }

        private void Fixed_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Fixed_CB.Checked == true)
            {
                Height_TB.Enabled = true;
                Height_TB.ReadOnly = true;
                Height_TB.Text = Width_TB.Text;

               
                    if (Fixed_CB.Checked == true)
                    {
                        Height_TB.Text = Width_TB.Text;
                    }
                    else
                    {

                    }
                        switch (ViewType)
                        {
                            case "SetView":
                                open.Set_View();
                                break;

                            case "FrameSetView":
                                open.Frame_Set_View();
                                break;

                            case "DLFrameSetView":
                                open.DL_Frame_Set_View();
                                break;

                            case "EQCBSetView":
                                open.Eq_CB_Set_View();
                                break;

                            case "EQCBSetView_ING":
                                open.Eq_CB_Set_View_ING();
                                break;

                            case "FilterCBafterSetView":
                                open.Filter_CB_after_Set_View();
                                break;

                            case "Code_200_SetView":
                                open.Code_200_Set_View();
                                break;

                        }

                 
                
            }
            else
            {
                Height_TB.Enabled = true;
                Height_TB.ReadOnly = false;
            }
                
            
        }

        private void Frame_View_CB_CheckedChanged(object sender, EventArgs e)
        {
            Camera_NO_Filter_TB.Text = "";
            textBox4.Text = "";

            if (Frame_View_CB.Checked && Exceed_CB.Checked)
            {
                open.Frame_Set_View();
            }
            else if (Frame_View_CB.Checked && !Exceed_CB.Checked)
            {
                open.Frame_Set_View();
                Exceed_CB.Enabled = false;
            }
            else if(Eq_filter == 1)
            {
                return;
            }
            else
            {
                Exceed_CB.Enabled = true;
                Frame_S_Page_TB.Text = "";
                Frame_E_Page_TB.Text = "";
                Frame_S_TB.Text = "";
                Frame_E_TB.Text = "";
                Exceed_CB.CheckState = CheckState.Unchecked;

                open.Set_View();

                EQ_Search_TB.Text = null;
                Initial_Equipment_DF_List();
                Equipment_DF_CLB.Items.Clear();

                for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                    Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

                
                Print_List();
                Select_All_BTN_Click(null, null);
            }
        }

        //private void Manual_Mode_RB_CheckedChanged(object sender, EventArgs e)
        //{

        //    if(DicInfo.Count > 0)
        //    {
        //        if(View_Mode_RB.Checked)
        //        {
        //            MessageBox.Show("Load 전에 바꿔주십시오.");
        //            View_Mode_RB.disabled
        //        }
        //    }
        //}

        //private void View_Mode_RB_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (DicInfo.Count > 0)
        //    {
        //        if (Manual_Mode_RB.Checked)
        //        {
        //            MessageBox.Show("Load 전에 바꿔주십시오.");
        //            View_Mode_RB.Checked = true;
        //        }
        //    }
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            EQ_Search();
        }

        private void EQ_Search()
        {
            Eq_filter = 1;
            string txt = EQ_Search_TB.Text;
            txt = txt.ToUpper();

            for (int i = 0; i < Equipment_DF_CLB.Items.Count; i++)
            {
                if (Equipment_DF_CLB.Items[i].ToString().Contains(txt))
                    continue;
                else
                {
                    Equipment_DF_CLB.Items.RemoveAt(i);
                    i--;
                }

            }
            Select_All_BTN_Click(null, null);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Eq_filter = 0;
            EQ_Search_TB.Text = null;
            Initial_Equipment_DF_List();
            Equipment_DF_CLB.Items.Clear();

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            Select_All_BTN_Click(null, null);
        }

        private void 번코드변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selected_Pic = open.Select_Pic_List;

            Change_Code change = new Change_Code(this);
            change.ShowDialog();

            if (open.OpenViewType == "Code_200_SetView")
            {

                Sorted_dic = DicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                DicInfo = Sorted_dic;

                Return_update_Equipment_DF_CLB(selected_Pic);
            }
            selected_Pic.Clear();
            if(MessageBox.Show("코드 변경 후 IMG TXT 파일 변경 하시겠습니까?","알림",MessageBoxButtons.YesNo)== System.Windows.Forms.DialogResult.Yes)
            {
                Func.Write_IMGTXT_inZip(ZipFilePath, DicInfo, Sdip_200_code_dicInfo, DicInfo_Copy, open.F12_del_list);
                MessageBox.Show("변경되었습니다.");
            }
            if (MessageBox.Show("코드 변경 후 MAP TXT 파일 변경 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                mAPTXTUpdateToolStripMenuItem1_Click(null, null);
               
            }
        }

        private void Camera_NO_Filter_TB_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.KeyCode == Keys.Enter)
            {
                Filter_Check();
            }
            
            //int Num;
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (Camera_NO_Filter_TB.Text == "")
            //    {
            //        open.DicInfo_Filtered = new Dictionary<string, ImageInfo>(DicInfo); 
            //        open.Set_View();
            //        Print_List();
            //        Camera_NO_Filter_TB.Text = "";
            //        return;

            //    }
            //    //else if (!int.TryParse(Camera_NO_Filter_TB.Text, out Num))
            //    //{
            //    //    MessageBox.Show("입력을 숫자로 부탁드립니다.");
            //    //    Camera_NO_Filter_TB.Text = "";
            //    //    return;
            //    //}

            //    string[] Split_String = null;
            //    Split_String = Camera_NO_Filter_TB.Text.Split(',');


            //    if (int.Parse(Split_String[0]) > 0)
            //    {

            //        if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            //        {
            //            foreach (string No in open.Frame_dicInfo_Filter.Keys.ToList())
            //            {
            //                if (open.Frame_dicInfo_Filter.ContainsKey(No))
            //                {
            //                    if (Split_String.Contains(open.Frame_dicInfo_Filter[No].CameraNo.ToString()))
            //                    {
            //                        continue;
            //                    }
            //                    else
            //                        open.Frame_dicInfo_Filter.Remove(No);

            //                    if (open.Frame_dicInfo_Filter.Count == 0)
            //                    {
            //                        MessageBox.Show("해당 카메라 이미지가 없습니다.");
            //                        Camera_NO_Filter_TB.Text = string.Empty;
            //                    }
            //                }
            //            }
            //            open.Frame_Set_Image();
            //        }
            //        else
            //        {
            //            if(Eq_CB_dicInfo.Count >0)
            //            {
            //                Sorted_dic = new Dictionary<string, ImageInfo>(Eq_CB_dicInfo);
            //                Dictionary<string, ImageInfo> before_dic = new Dictionary<string, ImageInfo>(open.DicInfo_Filtered);
            //                open.DicInfo_Filtered.Clear();

            //                foreach(string No in Sorted_dic.Keys.ToList())
            //                {
            //                    if (Sorted_dic.ContainsKey(No))
            //                    {
            //                        if (Split_String.Contains(Sorted_dic[No].CameraNo.ToString()))
            //                        {

            //                        }
            //                        else
            //                        {
            //                            Sorted_dic.Remove(No);
            //                        }

            //                    }
            //                }

            //                open.DicInfo_Filtered = Sorted_dic;

            //                if (open.DicInfo_Filtered.Count == 0)
            //                {
            //                    open.DicInfo_Filtered = new Dictionary<string, ImageInfo>(before_dic);
            //                    MessageBox.Show("해당 카메라 이미지가 없습니다.");
            //                    //Camera_NO_Filter_TB.Text = string.Empty;

            //                    open.Set_Image();
            //                    Print_List();
            //                    //Sorted_dic.Clear();

            //                }
            //                else
            //                {
            //                    open.Set_Image();
            //                    Print_List();
            //                    //Sorted_dic.Clear();
            //                }

            //            }
            //            else
            //            {
            //                Sorted_dic = new Dictionary<string, ImageInfo>(open.DicInfo_Filtered);
            //                open.DicInfo_Filtered.Clear();
            //                foreach (string No in DicInfo.Keys.ToList())
            //                {
            //                    if (DicInfo.ContainsKey(No))
            //                    {
            //                        if (Split_String.Contains(DicInfo[No].CameraNo.ToString()))
            //                        {
            //                            open.DicInfo_Filtered.Add(No, DicInfo[No]);
            //                        }

            //                    }
            //                }

            //                if (open.DicInfo_Filtered.Count == 0)
            //                {
            //                    open.DicInfo_Filtered = new Dictionary<string, ImageInfo>(Sorted_dic);
            //                    MessageBox.Show("해당 카메라 이미지가 없습니다.");
            //                    //Camera_NO_Filter_TB.Text = string.Empty;
            //                }
            //                open.Set_Image();
            //                Print_List();
            //                Sorted_dic.Clear();
            //            }

            //            //Sorted_dic.Clear();


            //        }

            //        Print_Image_BT_Click(null, null);

            //    }
            //    else if (Camera_NO_Filter_TB.Text == "")
            //    {
            //        switch (ViewType)
            //        {
            //            case "SetView":
            //                open.Set_View();
            //                Print_List();
            //                break;

            //            case "FrameSetView":
            //                open.Frame_Set_View();
            //                break;

            //            case "DLFrameSetView":
            //                open.DL_Frame_Set_View();
            //                break;

            //            case "EQCBSetView":
            //                open.Eq_CB_Set_View();
            //                break;

            //            case "EQCBSetView_ING":
            //                open.Eq_CB_Set_View_ING();
            //                break;

            //            case "FilterCBafterSetView":
            //                open.Filter_CB_after_Set_View();
            //                break;

            //        }
            //        Print_Image_BT_Click(null, null);
            //    }
            //    else
            //        MessageBox.Show("숫자만 입력해주세요.");



            //    Print_Image_BT.Focus();
            //}
        }

        private void Print_Image_BT_Click(object sender, EventArgs e)
        {
            switch (ViewType)
            {
                case "SetView":
                    open.Set_View();
                    break;

                case "FrameSetView":
                    open.Frame_Set_View();
                    break;

                case "DLFrameSetView":
                    open.DL_Frame_Set_View();
                    break;

                case "EQCBSetView":
                    open.Eq_CB_Set_View();
                    break;

                case "EQCBSetView_ING":
                    open.Eq_CB_Set_View_ING();
                    break;

                case "FilterCBafterSetView":
                    open.Filter_CB_after_Set_View();
                    break;

                case "Code_200_SetView":
                    open.Code_200_Set_View();
                    break;

            }
            Filter_Check();
        }

        private void Rotate_CLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRotation_Option(Rotate_CLB.SelectedIndex);

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {
                open.Frame_Set_Image();
            }
            else if (ViewType == "SetView" || ViewType == "EQCBSetView")
            {
                open.Set_Image();
            }
        }

        private void ImageSize_Filter()
        {
            Dictionary<string, ImageInfo> IamgeSize_dic = new Dictionary<string, ImageInfo>(DicInfo);
            if (ImageSize_CB.SelectedIndex == 0)
                return;

            if (checkBox1.Checked)
            {
                

                foreach (string pair in IamgeSize_dic.Keys.ToList())
                {
                    if (!ImageSize_CB.SelectedItem.Equals(IamgeSize_dic[pair].ImageSize))
                    {
                        IamgeSize_dic.Remove(pair);
                    }
                }
                open.DicInfo_Filtered = IamgeSize_dic;
            }

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {

                open.Frame_Set_View();
            }
            else if (ViewType == "SetView")
            {
                open.Set_View();
                Print_List();
            }
        }

        private void ImageSize_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageSize_Filter_NO = 1;
            ImageSize_Filter();
            ImageSize_Filter_NO = 0;
        }

        private void Code_200_View_Click(object sender, EventArgs e)
        {
            Code_200_Set();
            Code200View code200 = new Code200View(this);
            code200.ShowDialog();
        }

        private void EQ_Search_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Eq_filter = 1;
                EQ_Search();
            }
        }

        private void FilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYLocationFilter XYFilter = new XYLocationFilter(open,open.Befroe_X,open.Before_Y);
            XYFilter.ShowDialog();
        }

        private void iMGTXTUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" IMG TXT를 변경하시겠습니까?", "IMG TXT Update", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    
                   
                    waitform.Show();
                    Func.Write_IMGTXT_inZip(ZipFilePath, DicInfo, Sdip_200_code_dicInfo, DicInfo_Copy, open.F12_del_list);
                    waitform.Close();
                    MessageBox.Show("변경되었습니다.");

                }
                catch (Exception ex)
                {
                    waitform.Close();
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        public void mAPTXTUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            

            if (MessageBox.Show(" MAP TXT를 변경하시겠습니까?", "IMG TXT Update", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    waitform.Show();
                    final_Frame_List.Clear();

                    if (open.F12_del_list.Count > 0)
                    {
                        foreach (string pair in open.F12_del_list)
                        {
                            if (dicInfo.ContainsKey(pair))
                            {

                            }
                            else
                            {
                                dicInfo.Add(pair, DicInfo_Copy[pair]);
                            }
                        }
                    }


                    foreach (string pair in DicInfo.Keys.ToList())
                    {
                        if (final_Frame_List.Contains(DicInfo[pair].FrameNo))
                        {

                        }
                        else
                        {
                            if (DicInfo[pair].sdip_no != "1")
                            {
                                final_Frame_List.Add(DicInfo[pair].FrameNo);
                            }


                        }
                    }

                    Counting_IMG_inZip(ZipFilePath);

             

                    Dictionary<int, int> special_frame = new Dictionary<int, int>();
                    foreach (KeyValuePair<int, int> pair in Map_List_Dic_main)
                    {

                        if (pair.Value == 88)
                        {
                                if (Map_List_Dic_Compare_main[pair.Key] != 39 && Map_List_Dic_Compare_main[pair.Key] != 40)
                                {
                                    
                                }
                                else
                                {
                                special_frame.Add(pair.Key, Map_List_Dic_main[pair.Key]);
                                }
                        }

                    }

                    
                   
                    foreach(int pair in Map_List_Dic_main.Keys.ToList())
                    {
                        if(Map_List_Dic_main[pair] !=88)
                        {
                            if(special_frame.ContainsKey(pair))
                            {

                            }
                            else
                            {
                                special_frame.Add(pair, Map_List_Dic_main[pair]);
                            }
                            
                        }
                    }
                   
                    foreach(int pair in special_frame.Keys.ToList())
                    {
                        if(Exception_Frame.Contains(pair))
                        {
                            Exception_Frame.Remove(pair);
                        }
                    }

                    foreach (int pair in Map_List_Dic_main.Keys.ToList())
                    {
                        if (Exception_Frame.Contains(pair))
                        {
                            Map_List_Dic_main.Remove(pair);
                        }
                    }
                    foreach (int pair in Map_List_Dic_Compare_main.Keys.ToList())
                    {
                        if (Exception_Frame.Contains(pair))
                        {
                            Map_List_Dic_Compare_main.Remove(pair);
                        }
                    }
                   



                    foreach (int pair in final_Frame_List)
                    {
                        if(Map_List_Dic_main.Keys.ToList().Contains(pair))
                        {

                        }
                        else
                        {
                            Map_List_Dic_main.Add(pair, 88);
                            Map_List_Dic_Compare_main.Add(pair, 31);
                        }
                        
                    }
                    List<int> chec = new List<int>();
                    int last = Map_List_Dic_main.Count;
                    int last_frame = Map_List_Dic_main.ElementAt(last-1).Key;
                    foreach (int frame_del in Map_List_Dic_main.Keys.ToList())
                    {
                        if(final_Frame_List.Contains(frame_del))
                        { }
                        else
                        {
                            if (frame_del == 1)
                            {

                            }
                            else if (frame_del == last_frame)
                            {

                            }
                            else if (special_frame.Keys.ToList().Contains(frame_del))
                            { }
                            else if(sdip_200_frame.Contains(frame_del))
                            { }
                            else
                            {
                                Map_List_Dic_main.Remove(frame_del);
                            }
                            
                        }

                    }

                    last = Map_List_Dic_Compare_main.Count;
                    last_frame = Map_List_Dic_Compare_main.ElementAt(last - 1).Key;
                   
                    foreach (int frame_del in Map_List_Dic_Compare_main.Keys.ToList())
                    {
                        if (final_Frame_List.Contains(frame_del))
                        { }
                        else
                        {
                            if (frame_del == 1)
                            {

                            }
                            else if (frame_del == last_frame)
                            {

                            }
                            else if (special_frame.Keys.ToList().Contains(frame_del))
                            { }
                            else if (sdip_200_frame.Contains(frame_del))
                            { }
                            else
                            {
                                Map_List_Dic_Compare_main.Remove(frame_del);
                            }
                        }

                    }


                    Sorted_Map_dic = Map_List_Dic_main.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    Map_List_Dic_main = new Dictionary<int,int>(Sorted_Map_dic);
                    Sorted_Map_dic.Clear();

            


                    Sorted_Map_dic = Map_List_Dic_Compare_main.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                    Map_List_Dic_Compare_main = new Dictionary<int, int>(Sorted_Map_dic);
                    Sorted_Map_dic.Clear();
                    Map_TXT_NO_Counting.Clear();

                    foreach (int pair in Map_List_Dic_main.Keys.ToList())
                    {
                        if (Map_TXT_NO_Counting.ContainsKey(Map_List_Dic_main[pair]) == false)
                            Map_TXT_NO_Counting.Add(Map_List_Dic_main[pair], 1);
                        else
                        {
                            Map_TXT_NO_Counting[Map_List_Dic_main[pair]] = Map_TXT_NO_Counting[Map_List_Dic_main[pair]] + 1;
                        }

                    }


                    Func.Map_TXT_Update_inZip(ZipFilePath, Map_TXT_NO_Counting, Map_List_Dic_main, Map_List_Dic_Compare_main, Between);
                    waitform.Close();
                    MessageBox.Show("변경되었습니다.");
                }
                catch (Exception ex)
                {
                    waitform.Close();
                    MessageBox.Show(ex.ToString());
                }

            }



        }

        private void Update_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Util.OpenFolderDlg();

            if (!string.IsNullOrEmpty(path)) //예외처리
            {
                string Version = string.Empty;


                
                if (NetworkFunc.SaveFilesToStemcoNas(path, out Version))
                {
                    string LastVersion = NetworkFunc.GetLastViewPortVersion(MYSQL_STR.CONNECTION_STEMCO);

                    if (LastVersion != Version) // 지난버전 번호랑 다르면..! 여기서 작거나 높은 처리 추가하면 좋을듯함
                    {
                        if (NetworkFunc.SaveVersionToStemcoDB(Version, "상세설명 추가", "1"))
                        {
                            MessageBox.Show("성공");
                        }
                    }
                }
            }

        }

        private void xyFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYLocationFilter XYFilter = new XYLocationFilter(open,open.Befroe_X,open.Before_Y);
            XYFilter.ShowDialog();
        }

        private void S_Page_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                open.Page_Filter(int.Parse(S_Page_TB.Text));
            }
        }

        private void Print_Image_EQ_CheckedChanged(object sender, EventArgs e)
        {

            if (Frame_View_CB.Checked)
                open.Frame_Cheked_State_DF();
            else
                open.Cheked_State_DF();
        }

        private void Exceed_CB_CheckedChanged(object sender, EventArgs e)
        {
            string Exceed_No = Exceed_TB.Text;
            int x = 0;
            exceed_Count.Clear();
            exceed_List.Clear();
            exceed_filter.Clear();

            if (Exceed_CB.Checked)
            {
                foreach (string pair in DicInfo.Keys.ToList())
                {
                    if (!exceed_Count.ContainsKey(DicInfo[pair].FrameNo))
                    {
                        exceed_Count.Add(DicInfo[pair].FrameNo, 1);
                        exceed_List.Add(DicInfo[pair].FrameNo);
                    }
                    else
                    {
                        x = exceed_Count[DicInfo[pair].FrameNo] + 1;
                        exceed_Count[DicInfo[pair].FrameNo] = x;
                    }

                }

                foreach (int pair in exceed_Count.Keys.ToList())
                {
                    if (exceed_Count[pair] >= int.Parse(Exceed_No))
                    {
                        continue;
                    }
                    else
                    {
                        exceed_Count.Remove(pair);
                        exceed_List.Remove(pair);
                    }

                }

                if (exceed_List.Count > 0)
                {
                    foreach (string pair in open.DicInfo_Filtered.Keys.ToList())
                    {
                        if (exceed_Count.ContainsKey(open.DicInfo_Filtered[pair].FrameNo))
                        {
                            exceed_filter.Add(pair, open.DicInfo_Filtered[pair]);
                        }

                    }

                    open.Set_View();
                    Print_List();
                }
                else
                {
                    MessageBox.Show("해당 프레임이 없습니다.");
                    Exceed_TB.Text = "41";
                    Exceed_CB.Checked = false;
                }
                    
               
            }

        }

        private void Frame_S_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Filter_Check();
            }
            //int Num;
            //List<int> Frame_filter_List = new List<int>();
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if(Frame_S_TB.Text == "")
            //    {
            //        open.DicInfo_Filtered = new Dictionary<string, ImageInfo>(DicInfo);
            //        open.Set_View();
            //        Print_List();
            //        Frame_E_TB.Text = "";
            //        return;

            //    }
            //    else if(!int.TryParse(Frame_S_TB.Text,out Num))
            //    {
            //        MessageBox.Show("입력을 숫자로 부탁드립니다.");
            //        Frame_S_TB.Text = "";
            //        return;
            //    }
            //    open.Set_Frame_Filter();

            //    if (open.Frame_List_Img.Contains(int.Parse(Frame_S_TB.Text)))
            //    {
            //        open.Frame_Filter(int.Parse(Frame_S_TB.Text));
            //    }
            //    else
            //    {

            //        MessageBox.Show("해당 프레임은 존재하지 않습니다.");
            //        Frame_S_TB.Text = Frame_E_TB.Text;
            //    }

            //}
            //open.Frame_List_Img.Clear();
        }

        private void FormViewPort_MouseMove(object sender, MouseEventArgs e)
        {

            this.Focus();
        }

        private void listFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Setting == 1)
            {
                ListFilertView Listfilter = new ListFilertView(this);
                Listfilter.ShowDialog();
                Set_List_Filter();
            }
            else
            {
                MessageBox.Show("로드 후 사용 부탁드립니다");
            }
           
        }

        private void frameListFilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Setting == 1)
            {
                ListFilertView Listfilter = new ListFilertView(this);
                Listfilter.ShowDialog();
                Set_Frame_List_Filter();
            }
            else
            {
                MessageBox.Show("로드 후 사용 부탁드립니다");
            }

        }

        public void Set_List_Filter()
        {
            ListFiler = "List_FIlter";
            List_filter = 1;
            set_filter.Clear();

            if (List_Filter_Main.Count > 0 && List_Filter_Main[0].Length > 7)
            {
                for (int i = 0; i < List_Filter_Main.Count; i++)
                {

                    set_filter.Add(List_Filter_Main[i], DicInfo[List_Filter_Main[i]]);

                }
                open.DicInfo_Filtered = set_filter;
                open.Set_View();

                Print_List();
            }

            List_filter = 0;
        }

        public void Set_Frame_List_Filter()
        {
            ListFiler = "FrameList_FIlter";
            List_filter = 1;
            set_filter.Clear();

            if (List_Filter_Main.Count > 0 && List_Filter_Main[0].Length < 7)
            {
                for (int i = 0; i < List_Filter_Main.Count; i++)
                {

                    set_filter_frame.Add(int.Parse(List_Filter_Main[i]));

                }


                foreach (string pair in DicInfo.Keys.ToList())
                {
                    if (set_filter_frame.Contains(DicInfo[pair].FrameNo))
                    {
                        set_filter.Add(pair, DicInfo[pair]);
                    }

                }
                open.DicInfo_Filtered = set_filter;
                open.Set_View();
                Print_List();
            }

            List_filter = 0;
        }

        private void Set_Filter_Frame()
        {

            set_filter.Clear();


            foreach (string pair in DicInfo.Keys.ToList())
            {
                if (set_filter_frame.Contains(DicInfo[pair].FrameNo))
                {
                    set_filter.Add(pair, DicInfo[pair]);
                }

            }

        }

        private void Frame_S_Page_TB_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                Camera_NO_Filter_TB.Text = "";
                textBox4.Text = "";

                if (int.Parse(Frame_S_Page_TB.Text) <= int.Parse(Frame_E_Page_TB.Text) && int.Parse(Frame_S_Page_TB.Text) > 0)
                {
                    open.No_Frmae_Filter(int.Parse(Frame_S_Page_TB.Text));
                }
                else
                {
                    MessageBox.Show("잘 못 입력하였습니다.");
                    Frame_S_Page_TB.Text = "1";
                    open.No_Frmae_Filter(int.Parse(Frame_S_Page_TB.Text));
                }

            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Filter_Check();
            }
            //int Num;
            //if (e.KeyCode == Keys.Enter)
            //{
                
            //    if (textBox4.Text == "")
            //    {
            //        State_Filter = 1;
            //        open.DicInfo_Filtered = new Dictionary<string, ImageInfo>(DicInfo);
            //        open.Set_View();
            //        Print_List();
            //        textBox4.Text = "";
            //        return;

            //    }
             

            //    if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            //    {
            //        if (textBox4.Text == "양품" || textBox4.Text == "선택")
            //        {
            //            State_Filter = 1;
            //            foreach (string pair in open.Frame_dicInfo_Filter.Keys.ToList())
            //            {
            //                if (open.Frame_dicInfo_Filter[pair].ReviewDefectName == textBox4.Text)
            //                {

            //                }
            //                else
            //                {
            //                    open.Frame_dicInfo_Filter.Remove(pair);

            //                }
            //            }

            //            open.Frame_Set_Image();
                     
            //            //textBox4.Text = string.Empty;
            //            State_Filter = 0;
            //        }
            //        else
            //        {
            //            textBox4.Text = string.Empty;
            //            MessageBox.Show("양품 or 선택으로 입력.");
            //        }
            //    }
            //    else
            //    {
            //        if (textBox4.Text == "양품" || textBox4.Text == "선택")
            //        {
            //            State_Filter = 1;
            //            open.DicInfo_Filtered.Clear();
            //            foreach (string pair in DicInfo.Keys.ToList())
            //            {
            //                if (DicInfo[pair].ReviewDefectName == textBox4.Text)
            //                {
            //                    open.DicInfo_Filtered.Add(pair, DicInfo[pair]);
            //                }
            //                else
            //                {
                              

            //                }
            //            }

            //            open.Set_View();
            //            Print_List();
            //            //textBox4.Text = string.Empty;
            //            State_Filter = 0;
            //        }
            //        else
            //        {
            //            textBox4.Text = string.Empty;
            //            MessageBox.Show("양품 or 선택으로 입력.");
            //        }
            //    }
                

            //}

        }

        private void Manual_Mode_RB_Click(object sender, EventArgs e)
        {
            if (DicInfo.Count > 0 && Manual_Mode_RB.Checked)
            {
                if(MessageBox.Show("재로드 하시겠습니까?","알림",MessageBoxButtons.YesNo)== System.Windows.Forms.DialogResult.Yes)
                {
                    mode_change = 1;

                    zipLoadFileToolStripMenuItem_Click(null, null);
                }
                else
                {
                    if(curruent_viewtype == 0)
                    {
                        View_Mode_RB.Checked = true;
                    }
                    else
                    {
                        View_Mode_RB.Checked = false;
                        Manual_Mode_RB.Checked = true;
                    }
                    

                }

               // View_Mode_RB.Checked = false;
            }
        }

        private void View_Mode_RB_Click(object sender, EventArgs e)
        {
            if (DicInfo.Count > 0 && View_Mode_RB.Checked)
            {
                if (MessageBox.Show("재로드 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    mode_change = 1;
                    zipLoadFileToolStripMenuItem_Click(null, null);
                }
                else
                {
                    if (curruent_viewtype == 0)
                    {
                        View_Mode_RB.Checked = true;
                    }
                    else
                    {
                        View_Mode_RB.Checked = false;
                        Manual_Mode_RB.Checked = true;
                    }
                   
                }
                //Manual_Mode_RB.Checked = false;



            }
        }

        private void Height_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Fixed_CB.Checked == true)
                {
                    Height_TB.Text = Width_TB.Text;
                }
                else
                {

                }

                {
                    switch (ViewType)
                    {
                        case "SetView":
                            open.Set_View();
                            break;

                        case "FrameSetView":
                            open.Frame_Set_View();
                            break;

                        case "DLFrameSetView":
                            open.DL_Frame_Set_View();
                            break;

                        case "EQCBSetView":
                            open.Eq_CB_Set_View();
                            break;

                        case "EQCBSetView_ING":
                            open.Eq_CB_Set_View_ING();
                            break;

                        case "FilterCBafterSetView":
                            open.Filter_CB_after_Set_View();
                            break;

                        case "Code_200_SetView":
                            open.Code_200_Set_View();
                            break;

                    }

                }
            }
        }
    
        private void Frame_Interval_CheckedChanged(object sender, EventArgs e)
        {
            if(Frame_View_CB.Checked && Frame_Interval_CB.Checked)
            {
                MessageBox.Show("Frame 구간 검색은 Frame 별 체크 해제 후 사용 부탁드립니다.");
                Frame_Interval_CB.Checked = false;
                return;
            }
            if (Frame_Interval_CB.Checked == true)
            {
                Frame_E_TB.Enabled = true;
                Frame_E_TB.ReadOnly = false;
            }
            else
            {
              Frame_E_TB.Enabled = true;
              Frame_E_TB.ReadOnly = true;

              if(!Frame_View_CB.Checked)
               Frame_E_TB.Text = string.Empty;


            }
        }

        private void Frame_E_TB_KeyDown(object sender, KeyEventArgs e)
        {
            List<int> Frame_inter = new List<int>();
            
            if (e.KeyCode == Keys.Enter)
            {
                int min = int.Parse(Frame_S_TB.Text);
                int max = int.Parse(Frame_E_TB.Text);
                open.Set_Frame_Filter();
                for(int i = 0; i < open.Frame_List_Img.Count; i++)
                {
                    if(min <= open.Frame_List_Img[i] && open.Frame_List_Img[i] <= max)
                    {
                        if (Frame_inter.Contains(open.Frame_List_Img[i]))
                        {

                        }
                        else
                        {
                            Frame_inter.Add(open.Frame_List_Img[i]);

                        }
                    }
                }

                if (Frame_inter.Count > 0)
                {
                    open.Frame_Interval_Filter(Frame_inter);
                }
                else
                {

                    MessageBox.Show("해당 프레임은 존재하지 않습니다.");
                    Frame_S_TB.Text = "";
                    Frame_E_TB.Text = "";
                }

            }
            open.Frame_List_Img.Clear();
        }

        private void Manual_Mode_RB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Cols_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                switch (ViewType)
                {
                    case "SetView":
                        open.Set_View();
                        break;

                    case "FrameSetView":
                        open.Frame_Set_View();
                        break;

                    case "DLFrameSetView":
                        open.DL_Frame_Set_View();
                        break;

                    case "EQCBSetView":
                        open.Eq_CB_Set_View();
                        break;

                    case "EQCBSetView_ING":
                        open.Eq_CB_Set_View_ING();
                        break;

                    case "FilterCBafterSetView":
                        open.Filter_CB_after_Set_View();
                        break;

                }

            }
        }

        private void Filter_Check()
        {
            Set_EQ_Dic();
            if (CheckEQ_Dic.Count > 0)
            {

                if(ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
                {
                    State_Filter = 1;
                    Dictionary<string, ImageInfo> before_dic = new Dictionary<string, ImageInfo>(open.Frame_dicInfo_Filter);
                    open.Filter_Frame_Set_IMG();
                    Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(open.Frame_dicInfo_Filter);
                    
                    
                    if(Filter_CheckEQ_Dic.Count == 0)
                    {
                        MessageBox.Show("해당 이미지가 없습니다.");
                        
                        State_Filter = 0;
                        return;
                    }
                   
                    if (Camera_NO_Filter_TB.Text != "")
                    {
                        string[] Split_String = null;
                        Split_String = Camera_NO_Filter_TB.Text.Split(',');

                        foreach (string No in Filter_CheckEQ_Dic.Keys.ToList())
                        {
                            if (Filter_CheckEQ_Dic.ContainsKey(No))
                            {
                                if (Split_String.Contains(Filter_CheckEQ_Dic[No].CameraNo.ToString()))
                                {

                                }
                                else
                                {
                                    Filter_CheckEQ_Dic.Remove(No);
                                }

                            }
                        }


                        if (Filter_CheckEQ_Dic.Count == 0)
                        {
                            Filter_CheckEQ_Dic = before_dic;
                            MessageBox.Show("해당 카메라 이미지가 없습니다.");
                            
                            Camera_NO_Filter_TB.Text = "";
                        }

                    }

                    if (textBox4.Text != "")
                    {
                        if (textBox4.Text == "양품" || textBox4.Text == "선택")
                        {


                            foreach (string pair in Filter_CheckEQ_Dic.Keys.ToList())
                            {
                                if (Filter_CheckEQ_Dic[pair].ReviewDefectName == textBox4.Text)
                                {

                                }
                                else
                                {
                                    Filter_CheckEQ_Dic.Remove(pair);
                                }
                            }



                        }
                        else
                        {
                            textBox4.Text = string.Empty;
                            MessageBox.Show("양품 or 선택으로 입력.");
                            
                            Filter_CheckEQ_Dic = before_dic;
                        }
                    }
                    else
                    {


                    }

               
                    open.Frame_dicInfo_Filter = Filter_CheckEQ_Dic;
                    open.Frame_Set_Image();
                    State_Filter = 0;
                }
                else
                {
                    
                    Filter_CheckEQ_Dic = new Dictionary<string, ImageInfo>(CheckEQ_Dic);
                    Dictionary<string, ImageInfo> before_dic = new Dictionary<string, ImageInfo>(open.DicInfo_Filtered);
                    State_Filter = 1;
                    //EQ 필터체크
                    if (Camera_NO_Filter_TB.Text != "")
                    {
                        string[] Split_String = null;
                        Split_String = Camera_NO_Filter_TB.Text.Split(',');

                        foreach (string No in Filter_CheckEQ_Dic.Keys.ToList())
                        {
                            if (Filter_CheckEQ_Dic.ContainsKey(No))
                            {
                                if (Split_String.Contains(Filter_CheckEQ_Dic[No].CameraNo.ToString()))
                                {

                                }
                                else
                                {
                                    Filter_CheckEQ_Dic.Remove(No);
                                }

                            }
                        }


                        if (Filter_CheckEQ_Dic.Count == 0)
                        {
                            Filter_CheckEQ_Dic = before_dic;
                            MessageBox.Show("해당 카메라 이미지가 없습니다.");
                            State_Filter = 0;
                            Camera_NO_Filter_TB.Text = "";
                        }

                    }
                    else
                    {

                    }

                    if (textBox4.Text != "")
                    {
                        if (textBox4.Text == "양품" || textBox4.Text == "선택")
                        {


                            foreach (string pair in Filter_CheckEQ_Dic.Keys.ToList())
                            {
                                if (Filter_CheckEQ_Dic[pair].ReviewDefectName == textBox4.Text)
                                {

                                }
                                else
                                {
                                    Filter_CheckEQ_Dic.Remove(pair);
                                }
                            }



                        }
                        else
                        {
                            textBox4.Text = string.Empty;
                            MessageBox.Show("양품 or 선택으로 입력.");
                            State_Filter = 0;
                            Filter_CheckEQ_Dic = before_dic;
                        }
                    }
                    else
                    {


                    }

                    if (Frame_S_TB.Text != "")
                    {
                        int Num;
                        List<int> Frame_filter_List = new List<int>();
                        if (!int.TryParse(Frame_S_TB.Text, out Num))
                        {
                            MessageBox.Show("입력을 숫자로 부탁드립니다.");
                            Frame_S_TB.Text = "";
                            open.DicInfo_Filtered = Filter_CheckEQ_Dic;
                            open.Set_View();
                            State_Filter = 0;
                            Print_List();

                            return;
                        }

                        foreach (string pair in Filter_CheckEQ_Dic.Keys.ToList())
                        {
                            if (Frame_filter_List.Contains(Filter_CheckEQ_Dic[pair].FrameNo))
                            {

                            }
                            else
                            {
                                Frame_filter_List.Add(Filter_CheckEQ_Dic[pair].FrameNo);

                            }
                        }

                        if (Frame_filter_List.Contains(int.Parse(Frame_S_TB.Text)))
                        {
                            open.Frame_Filter(int.Parse(Frame_S_TB.Text));
                        }
                        else
                        {

                            MessageBox.Show("해당 프레임은 존재하지 않습니다.");
                            Frame_S_TB.Text = Frame_E_TB.Text;
                            open.DicInfo_Filtered = Filter_CheckEQ_Dic;
                            open.Set_View();
                            Print_List();
                            State_Filter = 0;
                            return;
                        }

                        Filter_CheckEQ_Dic.Clear();
                    }
                    else
                    {

                    }
                    /// 뷰
                    if (Frame_S_TB.Text != "")
                    {


                    }
                    else
                    {
                        open.DicInfo_Filtered = Filter_CheckEQ_Dic;
                        open.Set_View();
                        Print_List();
                    }
                    State_Filter = 0;

                }
               
            }

        }
    }


}

public static class ExtensionMethods
{

    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {

        Type dgvType = dgv.GetType();

        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);



        pi.SetValue(dgv, setting, null);

    }

}




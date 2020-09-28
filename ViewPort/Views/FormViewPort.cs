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

namespace ViewPort
{


    public partial class FormViewPort : MetroForm
    {
        #region MEMBER VARIABLES

        ImageViewer open = new ImageViewer();

        Dictionary<string, ImageInfo> eq_CB_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Copy = new Dictionary<string, ImageInfo>();
        Dictionary<string, txtInfo> dicTxt_info = new Dictionary<string, txtInfo>();
        Dictionary<string, ImageInfo> dicInfo_Waiting_Del = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> return_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> sdip_NO1_dicInfo = new Dictionary<string, ImageInfo>();
        
        Dictionary<string, ImageInfo> f9_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> f10_code_dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> f5_code_dicInfo = new Dictionary<string, ImageInfo>();

        string[] dic_ready = null;
        string viewType = null;
        int Rotate_Option = 1;
        //List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        //List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();
        List<int> MAP_LIST = new List<int>();
        List<int> frame_List_main = new List<int>();
        List<int> f9_Frame_List_Main = new List<int>();
        List<int> f10_Frame_List_Main = new List<int>();
        List<string> Eq_Filter_Select_Key_List = new List<string>();

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

        int btnColumnIdx;
        Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
        private int _load_State;
        private string dirPath;
        private string zipFilePath;
        private string ref_DirPath;

        public Dictionary<string, ImageInfo> DicInfo
        {
            get { return dicInfo; }
            set { dicInfo = value; }
        }
        public string ViewType { get => viewType; set => viewType = value; }

        public Dictionary<string, ImageInfo> Eq_CB_dicInfo { get => eq_CB_dicInfo; set => eq_CB_dicInfo = value; }

        public Dictionary<string, ImageInfo> F9_code_dicInfo { get => f9_code_dicInfo; set => f9_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> F10_code_dicInfo { get => f10_code_dicInfo; set => f10_code_dicInfo = value; }

        public Dictionary<string, ImageInfo> F5_code_dicInfo { get => f5_code_dicInfo; set => f5_code_dicInfo = value; }
        public Dictionary<string, ImageInfo> Sdip_NO1_dicInfo { get => sdip_NO1_dicInfo; set => sdip_NO1_dicInfo = value; }
        public Dictionary<string, ImageInfo> DicInfo_Copy { get => dicInfo_Copy; set => dicInfo_Copy = value; }
        public Dictionary<string, ImageInfo> Return_dicInfo { get => return_dicInfo; set => return_dicInfo = value; }
        public Dictionary<string, ImageInfo> Waiting_Del { get => dicInfo_Waiting_Del; set => dicInfo_Waiting_Del = value; }

        public List<int> Frame_List_Main { get => frame_List_main; set => frame_List_main = value; }

        public List<int> F9_Frame_List_Main { get => f9_Frame_List_Main; set => f9_Frame_List_Main = value; }

        public List<int> F10_Frame_List_Main { get => f10_Frame_List_Main; set => f10_Frame_List_Main = value; }

        public List<string> F5_Img_KeyList_Main { get => f5_Img_KeyList; set => f5_Img_KeyList = value; }


        public List<int> mAP_LIST { get => MAP_LIST; set => MAP_LIST = value; }
        public List<int> Contain_200_Frame_Main { get => contain_200_Frame_Main; set => contain_200_Frame_Main = value; }
        public List<string> selected_Pic { get => Selected_Pic; set => Selected_Pic = value; }

        public List<string> Dl_Apply_List_Main { get => dl_Apply_List_Main; set => dl_Apply_List_Main = value; }

        public List<string> Dl_NotApply_List_Main { get => dl_NotApply_List_Main; set => dl_NotApply_List_Main = value; }
        public List<string> Dl_List_Main { get => dl_List_Main; set => dl_List_Main = value; }
        public string ZipFilePath { get => zipFilePath; set => zipFilePath = value; }
        public string REF_DirPath { get => ref_DirPath; set => ref_DirPath = value; }
        public string DirPath { get => dirPath; set => dirPath = value; }
        public int Load_State { get => _load_State; set => _load_State = value; }

        public int GetLoad_State()
        {
            return Load_State;
        }


        #endregion

        #region Initialize CODE
        public FormViewPort()
        {
            InitializeComponent();
            Init();
            InitialData();

            
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
            dataGridView1.RowHeadersWidth = 30;
           
                foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                    dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

        }
        public void Frame_Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;

            foreach (KeyValuePair<string, ImageInfo> kvp in open.Frame_dicInfo_Filter)
                dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);




        }

        private void Eq_Filter_after_Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;

            foreach (KeyValuePair<string, ImageInfo> kvp in eq_CB_dicInfo)
                dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);
            
        }
        public void Return_Img_Print()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            Return_dicInfo = new Dictionary<string, ImageInfo>(open.DicInfo_Filtered);
            dataGridView1.RowHeadersWidth = 30;

            for(int i = 0; i < selected_Pic.Count; i++)
            {
                Return_dicInfo.Add(selected_Pic[i], dicInfo[selected_Pic[i]]);

                if (dt.Rows.Contains(selected_Pic[i]))
                    continue;
                else
                    dt.Rows.Add(dicInfo[selected_Pic[i]].Imagename, dicInfo[selected_Pic[i]].ReviewDefectName);
            }

            dt.DefaultView.Sort = "Image Name";
            
            
            Sorted_dic = Return_dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Return_dicInfo = Sorted_dic;

            

            //open.DicInfo_Filtered = Return_dicInfo;



            if (Selected_Equipment_DF_List.Count > 0)
                open.Eq_CB_Set_View_ING();
            else
                open.Set_Image();

            selected_Pic.Clear();
        }
        public void Wait_Del_Print_List()
        {

            DataTable dt_del = (DataTable)dataGridView2.DataSource;
            //dt_del.Rows.Clear();
            dataGridView2.RowHeadersWidth = 30;

            foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Delete)
            {
                DataRow dr = dt_del.NewRow();
                if (dt_del.Rows.Contains(kvp.Key))
                    continue;
                else
                    dt_del.Rows.Add(kvp.Key, kvp.Value.DeleteCheck);
            }

            dataGridView2.DataSource = dt_del;
        }
        public void Img_txt_Info_Combine()
        {
            
            foreach (KeyValuePair<string, Models.txtInfo> kvp in dicTxt_info)
            {
                if(dicInfo.ContainsKey(kvp.Key))
                {
                    dicInfo[kvp.Key].sdip_no = kvp.Value.SDIP_No;
                    dicInfo[kvp.Key].sdip_result = kvp.Value.SDIP_Result;

                    if(dicInfo[kvp.Key].sdip_no == "1")
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
        public void Dl_PrintList()
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
                    if(Eq_CB_dicInfo.ContainsKey(Selected_Pic[i]))
                        Eq_CB_dicInfo.Remove(Selected_Pic[i]);
                }
                //dt.AcceptChanges();
            }
            
            if(Eq_CB_dicInfo.Count>0)
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
            DicInfo.Clear();


            foreach(KeyValuePair<string,ImageInfo> pair in Dl_DicInfo)
            {
                if (!Selected_Pic.Contains(pair.Key))
                {
                    DicInfo.Add(pair.Key, pair.Value);
                }
                    
            }
           
            dt.Rows.Clear();
            

            foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);

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
            dt.AcceptChanges();
        }

        public void ALL_Changeed_State()
        {
            int index = 0;
            

            DataTable dt = (DataTable)dataGridView1.DataSource;
           
           
            //dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;
            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = open.Frame_dicInfo_Filter[dataGridView1.Rows[2].Cells[1].Value.ToString().Substring(0, 12)].ReviewDefectName;
                }
            }
            else
            {
                //foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Filtered)
                //    dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[2].Value = open.DicInfo_Filtered[dataGridView1.Rows[2].Cells[1].Value.ToString().Substring(0, 12)].ReviewDefectName;
                }
            }
                
      
        }
        private void _filterAct_bt_Click(object sender, EventArgs e)
        {
            Eq_CB_dicInfo.Clear();
            Initial_Equipment_DF_FilterList();

            //eq_CB_dicInfo = new Dictionary<string, ImageInfo>(dicInfo);

            if (Waiting_Del.Count > 0)
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
                    eq_CB_dicInfo.Remove(kvp.Key);

            }

            Sorted_dic = eq_CB_dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Eq_CB_dicInfo = Sorted_dic;

            Eq_Filter_after_Print_List();
            open.Main = this;
            open.Eq_CB_Set_View();

            if (Frame_View_CB.Checked)
            {
                Frame_View_CB.Checked = false;
                Frame_S_Page_TB.Text = " ";
                Frame_E_Page_TB.Text = " ";
            }
                
        }
        
        private void Initial_Equipment_DF_FilterList()
        {
            
            for (int i = 0; i < Selected_Equipment_DF_List.Count; i++)
            {
                foreach(KeyValuePair<string, ImageInfo> pair in DicInfo)
                {
                   
                        if (pair.Value.EquipmentDefectName == Selected_Equipment_DF_List[i])
                        {
                             if(Eq_CB_dicInfo.ContainsKey(pair.Key) == false)   
                                Eq_CB_dicInfo.Add(pair.Key, pair.Value);
                        }
                   
                }

            }

            
        }

        private void Equipment_DF_CLB_SelectedValueChanged(object sender, EventArgs e)
        {
            Selected_Equipment_DF_List.Clear();
            
            foreach (int index in Equipment_DF_CLB.CheckedIndices)
                Selected_Equipment_DF_List.Add(Equipment_DF_CLB.Items[index].ToString().Split('-')[0]);
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

        private void ZipLoadFile_Async()
        {
            Load_State = 1;
            string path = Util.OpenFileDlg(ZIP_STR.EXETENSION);
            string FileName = string.Empty;
      

            if (string.IsNullOrEmpty(path) == false)
            {
                FileName = Util.GetFileName();
                DirPath = Directory.GetParent(path).ToString();
                ZipFilePath = path;

                if(MessageBox.Show("" + FileName + "로트 파일을 로드 하시겠습니까?", "프로그램 로드", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FormLoading formLoading = new FormLoading(path, this);
                    formLoading.ShowDialog();


                    DicInfo = formLoading.Dic_Load;
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



                    dataGridView1.DataSource = formLoading.Dt;



                    dataGridView1.Columns[0].Width = 50;
                    dataGridView1.Columns[1].Width = 150;
                    dataGridView1.Columns[2].Width = 50;


                    dataGridView1.RowHeadersWidth = 30;



                    formLoading.Dispose();
                }
                else
                {
                    ZipFilePath = null;
                }
                
            }
        }

        private void zipLoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialData();
            ZipLoadFile_Async();
            int x = 1;
            int index = 0;
            if (ZipFilePath != null)
            {
                Img_txt_Info_Combine();
                                
                dicInfo_Copy = new Dictionary<string, ImageInfo>(DicInfo);

                if (Manual_Mode_RB.Checked)
                {
                    //MAP 정보에서 제외될 프레임 적용
                    //for (int i = 0; i < MAP_LIST.Count; i++)
                    //{
                    //    foreach (string key in DicInfo.Keys.ToList())
                    //    {
                    //        if (dicInfo[key].FrameNo == MAP_LIST[i])
                    //        {
                    //        }
                    //        else
                    //        {
                    //            if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[key].EquipmentDefectName)) == -1)
                    //                continue;
                    //            else
                    //            {
                    //                index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[key].EquipmentDefectName));
                    //                x = All_Equipment_DF_List[index].Item2;
                    //                All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[key].EquipmentDefectName, --x);

                    //                if (x == 0)
                    //                {
                    //                    All_Equipment_DF_List.RemoveAt(index);

                    //                }

                    //            }

                    //            dicInfo.Remove(key);
                    //        }

                    //    }
                    //}
                    //foreach (string key in DicInfo.Keys.ToList())
                    //{
                    //   if(MAP_LIST.Contains(dicInfo[key].FrameNo))
                    //    {

                    //    }
                    //   else
                    //   {
                    //        if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[key].EquipmentDefectName)) == -1)
                    //            continue;
                    //        else
                    //        {
                    //            index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[key].EquipmentDefectName));
                    //            x = All_Equipment_DF_List[index].Item2;
                    //            All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[key].EquipmentDefectName, --x);

                    //            if (x == 0)
                    //            {
                    //                All_Equipment_DF_List.RemoveAt(index);

                    //            }

                    //        }

                    //        dicInfo.Remove(key);
                    //   }

                    //}

                    //SDIP 코드 211~230 제외
                    foreach (string pair in dicInfo.Keys.ToList())
                    {
                        if (F5_Img_KeyList_Main.Contains(pair))
                        {
                            F5_code_dicInfo.Add(pair, dicInfo[pair]);
                        }


                        if (dicInfo[pair].sdip_no  != "-" && 200 <= int.Parse(dicInfo[pair].sdip_no) && int.Parse(dicInfo[pair].sdip_no) <= 299)
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

                            dicInfo.Remove(pair);
                        }

                    }
                }
                foreach (KeyValuePair<string, ImageInfo> pair in DicInfo)
                {
                    if (F9_Frame_List_Main.Contains(pair.Value.FrameNo))
                    {
                        if (int.Parse(pair.Value.sdip_no) != 1)
                        {
                            F9_code_dicInfo.Add(pair.Key, pair.Value);
                        }

                    }
                    else if (F10_Frame_List_Main.Contains(pair.Value.FrameNo))
                    {
                        if (int.Parse(pair.Value.sdip_no) != 1)
                        {
                            F10_code_dicInfo.Add(pair.Key, pair.Value);
                        }

                    }

                }


                All_LotID_List.Sort();
                Initial_Equipment_DF_List();
                
                if(checkBox1.Checked)
                {
                    for (int i = 0; i < ImageSizeList.Count; i++)
                        ImageSize_CB.Items.Add(ImageSizeList.ElementAt(i));

                    ImageSize_CB.SelectedIndex = 0;
                }
                

                for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                    Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

                
                Select_All_BTN_Click(null, null);
                MessageBox.Show(MSG_STR.SUCCESS);

                open.Main = this;
                open.Set_View();
            }
            
        }

        private void update_Equipment_DF_CLB(List<string> deleted_pic)
        {
            
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = {'-'};
            for (int i = 0; i < deleted_pic.Count; i++)
            {
                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName)) == -1)
                    continue;
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[deleted_pic[i]].EquipmentDefectName, --x);
                    
                    if(x==0)
                    {
                        All_Equipment_DF_List.RemoveAt(index);
                        
                    }
                   
                }
                    
            }
            Equipment_DF_CLB.Items.Clear();
            Initial_Equipment_DF_List();
            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            if (EQ_Search_TB != null)
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

           
        }

        private void Return_update_Equipment_DF_CLB(List<string> deleted_pic)
        {
            Initial_Equipment_DF_List();
            List<string> changed_eq = new List<string>();
            int x = 1;
            int index = 0;
            Char[] sd = {'-'};
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

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            if (EQ_Search_TB != null)
            {
                EQ_Search();

            }
           
               


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

            }
        }

        private void Width_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Height_TB.Text = Width_TB.Text;
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
            Wait_Del_Img_List = open.DicInfo_Delete.Keys.ToList();
            DeleteWaiting deleteWaiting = new DeleteWaiting(this);
            deleteWaiting.Waiting_Img = Waiting_Del;
            deleteWaiting.ZipFilePath = zipFilePath;
            deleteWaiting.Set_View_Del();
            deleteWaiting.Show();
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if(e.ColumnIndex == 0)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells["Image Name"].Value.ToString().Substring(0,12);

                open.SelectGrid_Img_View(id);

            }
            
           
        }

        public void Delete_ZipImg()
        {

            Func.DeleteJPG_inZIP(zipFilePath, dicInfo_Waiting_Del);
            
            foreach(KeyValuePair<string, ImageInfo> pair in dicInfo_Waiting_Del)
            {
                if (DicInfo.ContainsKey(pair.Key))
                    DicInfo.Remove(pair.Key);
            }


            dicInfo_Waiting_Del.Clear();
            ((DataTable)dataGridView2.DataSource).Rows.Clear();

            int ci = open.DicInfo_Delete.Count;

        }

        private void FormViewPort_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show(" IMG TXT를 변경하시겠습니까?", "IMG TXT Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Func.Write_IMGTXT_inZip(ZipFilePath, DicInfo);
            }

            if (Waiting_Del.Count > 0)
            {
                if (MessageBox.Show("" + open.DicInfo_Delete.Count + "개의 이미지를 삭제하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Delete_ZipImg();
                    Dispose(true);
                }
                else if (MessageBox.Show("프로그램을 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Dispose(true);

                }
                else
                {
                    e.Cancel = true;
                    return;
                }


            }

            else if(MessageBox.Show( "프로그램을 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Dispose(true);
                
            }
            else
            {
                e.Cancel = true;
                return;
            }
            


        }

        private void 중간저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Func.SaveDelFileID(Waiting_Del);

        }

        private void 저장불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Func.LoadDelFileID(this, Waiting_Del, dicInfo_Copy);

            
            
        }

     
        public void Load_saveFile()
        {
            open.DicInfo_Delete = Waiting_Del;
            open.Load_Del( );
            Dl_PrintList();
            Wait_Del_Print_List();
        }

        private void Fixed_CB_CheckedChanged(object sender, EventArgs e)
        {
            if (Fixed_CB.Checked == true)
            {
                Height_TB.Enabled = false;
                Height_TB.Text = Width_TB.Text;
            }
            else
                Height_TB.Enabled = true;
        }

        private void Frame_View_CB_CheckedChanged(object sender, EventArgs e)
        {
            if(Frame_View_CB.Checked)
            {
                open.Frame_Set_View();
            }
                
            else
            {
                Frame_S_Page_TB.Text = "";
                Frame_E_Page_TB.Text = "";
                Frame_S_TB.Text = "";
                Frame_E_TB.Text = "";

                open.Set_View();

                EQ_Search_TB.Text = null;
                Initial_Equipment_DF_List();
                Equipment_DF_CLB.Items.Clear();

                for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                    Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

                Select_All_BTN_Click(null, null);
                Print_List();
                Select_All_BTN_Click(null, null);
            }
        }

        private void Manual_Mode_RB_CheckedChanged(object sender, EventArgs e)
        {
            if(View_Mode_RB.Checked)
            {

            }
        }

        private void View_Mode_RB_CheckedChanged(object sender, EventArgs e)
        {
            if (Manual_Mode_RB.Checked)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EQ_Search();
        }

        private void EQ_Search()
        {
            string txt = EQ_Search_TB.Text;
           
            for(int i =0; i < Equipment_DF_CLB.Items.Count; i++)
            {
                if (Equipment_DF_CLB.Items[i].ToString().Contains(txt))
                    continue;
                else
                {
                    Equipment_DF_CLB.Items.RemoveAt(i);
                    i--;
                }
                    
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            EQ_Search_TB.Text = null;
            Initial_Equipment_DF_List();
            Equipment_DF_CLB.Items.Clear();

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

            Select_All_BTN_Click(null, null);
        }

        private void 번코드변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_Code change = new Change_Code(open);
            change.Show();
        }

        private void Camera_NO_Filter_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int isInt;

                if (int.TryParse(Camera_NO_Filter_TB.Text, out isInt))
                {
                    

                    switch (ViewType)
                    {
                        case "SetView":
                            open.Set_View();
                            Print_List();
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
                    Print_Image_BT_Click(null, null);

                }
                else if(Camera_NO_Filter_TB.Text =="")
                {
                    switch (ViewType)
                    {
                        case "SetView":
                            open.Set_View();
                            Print_List();
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
                    Print_Image_BT_Click(null, null);
                }
                else
                    MessageBox.Show("숫자만 입력해주세요.");



                Print_Image_BT.Focus();
            }
        }

        private void Print_Image_BT_Click(object sender, EventArgs e)
        {

        }

        private void Rotate_CLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRotation_Option(Rotate_CLB.SelectedIndex);

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {

                open.Frame_Set_Image();
            }
            else if(ViewType == "SetView")
            {
                open.Set_Image();
            }
        }

        private void ImageSize_Filter()
        {
            if (ImageSize_CB.SelectedIndex == 0)
                return;

            if (checkBox1.Checked)
            {
                DicInfo = new Dictionary<string, ImageInfo>(dicInfo_Copy);
                
                foreach(string pair in DicInfo.Keys.ToList())
                {
                    if (!ImageSize_CB.SelectedItem.Equals(DicInfo[pair].ImageSize))
                    {
                        DicInfo.Remove(pair);
                    }
                }
            }

            if (ViewType == "FrameSetView" || ViewType == "DLFrameSetView")
            {

                open.Frame_Set_Image();
            }
            else if (ViewType == "SetView")
            {
                open.Set_Image();
            }
        }

        private void ImageSize_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageSize_Filter();
        }

        private void 업데이트ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void FilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYLocationFilter XYFilter = new XYLocationFilter(open);
            XYFilter.Show();

        }
    }


}
 
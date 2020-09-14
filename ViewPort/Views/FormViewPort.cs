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
        string[] dic_ready = null;

        //List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        //List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();
        List<string> MAP_LIST = new List<string>();
        List<int> frame_List_main = new List<int>();
        List<string> Eq_Filter_Select_Key_List = new List<string>();

        List<string> Wait_Del_Img_List = new List<string>();
        List<string> Selected_Equipment_DF_List = new List<string>();
        List<string> ImageSizeList = new List<string>();
        List<string> Selected_Pic = new List<string>();
        List<string> Change_state_List = new List<string>();
        List<string> dl_List_Main = new List<string>();

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
        public Dictionary<string, ImageInfo> Eq_CB_dicInfo { get => eq_CB_dicInfo; set => eq_CB_dicInfo = value; }
        public Dictionary<string, ImageInfo> Return_dicInfo { get => return_dicInfo; set => return_dicInfo = value; }
        public Dictionary<string, ImageInfo> Waiting_Del { get => dicInfo_Waiting_Del; set => dicInfo_Waiting_Del = value; }

        public List<int> Frame_List_Main { get => frame_List_main; set => frame_List_main = value; }
        public List<string> selected_Pic { get => Selected_Pic; set => Selected_Pic = value; }

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



        private void Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;
           
                foreach (KeyValuePair<string, ImageInfo> kvp in dicInfo)
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
                dr = dt.Rows.Find(Waiting_Del[Selected_Pic[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                dt.Rows[index].Delete();
             

                //dt.AcceptChanges();
            }
            
        }

        public void Changeed_State()
        {
            int index = 0;
            Change_state_List = open.Change_state;

            DataTable dt = (DataTable)dataGridView1.DataSource;


            for (int i = 0; i < Change_state_List.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(dicInfo[Change_state_List[i]].Imagename);
                index = dt.Rows.IndexOf(dr);
                if (dicInfo.ContainsKey(Change_state_List[i]))
                    dt.Rows[index][1] = dicInfo[Change_state_List[i]].ReviewDefectName;

                //dt.AcceptChanges();
            }
        }

        private void _filterAct_bt_Click(object sender, EventArgs e)
        {

            //eq_CB_dicInfo = new Dictionary<string, ImageInfo>(dicInfo);
            if(Waiting_Del.Count >0)
            {
                foreach (KeyValuePair<string, ImageInfo> kvp in Waiting_Del)
                    eq_CB_dicInfo.Remove(kvp.Key);
                             
            }


            Initial_Equipment_DF_FilterList();
            Eq_Filter_after_Print_List();
            open.Main = this;
            open.Eq_CB_Set_View();
        }
        
        private void Initial_Equipment_DF_FilterList()
        {
            
            for (int i = 0; i < Selected_Equipment_DF_List.Count; i++)
            {
                foreach(KeyValuePair<string, ImageInfo> pair in dicInfo)
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
                Selected_Equipment_DF_List.Add(All_Equipment_DF_List.ElementAt(index).Item1);
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

            if (string.IsNullOrEmpty(path) == false)
            {
                DirPath = Directory.GetParent(path).ToString();
                ZipFilePath = path;





                FormLoading formLoading = new FormLoading(path);
                formLoading.ShowDialog();

                
                DicInfo = formLoading.Dic_Load;

                MAP_LIST = formLoading.Map_List;

                Dl_List_Main = formLoading.Dl_List;

                Frame_List_Main = formLoading.Frame_List;

                for (int i = 0; i < MAP_LIST.Count; i++)
                {
                    if(Frame_List_Main.Contains(int.Parse(MAP_LIST[i])))
                    {
                        Frame_List_Main.Remove(int.Parse(MAP_LIST[i]));
                        
                    }
                }


                dicTxt_info = formLoading.DicTxt_info;
                
                All_Equipment_DF_List = formLoading.All_Equipment_DF_List;
                All_LotID_List = formLoading.All_LotID_List;
                
                

                dataGridView1.DataSource = formLoading.Dt;
                dataGridView1.RowHeadersWidth = 30;

                formLoading.Dispose();
            }
        }

        private void zipLoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialData();
            ZipLoadFile_Async();
            if(ZipFilePath != null)
            {
                Img_txt_Info_Combine();
                dicInfo_Copy = new Dictionary<string, ImageInfo>(DicInfo);
                All_LotID_List.Sort();
                Initial_Equipment_DF_List();

                for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                    Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + "-" + All_Equipment_DF_List.ElementAt(i).Item2);

                MessageBox.Show(MSG_STR.SUCCESS);

                open.Main = this;
                open.Set_View();
            }
            
        }

        private void update_Equipment_DF_CLB(List<string> deleted_pic)
        {
            Initial_Equipment_DF_List();
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
                    continue;
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dicInfo[deleted_pic[i]].EquipmentDefectName));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(dicInfo[deleted_pic[i]].EquipmentDefectName, ++x);

                    if (x == 0)
                    {
                        All_Equipment_DF_List.RemoveAt(index);
                    }




                }

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
        private void Width_TB_TextChanged(object sender, EventArgs e)
        {
            Height_TB.Text = Width_TB.Text;
            open.Set_View();
        }

        private void Width_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Height_TB.Text = Width_TB.Text;
                open.Set_View();

            }
        }

        private void Rows_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                open.Set_View();
        }

        private void Print_Image_State_CheckedChanged(object sender, EventArgs e)
        {
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
            deleteWaiting.Waiting_Img = open.DicInfo_Delete;
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

            dicInfo_Waiting_Del.Clear();
            ((DataTable)dataGridView2.DataSource).Rows.Clear();

            int ci = open.DicInfo_Delete.Count;

        }

        private void FormViewPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Waiting_Del.Count > 0)
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
                open.Set_View();
            else
            {
                Frame_S_Page_TB.Text = "";
                Frame_E_Page_TB.Text = "";

                open.Set_View();
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
    }


}
 
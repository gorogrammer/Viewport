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

namespace ViewPort
{


    public partial class FormViewPort : Form
    {
        #region MEMBER VARIABLES

        ImageViewer open = new ImageViewer();

        Dictionary<string, ImageInfo> dicInfo = new Dictionary<string, ImageInfo>();
        Dictionary<string, ImageInfo> dicInfo_Copy = new Dictionary<string, ImageInfo>();
        Dictionary<string, txtInfo> dicTxt_info = new Dictionary<string, txtInfo>();
        Dictionary<string, ImageInfo> dicInfo_Waiting_Del = new Dictionary<string, ImageInfo>();
        string[] dic_ready = null;

        //List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        //List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();

        List<string> Wait_Del_Img_List = new List<string>();
        List<string> Selected_Equipment_DF_List = new List<string>();
        List<string> ImageSizeList = new List<string>();
        List<string> Selected_Pic = new List<string>();
        List<string> Change_state_List = new List<string>();



        private int _load_State;
        private string dirPath;
        private string zipFilePath;
        private string ref_DirPath;

        public Dictionary<string, ImageInfo> DicInfo
        {
            get { return dicInfo; }
            set { dicInfo = value; }
        }

        public Dictionary<string, ImageInfo> Waiting_Del { get => dicInfo_Waiting_Del; set => dicInfo_Waiting_Del = value; }

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
                dt.Rows.Add(kvp.Key, kvp.Value.ReviewDefectName);

        }

        public void Wait_Del_Print_List()
        {

            DataTable dt = (DataTable)dataGridView2.DataSource;
            dt.Rows.Clear();
            dataGridView2.RowHeadersWidth = 30;

            foreach (KeyValuePair<string, ImageInfo> kvp in open.DicInfo_Delete)
                dt.Rows.Add(kvp.Key, kvp.Value.DeleteCheck);
        }
        public void Img_txt_Info_Combine()
        {
            foreach (KeyValuePair<string, Models.txtInfo> kvp in dicTxt_info)
            {
                dicInfo[kvp.Key].sdip_no = kvp.Value.SDIP_No;
                dicInfo[kvp.Key].sdip_result = kvp.Value.SDIP_Result;
            }
        }
        public void Dl_PrintList()
        {
            int index = 0;

            Selected_Pic = open.Select_Pic_List;

            DataTable dt = (DataTable)dataGridView1.DataSource;


            for (int i = 0; i < Selected_Pic.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows.Find(Selected_Pic[i]);
                index = dt.Rows.IndexOf(dr);
                dt.Rows[index].Delete();
                dt.AcceptChanges();

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
                dr = dt.Rows.Find(Change_state_List[i]);
                index = dt.Rows.IndexOf(dr);
                if (dicInfo.ContainsKey(Change_state_List[i]))
                    dt.Rows[index][1] = dicInfo[Change_state_List[i]].ReviewDefectName;

                dt.AcceptChanges();
            }
        }

        private void _filterAct_bt_Click(object sender, EventArgs e)
        {

            dicInfo = open.DicInfo_Filtered;


            Initial_Equipment_DF_FilterList();
            Print_List();
            open.Main = this;
            open.Set_View();
        }

        private void Initial_Equipment_DF_FilterList()
        {
            for (int i = 0; i < dicInfo.Count; i++)
            {
                if (Selected_Equipment_DF_List.FindIndex(s => s.Equals(dicInfo[dicInfo.Keys.ElementAt(i)].EquipmentDefectName)) == -1)
                {
                    dicInfo.Remove(dicInfo.Keys.ElementAt(i));
                    i--;
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

            DirPath = Directory.GetParent(path).ToString();
            ZipFilePath = path;

            FormLoading formLoading = new FormLoading(path);
            formLoading.ShowDialog();

            dicInfo = formLoading.Dic_Load;
            dicTxt_info = formLoading.DicTxt_info;
            All_Equipment_DF_List = formLoading.All_Equipment_DF_List;
            All_LotID_List = formLoading.All_LotID_List;

            dataGridView1.DataSource = formLoading.Dt;
            dataGridView1.RowHeadersWidth = 30;

            formLoading.Dispose();
        }

        private void zipLoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialData();
            ZipLoadFile_Async();
            Img_txt_Info_Combine();
            dicInfo_Copy = dicInfo;
            All_LotID_List.Sort();
            Initial_Equipment_DF_List();

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + " - " + All_Equipment_DF_List.ElementAt(i).Item2);

            MessageBox.Show(MSG_STR.SUCCESS);

            open.Main = this;
            open.Set_View();
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
            open.Cheked_State_DF();
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Wait_Del_Img_List = open.DicInfo_Delete.Keys.ToList();
            DeleteWaiting deleteWaiting = new DeleteWaiting();
            deleteWaiting.Waiting_Img = open.DicInfo_Delete;
            deleteWaiting.ZipFilePath = zipFilePath;
            deleteWaiting.Set_View_Del();
            deleteWaiting.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells["Image Name"].Value.ToString();

            open.SelectGrid_Img_View(id);
        }

        private void Delete_ZipImg()
        {

            Func.DeleteJPG_inZIP(zipFilePath, dicInfo_Waiting_Del);

        }

        private void FormViewPort_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("" + open.DicInfo_Delete.Count + "개의 이미지를 삭제하시겠습니까?", "서버종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Delete_ZipImg();
                Dispose(true);
            }

            else
            {
                e.Cancel = true;
                return;
            }
        }
    }


}
 
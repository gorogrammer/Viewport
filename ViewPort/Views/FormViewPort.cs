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


    public partial class FormViewPort : Form, MyInterface
    {
        #region MEMBER VARIABLES

        ImageViewer open = new ImageViewer();
        Dictionary<string, ImageListInfo> dicInfo = new Dictionary<string, ImageListInfo>();
        Dictionary<string, txtInfo> dicTxt_info = new Dictionary<string, txtInfo>();
        string[] dic_ready = null;

        List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();

        List<string> Selected_Equipment_DF_List = new List<string>();
        List<string> ImageSizeList = new List<string>();



        private int _load_State; 
        private string dirPath;
        private string zipFilePath;
        private string ref_DirPath;

        public Dictionary<string, ImageListInfo> DicInfo
        {
            get { return dicInfo; }
            set { dicInfo = value; }
        }


        public string ZipFilePath { get => zipFilePath; set => zipFilePath = value; }
        public string REF_DirPath { get => ref_DirPath; set => ref_DirPath = value; }
        public string DirPath { get => dirPath; set => dirPath = value; }
        public int Load_State { get => _load_State; set => _load_State = value; }

        public int GetLoad_State()
        {
            return Load_State;
        }

        public void GetFilterList(List<ImageListInfo> OutputData)
        {
            FilterList.ForEach((item) => { OutputData.Add(item.Clone()); });
        }
        public Dictionary<string, ImageListInfo> GetDicinfo(Dictionary<string, ImageListInfo> OutputData)
        {
            OutputData = dicInfo;

            return OutputData;
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
            dataGridView1.DataSource = dt;

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
            if (All_Equipment_DF_List.Count <= 0)
                return;

            List<string> Each_Equipment_DF_Name = new List<string>();
            int[] Each_Equipment_DF_Count = new int[All_Equipment_DF_List.Count];

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Each_Equipment_DF_Name.Add(All_Equipment_DF_List.ElementAt(i).Item1);

            for (int i = 0; i < ImageDatabase.Count; i++)
                Each_Equipment_DF_Count[All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(ImageDatabase.ElementAt(i).EquipmentDefectName))]++;

            All_Equipment_DF_List.Clear();
            for (int i = 0; i < Each_Equipment_DF_Name.Count; i++)
                All_Equipment_DF_List.Add(new Tuple<string, int>(Each_Equipment_DF_Name.ElementAt(i), Each_Equipment_DF_Count[i]));

            All_Equipment_DF_List = All_Equipment_DF_List.OrderBy(s => s.Item2).ThenByDescending(s => s.Item2).ToList();
            All_Equipment_DF_List.Reverse();
        }

        private void Print_List()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;
            

            for (int i = 0; i < FilterList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[COLUMN_STR.GRID_IMGNAME] = dicInfo.Keys.ElementAt(i);
                dr[COLUMN_STR.GRID_STATE] = dicInfo[dicInfo.Keys.ElementAt(i)].ReviewDefectName;
                
                dt.Rows.Add(dr);

            }


        }

        public void Dl_PrintList()
        {
            FilterList.Clear();
            open.GetFilterList_Image(FilterList);

            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();
            dataGridView1.RowHeadersWidth = 30;

            for (int i = 0; i < FilterList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[COLUMN_STR.GRID_IMGNAME] = FilterList[i].Imagename;
                dr[COLUMN_STR.GRID_STATE]       = FilterList[i].ReviewDefectName; ;

                dt.Rows.Add(dr);

            }
        }

        private void _filterAct_bt_Click(object sender, EventArgs e)
        {
            FilterList.Clear();
            FilterList = ImageDatabase.ToList();
            Initial_Equipment_DF_FilterList();
            Print_List();
            open.Main = this;
            open.Set_View();
        }

        private void Initial_Equipment_DF_FilterList()
        {
            for (int i = 0; i < FilterList.Count; i++)
            {
                if (Selected_Equipment_DF_List.FindIndex(s => s.Equals(FilterList.ElementAt(i).EquipmentDefectName)) == -1)
                {
                    FilterList.RemoveAt(i);
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

        private void zipLoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ZipFileDialog = new OpenFileDialog();

            ZipFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //ZipFileDialog.Multiselect = true;
            ZipFileDialog.Filter = ZIP_STR.EXETENSION;

            if (ZipFileDialog.ShowDialog() != DialogResult.OK)
                return;

            InitialData();



            DirPath = Directory.GetParent(ZipFileDialog.FileName).ToString();
            ZipFilePath = ZipFileDialog.FileName;

            Load_State = 1;



            Func.SearchTXT_inZip(ZipFilePath, dic_ready, dicTxt_info);
            Func.SearchJPG_inZip(ZipFilePath, All_LotID_List, All_VerifyDF_List, All_Equipment_DF_List,  ImageDatabase,  dicInfo);
            FilterList = ImageDatabase.ToList();

            All_LotID_List.Sort();
            All_VerifyDF_List.Sort();
            Initial_Equipment_DF_List();
            ImageSizeList.Sort();
            ImageSizeList.Insert(0, FILTER_STR.IMGSIZE_ALL);



            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + " - " + All_Equipment_DF_List.ElementAt(i).Item2);

            Print_List();
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
    }
}

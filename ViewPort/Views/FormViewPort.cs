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
        List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();
        List<string> ImageSizeList = new List<string>();      
        
        private string _zipFilePath;
        public string ZipFilePath { get => _zipFilePath; set => _zipFilePath = value; }       

        private int _loadState; // -1:없음, 0:폴더, 1:파일
        public int LoadState { get => _loadState; set => _loadState = value; }

        #endregion

        #region UI CODE

        public FormViewPort()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            LoadState = -1;

            splitContainer1.Panel2.Controls.Add(open);
            open.Dock = DockStyle.Fill;

            DataTable dt = new DataTable();

            dt.Columns.Add("Lot ID");
            dt.Columns.Add("Verify 결과");
            dt.Columns.Add("D/L 결과(현재)");
            dt.Columns.Add("D/L 결과(추가)");
            dt.Columns.Add("검증 결과");
            dt.Columns.Add("Image 명");
            dt.Columns.Add("Frame");
            dt.Columns.Add("Camera");
            dt.Columns.Add("설비 검출명");
            dt.Columns.Add("Image 크기");

            dataGridView1.DataSource = dt;
        }

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

        private void SortList()
        {
            All_LotID_List.Sort();
            All_VerifyDF_List.Sort();
        }

        private void zipLoadFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ZipFilePath = Util.CommonOpenFileDlg(false);
            LoadState = 1;

            Func.SearchJPG_inZip(ZipFilePath, ImageDatabase, All_LotID_List, All_VerifyDF_List, All_Equipment_DF_List);

            SortList();
            Initial_Equipment_DF_List();

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + " - " + All_Equipment_DF_List.ElementAt(i).Item2);

            Print_List();
            MessageBox.Show(MSG_STR.LOAD_SUCCESS);

            open.Main = this;
            open.Set_View();
        }

        private void Print_List()
        {
            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();

            DataTable dt = (DataTable)dataGridView1.DataSource;
            DataRow dr = dt.NewRow();

            for (int i = 0; i < ImageDatabase.Count; i++)
                dt.Rows.Add(ImageDatabase.ElementAt(i).GetData());

            st.Stop();
        }

        #endregion

        public void GetFilterList(List<ImageListInfo> OutputData)
        {
            ImageDatabase.ForEach((item) => { OutputData.Add(item.Clone()); });
        }

    }
}

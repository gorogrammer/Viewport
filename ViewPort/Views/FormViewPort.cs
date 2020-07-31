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
    #region UI & INTERFACE
    public interface MyInterface
    {
        int GetLoad_State();

        void GetFilterList(List<ImageListInfo> OutputData);

    }

    #endregion

    public partial class FormViewPort : Form
    {
        ImageViewer open = new ImageViewer();


        TextBox[] Filter_TB_List = new TextBox[8];
        CheckBox[] Filter_CB_List = new CheckBox[8];
        CheckBox[] Exact_Filter_CB_List = new CheckBox[8];
        List<ImageListInfo> ImageDatabase = new List<ImageListInfo>();
        List<ImageListInfo> FilterList = new List<ImageListInfo>();

        List<string> All_LotID_List = new List<string>();
        List<string> All_VerifyDF_List = new List<string>();
        List<Tuple<string, int>> All_Equipment_DF_List = new List<Tuple<string, int>>();

        List<string> Selected_Equipment_DF_List = new List<string>();
        List<string> ImageSizeList = new List<string>();


        string DirPath, REF_DirPath, ZipFilePath;
        int Load_State; // -1:없음, 0:폴더, 1:파일
        int Resize_Width, Resize_Height;
        int Image_Aggregation_Option = 1;
        int Rotate_Option = 3;
        public FormViewPort()
        {
            InitializeComponent();
            ImageViwerOpen();
            gridview_Load();

        }

        public string GetZipFilePath()
        {
            return ZipFilePath;
        }
        public int GetLoad_State()
        {
            return Load_State;
        }
        public void GetFilterList(List<ImageListInfo> OutputData)
        {
            //return FilterList.CopyTo(OutputData);
            ImageDatabase.ForEach((item) => { OutputData.Add(item.Clone()); });
        }

        private void ImageViwerOpen()
        {

            splitContainer1.Panel2.Controls.Add(open);

            open.Dock = DockStyle.Fill;
        }

        private void InitialData()
        {
            ////ZipFileList.Clear();
            //ImageDatabase.Clear();
            //FilterList.Clear();
            //All_LotID_List.Clear();
            //All_VerifyDF_List.Clear();
            //All_Equipment_DF_List.Clear();
            //Selected_Equipment_DF_List.Clear();
            //ImageSizeList.Clear();
            //ImageSize_CB.Items.Clear();
            //Equipment_DF_CLB.Items.Clear();

            Load_State = -1;
            DirPath = "";
            REF_DirPath = "";
            Image_Aggregation_Option = 1;
            Rotate_Option = 0;
        }
        private void Initial_Equipment_DF_List()
        {
            if (All_Equipment_DF_List.Count <= 0)
                return;

            List<string> Each_Equipment_DF_Name = new List<string>();
            int[] Each_Equipment_DF_Count = new int[All_Equipment_DF_List.Count];

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Each_Equipment_DF_Name.Add(All_Equipment_DF_List.ElementAt(i).Item1);
            //Each_Equipment_DF_Count.Initialize();

            for (int i = 0; i < ImageDatabase.Count; i++)
                Each_Equipment_DF_Count[All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(ImageDatabase.ElementAt(i).EquipmentDefectName))]++;

            All_Equipment_DF_List.Clear();
            for (int i = 0; i < Each_Equipment_DF_Name.Count; i++)
                All_Equipment_DF_List.Add(new Tuple<string, int>(Each_Equipment_DF_Name.ElementAt(i), Each_Equipment_DF_Count[i]));

            All_Equipment_DF_List = All_Equipment_DF_List.OrderBy(s => s.Item2).ThenByDescending(s => s.Item2).ToList();
            All_Equipment_DF_List.Reverse();
        }

        private void gridview_Load()
        {



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

        private void zipLoadFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ZipFileDialog = new OpenFileDialog();

            ZipFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //ZipFileDialog.Multiselect = true;
            ZipFileDialog.Filter = "Zip Files (*.zip)|*.zip";

            if (ZipFileDialog.ShowDialog() != DialogResult.OK)
                return;

            InitialData();

            /*
            foreach (string EachFilePath in ZipFileDialog.FileNames)
                ZipFileList.Add(EachFilePath);
            ZipFileList.Sort();
            */

            DirPath = Directory.GetParent(ZipFileDialog.FileName).ToString();
            ZipFilePath = ZipFileDialog.FileName;
            //Path_TB.Text = Path.GetFileNameWithoutExtension(ZipFilePath);
            Load_State = 1;
            Rotate_Option = 1;

            /*
            for (int i = 0; i < ZipFileList.Count; i++)
                SearchJPG_inZip(ZipFileList.ElementAt(i));
            */
            SearchJPG_inZip(ZipFilePath);

            All_LotID_List.Sort();
            All_VerifyDF_List.Sort();
            Initial_Equipment_DF_List();
            ImageSizeList.Sort();
            ImageSizeList.Insert(0, "전체");

            //for (int i = 0; i < ImageSizeList.Count; i++)
            //    ImageSize_CB.Items.Add(ImageSizeList.ElementAt(i));

            //for (int i = 0; i < All_Equipment_DF_List.Count; i++)
            //    Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + " - " + All_Equipment_DF_List.ElementAt(i).Item2);

            //Initial_FilterList();
            //Print_List();

            for (int i = 0; i < All_Equipment_DF_List.Count; i++)
                Equipment_DF_CLB.Items.Add(All_Equipment_DF_List.ElementAt(i).Item1 + " - " + All_Equipment_DF_List.ElementAt(i).Item2);

            Print_List();
            MessageBox.Show("Done.");

            open.Main = this;
            open.Set_View();

            //    open.Main = this;
            //    open.Set_Image_DB();
        }

        private void Print_List()
        {
            int rowIndex = 0;
            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();

            DataTable dt = (DataTable)dataGridView1.DataSource;
            DataRow dr = dt.NewRow();

            for (int i = 0; i < ImageDatabase.Count; i++)
                dt.Rows.Add(ImageDatabase.ElementAt(i).GetData());

            //foreach (DataGridViewRow row in QuickDGV.Rows)
            //{
            //    //if (row.IsNewRow) continue;
            //    //row.HeaderCell.Value = rowNumber++.ToString();
            //    row.HeaderCell.Value = (FilterList.ElementAt(rowIndex++).index + 1).ToString();
            //}

            ////QuickDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            //QuickDGV.RowHeadersWidth = 70;
            //QuickDGV.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //QuickDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ////QuickDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //QuickDGV.ResumeLayout();

            st.Stop();
            //MessageBox.Show(st.Elapsed.ToString());


        }

        public void SearchJPG_inZip(string FilePath)
        {
            ZipArchive zip, subZip;
            Stream subEntryMS;
            Bitmap ImgInfo = null;

            string Lot_ID, Verify_Defect;
            string FileName, Equipment_Name;
            string ImageSize;
            int FrameNo, CameraNo;

            try
            {
                zip = ZipFile.Open(FilePath, ZipArchiveMode.Read);       // Zip파일(Lot) Load

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1)             // Zip파일 내에 Zip파일이 있을 경우...
                    {
                        subEntryMS = entry.Open();           // 2중 압축파일을 MemoryStream으로 읽는다.
                        subZip = new ZipArchive(subEntryMS);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.
                        foreach (ZipArchiveEntry subEntry in subZip.Entries)        // 2중 압축파일 내에 있는 파일을 탐색
                        {
                            if (subEntry.Name.ToUpper().IndexOf(".JPG") != -1)  // jpg 파일이 있을 경우 ( <= 각 이미지 파일에 대한 처리는 여기서... )
                            {
                                Lot_ID = Path.GetFileName(FilePath).Replace(".zip", "");
                                Verify_Defect = "-";
                                FileName = subEntry.Name.Replace(".jpg", "");
                                FrameNo = int.Parse(subEntry.Name.Substring(1, 5));
                                CameraNo = int.Parse(subEntry.Name.Substring(6, 2));
                                Equipment_Name = subEntry.Name.Substring(13, FileName.Length - 13).Split('@')[0].Split('.')[0];



                                ImageSize = "-";


                                if (FileName.Split('@').Length >= 3)
                                {
                                    Lot_ID = FileName.Split('@')[2].Split('.')[0];
                                    Verify_Defect = FileName.Split('@')[1];
                                }

                                if (All_LotID_List.FindIndex(s => s.Equals(Lot_ID)) == -1)
                                    All_LotID_List.Add(Lot_ID);
                                if (All_VerifyDF_List.FindIndex(s => s.Equals(Verify_Defect)) == -1)
                                    All_VerifyDF_List.Add(Verify_Defect);
                                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name)) == -1)
                                    All_Equipment_DF_List.Add(new Tuple<string, int>(Equipment_Name, -1));

                                ImageDatabase.Add(new ImageListInfo(ImageDatabase.Count, Lot_ID, Verify_Defect, "-", "-", "-", FileName, FrameNo, CameraNo, Equipment_Name, ImageSize, Directory.GetParent(FilePath).ToString()));
                            }
                        }
                        subZip.Dispose();
                    }
                    if (entry.Name.ToUpper().IndexOf("_XY.TXT") != -1)             // Zip파일 내에 Zip파일이 있을 경우...
                    {
                        //Set_Cordination(entry.Open());
                    }
                }
                zip.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Extract ERROR!\n" + ex.ToString());
                return;
            }
        }
        
    }
}

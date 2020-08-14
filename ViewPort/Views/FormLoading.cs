using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using ViewPort.Functions;

namespace ViewPort.Views
{
    public partial class FormLoading : Form
    {
        #region MEMBER Variables
        private DataTable dt;
        private Dictionary<string, ImageInfo> dic_Load;
        private Dictionary<string, txtInfo> dicTxt_info;
        private List<string> all_LotID_List;
        private List<string> all_VerifyDF_List;
        private List<Tuple<string, int>> all_Equipment_DF_List;

        public Dictionary<string, ImageInfo> Dic_Load { get => dic_Load; set => dic_Load = value; }
        public Dictionary<string, txtInfo> DicTxt_info { get => dicTxt_info; set => dicTxt_info = value; }
        public List<string> All_LotID_List { get => all_LotID_List; set => all_LotID_List = value; }
        public List<string> All_VerifyDF_List { get => all_VerifyDF_List; set => all_VerifyDF_List = value; }
        public List<Tuple<string, int>> All_Equipment_DF_List { get => all_Equipment_DF_List; set => all_Equipment_DF_List = value; }
        public DataTable Dt { get => dt; set => dt = value; }

        #endregion

        #region SAFE FUNCTION

        private void AddProgressBarValueSafe(int AddValue)
        {
            try
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value += AddValue));
                }
                else
                {
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value += AddValue));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SetProgressBarValueSafe(int Value)
        {
            try
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value = Value));
                }
                else
                {
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value = Value));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SetProgressBarMaxSafe(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.BeginInvoke(new Action(() => progressBar1.Maximum = value));
            }
            else
            {
                progressBar1.Maximum = value;
            }
        }

        private void ExitProgressBarSafe()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => this.Close()));
            }
            else
            {
                this.Close();
            }
        }

        private void EditFormNameSafe(string Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => this.Text = Value));
            }
            else
            {
                this.Text = Value;
            }
        }

        #endregion

        public FormLoading(string path)
        {
            dic_Load = new Dictionary<string, ImageInfo>();
            dicTxt_info = new Dictionary<string, txtInfo>();

            All_LotID_List = new List<string>();
            All_Equipment_DF_List = new List<Tuple<string, int>>();
            All_VerifyDF_List = new List<string>();

            InitializeComponent();

            DoLoadingThread(path);
        }

        public void DoLoadingThread(string path)
        {
            Thread LoadThread = new Thread(new ParameterizedThreadStart(Loading));
            LoadThread.Start(path);
        }

        private void Loading(object path)
        {
            string FilePath = (string)path;
            string LotName = Path.GetFileNameWithoutExtension(FilePath);

            EditFormNameSafe(MSG_STR.LOAD_ZIP);

            ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read);   // Zip파일(Lot) Load
            {
                SetProgressBarMaxSafe(zip.Entries.Count);
                foreach (ZipArchiveEntry entry in zip.Entries)
                    LoadSubZipAsync(LotName, entry);
            }

            EditFormNameSafe(MSG_STR.LOAD_SDIP_TXT);
            LoadTxtAsync(FilePath);

            EditFormNameSafe(MSG_STR.LOAD_ROWS);
            MakeDataTables();

            ExitProgressBarSafe();
        }

        private void LoadSubZipAsync(string LotName, ZipArchiveEntry entry)
        {
            if (entry.Name.ToUpper().IndexOf(".ZIP") == -1)
            {
                AddProgressBarValueSafe(1); // Count 추가
                return;
            }

            Stream subEntryMS = entry.Open();
            ZipArchive subZip = new ZipArchive(subEntryMS);

            foreach (ZipArchiveEntry subEntry in subZip.Entries)        // 2중 압축파일 내에 있는 파일을 탐색
            {
                if (!subEntry.Name.ToUpper().Contains(".JPG"))
                    continue;

                string FileName = Func.GetFileNameWithoutJPG(subEntry.Name);
                string File_ID = Func.GetPureFileID(subEntry.Name);
                string Equipment_Name = Func.GetEqpName(subEntry.Name);
                int FrameNo = Func.GetFrameNumber(subEntry.Name);
                int CameraNo = Func.GetCamNumber(subEntry.Name);


                if (All_LotID_List.FindIndex(s => s.Equals(LotName)) == -1)
                    All_LotID_List.Add(LotName);

                int x = 1;
                int index = 0;

                if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name)) == -1)
                    All_Equipment_DF_List.Add(new Tuple<string, int>(Equipment_Name, x));
                else
                {
                    index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name));
                    x = All_Equipment_DF_List[index].Item2;
                    All_Equipment_DF_List[index] = new Tuple<string, int>(Equipment_Name, ++x);

                }
                Dic_Load.Add(File_ID, new ImageInfo(LotName, FileName, CameraNo, FrameNo, Equipment_Name, "-", "-", "양품"));
            }

            subZip.Dispose();
            AddProgressBarValueSafe(1);
        }

        private void LoadTxtAsync(string FilePath)
        {
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetLotNameFromPath(FilePath));

                if (ImgEntry == null)
                {
                    MessageBox.Show(MSG_STR.NONE_SDIP_TXT);
                    return;
                }

                StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                string text = SR.ReadToEnd();
                string[] items = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < items.Length - 2; i++)
                {
                    string[] dic_ready = items[i + 1].Split(',');
                    dicTxt_info.Add(dic_ready[0].Substring(0, 12), new txtInfo(dic_ready[0].Substring(13, dic_ready[0].Length - 13), dic_ready[8], dic_ready[10], "양품"));
                }
            }
        }

        private void MakeDataTables()
        {
            Dt = new DataTable();
            Dt.Columns.Add(COLUMN_STR.GRID_IMGNAME);
            Dt.Columns.Add(COLUMN_STR.GRID_STATE);
            Dt.PrimaryKey = new DataColumn[] { dt.Columns[COLUMN_STR.GRID_IMGNAME] };

            SetProgressBarValueSafe(0);
            SetProgressBarMaxSafe(dic_Load.Count);

            foreach (KeyValuePair<string, ImageInfo> kvp in dic_Load)
            {
                Dt.Rows.Add(kvp.Key, kvp.Value.ReviewDefectName);
                AddProgressBarValueSafe(1);
            }

        }

    }

}

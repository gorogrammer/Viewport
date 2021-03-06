﻿using System;
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
        FormViewPort main;
        private DataTable dt;
        private Dictionary<string, ImageInfo> dic_Load;
        private Dictionary<string, txtInfo> dicTxt_info;
        private List<string> all_LotID_List;
        private List<string> all_VerifyDF_List;
        private List<Tuple<string, int>> all_Equipment_DF_List;
        private List<int> map_List;
        private List<string> ignore_map_List;
        private List<int> frame_List;
        private List<string> map_List_Compare;
        private List<string> dl_List;
        private Dictionary<int,int> map_List_Dic;
        private Dictionary<int, int> map_List_Dic_Compare;
        private List<string> dl_Apply_List;
        private List<string> dl_NOt_Apply_List;
        private List<int> contain_200_Frame_List;
        private List<int> f9_Frame_List;
        private List<string> imageSizeList;
        private List<int> f10_Frame_List;
        private List<string> f5_dic_Load;
        private List<string> sdip_no_200;
        private bool zip_Error;
        private List<string> overlap_key;

        private string[] final_text;
        private Dictionary<string, ImageInfo> f5_code_dicInfo_Loading;
        private Dictionary<string, ImageInfo> sdip_200_code_dicInfo;
        private Dictionary<string, ImageInfo> sdip_no1_dicload;
        private Dictionary<string, ImageInfo> Sorted_dic_GRID;
        private string betweenEA;
        public Dictionary<string, ImageInfo> Dic_Load { get => dic_Load; set => dic_Load = value; }

        public Dictionary<string, ImageInfo> F5_code_dicInfo_Loading { get => f5_code_dicInfo_Loading; set => f5_code_dicInfo_Loading = value; }
        public Dictionary<string, ImageInfo> Sdip_200_code_dicInfo { get => sdip_200_code_dicInfo; set => sdip_200_code_dicInfo = value; }

        public Dictionary<string, ImageInfo> Sdip_no1_dicload { get => sdip_no1_dicload; set => sdip_no1_dicload = value; }
        public Dictionary<string, txtInfo> DicTxt_info { get => dicTxt_info; set => dicTxt_info = value; }
        public List<string> All_LotID_List { get => all_LotID_List; set => all_LotID_List = value; }
        public List<string> Ignore_map_List { get => ignore_map_List; set => ignore_map_List = value; }
        public List<string> Overlap_key { get => overlap_key; set => overlap_key = value; }
        public bool Zip_Error { get => zip_Error; set => zip_Error = value; }
        public string[] Final_text { get => final_text; set => final_text = value; }
        public List<string> Sdip_no_200 { get => sdip_no_200; set => sdip_no_200 = value; }
        public string BetweenEA { get => betweenEA; set => betweenEA = value; }
        public List<string> All_VerifyDF_List { get => all_VerifyDF_List; set => all_VerifyDF_List = value; }
        public List<int> Map_List { get => map_List; set => map_List = value; }
        public List<string> Map_List_Compare { get => map_List_Compare; set => map_List_Compare = value; }
        public List<string> Dl_List { get => dl_List; set => dl_List = value; }
        public List<string> ImageSizeList { get => imageSizeList; set => imageSizeList = value; }

        public List<int> Contain_200_Frame_List { get => contain_200_Frame_List; set => contain_200_Frame_List = value; }
        public List<int> F9_Frame_List { get => f9_Frame_List; set => f9_Frame_List = value; }

        public List<int> F10_Frame_List { get => f10_Frame_List; set => f10_Frame_List = value; }

        public List<string> F5_dic_Load { get => f5_dic_Load; set => f5_dic_Load = value; }

        public List<string> Dl_Apply_List { get => dl_Apply_List; set => dl_Apply_List = value; }

        public List<string> Dl_NOt_Apply_List { get => dl_NOt_Apply_List; set => dl_NOt_Apply_List = value; }
        public List<int> Frame_List { get => frame_List; set => frame_List = value; }
        public List<Tuple<string, int>> All_Equipment_DF_List { get => all_Equipment_DF_List; set => all_Equipment_DF_List = value; }
        public DataTable Dt { get => dt; set => dt = value; }
        public Dictionary<int, int> Map_List_Dic { get => map_List_Dic; set => map_List_Dic = value; }
        public Dictionary<int, int> Map_List_Dic_Compare { get => map_List_Dic_Compare; set => map_List_Dic_Compare = value; }
        #endregion

        #region SAFE FUNCTION

        private void AddProgressBarValueSafe(int AddValue)
        {
            try
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value += AddValue));
                }
                else
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value += AddValue));
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
                    progressBar1.Invoke(new Action(() => progressBar1.Value = Value));
                }
                else
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value = Value));
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
                progressBar1.Invoke(new Action(() => progressBar1.Maximum = value));
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
                this.Invoke(new Action(() => this.Close()));
            }
            else if(zip_Error)
            {
                this.Close();
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
                this.Invoke(new Action(() => this.Text = Value));
            }
            else
            {
                this.Text = Value;
            }
        }

        #endregion

        public FormLoading(string path, FormViewPort parent)
        {
            zip_Error = false;
            dic_Load = new Dictionary<string, ImageInfo>();
            dicTxt_info = new Dictionary<string, txtInfo>();
            betweenEA = string.Empty;
            All_LotID_List = new List<string>();
            All_Equipment_DF_List = new List<Tuple<string, int>>();
            All_VerifyDF_List = new List<string>();
            Map_List = new List<int>();
            Map_List_Compare = new List<string>();
            Frame_List = new List<int>();
            Map_List_Dic = new Dictionary<int, int>();
            Map_List_Dic_Compare = new Dictionary<int, int>();
            Dl_List = new List<string>();
            Dl_Apply_List = new List<string>();
            Dl_NOt_Apply_List = new List<string>();
            Ignore_map_List = new List<string>();
            Contain_200_Frame_List = new List<int>();
            F9_Frame_List = new List<int>();
            ImageSizeList = new List<string>();
            main = parent;
            F10_Frame_List = new List<int>();
            F5_dic_Load = new List<string>();
            F5_code_dicInfo_Loading = new Dictionary<string, ImageInfo>();
            sdip_no1_dicload = new Dictionary<string, ImageInfo>();
            Sdip_no_200 = new List<string>();
            Sorted_dic_GRID = new Dictionary<string, ImageInfo>();
            Overlap_key = new List<string>();

            this.Opacity = 0.9;

            InitializeComponent();

            this.progressBar1.Size = new System.Drawing.Size(550, 26);
            this.ClientSize = new System.Drawing.Size(550, 26);
            DoLoadingThread(path);
        }

        public FormLoading()
        {
            
            InitializeComponent();

        }

        public void DoLoadingThread(string path)
        {
            
            Thread LoadThread = new Thread(new ParameterizedThreadStart(Loading));
            LoadThread.Start(path);
        }

        private void All_clear()
        {
            dic_Load.Clear();
            dicTxt_info.Clear();
            zip_Error = false;
            All_LotID_List.Clear();
            All_Equipment_DF_List.Clear();
            All_VerifyDF_List.Clear();
            Map_List.Clear();
            Map_List_Compare.Clear();
            Frame_List.Clear();
            Map_List_Dic.Clear();
            Map_List_Dic_Compare.Clear();
            Dl_List.Clear();
            Dl_Apply_List.Clear();
            Dl_NOt_Apply_List.Clear();
            Ignore_map_List.Clear();
            Contain_200_Frame_List.Clear();
            F9_Frame_List.Clear();
            ImageSizeList.Clear();
            Overlap_key.Clear();
            F10_Frame_List.Clear();
            F5_dic_Load.Clear();
            F5_code_dicInfo_Loading.Clear();
            sdip_no1_dicload.Clear();
            Sdip_no_200.Clear();
            Sorted_dic_GRID.Clear();
        }
        private void Loading(object path)
        {
            string FilePath = (string)path;
            string LotName = Path.GetFileNameWithoutExtension(FilePath);
           
            EditFormNameSafe(MSG_STR.LOAD_ZIP);

            All_clear();
            //Load_Old_Map_TxtAsync(FilePath);
            Load_Map_TxtAsync(FilePath);
            if(zip_Error)
            {
                FormViewPort test = new FormViewPort();
                ExitProgressBarSafe();
                

                return;
            }
            Load_DL_TxtAsync(FilePath);
            LoadTxtAsync(FilePath);


            ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read,Encoding.UTF8);   // Zip파일(Lot) Load
            {
               
                SetProgressBarMaxSafe(zip.Entries.Count);

               
                foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                {
                  
                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1 )
                    {
                        if (!main.View_Mode_RB.Checked &&!Ignore_map_List.Contains(int.Parse(entry.Name.Substring(0, 5)).ToString()))
                            Frame_List.Add(int.Parse(entry.Name.Substring(0, 5)));
                        else
                            Frame_List.Add(int.Parse(entry.Name.Substring(0, 5)));
                    }
                    if(!zip_Error)
                        LoadSubZipAsync(LotName, entry);
                    else
                    {
                        
                        zip.Dispose();
                        ExitProgressBarSafe();

                        return;
                    }
                   
                }

                zip.Dispose();
            }
            if (zip_Error)
            {
                FormViewPort test = new FormViewPort();
                ExitProgressBarSafe();
                test.zipLoadFileToolStripMenuItem_Click(null, null);
                
            }
            else
            {
                EditFormNameSafe(MSG_STR.LOAD_SDIP_TXT);


                Load_XY_TxtAsync(FilePath);


                EditFormNameSafe(MSG_STR.LOAD_ROWS);
                //Filtering();

                IMG_TXT_combine();
                MakeDataTables();

                //this.Close();
                ExitProgressBarSafe();
            }


        
          
        }

        private void LoadSubZipAsync(string LotName, ZipArchiveEntry entry)
        {
            string ImageSize;
            Bitmap ImgInfo = null;

            if (entry.Name.ToUpper().IndexOf(".ZIP") == -1)
            {
                AddProgressBarValueSafe(1); // Count 추가
                return;
            }

            //for (int p = 0; p < Contain_200_Frame_List.Count; p++)
            //{
            //    if (Map_List.Contains(Contain_200_Frame_List[p]))
            //    {
            //        Map_List.Remove(Contain_200_Frame_List[p]);
            //        p--;
            //    }
            //}

            Stream subEntryMS = entry.Open();
            ZipArchive subZip = null;
            try
            {
                
                subZip = new ZipArchive(subEntryMS,ZipArchiveMode.Read, false, Encoding.Default);
            }
            catch(Exception ex)
            {
                MessageBox.Show(entry.Name + " 압축파일에 문제가 있습니다. 확인 부탁드립니다.");
                zip_Error = true;
                return;
            }
            

            
            foreach (ZipArchiveEntry subEntry in subZip.Entries)        // 2중 압축파일 내에 있는 파일을 탐색
            {
                
                //string subEn_Name = subEntry.Name.ToString();
                //byte[] bytes = Encoding.Default.GetBytes(subEn_Name);
                //subEn_Name = Encoding.Default.GetString(bytes);

                if (!subEntry.Name.ToUpper().Contains(".JPG"))
                    continue;

                string FileName = Func.GetFileNameWithoutJPG(subEntry.Name);
                string File_ID = Func.GetPureFileID(subEntry.Name);
                string Equipment_Name = Func.GetEqpName(subEntry.Name);
                int FrameNo = Func.GetFrameNumber(subEntry.Name);
                int CameraNo = Func.GetCamNumber(subEntry.Name);

                if(File_ID == "B00005090009")
                { }
                
                if (main.checkBox1.Checked)
                {
                    ImgInfo = new Bitmap(subEntry.Open());
                    ImageSize = ImgInfo.Size.Width + "*" + ImgInfo.Size.Height;
                    ImgInfo.Dispose();
                    if (ImageSizeList.FindIndex(c => c.Equals(ImageSize)) == -1)
                        ImageSizeList.Add(ImageSize);
                }
                else
                    ImageSize = "-";

                if (All_LotID_List.FindIndex(s => s.Equals(LotName)) == -1)
                    All_LotID_List.Add(LotName);

                int x = 1;
                int index = 0;
                //if(main.)

                if(FrameNo==18)
                {

                }
                if(main.View_Mode_RB.Checked)
                {
                    if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name)) == -1)
                        All_Equipment_DF_List.Add(new Tuple<string, int>(Equipment_Name, x));
                    else
                    {
                        index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name));
                        x = All_Equipment_DF_List[index].Item2;
                        All_Equipment_DF_List[index] = new Tuple<string, int>(Equipment_Name, ++x);

                    }


                    Dic_Load.Add(File_ID, new ImageInfo(LotName, FileName, CameraNo, FrameNo, Equipment_Name, "0", "0", "양품", "O", "0", "0", ImageSize, "", "0"));
                }
                else
                {
                    if (Map_List.Count > 0)
                    {
                        if (Frame_List.Contains(FrameNo))
                        {
                            if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name)) == -1)
                                All_Equipment_DF_List.Add(new Tuple<string, int>(Equipment_Name, x));
                            else
                            {
                                index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name));
                                x = All_Equipment_DF_List[index].Item2;
                                All_Equipment_DF_List[index] = new Tuple<string, int>(Equipment_Name, ++x);

                            }

                            if(Dic_Load.ContainsKey(File_ID))
                            {
                                Overlap_key.Add(FileName);
                            }
                            else
                            {
                                Dic_Load.Add(File_ID, new ImageInfo(LotName, FileName, CameraNo, FrameNo, Equipment_Name, "0", "0", "양품", "O", "0", "0", ImageSize, "", "0"));
                            }
                           
                            
                        }
                    }
                    else
                    {
                        if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name)) == -1)
                            All_Equipment_DF_List.Add(new Tuple<string, int>(Equipment_Name, x));
                        else
                        {
                            index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(Equipment_Name));
                            x = All_Equipment_DF_List[index].Item2;
                            All_Equipment_DF_List[index] = new Tuple<string, int>(Equipment_Name, ++x);

                        }
                        Dic_Load.Add(File_ID, new ImageInfo(LotName, FileName, CameraNo, FrameNo, Equipment_Name, "0", "0", "양품", "O", "0", "0", ImageSize, "", "0"));
                    }

                }


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
                string[] encoding_check = items[0].Split(',');
                if(encoding_check[2] == "자동판정")
                {

                }
                else
                {
                    
                    StreamReader SR1 = new StreamReader(ImgEntry.Open(), Encoding.UTF8);
                    string text_1 = SR1.ReadToEnd();

                    items = text_1.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                }



                for (int i = 0; i < items.Length - 2; i++)
                {
                    string[] dic_ready = items[i + 1].Split(',');
                    dicTxt_info.Add(dic_ready[0].Substring(0, 12), new txtInfo(dic_ready[0].Substring(13, dic_ready[0].Length - 13), dic_ready[8], dic_ready[10], "양품", "0","0","0"));

                    if(dic_ready[8]=="" || dic_ready[8] ==" "|| dic_ready[8] == "-")
                    {
                        dic_ready[8] = "0";
                    }
                    if (200 <= int.Parse(dic_ready[8]) && int.Parse(dic_ready[8]) <= 299)
                    {
                        Sdip_no_200.Add(dic_ready[0].Substring(0, 12));
                    }


                    if (int.Parse(dic_ready[8]) == 221)
                    {
                        F5_dic_Load.Add(dic_ready[0].Substring(0, 12));
                    }
                    else if (int.Parse(dic_ready[8]) == 222)
                    {
                        F5_dic_Load.Add(dic_ready[0].Substring(0, 12));
                    }
                    else if (int.Parse(dic_ready[8]) == 215)
                    {
                        F5_dic_Load.Add(dic_ready[0].Substring(0, 12));
                    }

                    if (Dl_Apply_List.Contains(dic_ready[8]))
                    {
                        
                        if (F9_Frame_List.Contains(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5))))
                            continue;
                        else
                            F9_Frame_List.Add(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5)));
                    }
                    else if (Dl_NOt_Apply_List.Contains(dic_ready[8]))
                    {
                        if (F10_Frame_List.Contains(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5))))
                            continue;
                        else
                            F10_Frame_List.Add(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5)));
                    }
                    else if(int.Parse(dic_ready[8]) >= 200 && int.Parse(dic_ready[8]) <= 299)
                    {
                        if (Contain_200_Frame_List.Contains(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5))))
                            continue;
                        else
                            Contain_200_Frame_List.Add(int.Parse((dic_ready[0].Substring(0, 12)).Substring(1, 5)));
                    }


                }
                zip.Dispose();
            }
            
        }

        private void Load_XY_TxtAsync(string FilePath)
        {
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetXYFromPath(FilePath));

                if (ImgEntry == null)
                {
                    MessageBox.Show(MSG_STR.NONE_XY_TXT);
                    return;
                }

                StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                string text = SR.ReadToEnd();
                string[] items = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int E_index = Array.FindIndex(items, i => i == "E");
                
                for (int i = E_index+1; i < items.Length; i++)
                {
                    if(items[i] != "")
                    {
                        if(i == 12449)
                        { }

                        string[] dic_ready = items[i].Split(',');
                        if (dicTxt_info.ContainsKey(dic_ready[0].Substring(0, 12)))
                        {
                            dicTxt_info[dic_ready[0].Substring(0, 12)].Master_No = dic_ready[1];
                            dicTxt_info[dic_ready[0].Substring(0, 12)]._x_Location = dic_ready[2];
                            dicTxt_info[dic_ready[0].Substring(0, 12)]._y_Location = dic_ready[3];
                        }
                        else if (Dic_Load.ContainsKey(dic_ready[0].Substring(0, 12)))
                        {

                            Dic_Load[dic_ready[0].Substring(0, 12)].Master_NO = dic_ready[1];
                            Dic_Load[dic_ready[0].Substring(0, 12)].X_Location = dic_ready[2];
                            Dic_Load[dic_ready[0].Substring(0, 12)].Y_Location = dic_ready[3];
                        }

                    }


                }
                zip.Dispose();
            }
            
        }

        private void Load_DL_TxtAsync(string FilePath)
        {
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetDLFromPath(FilePath));

                if (ImgEntry == null)
                {
                    MessageBox.Show(MSG_STR.NONE_DL_TXT);
                    return;
                }

                StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                string text = SR.ReadToEnd();
                string[] items = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for(int i = 1; i < items.Length - 1; i++ )
                {
                    Dl_List.Add(items[i]);
                    
                }

                for(int p = 2; p < Dl_List.Count-1; p++)
                {
                    string[] split_string = Dl_List[p].Split(',', '%', ' ');
                    if(split_string[0] != "New" && split_string.Length > 2)
                    {
                        if (double.Parse(split_string[6]) < double.Parse(split_string[8]))
                            Dl_Apply_List.Add(split_string[0]);
                        else
                            Dl_NOt_Apply_List.Add(split_string[0]);
                    }
                  
                    
                }

                zip.Dispose();
            }

        }
        //private void Load_Old_Map_TxtAsync(string FilePath)
        //{
        //    using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
        //    {
        //        ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetOldMapFromPath(FilePath));

        //        if (ImgEntry == null)
        //        {
        //            MessageBox.Show(MSG_STR.NONE_MAP_TXT);
        //            return;
        //        }

        //        StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
        //        string text = SR.ReadToEnd();

        //        char[] df = { '@' };
        //        Final_text = text.Split(df);
        //        string[] items = text.Split(' ', '!');
        //        var items_List = items.ToList();
        //        int index = items_List.IndexOf("");


        //        for (int i = 0; i < items.Length; i++)
        //        {
        //            if (i < index && items[i].Length <= 9 && items[i].Length > 5)
        //            {

        //                map_List_Dic.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
        //            }

        //            else if (i > index && items[i].Length <= 9 && items[i].Length > 5)
        //            {
        //                Map_List_Compare.Add(items[i]);
        //                if (items[i].Contains("E@"))
        //                {
        //                    string change = items[i].Replace("E@", "");
        //                    items[i] = change;
        //                    Map_List_Dic_Compare.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
        //                }
        //                else
        //                    Map_List_Dic_Compare.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
        //            }


        //        }
        //        if (Map_List_Compare[Map_List_Compare.Count - 1].Contains("E@"))
        //        {
        //            string change = Map_List_Compare[Map_List_Compare.Count - 1].Replace("E@", "");
        //            Map_List_Compare[Map_List_Compare.Count - 1] = change;

        //        }
        //        zip.Dispose();

        //        main.Map_List_Dic_main = Map_List_Dic;
        //        main.Map_List_Dic_Compare_main = Map_List_Dic_Compare;
        //        main.Final_text_main = Final_text[1];
        //    }

        //    foreach (KeyValuePair<int, int> pair in map_List_Dic)
        //    {

        //        if (pair.Value == 88)
        //        {

        //            if (map_List_Dic_Compare[pair.Key] != 39 && map_List_Dic_Compare[pair.Key] != 40)
        //            {
        //                map_List.Add(pair.Key);
        //            }
        //            else
        //            {
        //                Ignore_map_List.Add(pair.Key.ToString());
        //            }

        //        }
        //        else
        //        {
        //            Ignore_map_List.Add(pair.Key.ToString());
        //        }


        //    }



        //}


        private void Load_Map_TxtAsync(string FilePath)
        {
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
            {
                try
                {

                    ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetMapFromPath(FilePath));
                    if (ImgEntry == null)
                    {
                        MessageBox.Show(MSG_STR.NONE_MAP_TXT);
                        return;
                    }

                    StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                    string text = SR.ReadToEnd();

                    char[] df = { '@' };
                    Final_text = text.Split(df);
                    string[] items = text.Split(' ', '!');
                    var items_List = items.ToList();
                    int index = items_List.IndexOf("");

                    int x = items_List.FindIndex(a=>a.Contains("K"));

                    for (int i = 0; i < items.Length; i++)
                    {

                        try
                        {

                            if (i < index && items[i].Length <= 9 && items[i].Length > 5)
                            {

                                map_List_Dic.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
                            }

                            else if (i > index && items[i].Length <= 9 && items[i].Length > 5 && i < x)
                            {

                                Map_List_Compare.Add(items[i]);
                                if (items[i].Contains("E@"))
                                {
                                    string change = items[i].Replace("E@", "");
                                    items[i] = change;
                                    Map_List_Dic_Compare.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
                                }
                                else
                                    Map_List_Dic_Compare.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
                            }
                            else if (items[i].Contains("E") && i < x)
                            {

                                int value = 0;
                                string[] change = items[i].Split('E');
                                if (int.TryParse(change[0], out value))
                                {
                                    betweenEA = string.Empty;
                                    betweenEA = "E" + change[1];
                                    items[i] = change[0];
                                    Map_List_Dic_Compare.Add(int.Parse(items[i].Substring(2)), int.Parse(items[i].Substring(0, 2)));
                                }
                                break;
                            }
                     


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("MAP TXT에 오류가 있습니다. 확인 부탁드립니다.");
                            zip_Error = true;
                            return;
                        }


                    }
                    if (Map_List_Compare[Map_List_Compare.Count - 1].Contains("E@"))
                    {
                        string change = Map_List_Compare[Map_List_Compare.Count - 1].Replace("E@", "");
                        Map_List_Compare[Map_List_Compare.Count - 1] = change;

                    }
                    zip.Dispose();

                    main.Between = betweenEA;
                    main.Map_List_Dic_main = Map_List_Dic;
                    main.Map_List_Dic_Compare_main = Map_List_Dic_Compare;
                    main.Final_text_main = Final_text[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show( " 압축파일에 문제가 있습니다. 확인 부탁드립니다.");
                    
                    return;
                }

                

             




             
            }

            foreach (KeyValuePair<int, int> pair in map_List_Dic)
            {
                
                if (pair.Value == 88 )
                {
                    try
                    {
                        if (map_List_Dic_Compare[pair.Key] != 39 && map_List_Dic_Compare[pair.Key] != 40)
                        {
                            map_List.Add(pair.Key);
                        }
                        else
                        {
                            Ignore_map_List.Add(pair.Key.ToString());
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Map TXT에서 " + pair.Key.ToString() + " 프레임이 매칭되지 않습니다.\r 제외 후 출력됩니다.");
                    }
                
                }
                else
                {
                    Ignore_map_List.Add(pair.Key.ToString());
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

            Sorted_dic_GRID = dic_Load.OrderBy(s => s.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            Dic_Load = Sorted_dic_GRID;

            foreach (KeyValuePair<string, ImageInfo> kvp in dic_Load)
            {
                Dt.Rows.Add(kvp.Value.Imagename, kvp.Value.ReviewDefectName);
                AddProgressBarValueSafe(1);
            }

        }

        private void Filtering()
        {
            int x = 0;
            int index = 1;
            foreach (string pair in dic_Load.Keys.ToList())
            {
                if (F5_dic_Load.Contains(pair))
                {
                    F5_code_dicInfo_Loading.Add(pair, dic_Load[pair]);
                }


                if (dic_Load[pair].sdip_no != "-" && 200 <= int.Parse(dic_Load[pair].sdip_no) && int.Parse(dic_Load[pair].sdip_no) <= 299)
                {
                    if (All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dic_Load[pair].EquipmentDefectName)) == -1)
                        continue;
                    else
                    {
                        index = All_Equipment_DF_List.FindIndex(s => s.Item1.Equals(dic_Load[pair].EquipmentDefectName));
                        x = All_Equipment_DF_List[index].Item2;
                        All_Equipment_DF_List[index] = new Tuple<string, int>(dic_Load[pair].EquipmentDefectName, --x);

                        if (x == 0)
                        {
                            All_Equipment_DF_List.RemoveAt(index);

                        }

                    }
                    Sdip_200_code_dicInfo.Add(pair, dic_Load[pair]);
                    dic_Load.Remove(pair);
                }

            }
        }

        public void IMG_TXT_combine()
         {
            Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
            Sorted_dic = Dic_Load.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            Dic_Load = Sorted_dic;

            foreach (KeyValuePair<string, Models.txtInfo> kvp in DicTxt_info)
            {
                if (Dic_Load.ContainsKey(kvp.Key))
                {
                    Dic_Load[kvp.Key].sdip_no = kvp.Value.SDIP_No;
                    Dic_Load[kvp.Key].sdip_result = kvp.Value.SDIP_Result;
                    Dic_Load[kvp.Key].X_Location = kvp.Value.X_Location;
                    Dic_Load[kvp.Key].Y_Location = kvp.Value.Y_Location;
                    Dic_Load[kvp.Key].Master_NO = kvp.Value.Master_No;


                    if (Dic_Load[kvp.Key].sdip_no == "1")
                    {
                        Sdip_no1_dicload.Add(kvp.Key, Dic_Load[kvp.Key]);
                    }

                }

            }
        }

    }

}

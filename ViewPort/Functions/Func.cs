using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using ViewPort.Views;

namespace ViewPort.Functions
{
    public class Func
    {
        Dictionary<string, ImageInfo> Combine_DicInfo_SDIP200 = new Dictionary<string, ImageInfo>();
        List<string> Exception_Frame = new List<string>();
        List<string> f12_List_del = new List<string>();
        List<string> WorkerList = new List<string>();
        public static void SearchTXT_inZip(string FilePath, Dictionary<string, txtInfo> dicTxt_info)
        {
            ZipArchive zip;
            List<string> lines = new List<string>();
            zip = ZipFile.Open(FilePath, ZipArchiveMode.Read);
            Char[] sd = { '_', ',' };

            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name.ToUpper().IndexOf(".TXT") != -1 && entry.Name.ToUpper().IndexOf("IMG") != -1)
                {
                    StreamReader SR = new StreamReader(entry.Open(), Encoding.Default);
                    
                    string text = SR.ReadToEnd();

                    string[] items = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int i = 0; i < items.Length - 2; i++)
                    {
                        string[] dic_ready = items[i + 1].Split(',');
                        dicTxt_info.Add(dic_ready[0].Substring(0, 12), new txtInfo(dic_ready[0].Substring(13, dic_ready[0].Length - 13), dic_ready[8], dic_ready[10], "양품","0","0","0"));

                    }
                }
            }
            zip.Dispose();
        }

       

        public static void DeleteJPG_inZIP(string FilePath, Dictionary<string, ImageInfo> dicInfo_del)
        {
            
            List<int> del_frame_List = new List<int>();
            ZipArchive zip, subZip;
            Stream subEntryMS;
            int dl_no = 0;
            Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
            Sorted_dic = dicInfo_del.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            dicInfo_del = Sorted_dic;

            foreach (string name in dicInfo_del.Keys.ToList())
            {
                if (del_frame_List.Contains(dicInfo_del[name].FrameNo))
                {

                }
                else
                {
                    del_frame_List.Add(dicInfo_del[name].FrameNo);
                }
            }


          try
            {
                //zip = ZipFile.Open(FilePath, ZipArchiveMode.Update);       // Zip파일(Lot) Load
                using(zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
                {
                   // ProgressBar1 progressBar = new ProgressBar1();
                   // progressBar.Show();
                   // progressBar.Focus();
                    //progressBar.TopLevel = true;
                   // progressBar.TopMost = true;
                    //progressBar.SetProgressBarMaxSafe(zip.Entries.Count);
                    //progressBar.Text = "Delete ZIP IMG...";
                    foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                    {

                        if (entry.Name.ToUpper().IndexOf(".ZIP") != -1 && del_frame_List.Contains(int.Parse(entry.Name.Substring(0, 5))))             // Zip파일 내에 Zip파일이 있을 경우...
                        {


                            subEntryMS = entry.Open();           // 2중 압축파일을 MemoryStream으로 읽는다.
                                                                 // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.

                            using (subZip = new ZipArchive(subEntryMS, ZipArchiveMode.Update,false,Encoding.Default))
                            {
                                //for (int i = 0; i < subZip.Entries.Count; i++)
                                //{
                                //    if (dicInfo_del.ContainsKey(subZip.Entries[i].Name.Substring(0, 12)))
                                //    {

                                //        ZipArchiveEntry del_entry = subZip.GetEntry(subZip.Entries[i].Name);
                                //        del_entry.Delete();
                                //        dl_no++;
                                //        i--;
                                //    }


                                //}

                                foreach(var item in subZip.Entries.ToList().OrderBy(x => x.Name))
                                {
                                    if(dicInfo_del.ContainsKey(item.Name.Substring(0,12)))
                                    {
                                        item.Delete();
                                        dl_no++;
                                    
                                    }
                                }

                                subZip.Dispose();
                            }


                          //  progressBar.AddProgressBarValueSafe(1);
                        }
                        if (dl_no == dicInfo_del.Count)
                        {
                            //progressBar.SetProgressBarValueSafe(zip.Entries.Count);
                          //  progressBar.ExitProgressBarSafe();
                            zip = null;
                            return;
                        }
                           
                    }
                    //progre
                    
                }

                //using (FileStream zipToOpen = new FileStream(FilePath, FileMode.Open))
                //{
                //    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                //    {
                //       foreach(var item in archive.Entries.OrderBy(x => x.Name))
                //        {
                //            if(del_frame_List.Contains(int.Parse(item.Name.Split('.')[0])))
                //            {

                //            }
                //        }
                //    }
                //}
                

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Extract ERROR!\n" + ex.ToString());

                return;
            }
            
        }

        public void Counting_IMG_inZip(string FilePath)
        {
            Exception_Frame.Clear();
            ZipArchive  subZip;
            Stream subEntryMS;

            ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read);   // Zip파일(Lot) Load
            {

                foreach (ZipArchiveEntry entry in zip.Entries.OrderBy(x => x.Name))
                {
                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1)
                    {
                        subEntryMS = entry.Open();           
                        subZip = new ZipArchive(subEntryMS);
                        if(subZip.Entries.Count == 0)
                        {
                            Exception_Frame.Add(entry.Name);
                        }
                        subZip.Dispose();
                    }
                    
                }

                zip.Dispose();
            }
            
        }
        public static void Map_TXT_Update_inZip(string FilePath, Dictionary<int, int> No_Counting, Dictionary<int, int> Front, Dictionary<int, int> Back, string bt)
        {

            ///////////////////////////
            ProgressBar1 progressBar = new ProgressBar1();
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetMapFromPath(FilePath));
                progressBar.Show();
                progressBar.Text = "MapTXT Update...";
                progressBar.SetProgressBarMaxSafe(100);
                if (ImgEntry == null)
                {
                    MessageBox.Show(MSG_STR.NONE_MAP_TXT);
                    return;
                }

                StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                string text = SR.ReadToEnd();
                SR.Close();
                char[] df = { '@' };
                string[] Final = null;

                string Final_Text = string.Empty;
                string[] items = text.Split(' ', '!');
                var items_List = items.ToList();

                if(bt != string.Empty)
                {
                    Final = text.Split(df);

                    Final_Text = bt + Final[1];
                }
                else
                {
                    Final = text.Split(df);

                    Final_Text = "E@" + Final[1];

                }
                

                string Start_Text = items[0].Substring(0, 3);
                string Frame_Count = items[0].Substring(3, 440);
                string No_Change_Text = items[0].Substring(443);


                StringBuilder Change_Frame_Count = new StringBuilder(Start_Text);
                string Zero = string.Empty;
                int Compare_No_length = 0;

                List<int> Compare_No = new List<int>();

                for (int i = 0; i <= Frame_Count.Length / 5 - 1; i++)
                {
                    Compare_No.Add(0);
                    //Compare_No.Add(int.Parse(Frame_Count.Substring(i * 5, 5)));

                }

                foreach (int pair in No_Counting.Keys.ToList())
                {
                    Compare_No[pair - 1] = No_Counting[pair];
                }

                for (int p = 0; p <= Compare_No.Count - 1; p++)
                {
                    Compare_No_length = Compare_No[p].ToString().Length;

                    if (Compare_No_length == 1)
                    {
                        Change_Frame_Count.Append("0000");
                    }
                    else if (Compare_No_length == 2)
                        Change_Frame_Count.Append("000");
                    else if (Compare_No_length == 3)
                        Change_Frame_Count.Append("00");
                    else if (Compare_No_length == 4)
                        Change_Frame_Count.Append("0");


                    Change_Frame_Count.Append(Compare_No[p]);
                }

                Change_Frame_Count.Append(No_Change_Text);

                
                


                foreach (int pair in Front.Keys.ToList())
                {
                    if(Front[pair] < 10)
                    {
                        Change_Frame_Count.Append(" " +"0"+Front[pair].ToString());
                    }
                    else
                    {
                        Change_Frame_Count.Append(" " + Front[pair].ToString());
                    }
                    

                    Compare_No_length = pair.ToString().Length;

                    if (Compare_No_length == 1)
                    {
                        Change_Frame_Count.Append("0000");
                    }
                    else if (Compare_No_length == 2)
                        Change_Frame_Count.Append("000");
                    else if (Compare_No_length == 3)
                        Change_Frame_Count.Append("00");
                    else if (Compare_No_length == 4)
                        Change_Frame_Count.Append("0");

                    Change_Frame_Count.Append(pair);
                }
                Change_Frame_Count.Append("!");

                foreach (int pair in Back.Keys.ToList())
                {
                   

                    if (Back[pair] < 10)
                    {
                        Change_Frame_Count.Append(" " + "0" + Back[pair].ToString());
                    }
                    else
                    {
                        Change_Frame_Count.Append(" " + Back[pair].ToString());
                    }

                    Compare_No_length = pair.ToString().Length;

                    if (Compare_No_length == 1)
                    {
                        Change_Frame_Count.Append("0000");
                    }
                    else if (Compare_No_length == 2)
                        Change_Frame_Count.Append("000");
                    else if (Compare_No_length == 3)
                        Change_Frame_Count.Append("00");
                    else if (Compare_No_length == 4)
                        Change_Frame_Count.Append("0");

                    Change_Frame_Count.Append(pair);
                }
                Change_Frame_Count.Append(Final_Text);

                ImgEntry.Delete();
                ZipArchiveEntry readmeEntry = zip.CreateEntry(Func.GetMapFromPath(FilePath));
                


                using (StreamWriter SW = new StreamWriter(readmeEntry.Open()))
                {
                    SW.WriteLine(Change_Frame_Count.ToString());
                   
                }
                progressBar.tabProgressBarSafe(100);
                
                zip.Dispose();
            }
            progressBar.ExitProgressBarSafe();
        }

        public static void Write_IMGTXT_inZip(string FilePath, Dictionary<string, ImageInfo> dicInfo, Dictionary<string, ImageInfo> SDIP_200_CODE, Dictionary<string, ImageInfo> dicinfo_copy, List<string> f12_del)
        {
            ProgressBar1 progressBar = new ProgressBar1();
            Dictionary<string, ImageInfo> Sorted_dic = new Dictionary<string, ImageInfo>();
            foreach (string pair in SDIP_200_CODE.Keys.ToList())
            {
                if(dicInfo.ContainsKey(pair))
                {

                }
                else
                {
                    dicInfo.Add(pair, SDIP_200_CODE[pair]);
                }
            }

            if(f12_del.Count>0)
            {
                foreach (string pair in f12_del)
                {
                    if (dicInfo.ContainsKey(pair))
                    {

                    }
                    else
                    {
                        dicInfo.Add(pair, dicinfo_copy[pair]);
                    }
                }
            }
            //Sorted_dic = dicInfo.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            //dicInfo = Sorted_dic;

            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
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
                if (encoding_check[2] == "자동판정")
                {
                    SR.Close();
                }
                else
                {
                    SR.Close();

                    StreamReader SR1 = new StreamReader(ImgEntry.Open(), Encoding.UTF8);
                    string text_1 = SR1.ReadToEnd();

                    items = text_1.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    SR1.Close();
                }

                string top = string.Empty;

                top = items[0];
                
                ImgEntry.Delete();

                ZipArchiveEntry readmeEntry = zip.CreateEntry(Func.GetLotNameFromPath(FilePath));

                using (StreamWriter SW = new StreamWriter(readmeEntry.Open(),Encoding.Default))
                {
                    progressBar.SetProgressBarMaxSafe((int)readmeEntry.Length);
                    progressBar.Text = "IMG TXT Change...";
                    progressBar.TopMost = true;
                    progressBar.Show();
                    SW.WriteLine(top);
                    for (int i = 0; i < dicInfo.Count; i++)
                    {
                        progressBar.AddProgressBarValueSafe(1);
                        SW.WriteLine(dicInfo.Keys.ElementAt(i) + "_" + dicInfo[dicInfo.Keys.ElementAt(i)].EquipmentDefectName + ",1,0,,,,,," + dicInfo[dicInfo.Keys.ElementAt(i)].sdip_no + ",," + dicInfo[dicInfo.Keys.ElementAt(i)].sdip_result + dicInfo[dicInfo.Keys.ElementAt(i)].Change_Code);

                    }
                }

                progressBar.ExitProgressBarSafe();
                zip.Dispose();
            }
          
        }

        private static Encoding GetTextEncodingInfo(string path)
        {
            Encoding enc;
            using (StreamReader sr = new StreamReader(path, true))
            {
                enc = sr.CurrentEncoding;
                sr.Close();
            }

            return enc;
        }
        public static string GetFileNameWithoutJPG(string str)
        {
            return str.Replace(".jpg", "");
        }

        public static string GetPureFileID(string str)
        {
            return str.Split('_')[0];
        }

        public static string GetEqpName(string str)
        {
            return str.Substring(13, str.Length - 13).Split('@')[0].Split('.')[0];
        }

        public static int GetFrameNumber(string str)
        {
            return int.Parse(str.Substring(1, 5));
        }

        public static int GetCamNumber(string str)
        {
            return int.Parse(str.Substring(6, 2));
        }

        public static string GetLotNameFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + "_Img.txt";
        }

        public static string GetXYFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + "_XY.txt";
        }
        public static string GetComentFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + "_Com.txt";
        }
        public static string GetMapFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + ".txt";
        }

        public static string GetOldMapFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + "_Old.txt";
        }

        public static string GetDLFromPath(string str)
        {
            return Path.GetFileNameWithoutExtension(str) + "_DL.txt";
        }

        public static void SaveDelFileID(Dictionary<string, ImageInfo> Waiting_Del,List<string> listdel)
        {
            
            string txtFilePath = string.Empty;
            if(Waiting_Del.Count > 0)
            {
                string Lot_Name = Waiting_Del.Values.ElementAt(0).LotID;
                SaveFileDialog saveFile = new SaveFileDialog();

                saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
                saveFile.FileName = Lot_Name + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (File.Exists(saveFile.FileName))
                {
                    txtFilePath = saveFile.InitialDirectory + "\\" + saveFile.FileName;

                    File.Delete(txtFilePath);

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath = saveFile.FileName;
                        File.WriteAllLines(txtFilePath, Waiting_Del.Keys);
                    }


                }
                else
                {
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath = saveFile.FileName;
                        File.WriteAllLines(txtFilePath, Waiting_Del.Keys);
                        StreamWriter writer;
                        writer = File.AppendText(txtFilePath);
                        writer.WriteLine("a");
                        writer.Close();
                        File.AppendAllLines(txtFilePath, listdel);
                    }
                }
                MessageBox.Show("저장 되었습니다.");
            }
            else
            {
                MessageBox.Show("저장할 데이터가 없습니다.", "알림", MessageBoxButtons.OK);
                
               

            }
                
            


        }
        public static void Rewrite_XY_TxtAsync(string FilePath, Dictionary<string, ImageInfo> Waiting_Del)
        {
            List<string> dic_ready = new List<string>();
            int del_index = 0;
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetXYFromPath(FilePath));

                if (ImgEntry == null)
                {
                    MessageBox.Show(MSG_STR.NONE_XY_TXT);
                    return;
                }

                StreamReader SR = new StreamReader(ImgEntry.Open(), Encoding.Default);
                string text = SR.ReadToEnd();
                SR.Dispose();
                string[] items = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int E_index = Array.FindIndex(items, i => i == "E") + 1;
                dic_ready = items.ToList();
                
                int Maxindex = dic_ready.Count - 1;
                
                using (StreamWriter writer = new StreamWriter(ImgEntry.Open(),Encoding.Default))
                {
                    string TestList = string.Empty;
                    for(int i=0; i< E_index; i++)
                    {
                        TestList = dic_ready[i];
                        writer.WriteLine(TestList);
                        
                    }
                    for (int i = E_index; i < Maxindex; i++)
                    {
                        bool deleteCheck = false;
                        if (dic_ready[i] == "")
                        {
                            return;
                        }
                        else
                        {
                            foreach (string del in Waiting_Del.Keys)
                            {
                                if (del.Equals(dic_ready[i].Substring(0, 12)))
                                {
                                    deleteCheck = true;
                                }
                            }
                            if (deleteCheck)
                            {

                            }
                            else
                            {
                                TestList = dic_ready[i];
                                writer.WriteLine(TestList);
                                
                            }
                        }
                    }
                    
                }
               

               

                zip.Dispose();
            }
        }

        
        public static void LoadDelFileID(FormViewPort parent, Dictionary<string, ImageInfo> Waiting_Del, Dictionary<string, ImageInfo> dicInfo)
        {
            FormViewPort Main = parent;
            
            string txtFilePath = string.Empty;

            OpenFileDialog loadFile = new OpenFileDialog();

            loadFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            loadFile.FileName = "*.txt";
            loadFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            StringBuilder sb = new StringBuilder();


            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                txtFilePath = loadFile.FileName;
                StreamReader SR = new StreamReader(txtFilePath, Encoding.Default);

                string text = SR.ReadToEnd();
                string[] items = text.Split(new string[] { "\r\n", "a" }, StringSplitOptions.None);
                int split_index = Array.FindIndex(items, i => i == "");
                int index = items.Count() - 1;

                items = items.Where(s => s != items[index]).ToArray();

                for (int p = 0; p < items.Length; p++)
                {
                    if(p < split_index)
                    {
                        if (Waiting_Del.ContainsKey(items[p]))
                            continue;
                        else
                        {
                            Waiting_Del.Add(items[p], dicInfo[items[p]]);

                        }
                    }
                    else
                    {
                        if (Main.F12_del_list_main.Contains(items[p]))
                        {

                        }
                        else
                            Main.F12_del_list_main.Add(items[p]);
                    }
                }

         

            }

            Main.Waiting_Del = Waiting_Del;
            Main.Load_saveFile();
            }
        public static void Worker_Insert_Alzip(string Coment, string FilePath)
        {
            Random random = new Random();
            string FakeSTR = string.Empty;
            string OldData = string.Empty;
            for(int i=0; i< 10; i++)
            {
                FakeSTR = FakeSTR + random.Next(100).ToString();
            }
            
            // List<string> dic_ready = new List<string>();
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry ImgEntry;
                List<string> ComentData = new List<string>();
                       ImgEntry = zip.GetEntry(Func.GetComentFromPath(FilePath));
                if (ImgEntry == null)
                {
                    ImgEntry = zip.CreateEntry(Func.GetComentFromPath(FilePath));
                    using (StreamWriter SW = new StreamWriter(ImgEntry.Open()))
                    {
                        FakeSTR = FakeSTR + "," + Coment;
                        SW.WriteLine(FakeSTR);
                    }

                    zip.Dispose();

                    return;
                }
                using (StreamReader SR = new StreamReader(ImgEntry.Open()))
                {

                    
                    //ComentData.RemoveAt(ComentData.Count - 1);
                    OldData = SR.ReadToEnd();
                    if (OldData.Contains(Coment))
                    {
                        return ;
                    }
                    // ComentData.Add("\r\n");

                   
                }
               
                  
                using (StreamWriter SW = new StreamWriter(ImgEntry.Open()))
                {
                    FakeSTR = FakeSTR + "," + Coment;
                    SW.Write(OldData);
                    SW.WriteLine(FakeSTR);
                    
                    
                }

                 zip.Dispose();
                    
               
            }
            
        }
        public static void Coment_Insert_Alzip(string Coment, string FilePath)
        {

            // List<string> dic_ready = new List<string>();
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry ImgEntry;
                string OldData = string.Empty;
                List<string> b = new List<string>();
                ImgEntry = zip.GetEntry(Func.GetComentFromPath(FilePath));
                using (StreamReader SR = new StreamReader(ImgEntry.Open()))
                {

                    OldData= SR.ReadToEnd();
                    //ComentData.RemoveAt(ComentData.Count - 1);

                }


                using (StreamWriter SW = new StreamWriter(ImgEntry.Open()))
                {
                    
                    SW.Write(OldData);
                    SW.WriteLine("@");                   
                    SW.WriteLine(Coment);
                    
                }

                zip.Dispose();


            }

        }
        public static DataTable Get_Lot_WorkerList(string FilePath)
        {
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Read))
            {
                ZipArchiveEntry ImgEntry;
                List<string> ComentData = new List<string>();
                ImgEntry = zip.GetEntry(Func.GetComentFromPath(FilePath));
                using (StreamReader SR = new StreamReader(ImgEntry.Open()))
                {

                    ComentData.AddRange(SR.ReadToEnd().Split('@')[0].Replace("\r\n", ",").Split(',').ToList());
                    //ComentData.RemoveAt(ComentData.Count - 1);

                   
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("Lot작업자");
                foreach (string Worker in ComentData)
                {
                    DES des = new DES("carlo123");
                    if (Worker != "")
                    {
                        string decrypt = des.result(DesType.Decrypt, Worker);
                        dt.Rows.Add(decrypt);
                    }
                }
                return dt;
            }
        }
        public static void Delete_Insert_text(List<string> deleteList, string FilePath,string Worker,string LotName)
        {
            try
            {
                List<string> ReadText = new List<string>();
                string reFilePath = FilePath.Replace("/", @"\");
                string deleteTxtPath = reFilePath + @"\" + LotName + "_" + Worker + ".txt";
                if (File.Exists(deleteTxtPath))
                {
                    using (StreamReader SR = new StreamReader(deleteTxtPath))
                    {
                        ReadText.AddRange(SR.ReadToEnd().Replace("\r\n", ",").Split(',').ToList());
                        ReadText.RemoveAt(ReadText.Count - 1);
                    }

                    using (StreamWriter SW = new StreamWriter(deleteTxtPath))
                    {
                        deleteList.AddRange(ReadText);
                        foreach (string delete in deleteList)
                        {
                            SW.WriteLine(delete);
                        }
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(deleteTxtPath))
                    {
                        deleteList.AddRange(ReadText);
                        foreach (string delete in deleteList)
                        {
                            SW.WriteLine(delete);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public static string GetMachineName(string MachineData)
        {
            if(MachineData == MachineInfo.EAOI_2M_01)
            {
                return MachineInfo.EAOI_2M_01_STR;
            }
            else if(MachineData == MachineInfo.EAOI_2M_02)
            {
                return MachineInfo.EAOI_2M_02_STR;
            }
            else if (MachineData == MachineInfo.EAOI_2M_04)
            {
                return MachineInfo.EAOI_2M_04_STR;
            }
            else if (MachineData == MachineInfo.EAOI_2M_05)
            {
                return MachineInfo.EAOI_2M_05_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_01)
            {
                return MachineInfo.FVI_1M_01_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_02)
            {
                return MachineInfo.FVI_1M_02_STR;
            }
            else if(MachineData == MachineInfo.FVI_1M_03)
            {
                return MachineInfo.FVI_1M_03_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_04)
            {
                return MachineInfo.FVI_1M_04_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_05)
            {
                return MachineInfo.FVI_1M_05_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_06)
            {
                return MachineInfo.FVI_1M_06_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_07)
            {
                return MachineInfo.FVI_1M_07_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_08)
            {
                return MachineInfo.FVI_1M_08_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_09)
            {
                return MachineInfo.FVI_1M_09_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_10)
            {
                return MachineInfo.FVI_1M_10_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_11)
            {
                return MachineInfo.FVI_1M_11_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_12)
            {
                return MachineInfo.FVI_1M_12_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_13)
            {
                return MachineInfo.FVI_1M_13_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_14)
            {
                return MachineInfo.FVI_1M_14_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_15)
            {
                return MachineInfo.FVI_1M_15_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_16)
            {
                return MachineInfo.FVI_1M_16_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_17)
            {
                return MachineInfo.FVI_1M_17_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_18)
            {
                return MachineInfo.FVI_1M_18_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_19)
            {
                return MachineInfo.FVI_1M_19_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_20)
            {
                return MachineInfo.FVI_1M_20_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_21)
            {
                return MachineInfo.FVI_1M_21_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_22)
            {
                return MachineInfo.FVI_1M_22_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_23)
            {
                return MachineInfo.FVI_1M_23_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_24)
            {
                return MachineInfo.FVI_1M_24_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_25)
            {
                return MachineInfo.FVI_1M_25_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_30)
            {
                return MachineInfo.FVI_1M_30_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_33)
            {
                return MachineInfo.FVI_1M_33_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_35)
            {
                return MachineInfo.FVI_1M_35_STR;
            }
            else if (MachineData == MachineInfo.FVI_1M_36)
            {
                return MachineInfo.FVI_1M_36_STR;

            }
            else if (MachineData == MachineInfo.FVI_2M_37)
            {
                return MachineInfo.FVI_2M_37_STR;
            }
            else if (MachineData == MachineInfo.FVI_2M_38)
            {
                return MachineInfo.FVI_2M_38_STR;
            }
            else if (MachineData == MachineInfo.FVI_2M_39)
            {
                return MachineInfo.FVI_2M_39_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_01)
            {
                return MachineInfo.SOI_1M_01_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_02)
            {
                return MachineInfo.SOI_1M_02_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_03)
            {
                return MachineInfo.SOI_1M_03_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_04)
            {
                return MachineInfo.SOI_1M_04_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_05)
            {
                return MachineInfo.SOI_1M_05_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_06)
            {
                return MachineInfo.SOI_1M_06_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_07)
            {
                return MachineInfo.SOI_1M_07_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_08)
            {
                return MachineInfo.SOI_1M_08_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_09)
            {
                return MachineInfo.SOI_1M_09_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_10)
            {
                return MachineInfo.SOI_1M_10_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_11)
            {
                return MachineInfo.SOI_1M_11_STR;
            }
            else if (MachineData == MachineInfo.SOI_1M_12)
            {
                return MachineInfo.SOI_1M_12_STR;
            }
            else if (MachineData == MachineInfo.SOI_2M_01)
            {
                return MachineInfo.SOI_2M_01_STR;
            }
            else if (MachineData == MachineInfo.SOI_2M_02)
            {
                return MachineInfo.SOI_2M_02_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_01_A)
            {
                return MachineInfo.TOI_1M_01_A_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_01_B)
            {
                return MachineInfo.TOI_1M_01_B_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_02_A)
            {
                return MachineInfo.TOI_1M_02_A_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_02_B)
            {
                return MachineInfo.TOI_1M_02_B_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_03_A)
            {
                return MachineInfo.TOI_1M_03_A_STR;
            }
            else if (MachineData == MachineInfo.TOI_1M_03_B)
            {
                return MachineInfo.TOI_1M_03_B_STR;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}

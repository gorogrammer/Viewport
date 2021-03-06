﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

                             

                        }
                        if (dl_no == dicInfo_del.Count)
                        {
                            zip = null;
                            return;
                        }
                           
                    }

                    
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
            using (ZipArchive zip = ZipFile.Open(FilePath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry ImgEntry = zip.GetEntry(Func.GetMapFromPath(FilePath));

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
                zip.Dispose();
            }

        }

        public static void Write_IMGTXT_inZip(string FilePath, Dictionary<string, ImageInfo> dicInfo, Dictionary<string, ImageInfo> SDIP_200_CODE, Dictionary<string, ImageInfo> dicinfo_copy, List<string> f12_del)
        {

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
                    SW.WriteLine(top);
                    for (int i = 0; i < dicInfo.Count; i++)
                    {
                        SW.WriteLine(dicInfo.Keys.ElementAt(i) + "_" + dicInfo[dicInfo.Keys.ElementAt(i)].EquipmentDefectName + ",1,0,,,,,," + dicInfo[dicInfo.Keys.ElementAt(i)].sdip_no + ",," + dicInfo[dicInfo.Keys.ElementAt(i)].sdip_result + dicInfo[dicInfo.Keys.ElementAt(i)].Change_Code);
                    }
                }

         
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
          
    }
}

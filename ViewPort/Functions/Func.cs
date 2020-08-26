using System;
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

namespace ViewPort.Functions
{
    public class Func
    {
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
                        dicTxt_info.Add(dic_ready[0].Substring(0, 12), new txtInfo(dic_ready[0].Substring(13, dic_ready[0].Length - 13), dic_ready[8], dic_ready[10], "양품"));

                    }

                }
            }
        }

        public static void SearchJPG_inZip(string FilePath, List<string> All_LotID_List, List<string> All_VerifyDF_List, List<Tuple<string, int>> All_Equipment_DF_List,
               ConcurrentDictionary<string, ImageInfo> dicInfo)
        {
            ZipArchive zip, subZip;
            Stream subEntryMS;
            string Lot_ID, Verify_Defect;
            string FileName, Equipment_Name, File_ID;

            int FrameNo, CameraNo;


            //ImageDatabase = null;
            //dicInfo = null;

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
                                File_ID = FileName.Split('_')[0];
                                FrameNo = int.Parse(subEntry.Name.Substring(1, 5));
                                CameraNo = int.Parse(subEntry.Name.Substring(6, 2));
                                Equipment_Name = subEntry.Name.Substring(13, FileName.Length - 13).Split('@')[0].Split('.')[0];

                                

                                if (FileName.Split('@').Length >= 3)
                                {
                                    Lot_ID = FileName.Split('@')[2].Split('.')[0];

                                }

                                if (All_LotID_List.FindIndex(s => s.Equals(Lot_ID)) == -1)
                                    All_LotID_List.Add(Lot_ID);
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


                                //ImageDatabase.Add(new ImageListInfo(ImageDatabase.Count, Lot_ID, Verify_Defect, "-", "-", "양품", FileName, File_ID, FrameNo, CameraNo, Equipment_Name, ImageSize, Directory.GetParent(FilePath).ToString()));
                                //dicInfo.Add(File_ID, new ImageListInfo(ImageDatabase.Count, Lot_ID, Verify_Defect, "-", "-", "양품", FileName, File_ID, FrameNo, CameraNo, Equipment_Name, ImageSize, Directory.GetParent(FilePath).ToString()));
                                dicInfo.TryAdd(File_ID, new ImageInfo(Lot_ID, FileName, CameraNo, FrameNo, Equipment_Name, "-", "-", "양품","O"));

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

        public static void DeleteJPG_inZIP(string FilePath, Dictionary<string, ImageInfo> dicInfo_del)
        {
            ZipArchive zip, subZip;
            Stream subEntryMS;
          try
            {
                zip = ZipFile.Open(FilePath, ZipArchiveMode.Update);       // Zip파일(Lot) Load

                
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    
                    if (entry.Name.ToUpper().IndexOf(".ZIP") != -1)             // Zip파일 내에 Zip파일이 있을 경우...
                    {   

                        subEntryMS = entry.Open();           // 2중 압축파일을 MemoryStream으로 읽는다.
                        subZip = new ZipArchive(subEntryMS, ZipArchiveMode.Update);         // MemoryStream으로 읽은 파일(2중 압축파일) 각각을 ZipArchive로 읽는다.
                   
                        for(int i = 0; i < subZip.Entries.Count; i++ )
                        {
                            if (dicInfo_del.ContainsKey(subZip.Entries[i].Name.Substring(0, 12)))
                            {

                                ZipArchiveEntry del_entry = subZip.GetEntry(subZip.Entries[i].Name);
                                del_entry.Delete();

                                i--;
                            }
                        }

                        subZip.Dispose();
                       
                    }
                    zip.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Extract ERROR!\n" + ex.ToString());

                return;
            }
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

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace ViewPort.Functions
{
    public class Util
    {
        public static string OpenFolderDlg()
        {
            try
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();

                if (folderDlg.ShowDialog() == DialogResult.OK)
                {
                    return folderDlg.SelectedPath;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return string.Empty;
        }

        public static string CommonOpenFileDlg(bool isFolderPicker)
        {   
            try
            {
                var CommonOpenfiledlg = new CommonOpenFileDialog();
                CommonOpenfiledlg.Multiselect = false;

                if (isFolderPicker)
                    CommonOpenfiledlg.IsFolderPicker = true;

                if (CommonOpenfiledlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Directory.Exists(CommonOpenfiledlg.FileNames.ElementAt(0)) && CommonOpenfiledlg.FileNames.Count() == 1)
                    {                        
                        string path = CommonOpenfiledlg.FileNames.ElementAt(0); //Returns FolderPath
                        Console.WriteLine(path);
                        return path;
                    }
                    else
                    {
                        string path = CommonOpenfiledlg.FileNames.ElementAt(0); //Returns FilePath
                        Console.WriteLine(path);
                        return path;
                    }
                }
                else
                {
                    Console.WriteLine(MSG_STR.NON_SELECTED);
                    return string.Empty;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return string.Empty;
            }
        }

        public static string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
    }
}

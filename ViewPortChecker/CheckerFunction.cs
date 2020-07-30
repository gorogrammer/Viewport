using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPortNetwork;

namespace ViewPortChecker
{
    public class CheckerFunction
    {
        public static string GetAssemblyVersion(string path)
        {
            FileInfo exeInfo = new FileInfo(path);
            string ReturnValue = string.Empty;
            if (!exeInfo.Exists)
            {
                MessageBox.Show("경로가 입력되지 않았거나 Release File이 없습니다.");
                return ReturnValue;
            }
            else
            {
                Assembly asm = Assembly.ReflectionOnlyLoadFrom(exeInfo.FullName);
                ReturnValue = asm.GetName().Version.ToString();
                asm = null;                
            }
            
            return ReturnValue;
        }

        public static bool Update(string Version)
        {
            try
            {
                string NewVersionFolder = Path.Combine(NET_DEF.CARLO_NAS_PATH, Version);

                foreach (string filePath in Directory.GetFiles(NewVersionFolder))
                {
                    if (filePath.Contains(CHECKER_STR.CHECKER))
                        continue;

                    string FileName = Path.GetFileName(filePath);
                    File.Copy(filePath, Path.Combine(CHECKER_STR.INSTORAGE, FileName), true);
                }
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

    }
}

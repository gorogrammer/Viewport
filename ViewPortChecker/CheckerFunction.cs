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
        public static bool Update(string Version)
        {
            try
            {
                string NewVersionFolder = Path.Combine(NET_DEF.STEMCO_NAS_PATH, Version);

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

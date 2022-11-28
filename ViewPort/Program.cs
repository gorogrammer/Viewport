using SDIP.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace ViewPort
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ///////////////////////////////////////////////////
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, bargs) =>
            //{
            //    string dllName = new AssemblyName(bargs.Name).Name + ".dll";
            //    var assem = Assembly.GetExecutingAssembly();
            //    string resourceName = null;
            //    foreach (string str in assem.GetManifestResourceNames())
            //    {
            //        if (str.IndexOf(dllName) != -1)
            //        {
            //            resourceName = str;
            //            break;
            //        }
            //    }
            //    if (resourceName == null) return null;

            //    using (var stream = assem.GetManifestResourceStream(resourceName))
            //    {
            //        Byte[] assemblyData = new Byte[stream.Length];
            //        stream.Read(assemblyData, 0, assemblyData.Length);
            //        return Assembly.Load(assemblyData);
            //    }
            //};// --> DLL 없이 단일파일 배포에 사용
            ///////////////////////////////////////////////////
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SDIP.Forms.FormLogin loginForm = new SDIP.Forms.FormLogin();
            loginForm.StartPosition = FormStartPosition.CenterParent;
            loginForm.ShowDialog();
            if (loginForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Application.Run(new FormViewPort(loginForm.UseInfomation));
            }
            else
            {
                

            }
            
        }
    }
}

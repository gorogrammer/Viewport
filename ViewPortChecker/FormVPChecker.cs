using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using ViewPortNetwork;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.IO;
using CredentialManagement;
namespace ViewPortChecker
{
    public partial class FormViewPortChecker : Form
    {
        public string VersionInfo;
        public FormViewPortChecker()
        {
            InitializeComponent();
            Init();
            Thread ManualAlgorithmThread = new Thread(new ParameterizedThreadStart(CheckerDefaultThread));
            ManualAlgorithmThread.Start(4);
            CenterToScreen();
        }

        void Init()
        {

            Credential checkCredential_91 = new Credential
            {
                Target = "16.100.29.91",
                Username = "admin",
                Password = "qwe123!@#",
                PersistanceType = PersistanceType.LocalComputer
            };
            if (checkCredential_91.Load() == true)
            { }
            else
            { checkCredential_91.Save(); }

            Credential checkCredential_90 = new Credential
            {
                Target = "16.100.29.90",
                Username = "admin",
                Password = "qwe123!@#",
                PersistanceType = PersistanceType.LocalComputer
            };
            if (checkCredential_90.Load() == true)
            { }
            else
            { checkCredential_90.Save(); }



            if (File.Exists(CHECKER_STR.PATH))
                VersionInfo = AssemblyName.GetAssemblyName(CHECKER_STR.PATH).Version.ToString();
        }

        #region SafeThread Functions

        private void AddProgressBarValueSafe(int AddValue)
        {
            try
            {
                if (UI_ProgressBar.InvokeRequired)
                {
                    UI_ProgressBar.BeginInvoke(new Action(() => UI_ProgressBar.Value += AddValue));
                }
                else
                {
                    UI_ProgressBar.BeginInvoke(new Action(() => UI_ProgressBar.Value += AddValue));
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
                if (UI_ProgressBar.InvokeRequired)
                {
                    UI_ProgressBar.BeginInvoke(new Action(() => UI_ProgressBar.Value = Value));
                }
                else
                {
                    UI_ProgressBar.BeginInvoke(new Action(() => UI_ProgressBar.Value = Value));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SetProgressBarMaxSafe(int value)
        {
            if (UI_ProgressBar.InvokeRequired)
            {
                UI_ProgressBar.BeginInvoke(new Action(() => UI_ProgressBar.Maximum = value));
            }
            else
            {
                UI_ProgressBar.Maximum = value;
            }
        }

        private void ExitSafe()
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

        private void EditLabelTextSafe(string Value)
        {
            if (LB_Status.InvokeRequired)
            {
                LB_Status.BeginInvoke(new Action(() => LB_Status.Text = Value));
            }
            else
            {
                LB_Status.Text = Value;
            }
        }

        #endregion

        public bool Start_ViewPort()
        {
            try
            {
                ProcessStartInfo proInfo = new ProcessStartInfo();                
                proInfo.UseShellExecute = true;
                proInfo.FileName = CHECKER_STR.PATH;
                Process.Start(proInfo);
                return true;

            }
            catch (Exception e)
            {
                //FileIO.LogSave(e.ToString());
            }
            return false;
        }

        public bool Update_ViewPort(string Version)
        {
            if (Exit_ViewPort())//종료된 것 확인
            {
                Thread.Sleep(1000);

                if (CheckerFunction.Update(Version))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool Exit_ViewPort()
        {
            try
            {
                if (KillViewPort())
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        public bool KillViewPort()
        {
            while (true)
            {
                try
                {
                    Process[] processList = Process.GetProcessesByName(CHECKER_STR.NAME);
                    if (processList.Length > 0)
                    {
                        foreach (Process p in processList)
                            p.Kill();

                        return true;
                    }

                    //DEBUG Mode Kill
                    Process[] processList_ = Process.GetProcessesByName(CHECKER_STR.PROC_NAME);
                    if (processList_.Length > 0)
                    {
                        foreach (Process p in processList_)
                            p.Kill();

                        return true;
                    }

                    if (processList.Length == 0 && processList_.Length == 0)
                        return true;
                    
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void CheckerDefaultThread(object delaySecond)
        {
            int second = (int)delaySecond;
            SetProgressBarValueSafe(0);
            SetProgressBarMaxSafe(second);

            EditLabelTextSafe(MSG_STR.PING_NAS);
            Thread.Sleep(1000);

            //Setup file을 통해 설치된 경로에 실행파일이 있는지 확인
            if (string.IsNullOrEmpty(VersionInfo)) { MessageBox.Show(MSG_STR.ERR_CHECK_VER_THIS); ExitSafe(); }

            //NAS IP와 통신되는 지 확인
            if (!NetworkFunc.Connect(NET_DEF.STEMCO_NAS_IP)) { MessageBox.Show(MSG_STR.ERR_PING_NAS); ExitSafe(); }

            EditLabelTextSafe(MSG_STR.PING_DB);
            Thread.Sleep(1000);

            //DB IP와 통신되는지 확인
            if (!NetworkFunc.Connect(NET_DEF.STEMCO_DB_IP)) { MessageBox.Show(MSG_STR.ERR_PING_DB); ExitSafe(); }

            EditLabelTextSafe(MSG_STR.CHECK_VER);
            Thread.Sleep(1000);

            //마지막 최신버전 확인
            string LastVer = NetworkFunc.GetLastViewPortVersion(MYSQL_STR.CONNECTION_STEMCO);
            if (string.IsNullOrEmpty(LastVer)) { MessageBox.Show(MSG_STR.ERR_CHECK_VER); ExitSafe(); }

            if (LastVer != VersionInfo)
            {
                if (Update_ViewPort(LastVer))
                    EditLabelTextSafe(MSG_STR.UPDATE_VER);
                else
                {
                    MessageBox.Show(MSG_STR.ERR_UPDATE_VER);
                    return;
                }
            }

            Thread.Sleep(1000);
            EditLabelTextSafe(MSG_STR.CHECK_VER_OK);
            Thread.Sleep(1000);

            EditLabelTextSafe(MSG_STR.EXCUTE_READY);
            Thread.Sleep(1000);

            Start_ViewPort();
            ExitSafe();
        }
    }
}

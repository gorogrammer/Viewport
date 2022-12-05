using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace ViewPort.Views
{
    public partial class ProgressBar1 : Form
    {
        private static bool isCloseCall = false;
        public static int Count = 0;
        public void ShowBar()
        {
            
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            Control mainWindow = Control.FromHandle(process.MainWindowHandle);
            
            isCloseCall = false;
          //  Thread thread = new Thread(new ParameterizedThreadStart());
          //  thread.Start();
        }
        public static void CloseBar(Form form)
        {
            isCloseCall = true;

            SetForegroundWindow(form.Handle);
            form.BringToFront();
        }
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void ThreadShowWait()
        {
            
            ProgressBar1 progressBar = new ProgressBar1();
           
            progressBar.Show();
            progressBar.BringToFront();
            

        }
        private bool cannotClose = true;
        

        
        public ProgressBar1()
        {
            
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        
        public void CloseForce()
        {
            cannotClose = false;
            this.Close();
        }
        public void valuePlus()
        {
            progressBar2.Value++;
        }
        public void initBar()
        {
            progressBar2.Value = 0;
            progressBar2.BringToFront();
        }
        public void valueChange(int Plus)
        {
            progressBar2.BringToFront();
            while (progressBar2.Value != Plus)
            {
                progressBar2.Value = progressBar2.Value + 1;
                Thread.Sleep(1);
            }
        }
        public void MaxValue(int max)
        {
            progressBar2.BringToFront();
            progressBar2.Maximum = max;
        }
        protected override void OnLoad(EventArgs e)
        {
            //progressBar2.Maximum = Count;
            progressBar2.Minimum = 0;
            //progressBar2.Value = progressBar2.Value + 1;
            base.OnLoad(e);
        }
        private void OnFrameChanged()
        {
            this.Invalidate();
        }
        public void AddProgressBarValueSafe(int AddValue)
        {
            try
            {
                if (progressBar2.InvokeRequired)
                {
                    progressBar2.Invoke(new Action(() => progressBar2.Value += AddValue));
                }
                else
                {
                    progressBar2.Invoke(new Action(() => progressBar2.Value += AddValue));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void SetProgressBarValueSafe(int Value)
        {
            try
            {
                if (progressBar2.InvokeRequired)
                {
                    progressBar2.Invoke(new Action(() => progressBar2.Value = Value));
                }
                else
                {
                    progressBar2.Invoke(new Action(() => progressBar2.Value = Value));
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void SetProgressBarMaxSafe(int value)
        {
            if (progressBar2.InvokeRequired)
            {
                progressBar2.Invoke(new Action(() => progressBar2.Maximum = value));
            }
            else
            {
                progressBar2.Maximum = value;
            }
        }
        public void tabProgressBarSafe(int count)
        {
            for(int i=0; i< count; i++)
            {
                AddProgressBarValueSafe(1);
                Thread.Sleep(1);
            }
        }
        public void ExitProgressBarSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Close()));
            }
           
            else
            {
                this.Close();
            }
        }

    }
}

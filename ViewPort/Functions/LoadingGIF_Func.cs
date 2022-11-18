using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ViewPort.Models;
using ViewPort.Views;

namespace ViewPort.Functions
{
    public class LoadingGIF_Func
    {
        LoadingGIF wait;
        Thread loadthread;

        public void Show()
        {
            loadthread = new Thread(new ThreadStart(LoadingProcess));
            loadthread.Start();
        }

        public void Show(Form parent)
        {
            loadthread = new Thread(new ParameterizedThreadStart(LoadingProcess));
            loadthread.Start(parent);
        }

        public void Close()
        {
            if(wait != null)
            {
                if (wait.InvokeRequired)
                {
                    wait.BeginInvoke(new System.Threading.ThreadStart(wait.CloseWaitForm));
                    wait = null;
                    loadthread = null;
                }

            }
        }
        private void LoadingProcess()
        {
            wait = new LoadingGIF();
            wait.ShowDialog();
        }

        private void LoadingProcess(object parent)
        {
            Form parent1 = parent as Form;
            wait = new LoadingGIF(parent1);
            wait.ShowDialog();
        }
    }
}

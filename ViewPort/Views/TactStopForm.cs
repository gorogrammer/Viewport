using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
namespace ViewPort.Views
{
    public partial class TactStopForm : MetroForm
    {
        public int StopingTime = 0;
        bool Exit = false;
        public TactStopForm()
        {
            InitializeComponent();
            StopTime.Text = DateTime.Now.ToString("yyyy년MM월dd일 hh시mm분ss초");
            timer1.Start();
            WorkStart_BT.Enabled = false;
        }
        
        private void WorkStart_BT_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                timer1.Stop();
            DialogResult = DialogResult.OK;
            Exit = true;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NowTime.Text = DateTime.Now.ToString("yyyy년MM월dd일 hh시mm분ss초");
            StopingTime = StopingTime + 1;
            if(StopingTime >= 1)
            {
                WorkStart_BT.Enabled = true;
            }
        }

        private void TactStopForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Exit)
            {
                e.Cancel = true;
            }
        }
    }
}

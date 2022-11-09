using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Functions;

namespace ViewPort.Views
{
    public partial class ManagerForm : Form
    {
        ImageViewer ImageViewer;
        public ManagerForm(ImageViewer image)
        {
            InitializeComponent();
            ImageViewer = image;
        }

        private void NormalCheck_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void ShortCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ShortCheck.Checked)
                ImageViewer.Set_EngData(EQ_STR.SHORT);
        }

        private void 돌기Check_CheckedChanged(object sender, EventArgs e)
        {
            if (돌기Check.Checked)
                ImageViewer.Set_EngData(EQ_STR.SPIN);
        }

        private void OpenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenCheck.Checked)
                ImageViewer.Set_EngData(EQ_STR.OPEN);
        }

        private void MBCheck_CheckedChanged(object sender, EventArgs e)
        {
            if(MBCheck.Checked)
              ImageViewer.Set_EngData(EQ_STR.MB);
        }

        private void TopCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (TopCheck.Checked)
                ImageViewer.Set_EngData(EQ_STR.TOP);
        }

        private void 변색Check_CheckedChanged(object sender, EventArgs e)
        {
            if (변색Check.Checked)
                ImageViewer.Set_EngData(EQ_STR.DISCOLORATION);
        }

        private void SRCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (SRCheck.Checked)
                ImageViewer.Set_EngData(EQ_STR.SR);
        }
    }
}

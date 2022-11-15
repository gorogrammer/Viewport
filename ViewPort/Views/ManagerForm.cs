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
        int Checked = 0;
        public ManagerForm(ImageViewer image)
        {
            InitializeComponent();
            ImageViewer = image;
            this.Focus();
        }

        private void NormalCheck_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void ShortCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ShortCheck.Checked)
            {
                Checked++;
                if(Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.SHORT);
                }
                else
                ImageViewer.Set_EngData(EQ_STR.SHORT, "8", "5", "200", "200");
                
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.SHORT, Checked);
                Checked--;
            }
        }

        private void 돌기Check_CheckedChanged(object sender, EventArgs e)
        {
            if (돌기Check.Checked)
            {

                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.SPIN);
                }
                else
                ImageViewer.Set_EngData(EQ_STR.SPIN, "8", "5", "200", "200");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.SPIN, Checked);
                Checked--;
            }
        }

        private void OpenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenCheck.Checked)
            {
                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.OPEN);
                }
                else
                    ImageViewer.Set_EngData(EQ_STR.OPEN, "12", "7", "120", "120");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.OPEN,Checked);
                Checked--;
            }
        }

        private void MBCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (MBCheck.Checked)
            {
                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.MB);
                }
                else
                ImageViewer.Set_EngData(EQ_STR.MB, "12", "7", "120", "120");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.MB, Checked);
                Checked--;
            }
        }

        private void TopCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (TopCheck.Checked)
            {
                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.TOP);
                }
                else
                    ImageViewer.Set_EngData(EQ_STR.TOP, "12", "7", "100", "100");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.TOP, Checked);
                Checked--;
            }
        }

        private void 변색Check_CheckedChanged(object sender, EventArgs e)
        {
            if (변색Check.Checked)
            {
                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.DISCOLORATION);
                }
                else
                    ImageViewer.Set_EngData(EQ_STR.DISCOLORATION, "13", "8", "120", "120");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.DISCOLORATION, Checked);
                Checked--;
            }
        }

        private void SRCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (SRCheck.Checked)
            {
                Checked++;
                if (Checked > 1)
                {
                    ImageViewer.Set_MultiCheck_EngData(EQ_STR.SR);
                }
                else
                    ImageViewer.Set_EngData(EQ_STR.SR, "10", "6", "180", "180");
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.SR, Checked);
                Checked--;
            }
        }
    }
}

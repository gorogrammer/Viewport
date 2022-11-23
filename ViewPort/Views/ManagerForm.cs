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
using MetroFramework.Forms;

namespace ViewPort.Views
{
    public partial class ManagerForm : MetroForm
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
            if (NormalCheck.Checked)
            {
                ShortCheck.Checked = false;
                SRCheck.Checked = false;
                MBCheck.Checked = false;
                OpenCheck.Checked = false;
                돌기Check.Checked = false;
                변색Check.Checked = false;
                TopCheck.Checked = false;
              //  ImageViewer.Normal_Data();
                ImageViewer.Set_EngData(EQ_STR.DEFAULT, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
            }
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
                ImageViewer.Set_EngData(EQ_STR.SHORT,textBox20.Text, textBox19.Text,textBox4.Text,textBox3.Text);
                
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
                ImageViewer.Set_EngData(EQ_STR.SPIN, textBox22.Text, textBox21.Text, textBox6.Text, textBox5.Text);
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
                    ImageViewer.Set_EngData(EQ_STR.OPEN, textBox24.Text, textBox23.Text, textBox8.Text, textBox7.Text);
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
                ImageViewer.Set_EngData(EQ_STR.MB, textBox26.Text, textBox25.Text, textBox10.Text, textBox9.Text);
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
                    ImageViewer.Set_EngData(EQ_STR.TOP, textBox28.Text, textBox27.Text, textBox12.Text, textBox11.Text);
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
                    ImageViewer.Set_EngData(EQ_STR.DISCOLORATION, textBox30.Text, textBox29.Text, textBox14.Text, textBox13.Text);
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
                    ImageViewer.Set_EngData(EQ_STR.SR, textBox32.Text, textBox31.Text, textBox16.Text, textBox15.Text);
            }
            else
            {
                ImageViewer.Del_EngData(EQ_STR.SR, Checked);
                Checked--;
            }
        }
    }
}

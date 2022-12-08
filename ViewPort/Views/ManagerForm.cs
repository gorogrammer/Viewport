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
        //FormViewPort Main;
        int Checked = 0;
        bool Reset = false;
        public ManagerForm(ImageViewer image)
        {
            InitializeComponent();
            
            ImageViewer = image;
            ImageViewer.Manager = this;
            ImageViewer.Main.button1_Click(null, null);
            this.Focus();
        }

        private void NormalCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (NormalCheck.Checked)
            {
                ImageViewer.Main.Eng_dicinfo.Clear();
                Reset = true;
                Checked = 0;
                ShortCheck.Checked = false;
               // ShortCheck.Checked = false;
                SRCheck.Checked = false;
                MBCheck.Checked = false;
                OpenCheck.Checked = false;
                돌기Check.Checked = false;
                변색Check.Checked = false;
                TopCheck.Checked = false;
              //  ImageViewer.Normal_Data();
                ImageViewer.Set_EngData(EQ_STR.DEFAULT, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
            }
            else
            {
                if (Checked < 1)
                {
                    Checked = 0;
                }
                //MessageBox.Show("Default값은 Normal 입니다.");
                //ImageViewer.Del_EngData(EQ_STR.DEFAULT, 1);
            }
            Reset = false;
            this.Activate();
        }

        private void ShortCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (ShortCheck.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.SHORT, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.SHORT, textBox20.Text, textBox19.Text, textBox4.Text, textBox3.Text);

                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.SHORT, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }

                this.Activate();
            }
        }

        private void 돌기Check_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (돌기Check.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.SPIN, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.SPIN, textBox22.Text, textBox21.Text, textBox6.Text, textBox5.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.SPIN, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void OpenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {

                if (OpenCheck.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.OPEN, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.OPEN, textBox24.Text, textBox23.Text, textBox8.Text, textBox7.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.OPEN, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void MBCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (MBCheck.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.MB, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.MB, textBox26.Text, textBox25.Text, textBox10.Text, textBox9.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.MB, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void TopCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (TopCheck.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.TOP, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.TOP, textBox28.Text, textBox27.Text, textBox12.Text, textBox11.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.TOP, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void 변색Check_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (변색Check.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.DISCOLORATION, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.DISCOLORATION, textBox30.Text, textBox29.Text, textBox14.Text, textBox13.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.DISCOLORATION, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void SRCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!Reset)
            {
                if (SRCheck.Checked)
                {
                    if (NormalCheck.Checked)
                        NormalCheck.Checked = false;
                    Checked++;
                    if (Checked > 1)
                    {
                        ImageViewer.Set_MultiCheck_EngData(EQ_STR.SR, textBox18.Text, textBox17.Text, textBox1.Text, textBox2.Text);
                    }
                    else
                        ImageViewer.Set_EngData(EQ_STR.SR, textBox32.Text, textBox31.Text, textBox16.Text, textBox15.Text);
                }
                else
                {
                    ImageViewer.Del_EngData(EQ_STR.SR, Checked);
                    Checked--;
                    if (Checked == 0)
                        NormalCheck.Checked = true;
                }
                this.Activate();
            }
        }

        private void ManagerForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.Activate();
        }

        private void ShortCheck_Click(object sender, EventArgs e)
        {
            
        }

        private void ShortCheck_MouseUp(object sender, MouseEventArgs e)
        {
            
        }
        public string Del_Single_Check()
        {
            string Data=string.Empty;
            if (ShortCheck.Checked)
            {
                Data = EQ_STR.SHORT +"," +textBox20.Text+"," + textBox19.Text+"," + textBox4.Text+"," + textBox3.Text;

                return Data;
            }
            else if (돌기Check.Checked)
            {
                Data = EQ_STR.SPIN + "," + textBox22.Text + "," + textBox21.Text + "," + textBox6.Text + "," + textBox5.Text;

                return Data;
            }
            else if (OpenCheck.Checked)
            {
                Data = EQ_STR.OPEN + "," + textBox24.Text + "," + textBox23.Text + "," + textBox8.Text + "," + textBox7.Text;

                return Data;
            }
            else if (MBCheck.Checked)
            {
                Data = EQ_STR.MB + "," + textBox26.Text + "," + textBox25.Text + "," + textBox10.Text + "," + textBox9.Text;

                return Data;
            }
            else if (TopCheck.Checked)
            {
                Data = EQ_STR.TOP + "," + textBox28.Text + "," + textBox27.Text + "," + textBox12.Text + "," + textBox11.Text;

                return Data;
            }
            else if (변색Check.Checked)
            {
                Data = EQ_STR.DISCOLORATION + "," + textBox30.Text + "," + textBox29.Text + "," + textBox14.Text + "," + textBox13.Text;

                return Data;
            }
            else if (SRCheck.Checked)
            {
                Data = EQ_STR.DISCOLORATION + "," + textBox32.Text + "," + textBox31.Text + "," + textBox16.Text + "," + textBox15.Text;

                return Data;
            }
            return Data;
        }

        private void ManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ImageViewer.Main.EngrMode = false;
            ImageViewer.Set_EngData(EQ_STR.DEFAULT, "12","7", "120", "120");
            ImageViewer.Main.Eng_dicinfo.Clear();
        }
    }
}

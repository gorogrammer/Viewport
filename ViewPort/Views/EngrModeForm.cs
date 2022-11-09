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
    public partial class EngrModeForm : Form
    {
        string Form_Str;
        FormViewPort Main;
        ManagerForm managerForm;
        ImageViewer ImageViewer;
        public EngrModeForm(FormViewPort MainForm,ImageViewer image,string Form)
        {
           
            InitializeComponent();
            Main = MainForm;
            ImageViewer = image;
            Form_Str = Form;
            if (Main.EngrMode)
                EngState.Text = "ON";
            else if (!Main.EngrMode)
                EngState.Text = "OFF";
            
        }

        private void EngStateBT_Click(object sender, EventArgs e)
        {
            if (Form_Str == FORM_STR.ViewPort)
            {
                if (EngPW.Text == Main.EngrModePW)
                {
                    if (Main.EngrMode)
                    {
                        Main.EngrMode = false;
                        EngState.Text = "OFF";
                        if (managerForm != null)
                            managerForm.Close();
                    }
                    else if (!Main.EngrMode)
                    {
                        Main.EngrMode = true;
                        EngState.Text = "ON";
                        managerForm = new ManagerForm(ImageViewer);
                        managerForm.Show();
                    }
                }
            }
            else if(Form_Str == FORM_STR.DLForm)
            {
                if (EngPW.Text == Main.EngrModePW)
                {
                    if (Main.EngrMode)
                    {
                        Main.EngrMode = false;
                        EngState.Text = "OFF";
                        if (managerForm != null)
                            managerForm.Close();
                    }
                    else if (!Main.EngrMode)
                    {
                        Main.EngrMode = true;
                        EngState.Text = "ON";
                        managerForm = new ManagerForm(ImageViewer);
                        managerForm.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Password Error");
            }
        }
    }
}

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
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ViewPort.Views
{
    public partial class EngrModeForm : MetroForm
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        FormViewPort Main;
        ManagerForm managerForm;
        ImageViewer ImageViewer;
        DL_Eng_Form dL_Eng_Form;
        DB_ViewForm _ViewForm;
        public string FilePath;
        string engModeCheck = string.Empty;

        public string EngModeCheck { get => engModeCheck; set => engModeCheck = value; }

        public EngrModeForm(FormViewPort MainForm,ImageViewer image)
        {
           
            InitializeComponent();
            Main = MainForm;
            ImageViewer = image;
            //Form_Str = Form;
            if (Main.EngrMode)
                EngState.Text = "ON";
            else if (!Main.EngrMode)
                EngState.Text = "OFF";
            this.Focus();
            
        }

        private void EngStateBT_Click(object sender, EventArgs e)
        {
            if (EngModeCheck == FORM_STR.ViewPort)
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
                        this.Close();
                    }
                }
            }
            else if(EngModeCheck == FORM_STR.DLForm)
            {
                if (EngPW.Text == Main.EngrModePW)
                {
                    if (Main.EngrMode)
                    {
                        Main.EngrMode = false;
                        EngState.Text = "OFF";
                        if (dL_Eng_Form != null)
                            dL_Eng_Form.Close();
                    }
                    else if (!Main.EngrMode)
                    {
                        Main.EngrMode = true;
                        EngState.Text = "ON";
                        dL_Eng_Form = new DL_Eng_Form(ImageViewer,Main.DI_List_Sever);
                        dL_Eng_Form.LotName = Main.LotName;
                        List<string> dl = new List<string>();
                        DBFunc dB = new DBFunc();
                        dB.LimitSetting(dl);
                        dL_Eng_Form.Dl_LIst_ADD(dl);                   
                        dL_Eng_Form.Show();
                        this.Close();
                    }
                }
            }
            else if(engModeCheck == FORM_STR.DBForm)
            {
                if (EngPW.Text == Main.EngrModePW)
                {
                    if (Main.EngrMode)
                    {
                        Main.EngrMode = false;
                        EngState.Text = "OFF";
                        if (dL_Eng_Form != null)
                            dL_Eng_Form.Close();
                    }
                    else if (!Main.EngrMode)
                    {
                        Main.EngrMode = true;
                        EngState.Text = "ON";
                        _ViewForm = new DB_ViewForm(ImageViewer,FilePath);                       
                       // List<string> dl = new List<string>();
                        //DBFunc dB = new DBFunc();
                        //dB.LimitSetting(dl);
                        _ViewForm.ShowDialog();
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Password Error");
            }
        }

        private void EngrModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void EngrModeForm_Load(object sender, EventArgs e)
        {
           // ShowWindow(this.Handle, 1);
        }

        private void EngPW_Click(object sender, EventArgs e)
        {
            EngPW.Text = "";
        }
    }
}

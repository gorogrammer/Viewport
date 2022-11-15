﻿using System;
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
    public partial class EngrModeForm : MetroForm
    {
        FormViewPort Main;
        ManagerForm managerForm;
        ImageViewer ImageViewer;
        DL_Eng_Form dL_Eng_Form;
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
                        dL_Eng_Form.Dl_LIst_ADD(Main.DI_List_Sever);                   
                        dL_Eng_Form.ShowDialog();
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
            Main.EngrMode = false;
        }
    }
}

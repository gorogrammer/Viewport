using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using MetroFramework.Forms;

using SDIP;
using ViewPort.Functions;
//using SDIP.User;

namespace SDIP.Forms
{
    public partial class FormLogin : MetroForm
    {
        public string information = string.Empty;
        public UseInfomation UseInfomation = new UseInfomation(); 

        //public UseInfomation UseInfomation = { useInfomation  }
        ViewPort.Functions.DBFunc db = new ViewPort.Functions.DBFunc();
        public FormLogin()
        {
            InitializeComponent();
            
            this.Text += " " + Assembly.GetExecutingAssembly().GetName().Version;
            TB_ID.Focus();
            TB_PASSWORD.PasswordChar = '*';
            this.FocusMe();
            //SDIPUser user = new SDIPUser();
        }

        private void LogIn()
        {


            if (db.DBConnection(TB_ID.Text, TB_PASSWORD.Text))
            {
                UseInfomation.Authorization = db.Authorization;
                if (db.Authorization == Enums.PERMISSION.None.ToString())
                {
                    MessageBox.Show("관리자에게 승인요청 중 입니다.");
                    return;
                }
                UseInfomation.Name = db.Information;
                UseInfomation.OffLineMode = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                
                    MessageBox.Show("로그인실패");
            }
        }

        private void TB_PASSWORD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LogIn();
        }

        private void BTN_LOGIN_Click(object sender, EventArgs e)
        {
            LogIn();
        }

        private void BTN_등록_Click(object sender, EventArgs e)
        {
            ViewPort.Views.RegForm regForm = new ViewPort.Views.RegForm();

            regForm.ShowDialog();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            UseInfomation.OffLineMode = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

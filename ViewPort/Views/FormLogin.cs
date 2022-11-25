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
//using SDIP.User;

namespace SDIP.Forms
{
    public partial class FormLogin : MetroForm
    {
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


            if (db.DBConnection(Int32.Parse(TB_ID.Text), TB_PASSWORD.Text))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("로그인실패");
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
    }
}

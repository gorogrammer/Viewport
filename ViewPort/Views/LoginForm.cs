using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewPort.Views
{
    public partial class LoginForm : Form
    {
        Functions.DBFunc db = new Functions.DBFunc();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginBT_Click(object sender, EventArgs e)
        {
            
            
            if(db.DBConnection(Int32.Parse(textBox1.Text),textBox2.Text))
                    MessageBox.Show("로그인성공");

            this.Close();

        }

        private void RegBT_Click(object sender, EventArgs e)
        {
            RegForm regForm = new RegForm();
            regForm.ShowDialog();

        }
    }
}

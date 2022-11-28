using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace ViewPort.Views
{
    public partial class RegForm : MetroForm
    {
        Functions.DBFunc db = new Functions.DBFunc();
        public RegForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void RegBT_Click(object sender, EventArgs e)
        {
           if(db.DBRegister( textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("등록완료\n 관리자가 승인 후 로그인 가능합니다.");

                this.Close();
            }
                
                    


        }
    }
}

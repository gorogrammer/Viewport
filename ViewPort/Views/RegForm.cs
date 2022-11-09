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
    public partial class RegForm : Form
    {
        Functions.DBFunc db = new Functions.DBFunc();
        public RegForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void RegBT_Click(object sender, EventArgs e)
        {
           if(db.DBRegister(Int32.Parse(textBox1.Text), textBox3.Text, textBox2.Text))
            {
                MessageBox.Show("등록완료");

                this.Close();
            }
                
                    


        }
    }
}

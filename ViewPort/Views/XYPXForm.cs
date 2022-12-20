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
    public partial class XYPXForm : MetroForm
    {
        public int XYPX=0;
        public XYPXForm()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(metroTextBox1.Text, out XYPX))
                MessageBox.Show("숫자를 입력해주세요");
            else
            {
                XYPX = int.Parse(metroTextBox1.Text);
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

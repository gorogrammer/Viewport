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
    public partial class ComentsForm : MetroForm
    {
        public string Coment = string.Empty;
        public ComentsForm()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Coment = richTextBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ComentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}

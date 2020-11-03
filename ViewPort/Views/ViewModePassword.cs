using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewPort.Models;
using System.IO.Compression;
using System.IO;

namespace ViewPort.Views
{
    public partial class ViewModePassword : Form
    {
        ImageViewer  open;
        public ViewModePassword(ImageViewer parent)
        {
            InitializeComponent();
            open = parent;
        }

        public void Check_PSW()
        {
            if(this.PSW_Input_TB.Text == "1234")
            {
                this.Close();
               if(open.Shift_del == 1)
               {
                    open.Key_shift_del();
               }
               else
               {
                    open.Key_only_del();
               }
                
                open.ViewMode_PSW_Check = 1;
            }
            else
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.", "오류", MessageBoxButtons.OK);
                this.Close();
            }
           
        }

        private void PSW_Check_BT_Click(object sender, EventArgs e)
        {
            Check_PSW();
        }
    }
}

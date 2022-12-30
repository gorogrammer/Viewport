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
using DevExpress.XtraEditors;

namespace ViewPort.Views
{
    public partial class RegOn : XtraForm
    {
        public DBFunc dBFunc = new DBFunc();
        public int index = -1;
        public RegOn()
        {
            InitializeComponent();
            dataGridView1.DataSource = dBFunc.GetRegUser();
        }

        private void metroButton1_Click(object sender, EventArgs e) //승인
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                
                    string name = row.Cells[0].Value.ToString();
                    if (dBFunc.UplaodUser(Enums.PERMISSION.user.ToString(), name))
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                
            }
            else
            {
                MessageBox.Show("선택된 데이터가 없습니다.");
            }
        }

        private void metroButton2_Click(object sender, EventArgs e) //전체승인
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("데이터가 존재하지 않습니다.");
                return;
            }
            foreach (DataGridViewRow dataRow in dataGridView1.Rows)
            {
                string name = dataRow.Cells[0].Value.ToString();
                dBFunc.UplaodUser(Enums.PERMISSION.user.ToString(), name);
            }
            ((DataTable)dataGridView1.DataSource).Rows.Clear();
        }

        private void metroButton3_Click(object sender, EventArgs e) //삭제
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}

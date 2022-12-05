using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using MetroFramework.Forms;
using ViewPort.Functions;

namespace ViewPort.Views
{
    public partial class DB_ViewForm : MetroForm
    {
        ImageViewer Image;
        public string beforData;
        public string FilePath;
        public DB_ViewForm(ImageViewer image ,string filePath)
        {
            Image = image;
            InitializeComponent();
            FilePath = filePath;
            GetData();
            if (tabControl1.SelectedTab.Text == "DeletePath")
            {
                button5.Enabled = true;
                button4.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
                button4.Enabled = false;
                button2.Enabled = false;

            }
            
            //  gridView1.BestFitColumns();

        }
        private void GetData()
        {
            DBFunc dBFunc = new DBFunc();
            gridControl2.DataSource = dBFunc.GetDeletePath();
            gridControl1.DataSource = dBFunc.GetLog();
            LotGrid.DataSource = dBFunc.GetLot();
            UserGrid.DataSource = dBFunc.GetUser();
            gridControl3.DataSource = Func.Get_Lot_WorkerList(FilePath);
            gridView2.BestFitColumns();
            gridView3.BestFitColumns();
            gridView5.BestFitColumns();
            gridView4.BestFitColumns();

            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "DeletePath")
            {
                DeletePath deletePath = new DeletePath();
                 PropertyForm propertyForm = new PropertyForm(deletePath);
                propertyForm.ShowDialog();

                if(propertyForm.DialogResult == DialogResult.OK)
                {
                    DBFunc dBFunc = new DBFunc();
                    gridControl2.DataSource = dBFunc.GetDeletePath();
                }
            }
            else
            {
                MessageBox.Show("조회만 가능한 테이블입니다.");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = null;
            if(tabControl1.SelectedTab.Text == "DeletePath")
            {
                button5.Enabled = true;
                button4.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
                button4.Enabled = false;
                button2.Enabled = false;

            }
        }

        private void gridView5_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DeletePath Path = new DeletePath();

            Path.PathName = (string)gridView5.GetDataRow(e.RowHandle).ItemArray[0];
            Path.MachineType = (string)gridView5.GetDataRow(e.RowHandle).ItemArray[1];
            Path.WorkType = (Enums.WORKTYPE)gridView5.GetDataRow(e.RowHandle).ItemArray[2];
            Path.Path = (string)gridView5.GetDataRow(e.RowHandle).ItemArray[3];
            beforData = (string)gridView5.GetDataRow(e.RowHandle).ItemArray[0];
            propertyGrid1.SelectedObject = Path;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBFunc dB = new DBFunc();
            object obj = propertyGrid1.SelectedObject;
            List<string> Column = new List<string>();
            List<string> value = new List<string>();
            Column = Enum.GetNames(typeof(Enums.DELETEPATHCOL)).ToList();
            foreach (string col in Column)
            {
                value.Add(obj.GetType().GetProperty(col).GetValue(obj, null).ToString());
            }
            if (dB.UplaodDeletePath(value,beforData))
            {
                gridControl2.DataSource = dB.GetDeletePath();
            };
        }

        private void DB_ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Image.Main.EngrMode = false;
        }
    }
}

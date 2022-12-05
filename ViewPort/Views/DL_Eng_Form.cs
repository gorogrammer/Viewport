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
    public partial class DL_Eng_Form : MetroForm
    {
        List<string> Dl_Data = new List<string>();
        public string LotName = string.Empty;
        ImageViewer image;
        public DL_Eng_Form(ImageViewer imageViewer,List<string> dl)
        {
            
            InitializeComponent();
            Dl_Data = dl;
            image = imageViewer;
        }
        public void Dl_LIst_ADD(List<string> Dl_Data)
        {
           
            
            //textBox1.Text = string.Empty;

            if (Dl_Data.Count > 0)
            {
               

                DataTable dt = new DataTable();
                dt.Columns.Add("SDIP Code");
                dt.Columns.Add("SDIP 불량명");
                dt.Columns.Add("Limit불량기준(%)");
                dt.Columns.Add("Alram내용");


                foreach (string dl in Dl_Data)
                {
                    string[] dl_Split = dl.Split(',');
                    string[] Code = dl_Split[0].Split('.');
                    dt.Rows.Add(Code[0], Code[1], dl_Split[1], dl_Split[2]);

                    dataGridView1.DataSource = dt;
                }
            }
            else
            {

            }



        }

        private void button1_Click(object sender, EventArgs e)   //Save 
        {
            
            List<string> UpdateDL = new List<string>();
            DataTable dt = (DataTable)dataGridView1.DataSource;
            foreach(DataRow data in dt.Rows)
            {
               
               UpdateDL.Add(data.ItemArray[0].ToString() + "," + data.ItemArray[1].ToString() + "," + data.ItemArray[2].ToString() + "," +data.ItemArray[3].ToString());
            }
            Functions.DBFunc dBFunc = new Functions.DBFunc();
            if(dBFunc.DB_DL_UpDate(UpdateDL, LotName))
                MessageBox.Show("Update 완료");
            else
                MessageBox.Show("Update 실패");
            

        }

        private void DL_Eng_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            image.Main.EngrMode = false;
        }
    }
}

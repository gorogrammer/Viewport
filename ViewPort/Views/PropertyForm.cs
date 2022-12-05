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

namespace ViewPort.Views
{
    public partial class PropertyForm : Form
    {
        public PropertyForm(object data)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = data;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            DBFunc dB = new DBFunc();
            object obj = propertyGrid1.SelectedObject;
            List<string> Column = new List<string>();
            List<string> value = new List<string>();
            Column = Enum.GetNames(typeof(Enums.DELETEPATHCOL)).ToList();
            foreach(string col in Column)
            {
                value.Add(obj.GetType().GetProperty(col).GetValue(obj, null).ToString());
            }
            if (dB.InsertDeletePath(value)) 
            {
                DialogResult = DialogResult.OK;
                this.Close();
            };
        }
    }
}

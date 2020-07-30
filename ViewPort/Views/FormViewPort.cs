using MySql.Data.MySqlClient;
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
using ViewPortNetwork;

namespace ViewPort
{
    public partial class FormViewPort : Form
    {
        public FormViewPort()
        {
            InitializeComponent();
        }

        void InitNetworkService()
        {
            using (MySqlConnection conn = new MySqlConnection(MYSQL_STR.CONNECTION_CARLO))
            {
            }
        }
    }
}

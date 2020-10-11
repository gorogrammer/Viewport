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
    public partial class Code200View : Form
    {
        FormViewPort Main;
        List<string> selected_Code_200_List = new List<string>();
        Dictionary<string, ImageInfo> filter_200_dic = new Dictionary<string, ImageInfo>();

        public Dictionary<string, ImageInfo> Filter_200_dic { get => filter_200_dic; set => filter_200_dic = value; }
        public List<string> Selected_Code_200_List { get => selected_Code_200_List; set => selected_Code_200_List = value; }
        public Code200View(FormViewPort parent)
        {
            InitializeComponent();
            Main = parent;

            for (int i = 0; i < Main.CODE_200_List.Count; i++)
                Code_200_CLB.Items.Add(Main.CODE_200_List.ElementAt(i).Item1 + "-" + Main.Sdip_result_dic[Main.CODE_200_List.ElementAt(i).Item1] + "-" + Main.CODE_200_List.ElementAt(i).Item2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Filter_200_dic.Clear();
            Initial_Code_200_CLB_FilterList();
            Main.Filter_200_dic_Main = Filter_200_dic;

            Main.Code_200_Filter();
            
            this.Close();
        }

        private void Code_200_CLB_SelectedValueChanged(object sender, EventArgs e)
        {
            Selected_Code_200_List.Clear();

            foreach (int index in Code_200_CLB.CheckedIndices)
                Selected_Code_200_List.Add(Code_200_CLB.Items[index].ToString().Split('-')[0]);
        }

        private void Initial_Code_200_CLB_FilterList()
        {
            for (int i = 0; i < Selected_Code_200_List.Count; i++)
            {
                foreach (KeyValuePair<string, ImageInfo> pair in Main.Sdip_200_code_dicInfo)
                {

                    if (pair.Value.sdip_no == Selected_Code_200_List[i])
                    {
                        Filter_200_dic.Add(pair.Key, pair.Value);
                    }

                }

            }
        }
    }
}

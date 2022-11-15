using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace ViewPort.Views
{
    public partial class AlramForm : MetroForm
    {
        public AlramForm()
        {
            InitializeComponent();
           // richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        public void AlarmText(List<string> Text)
        {
            foreach(string Limit in Text)
            {
                string[] LimitSplit = Limit.Split(':');
                richTextBox1.AppendText(Limit + "\n");
                Regex regex = new Regex(LimitSplit[1]);
                MatchCollection mc = regex.Matches(richTextBox1.Text);
                int iCursorPosition = richTextBox1.SelectionStart;
                foreach (Match m in mc)

                {

                    int iStartIdx = m.Index;

                    int iStopIdx = m.Length;



                    richTextBox1.Select(iStartIdx, iStopIdx);

                    richTextBox1.SelectionColor = Color.Red;
                    
                    richTextBox1.SelectionStart = iCursorPosition;

                    richTextBox1.SelectionColor = Color.Black;

                }
            }
        }
    }
}

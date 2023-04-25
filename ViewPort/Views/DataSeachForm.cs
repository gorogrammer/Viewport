using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ViewPort.Models;
using DevExpress.XtraCharts;

namespace ViewPort.Views
{
    public partial class DataSeachForm : DevExpress.XtraEditors.XtraForm
    {
        public DataTable LOT, LOG, User,SearchData;
        Dictionary<string, UserInfo> UserList = new Dictionary<string, UserInfo>();

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e) //조회
        {
            try
            {
                chartControl1.Series.Clear();
                chartControl2.Series.Clear();
                chartControl3.Series.Clear();
                chartControl4.Series.Clear();
                chartControl5.Series.Clear();
                chartControl6.Series.Clear();
                UserList.Clear();
                SearchData = new DataTable();
                SearchData.Columns.Add("사원번호");
                SearchData.Columns.Add("Lot처리량");
                SearchData.Columns.Add("근무일수");
                SearchData.Columns.Add("유휴시간");
                SearchData.Columns.Add("장당처리속도(h)");
                SearchData.Columns.Add("양품처리 이미지 수");
                SearchData.Columns.Add("View 이미지 수");

                DateTime StartTime = S_DateTime.Value.AddHours(8);
                DateTime EndTime = E_DateTime.Value.AddHours(8);
                Dictionary<string, List<string>> overlap = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> Days = new Dictionary<string, List<string>>();
                foreach (DataRow row in User.Rows)
                {
                    UserList.Add(row.ItemArray[0].ToString(), new UserInfo(0, 0, 0, 0, 0, 0));
                    overlap.Add(row.ItemArray[0].ToString(), new List<string>());
                    Days.Add(row.ItemArray[0].ToString(), new List<string>());
                }


                foreach (DataRow row in LOG.Rows)
                {
                    string Lot = row.ItemArray[0].ToString();
                    bool Finally = Convert.ToBoolean(int.Parse(row.ItemArray[6].ToString()));
                    string Worker = row.ItemArray[1].ToString();
                    int Idle = int.Parse(row.ItemArray[5].ToString());
                    // int Speed = double.Parse(row.ItemArray[4].ToString());
                    DateTime finalTime = new DateTime();
                    if (DateTime.TryParse(row.ItemArray[3].ToString(), out finalTime))
                    {
                        // finalTime = DateTime.TryParse(row.ItemArray[3].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Error");
                        return;
                    }
                    if (Finally && StartTime < finalTime && EndTime > finalTime)
                    {
                        if (!overlap[Worker].Contains(Lot))
                        {
                            UserList[Worker].SuLot = UserList[Worker].SuLot + 1;
                            UserList[Worker].IdleTime = UserList[Worker].IdleTime + Idle;
                            if (!Days[Worker].Contains(finalTime.Date.ToString("MM/dd")))
                                Days[Worker].Add(finalTime.Date.ToString("MM/dd"));
                            overlap[Worker].Add(Lot);
                        }
                    }





                }
                foreach (string Worker in UserList.Keys)
                {
                    foreach (DataRow row in LOT.Rows)
                    {

                        if (overlap[Worker].Contains(row.ItemArray[0].ToString()))
                        {
                            int LotImageCnt = int.Parse(row.ItemArray[1].ToString());
                            int WorkImageCnt = int.Parse(row.ItemArray[2].ToString());

                            UserList[Worker].ViewImage = UserList[Worker].ViewImage + LotImageCnt;
                            UserList[Worker].PassImage = UserList[Worker].PassImage + (LotImageCnt - WorkImageCnt);



                        }

                    }
                    UserList[Worker].WorkTime = Days[Worker].Count;
                    if (UserList[Worker].WorkTime != 0)
                        UserList[Worker].ImageSpeed = UserList[Worker].ViewImage / (UserList[Worker].WorkTime * 8);

                    if(UserList[Worker].WorkTime !=0)
                    SearchData.Rows.Add(Worker, UserList[Worker].SuLot, UserList[Worker].WorkTime, UserList[Worker].IdleTime, UserList[Worker].ImageSpeed, UserList[Worker].PassImage, UserList[Worker].ViewImage);
                }


                dataGridView1.DataSource = SearchData;
                GraphData();
            }
            catch
            {
                MessageBox.Show("Data Error");
            }
        }


        public DataSeachForm()
        {
            InitializeComponent();
            string S_Dtime = DateTime.Now.ToString("yyyy/MM/dd");
            string E_Dtime = DateTime.Now.AddDays(+1).ToString("yyyy/MM/dd");
            S_DateTime.Value = Convert.ToDateTime(S_Dtime);
            E_DateTime.Value = Convert.ToDateTime(E_Dtime);
        }
        private void GraphData()
        {
           // ChartSetting();
            Series[] Series = new Series[SearchData.Columns.Count -1];
            for(int i=1; i< SearchData.Columns.Count; i++)
            {
                Series[i -1] = SeriesSetting(SearchData.Columns[i].ColumnName);
            }
            foreach (KeyValuePair<string, UserInfo> keyValuePair in UserList)
            {
               
                    Series[0].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.SuLot));
                    Series[1].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.WorkTime));
                    Series[2].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.IdleTime));
                    Series[3].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.ImageSpeed));
                    Series[4].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.PassImage));
                    Series[5].Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value.ViewImage));

                

            }
            chartControl1.Series.Add(Series[0]);
            chartControl2.Series.Add(Series[1]);
            chartControl4.Series.Add(Series[2]);
            chartControl3.Series.Add(Series[3]);
            chartControl5.Series.Add(Series[4]);
            chartControl6.Series.Add(Series[5]);
            ChartSetting();
        }
        public void ChartSetting()
        {
            
            ((XYDiagram)chartControl1.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl1.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl1.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl1.Diagram).EnableAxisYScrolling = true;

            ((XYDiagram)chartControl2.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl2.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl2.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl2.Diagram).EnableAxisYScrolling = true;

            ((XYDiagram)chartControl3.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl3.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl3.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl3.Diagram).EnableAxisYScrolling = true;

            ((XYDiagram)chartControl4.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl4.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl4.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl4.Diagram).EnableAxisYScrolling = true;

            ((XYDiagram)chartControl5.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl5.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl5.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl5.Diagram).EnableAxisYScrolling = true;

            ((XYDiagram)chartControl6.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)chartControl6.Diagram).EnableAxisYZooming = true;
            ((XYDiagram)chartControl6.Diagram).EnableAxisXScrolling = true;
            ((XYDiagram)chartControl6.Diagram).EnableAxisYScrolling = true;

            

            chartControl1.Series[0].SeriesPointsSorting = SortingMode.Ascending;
            chartControl2.Series[0].SeriesPointsSorting = SortingMode.Ascending;
            chartControl3.Series[0].SeriesPointsSorting = SortingMode.Ascending;
            chartControl4.Series[0].SeriesPointsSorting = SortingMode.Ascending;
            chartControl5.Series[0].SeriesPointsSorting = SortingMode.Ascending;
            chartControl6.Series[0].SeriesPointsSorting = SortingMode.Ascending;

            
        }
        public Series SeriesSetting(string ChartName)
        {
            Series LotChart_1 = new Series(ChartName, ViewType.Bar);
            LotChart_1.ArgumentScaleType = ScaleType.Qualitative;
            LotChart_1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((BarSeriesView)LotChart_1.View).ColorEach = true;

            // LotChart_1.SeriesPointsSorting = SortingMode.Ascending;
           

            return LotChart_1;
        }
        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
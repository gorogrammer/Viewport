using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using ViewPort.Functions;
using DevExpress.XtraGrid;
using DevExpress.XtraCharts;

namespace ViewPort.Views
{
    public partial class DB_ViewForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DBFunc dBFunc = new DBFunc();
        ImageViewer Image;
        DataTable LOG, LOT, DeletePath, User,Worker;
        string beforData= string.Empty;
        string FilePath= string.Empty;
        string ComentLog=string.Empty;
        int NoneUser = 0;
        ChartControl chartControl = new ChartControl();
        
        public DB_ViewForm(ImageViewer image, string filePath)
        {
            Image = image;
            InitializeComponent();
            accordionControl1.SelectedElement = accordionControl1.Elements[0];
            FilePath = filePath;
            GetData();
            accordionControl1.SelectElement(accordionControl1.Elements.Element);
            if (NoneUser > 0)
                RegBT.Caption = "가입승인요청목록 ( " + NoneUser.ToString() + " ) ";
            else { }
            chartControl.MouseClick += ChartControl_MouseClick;
            chartControl.RuntimeHitTesting = true;
            //  gridView1.BestFitColumns();

        }

        private void ChartControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (accordionControl1.SelectedElement.Text == "C_Lot")
            {
                ChartHitInfo chartHit = chartControl.CalcHitInfo(e.X, e.Y);
                List<string> User = new List<string>();
                SeriesPoint point = chartHit.SeriesPoint;
                List<string> LotData = new List<string>();
                if (point != null)
                {
                    foreach (DataRow row in LOG.Rows)
                    {
                        LotData.Add(row.ItemArray[0].ToString() + "," + row.ItemArray[1].ToString() + "");

                    }
                    LotData = LotData.Distinct().ToList();
                    foreach (string str in LotData)
                    {
                        string[] value = str.Split(',');

                        if (value[0].Equals(point.Argument))
                        {
                            User.Add(value[1]);
                        }

                    }
                    if (User.Count == 0)
                    {
                        MessageBox.Show("해당 Lot의 작업자가 없습니다.");
                    }
                    else
                    {
                        DonutForm donutForm = new DonutForm(User);
                        donutForm.ShowDialog();
                    }
                }
                else
                {

                    // 빈 곳을 클릭했을 때 처리 루틴...

                    // MessageBox.Show("Clicked Empty space");

                }
            }
        }

        private void GetData()
        {

            DeletePath = dBFunc.GetDeletePath();
            LOG = dBFunc.GetLog();
            LOT = dBFunc.GetLot();
            User = dBFunc.GetUser();
            // gridControl3.DataSource = Func.Get_Lot_WorkerList(FilePath);
            gridView3.BestFitColumns();
            NoneUser = dBFunc.GetNoneUser();



        }

        #region Click
        private void LogMenu_Click(object sender, EventArgs e)
        {

            LotGrid.DataSource = null;
            gridView3.Columns.Clear();
            LotGrid.DataSource = LOG;
        }

        private void LotMenu_Click(object sender, EventArgs e)
        {
            LotGrid.DataSource = null;
            gridView3.Columns.Clear();
            LotGrid.DataSource = LOT;
        }

        private void DeletePathMenu_Click(object sender, EventArgs e)
        {
            LotGrid.DataSource = null;
            gridView3.Columns.Clear();
            LotGrid.DataSource = DeletePath;

        }

        private void UserMenu_Click(object sender, EventArgs e)
        {
            LotGrid.DataSource = null;
            gridView3.Columns.Clear();
            LotGrid.DataSource = User;
        }

        private void DB_ViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Image.Main.EngrMode = false;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }



        private void button5_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(accordionControl1.SelectedElement == null)
            {
                return;
            }
            if (accordionControl1.SelectedElement.Text == "DeletePath")
            {
                DeletePath deletePath = new DeletePath();
                PropertyForm propertyForm = new PropertyForm(deletePath);
                propertyForm.ShowDialog();

                if (propertyForm.DialogResult == DialogResult.OK)
                {
                    DBFunc dBFunc = new DBFunc();
                    DeletePath = dBFunc.GetDeletePath();
                    DeletePathMenu_Click(null, null);
                }
            }
            else
            {
                MessageBox.Show("조회만 가능한 테이블입니다.");
            }
        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (accordionControl1.SelectedElement.Text == "DeletePath")
            {
                DeletePath Path = new DeletePath();

                Path.PathName = (string)gridView3.GetDataRow(e.RowHandle).ItemArray[0];
                Path.MachineType = (string)gridView3.GetDataRow(e.RowHandle).ItemArray[1];
                Path.WorkType = (Enums.WORKTYPE)gridView3.GetDataRow(e.RowHandle).ItemArray[2];
                Path.Path = (string)gridView3.GetDataRow(e.RowHandle).ItemArray[3];
                beforData = (string)gridView3.GetDataRow(e.RowHandle).ItemArray[0];
                propertyGrid1.SelectedObject = Path;
            }
        }

        private void RegBT_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (NoneUser <= 0)
                MessageBox.Show("가입승인 요청자가 없습니다.");
            else
            {
                RegOn regOn = new RegOn();
                regOn.ShowDialog();
                User = dBFunc.GetUser();
                NoneUser = dBFunc.GetNoneUser();
                if (NoneUser > 0)
                    RegBT.Caption = "가입승인요청목록 ( " + NoneUser.ToString() + " ) ";
                else
                {
                    RegBT.Caption = "가입승인요청목록";
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DBFunc dB = new DBFunc();
            object obj = propertyGrid1.SelectedObject;
            List<string> Column = new List<string>();
            List<string> value = new List<string>();
            if (accordionControl1.SelectedElement.Text == "DeletePath")
            {

                Column = Enum.GetNames(typeof(Enums.DELETEPATHCOL)).ToList();
                foreach (string col in Column)
                {
                    value.Add(obj.GetType().GetProperty(col).GetValue(obj, null).ToString());
                }
                if (dB.UplaodDeletePath(value, beforData))
                {
                    LotGrid.DataSource = dB.GetDeletePath();
                    propertyGrid1.SelectedObject = null;
                    //DeletePathMenu_Click(null, null);
                }
            }

        }
        private void DataSerach_BT_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataSeachForm dataSeachForm = new DataSeachForm();
            dataSeachForm.LOT = dBFunc.GetLot();
            dataSeachForm.LOG = dBFunc.GetLog();
            dataSeachForm.User = dBFunc.GetUser();
            dataSeachForm.ShowDialog();
        }
        #endregion

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {

        }

        private void KPI_Click(object sender, EventArgs e)
        {

        }

        private void accordionControl1_SelectedElementChanged(object sender, DevExpress.XtraBars.Navigation.SelectedElementChangedEventArgs e)
        {
            LotGrid.DataSource = null;
            propertyGrid1.SelectedObject = null;
            splitContainer2.Visible = true;
            gridView3.Columns.Clear();
            switch (e.Element.Text)
            {
                case ACCMENU_STR.USER:          LotGrid.DataSource = User;              break;
                case ACCMENU_STR.LOG:           LotGrid.DataSource = LOG;               break;
                case ACCMENU_STR.LOT:           LotGrid.DataSource = LOT;               break;
                case ACCMENU_STR.DELETEPATH:    LotGrid.DataSource = DeletePath;        break;
                case ACCMENU_STR.C_Lot:         CreateLotGraph();                       break;
                case ACCMENU_STR.UserLog:       CreateUserLog();                        break;
                case ACCMENU_STR.LotWorker:     ZipLoad_WorkerCheck();                  break;
                case ACCMENU_STR.Coments:       ZipLoad_Coment();                       break;



            }



        }
        private void ZipLoad_MouseHover(object sender, EventArgs e)
        {
            
        }
        private void Zip_Click(object sender, EventArgs e)
        {

        }

        private void DB_ViewForm_MouseHover(object sender, EventArgs e)
        {

        }



        private void barButtonItem1_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        private void ZipLoad_BT_ItemClick(object sender, ItemClickEventArgs e)
        {
            string path = Util.OpenFileDlg(ZIP_STR.EXETENSION);
            if (path != "")
            {
                Worker = Func.Get_Lot_WorkerList(path,out ComentLog);             
                if(Worker!=null)
                    MessageBox.Show("ZipLoad완료");
            }
            
                
        }

        private void CreateLotGraph()
        {
            try
            {
                splitContainer2.Visible = false;
                chartControl.Series.Clear();
                chartControl.Titles.Clear();
                chartControl.Parent = splitContainer1.Panel2;
                chartControl.Dock = DockStyle.Fill;
                LOT = dBFunc.GetLot();
                Series LotChart_1 = new Series("양품", ViewType.FullStackedBar);
                Series LotChart_2 = new Series("삭제", ViewType.FullStackedBar);
                LotChart_1.ArgumentScaleType = ScaleType.Auto;
                LotChart_1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                LotChart_2.ArgumentScaleType = ScaleType.Auto;
                LotChart_2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((FullStackedBarSeriesView)LotChart_1.View).Color = Color.Gold;
                ((FullStackedBarSeriesView)LotChart_2.View).Color = Color.Red;
                ((FullStackedBarSeriesView)LotChart_1.View).Transparency = 50;
                ((FullStackedBarSeriesView)LotChart_2.View).Transparency = 50;
                foreach (DataRow data in LOT.Rows)
                {
                    string LotName = Convert.ToString(data.ItemArray[0]);
                    int DataValue = Convert.ToInt32(data.ItemArray[1]);
                    int DataValue_D = Convert.ToInt32(data.ItemArray[2]);
                    LotChart_1.Points.Add(new SeriesPoint(LotName, DataValue));
                    LotChart_2.Points.Add(new SeriesPoint(LotName, DataValue_D));
                }
                //chartControl. += HotTrackEventHandler;
                //  for(int i =0; i< LotChart_1.Points.Count; i++)
                //  {
                //      LotChart_1.Points[i].Color = Color.Yellow;
                //      LotChart_2.Points[i].Color = Color.Red;

                //  }

                chartControl.Series.AddRange(new Series[] { LotChart_1, LotChart_2 });


                ((XYDiagram)chartControl.Diagram).EnableAxisXZooming = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisYZooming = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisXScrolling = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisYScrolling = true;

                chartControl.Titles.Add(new ChartTitle());

                chartControl.Titles[0].Text = "Lot 상태 비율";
            }
            catch
            {
                MessageBox.Show("Data Error");
            }
        }

        public void HotTrackEventHandler(object sender, HotTrackEventArgs e)
        {
            MessageBox.Show(e.Object.GetType().Name);
            
        }

        private void CreateUserLog()
        {
            try
            {
                splitContainer2.Visible = false;
                chartControl.Series.Clear();
                chartControl.Titles.Clear();
                chartControl.Parent = splitContainer1.Panel2;
                chartControl.Dock = DockStyle.Fill;
                LOG = dBFunc.GetLog();
                List<string> LotData = new List<string>();
                Dictionary<string, int> LOGData = new Dictionary<string, int>();
                Dictionary<string, int> valuePairs = new Dictionary<string, int>();
                foreach (DataRow row in LOT.Rows)
                {
                    if (!LOGData.ContainsKey(row.ItemArray[0].ToString()))
                        LOGData.Add(row.ItemArray[0].ToString(), int.Parse(row.ItemArray[2].ToString()));
                }
                foreach (DataRow row in LOG.Rows)
                {
                    LotData.Add(row.ItemArray[0].ToString() + "," + row.ItemArray[1].ToString() + "");

                }
                LotData = LotData.Distinct().ToList();
                foreach (string str in LotData)
                {
                    string[] value = str.Split(',');


                    if (!valuePairs.ContainsKey(value[1]))
                    {
                        valuePairs.Add(value[1], LOGData[value[0]]);
                    }
                    else
                    {

                        valuePairs[value[1]] = valuePairs[value[1]] + LOGData[value[0]];
                    }
                }

                Series LotChart_1 = new Series("작업 수", ViewType.Bar);
                LotChart_1.ArgumentScaleType = ScaleType.Qualitative;
                LotChart_1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((BarSeriesView)LotChart_1.View).ColorEach = true;
                foreach (KeyValuePair<string, int> keyValuePair in valuePairs)
                {
                    LotChart_1.Points.Add(new SeriesPoint(keyValuePair.Key, keyValuePair.Value));

                }
                LotChart_1.SeriesPointsSorting = SortingMode.Ascending;
                LotChart_1.LegendTextPattern = "{A}";
                chartControl.Series.Add(LotChart_1);

                ((XYDiagram)chartControl.Diagram).EnableAxisXZooming = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisYZooming = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisXScrolling = true;
                ((XYDiagram)chartControl.Diagram).EnableAxisYScrolling = true;
            }
            catch
            {
                MessageBox.Show("Data Error");
            }
        }
        private void ZipLoad_WorkerCheck()
        {
            if (Worker == null)
                MessageBox.Show("Zip파일을 Load 해주세요.");
            else
                LotGrid.DataSource = Worker;
        }
        private void ZipLoad_Coment()
        {
            try
            {
                if (ComentLog == string.Empty)
                {
                    MessageBox.Show("Zip파일을 Load 해주세요.");
                    return;
                }
                DataTable DT = new DataTable();

                DT.Columns.Add("내용");

                DT.Rows.Add(ComentLog);

                LotGrid.DataSource = DT;
            }
            catch
            {

            }
        }
    }
}
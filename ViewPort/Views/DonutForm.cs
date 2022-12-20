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
using DevExpress.XtraCharts;

namespace ViewPort.Views
{
    public partial class DonutForm : DevExpress.XtraEditors.XtraForm
    {
        public List<string> data = new List<string>();
        public DonutForm(List<string> Data)
        {
            InitializeComponent();
            data = Data;
            GetDoughnut2DChartControl();
        }
        private void GetDoughnut2DChartControl()
        {
           

            Series series = new Series("시리즈 1", ViewType.Doughnut);

            series.ArgumentScaleType = ScaleType.Qualitative;

            series.Label.PointOptions.PointView = PointView.ArgumentAndValues;
            series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            series.Label.PointOptions.ValueNumericOptions.Precision = 0;

            series.SeriesPointsSorting = SortingMode.Ascending;
            series.SeriesPointsSortingKey = SeriesPointKey.Argument;

            ((DoughnutSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMinIndent = 5;

            ((DoughnutSeriesView)series.View).ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9.5));
            ((DoughnutSeriesView)series.View).ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));

            ((DoughnutSeriesView)series.View).ExplodeMode = PieExplodeMode.UseFilters;
            ((DoughnutSeriesView)series.View).ExplodedDistancePercentage = 30;
            ((DoughnutSeriesView)series.View).RuntimeExploding = true;
            ((DoughnutSeriesView)series.View).HoleRadiusPercent = 30;
            int value = 100 / data.Count();
            foreach (string user in data)
            {
                series.Points.Add(new SeriesPoint(user,value));
            }

            chartControl1.Series.Add(series);

            chartControl1.Titles.Add(new ChartTitle());

            chartControl1.Titles[0].Text = "Lot 작업자 목록";

           
        }
    }
    
}
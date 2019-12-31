using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraCharts;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmMusteriKarlikRaporu : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriKarlikRaporu()
        {
            InitializeComponent();
        }

        private void frmistatistikRaporlari_Load(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            if (this.Tag.ToString() == "0")
            {
                xtraTabPage1.PageVisible = true;
                xtraTabPage2.PageVisible = false;
                Musteriistatistikgetir();
            }
            if (this.Tag.ToString() == "1")
            {
                cbTarihAraligi.SelectedIndex = 4;
                xtraTabPage1.PageVisible = false;
                xtraTabPage2.PageVisible = true;
                simpleButton3_Click(sender, e);
            }

            ViewType[] restrictedTypes = new ViewType[]
            { 
                ViewType.PolarArea, ViewType.PolarLine, ViewType.SideBySideGantt,
		        ViewType.SideBySideRangeBar, ViewType.RangeBar, ViewType.Gantt, ViewType.PolarPoint, ViewType.Stock, ViewType.CandleStick,
			   ViewType.Bubble 
            };
            series.SelectedIndex = 0;

            //panelControl4.Visible = true;
        }

        void Musteriistatistikgetir()
        {
            gridControl1.DataSource = DB.GetData(@"select  pkFirma,OzelKod,Firmaadi,SUM(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as AlisVerisToplami,count(s.pkSatislar) as GelisAdedi,
            SUM(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) from Firmalar f with(nolock)
            inner join Satislar s with(nolock) on s.fkFirma=f.pkFirma
            inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
            where s.Siparis=1 and fkSatisDurumu not in(1,10,11)
            group by  pkFirma,OzelKod,Firmaadi");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Musteriistatistikgetir();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if (xtraTabControl1.SelectedTabPageIndex==1)
              //  cbTarihAraligi.SelectedIndex = 0;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string sql = @"select convert(datetime,GuncellemeTarihi,112) as GuncellemeTarihi,
            AlisVerisToplami,Kar,case when AlisVerisToplami=0 then 
            0 else
            round((Kar*100)/AlisVerisToplami,2) end KarYuzde from (
            select  convert(varchar(10),s.GuncellemeTarihi,112) as GuncellemeTarihi,SUM(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as AlisVerisToplami,
            SUM(sd.Adet*((sd.SatisFiyati-sd.AlisFiyati)-sd.iskontotutar)) as Kar
             from Satislar s with(nolock)
            inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
            where s.Siparis=1 and fkSatisDurumu not in(1,10,11)
            and GuncellemeTarihi>=@ilktar and GuncellemeTarihi<=@sontar
            --and GuncellemeTarihi>='2014-05-01'and GuncellemeTarihi<='2014-06-18'
            group by convert(varchar(10),GuncellemeTarihi,112)
            ) as x
            order by 1";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));

            DataTable dt =DB.GetData(sql, list);
            gridControl2.DataSource = dt;
            pivotGridControl1.DataSource = dt;

            if (pivotGridControl1.Cells.RowCount == 1)
               pivotGridControl1.Cells.Selection = new Rectangle(0, 0, pivotGridControl1.Cells.ColumnCount , pivotGridControl1.Cells.RowCount);
            else if (pivotGridControl1.Cells.RowCount > 0)
                pivotGridControl1.Cells.Selection = new Rectangle(0, 0, pivotGridControl1.Cells.ColumnCount, pivotGridControl1.Cells.RowCount-1);

            TimeSpan ts = sontarih.DateTime - ilktarih.DateTime;
            if (ts.Days> 365)
            {
                pgdYil.Visible = true;
            }
            else
                pgdYil.Visible = false;
        }

        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
        }

        private void sorguTarihAraligi(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
                       int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            ilktarih.DateTime = d1.AddSeconds(sec1);
            ilktarih.ToolTip = ilktarih.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);
            sontarih.ToolTip = sontarih.DateTime.ToString();
        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);



            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {

                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                             0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);



            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);




            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private void cbKarYuzde_CheckedChanged(object sender, EventArgs e)
        {
            gridControl2.Visible = cbKarYuzde.Checked;
            pivotGridControl1.Visible = !cbKarYuzde.Checked;
            //panelControl4.Visible = !cbKarYuzde.Checked;
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            panelControl4.Visible = checkEdit1.Checked;
            panelControl4.Dock = System.Windows.Forms.DockStyle.Left;
            panelControl4.SendToBack();
        }

        private void GrafikYenile()
        {
            //try
            //{
            //    if (carigrafik.Checked == true)
            //    {
            //chartControl1.Series[0].Visible = true;
            //chartControl1.Series[1].Visible = true;
                    //cariAdet.Visible = true;
                    //cariTutar.Visible = true;
            //    }
            //    if (tutargarfik.Checked == true)
            //    {
            //        chartGenel.Series[0].Visible = false;
            //        chartGenel.Series[1].Visible = true;
            //        cariAdet.Visible = false;
            //        cariTutar.Visible = true;
            //    }
            //    if (adetigrafik.Checked == true)
            //    {
            //        chartGenel.Series[0].Visible = true;
            //        chartGenel.Series[1].Visible = false;
            //        cariAdet.Visible = true;
            //        cariTutar.Visible = false;
            //    }
            //}
            //catch (Exception exp)
            //{
            //}
        }

        private void series_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewType viewType;
            switch ((string)series.SelectedItem)
            {
                case "Pie 3D":
                    viewType = ViewType.Pie3D;
                    break;
                case "Doughnut":
                    viewType = ViewType.Doughnut;
                    break;
                case "Doughnut 3D":
                    viewType = ViewType.Doughnut3D;
                    break;
                case "Bar":
                    viewType = ViewType.Bar;
                    break;
                case "Line":
                    viewType = ViewType.Line;
                    break;
                case "FullStackedBar":
                    viewType = ViewType.FullStackedBar;
                    break;
                default:
                    viewType = ViewType.FullStackedBar;
                    break;
            }

            chartControl1.SeriesTemplate.ChangeView(viewType);

            DoughnutSeriesView view = chartControl1.Series[0].View as DoughnutSeriesView;
            if (view != null)
                view.HoleRadiusPercent = Convert.ToInt32("55");

            Diagram3D diagram = chartControl1.Diagram as Diagram3D;
            if (diagram != null)
            {
                diagram.RuntimeRotation = true;
                diagram.RuntimeScrolling = true;
                diagram.RuntimeZooming = true;
            }
            GrafikYenile();
        }

        private void series_EditValueChanged(object sender, EventArgs e)
        {
            //if (series.SelectedIndex.ToString() == "0")
            //{
            //    chartFull.Visible = false;
            //    chartGenel.PivotGridDataSourceOptions.RetrieveDataByColumns = true;
            //}

            //if (series.SelectedIndex.ToString() == "1")
            //{
            //    chartFull.Visible = false;
            //    chartGenel.PivotGridDataSourceOptions.RetrieveDataByColumns = true;
            //}
            //if (series.SelectedIndex.ToString() == "2")
            //{
            //    chartFull.Visible = false;
            //    chartGenel.PivotGridDataSourceOptions.RetrieveDataByColumns = true;
            //}
            //if (series.SelectedIndex.ToString() == "3")
            //{
            //    chartFull.Visible = false;
            //    chartGenel.PivotGridDataSourceOptions.RetrieveDataByColumns = true;
            //}
            //if (series.SelectedIndex.ToString() == "4")
            //{
            //chartControl1.Visible = true;
            //chartControl1.Dock = DockStyle.Fill;
            //chartControl1.PivotGridDataSourceOptions.RetrieveDataByColumns = false;
            //}
        }

        private void chartControl1_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            if (e.Series.Tag.ToString() == "Genel Toplam | Kar Yüzde")
            {
            //    //e.LabelText = e.LegendText + " Adeti: " + String.Format("{0:0,0}", Convert.ToDecimal(e.SeriesPoint.Values[0].ToString())) + "\n" +
            //    //"Oranı: " + e.LabelText;// +e.LegendText;

                e.LabelText = "%" +String.Format("{0:0,0.##}", Convert.ToDecimal(e.SeriesPoint.Values[0].ToString()));
            }
            else
                e.LabelText = String.Format("{0:#,#.##}", Convert.ToDecimal(e.SeriesPoint.Values[0].ToString()));
            //else
            ////if (e.Series.Tag == "Cari İşlem Adetleri ve Oranları")
            //{
            //    //e.LabelText = e.SeriesPoint.Argument.ToString() + " Tutarı: " + String.Format("{0:0,0.##}", Convert.ToDecimal(e.SeriesPoint.Values[0].ToString())) + "\n" +
            //    //    "Oranı: " + e.LabelText;// +e.LegendText;

            //    e.LabelText = "Tutarı: " + String.Format("{0:0,0.##}", Convert.ToDecimal(e.SeriesPoint.Values[0].ToString())) + "\n" +
            //        "Oranı: " + e.LabelText;// +e.LegendText;
            //}
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void cbGosterimSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            //günlük
            if(cbGosterimSekli.SelectedIndex==0)
            {
                pivotGridField3.ValueFormat.FormatType=DevExpress.Utils.FormatType.DateTime;
                pivotGridField3.ValueFormat.FormatString = "d";
                pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateDay;
            }
            //haftanın günleri pazartesileri vb..
            else if (cbGosterimSekli.SelectedIndex == 1)
            {
                pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotGridField3.ValueFormat.FormatString = "dddd";
                pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateDayOfWeek; //DateWeekOfMonth;
            }
            //haftalık
            else if (cbGosterimSekli.SelectedIndex == 2)
            {
                pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotGridField3.ValueFormat.FormatString = "d";
                pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateWeekOfMonth;
            }
            //aylık
            else if (cbGosterimSekli.SelectedIndex == 3)
            {
                pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotGridField3.ValueFormat.FormatString = "MMMM";
                pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateMonth;
            }
            //yılık
            else if (cbGosterimSekli.SelectedIndex == 4)
            {
                pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotGridField3.ValueFormat.FormatString = "y";
                pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateYear;

                //pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateQuarter;
            }
            //yıllık
            //else if (cbGosterimSekli.SelectedIndex == 5)
            //{
            //    pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //    pivotGridField3.ValueFormat.FormatString = "yyyy";
            //    pivotGridField3.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.WeekAge;
            //}

            simpleButton3_Click(sender, e);
        }

        private void labelControl4_Click(object sender, EventArgs e)
        {

        }
    }
}
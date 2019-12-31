using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
//using System.Threading;

namespace GPTS
{
    public partial class frmStokSatisGrafigi : DevExpress.XtraEditors.XtraForm
    {
        string _fkstokkarti="0";
        public frmStokSatisGrafigi(string fkstokkarti)
        {
            InitializeComponent();
            _fkstokkarti=fkstokkarti;
        }
        private void ucStokSatisGrafigi_Load(object sender, EventArgs e)
        {
            //ilktarih.DateTime = DateTime.Now.AddDays(-10);
            //sontarih.DateTime = DateTime.Now;
            cbTarihAraligi.SelectedIndex = 6;
            //Thread ikincigorev = new Thread(new ThreadStart(Stoklar));
            //ikincigorev.Start();
            Stoklar();
            gridyukle();
        }
        void gridyukle()
        {
            string sql = @"SELECT top 20 SK.pkStokKarti, SK.Stokadi, SK.Mevcut, sum(SD.Adet) as Satilan, 
            sum(SD.SatisFiyati) as SatisFiyati,SD.Tarih FROM  SatisDetay SD with(nolock)
            INNER JOIN StokKarti SK with(nolock) ON SD.fkStokKarti = SK.pkStokKarti
            where SD.Tarih>'@ilktar' and SD.Tarih<'@sontar'";

            if (lueStoklar.EditValue != null && lueStoklar.EditValue.ToString() != "0")
                sql = sql + " and SK.pkStokKarti=" + lueStoklar.EditValue.ToString();

            sql=sql+" group by SK.pkStokKarti, SK.Stokadi, SK.Mevcut,SD.Tarih order by Satilan desc";

            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd 00:00"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd 23:59"));

            pivotGridControl1.DataSource = DB.GetData(sql);
            int c1 = pivotGridControl1.Cells.ColumnCount;
            if (c1 > 1)
                c1 = c1 - 1;

            int c2 = pivotGridControl1.Cells.RowCount;
            if (c2 > 1)
                c2 = c2 - 1;

            pivotGridControl1.Cells.Selection = new Rectangle(0, 0, c1, c2);  
        }
        

        void Stoklar()
        {
            lueStoklar.Properties.DataSource = DB.GetData("select 0 as pkStokKarti,'Tümü' as StokAdi union all select pkStokKarti,StokAdi FROM StokKarti with(nolock) where Aktif=1");
            lueStoklar.EditValue = Convert.ToInt32(_fkstokkarti);

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           gridyukle();
        }

        private void cbGosterimSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            //günlük
            if (cbGosterimSekli.SelectedIndex == 0)
            {
                pivotGridField3.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
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

    }
}

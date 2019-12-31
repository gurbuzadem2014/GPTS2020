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
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class frmMusteriGelirGider : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriGelirGider()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmMusteriGelirGider_Load(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste())
            {
                Close();
                return;
            }

            //ilkdate.DateTime = DateTime.Now;
            //sondate.DateTime = DateTime.Now;
            cbTarihAraligi.SelectedIndex = 4;

            kasahareket();

            Tasarimlar();
        }

        void kasahareket()
        {
            string sql = @"sp_KasaHareketleriGelirGider @ilktar,@sontar,@GiderOlarakisle,@GelirOlarakisle";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilkdate.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sondate.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@GiderOlarakisle", cbGiderOlarakisle.Checked));
            list.Add(new SqlParameter("@GelirOlarakisle", cbGelirOlarakisle.Checked));
            gridControl1.DataSource = DB.GetData(sql, list);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            kasahareket();
        }

        private void sondate_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void ilkdate_EditValueChanged(object sender, EventArgs e)
        {

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
            ilkdate.DateTime = d1.AddSeconds(sec1);
            ilkdate.ToolTip = ilkdate.DateTime.ToString();

            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sondate.DateTime = d2.AddSeconds(sec2);
            sondate.ToolTip = sondate.DateTime.ToString();

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
        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilkdate.Properties.EditMask = "D";
            sondate.Properties.EditMask = "D";
            ilkdate.Properties.DisplayFormat.FormatString = "D";
            ilkdate.Properties.EditFormat.FormatString = "D";
            sondate.Properties.DisplayFormat.FormatString = "D";
            sondate.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// Bu gün
            {
                ilkdate.Properties.DisplayFormat.FormatString = "f";
                sondate.Properties.EditFormat.FormatString = "f";
                ilkdate.Properties.EditFormat.FormatString = "f";
                sondate.Properties.DisplayFormat.FormatString = "f";
                ilkdate.Properties.EditMask = "f";
                sondate.Properties.EditMask = "f";

                sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilkdate.Properties.DisplayFormat.FormatString = "f";
                sondate.Properties.EditFormat.FormatString = "f";
                ilkdate.Properties.EditFormat.FormatString = "f";
                sondate.Properties.DisplayFormat.FormatString = "f";
                ilkdate.Properties.EditMask = "f";
                sondate.Properties.EditMask = "f";

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

                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                //                    0, 0, 0, 0, 0, 0, false);
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
                sondate.Enabled = true;
                ilkdate.Enabled = true;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "A4");
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriGelirGiderGrid.xml";
            gridView2.SaveLayoutToXml(Dosya);

            gridView2.OptionsBehavior.AutoPopulateColumns = false;
            gridView2.OptionsCustomization.AllowColumnMoving = false;
            gridView2.OptionsCustomization.AllowColumnResizing = false;
            gridView2.OptionsCustomization.AllowQuickHideColumns = false;
            gridView2.OptionsCustomization.AllowRowSizing = false;
        }

        void Tasarimlar()
        {
            string Dosya = DB.exeDizini + "\\MusteriGelirGiderGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                //gridView7.ActiveFilter.Clear();
            }
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriGelirGiderGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);

            //simpleButton19_Click(sender, e);
        }
    }
}
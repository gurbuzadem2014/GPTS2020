using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmAcikSatislarveAlislar : DevExpress.XtraEditors.XtraForm
    {
        public frmAcikSatislarveAlislar()
        {
            InitializeComponent();
        }

        private void frmLogs_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;

           simpleButton1_Click(sender, e);

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "";

//@"select L.*,K.KullaniciAdi from Logs L with(nolock)
//left join Kullanicilar K with(nolock)
//on L.fkKullanicilar=K.pkKullanicilar
//where Tarih>'" + ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm") + "' and Tarih<'" +
//              sontarih.DateTime.ToString("yyyy-MM-dd HH:mm") + "'";

            sql = @"select pkSatislar,'Satış' as AlSat,Tarih,fkfirma,BilgisayarAdi,GuncellemeTarihi,fkKullanici from Satislar with(nolock) where isnull(Siparis,0)=0
            union all
            select pkAlislar,'Alış' as AlSat,Tarih,fkfirma,BilgisayarAdi,GuncellemeTarihi,fkKullanici from Alislar with(nolock) where isnull(Siparis,0)=0";
            gridControl1.DataSource=  DB.GetData(sql);
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

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
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

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage2)
                gridControl2.DataSource= DB.GetData(@"select * from SatislarSilinen ss with(nolock)
left join Firmalar f with(nolock) on ss.fkFirma=f.pkFirma
left join Kullanicilar k with(nolock) on ss.fkKullanicilar=k.pkKullanicilar
");
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;

            if (i < 0) return;

            DataRow dr = gridView1.GetDataRow(i);
            string pkSatislar = dr["pkSatislar"].ToString();
            //alış faturası
            if (dr["AlSat"].ToString() == "Satış")
            {
                frmFisSatisGecmis FisSatisGecmis = new frmFisSatisGecmis(false);
                FisSatisGecmis.fisno.Text = pkSatislar;
                FisSatisGecmis.ShowDialog();
            }
            else if (dr["AlSat"].ToString() == "Alış")
            {
                frmFisAlisGecmis FisSatisGecmis = new frmFisAlisGecmis(false);
                FisSatisGecmis.fisno.Text = pkSatislar;
                FisSatisGecmis.ShowDialog();
            }

        }

        private void buFişiAdminYetkilisineAtaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fisno_s = dr["pkSatislar"].ToString();
            string fisno_a = dr["pkSatislar"].ToString();
            string alsat = dr["AlSat"].ToString();

            if(alsat== "Satış")
                DB.ExecuteSQL("update Satislar set fkKullanici=" + DB.fkKullanicilar + " where pkSatislar=" + fisno_s);
            else
                DB.ExecuteSQL("update Alislar set fkKullanici=" + DB.fkKullanicilar + " where pkAlislar=" + fisno_a);

            formislemleri.Mesajform("Kullanıcı Değiştirildi. Satış veya Alış Ekranını Yenileyiniz!", "B", 200);

            simpleButton1_Click(sender, e);
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            string s = formislemleri.MesajBox("Alış Silinsin mi?", "Alış Detay Boş ise Sil", 1, 3);
            if (s == "0") return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkAlislar = dr["fkSatislar"].ToString();


            if (DB.GetData("select * from AlisDetay with(nolock) where fkAlislar=" + pkAlislar).Rows.Count == 0)
            {
                int i = DB.ExecuteSQL("delete from Alislar where pkAlislar=" + pkAlislar);
                //if (i == 0)
                    MessageBox.Show("Mesaj" + i);
            }
            else
                MessageBox.Show("Alış Bulunamadı");

        }
    }
}
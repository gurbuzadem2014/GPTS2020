using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmBaBsFormlari : DevExpress.XtraEditors.XtraForm
    {
        public frmBaBsFormlari()
        {
            InitializeComponent();
        }
        void FaturaListesi()
        {
            string sql = @"select 
pkFaturaToplu,
FT.Tarih,
FT.FaturaTarihi,
FT.SeriNo,
FT.FaturaNo,
F.pkFirma,
F.Firmaadi,
FT.Aciklama,
Tutar as FaturaTutar,
0 as KdvTutar,
Tutar as Tutar,FT.OnayTarihi 
from FaturaToplu FT with(nolock)
left join Firmalar F with(nolock)
on FT.fkFirma=F.pkFirma
where FT.iptal is null and FT.FaturaTarihi>='@ilktarih' and FT.FaturaTarihi<='@sontarih'
";
            sql = sql.Replace("@ilktarih", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm"));
            sql = sql.Replace("@sontarih", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm"));
            
            gridControl3.DataSource = DB.GetData(sql);
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FaturaListesi();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
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
        void KullaniciListesi()
        {
            lueKullanicilar.Properties.DataSource = DB.GetData("select pkKullanicilar,adisoyadi,KullaniciAdi from Kullanicilar  with(nolock) where durumu=1 ");
            lueKullanicilar.EditValue = int.Parse(DB.fkKullanicilar);
        }
        private void frmBaBsFormlari_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;
            KullaniciListesi();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;
            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);

            string pkFirma = dr["pkFirma"].ToString();
            string pkFaturaToplu = dr["pkFaturaToplu"].ToString();

            //if (DB.GetData("select * from Satislar where fkFirma=" + pkFirma).Rows.Count > 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Müşteri Kartı Hareket Gördüğü için Silemezsiniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Faturayı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
                return;
            DB.ExecuteSQL("Delete From FaturaToplu where pkFaturaToplu=" + pkFaturaToplu);
            formislemleri.Mesajform("Fatura Silindi", "S", 200);

            FaturaListesi();
        }

        private void Getir()
        {
            int i = gridView4.FocusedRowHandle;
            if (i < 0) return;
            DataRow dr = gridView4.GetDataRow(i);

            gridControl2.DataSource = DB.GetData(@"select * from BelgeTakip BT with(nolock) 
            left join BelgeDurumu BD with(nolock) on BD.pkBelgeDurumu=BT.fkBelgeDurumu
            where fkFaturaToplu=" + dr["pkFaturaToplu"].ToString());
        }

        private void gridView4_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            Getir();
        }


        private void faturayıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView4.FocusedRowHandle;
            if (i < 0) return;
            DataRow dr = gridView4.GetDataRow(i);
            string fkFaturaToplu = dr["pkFaturaToplu"].ToString();

            string sql = "insert into BelgeTakip (Tarih,fkBelgeDurumu,fkFaturaToplu,fkKullanici,Aciklama)" +
                " values(getdate(),@fkBelgeDurumu,@fkFaturaToplu,@fkKullanici,'@Aciklama')";

            sql = sql.Replace("@fkBelgeDurumu", "6");//lueBelgeDurumu.EditValue.ToString());
            sql = sql.Replace("@fkFaturaToplu", fkFaturaToplu);
            sql = sql.Replace("@fkKullanici", DB.fkKullanicilar);
            sql = sql.Replace("@Aciklama", "Fatura Kapatıldı");
            DB.ExecuteSQL(sql);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
             int i = gridView4.FocusedRowHandle;

            if (i < 0) return;
            DataRow dr = gridView4.GetDataRow(i);
            string pkFaturaToplu = dr["pkFaturaToplu"].ToString();

            string sql = "update FaturaToplu set OnayTarihi=getdate(),fkKullanicilarOnay=@fkKullanicilarOnay " +
                 " where pkFaturaToplu=" + pkFaturaToplu;

            sql = sql.Replace("@fkKullanicilarOnay", lueKullanicilar.EditValue.ToString());
            DB.ExecuteSQL(sql);

            //Belge Takibi
            sql = "insert into BelgeTakip (Tarih,fkBelgeDurumu,fkFaturaToplu,fkKullanici,Aciklama)" +
                " values(getdate(),@fkBelgeDurumu,@fkFaturaToplu,@fkKullanici,'@Aciklama')";

            sql = sql.Replace("@fkBelgeDurumu", "7");//gönderilme ve onaylama için 
            sql = sql.Replace("@fkFaturaToplu", pkFaturaToplu);
            sql = sql.Replace("@fkKullanici", DB.fkKullanicilar);
            sql = sql.Replace("@Aciklama", "Onaylama İçin Kullanıcıya Gönderildi");
            DB.ExecuteSQL(sql);
        }
    }
}
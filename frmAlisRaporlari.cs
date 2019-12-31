using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using System.Diagnostics;

namespace GPTS
{
    public partial class frmAlisRaporlari : DevExpress.XtraEditors.XtraForm
    {
        public frmAlisRaporlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmAlisRaporlari_Load(object sender, EventArgs e)
        {
            cbTarihAraligi.SelectedIndex = 0;
            simpleButton1_Click_1(sender, e);
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.ShowDialog();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkSatislar = int.Parse(dr["pkAlislar"].ToString());
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
        }

        void Alislar()
        {
            string sql = @"SELECT a.pkAlislar, a.GuncellemeTarihi, a.fkFirma, a.fkKullanici, Tedarikciler.Firmaadi,
                        sum(sd.Adet*sd.AlisFiyati) as Tutar,sum(sd.Adet*(sd.SatisFiyati-sd.AlisFiyati)) as Kar,
                        adu.Durumu,a.Aciklama,a.AlinanPara,k.adisoyadi as KullaniciAdiSoyadi,sum(sd.iskontotutar) as iskontotutar,sum(sd.iskontoyuzdetutar) as iskontoyuzdetutar,a.ToplamTutar,
                        a.EskiFis,a.FaturaNo,a.FaturaTarihi,a.OdemeSekli,a.OdenenKrediKarti,a.OdenenNakit,a.OdenenCek,
                        a.DuzenlemeTarihi,a.AcikHesap FROM  Alislar a with(nolock)
                        inner join AlisDetay sd with(nolock) On sd.fkAlislar=a.pkAlislar
                        left JOIN Tedarikciler with(nolock) ON a.fkFirma = Tedarikciler.pkTedarikciler
                        left join AlisDurumu adu with(nolock) on adu.pkAlisDurumu=a.fkSatisDurumu 
                        left join Kullanicilar k with(nolock) on k.pkKullanicilar=a.fkKullanici
                        where a.Siparis=1 and (a.fkSube is null or a.fkSube=@fkSube) and ";

            if (cbTarih.SelectedIndex == 1)
                sql = sql + " a.GuncellemeTarihi Between @ilktar and @sontar";
            else
                sql = sql + " a.FaturaTarihi Between @ilktar and @sontar";

            sql = sql + @" group by a.pkAlislar, a.GuncellemeTarihi, a.fkFirma, a.fkKullanici, Tedarikciler.Firmaadi,adu.Durumu,
                        a.Aciklama,a.AlinanPara,k.adisoyadi,a.ToplamTutar,a.EskiFis,a.FaturaNo,
                        a.FaturaTarihi,a.OdemeSekli,a.OdenenKrediKarti,a.OdenenNakit,a.OdenenCek,a.DuzenlemeTarihi,a.AcikHesap";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

            gridControl1.DataSource = DB.GetData(sql, list);

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                Alislar();
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
                list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                string sql = @"SELECT a.GuncellemeTarihi as Tarih,a.pkAlislar,StokKarti.pkStokKarti, StokKarti.Barcode, StokKarti.Stokadi, StokGruplari.StokGrup, StokAltGruplari.StokAltGrup, StokKarti.Stoktipi, 
ad.SatisFiyati,ad.AlisFiyati, ad.iskontotutar, ad.Adet, ad.KdvOrani,
                      ad.Adet * (ad.SatisFiyati - ad.iskontotutar) AS iskontolututar, 
                      t.Firmaadi, k.KullaniciAdi
FROM Alislar  a with(nolock)
INNER JOIN AlisDetay  ad with(nolock) ON a.pkAlislar = ad.fkAlislar 
INNER JOIN StokKarti with(nolock) ON ad.fkStokKarti = StokKarti.pkStokKarti 
INNER JOIN Tedarikciler t with(nolock) ON a.fkFirma = t.pkTedarikciler 
INNER JOIN Kullanicilar k with(nolock) ON a.fkKullanici = k.pkKullanicilar 
LEFT JOIN StokAltGruplari with(nolock) ON StokKarti.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari 
LEFT JOIN StokGruplari with(nolock) ON StokKarti.fkStokGrup = StokGruplari.pkStokGrup
where (a.fkSube is null or a.fkSube=@fkSube) and";

                if (cbTarih.SelectedIndex == 1)
                    sql = sql + " a.Siparis=1 and a.GuncellemeTarihi Between @ilktar and @sontar";
                else
                    sql = sql + " a.Siparis=1 and a.FaturaTarihi Between @ilktar and @sontar";

                gridControl2.DataSource = DB.GetData(sql, list);

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


            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih.DateTime = d2.AddSeconds(sec2);

        }

        private void iptalEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(secilen);
            string Durumu = dr["Durumu"].ToString();
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(Durumu + " İptal Edilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;


            DB.pkSatislar = int.Parse(dr["pkAlislar"].ToString());
            int sonuc = DB.ExecuteSQL("UPDATE Alislar Set fkSatisDurumu=5 where pkAlislar=" + DB.pkAlislar.ToString());
            if (sonuc == 0)
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(Durumu + " İptal Edildi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //gridView1.DeleteSelectedRows();
            }
            Alislar();
            gridView1.FocusedRowHandle = secilen;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.pkTedarikciler = int.Parse(dr["fkFirma"].ToString());
            frmTedarikciKarti KurumKarti = new frmTedarikciKarti(dr["fkFirma"].ToString());
            KurumKarti.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            DataRow dr = gridView1.GetDataRow(secilen);
            string Durumu = dr["Durumu"].ToString();
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(Durumu + " Sipariş olarak Değiştirilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;


            DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
            int sonuc = DB.ExecuteSQL("UPDATE Satislar Set fkSatisDurumu=2 where pkSatislar=" + DB.pkSatislar.ToString());
            if (sonuc == 0)
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Sipariş Olarak Çevrildi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //gridView1.DeleteSelectedRows();
            }
            Alislar();
            gridView1.FocusedRowHandle = secilen;
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
                sorguTarihAraligi((-ti.Days), 0, 0, 0, 0, 0, false,
                                   0, 0, 0, 0, 0, 0, false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);

                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                //                    0, 0, 0, 0, 0, 0, false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, 0, 0, 0, false,
                                 (-DateTime.Now.Day), 0, 0, 0, 0, 0, false);



            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
                                  0, 0, 0, 0, 0, 0, false);




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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            if (secilen < 0) return;
            DataRow dr = gridView1.GetDataRow(secilen);
            string Durumu = dr["Durumu"].ToString();
            if (Durumu == "Teklif")
            {
                toolStripMenuItem3.Enabled = true;
                iptalEtToolStripMenuItem.Enabled = true;
            }
            else
                toolStripMenuItem3.Enabled = false;

            if (Durumu == "İptal")
                iptalEtToolStripMenuItem.Enabled = false;
            else
                iptalEtToolStripMenuItem.Enabled = true;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void RaporOnizleme(bool Disigner)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatislar = dr["pkAlislar"].ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            //            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
            //inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
            //inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
            //where s.pkSatislar =" + pkSatislar;
            //Barkod.DataSource = DB.GetData(sql);
            Barkod.DataSource = gridControl1.DataSource;
            RaporDosyasi = exedizini + "\\Raporlar\\AlisRaporlari.repx";
            //rapor.DataSource = gCPerHareketleri.DataSource;
            //rapor.CreateDocument();
            Barkod.LoadLayout(RaporDosyasi);
            //rapor.FindControl("xlKasaAdi", true).Text = lueKasa.Text + " " + ilkdate.DateTime.ToString("dd.MM.yyyy") +
            //    "-" + sondate.DateTime.ToString("dd.MM.yyyy");
            if (Disigner)
                Barkod.ShowDesignerDialog();
            else
                Barkod.ShowRibbonPreview();
        }

        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }

        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                printableLink.Component = gridControl1;
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                printableLink.Component = gridControl2;


            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void ucSatislar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporOnizleme(true);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton1_Click_1(sender, e);
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmSatisUrunBazindaDetay StokTekSatislari = new frmSatisUrunBazindaDetay("0","","");
            StokTekSatislari.Tag = "1";
            StokTekSatislari.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            StokTekSatislari.ilktarih.EditValue = ilktarih.EditValue;
            StokTekSatislari.sontarih.EditValue = sontarih.EditValue;
            StokTekSatislari.ShowDialog();
        }

        private void tedarikçiHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmTedarikcilerHareketleri TedarikciHareketleri = new frmTedarikcilerHareketleri();
            TedarikciHareketleri.musteriadi.Tag = dr["fkFirma"].ToString();
            TedarikciHareketleri.Show();
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            frmStokKarti sk = new frmStokKarti();
            sk.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            sk.ShowDialog();

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            RaporOnizleme(false);
        }

        private void excelGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dosya = "";

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                dosya = Application.StartupPath + "\\AlisRaporuFisBazinda.Xls";
                gridView1.ExportToXls(dosya);
            }

            Process.Start(dosya);
        }

        private void excelGönderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string dosya = "";

            dosya = Application.StartupPath + "\\AlisRaporuUrunBazinda.Xls";
            gridView2.ExportToXls(dosya);

            Process.Start(dosya);
        }
    }
}

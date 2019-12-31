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
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmTransferRaporlari : DevExpress.XtraEditors.XtraForm
    {
        public frmTransferRaporlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void ucSatislar_Load(object sender, EventArgs e)
        {
            //DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
           
            cbTarihAraligi.SelectedIndex = 0;

            simpleButton1_Click_1(sender, e);
            //dizayn
            string Dosya = DB.exeDizini + "\\TransferRaporlariGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

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
            string pkDepoTransfer = dr["pkDepoTransfer"].ToString();
        }

        void Transferler()
        {
            string sql = @"SELECT distinct dt.pkDepoTransfer, dt.fkDepolar,AD.DepoAdi as KaynakDepo, dt.transfer_tarihi, dt.fkKullanicilar, dt.kayit_tarihi, dt.aciklama, 
dt.siparis, dt.bilgisayar_adi, dt.aktarildi, dtd.fkDepolar_alici,DA.DepoAdi as HedepDepo FROM DepoTransfer dt with(nolock)
INNER JOIN  DepoTransferDetay dtd with(nolock) ON dt.pkDepoTransfer = dtd.fkDepoTransfer
LEFT JOIN Depolar AD with(nolock) ON AD.pkDepolar=dt.fkDepolar
LEFT JOIN Depolar DA with(nolock) ON DA.pkDepolar=dtd.fkDepolar_alici
where dt.transfer_tarihi>=@ilktar and dt.transfer_tarihi<=@sontar";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gridControl1.DataSource = DB.GetData(sql, list);
        }

        void OdemeSekliGetir()
        {
            string sql = @"select OdemeSekli,SUM(Borc-Alacak) as Tutar from KasaHareket
where fkSatislar is not null and AktifHesap=1 and Tarih Between @ilktar and @sontar
group by OdemeSekli
union all
select 'Açık Hesap' as OdemeSekli,sum(isnull(AcikHesap,0)) as Tutar from Satislar
where Siparis=1 and GuncellemeTarihi Between @ilktar and @sontar
union all
SELECT 'Nakit(Bonus)' as OdemeSekli, isnull(sum(Bonus),0) as Tutar FROM BonusKullanilan
where Tarih Between @ilktar and @sontar";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gcOdemeSekli.DataSource = DB.GetData(sql, list);
        }

        void SatisDurumlari()
        {
            string sql = @"select sd.Durumu,SUM(sad.Adet*(sad.SatisFiyati-sad.iskontotutar)) as ToplamTutar,sum(isnull(AcikHesap,0)) as AcikHesap,sum(isnull(AcikHesapOdenen,0)) as AcikHesapOdenen from Satislar s with(nolock)
inner join SatisDetay sad with(nolock) on sad.fkSatislar=s.pkSatislar
inner join SatisDurumu  sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu
where Siparis=1 and GuncellemeTarihi Between @ilktar and @sontar
group by sd.Durumu";
ArrayList list = new ArrayList();
list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gridControl2.DataSource = DB.GetData(sql, list);
        }
        
        void Satislar_ayrintili()
        {
            string sql = @"exec sp_FisBazindaSatisRaporu_Ayrintili @ilktar,@sontar";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gridControl3.DataSource = DB.GetData(sql, list);
        }

        void Satislar_ayrintili2()
        {
            string sql = @"SELECT  Satislar.GuncellemeTarihi,Satislar.pkSatislar,StokKarti.pkStokKarti,StokKarti.Barcode, StokKarti.Stokadi, 
                      StokGruplari.StokGrup, StokAltGruplari.StokAltGrup,StokKarti.Stoktipi, 
                      SatisDetay.SatisFiyati, SatisDetay.AlisFiyati, (SatisDetay.iskontotutar+isnull(Faturaiskonto,0)) as iskontotutar
                      , SatisDetay.Adet,(SatisDetay.Adet*(SatisDetay.SatisFiyati-(SatisDetay.iskontotutar+isnull(Faturaiskonto,0)))) as iskontolututar
                      ,(SatisDetay.Adet*(SatisDetay.SatisFiyati-SatisDetay.AlisFiyati-(SatisDetay.iskontotutar+isnull(Faturaiskonto,0)))) as Kar,
                      f.OzelKod,f.Firmaadi,sdu.Durumu,OdemeSekli FROM  Satislar with(nolock)
                      INNER JOIN SatisDetay with(nolock) ON Satislar.pkSatislar = SatisDetay.fkSatislar 
                      left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=Satislar.fkSatisDurumu
                      INNER JOIN  StokKarti with(nolock) ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti 
                      LEFT JOIN StokAltGruplari with(nolock) ON StokKarti.fkStokAltGruplari = StokAltGruplari.pkStokAltGruplari 
                      LEFT JOIN StokGruplari ON StokKarti.fkStokGrup = StokGruplari.pkStokGrup
                      INNER JOIN Firmalar f with(nolock) on f.pkFirma=Satislar.fkFirma 
                      WHERE (Satislar.Siparis = 1) AND(Satislar.GuncellemeTarihi BETWEEN @ilktar AND @sontar)";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"))); 
            gridControl4.DataSource = DB.GetData(sql, list);
        }

        void Satislar_Kullanicibazinda()
        {
            string sql = @"SELECT sum(SatisDetay.Adet) as Adet,sum(SatisDetay.Adet*(SatisDetay.SatisFiyati-SatisDetay.iskontotutar)) as SatisTutari
                      ,sum(SatisDetay.Adet*(SatisDetay.SatisFiyati-SatisDetay.AlisFiyati-SatisDetay.iskontotutar)) as Kar,
                      k.KullaniciAdi,k.adisoyadi,sd.Durumu FROM  Satislar with(nolock)
INNER JOIN SatisDetay with(nolock) ON Satislar.pkSatislar = SatisDetay.fkSatislar 
INNER JOIN  StokKarti with(nolock) ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti 
INNER JOIN Kullanicilar k with(nolock) on k.pkKullanicilar=Satislar.fkKullanici 
INNER JOIN SatisDurumu sd with(nolock) on sd.pkSatisDurumu=Satislar.fkSatisDurumu
WHERE (Satislar.Siparis = 1) AND (Satislar.GuncellemeTarihi BETWEEN @ilktar AND @sontar)
group by k.KullaniciAdi,k.adisoyadi,sd.Durumu";//convert(date,Satislar.GuncellemeTarihi),
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            gridControl5.DataSource = DB.GetData(sql, list);
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                Transferler();
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
                Satislar_ayrintili();
            else if (xtraTabControl1.SelectedTabPageIndex == 2)
                Satislar_ayrintili2();
            else if (xtraTabControl1.SelectedTabPageIndex == 3)
                Satislar_Kullanicibazinda();
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
            if (cbTarihAraligi.SelectedIndex== 0)// Bu gün
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

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        void RaporOnizleme(bool Disigner)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSatislar = dr["pkSatislar"].ToString();
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string RaporDosyasi = "Rapor.repx";
            //xrBarkod Barkod = new xrBarkod();
            DevExpress.XtraReports.UI.XtraReport Barkod = new DevExpress.XtraReports.UI.XtraReport();
            string sql = @"SELECT sk.pkStokKarti,sk.StokKod,sk.Stokadi,sd.Adet,sd.SatisFiyati,sd.iskontotutar,sd.iskontoyuzde,sd.Tarih FROM Satislar s 
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where s.pkSatislar =" + pkSatislar;
            Barkod.DataSource = DB.GetData(sql);

            RaporDosyasi = exedizini + "\\Raporlar\\satisfisia4.repx";
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
            if(xtraTabControl1.SelectedTabPageIndex==0) 
               printableLink.Component = gridControl1;
            if (xtraTabControl1.SelectedTabPageIndex == 1)
                printableLink.Component = gridControl3;
            if (xtraTabControl1.SelectedTabPageIndex == 2)
                printableLink.Component = gridControl4;
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

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisTrasferGecmis FisNoBilgisi = new frmFisTrasferGecmis(false);
            //FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkDepoTransfer"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //frmStokSatisGrafigi StokSatisGrafigi = new frmStokSatisGrafigi();
            //StokSatisGrafigi.ShowDialog();
        }

        private void gridView3_EndSorting(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount > 0)
                gridView3.FocusedRowHandle = 0;
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmSatisUrunBazindaDetay StokTekSatislari = new frmSatisUrunBazindaDetay("0", "", "");
            StokTekSatislari.Tag = "0";
            StokTekSatislari.pkStokKarti.Text = dr["pkStokKarti"].ToString();
            StokTekSatislari.ilktarih.EditValue = ilktarih.EditValue;
            StokTekSatislari.sontarih.EditValue = sontarih.EditValue;
            StokTekSatislari.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
          simpleButton1_Click_1(sender,e);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmMusteriKarlikRaporu istatistikRaporlari = new frmMusteriKarlikRaporu();
            istatistikRaporlari.Tag = 1;
            istatistikRaporlari.Show();
        }

        private void gridView5_EndSorting(object sender, EventArgs e)
        {
            if (gridView5.DataRowCount > 0)
                gridView5.FocusedRowHandle = 0;
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;
            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.TopMost = true;
            FisNoBilgisi.fisno.Text = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void stokKartıDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmFisTrasferGecmis FisNoBilgisi = new frmFisTrasferGecmis(false);
            //FisNoBilgisi.TopMost = true;
            //FisNoBilgisi.fisno.Text = dr["pkDepoTransfer"].ToString();
            //FisNoBilgisi.ShowDialog();

            //gridControl çift tıkda var 
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(true);
            //FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            //FisNoBilgisi.ShowDialog();
        }

        private void btnFisYazdir_Click(object sender, EventArgs e)
        {
            YazdirDizayn(false);
        }

        void YazdirDizayn(bool Yazdir)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\FisToplu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FisToplu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }

            gridView1.ActiveFilterString = "Sec=1";
            string secilen_idler = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    secilen_idler = secilen_idler + dr["pkSatislar"].ToString() + ",";
                    //DB.ExecuteSQL("UPDATE Satislar Set Yazdir=1 where pkSatislar=" + dr["pkSatislar"].ToString());
                }

            }
            gridView1.ClearColumnsFilter();

            if (secilen_idler.Length == 0)
            {
                formislemleri.Mesajform("Seçim yapınız", "K", 200);
                gridView1.Focus();
                return;
            }
            secilen_idler = secilen_idler.Substring(0, secilen_idler.Length - 1);

            FisYazdir(Yazdir, DosyaYol, secilen_idler);
        }

        void FisYazdir(bool Disigner, string RaporDosyasi, string secilen_idler)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.pkSatislar,s.GuncellemeTarihi as Tarih,
                s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,
                sd.iskontotutar,
                (sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu,
                f.Firmaadi,Devir as Bakiye
                from Satislar s with(nolock)
                inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu 
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                inner join Firmalar f with(nolock) on f.pkFirma=s.fkFirma
                where pkSatislar in(" + secilen_idler + ") order by s.pkSatislar desc");//, list);
                //and s.GuncellemeTarihi between @ilktar and @sontar and s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu not in(1,10)
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                
                //DataTable Fis = DB.GetData(@"SELECT Admin");//*,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                //Fis.TableName = "Fis";
                //ds.Tables.Add(Fis);
                //kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket with(nolock) where fkSatislar is null and fkFirma="
                //    + musteriadi.Tag.ToString() + " order by pkKasaHareket DESC");
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.Name = "Müşteri Hesap Extresi";
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void fişTopluDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirDizayn(true);
        }

        private void fişDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
              if (gridView1.FocusedRowHandle < 0) return;

               DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

               fişDüzenleToolStripMenuItem.Tag = dr["pkSatislar"].ToString();

               Close();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\TransferRaporlariGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\TransferRaporlariGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void sutünSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }
    }
}

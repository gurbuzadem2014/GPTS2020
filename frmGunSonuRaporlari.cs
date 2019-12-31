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
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.islemler;
using DevExpress.XtraGrid.Views.Grid;

namespace GPTS
{
    public partial class frmGunSonuRaporlari : DevExpress.XtraEditors.XtraForm
    {
        public frmGunSonuRaporlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmGunSonuRaporlari_Load(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste())
            //{
            //    Close();
            //    return;
            //}

            #region Yetkiler
            DataTable dtYetki = DB.GetData(@"select * from ModullerYetki with(nolock) where Kod like '3%' and fkKullanicilar=" + DB.fkKullanicilar);

            for (int i = 0; i < dtYetki.Rows.Count; i++)
            {
                string Kod = dtYetki.Rows[i]["Kod"].ToString();
                string yetki = dtYetki.Rows[i]["Yetki"].ToString();
                ////gün sonu Raporu
                if (Kod == "3.3" && yetki == "False")
                {
                    Close();
                    break;
                }
                //yeni ekran yapıldı
                //else if (Kod == "3.3.1" && yetki == "False")//Gün Sonu Raporu Liste
                //{
                //    btnListele.Enabled =  false;
                //}
                //else if (Kod == "3.7" && yetki == "False")//Kasa Hareketi Sil
                //{
                //    tsmHareketSil.Enabled = false;
                //}
               
            }
            #endregion

            ilktarih.Properties.DisplayFormat.FormatString = "f";
            sontarih.Properties.EditFormat.FormatString = "f";
            ilktarih.Properties.EditFormat.FormatString = "f";
            sontarih.Properties.DisplayFormat.FormatString = "f";
            ilktarih.Properties.EditMask = "f";
            sontarih.Properties.EditMask = "f";

            sorguTarihAraligi(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            ilktarih.DateTime = DateTime.Today;
            sontarih.DateTime = DateTime.Today.AddHours(23).AddMinutes(59);

            kasalar();
            
            //MusteriTahsilatlari();
            //Satislar();
            //KasaBakiyeDUzeltmeListesi();

            deTarih.DateTime = DateTime.Now;

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            lUEKullanicilar.ItemIndex = 0;

            if (Degerler.ilktarih_devir_sontarih)
            {
                cbSonTarih.Checked = true;
                SonTarihGetir();
                //Gunluk();
            }
            else
                Gunluk();


            KasaHareketleriGridYukle();
        }

        void SonTarihGetir()
        {
            DataTable dt = DB.GetData("select top 1 DATEADD(MINUTE,1,Tarih) as Tarih from KasaGunluk with(nolock) order by 1 desc");
            if (dt.Rows.Count > 0)
            {
                string tar = dt.Rows[0]["Tarih"].ToString();
                ilktarih.DateTime = Convert.ToDateTime(tar);
            }
        }

        void kasalar()
        {
            lueKasalar.Properties.DataSource = DB.GetData("select 0 as pkKasalar, 'Tümü' as KasaAdi "+
                "union all select pkKasalar,KasaAdi from Kasalar with(nolock) where Aktif=1");
            lueKasalar.ItemIndex = 0;
        }

        private DataTable GunSonuDB()
        {
            string sql = @"exec sp_GunSonuRaporu @ilktar,@sontar,@kasaid";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            list.Add(new SqlParameter("@kasaid", lueKasalar.EditValue.ToString()));
            return DB.GetData(sql, list);
        }

        void Gunluk()
        {
            if (btnListele.Enabled == false) return;

            gcGunSonuOzet.DataSource = GunSonuDB();

            //int i = gridView5.FocusedRowHandle;
            //if (i < 0) return;
            //DataRow dr = gridView5.GetDataRow(i);
            //string tip = dr["Tip"].ToString();

            DataRow dr = gridView5.GetDataRow(8);
            kasadaki.Value = decimal.Parse(dr["Tutar"].ToString());
        }

        void MusteriTahsilatlari()
        {
            string sql = @"exec sp_MusteriTahsilatlari @ilktar,@sontar";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));

            gridControl2.DataSource = DB.GetData(sql, list);
            //DataRow dr = gridView2.GetDataRow(7);
            //kasadaki.Value = decimal.Parse(dr["Tutar"].ToString());

        }

        void KasaGunlukListesi()
        {
            gridControl1.DataSource = DB.GetData(@"select top 31 * from KasaGunluk KG with(nolock)"+
                " left join Kullanicilar K with(nolock) on K.pkKullanicilar=KG.fkKullanici order by Tarih desc");
//select pkKasaHareket,Tarih,Borc,Alacak,OdemeSekli,Aciklama,Tutar,k.KullaniciAdi  from KasaHareket as kh with(nolock)
 //           inner join Kullanicilar k with(nolock) on k.pkKullanicilar=kh.fkPersoneller
   //         where OdemeSekli in('Kasa Bakiye Düzeltme','Devir Girişi')");
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["fkFirma"].ToString(),"");
            KurumKarti.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.ShowDialog();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
        }

        void TabListele()
        {
            if (xtraTabControl1.SelectedTabPage == xTab2)
                MusteriTahsilatlari();
            if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                KasaGunlukListesi();
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
                kasahareketleri();
            if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
                Bankahareketleri();
        }

        void KasaHareketleriGridYukle()
        {
            string Dosya = DB.exeDizini + "\\GunSonuKasaHareketGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView3.RestoreLayoutFromXml(Dosya);
                gridView3.ActiveFilter.Clear();
            }
        }
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            TabListele();

            Gunluk();
        }

        void OdemeKaydet(string OdemeSekli, string pkFirma, string pkSatislar, string ToplamTutar)
        {
            if (OdemeSekli == "Nakit")
            {
                //Nakit
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", pkFirma));
                list.Add(new SqlParameter("@Borc", ToplamTutar.ToString()));
                list.Add(new SqlParameter("@Aciklama", "Nakit Ödeme"));
                list.Add(new SqlParameter("@fkSatislar", pkSatislar));
                //list.Add(new SqlParameter("@Tarih", guncellemetarihi.DateTime));
                //guncellemetarihi.DateTime = DateTime.Now;
                DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkKasalar,fkFirma,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                    " values(0,1,@fkFirma,4,1,@Borc,0,1,@Aciklama,'Nakit',@fkSatislar)", list);
                DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + ToplamTutar.ToString() + " where pkFirma=" + pkFirma);
                //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
                //MevcutGuncelle();
                return;
            }
            //kredi kartı ile ödeme yaptıysa
            if (OdemeSekli == "Kredi Kartı")
            {
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkFirma", pkFirma));
                list2.Add(new SqlParameter("@Borc", ToplamTutar.ToString()));
                list2.Add(new SqlParameter("@fkBankalar", "1"));
                list2.Add(new SqlParameter("@Aciklama", "K.Kartı Ödemesi"));
                list2.Add(new SqlParameter("@fkSatislar", pkSatislar));
                DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar)" +
                    " values(@fkBankalar,@fkFirma,8,1,@Borc,0,1,@Aciklama,'Kredi Kartı',@fkSatislar)", list2);
                DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + ToplamTutar.ToString() + " where pkFirma=" + pkFirma);
                //DB.ExecuteSQL("UPDATE Satislar SET Odenen=Odenen+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkSatislar=" + pkSatis);
            }
            //açık hesap
            else //if (OdemeSekli == "Açık Hesap")
            {
                ArrayList list4 = new ArrayList();
                list4.Add(new SqlParameter("@fkSatislar", pkSatislar));
                list4.Add(new SqlParameter("@AcikHesap", ToplamTutar.ToString()));
                DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=@AcikHesap where pkSatislar=@fkSatislar", list4);
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + aratoplam.Value.ToString().Replace(",", ".") + " where pkFirma=" + pkFirma);
            }
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            DB.pkSatislar = 0;
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

        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilkTarih", ilktarih.DateTime.AddMinutes(-1)));
            list.Add(new SqlParameter("@sonTarih", ilktarih.DateTime));
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"SELECT Personeller.pkpersoneller,Firmalar.pkFirma,StokKarti.Stokadi, 
Firmalar.Firmaadi, Firmalar.Tel, Firmalar.Adres, (Firmalar.Alacak-Firmalar.Borc) as Bakiye, Firmalar.OzelKod,
Personeller.adi,Personeller.soyadi,SatisDetay.Adet,SatisDetay.SatisFiyati,(SatisDetay.Adet*SatisDetay.SatisFiyati-SatisDetay.iskontotutar) as Tutar,
Satislar.OdemeSekli
FROM  Satislar 
INNER JOIN SatisDetay ON Satislar.pkSatislar = SatisDetay.fkSatislar 
INNER JOIN StokKarti ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti 
INNER JOIN Firmalar ON Satislar.fkFirma = Firmalar.pkFirma
INNER JOIN Personeller ON  Personeller.pkpersoneller=Satislar.fkPerTeslimEden
WHERE  Satislar.Siparis = 0 and Satislar.fkSatisDurumu=10 
and Satislar.TeslimTarihi between @ilkTarih and @sonTarih 
and Satislar.fkPerTeslimEden=@fkPerTeslimEden
order by Personeller.pkpersoneller,Firmalar.pkFirma", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT TOP 1 * FROM PERSONELLER");
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                //string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSatis.repx";
                XtraReport rapor = new XtraReport();
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
            printableLink.Component = gcGunSonuOzet;
            printableLink.Landscape = true;
            printableLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            printableLink.CreateDocument(Printing);          
            ShowRibbonPreviewDialog(printableLink);


            printableLink.Dispose();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            //FisYazdir(false, "", "Siparis", "");
            yazdir();
        }

        private void ucSatislar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdir(true);
            //FisYazdir(true, "", "Siparis", "");
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();
            //KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();
            //KasaHareketDuzelt.Tag = this.Tag;
            //KasaHareketDuzelt.ShowDialog();
            //KasaBakiyeDUzeltme();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            TabListele();
        }


        private void seçilenleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenleri Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            int sonuc = 0, basarilisay = 0;
            bool hatavar = false;
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
                sonuc = DB.ExecuteSQL("DELETE FROM SatisDetay where fkSatislar=" + DB.pkSatislar.ToString());
                sonuc = DB.ExecuteSQL("DELETE FROM Satislar where pkSatislar=" + DB.pkSatislar.ToString());
                if (sonuc == 0)
                    hatavar = true;
                basarilisay++;
            }
            if (hatavar)
                DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (basarilisay > 0)
                DevExpress.XtraEditors.XtraMessageBox.Show(basarilisay.ToString() + " Siparişe Çevrildi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridView1.FocusedRowHandle = secilen;
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //simpleButton1_Click_1(sender,e);
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            //if (gridView1.DataRowCount < 1000)
            //{
            //    for (int i = 0; i < gridView1.DataRowCount; i++)
            //    {
            //        gridView1.SetRowCellValue(i, "Sec", false);
            //    }
            //}
            //if (gridView1.SelectedRowsCount == 1)
            //{
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //    if (dr["Sec"].ToString() == "True")
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", false);
            //    else
            //        gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", true);
            //    return;
            //}
            //for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            //{
            //    int si = gridView1.GetSelectedRows()[i];
            //    DataRow dr = gridView1.GetDataRow(si);
            //    gridView1.SetRowCellValue(si, "Sec", true);
            //}
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    DataRow dr = gridView1.GetDataRow(e.RowHandle);
            //    if (dr["Sec"].ToString() == "True")
            //    {
            //        e.Appearance.BackColor = Color.SlateBlue;

            //        e.Appearance.ForeColor = Color.White;

            //    }
            //} 
        }

        private void olmasigereken_EditValueChanged(object sender, EventArgs e)
        {
            fark.Value = olmasigereken.Value -kasadaki.Value ;
        }

        private void btnyazdirfis_Click(object sender, EventArgs e)
        {
            Yazdir(false);
        }

        void Yazdir(bool dizayn)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\GunSonu.repx");
            rapor.Name = "GunSonu";
            rapor.Report.Name = "GunSonu";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                rapor.FindControl("label3", true).Text = ilktarih.Text + "         " + sontarih.Text;
                //string pkKasaHareket = "0";
                //if (gridView1.FocusedRowHandle >= 0)
                //{
                //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //    pkKasaHareket = dr["pkKasaHareket"].ToString();
                //}
               // string sql = "select * from KasaHareket where pkKasaHareket=" + pkKasaHareket;
                rapor.DataSource = GunSonuDB();
                    //DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //if (dr["Tipi"].ToString() != "Kasa Hareketleri") return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkKasaHareket"].ToString() + " Kasa Hareketi Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
                DB.ExecuteSQL(sql);
            }
            catch (Exception exp)
            {
               // btnSil.Enabled = true;
                return;
            }
            //btnSil.Enabled = false;
            //btnListele_Click(sender, e);
            KasaGunlukListesi();
        }

        private void gridView5_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int i=gridView5.FocusedRowHandle;

            if(i<0) return;

            DataRow dr = gridView5.GetDataRow(i);
            string tip = dr["Tip"].ToString();
            string baslik = dr["Tur"].ToString();

           
            //devirler
            if (tip == "1")
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }
            //kasadaki-para
            else if (tip == "9")
            {
                //xtraTabPage2.Text = baslik;
                xtraTabControl1.SelectedTabPage = xtraTabPage2;               
            }
            //BANKAdaki-para GİREN VE ÇIKAN
            else if (tip == "6" || tip == "7" || tip == "11")
            {
                //xtraTabPage2.Text = baslik;
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            else
                xtraTabControl1.SelectedTabPage = xTab2;

            TabListele();

        }

        void kasahareketleri()
        {
            string sql = "exec HSP_KasaHareketi_Gunsonu @ilktar,@sontar,@fkSube,@devir,@gunsonu";
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@gruplu", gruplu));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));
            list.Add(new SqlParameter("@devir", cbDevirGoster.Checked));
            list.Add(new SqlParameter("@gunsonu", cbGunSonu.Checked));
            gcKasaHareketleri.DataSource = DB.GetData(sql, list);
        }

        void Bankahareketleri()
        {
            string sql = "exec HSP_BankaHareketi @ilktar,@sontar,@fkSube,@devir";
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@gruplu", gruplu));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));
            list.Add(new SqlParameter("@fkSube", Degerler.fkSube));
            list.Add(new SqlParameter("@devir", cbDevirGoster.Checked));

            gcKrediKarti.DataSource = DB.GetData(sql, list);
        }



        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["fkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void btnDevirBakiye_Click(object sender, EventArgs e)
        {
            if (olmasigereken.Value == null)
            {
                formislemleri.Mesajform("Kasadaki Parayı giriniz", "K", 200);
                olmasigereken.Focus();
                return;
            }
            if (olmasigereken.Text == "")
            {
                formislemleri.Mesajform("Kasadaki Parayı giriniz", "K", 200);
                olmasigereken.Focus();
                return;
            }
            string pkkasahareket = "0";
            decimal _borc = 0,_alacak=0,_fark=fark.Value;
            #region kasahareketi
            
            if (_fark != 0)
            {
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show(fark.Value.ToString() + " Fark Oluştu.Kasa Düzeltinsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.Yes)
                {
                    string sql2 = @"INSERT INTO KasaHareket (fkKasalar,fkKullanicilar,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar,BilgisayarAdi)
                    values(@fkKasalar,@fkKullanicilar,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Kasa Bakiye Düzeltme',@Tutar,@BilgisayarAdi)  SELECT IDENT_CURRENT('KasaHareket')";

                    ArrayList listk = new ArrayList();
                    listk.Add(new SqlParameter("@fkKasalar", Degerler.fkKasalar));
                    if (_fark > 0)
                    {
                        listk.Add(new SqlParameter("@Borc", _fark.ToString().Replace(",", ".").Replace("-", "")));
                        listk.Add(new SqlParameter("@Alacak", "0"));
                    }
                    else
                    {
                        listk.Add(new SqlParameter("@Borc", "0"));
                        listk.Add(new SqlParameter("@Alacak", _fark.ToString().Replace(",", ".").Replace("-", "")));
                    }
                    listk.Add(new SqlParameter("@Tipi", int.Parse("1")));
                    listk.Add(new SqlParameter("@Aciklama", txtAciklama.Text));
                    listk.Add(new SqlParameter("@donem", deTarih.DateTime.Month));
                    listk.Add(new SqlParameter("@yil", deTarih.DateTime.Year));
                    listk.Add(new SqlParameter("@fkFirma", "0"));
                    listk.Add(new SqlParameter("@AktifHesap", cbAktifHesap.Checked));
                    listk.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                    listk.Add(new SqlParameter("@Tutar", kasadaki.Value.ToString().Replace(",", ".")));

                    listk.Add(new SqlParameter("@Tarih", deTarih.DateTime));
                    listk.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));

                    pkkasahareket = DB.ExecuteScalarSQL(sql2, listk);

                    if (pkkasahareket.Substring(0,1)== "H")
                    {
                        formislemleri.Mesajform("Hata Oluştu " + pkkasahareket, "K", 200);
                        return;
                    }
                }
            }
            
            #endregion
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("fkKasalar", "1"));
            list.Add(new SqlParameter("fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("Tarih", deTarih.DateTime));
            list.Add(new SqlParameter("KasadakiPara", olmasigereken.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("OlmasiGereken", kasadaki.Value.ToString().Replace(",",".")));
            list.Add(new SqlParameter("Aciklama", txtAciklama.Text));
            list.Add(new SqlParameter("fkKasaHareket", pkkasahareket));

            string sonuc = DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama,fkKasaHareket) 
            values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama,@fkKasaHareket)", list);
            if (sonuc.Substring(0, 1) == "H")
             {
                 formislemleri.Mesajform("Hata Oluştu " + sonuc, "K", 200);
             }

            xtraTabControl1.SelectedTabPage = xtraTabPage1;

            Gunluk();

            TabListele();

            olmasigereken.Value = 0;
            fark.Value = 0;
        }

        private void silToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;
            DataRow dr = gridView1.GetDataRow(i);
            string pkKasaGunluk = dr["pkKasaGunluk"].ToString();
            string fkKasaHareket = dr["fkKasaHareket"].ToString();
            
            string Tarih = dr["Tarih"].ToString();

            //ve Kasa Hareketini
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(Tarih + " Tarihli Kasa Günlük Hareketini Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            int sonuc = DB.ExecuteSQL("delete from KasaGunluk where pkKasaGunluk=" + pkKasaGunluk);
            
            if (fkKasaHareket!="" && sonuc==1)
               DB.ExecuteSQL("delete from KasaHareket where pkKasaHareket=" + fkKasaHareket);

            KasaGunlukListesi();

            Gunluk();

            olmasigereken.Value = 0;
            fark.Value = 0;
        }

        void kod()
        {
            inputForm sifregir = new inputForm();
            sifregir.Text = "Devir Girişi";
            sifregir.GirilenCaption.Text = "Devir Tutarını Giriniz.";
            sifregir.Girilen.Text = kasadaki.Text;

            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir.ShowDialog();
            //if (sifregir.Girilen.Text != Degerler.OzelSifre) return;

            if (sifregir.Girilen.Text == "") return;
            try
            {
                decimal DevirBakiye = decimal.Parse(sifregir.Girilen.Text);

                string sql = @"INSERT INTO KasaHareket (fkKasalar,fkPersoneller,Tarih,Modul,Tipi,Borc,Alacak,Aciklama,donem,yil,Odendi,AktifHesap,fkFirma,fkTedarikciler,OdemeSekli,Tutar)
                    values(@fkKasalar,@fkPersoneller,@Tarih,3,@Tipi,@Borc,@Alacak,@Aciklama,@donem,@yil,0,@AktifHesap,@fkFirma,0,'Devir Girişi',@Tutar)";
                ArrayList list0 = new ArrayList();
                list0.Add(new SqlParameter("@fkKasalar", "1"));
                if (DevirBakiye > 0)
                {
                    list0.Add(new SqlParameter("@Borc", DevirBakiye.ToString().Replace(",", ".").Replace("-", "")));
                    list0.Add(new SqlParameter("@Alacak", "0"));
                }
                else
                {
                    list0.Add(new SqlParameter("@Borc", "0"));
                    list0.Add(new SqlParameter("@Alacak", DevirBakiye.ToString().Replace(",", ".").Replace("-", "")));
                }
                list0.Add(new SqlParameter("@Tipi", int.Parse("1")));
                list0.Add(new SqlParameter("@Aciklama", "Devir Bakiye"));
                list0.Add(new SqlParameter("@donem", DateTime.Now.Month));
                list0.Add(new SqlParameter("@yil", DateTime.Now.Year));
                list0.Add(new SqlParameter("@fkFirma", "0"));
                list0.Add(new SqlParameter("@AktifHesap", "0"));
                list0.Add(new SqlParameter("@fkPersoneller", DB.fkKullanicilar));
                list0.Add(new SqlParameter("@Tutar", "0"));

                list0.Add(new SqlParameter("@Tarih", DateTime.Now));

                string sonuc = DB.ExecuteSQL(sql, list0);
                if (sonuc != "0")
                {
                    MessageBox.Show("Hata Oluştu Tekrar deneyiniz");
                    return;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (lUEKullanicilar.EditValue.ToString().Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Gün Sorunu Raporu " +
                lUEKullanicilar.EditValue.ToString() + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\GunSonu.repx");
            rapor.Name = "GunSonu";
            rapor.Report.Name = "GunSonu.repx";
            
            try
            {
                rapor.FindControl("label3", true).Text = ilktarih.Text + "\r" + sontarih.Text;

                string dosyaadi = Application.StartupPath + "\\GunSonu.pdf";
                rapor.DataSource = gcGunSonuOzet.DataSource;
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gün Sonu Raporu", dosyaadi, "Gün Sonu Raporu");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

            /*
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\GunSonu.repx");
            rapor.Name = "GunSonu";
            rapor.Report.Name = "GunSonu";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                //string pkKasaHareket = "0";
                //if (gridView1.FocusedRowHandle >= 0)
                //{
                //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //    pkKasaHareket = dr["pkKasaHareket"].ToString();
                //}
                // string sql = "select * from KasaHareket where pkKasaHareket=" + pkKasaHareket;
                rapor.DataSource = GunSonuDB();
                //DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
             */
        }

        private void silToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string fkSatislar = dr["FisNo"].ToString();

            if (fkSatislar != "") return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkKasaHareket"].ToString() + " Kasa Hareketi Silinsin mi?", "GPTS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            try
            {
                //if (fkCek != "")
                //    DB.ExecuteSQL("update Cekler set fkCekTuru=0 where pkCek=" + fkCek);

                //if (fkTaksitler != "")
                //{
                //    DB.ExecuteSQL("UPDATE Taksitler Set Odenen=0" +
                //        ",OdendigiTarih=null,OdemeSekli='silindi'" +
                //        " where pkTaksitler=" + fkTaksitler);
                //}


                string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
                DB.ExecuteSQL(sql);
            }
            catch (Exception exp)
            {
                //btnSil.Enabled = true;
                return;
            }
            //btnSil.Enabled = false;
            //btnListele_Click(sender, e);
            kasahareketleri();
            Gunluk();
        }

        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }
        void KasaBakiyeDuzenle()
        {
            frmKasaBakiyeDuzeltme KasaBakiyeDuzeltme = new frmKasaBakiyeDuzeltme(0);
            //KasaBakiyeDuzeltme.pkKasalar.Text = "1";
            KasaBakiyeDuzeltme.ceKasadakiParaYeni.Value = fark.Value;
            KasaBakiyeDuzeltme.ShowDialog();

            Gunluk();
            TabListele();

            olmasigereken.Value = 0;
            fark.Value = 0;
        }
        private void kasaBakiyeDüzeltmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

       

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridView5_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string tip = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Tip"]);
                //string Fiyat = View.GetRowCellDisplayText(e.RowHandle, View.Columns["SatisFiyati"]);
                if (tip == "9")
                    e.Appearance.ForeColor = Color.Blue;
                   //e.Appearance.BackColor = Color.Blue;

            }
        }

        private void gridView5_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(e.RowHandle);
            if (dr == null)
            {
                return;
            }
            else if (e.Column.FieldName == "Tutar" && dr["Tur"].ToString() == "DEVİR ÖNCESİ TOPLAM")
            {
                e.Appearance.BackColor = Color.Green;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tur" && dr["Tur"].ToString() == "DEVİR ÖNCESİ TOPLAM")
            {
                e.Appearance.BackColor = Color.Green;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tutar" && dr["Tur"].ToString() == "GÜNLÜK KAR")
            {
                e.Appearance.BackColor = Color.Yellow;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tur" && dr["Tur"].ToString() == "GÜNLÜK KAR")
            {
                e.Appearance.BackColor = Color.Yellow;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Black;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tutar" && dr["Tur"].ToString() == "KASADAKİ PARA")
            {
                e.Appearance.BackColor = Color.Blue;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tur" && dr["Tur"].ToString() == "KASADAKİ PARA")
            {
                e.Appearance.BackColor = Color.Blue;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tutar" && dr["Tur"].ToString() == "GÜNLÜK CİRO")
            {
                e.Appearance.BackColor = Color.BlueViolet;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
            else if (e.Column.FieldName == "Tur" && dr["Tur"].ToString() == "GÜNLÜK CİRO")
            {
                e.Appearance.BackColor = Color.BlueViolet;
                //e.Appearance.ForeColor = Color.Black;
                e.Appearance.Font = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                e.Appearance.ForeColor = Color.White;
                //e.Appearance.BackColor2 = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            deTarih.DateTime = DateTime.Now;
        }

        private void devirBakiyeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            KasaBakiyeDuzenle();
            kasahareketleri();
        }

        private void kasaBakiyeDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KasaBakiyeDuzenle();
            Gunluk();
        }

        private void cbDevirGoster_CheckedChanged(object sender, EventArgs e)
        {
            kasahareketleri();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\GunSonuKasaHareketGrid.xml";
            gridView3.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Dosya = DB.exeDizini + "\\GunSonuKasaHareketGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }
        }

        private void alanListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView3.ShowCustomization();
            gridView3.OptionsBehavior.AutoPopulateColumns = true;
            gridView3.OptionsCustomization.AllowColumnMoving = true;
            gridView3.OptionsCustomization.AllowColumnResizing = true;
            gridView3.OptionsCustomization.AllowQuickHideColumns = true;
            gridView3.OptionsCustomization.AllowRowSizing = true;
        }

        private void btnBugun_Click(object sender, EventArgs e)
        {
            ilktarih.DateTime = DateTime.Today;
        }

        private void cbSonTarih_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSonTarih.Checked)
            {
                SonTarihGetir();
                DB.ExecuteSQL("update Sirketler set  ilktarih_devir_sontarih=1");
                Degerler.ilktarih_devir_sontarih = true;
            }
            else
            {
                ilktarih.DateTime = DateTime.Today;
                DB.ExecuteSQL("update Sirketler set  ilktarih_devir_sontarih=0");
                Degerler.ilktarih_devir_sontarih = false;
            }
            Gunluk();

           
        }

        private void btOnizleEpostaGonder_Click(object sender, EventArgs e)
        {
            gridView5.ViewCaption = "Gün Sonu " + ilktarih.Text+" - " + sontarih.Text;
            gridView5.OptionsView.ShowViewCaption = true;
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gcGunSonuOzet;
            printableLink.Landscape = true;
            printableLink.PaperKind = System.Drawing.Printing.PaperKind.A5Rotated;
            printableLink.CreateDocument(Printing);

            string dosyaadi = DB.exeDizini + "\\gunsonu.pdf";

            printableLink.ExportToPdf(dosyaadi);

            //ShowRibbonPreviewDialog(printableLink);
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process.StartInfo.FileName = filePath;
            //process.Start();

            DB.epostagonder(lUEKullanicilar.EditValue.ToString(), "Gün Sonu Raporu", dosyaadi, "Gün Sonu Raporu");
            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            printableLink.Dispose();

            gridView5.OptionsView.ShowViewCaption = false;
        }
    }
}

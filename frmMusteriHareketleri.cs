using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraReports.UI;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmMusteriHareketleri : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriHareketleri()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 50;
        }

        private void frmCariHareketlerMusteri_Load(object sender, EventArgs e)
        {
            DataTable dtYetki = DB.GetData(@"select * from ModullerYetki with(nolock)
            where Kod in('15.2','3.7') and fkKullanicilar=" + DB.fkKullanicilar);
            for (int i = 0; i < dtYetki.Rows.Count; i++)
            {
                string Kod = dtYetki.Rows[i]["Kod"].ToString();
                string yetki = dtYetki.Rows[i]["Yetki"].ToString();
                if (Kod== "15.2" && yetki == "False")
                {
                    formislemleri.Mesajform("Bu Sayfaya Girme Yetkiniz yok", "K", 100);
                    Close();
                }
                else if (Kod == "3.7" && yetki == "False")//Kasa Hareketi Sil
                {
                    hareketiSilToolStripMenuItem.Enabled = false;
                }
            }

            ilktarih.DateTime = DateTime.Today;
            sontarih.DateTime = DateTime.Now;

            DB.pkTedarikciler = 0;

            DataTable dtp = DB.GetData(@"select pkFirma,OzelKod,Firmaadi,Eposta,convert(decimal(18,2),Bonus) as Bonus from Firmalar with(nolock) where pkFirma=" + musteriadi.Tag.ToString());
            if (dtp.Rows.Count == 0)
            {
                MessageBox.Show("Müşteri Bulunamadı");
                return;
            }
            else
            {
                musteriadi.AccessibleName = dtp.Rows[0]["Firmaadi"].ToString();
                musteriadi.Text = dtp.Rows[0]["OzelKod"].ToString() + " - " + dtp.Rows[0]["Firmaadi"].ToString();
                musteriadi.AccessibleDescription = dtp.Rows[0]["Eposta"].ToString();

                //bonus bilgileri
                DataTable dts = DB.GetData("select top 1 * from Sirketler with(nolock)");
                if (dts.Rows.Count > 0)
                {
                    if (dts.Rows[0]["BonusYuzde"].ToString() == "0")
                    {
                        lBonus.Visible = false;
                        ceBonusKalan.Visible = false;
                    }
                    else
                        ceBonusKalan.EditValue = dtp.Rows[0]["Bonus"].ToString();
                }

            }
            //string sql = @"exec sp_MusteriSatisListesi " + musteriadi.Tag.ToString();
            //gridControl1.DataSource = DB.GetData(sql);
            //vSatislar();
            cbTarihAraligi.SelectedIndex = 0;
            //Odemeler();
            cbTarihAraligi2.SelectedIndex = 0;
            

            BakiyeGetir();

            //System.Threading.Thread islem = new System.Threading.Thread(new System.Threading.ThreadStart(BakiyeGetir));
            //islem.Start();

            Taksitler();


            string Dosya = DB.exeDizini + "\\MusteriOdemeHareketleriGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView2.RestoreLayoutFromXml(Dosya);
                gridView2.ActiveFilter.Clear();
            }

            Dosya = DB.exeDizini + "\\MusteriFisHareketleriGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }

            //fatura tasarım
            Dosya = DB.exeDizini + "\\MusteriFaturaHareketleriGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView7.RestoreLayoutFromXml(Dosya);
                gridView7.ActiveFilter.Clear();
            }
        }
        
        void gelendurum()
        {
            DataTable dt = DB.GetData(@"select isnull(sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)),0) as SatisToplam from Satislar s with(nolock)
inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar 
where s.Siparis=1 and fkSatisDurumu not in(10,1) and s.fkFirma=" + musteriadi.Tag.ToString());
            if (dt.Rows.Count > 0)
                satistoplam.Value = decimal.Parse(dt.Rows[0][0].ToString());
            //ödemeler
            //DataTable dto = DB.GetData(@"SELECT isnull(sum(Borc),0) FROM KasaHareket with(nolock) WHERE fkFirma =" + musteriadi.Tag.ToString());
            //odemetoplam.Value =  decimal.Parse(dto.Rows[0][0].ToString());
        }
        
        void StokListesi()
        {
            string sql = @"SELECT SUM(SatisDetay.Adet) AS ToplamAdet, StokKarti.Stokadi,sum(iskontotutar) as iskontotutar, 
                      StokKarti.Barcode, 
SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati - SatisDetay.iskontotutar)) AS Tutar,
SUM((sd.SatisFiyatiKdvHaric-(sd.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100)) as TutarKdvHaric,
SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati-(SatisDetay.AlisFiyati + SatisDetay.iskontotutar))) AS Kar
FROM         Satislar INNER JOIN
                      SatisDetay ON Satislar.pkSatislar = SatisDetay.fkSatislar 
INNER JOIN  StokKarti ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti
WHERE  (Satislar.Siparis = 1) AND (Satislar.fkFirma =@fkFirma)
and (Satislar.GuncellemeTarihi between @ilktar and @sontar)
GROUP BY StokKarti.Stokadi, StokKarti.Barcode
ORDER BY StokKarti.Stokadi ";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl2.DataSource = DB.GetData(sql);
        }
        
        void SatisDetayMiktarBazinda()
        {
            string sql = @"SELECT sum(sd.Adet) as Miktar, sk.Stokadi,sk.Barcode, sd.AlisFiyati, sd.SatisFiyati, sd.iskontotutar, sd.KdvOrani, 
                           sum(sd.Adet * (sd.SatisFiyati - sd.iskontotutar)) AS Tutar,
                           sum(sd.Adet *((sd.SatisFiyatiKdvHaric-(sd.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100))) as TutarKdvHaric,
                           sum(sd.Adet * (sd.SatisFiyati - (sd.iskontotutar+sd.AlisFiyati))) as Kar
                          FROM   Satislar s with(nolock) INNER JOIN
                          SatisDetay sd with(nolock) ON s.pkSatislar = sd.fkSatislar INNER JOIN
                          StokKarti sk with(nolock) ON sd.fkStokKarti = sk.pkStokKarti
                          WHERE  s.Siparis = 1 and fkSatisDurumu not in(1,10,11) AND s.fkFirma =@fkFirma 
                          and (s.GuncellemeTarihi>=@ilktar and s.GuncellemeTarihi<=@sontar)
                         group by  sk.Stokadi,sk.Barcode, sd.AlisFiyati, sd.SatisFiyati, sd.iskontotutar, sd.KdvOrani ";
            //sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl2.DataSource = DB.GetData(sql, list);
        }

        void SatisDetayUrunBazinda()
        {
            string sql = @"SELECT Satislar.pkSatislar,SatisDetay.Tarih,SatisDetay.Adet as Miktar, StokKarti.Stokadi, SatisDetay.AlisFiyati, SatisDetay.SatisFiyati, SatisDetay.iskontotutar, SatisDetay.KdvOrani, 
                           StokKarti.Barcode, 
                        SatisDetay.Adet * (SatisDetay.SatisFiyati - SatisDetay.iskontotutar) AS Tutar,
(sd.SatisFiyatiKdvHaric-(sd.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100) as TutarKdvHaric,
                                                   
SatisDetay.Adet * (SatisDetay.SatisFiyati - (SatisDetay.iskontotutar+SatisDetay.AlisFiyati)) as Kar,
                           SDA.aciklama_detay FROM         Satislar with(nolock) 
                        INNER JOIN SatisDetay with(nolock) ON Satislar.pkSatislar = SatisDetay.fkSatislar 
                        LEFT JOIN SatisDetayAciklama SDA with(nolock) ON SDA.fkSatisDetay = SatisDetay.pkSatisDetay 
                        INNER JOIN StokKarti with(nolock) ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti
                        WHERE Satislar.Siparis = 1 and fkSatisDurumu not in(1,10,11) AND Satislar.fkFirma =@fkFirma
                        and (Satislar.GuncellemeTarihi>=@ilktar and Satislar.GuncellemeTarihi<=@sontar)
                        ORDER BY StokKarti.Stokadi ";
            sql = @"SELECT Satislar.pkSatislar,sd.Tarih,sd.Adet as Miktar, StokKarti.Stokadi, sd.AlisFiyati,
 sd.SatisFiyati, sd.iskontotutar, sd.KdvOrani, 
                           StokKarti.Barcode, 
                        sd.Adet * (sd.SatisFiyati - sd.iskontotutar) AS Tutar,
sd.Adet * (sd.SatisFiyatiKdvHaric-(sd.SatisFiyatiKdvHaric*sd.iskontoyuzdetutar)/100) as TutarKdvHaric,
                                                   
sd.Adet * (sd.SatisFiyati - (sd.iskontotutar+sd.AlisFiyati)) as Kar,
                           SDA.aciklama_detay FROM         Satislar with(nolock) 
                        INNER JOIN SatisDetay sd with(nolock) ON Satislar.pkSatislar = sd.fkSatislar 
                        LEFT JOIN SatisDetayAciklama SDA with(nolock) ON SDA.fkSatisDetay = sd.pkSatisDetay 
                        INNER JOIN StokKarti with(nolock) ON sd.fkStokKarti = StokKarti.pkStokKarti
                        WHERE Satislar.Siparis = 1 and fkSatisDurumu not in(1,10,11) AND Satislar.fkFirma =@fkFirma
                        and (Satislar.GuncellemeTarihi>=@ilktar and Satislar.GuncellemeTarihi<=@sontar)
                        ORDER BY StokKarti.Stokadi";
            //sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl3.DataSource = DB.GetData(sql, list);
        }
        

        void Yazdir(bool dizayn)
        {
            string  DosyaYol =DB.exeDizini + "\\Raporlar\\HesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DosyaYol);
            try
            {
                //string sql = "select * from Taksitler where pkTaksitler=" + pkTaksitler;//"exec personeldurum " + DB.pkPersoneller.ToString();
                rapor.DataSource = gcSatislar.DataSource;//DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowPreview();
            else
                rapor.ShowDesigner();
        }
        
        public void StartProcess(string path)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            try
            {
                process.StartInfo.FileName = path;
                process.Start();
                process.WaitForInputIdle();
            }
            catch { }
        }
        
        void EPostaliFisYazdir(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.GuncellemeTarihi as Tarih,s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,sd.iskontotutar,(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli from Satislar s
inner join SatisDetay sd on sd.fkSatislar=s.pkSatislar 
inner join StokKarti sk on sk.pkStokKarti=sd.fkStokKarti
where s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu<>10 and 
s.GuncellemeTarihi between @ilktar and @sontar order by s.pkSatislar,sd.pkSatisDetay", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //kasahareketleri
                DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket where fkSatislar is null and fkFirma="
                    + musteriadi.Tag.ToString() + " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                XtraReport rapor = new XtraReport();
                rapor.Name = "Müşteri Hesap Extresi";
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);

                DevExpress.XtraPrinting.XlsExportOptions xlsOptions = rapor.ExportOptions.Xls;
                xlsOptions.ShowGridLines = true;
                xlsOptions.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;

                // Export the report to XLS.
                string reportPath = exedizini + "\\MusteriExtre.xls";
                rapor.ExportToXls(reportPath);

                // Show the result.
                StartProcess(reportPath);

                //MemoryStream mem = new MemoryStream();
                //rapor.ExportToPdf(mem);
                //System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(mem,DB.exeDizini + "\\MusteriExtre.pdf", "application/pdf");
                // Create a new attachment and put the PDF report into it.
                //mem.Seek(0, System.IO.SeekOrigin.Begin);
                //mem.Close();
                //mem.Flush();
                DB.epostagonder( musteriadi.AccessibleDescription, "Exreniz", reportPath, "Extreniz...");

                //if (Disigner)
                //    rapor.ShowDesigner();
                //else
                  //  rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
                rapor.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        
        void FisYazdir(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));

            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.GuncellemeTarihi as Tarih,
                s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,
                sd.iskontotutar,
                (sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu,s.Aciklama,s.FaturaNo,
                sda.aciklama_detay from Satislar s with(nolock)
                inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                left join SatisDetayAciklama sda with(nolock) on sda.fkSatisDetay=sd.pkSatisDetay
                left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu 
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                where s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu not in(1,10,11) and 
                s.GuncellemeTarihi>=@ilktar and s.GuncellemeTarihi<=@sontar order by s.pkSatislar,sd.pkSatisDetay", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //BAŞLIK
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                //kasahareketleri
                DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket with(nolock) where fkSatislar is null and fkFirma="
                    + musteriadi.Tag.ToString()+ " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
               
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriHesapExtresi";
                rapor.Report.Name = "MusteriHesapExtresi";

                //rapor.Name = "Müşteri Hesap Extresi";
                //rapor.Report.Name = "Müşteri Hesap Extresi";
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
        void FisYazdirPdf(string pdfdosyaadi, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.GuncellemeTarihi as Tarih,
                s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,
                sd.iskontotutar,
                (sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu,s.Aciklama from Satislar s with(nolock)
                inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu 
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                where s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu not in(1,10,11) and 
                s.GuncellemeTarihi between @ilktar and @sontar order by s.pkSatislar,sd.pkSatisDetay", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //kasahareketleri
                DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket with(nolock) where fkSatislar is null and fkFirma="
                    + musteriadi.Tag.ToString() + " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriHesapExtresi";
                rapor.Report.Name = "MusteriHesapExtresi";

                //rapor.Name = "Müşteri Hesap Extresi";
                //rapor.Report.Name = "Müşteri Hesap Extresi";
                //if (Disigner)
                //    rapor.ShowDesigner();
                //else
                  //  rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
                rapor.ExportToPdf(pdfdosyaadi);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }
        void FisYazdir2(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec sp_MusteriHesapExresi @fkFirma,'@ilktar','@sontar'";
            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(sql, list);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                DataTable dtSanal = new DataTable();
                //dtSanal.Columns.Add(new DataColumn("FisNo", typeof(int)));
                dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                dtSanal.Columns.Add(new DataColumn("Hareket", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("OdemeSekli", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("FaturaNo", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Borc", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Alacak", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Bakiye", typeof(decimal)));
                decimal bakiye = 0;
                for (int i = 0; i < FisDetay.Rows.Count; i++)
                {
                    decimal borc = 0,alacak=0;
                    if (FisDetay.Rows[i]["Borc"].ToString() != "0,00")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["Borc"].ToString());
                        bakiye = bakiye - borc;
                    }
                    if (FisDetay.Rows[i]["Alacak"].ToString() != "0,00")
                    {
                        alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                        bakiye = bakiye + alacak;
                    }
                    DataRow dr;
                    dr = dtSanal.NewRow();
                    //dr["FisNo"] = FisDetay.Rows[i]["FisNo"];
                    dr["Tarih"] = FisDetay.Rows[i]["Tarih"];
                    dr["Hareket"] = FisDetay.Rows[i]["Hareket"].ToString();
                    dr["Aciklama"] = FisDetay.Rows[i]["Aciklama"];
                    dr["OdemeSekli"] = FisDetay.Rows[i]["OdemeSekli"];
                    dr["FaturaNo"] = FisDetay.Rows[i]["FaturaNo"];
                    dr["Borc"] = FisDetay.Rows[i]["Borc"];
                    dr["Alacak"] = FisDetay.Rows[i]["Alacak"].ToString();
                    dr["Bakiye"] = bakiye;
                    dtSanal.Rows.Add(dr);
                }
                dtSanal.TableName = "FisDetay";
                ds.Tables.Add(dtSanal);
                //Firma Bilgileri
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                ////kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select Tarih,OdemeSekli,Borc,Alacak,Tutar  from KasaHareket WHERE fkFirma=" + musteriadi.Tag.ToString());
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                
                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriHesapExtresi2";
                rapor.Report.Name = "MusteriHesapExtresi2";

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

        void FisYazdirFatura(bool Disigner, string RaporDosyasi,string eposta)
        {
            string sonfisler = "0";

            if (cbTarihAraligi.SelectedIndex == 0)//son 10 fiş
                sonfisler = "0";
            else
                sonfisler = "1";

            string sql = "exec hsp_FaturalarKesilenRapor @fkFirma,@ilktar,@sontar,"+ sonfisler;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));

            try
            {
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(sql,list);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                DataTable dtSanal = new DataTable();
                dtSanal.Columns.Add(new DataColumn("pkFaturalar", typeof(int)));
                dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                dtSanal.Columns.Add(new DataColumn("FaturaTarihi", typeof(DateTime)));
                dtSanal.Columns.Add(new DataColumn("Hareket", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Adres", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("FaturaNo", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Borc", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Alacak", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Bakiye", typeof(decimal)));
                decimal bakiye = 0;
                for (int i = 0; i < FisDetay.Rows.Count; i++)
                {
                    decimal borc = 0, alacak = 0;
                    if (FisDetay.Rows[i]["Borc"].ToString() != "0,00")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["Borc"].ToString());
                        bakiye = bakiye - borc;
                    }
                    if (FisDetay.Rows[i]["Alacak"].ToString() != "0,00")
                    {
                        alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                        bakiye = bakiye + alacak;
                    }
                    DataRow dr;
                    dr = dtSanal.NewRow();
                    dr["pkFaturalar"] = FisDetay.Rows[i]["pkFaturalar"];
                    dr["Tarih"] = FisDetay.Rows[i]["Tarih"];
                    dr["FaturaTarihi"] = FisDetay.Rows[i]["FaturaTarihi"];
                    dr["Hareket"] = FisDetay.Rows[i]["Hareket"].ToString();
                    dr["Aciklama"] = FisDetay.Rows[i]["Aciklama"];
                    dr["Adres"] = FisDetay.Rows[i]["Adres"];
                    dr["FaturaNo"] = FisDetay.Rows[i]["FaturaNo"];
                    dr["Borc"] = FisDetay.Rows[i]["Borc"];
                    dr["Alacak"] = FisDetay.Rows[i]["Alacak"].ToString();
                    dr["Bakiye"] = bakiye;
                    dtSanal.Rows.Add(dr);
                }

                dtSanal.TableName = "FisDetay";
                ds.Tables.Add(dtSanal);
                //Firma Bilgileri
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                ////kasahareketleri
                //string sql2 = "";

                ////if (cbTarihAraligi2.SelectedIndex == 0)
                //    sql2 +=  @"select top 10 ";
                ////else
                //  //  sql2 += "select ";

                //sql2 += " pkKasaHareket,'' as Hareket,Tarih,Borc,Alacak,Aciklama,OdemeSekli,Tutar,AktifHesap,fkTaksitler,fkCek from  KasaHareket kh with(nolock) " +
                //    "where fkSatislar is null and fkFirma =@fkFirma";

                //sql2 = sql2.Replace("@fkFirma", musteriadi.Tag.ToString());

                ////if (cbTarihAraligi2.SelectedIndex > 0)
                //  //  sql2 += " and kh.Tarih>@ilktar and kh.Tarih<@sontar";

                //sql2 += " order by pkKasaHareket desc";

                //ArrayList list = new ArrayList();
                //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
                //list.Add(new SqlParameter("@ilktar", ilktarih2.DateTime));
                //list.Add(new SqlParameter("@sontar", sontarih2.DateTime));

                //DataTable dtkasahareketleri = DB.GetData(sql2, list);
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;

                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriHesapExtresiFatura";
                rapor.Report.Name = "MusteriHesapExtresiFatura";

                if (Disigner)
                    rapor.ShowDesigner();
                else
                {
                    string pdfdosyaadi = Application.StartupPath + "\\MusteriHesapExtresiFatura" + musteriadi.Tag.ToString() + ".pdf";
                    if (eposta == "")
                        rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
                    else
                    {
                        rapor.ExportToPdf(pdfdosyaadi);

                        string b = DB.epostagonder(eposta, "Müşteri Fatura Hesap Extresi", pdfdosyaadi, musteriadi.Text + " Hesap Extresi");
                        if (b == "OK")
                        {
                            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);

                            if (File.Exists(pdfdosyaadi))
                                File.Delete(pdfdosyaadi);
                        }
                        else
                            formislemleri.Mesajform(b, "K", 200);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void FisPdf2(string PdfDosyasi, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec sp_MusteriHesapExresi @fkFirma,'@ilktar','@sontar'";
            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(sql, list);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                DataTable dtSanal = new DataTable();
                //dtSanal.Columns.Add(new DataColumn("FisNo", typeof(int)));
                dtSanal.Columns.Add(new DataColumn("Tarih", typeof(DateTime)));
                dtSanal.Columns.Add(new DataColumn("Hareket", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("OdemeSekli", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Aciklama", typeof(string)));
                dtSanal.Columns.Add(new DataColumn("Borc", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Alacak", typeof(decimal)));
                dtSanal.Columns.Add(new DataColumn("Bakiye", typeof(decimal)));
                decimal bakiye = 0;
                for (int i = 0; i < FisDetay.Rows.Count; i++)
                {
                    decimal borc = 0, alacak = 0;
                    if (FisDetay.Rows[i]["Borc"].ToString() != "0,00")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["Borc"].ToString());
                        bakiye = bakiye - borc;
                    }
                    if (FisDetay.Rows[i]["Alacak"].ToString() != "0,00")
                    {
                        alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                        bakiye = bakiye + alacak;
                    }
                    DataRow dr;
                    dr = dtSanal.NewRow();
                    //dr["FisNo"] = FisDetay.Rows[i]["FisNo"];
                    dr["Tarih"] = FisDetay.Rows[i]["Tarih"];
                    dr["Hareket"] = FisDetay.Rows[i]["Hareket"].ToString();
                    dr["Aciklama"] = FisDetay.Rows[i]["Aciklama"];
                    dr["OdemeSekli"] = FisDetay.Rows[i]["OdemeSekli"];
                    dr["Borc"] = FisDetay.Rows[i]["Borc"];
                    dr["Alacak"] = FisDetay.Rows[i]["Alacak"].ToString();
                    dr["Bakiye"] = bakiye;
                    dtSanal.Rows.Add(dr);
                }
                dtSanal.TableName = "FisDetay";
                ds.Tables.Add(dtSanal);
                //Firma Bilgileri
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);
                ////kasahareketleri
                //DataTable dtkasahareketleri = DB.GetData(@"select Tarih,OdemeSekli,Borc,Alacak,Tutar  from KasaHareket WHERE fkFirma=" + musteriadi.Tag.ToString());
                //dtkasahareketleri.TableName = "kasahareketleri";
                //ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;

                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriHesapExtresi2";
                rapor.Report.Name = "MusteriHesapExtresi2";
                rapor.ExportToPdf(PdfDosyasi);
                //if (Disigner)
                //    rapor.ShowDesigner();
                //else
                //    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void FisYazdir3(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "";
                      
            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                string fkFirma = musteriadi.Tag.ToString();

                //0.Satışlar
                sql = "select * from Satislar s with(nolock)  where Siparis=1 and fkSatisDurumu not in(10,1,11) and s.fkFirma=" +
                    fkFirma + " and GuncellemeTarihi>='@ilktar' and GuncellemeTarihi<='@sontar' order by pkSatislar desc";

                sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
                sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

                DataTable Fisler = DB.GetData(sql, list);
                Fisler.TableName = "Fisler";
                ds.Tables.Add(Fisler);

                //1.Satış detay
                sql = @"select sd.fkSatislar,sk.Stokadi,sd.SatisFiyati,sd.iskontotutar,sd.Adet,
                sd.Adet*sd.SatisFiyati-sd.iskontotutar as Tutar from Satislar s with(nolock) 
                left join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                where s.Siparis=1 and s.fkSatisDurumu not in(10,1,11) and s.fkFirma=" +
                fkFirma + " and s.GuncellemeTarihi>='@ilktar' and s.GuncellemeTarihi<='@sontar'"; ;

                sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
                sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

                DataTable FisDetay = DB.GetData(sql, list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //2.Kasa Hareketleri
                sql = @"select fkFirma,Tarih,OdemeSekli,Borc,Alacak,Tutar,fkSatislar  from KasaHareket with(nolock) WHERE fkSatislar is not null and fkFirma=" + fkFirma;
                DataTable dtkasahareketleri = DB.GetData(sql);
                dtkasahareketleri.TableName = "KasaHareketleri";
                ds.Tables.Add(dtkasahareketleri);
                
                //3.Firma Bilgileri
                sql = "SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + fkFirma;
                DataTable Musteri = DB.GetData(sql);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);

                //4.Başlık Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);

                //5.Kasa Tahsilatlari
                //sql = @"select pkKasaHareket,fkFirma,Tarih,OdemeSekli,Borc,Alacak,Tutar,fkSatislar  from KasaHareket with(nolock) WHERE fkSatislar is null and fkFirma=" + fkFirma;
                //DataTable dtTahsilat = DB.GetData(sql);
                //dtTahsilat.TableName = "KasaTahsilatlari";
                //ds.Tables.Add(dtTahsilat);

                //Satış1 ile satış detay2 bağlantı join
                DataRelation dRelation1 = new DataRelation("s_sd", ds.Tables[0].Columns["pkSatislar"], ds.Tables[1].Columns["fkSatislar"],false);
                ds.Relations.Add(dRelation1);

                //satış1 ile kasa hareket4
                DataRelation dRelation2 = new DataRelation("s_kh", ds.Tables[0].Columns["pkSatislar"], ds.Tables[2].Columns["fkSatislar"],false);
                ds.Relations.Add(dRelation2);

                //satış1 ile kasa hareket tahsilat5
                //DataRelation dRelation3 = new DataRelation("s_kht", ds.Tables[3].Columns["pkFirma"], ds.Tables[5].Columns["fkFirma"]);
                //ds.Relations.Add(dRelation3);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "MusteriHesapExtresi3";
                //rapor.Report.Name = "MusteriHesapExtresi3";

                /*
                //kasa hareketleri için sub report oluştur
                XRSubreport subreport = new XRSubreport();

                DetailBand detailBand = new DetailBand();
                //rapor.Bands.Add(detailBand);//hata verdiği için kapatıldı
                // Set the subreport's location.
                subreport.Location = new Point(110, 100);
                // Specify a detail report as a report source for the subreport.
                XtraReport detailReport = new XtraReport();
                DataSet ds2 = new DataSet();
                sql = @"select pkKasaHareket,fkFirma,Tarih,OdemeSekli,Borc,Alacak,Tutar,fkSatislar  from KasaHareket with(nolock) WHERE fkSatislar is null and fkFirma=" + fkFirma;
                DataTable dtTahsilat = DB.GetData(sql);
                dtTahsilat.TableName = "KasaTahsilatlari";
                ds2.Tables.Add(dtTahsilat);
                detailReport.DataSource = ds2;
                detailReport.LoadLayout(exedizini + "/Raporlar/MusteriHesapExtresi33.repx");
                subreport.ReportSource = detailReport;
                // Add the subreport to the detail band.
                detailBand.Controls.Add(subreport);
                */
                


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

        void FisPdf(string RaporDosyasi, string pdfDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select s.GuncellemeTarihi as Tarih,
s.pkSatislar as FisNo,sk.Stokadi,sd.Adet,(sd.SatisFiyati-sd.iskontotutar) as SatisFiyati,
sd.iskontotutar,
(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu from Satislar s with(nolock)
inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
left join SatisDurumu sdu with(nolock) on sdu.pkSatisDurumu=s.fkSatisDurumu 
inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
where s.fkFirma=@fkFirma and s.Siparis=1 and fkSatisDurumu not in(1,10,11) and 
s.GuncellemeTarihi between @ilktar and @sontar order by s.pkSatislar,sd.pkSatisDetay", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //kasahareketleri
                DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket with(nolock) where fkSatislar is null and fkFirma="
                    + musteriadi.Tag.ToString() + " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();

                rapor.DataSource = ds;
                //rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "Müşteri Hesap Extresi";
                rapor.Report.Name = "Müşteri Hesap Extresi";
                rapor.ExportToPdf(RaporDosyasi);
                //if (Disigner)
                //    rapor.ShowDesigner();
                //else
                //    rapor.ShowPreview();//.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void SozlesmeYazdir(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "";

            try
            {
                System.Data.DataSet ds = new DataSet("Test");

                string fkFirma = musteriadi.Tag.ToString();

                //0.Satışlar
                sql = "select * from Satislar s with(nolock)  where Siparis=1 and fkSatisDurumu not in(10,1,11) and s.fkFirma=" +
                    fkFirma + " and GuncellemeTarihi>='@ilktar' and GuncellemeTarihi<='@sontar' order by pkSatislar desc";

                sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
                sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

                DataTable Fisler = DB.GetData(sql, list);
                Fisler.TableName = "Fisler";
                ds.Tables.Add(Fisler);

                //1.Satış detay
                sql = @"select sd.fkSatislar,sk.Stokadi,sd.SatisFiyati,sd.iskontotutar,sd.Adet,
                sd.Adet*sd.SatisFiyati-sd.iskontotutar as Tutar from Satislar s with(nolock) 
                left join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
                inner join StokKarti sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti
                where s.Siparis=1 and s.fkSatisDurumu not in(10,1,11) and s.fkFirma=" +
                fkFirma + " and s.GuncellemeTarihi>='@ilktar' and s.GuncellemeTarihi<='@sontar'"; ;

                sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:00"));
                sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

                DataTable FisDetay = DB.GetData(sql, list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //2.Kasa Hareketleri
                sql = @"select fkFirma,Tarih,OdemeSekli,Borc,Alacak,Tutar,fkSatislar  from KasaHareket with(nolock) WHERE fkSatislar is not null and fkFirma=" + fkFirma;
                DataTable dtkasahareketleri = DB.GetData(sql);
                dtkasahareketleri.TableName = "KasaHareketleri";
                ds.Tables.Add(dtkasahareketleri);

                //3.Firma Bilgileri
                sql = "SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + fkFirma;
                DataTable Musteri = DB.GetData(sql);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);

                //4.Başlık Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + ilktarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "-" + sontarih.DateTime.ToString("dd.MM.yyyy HH:mm") + "' as TarihAraligi");
                Baslik.TableName = "Baslik";
                ds.Tables.Add(Baslik);

                //Satış1 ile satış detay2 bağlantı join
                //DataRelation dRelation1 = new DataRelation("s_sd", ds.Tables[0].Columns["pkSatislar"], ds.Tables[1].Columns["fkSatislar"], false);
                //ds.Relations.Add(dRelation1);

                //satış1 ile kasa hareket4
                //DataRelation dRelation2 = new DataRelation("s_kh", ds.Tables[0].Columns["pkSatislar"], ds.Tables[2].Columns["fkSatislar"], false);
                //ds.Relations.Add(dRelation2);

                //Seçilen Taksit Bilgileri
                if (gridView3.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
                   
                    DataTable dtTaksit = DB.GetData(@"SET LANGUAGE Turkish
SELECT *,DATENAME(DAY,ta.tarih) as gun,
DATENAME(MONTH,ta.tarih) AS ayadi,
DATENAME(YEAR,ta.tarih) AS yil,
ta.tarih as TaksitTarihi from Taksit t with(nolock)
left join Taksitler ta with(nolock) on ta.taksit_id = t.taksit_id
where t.taksit_id = " + dr["taksit_id"].ToString());
                    dtTaksit.TableName = "Taksitler";
                    ds.Tables.Add(dtTaksit);
                }
                //6.şirket bilgileri
                DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
                dtSirket.TableName = "Sirket";
                ds.Tables.Add(dtSirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "Sozlesme";

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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }

            FisYazdir(false, DosyaYol);
        }

        private void gridView3_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string okunma = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Durumu"]);

                string odenecek = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Odenecek"]);
                string odenen = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Odenen"]);

                if (okunma.Trim() == "0")
                {
                    e.Appearance.BackColor = Color.GreenYellow;
                    //e.Appearance.BackColor2 = Color.Blue;
                }
                else if (okunma.Trim() == "1")
                {
                    e.Appearance.BackColor = Color.Blue;
                    // e.Appearance.BackColor2 = Color.Red;
                }
                else if (okunma.Trim() == "2")
                {
                    e.Appearance.BackColor = Color.Aqua;
                    // e.Appearance.BackColor2 = Color.Red;
                }
                else if (okunma.Trim() == "3")
                {
                    e.Appearance.BackColor = Color.Red;
                    // e.Appearance.BackColor2 = Color.Red;
                }
                else
                    e.Appearance.BackColor = Color.Aqua;

                if (odenecek != odenen && odenen != "0,00 TL")
                   e.Appearance.BackColor = Color.PaleVioletRed;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();

            vSatislar();
            BakiyeGetir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        void vSatislar()
        {
            string sql = "";
            if (cbTarihAraligi.SelectedIndex == 0)
                sql = sql + @"select top 10 ";
            else
                sql = sql +"select ";

                sql = sql + @"pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
                        sum(sde.Adet*((sde.SatisFiyatiKdvHaric-(sde.SatisFiyatiKdvHaric*sde.iskontoyuzdetutar)/100))) AS TutarKdvHaric,
                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,sum(sde.iskontotutar) as iskontotutar,
                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,
                        s.GuncellemeTarihi,s.BonusTutar,s.Faturalandi,s.FaturaNo,s.irsaliye_tarihi,
                        k.KullaniciAdi
                        from Satislar s with(nolock)
                        left join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu
                        left join  Kullanicilar k with(nolock) on k.pkKullanicilar=s.fkKullanici
                        where s.Siparis=1 and s.fkSatisDurumu not in(1,10,11) and fkFirma=@fkFirma";

              if (cbTarihAraligi.SelectedIndex > 0)
              sql = sql + " and s.GuncellemeTarihi>@ilktar and s.GuncellemeTarihi<@sontar";

              sql = sql + @" group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi,
                        s.BonusTutar,s.Faturalandi,s.FaturaNo,s.irsaliye_tarihi,k.KullaniciAdi
                        order by s.GuncellemeTarihi desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));

            gcSatislar.DataSource =  DB.GetData(sql,list);

            if (cbTarihAraligi.SelectedIndex == 0 && gridView1.DataRowCount > 0)
            {
                DataRow drson = gridView1.GetDataRow(0);
                DataRow drilk = gridView1.GetDataRow(gridView1.DataRowCount-1);
                ilktarih.DateTime = Convert.ToDateTime(drilk["GuncellemeTarihi"].ToString());
                sontarih.DateTime = DateTime.Today.AddDays(100);//Convert.ToDateTime(drson["GuncellemeTarihi"].ToString());
            }
        }

        void vTeklifler()
        {
            string sql = @"select pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,
                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,
                        s.GuncellemeTarihi,s.TeklifTarihi from Satislar s with(nolock)
                        left join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
                        where s.Siparis=1 and TeklifTarihi is not null and fkFirma=@fkFirma
                        and s.GuncellemeTarihi between @ilktar and @sontar
                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,
                        s.GuncellemeTarihi,s.TeklifTarihi";
            if(cbTarihAraligi.SelectedIndex==0)
                sql = @"select top 10 pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
                        sum(sde.Adet*((sde.SatisFiyatiKdvHaric-(sde.SatisFiyatiKdvHaric*sde.iskontoyuzdetutar)/100))) AS TutarKdvHaric,
                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,
                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,
s.GuncellemeTarihi,s.TeklifTarihi from Satislar s with(nolock)
                        left join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
                        where s.Siparis=1 and TeklifTarihi is not null and fkFirma=@fkFirma
                        --and s.GuncellemeTarihi between @ilktar and @sontar
                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi,s.TeklifTarihi            
            order by pkSatislar desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gcTeklifler.DataSource = DB.GetData(sql, list);
        }

        void Odemeler()
        {
            string sql = "";

            if (cbTarihAraligi2.SelectedIndex == 0)
                sql = sql + @"select top 10 ";
            else
                sql = sql + "select ";

            sql += @"pkKasaHareket,'' as Hareket,Tarih,Borc,Alacak,kh.Aciklama,
            OdemeSekli,kh.Tutar,AktifHesap,fkTaksitler,fkCek,
			case when kh.fkBankalar>0 then B.BankaAdi
			else c.BankaAdi end BankaAdi from  KasaHareket kh with(nolock)
            LEFT JOIN Bankalar B with(nolock) ON B.PkBankalar = kh.fkBankalar
			LEFT JOIN Cekler c with(nolock) On c.pkCek=kh.fkCek
            where kh.fkFirma = @fkFirma";

            if (cbTarihAraligi2.SelectedIndex > 0)
                sql = sql + " and kh.Tarih>@ilktar and kh.Tarih<@sontar";

            sql += " order by pkKasaHareket desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih2.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih2.DateTime));

            gcOdemeleri.DataSource = DB.GetData(sql,list);
        }

        void Faturalar()
        {
            string sonfisler = "0";
            if (cbTarihAraligi.SelectedIndex == 0)
                sonfisler = "0";
            else
                sonfisler = "1";

            string sql = "exec hsp_FaturalarKesilenRapor @fkFirma,@ilktar,@sontar,"+sonfisler;
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59")));

            gcKesilenFaturalar.DataSource = DB.GetData(sql, list);
        }

        void BonusHareketleri()
        {
            gridControl4.DataSource =
            DB.GetData("select * from KasaHareket with(nolock) where OdemeSekli='Bonus Ödenen' and fkFirma=" + musteriadi.Tag.ToString());
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xTabSatislar)
                vSatislar();
            else if (xtraTabControl1.SelectedTabPage == xTabSatisDetay)
                SatisDetayMiktarBazinda();
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
                SatisDetayUrunBazinda();
            else if (xtraTabControl1.SelectedTabPage == xTabTeklif)
            {
                vTeklifler();
            }
            else if (xtraTabControl1.SelectedTabPage == xTabBonus)
            {
                BonusHareketleri();
            }
            else if (xtraTabControl1.SelectedTabPage == xTabFaturalar)
            {
               // cbTarihAraligi
                Faturalar();
            }
            else if (xtraTabControl1.SelectedTabPage == xtRandevular)
            {
                string sql = @"select * from Hatirlatma where fkFirma=@fkFirma";
                sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
                gcRandevular.DataSource = DB.GetData(sql);
            }
            else if (xtraTabControl1.SelectedTabPage == xTabOdemeler)
            {
                string sql = @"select pkKasaHareket,Tarih, Aciklama, Borc, Alacak, fkSatislar as FisNo,fkCek,fkTaksitler,OdemeSekli from KasaHareket kh with(nolock)
                        where fkFirma=@fkFirma and kh.Tarih>@ilktar and kh.Tarih<@sontar";

                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
                list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
                list.Add(new SqlParameter("@sontar", sontarih.DateTime));

                gridControl5.DataSource = DB.GetData(sql, list);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = musteriadi.Tag.ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            Odemeler();
            BakiyeGetir();
            Taksitler();
            TaksitBakiyesiHesapla();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.pkFirma.Text = musteriadi.Tag.ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();
            Odemeler();
            BakiyeGetir();
            Taksitler();
            TaksitBakiyesiHesapla();
        }

        void BakiyeGetir()
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + musteriadi.Tag.ToString() + ",0");
            if (dt.Rows.Count == 0)
            {
                satistoplam.Value = 0;
            }
            else
            {
                satistoplam.Value = decimal.Parse(dt.Rows[0][0].ToString());
            }
            //bakiye.Value = (ceDevir.Value + satistoplam.Value) - odemetoplam.Value - kalantaksittoplam.Value;
        }

        void Taksitler()
        {
            string sql = @"SELECT convert(bit,'0') as Sec,T.taksit_id,T.fkSatislar,TL.pkTaksitler, TL.Tarih, 
            TL.Odenecek, TL.Odenen, TL.SiraNo, TL.Aciklama, TL.Odenecek - TL.Odenen AS Kalan,
            TL.OdemeSekli,TL.HesabaGecisTarih,TL.OdendigiTarih,

            case when (TL.Odenecek - TL.Odenen)=0 then '0'
            when (TL.Odenecek - TL.Odenen)<0 then '1'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih>GETDATE() then '2'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih<GETDATE() then '3' 
            else '4' end as Durumu,TL.kayit_tarihi,
            T.fkSatislar FROM  Taksit T with(nolock) 
            left join Taksitler TL with(nolock) on TL.taksit_id = T.taksit_id 
            WHERE  T.fkFirma = " + musteriadi.Tag.ToString();
           
            if (!cbTumTaksitler.Checked)
               sql = sql + " and TL.Odenecek<>TL.Odenen";

            gcTaksitler.DataSource = DB.GetData(sql);

            if (gridView3.DataRowCount > 0)
            {
                if (gridColumn14.SummaryItem.SummaryValue == null) return;

                kalantaksittoplam.EditValue = gridColumn14.SummaryItem.SummaryValue;

                TaksitBakiyesiHesapla();
            }
            else
                kalantaksittoplam.Value = 0;
        }
       
        private void gridView1_StartSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
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

        private void sorguTarihAraligi2(int g1, int m1, int y1, int h1, int min1, int sec1, Boolean p1,
                      int g2, int m2, int y2, int h2, int min2, int sec2, Boolean p2)
        {

            DateTime d1 = DateTime.Now;

            d1 = d1.AddDays(g1);
            d1 = d1.AddMonths(m1);
            d1 = d1.AddYears(y1);
            d1 = d1.AddHours(h1);
            d1 = d1.AddMinutes(min1);
            ilktarih2.DateTime = d1.AddSeconds(sec1);


            DateTime d2 = DateTime.Now;
            d2 = DateTime.Now;
            d2 = d2.AddDays(g2);
            d2 = d2.AddMonths(m2);
            d2 = d2.AddYears(y2);
            d2 = d2.AddHours(h2);
            d2 = d2.AddMinutes(min2);
            sontarih2.DateTime = d2.AddSeconds(sec2);

        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";

            //ilktarih.Enabled = true;
            //sontarih.Enabled = true;
            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// top 10
            {
                //ilktarih.Enabled = false;
                //sontarih.Enabled = false;

                //ilktarih.Properties.DisplayFormat.FormatString = "f";
                //sontarih.Properties.EditFormat.FormatString = "f";
                //ilktarih.Properties.EditFormat.FormatString = "f";
                //sontarih.Properties.DisplayFormat.FormatString = "f";
                //ilktarih.Properties.EditMask = "f";
                //sontarih.Properties.EditMask = "f";

                //sorguTarihAraligi(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                //                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }else  if (cbTarihAraligi.SelectedIndex == 1)// son 30 gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 2)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 3)// Dün
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
            else if (cbTarihAraligi.SelectedIndex == 4)// yarın
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 6)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 7)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 8)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, 0, 0, 0, false,
                                 (-DateTime.Now.Day), 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 9)// bu yıl
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
            else if (cbTarihAraligi.SelectedIndex == 10)
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
                //son ödemeden sonra
            else if (cbTarihAraligi.SelectedIndex == 11)
            {
                ilktarih.Properties.DisplayFormat.FormatString = "G";
                sontarih.Properties.EditFormat.FormatString = "G";
                ilktarih.Properties.EditFormat.FormatString = "G";
                sontarih.Properties.DisplayFormat.FormatString = "G";
                ilktarih.Properties.EditMask = "G";
                sontarih.Properties.EditMask = "G";

                //ilktarih.Properties.DisplayFormat.FormatString = "dddd.MMMM.yyyy HH:mm:ss";

                sontarih.Enabled = true;
                ilktarih.Enabled = true;

                DataTable dt = DB.GetData("select isnull(Max(Tarih),getdate()) as MaxTarih from KasaHareket with(nolock) where fkSatislar is null and fkFirma=" + musteriadi.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Son Ödeme Bulunamadı");
                    return;
                }
                ilktarih.DateTime = Convert.ToDateTime(dt.Rows[0][0].ToString());
                sontarih.DateTime = DateTime.Now.AddYears(1);
            }

            simpleButton3_Click(sender, e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir(true, DosyaYol);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi2.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir2(false, DosyaYol);
        }

        private void hesapExtresiDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi2.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir2(true, DosyaYol);
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + musteriadi.Tag.ToString());

            string eposta = dt.Rows[0]["Eposta"].ToString();

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            
            if (secim == DialogResult.No) return;

            try
            {
                string rapordosyaadi = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi2.repx";
                //string rapordosyaadi = Application.StartupPath + "\\MusteriHesapExtresi2.repx";
                string pdfdosyaadi = Application.StartupPath + "\\MusteriHesapExtresi"+musteriadi.Tag.ToString()+".pdf";

                //FisPdf(pdfdosyaadi, rapordosyaadi);
                FisPdf2(pdfdosyaadi, rapordosyaadi);
                //rapor.FilterString = "[ID]=1";
                //rapor.ExportToPdf(dosyaadi);

                string b =DB.epostagonder(eposta, "Müşteri Hesap Extresi", pdfdosyaadi, musteriadi.Text + " Hesap Extresi");
                if (b == "OK")
                {
                    formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);

                    if (File.Exists(pdfdosyaadi))
                        File.Delete(pdfdosyaadi);
                }
                else
                    formislemleri.Mesajform(b, "K", 200);

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            /*
            if (musteriadi.AccessibleDescription == "")
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                return;
            }

            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            //FisYazdir(false, DosyaYol);
            EPostaliFisYazdir(false, DosyaYol);
             */
        }

        private void frmMusteriHareketleri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void hareketiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string sonuc = formislemleri.MesajBox("Kasa Hareketi Silmek İstediğinize Eminmisiniz", "Sil", 1, 1);

            if (sonuc == "0") return;

          
            
            string fkCek = dr["fkCek"].ToString();
            string fkTaksitler = dr["fkTaksitler"].ToString();
            string pkKasaHareket = dr["pkKasaHareket"].ToString();
            if (pkKasaHareket == "0") return;

            string mesaj = "";
            if (fkCek != "" && fkCek != "0")
                mesaj = "Çek Ödemesi olan Tahsilatı, ";
            if (fkTaksitler != "" && fkTaksitler != "0")
                mesaj = " Taksit Ödemesi olan Tahsilatı, ";
            
                mesaj = mesaj + " Silmek İstediğinize Eminmisiniz?";

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            if (fkCek != "")
                DB.ExecuteSQL("update Cekler set fkCekTuru=0 where pkCek=" + fkCek);

            if (fkTaksitler != "")
            {
                DB.ExecuteSQL("UPDATE Taksitler Set Odenen=0" +
                    ",OdendigiTarih=null" +
                    " where pkTaksitler=" + fkTaksitler);
            }


            string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket;
            DB.ExecuteSQL(sql);

            Odemeler();
            //gelendurum();
            BakiyeGetir();
            Taksitler();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            try
            {
                DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

                if (dr["pkKasaHareket"].ToString() == "0") return;

                frmKasaHareketDuzelt KasaHareketDuzelt = new frmKasaHareketDuzelt();

                KasaHareketDuzelt.pkKasaHareket.Text = dr["pkKasaHareket"].ToString();

                KasaHareketDuzelt.Tag = this.Tag;

                KasaHareketDuzelt.ShowDialog();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            Odemeler();

        }

        private void labelControl5_Click(object sender, EventArgs e)
        {
            DB.PkFirma = int.Parse(musteriadi.Tag.ToString());
            string pkFirma = musteriadi.Tag.ToString();

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma);
            DevirBakisiSatisKaydi.ShowDialog();
            Odemeler();
            BakiyeGetir();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresiFatura.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresiFatura.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdirFatura(false, DosyaYol,"");
        }

       

        private void hesapExtresiTopluFaturaDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\FaturaTopluExresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FaturaTopluExresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(true, DosyaYol);
        }
        void TaksitOde()
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            string pkTaksitler = dr["pkTaksitler"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = musteriadi.Tag.ToString();
            KasaGirisi.pkTaksitler.Text = pkTaksitler;
            KasaGirisi.tEaciklama.Text = dr["Tarih"].ToString() + "-Taksit Ödemesi-" + dr["Odenecek"].ToString();
            //KasaGirisi.ceTutarNakit.EditValue = dr["Odenecek"].ToString();
            decimal kalan = 0;
            decimal.TryParse(dr["Kalan"].ToString(), out kalan);
            KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            Odemeler();
            BakiyeGetir();
            Taksitler();
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
           
            //senet düzenle
            düzenleToolStripMenuItem_Click(sender, e);
        }
        private void gridView7_DoubleClick(object sender, EventArgs e)
        {
            if (gridView7.FocusedRowHandle < 0) return;
            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            string fs = dr["FS"].ToString();
            string fisno = dr["pkFaturalar"].ToString();
            if (fs == "F")
            {
                frmFaturaToplu FaturaToplu = new frmFaturaToplu(musteriadi.Tag.ToString());
                FaturaToplu.Tag = 1;
                FaturaToplu.ShowDialog();
            }
            else
            {
                frmFisSatisGecmis fis = new frmFisSatisGecmis(false);
                fis.fisno.Text = fisno;
                fis.ShowDialog();
            }
            //frmFaturaToplu FaturaToplu = new frmFaturaToplu(DB.PkFirma.ToString());
            //FaturaToplu.ShowDialog();
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            gridView1.ActiveFilterString = "Sec=1";
            
            string pkFaturaToplu="0";

            if (gridView1.DataRowCount > 0)
            {
                DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + musteriadi.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Müşteri Bulunamadı");
                    return;
                }

                string secilenfisler="";
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    secilenfisler=secilenfisler + dr["pkSatislar"].ToString()+",";
                }

                secilenfisler=secilenfisler.Substring(0,secilenfisler.Length-1);

                DataTable dtSatisDetay = DB.GetData("select pkSatisDetay,fkSatislar,fkFaturaToplu from SatisDetay with(nolock) where fkSatislar in(" +
                    secilenfisler + ")");

                for (int j = 0; j < dtSatisDetay.Rows.Count; j++)
			    {
                    if (j % sefadet.Value == 0)
                    {
                        DataTable dtKul = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" + DB.fkKullanicilar);

                        int faturano = 0;
                        int.TryParse(dtKul.Rows[0]["FaturaNo"].ToString(), out faturano);

                        ArrayList list = new ArrayList();
                        
                        list.Add(new SqlParameter("@FaturaNo", (faturano + 1)));
                        list.Add(new SqlParameter("@FaturaTarihi", DateTime.Today));
                        list.Add(new SqlParameter("@Aciklama", dt.Rows[0]["Firmaadi"].ToString()));
                        list.Add(new SqlParameter("@FaturaAdresi", dt.Rows[0]["Adres"].ToString()));
                        list.Add(new SqlParameter("@VergiDairesi", dt.Rows[0]["VergiDairesi"].ToString()));
                        list.Add(new SqlParameter("@VergiNo", dt.Rows[0]["VergiNo"].ToString()));
                        list.Add(new SqlParameter("@Yazdirildi", "0"));
                        list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
                        list.Add(new SqlParameter("@fkSatislar", dtSatisDetay.Rows[j]["fkSatislar"].ToString()));

                        pkFaturaToplu = DB.ExecuteScalarSQL(@"insert into FaturaToplu (fkFirma,FaturaNo,Tarih,FaturaTarihi,Aciklama,FaturaAdresi,
                        VergiDairesi,VergiNo,Yazdirildi,fkSatislar)
                        values(@fkFirma,@FaturaNo,GETDATE(),@FaturaTarihi,@Aciklama,@FaturaAdresi,@VergiDairesi,@VergiNo,@Yazdirildi,@fkSatislar)  
                        SELECT IDENT_CURRENT('FaturaToplu')", list);

                        DB.ExecuteSQL("update Kullanicilar set FaturaNo=FaturaNo+1 where pkKullanicilar=" + DB.fkKullanicilar);
                    }

                    DB.ExecuteSQL("UPDATE SatisDetay Set Stogaisle=1,fkFaturaToplu=" + pkFaturaToplu +
                                     " where pkSatisDetay=" + dtSatisDetay.Rows[j]["pkSatisDetay"].ToString());

                }
            }
            gridView1.ClearColumnsFilter();

            frmFaturaToplu TopluFatura = new frmFaturaToplu(musteriadi.Tag.ToString());
            TopluFatura.Tag = 1;
            //if (pkFaturaToplu!="0")
            //TopluFatura.txtFaturaNo.Tag = pkFaturaToplu;
            TopluFatura.lbMusteri.Text = musteriadi.Text;
            TopluFatura.lbMusteri.Tag = musteriadi.Tag;
            TopluFatura.Show();
            //yenile
            simpleButton3_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string pkTaksitler = dr["pkTaksitler"].ToString();
            string taksit_id = dr["taksit_id"].ToString();

            if (DB.GetData("select * from KasaHareket with(nolock) where fkTaksitler=" + pkTaksitler).Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce, Taksit Ödemesini Siliniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Taksit Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + pkTaksitler);

            DataTable dtTak = DB.GetData("select count(*) from Taksitler with(nolock) taksit_id=" + taksit_id);
            if (dtTak.Rows.Count>0 && dtTak.Rows[0][0].ToString() == "0")
               DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id);

            Taksitler();
        }

        private void ödendiYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string pkTaksitler = dr["pkTaksitler"].ToString();

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Taksit Ödenedi Olarak Değiştirilecek Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("update Taksitler set  Odenen=Odenecek,OdendigiTarih=GETDATE() where pkTaksitler=" + pkTaksitler);

            Taksitler();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            //this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(2, "0");
            SatisGoster.musteriadi.Text = musteriadi.AccessibleName;
            SatisGoster.ShowDialog();
            //this.TopMost = true;

            Taksitler();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            //this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(3, "0");
            SatisGoster.ShowDialog();
            //this.TopMost = true;
        }

        private void satistoplam_EditValueChanged(object sender, EventArgs e)
        {
            ceToplamBakiye.Value = satistoplam.Value;
        }

        private void musteriadi_Click(object sender, EventArgs e)
        {
            DB.PkFirma = int.Parse(musteriadi.Tag.ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(musteriadi.Tag.ToString(), "");
            KurumKarti.ShowDialog();

        }

        private void gridView8_DoubleClick(object sender, EventArgs e)
        {
            if (gridView8.FocusedRowHandle < 0) return;

            DataRow dr = gridView8.GetDataRow(gridView8.FocusedRowHandle);
            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void tümTaksitleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();

            if (sifregir.Girilen.Text != Degerler.OzelSifre) return;

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Taksit Bilgilerini Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            //for (int i = 0; i < gridView3.DataRowCount; i++)
           // {
             //   DataRow dr = gridView3.GetDataRow(i);
              //  string pkTaksitler = dr["pkTaksitler"].ToString();

               // DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + pkTaksitler);
            //}
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            DB.ExecuteSQL("delete from Taksitler where taksit_id=" + dr["taksit_id"].ToString());
            DB.ExecuteSQL("delete from Taksit where taksit_id=" + dr["taksit_id"].ToString());
            Taksitler();
        }

        private void tümSatışlarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Degerler.OzelSifre != "")
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();
                if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            }

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(musteriadi.Text + "\nTüm Satışları Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from KasaHareket where fkSatislar in(select pkSatislar from Satislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            //DB.ExecuteSQL("delete from KasaHareket where fkFirma=" + musteriadi.Tag.ToString());
            DB.ExecuteSQL("delete from SatisDetay where fkSatislar in(select pkSatislar from Satislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            DB.ExecuteSQL("delete from Satislar where fkFirma=" + musteriadi.Tag.ToString());

            vSatislar();
        }

        private void tümDevirBakiyeleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            if (Degerler.OzelSifre != "")
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();
                if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            }

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(musteriadi.Text + "\nTüm Devir Bakiye Düzeltme Hareketlerini Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("delete from KasaHareket where   (OdemeSekli='Kasa Bakiye Düzeltme' or " +
                        " OdemeSekli='Bakiye Düzeltme') and fkFirma=" + musteriadi.Tag.ToString());

            Odemeler();
            BakiyeGetir();
        }

        private void devirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.PkFirma = int.Parse(musteriadi.Tag.ToString());
            string pkFirma = musteriadi.Tag.ToString();

            frmMusteriBakiyeDuzeltme DevirBakisiSatisKaydi = new frmMusteriBakiyeDuzeltme(pkFirma);
            DevirBakisiSatisKaydi.ShowDialog();

            Odemeler();
            BakiyeGetir();
            TaksitBakiyesiHesapla();
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            if (gridView5.FocusedRowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(gridView5.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkSatislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void gcSatislar_Click(object sender, EventArgs e)
        {

        }

        private void makbuzKesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakbuzYazdir(false);
        }
        void MakbuzYazdir_eski_sil(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkKasaHareket = "0", fkFirma = "0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView2.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                    pkKasaHareket = dr["pkKasaHareket"].ToString();
                    fkFirma = dr["fkFirma"].ToString();

                    if (fkFirma == "") fkFirma = "0";
                }

                string sql = "select * from KasaHareket with(nolock) where pkKasaHareket=" + pkKasaHareket;
                //rapor.DataSource = DB.GetData(sql);

                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Makbuz.repx");
                rapor.Name = "Makbuz";
                rapor.Report.Name = "Makbuz";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex.Message);
            }

            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }
        void MakbuzYazdir(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string pkKasaHareket = "0", fkFirma = "0";
                System.Data.DataSet ds = new DataSet("Test");
                if (gridView2.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                    pkKasaHareket = dr["pkKasaHareket"].ToString();
                    fkFirma = musteriadi.Tag.ToString(); //dr["fkFirma"].ToString();
                    if (fkFirma == "") fkFirma = "0";
                }
                string sql = "select * from KasaHareket with(nolock) where pkKasaHareket=" + pkKasaHareket;
                //rapor.DataSource = DB.GetData(sql);

                DataTable FisDetay = DB.GetData(sql);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Makbuz.repx");
                rapor.Name = "Makbuz";
                rapor.Report.Name = "Makbuz";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                //if (cbYazdir.Checked)
                  //  rapor.Print();
                //else
                    rapor.ShowPreview();
            }
        }
        private void faturalanmadıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("UPDATE Satislar Set fkSatisDurumu=2 where pkSatislar=" + dr["pkSatislar"].ToString());
                    DB.ExecuteSQL("UPDATE SatisDetay Set fkFaturaToplu=0 where fkSatislar=" + dr["pkSatislar"].ToString());
                    gridView1.SetFocusedRowCellValue("Satış", "Fatura");
                }
            }
            vSatislar();
        }

        private void faturalandıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("UPDATE Satislar Set fkSatisDurumu=4 where pkSatislar=" + dr["pkSatislar"].ToString());
                    DB.ExecuteSQL("UPDATE SatisDetay Set fkFaturaToplu=-1 where fkSatislar=" + dr["pkSatislar"].ToString());
                    gridView1.SetFocusedRowCellValue("Durumu", "Fatura");
                }
            }

            vSatislar();
        }

        private void açıklamaDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFisAciklama fFisAciklama = new frmFisAciklama();
            fFisAciklama.memoozelnot.Text = dr["Aciklama"].ToString();
            fFisAciklama.ShowDialog();
            //btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            DB.ExecuteSQL("UPDATE Satislar SET Aciklama='" + fFisAciklama.memoozelnot.Text + "' where pkSatislar=" + dr["pkSatislar"].ToString());

            gridView1.SetFocusedRowCellValue("Aciklama", fFisAciklama.memoozelnot.Text);

            fFisAciklama.Dispose();
        }

        private void btnHesapExtre_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında");

            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi3.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi3.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(false, DosyaYol);
        }

        private void taksitEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTaksitGirisi TaksitGirisi = new frmTaksitGirisi(musteriadi.Tag.ToString());

            if (gridView1.FocusedRowHandle >= 0)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                if (dr["Sec"].ToString() == "True")
                {
                    TaksitGirisi.fkSatislar.Text = dr["pkSatislar"].ToString();
                    decimal tt = 0;
                    decimal.TryParse(dr["ToplamTutar"].ToString(), out tt);
                    TaksitGirisi.ToplamTutar.Value = tt;
                }
                else
                    TaksitGirisi.ToplamTutar.Value = ceTaksitBakiyesi.Value;
            }
            else
                TaksitGirisi.ToplamTutar.Value = ceTaksitBakiyesi.Value;

            TaksitGirisi.teMusteri.Tag = musteriadi.Tag.ToString();
            TaksitGirisi.teMusteri.Text = musteriadi.Text;
            
            TaksitGirisi.ShowDialog();

            Taksitler();

            TaksitBakiyesiHesapla();
        }

        private void btnTaksitYazdir_Click(object sender, EventArgs e)
        {
            YazdirTaksitler(false);
        }

        void YazdirTaksitler(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string fkFirma = musteriadi.Tag.ToString();
                System.Data.DataSet ds = new DataSet("Test");
                string sql = @"select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku,Odenecek-Odenen as Kalan  
                   from Taksit T with(nolock)
                   left join Taksitler TL with(nolock) on T.taksit_id=TL.taksit_id
                   where Kaydet=1";

                if (!cbTumTaksitler.Checked)
                   sql = sql+" and Odenecek<>Odenen"; 

                sql = sql + " and T.fkFirma=" + fkFirma;

                DataTable dtTaksitler = DB.GetData(sql);

                dtTaksitler.TableName = "Taksitler";
                ds.Tables.Add(dtTaksitler);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Taksitler.repx");
                rapor.Name = "Taksitler";
                rapor.Report.Name = "Taksitler";
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (cbDirekYazdir.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        private void dizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTaksitler(true);
        }

        void YazdirTaksitlerExtra(bool dizayn)
        {
            XtraReport rapor = new XtraReport();

            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string fkFirma = musteriadi.Tag.ToString();
                System.Data.DataSet ds = new DataSet("Test");

                string sql = @"select *,dbo.fnc_ParayiYaziyaCevir(Odenecek,2) as rakamoku,Odenecek-Odenen as Kalan  
                   from Taksit T with(nolock)
                   left join Taksitler TL with(nolock) on T.taksit_id=TL.taksit_id
                   where Kaydet=1";

                if (!cbTumTaksitler.Checked)
                    sql = sql + " and Odenecek<>Odenen";

                sql = sql + " and T.fkFirma=" + fkFirma;

                DataTable dtTaksitler = DB.GetData(sql);
                dtTaksitler.TableName = "Taksitler";
                ds.Tables.Add(dtTaksitler);
                //
                DataTable Cari = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Cari.TableName = "Cari";
                ds.Tables.Add(Cari);
                //şirket
                DataTable Sirket = DB.GetData("select top 1 * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                rapor.DataSource = ds;
                rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\TaksitlerExtra.repx");
                rapor.Name = "TaksitlerExtra";
                rapor.Report.Name = "TaksitlerExtra";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (dizayn)
                rapor.ShowDesigner();
            else
            {
                if (cbDirekYazdir.Checked)
                    rapor.Print();
                else
                    rapor.ShowPreview();
            }
        }

        private void taksitExtraYazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTaksitlerExtra(false);
        }

        private void taksitlerExtraDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YazdirTaksitlerExtra(true);
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView3.FocusedRowHandle;
            
            if (i < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            
            ftmTaksitKarti taksit = new ftmTaksitKarti(dr["pkTaksitler"].ToString());
            taksit.Text = dr["SiraNo"].ToString() + ". Taksit Bilgisi";
            taksit.ShowDialog();

            Taksitler();

            gridView3.FocusedRowHandle = i;
        }

        void TaksitBakiyesiHesapla()
        {
            if (kalantaksittoplam.EditValue == null)
            {
                labelControl4.Visible = false;
                labelControl6.Visible = false;
                ceTaksitBakiyesi.Visible = false;
                kalantaksittoplam.Visible = false;
                return;
            }
            if (decimal.Parse(kalantaksittoplam.EditValue.ToString()) > 0)
            {
                labelControl4.Visible = true;
                labelControl6.Visible = true;
                ceTaksitBakiyesi.Visible = true;
                kalantaksittoplam.Visible = true;
            }
            else
            {
                labelControl4.Visible = false;
                labelControl6.Visible = false;
                ceTaksitBakiyesi.Visible = false;
                kalantaksittoplam.Visible = false;
            }

            ceTaksitBakiyesi.EditValue = ceToplamBakiye.Value - kalantaksittoplam.Value;
        }
        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xTabCek)
            {
                ceklistesi();
            }
        }

        void ceklistesi()
        {
            gridControl1.DataSource = DB.GetData("select * from Cekler with(nolock) where fkFirma=" + musteriadi.Tag.ToString());
        }

        private void gridView9_DoubleClick(object sender, EventArgs e)
        {
            if (gridView9.FocusedRowHandle < 0) return;
            DataRow dr = gridView9.GetDataRow(gridView9.FocusedRowHandle);
            frmCekGirisi CekGirisi = new frmCekGirisi("0");
            CekGirisi.pkCek.Text = dr["pkCek"].ToString();
            CekGirisi.ShowDialog();
            
            ceklistesi();
        }

        private void cbTumTaksitler_CheckedChanged(object sender, EventArgs e)
        {
            Taksitler();
        }

        private void satisFişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string fkSatislar = dr["fkSatislar"].ToString();
            
            if (fkSatislar == "" || fkSatislar == "0") return;

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.Text = fkSatislar;
            FisNoBilgisi.ShowDialog();
        }

        private void btnOde_Click(object sender, EventArgs e)
        {
            TaksitOde();
        }

        private void kalantaksittoplam_EditValueChanged(object sender, EventArgs e)
        {
            ceTaksitBakiyesi.Value = ceToplamBakiye.Value - kalantaksittoplam.Value;
        }

        private void ceToplamBakiye_EditValueChanged(object sender, EventArgs e)
        {
            if (ceToplamBakiye.EditValue != null)
            {
                decimal bakiye = 0;
                decimal.TryParse(ceToplamBakiye.EditValue.ToString(), out bakiye);
                //if (bakiye > 0)
                //{
                //    ceToplamBakiye.BackColor = System.Drawing.Color.LightGreen;
                //    ceToplamBakiye.ForeColor = System.Drawing.Color.Black;
                //}
                //else 
                if (bakiye < 0)
                {
                    ceToplamBakiye.BackColor = System.Drawing.Color.Red;
                    ceToplamBakiye.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    ceToplamBakiye.BackColor = System.Drawing.Color.LightGreen;
                    ceToplamBakiye.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void gridView6_DoubleClick(object sender, EventArgs e)
        {
            if (gridView6.FocusedRowHandle < 0) return;

            DataRow dr = gridView6.GetDataRow(gridView6.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);

            FisNoBilgisi.fisno.EditValue = dr["fkSatislar"].ToString();

            FisNoBilgisi.ShowDialog();

            BonusHareketleri();
            BakiyeGetir();
        }

        private void hesapExtresiDetaylıDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi3.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi3.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(true, DosyaYol);
        }

        private void sbtnEpostaOzet_Click(object sender, EventArgs e)
        {

            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + musteriadi.Tag.ToString());

            string eposta = dt.Rows[0]["Eposta"].ToString();

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            try
            {
                string rapordosyaadi = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
                //string rapordosyaadi = Application.StartupPath + "\\MusteriHesapExtresi2.repx";
                string pdfdosyaadi = Application.StartupPath + "\\MusteriHesapExtresiFis" + musteriadi.Tag.ToString() + ".pdf";

                //FisPdf(pdfdosyaadi, rapordosyaadi);
                FisYazdirPdf(pdfdosyaadi, rapordosyaadi);
                //rapor.FilterString = "[ID]=1";
                //rapor.ExportToPdf(dosyaadi);

                string b = DB.epostagonder(eposta, "Müşteri Hesap Extresi", pdfdosyaadi, musteriadi.Text+ " Hesap Extresi");
                if (b == "OK")
                {
                    formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);

                    if (File.Exists(pdfdosyaadi))
                        File.Delete(pdfdosyaadi);
                }
                else
                    formislemleri.Mesajform(b, "K", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            /*
            if (musteriadi.AccessibleDescription == "")
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                return;
            }

            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            //FisYazdir(false, DosyaYol);
            EPostaliFisYazdir(false, DosyaYol);
             */
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            TaksitOde();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\Sozlesme.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("Sozlesme.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SozlesmeYazdir(false, DosyaYol);
        }

        private void müşteriSözleşmesiYazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\Sozlesme.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("Sozlesme.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SozlesmeYazdir(true, DosyaYol);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {

        }

        private void ePostaGönderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fkfirma = "0", FisNo = "0", fkSatisDurumu = "0", eposta = "@";

            if (gridView8.FocusedRowHandle < 0) return;

            DataRow dr = gridView8.GetDataRow(gridView8.FocusedRowHandle);

            fkfirma = musteriadi.Tag.ToString();//dr["fkFirma"].ToString();
            FisNo = dr["pkSatislar"].ToString();
            fkSatisDurumu = "1";// dr["Durumu"].ToString();
            //if (fkSatisDurumu == "Teklif") fkSatisDurumu = "1";

            DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkfirma);
            //DataTable dtFirma = DB.GetData("select * From Firmalar with(nolock) where pkFirma=" + fkfirma);
            eposta = Musteri.Rows[0]["eposta"].ToString();


            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir.GirilenCaption.Text = "E-Posta Adresi Giriniz";
            sifregir.Girilen.Text = eposta;

            sifregir.ShowDialog();
            eposta = sifregir.Girilen.Text;

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            #region  yazıcı Seçimi
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + fkSatisDurumu); //+ lueSatisTipi.EditValue.ToString());

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                //short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            //else if (dtYazicilar.Rows.Count > 1)
            //{
            //    short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

            //    frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, 1);//int.Parse(lueSatisTipi.EditValue.ToString()));

            //    YaziciAyarlari.ShowDialog();

            //    YaziciAyarlari.Tag = 0;
            //    YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

            //    if (YaziciAyarlari.YaziciAdi.Tag == null)
            //        YaziciAdi = "";
            //    else
            //        YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
            //    YaziciAyarlari.Dispose();
            //}
            #endregion

            if (YaziciAdi == "")
            {
                MessageBox.Show("Yazıcı Bulunamadı");
                return;
            }
            // else
            //FisYazdir(dizayner, pkSatisBarkod.Text, YaziciDosyasi, YaziciAdi);

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\" + YaziciDosyasi + ".repx");
            rapor.Name = "Teklif";
            rapor.Report.Name = "Teklif.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + FisNo + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + FisNo);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + FisNo);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                //DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);


                string dosyaadi = Application.StartupPath + "\\" + YaziciDosyasi + ".pdf";

                rapor.DataSource = ds;
                //rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(eposta, "Teklif Listesi", dosyaadi, "Teklif Listesi");

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl2, "A4");
        }

        private void yazdırToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl3, "A4");
        }

        private void yazdırToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gcSatislar, "A4");
        }

        private void ilktarih_Enter(object sender, EventArgs e)
        {
            ilktarih.Tag = "1";
        }

        private void sontarih_Enter(object sender, EventArgs e)
        {
            sontarih.Tag = "1";
        }

        private void sontarih_Leave(object sender, EventArgs e)
        {
            sontarih.Tag = "0";
        }

        private void ilktarih_Leave(object sender, EventArgs e)
        {
            ilktarih.Tag = "0";
        }

        private void sontarih_EditValueChanged(object sender, EventArgs e)
        {
            if (sontarih.Tag.ToString() == "1")
            {
                cbTarihAraligi.SelectedIndex = 10;
                sontarih.Tag = "0";
            }
        }

        private void ilktarih_EditValueChanged(object sender, EventArgs e)
        {
            if (ilktarih.Tag.ToString() == "1")
            {
                cbTarihAraligi.SelectedIndex = 10;
                ilktarih.Tag = "0";
            }
        }

        private void btnOdemeler_Click(object sender, EventArgs e)
        {
            Odemeler();
        }

        private void cbTarihAraligi2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih2.Properties.EditMask = "D";
            sontarih2.Properties.EditMask = "D";
            ilktarih2.Properties.DisplayFormat.FormatString = "D";
            ilktarih2.Properties.EditFormat.FormatString = "D";
            sontarih2.Properties.DisplayFormat.FormatString = "D";
            sontarih2.Properties.EditFormat.FormatString = "D";

            //ilktarih.Enabled = true;
            //sontarih.Enabled = true;
            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi2.SelectedIndex == 0)// top 10
            {
                ilktarih2.DateTime = DateTime.Now;
                sontarih2.DateTime = DateTime.Now;
                //ilktarih.Enabled = false;
                //sontarih.Enabled = false;

                //ilktarih.Properties.DisplayFormat.FormatString = "f";
                //sontarih.Properties.EditFormat.FormatString = "f";
                //ilktarih.Properties.EditFormat.FormatString = "f";
                //sontarih.Properties.DisplayFormat.FormatString = "f";
                //ilktarih.Properties.EditMask = "f";
                //sontarih.Properties.EditMask = "f";

                //sorguTarihAraligi(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                //                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 1)// son 30 gün
            {
                ilktarih2.Properties.DisplayFormat.FormatString = "f";
                sontarih2.Properties.EditFormat.FormatString = "f";
                ilktarih2.Properties.EditFormat.FormatString = "f";
                sontarih2.Properties.DisplayFormat.FormatString = "f";
                ilktarih2.Properties.EditMask = "f";
                sontarih2.Properties.EditMask = "f";

                sorguTarihAraligi2(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 2)// Bu gün
            {
                ilktarih2.Properties.DisplayFormat.FormatString = "f";
                sontarih2.Properties.EditFormat.FormatString = "f";
                ilktarih2.Properties.EditFormat.FormatString = "f";
                sontarih2.Properties.DisplayFormat.FormatString = "f";
                ilktarih2.Properties.EditMask = "f";
                sontarih2.Properties.EditMask = "f";

                sorguTarihAraligi2(0, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 3)// Dün
            {
                ilktarih2.Properties.DisplayFormat.FormatString = "f";
                sontarih2.Properties.EditFormat.FormatString = "f";
                ilktarih2.Properties.EditFormat.FormatString = "f";
                sontarih2.Properties.DisplayFormat.FormatString = "f";
                ilktarih2.Properties.EditMask = "f";
                sontarih2.Properties.EditMask = "f";

                sorguTarihAraligi2(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 4)// yarın
            {
                ilktarih2.Properties.DisplayFormat.FormatString = "f";
                sontarih2.Properties.EditFormat.FormatString = "f";
                ilktarih2.Properties.EditFormat.FormatString = "f";
                sontarih2.Properties.DisplayFormat.FormatString = "f";
                ilktarih2.Properties.EditMask = "f";
                sontarih2.Properties.EditMask = "f";

                sorguTarihAraligi2(1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 5)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi2((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi2.SelectedIndex == 6)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi2((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi2.SelectedIndex == 7)// Bu ay
            {
                sorguTarihAraligi2((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 8)// önceki ay
            {
                sorguTarihAraligi2((-DateTime.Now.Day + 1), -1, 0, 0, 0, 0, false,
                                 (-DateTime.Now.Day), 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi2.SelectedIndex == 9)// bu yıl
            {
                sorguTarihAraligi2((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
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
            else if (cbTarihAraligi2.SelectedIndex == 10)
            {
                sontarih2.Enabled = true;
                ilktarih2.Enabled = true;
            }
            //son ödemeden sonra
            else if (cbTarihAraligi2.SelectedIndex == 11)
            {
                ilktarih2.Properties.DisplayFormat.FormatString = "G";
                sontarih2.Properties.EditFormat.FormatString = "G";
                ilktarih2.Properties.EditFormat.FormatString = "G";
                sontarih2.Properties.DisplayFormat.FormatString = "G";
                ilktarih2.Properties.EditMask = "G";
                sontarih2.Properties.EditMask = "G";

                //ilktarih.Properties.DisplayFormat.FormatString = "dddd.MMMM.yyyy HH:mm:ss";

                sontarih2.Enabled = true;
                ilktarih2.Enabled = true;

                DataTable dt = DB.GetData("select isnull(Max(Tarih),getdate()) as MaxTarih from KasaHareket with(nolock) where fkSatislar is null and fkFirma=" + musteriadi.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Son Ödeme Bulunamadı");
                    return;
                }
                ilktarih2.DateTime = Convert.ToDateTime(dt.Rows[0][0].ToString());
                sontarih2.DateTime = DateTime.Now.AddYears(1);
            }

            Odemeler();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriOdemeHareketleriGrid.xml";
            gridView2.SaveLayoutToXml(Dosya);

            //gridView1.OptionsBehavior.AutoPopulateColumns = false;
            //gridView1.OptionsCustomization.AllowColumnMoving = false;
            //gridView1.OptionsCustomization.AllowColumnResizing = false;
            //gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            //gridView1.OptionsCustomization.AllowRowSizing = false;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriOdemeHareketleriGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);

        }

        private void kaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriFisHareketleriGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriFisHareketleriGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = musteriadi.Tag.ToString();
            //KasaGirisi.pkTaksitler.Text = pkTaksitler;
            KasaGirisi.tEaciklama.Text = dr["pkSatislar"].ToString() + "-Fiş Nolu Ödemesi-" + dr["ToplamTutar"].ToString();
            KasaGirisi.ceTutarNakit.EditValue = dr["ToplamTutar"].ToString();
            decimal kalan = 0;
            decimal.TryParse(dr["ToplamTutar"].ToString(), out kalan);
            KasaGirisi.ceTutarNakit.Value = kalan;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            Odemeler();
            BakiyeGetir();
            //Taksitler();
        }

        private void makbuzTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakbuzYazdir(true);
        }

        private void çekBilgisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmCekGirisi cekGirisi = new frmCekGirisi(musteriadi.Tag.ToString());
            cekGirisi.pkCek.Text = dr["fkCek"].ToString();
            cekGirisi.ShowDialog();
        }
        short yazdirmaadedi = 1;
        private void ePostaGönderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string fkfirma = "0", FisNo = "0", fkSatisDurumu = "0", eposta = "@";

            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            fkfirma = musteriadi.Tag.ToString();//dr["fkFirma"].ToString();
            FisNo = dr["pkSatislar"].ToString();
            //fkSatisDurumu = dr["Durumu"].ToString();
            //if (fkSatisDurumu == "Teklif")
                fkSatisDurumu = "2";
            DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkfirma);
            //DataTable dtFirma = DB.GetData("select * From Firmalar with(nolock) where pkFirma=" + fkfirma);
            eposta = Musteri.Rows[0]["eposta"].ToString();


            inputForm sifregir = new inputForm();
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir.GirilenCaption.Text = "E-Posta Adresi Giriniz";
            sifregir.Girilen.Text = eposta;

            sifregir.ShowDialog();
            eposta = sifregir.Girilen.Text;

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            #region  yazıcı Seçimi
            string YaziciAdi = "", YaziciDosyasi = "";

            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock) where Sec=1 and fkSatisDurumu=" + fkSatisDurumu); //+ lueSatisTipi.EditValue.ToString());

            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();

                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1, 2);//int.Parse(lueSatisTipi.EditValue.ToString()));

                YaziciAyarlari.ShowDialog();

                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            #endregion 

            if (YaziciAdi == "")
            {
                MessageBox.Show("Yazıcı Bulunamadı");
                return;
            }
            // else
            //FisYazdir(dizayner, pkSatisBarkod.Text, YaziciDosyasi, YaziciAdi);

            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\" + YaziciDosyasi + ".repx");
            rapor.Name = "Teklif";
            rapor.Report.Name = "Teklif.repx";
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + FisNo + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + FisNo);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + FisNo);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                //DataTable Musteri = DB.GetData("select *,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);


                string dosyaadi = Application.StartupPath + "\\" + YaziciDosyasi + ".pdf";

                rapor.DataSource = ds;
                //rapor.DataSource = gridControl2.DataSource;
                //rapor.FilterString = "[ID]=1";
                rapor.ExportToPdf(dosyaadi);

                DB.epostagonder(eposta, " Fiş Bilgisi", dosyaadi, musteriadi.Text + "Fiş No=" + FisNo);

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void btnFisDuzenle_Click(object sender, EventArgs e)
        {

            
        }

        private void fişDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string satis_id = dr["pkSatislar"].ToString();

            #region düzenle
            DataTable dtSatislar = DB.GetData("select * from Satislar with(nolock) where pkSatislar=" + satis_id);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Fiş Bulunamadı.", "K", 200);
                return;
            }

            string fkSatisDurumu = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            string fkCek = dtSatislar.Rows[0]["fkCek"].ToString();
            string fkFirma = dtSatislar.Rows[0]["fkFirma"].ToString();

            //if (Degerler.fkKullaniciGruplari != "1")
            //{
            //    if (KullaniciAdiSoyadi.Tag.ToString() != DB.fkKullanicilar)
            //    {
            //        DevExpress.XtraEditors.XtraMessageBox.Show("Bu Fişi Düzenleme Yetkiniz Bulunmamaktadır.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        return;
            //    }
            //}


            bool faturasivar = false;
            string mesaj = "";
            if (DB.GetData("select count(*) from SatisDetay with(nolock) where isnull(fkFaturaToplu,0)>0 and fkSatislar=" + satis_id).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Faturası Kesilmiş Hizmetler var! \n";
                //faturasivar = true;
            }
            if (DB.GetData("select count(*) from Taksit with(nolock) where fkSatislar=" + satis_id).Rows[0][0].ToString() != "0")
            {
                mesaj = mesaj + "Satışa Ait Taksit(ler) var! \n";
            }

            mesaj = mesaj + "Fişi Düzeltmek İstediğinize Eminmisiniz. \n Not: Ödemeler ve Stok Adetleri Geri Alınacaktır";

            #region mesaj ver
            //string sec = formislemleri.MesajBox(mesaj, "Satış Sil", 3, 0);
            //if (sec != "1") return;

            //frmMesajBox frmMesaj = new frmMesajBox(1);
            //frmMesaj.label1.Text = mesaj;
            //frmMesaj.label1.BackColor = System.Drawing.Color.GreenYellow;
            //frmMesaj.Show();
            #endregion
            

            DB.ExecuteSQL("UPDATE Satislar SET duzenleme_tarihi=getdate(),Siparis=0 where pkSatislar=" + satis_id);

            //hesaplar ve mevcutları geri alma durumu
            if (fkSatisDurumu != "1" && fkSatisDurumu != "10" && fkSatisDurumu != "11")
            {
                //string fkFirma = groupControl1.Tag.ToString();
                //alacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Alacak=Alacak-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);
                //borç
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);
                //Devir
                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir-" + Satis4Toplam.Text.ToString().Replace(",", ".") + " where pkFirma=" + fkFirma);

                bonusdus(satis_id);

                MevcutSatisGeriAl(satis_id);

                MevcutDepoGeriAl(satis_id);
            }

            //kasa hareketlerini sil
            DB.ExecuteSQL("DELETE FROM KasaHareket where fkSatislar=" + satis_id);
            //fatura altı iskontoları sil
            DB.ExecuteSQL("update SatisDetay set Faturaiskonto=0 where fkSatislar=" + satis_id);
            //taksitler
            DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + satis_id + ")");
            DB.ExecuteSQL("delete from Taksit where fkSatislar=" + satis_id);
            DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkSatislar=" + satis_id);

            DB.ExecuteSQL("delete from FaturaToplu " +
              " where pkFaturaToplu in(select fkFaturaToplu from SatisDetay where fkSatislar=" + satis_id + ")");

            DB.ExecuteSQL("update SatisDetay set fatura_durumu=null,fkFaturaToplu=null where fkSatislar=" + satis_id);

            #region çek durumunu değiştir

            if (fkCek != "")
            {
                DB.ExecuteSQL("update Cekler set fkCekTuru=0,fkFirma=0 where pkCek=" + fkCek);
                DB.ExecuteSQL("delete from HatirlatmaAnimsat where fkCek=" + fkCek);
            }
            //çeki verdiği için çek iade yapması gerekiyor
            //DataTable dt =
            //DB.GetData("select fkCek from Satislar with(nolock) where pkSatislar=" + fisno.Text);

            //DB.ExecuteSQL("update Cekler set fkCekTuru=10,fkFirma=0 where pkCek=" +
            //    dt.Rows[0]["fkCek"].ToString());
            #endregion

            //FisDuzenle = true;
            //this.btnFisDuzenle.Tag = "1";
            //Close();
            #endregion


            string musterino = musteriadi.Tag.ToString();
            frmSatisYeni satisduzen = new frmSatisYeni(musterino, satis_id);
            satisduzen.ShowDialog();

            vSatislar();
            Odemeler();
            BakiyeGetir();
            Taksitler();
            TaksitBakiyesiHesapla();
        }

        void bonusdus(string satis_id)
        {
            string sqlbonus = "";
            sqlbonus = "update Firmalar set Bonus=Bonus-s.BonusTutar From Satislar s with(nolock) " +
                     " where  Firmalar.pkFirma=s.fkFirma and s.pkSatislar=" + satis_id;
            DB.ExecuteSQL(sqlbonus);

            //bonus ile ödeme alınandan sil
            sqlbonus = @"update Firmalar set Firmalar.Bonus=Firmalar.Bonus+BonusKullanilan.Bonus from BonusKullanilan
            where  Firmalar.pkFirma=BonusKullanilan.fkFirma and BonusKullanilan.fkSatislar=@pkSatislar";
            sqlbonus = sqlbonus.Replace("@pkSatislar", satis_id);
            DB.ExecuteSQL(sqlbonus);
            //sil
            DB.ExecuteSQL("delete from BonusKullanilan where fkSatislar=" + satis_id);
        }

        void MevcutSatisGeriAl(string satis_id)
        {
            DataTable dt =
            DB.GetData("select * from SatisDetay with(nolock) where fkSatislar=" + satis_id);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //DataRow dr = gridView1.GetDataRow(i);
                string fkStokKarti = dt.Rows[i]["fkStokKarti"].ToString();
                string Adet = dt.Rows[i]["Adet"].ToString();
                decimal miktar = 0;
                decimal.TryParse(Adet, out miktar);
                if (miktar < 0)
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where pkStokKarti=" + fkStokKarti);
                else
                    DB.ExecuteSQL("UPDATE StokKarti SET Mevcut=isnull(Mevcut,0)+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                    " where pkStokKarti=" + fkStokKarti);
            }
        }

        void MevcutDepoGeriAl(string satis_id)
        {
            if (Degerler.DepoKullaniyorum)
            {

                DataTable dt =
                DB.GetData("select * from SatisDetay with(nolock) where fkSatislar=" + satis_id);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //DataRow dr = gridView1.GetDataRow(i);
                    string fkStokKarti = dt.Rows[i]["fkStokKarti"].ToString();
                    string fkDepolar = dt.Rows[i]["fkDepolar"].ToString();
                    string Adet = dt.Rows[i]["Adet"].ToString();
                    decimal miktar = 0;
                    decimal.TryParse(Adet, out miktar);

                    if (miktar < 0)
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet-" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                    else
                        DB.ExecuteSQL("UPDATE StokKartiDepo SET MevcutAdet=MevcutAdet+" + miktar.ToString().Replace(",", ".").Replace("-", "") +
                        " where fkStokKarti=" + fkStokKarti + " and fkDepolar=" + fkDepolar);
                }
            }
        }

        void TaksitleriSil(string fkSatislar)
        {
            DB.ExecuteSQL("delete from Taksit where fkSatislar=" + fkSatislar);
            DB.ExecuteSQL("delete from Taksitler where taksit_id=(select taksit_id from Taksit where fkSatislar=" + fkSatislar + ")");
        }

        private void ilktarih2_EditValueChanged(object sender, EventArgs e)
        {
            if (ilktarih2.Tag.ToString() == "1")
            {
                cbTarihAraligi2.SelectedIndex = 10;
                ilktarih.Tag = "0";
            }
        }

        private void sontarih2_EditValueChanged(object sender, EventArgs e)
        {
            if (sontarih2.Tag.ToString() == "1")
            {
                cbTarihAraligi2.SelectedIndex = 10;
                sontarih.Tag = "0";
            }
        }

        private void ilktarih2_Enter(object sender, EventArgs e)
        {
            ilktarih2.Tag = "1";
        }

        private void sontarih2_Enter(object sender, EventArgs e)
        {
            sontarih2.Tag = "1";
        }

        private void ilktarih2_Leave(object sender, EventArgs e)
        {
            ilktarih2.Tag = "0";
        }

        private void sontarih2_Leave(object sender, EventArgs e)
        {
            sontarih2.Tag = "0";
        }

        private void faturaDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Faturayı Düzenlemek istediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;


            if (gridView7.FocusedRowHandle < 0) return;

            DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);

            frmFaturaDuzelt fisduzenle = new frmFaturaDuzelt(dr["pkFaturalar"].ToString());
            fisduzenle.ShowDialog();
        }

        private void btnFaturaExtresiGonder_Click(object sender, EventArgs e)
        {

            DataTable dt = DB.GetData("select * from Firmalar with(nolock) where pkFirma=" + musteriadi.Tag.ToString());

            string eposta = dt.Rows[0]["Eposta"].ToString();

            if (eposta.Length < 10) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(eposta + " E-Posta(pdf) Gönderilsin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresiFatura.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresiFatura.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdirFatura(false, DosyaYol, eposta);
        }

        private void hesapExtresiTasarımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresiFatura.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresiFatura.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdirFatura(true, DosyaYol,"");
        }

        private void kaydetToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriFaturaHareketleriGrid.xml";
            gridView7.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\MusteriFaturaHareketleriGrid.xml";
            if (File.Exists(Dosya))
                File.Delete(Dosya);
        }
    }
}
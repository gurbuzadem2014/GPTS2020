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
    public partial class frmCariHareketleri : DevExpress.XtraEditors.XtraForm
    {
        public frmCariHareketleri()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 50;
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
                      StokKarti.Barcode, SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati - SatisDetay.iskontotutar)) AS Tutar,
SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati-(SatisDetay.AlisFiyati + SatisDetay.iskontotutar))) AS Kar
FROM         Satislar INNER JOIN
                      SatisDetay ON Satislar.pkSatislar = SatisDetay.fkSatislar 
INNER JOIN  StokKarti ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti
WHERE     (Satislar.Siparis = 1) AND (Satislar.fkFirma =@fkFirma)
and (Satislar.GuncellemeTarihi between @ilktar and @sontar)
GROUP BY StokKarti.Stokadi, StokKarti.Barcode
ORDER BY StokKarti.Stokadi ";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl2.DataSource = DB.GetData(sql);
        }
        
        void StokListesi2()
        {
            string sql = @"SELECT sum(sd.Adet) as Miktar, sk.Stokadi,sk.Barcode, sd.AlisFiyati, sd.SatisFiyati, sd.iskontotutar, sd.KdvOrani, 
                           sum(sd.Adet * (sd.SatisFiyati - sd.iskontotutar)) AS Tutar,
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
                
        void StokListesi3()
        {
            string sql = @"SELECT Satislar.pkSatislar,SatisDetay.Tarih,SatisDetay.Adet as Miktar, StokKarti.Stokadi, SatisDetay.AlisFiyati, SatisDetay.SatisFiyati, SatisDetay.iskontotutar, SatisDetay.KdvOrani, 
                           StokKarti.Barcode, SatisDetay.Adet * (SatisDetay.SatisFiyati - SatisDetay.iskontotutar) AS Tutar,
                           SatisDetay.Adet * (SatisDetay.SatisFiyati - (SatisDetay.iskontotutar+SatisDetay.AlisFiyati)) as Kar,
                           SDA.aciklama_detay FROM         Satislar with(nolock) 
                        INNER JOIN SatisDetay with(nolock) ON Satislar.pkSatislar = SatisDetay.fkSatislar 
                        INNER JOIN SatisDetayAciklama SDA with(nolock) ON SDA.fkSatisDetay = SatisDetay.pkSatisDetay 
                        INNER JOIN StokKarti with(nolock) ON SatisDetay.fkStokKarti = StokKarti.pkStokKarti
                        WHERE Satislar.Siparis = 1 and fkSatisDurumu not in(1,10,11) AND Satislar.fkFirma =@fkFirma
                        and (Satislar.GuncellemeTarihi>=@ilktar and Satislar.GuncellemeTarihi<=@sontar)
                        ORDER BY StokKarti.Stokadi ";
            //sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl3.DataSource = DB.GetData(sql, list);
        }
        
        private void frmCariHareketlerMusteri_Load(object sender, EventArgs e)
        {
            DB.pkTedarikciler = 0;
            cbTarihAraligi.SelectedIndex = 0;
            DataTable dtp = DB.GetData(@"select pkFirma,OzelKod,Firmaadi,Eposta,convert(decimal(18,2),Bonus) as Bonus,fkTedarikciler from Firmalar with(nolock) where pkFirma=" + 
                musteriadi.Tag.ToString());
           
            if (dtp.Rows.Count == 0)
            {
                MessageBox.Show("Cari Bulunamadı");
                return;
            }
            else
            {
                seFirma_id.Value = int.Parse(dtp.Rows[0]["pkFirma"].ToString());
                musteriadi.AccessibleName = dtp.Rows[0]["Firmaadi"].ToString();
                musteriadi.Text = dtp.Rows[0]["OzelKod"].ToString() + " - " + dtp.Rows[0]["Firmaadi"].ToString();

                musteriadi.AccessibleDescription = dtp.Rows[0]["Eposta"].ToString();
                if (dtp.Rows[0]["fkTedarikciler"].ToString() != "")
                {
                    seTedarikci_id.Value = int.Parse(dtp.Rows[0]["fkTedarikciler"].ToString());
                    this.Text = dtp.Rows[0]["Firmaadi"].ToString()+"/" +"Tedarikçi Adı";
                    musteriadi.Text = dtp.Rows[0]["OzelKod"].ToString() + " - " + dtp.Rows[0]["Firmaadi"].ToString()
                        +"/" + "Tedarikçi Adı";
                }

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
            vSatislar();

            Odemeler();

            BakiyeGetir();
            
            Taksitler();
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
            catch //(Exception ex)
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
                (sd.Adet*(sd.SatisFiyati-sd.iskontotutar)) as Tutar,OdemeSekli,sdu.Durumu,s.Aciklama,s.FaturaNo from Satislar s with(nolock)
                inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar
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

        void FisYazdirFatura(bool Disigner, string RaporDosyasi)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec hsp_FaturalarKesilen @fkFirma,'@ilktar','@sontar'";
            sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));

            //gcKesilenFaturalar.DataSource = DB.GetData(sql);//, list);

            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            //string sql = "exec hsp_FaturalarKesilen @fkFirma,'@ilktar','@sontar'";
            //sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));
            try
            {
                System.Data.DataSet ds = new DataSet("Fatura");
                DataTable FisDetay = DB.GetData(sql); //DB.GetData(sql, list);
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
                    if (FisDetay.Rows[i]["FaturaTutari"].ToString() != "0,00")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["FaturaTutari"].ToString());
                        bakiye = bakiye - borc;
                    }
                    //if (FisDetay.Rows[i]["Alacak"].ToString() != "0,00")
                    //{
                    //    alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                    //    bakiye = bakiye + alacak;
                    //}
                    DataRow dr;
                    dr = dtSanal.NewRow();
                    dr["pkFaturalar"] = FisDetay.Rows[i]["pkFaturalar"];
                    dr["Tarih"] = FisDetay.Rows[i]["Tarih"];
                    dr["FaturaTarihi"] = FisDetay.Rows[i]["FaturaTarihi"];
                    dr["Hareket"] = FisDetay.Rows[i]["Hareket"].ToString();
                    dr["Aciklama"] = FisDetay.Rows[i]["Aciklama"];
                    dr["Adres"] = FisDetay.Rows[i]["Adres"];
                    dr["FaturaNo"] = FisDetay.Rows[i]["FaturaNo"];
                    dr["Borc"] = FisDetay.Rows[i]["FaturaTutari"];
                    dr["Alacak"] = 0;// FisDetay.Rows[i]["Alacak"].ToString();
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

                rapor.Name = "MusteriHesapExtresiFatura";
                rapor.Report.Name = "MusteriHesapExtresiFatura";

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
            FisNoBilgisi.fisno.EditValue = dr["FisNo"].ToString();
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
            string sql = @"select 'Satış' as SA,pkSatislar as FisNo,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,sum(sde.iskontotutar) as iskontotutar,
                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,s.GuncellemeTarihi,s.BonusTutar,s.Faturalandi
                        from Satislar s with(nolock)
                        left join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
                        where s.Siparis=1 and fkSatisDurumu not in(1,10,11) and fkFirma=@fkFirma
                        and s.GuncellemeTarihi>@ilktar and s.GuncellemeTarihi<@sontar
                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi,
                        s.BonusTutar,s.Faturalandi";
            //      order by s.GuncellemeTarihi desc";
            sql = sql + @" union all select 'Alış' as SA,pkAlislar as FisNo,convert(bit,'0') as Sec,a.Tarih,sd.Durumu,a.ToplamTutar,
                        a.AcikHesap,a.Aciklama,a.OdemeSekli,0 as Odenen,a.AcikHesap,a.OdenenKrediKarti,sum(ade.iskontotutar) as iskontotutar,
                        sum(ade.AlisFiyati*ade.Adet) as AlisTutar,sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar)) as SatisTutar,
                        sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar-ade.AlisFiyati)) as Kar,a.GuncellemeTarihi,0,0
                        from Alislar a with(nolock)
                        left join AlisDetay ade with(nolock) on ade.fkAlislar=a.pkAlislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=a.fkSatisDurumu 
                        where a.Siparis=1 and a.fkSatisDurumu not in(1,10,11) and a.fkFirma=@fkTedarikci
                        and a.GuncellemeTarihi>@ilktar and a.GuncellemeTarihi<@sontar
                        group by pkAlislar,a.Tarih,sd.Durumu,a.ToplamTutar,a.AcikHesap,
                        a.Aciklama,a.OdemeSekli,a.AcikHesap,a.OdenenKrediKarti,a.GuncellemeTarihi";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@fkTedarikci", seTedarikci_id.Value.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));

            gcSatislar.DataSource =  DB.GetData(sql,list);
        }

        void vTeklifler()
        {
            string sql = @"select pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,
                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,s.GuncellemeTarihi,s.BonusTutar from Satislar s with(nolock)
                        left join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
                        where s.Siparis=1 and TeklifTarihi is not null and fkFirma=@fkFirma
                        and s.GuncellemeTarihi between @ilktar and @sontar
                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi,s.BonusTutar
                        order by s.GuncellemeTarihi desc";

//            if (cbTarihAraligi.SelectedIndex == 10)
//                sql = @"select pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
//                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,
//                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
//                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,s.GuncellemeTarihi from Satislar s with(nolock)
//                        inner join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
//                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
//                        where AcikHesap>0 and s.Siparis=1 and fkSatisDurumu<>10 and fkFirma=@fkFirma
//                        and s.GuncellemeTarihi between @ilktar and @sontar
//                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
//                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi
//                        order by s.GuncellemeTarihi desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gcTeklifler.DataSource = DB.GetData(sql, list);
        }

        void Odemeler()
        {
            string sql = @"select * from (
                        SELECT pkKasaHareket, fkKasalar,CONVERT(bit,AktifHesap) as AktifHesap,Tarih, Alacak, Borc, fkSatislar, OdemeSekli, fkFirma, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Müşteri Tahsilatı' as Hareket,fkCek,fkTaksitler
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' and Borc>0 and fkSatislar is null and fkFirma =@fkFirma
                        union all
                        SELECT pkKasaHareket, fkKasalar,CONVERT(bit,AktifHesap) as AktifHesap,Tarih, Alacak, Borc, fkSatislar, OdemeSekli, fkFirma, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Müşteriye Ödeme' as Hareket,fkCek,fkTaksitler
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' AND Alacak>0 and fkSatislar is null and fkFirma =@fkFirma
                        union all
                        SELECT pkKasaHareket, fkKasalar,CONVERT(bit,AktifHesap) as AktifHesap,Tarih, Alacak, Borc, fkSatislar, '' as OdemeSekli, fkFirma, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Bakiye Düzelteme' as Hareket,fkCek,fkTaksitler
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli='Bakiye Düzeltme' and fkSatislar is null and fkFirma =@fkFirma
                        union all
                        SELECT 0 as pkKasaHareket, 0 as fkKasalar,CONVERT(bit,'0') as AktifHesap,Min(Tarih) as Tarih, sum(Alacak) as Alacak, sum(Borc) as Borc,
                        0 as fkSatislar,  'Tümü' as OdemeSekli, Max(fkFirma), 
                        0 as fkPersoneller,0 as fkBankalar,'Satış Ödemeleri Toplamı' as Aciklama,sum(Tutar),'Satış Ödemeleri' as Hareket,0,0
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' and fkSatislar is not null and fkFirma =@fkFirma 
                        ) as x
                        order by Tarih desc";

            sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString()); 

            gcOdemeleri.DataSource = DB.GetData(sql);
        }

        void Faturalar()
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec hsp_FaturalarKesilen @fkFirma,'@ilktar','@sontar'";
            sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            sql = sql.Replace("@ilktar", ilktarih.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sql = sql.Replace("@sontar", sontarih.DateTime.ToString("yyyy-MM-dd HH:mm:59"));
            
            gcKesilenFaturalar.DataSource = DB.GetData(sql);//, list);
        }

        void BonusHareketleri()
        {
            gridControl4.DataSource =
            DB.GetData("select * from KasaHareket with(nolock) where OdemeSekli='Bonus Ödenen' and fkFirma=" + musteriadi.Tag.ToString());
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage==xTabSatislarAlislar)
                vSatislar();
            else if (xtraTabControl1.SelectedTabPage == xTabSatisDetay)
                StokListesi2();
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
                StokListesi3();
            else if (xtraTabControl1.SelectedTabPage == xTabTeklif)
                vTeklifler();
            else if (xtraTabControl1.SelectedTabPage == xTabBonus)
            {
                BonusHareketleri();
            }
            else if (xtraTabControl1.SelectedTabPage == xTabFaturalar)
            {
                Faturalar();
            }
            else if (xtraTabControl1.SelectedTabPage == xtRandevular)
            {
                string sql = @"select * from Hatirlatma where fkFirma=@fkFirma";
                sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
                gcRandevular.DataSource = DB.GetData(sql);
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
            TL.Odenecek, TL.Odenen, TL.SiraNo, T.aciklama, TL.Odenecek - TL.Odenen AS Kalan,
            TL.OdemeSekli,TL.HesabaGecisTarih,TL.OdendigiTarih,

            case when (TL.Odenecek - TL.Odenen)=0 then '0'
            when (TL.Odenecek - TL.Odenen)<0 then '1'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih>GETDATE() then '2'
            when (TL.Odenecek - TL.Odenen)>0 and TL.Tarih<GETDATE() then '3' 
            else '4' end as Durumu,
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

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// son 30 gün
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
            else if (cbTarihAraligi.SelectedIndex == 1)// Bu gün
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
            else if (cbTarihAraligi.SelectedIndex == 2)// Dün
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
            else if (cbTarihAraligi.SelectedIndex == 3)// yarın
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
            else if (cbTarihAraligi.SelectedIndex == 4)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, 0, 0, 0, false,
                                   0, 0, 0, 0, 0, 0, false);


            }
            else if (cbTarihAraligi.SelectedIndex == 5)// Bu hafta
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);

                //sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                //                    0, 0, 0, 0, 0, 0, false);

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 7)// önceki ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), -1, 0, 0, 0, 0, false,
                                 (-DateTime.Now.Day), 0, 0, 0, 0, 0, false);



            }
            else if (cbTarihAraligi.SelectedIndex == 8)// bu yıl
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
            else if (cbTarihAraligi.SelectedIndex == 9)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
                //son ödemeden sonra
            else if (cbTarihAraligi.SelectedIndex == 10)
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

                string  b =DB.epostagonder(eposta, "Müşteri Hesap Extresi", pdfdosyaadi, musteriadi.Text + " Hesap Extresi");
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
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
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
            FisYazdirFatura(false, DosyaYol);
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

            if(DB.GetData("select count(*) from Taksitler").Rows[0][0].ToString()=="0")
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

            }
            if (dizayn)
                rapor.ShowDesigner();
            else
                rapor.ShowPreview();
        }

        private void faturalanmadıYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("UPDATE Satislar Set fkSatisDurumu=2 where pkSatislar=" + dr["pkSatislar"].ToString());
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
                rapor.ShowPreview();
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
                rapor.ShowPreview();
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
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresiFatura.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriHesapExtresiFatura.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdirFatura(true, DosyaYol);
        }
    }
}
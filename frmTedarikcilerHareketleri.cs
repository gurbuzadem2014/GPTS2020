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
    public partial class frmTedarikcilerHareketleri : DevExpress.XtraEditors.XtraForm
    {
        public frmTedarikcilerHareketleri()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmCariHareketlerMusteri_Load(object sender, EventArgs e)
        {
            DB.pkTedarikciler = 0;
            cbTarihAraligi.SelectedIndex = 0;
            DataTable dtp = DB.GetData(@"select pkTedarikciler,OzelKod,Firmaadi,Eposta,0 as Bonus from Tedarikciler with(nolock) where pkTedarikciler=" + musteriadi.Tag.ToString());
            if (dtp.Rows.Count == 0)
            {
                MessageBox.Show("Tedarikçi Bulunamadı");
                return;
            }
            else
            {
                musteriadi.Text = dtp.Rows[0]["OzelKod"].ToString() + " - " + dtp.Rows[0]["Firmaadi"].ToString();
                musteriadi.AccessibleDescription = dtp.Rows[0]["Eposta"].ToString();
            }

            //vAlislar();

            Odemeler();

            BakiyeGetir();

        }

        void gelendurum()
        {
            DataTable dt = DB.GetData(@"select isnull(sum(ad.Adet*(ad.SatisFiyati-ad.iskontotutar)),0) as SatisToplam from Alislar a with(nolock)
            inner join AlisDetay ad with(nolock) on ad.fkAlislar=a.pkAlislar
            where a.Siparis=1 and fkSatisDurumu not in(10,1) and s.fkTedarikciler=" + musteriadi.Tag.ToString());
            if (dt.Rows.Count > 0)
                ceAlisToplam.Value = decimal.Parse(dt.Rows[0][0].ToString());
            //ödemeler
            DataTable dto = DB.GetData(@"SELECT isnull(sum(Borc),0) FROM KasaHareket with(nolock) WHERE fkTedarikciler =" + musteriadi.Tag.ToString());
            odemetoplam.Value =  decimal.Parse(dto.Rows[0][0].ToString());
        }
        
        void StokListesi()
        {
            string sql = @"SELECT SUM(SatisDetay.Adet) AS ToplamAdet, StokKarti.Stokadi,sum(iskontotutar) as iskontotutar, 
                      StokKarti.Barcode, SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati - SatisDetay.iskontotutar)) AS Tutar,
SUM(SatisDetay.Adet * (SatisDetay.SatisFiyati-(SatisDetay.AlisFiyati + SatisDetay.iskontotutar))) AS Kar
FROM  Alislarr INNER JOIN
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
            string sql = @"SELECT 0 as pkSatislar, null as Tarih,sum(ad.Adet) as Miktar, sk.Stokadi, ad.AlisFiyati, ad.SatisFiyati, ad.iskontotutar, ad.KdvOrani, 
                           sk.Barcode, sum(ad.Adet * (ad.SatisFiyati - ad.iskontotutar)) AS Tutar,
                           sum(ad.Adet * (ad.SatisFiyati - (ad.iskontotutar+ad.AlisFiyati))) as Kar
                          FROM         Alislar a with(nolock) INNER JOIN
                          AlisDetay  ad with(nolock) ON a.pkAlislar = ad.fkAlislar INNER JOIN
                          StokKarti sk with(nolock) ON ad.fkStokKarti = sk.pkStokKarti
                          WHERE  (a.Siparis = 1) 
                          AND (a.fkFirma =@fkFirma) 
                          and (a.GuncellemeTarihi between @ilktar and @sontar)
                          group by sk.Stokadi, ad.AlisFiyati, ad.SatisFiyati, ad.iskontotutar, ad.KdvOrani,sk.Barcode ";
            //sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString());
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl2.DataSource = DB.GetData(sql, list);
        }
                
        void StokListesi3()
        {
            string sql = @"SELECT a.pkAlislar as pkSatislar,ad.Tarih,ad.Adet as Miktar, sk.Stokadi, ad.AlisFiyati, ad.SatisFiyati, ad.iskontotutar, ad.KdvOrani, 
                           sk.Barcode, ad.Adet * (ad.SatisFiyati - ad.iskontotutar) AS Tutar,
                           ad.Adet * (ad.SatisFiyati - (ad.iskontotutar+ad.AlisFiyati)) as Kar
                          FROM  Alislar a with(nolock) INNER JOIN
                          AlisDetay  ad with(nolock) ON a.pkAlislar = ad.fkAlislar INNER JOIN
                          StokKarti sk with(nolock) ON ad.fkStokKarti = sk.pkStokKarti
                          WHERE  (a.Siparis = 1) AND (a.fkFirma =@fkFirma) 
                          and (a.GuncellemeTarihi between @ilktar and @sontar)
                          ORDER BY sk.Stokadi ";
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
            string sql = "exec sp_TedarikciHesapExresi @fkFirma,'@ilktar','@sontar'";
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
                    if (FisDetay.Rows[i]["Borc"].ToString() != "0,000000")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["Borc"].ToString());
                        bakiye = bakiye + borc;
                    }
                    if (FisDetay.Rows[i]["Alacak"].ToString() != "0,000000")
                    {
                        alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                        bakiye = bakiye - alacak;
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
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_TedarikciBakiyesi(pkTedarikciler) as Bakiye  FROM Tedarikciler with(nolock) WHERE pkTedarikciler=" + musteriadi.Tag.ToString());
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

                //rapor.Name = RaporDosyasi;
                rapor.Report.Name = RaporDosyasi;
                rapor.LoadLayout(RaporDosyasi);
                string reportPath = exedizini + "\\TedarikciExtresi.xls";
                rapor.ExportToXls(reportPath);
                rapor.Dispose();
                DB.epostagonder(musteriadi.AccessibleDescription, "Exreniz", reportPath, "Extreniz...");
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
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime.Date));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"select a.GuncellemeTarihi as Tarih,
a.pkAlislar as FisNo,sk.Stokadi,ad.Adet,(ad.AlisFiyati-((ad.AlisFiyati*ad.iskontoyuzdetutar)/100)) as SatisFiyati,
ad.iskontotutar,
(ad.Adet*(ad.AlisFiyati-ad.iskontotutar)) as Tutare,
(ad.Adet*(ad.AlisFiyati-((ad.AlisFiyati*ad.iskontoyuzdetutar)/100))) as Tutar,
OdemeSekli,adu.Durumu from Alislar a with(nolock)
inner join AlisDetay ad with(nolock) on ad.fkAlislar=a.pkAlislar
left join AlisDurumu adu with(nolock) on adu.pkAlisDurumu=a.fkSatisDurumu 
inner join StokKarti sk with(nolock) on sk.pkStokKarti=ad.fkStokKarti
where a.fkFirma=@fkFirma and a.Siparis=1 and fkSatisDurumu not in(1,10) 
and a.GuncellemeTarihi between @ilktar and @sontar 
order by a.pkAlislar,ad.pkAlisDetay", list);
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                //MÜŞTERİNİN SORUMLU OLDUĞU PERSONEL
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_TedarikciBakiyesi(pkTedarikciler) as Bakiye  FROM Tedarikciler WHERE pkTedarikciler=" + musteriadi.Tag.ToString());
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //kasahareketleri
                DataTable dtkasahareketleri = DB.GetData(@"select top 1 *,Tutar-Borc as OncekiBakiye from KasaHareket where fkAlislar is null and fkTedarikciler="
                    + musteriadi.Tag.ToString()+ " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

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

        void FisYazdir2(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec sp_TedarikciHesapExresi @fkFirma,'@ilktar','@sontar'";
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
                    decimal borc = 0,alacak=0;
                    if (FisDetay.Rows[i]["Borc"].ToString() != "0,000000")
                    {
                        borc = decimal.Parse(FisDetay.Rows[i]["Borc"].ToString());
                        bakiye = bakiye + borc;
                    }
                    if (FisDetay.Rows[i]["Alacak"].ToString() != "0,000000")
                    {
                        alacak = decimal.Parse(FisDetay.Rows[i]["Alacak"].ToString());
                        bakiye = bakiye - alacak;
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
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_TedarikciBakiyesi(pkTedarikciler) as Bakiye  FROM Tedarikciler with(nolock) WHERE pkTedarikciler=" + musteriadi.Tag.ToString());
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
                
                rapor.Name = RaporDosyasi;
                rapor.Report.Name = RaporDosyasi;
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

        void FisYazdir3(bool Disigner, string RaporDosyasi)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = "exec sp_MusteriHesapExresiTopluFatura @fkFirma,'@ilktar','@sontar'";
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
                    dr["Borc"] = FisDetay.Rows[i]["Borc"];
                    dr["Alacak"] = FisDetay.Rows[i]["Alacak"].ToString();
                    dr["Bakiye"] = bakiye;
                    dtSanal.Rows.Add(dr);
                }
                dtSanal.TableName = "FisDetay";
                ds.Tables.Add(dtSanal);
                //Firma Bilgileri
                DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar WHERE pkFirma=" + musteriadi.Tag.ToString());
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
                rapor.Name = RaporDosyasi;
                rapor.Report.Name = RaporDosyasi;
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
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
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
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        void vAlislar()
        {
            string sql = "";
            
            if(cbTarihAraligi.SelectedIndex==0)
                sql = sql + @"select top 10 ";
            else
                sql = sql + @"select ";

            sql=sql + @"pkAlislar,convert(bit,'0') as Sec,a.Tarih,ad.Durumu,a.ToplamTutar,
                        a.AcikHesap,a.Aciklama,a.OdemeSekli,0 as Odenen,a.AcikHesap,a.OdenenNakit,a.OdenenKrediKarti,
sum(ade.Adet*((ade.AlisFiyatiKdvHaric-(ade.AlisFiyatiKdvHaric*ade.iskontoyuzdetutar)/100))) AS TutarKdvHaric,
                        sum(ade.iskontotutar) as iskontotutar,
                        sum(ade.AlisFiyati*ade.Adet) as AlisTutar,sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar)) as SatisTutar,
                        sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar-ade.AlisFiyati)) as Kar,a.GuncellemeTarihi,0 as BonusTutar,
                        FaturaNo,FaturaTarihi 
                        from Alislar a with(nolock)
                        inner join AlisDetay ade with(nolock) on ade.fkAlislar=a.pkAlislar
                        left join AlisDurumu ad with(nolock) on ad.pkAlisDurumu=a.fkSatisDurumu 
                        where a.Siparis=1 and fkSatisDurumu not in(1,9,10,12) and a.fkFirma=@fkFirma";

            if(cbTarihAraligi.SelectedIndex>0)
                sql=sql +" and a.GuncellemeTarihi between @ilktar and @sontar";

                       sql=sql +@" group by pkAlislar,a.Tarih,ad.Durumu,a.ToplamTutar,a.AcikHesap,
                        a.Aciklama,a.OdemeSekli,a.AcikHesap,a.OdenenNakit,a.OdenenKrediKarti,a.GuncellemeTarihi,FaturaNo,FaturaTarihi 
                        order by a.GuncellemeTarihi desc";

//            if (cbTarihAraligi.SelectedIndex==10)
//                 sql = @"select pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
//                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,sum(sde.iskontotutar) as iskontotutar,
//                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
//                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,s.GuncellemeTarihi from Satislar s with(nolock)
//                        inner join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
//                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
//                        where AcikHesap>0 and s.Siparis=1 and fkSatisDurumu not in(1,10) and fkFirma=@fkFirma
//                        and s.GuncellemeTarihi between @ilktar and @sontar
//                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
//                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi
//                        order by s.GuncellemeTarihi desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            
            gcSatislar.DataSource =  DB.GetData(sql,list);

            if (cbTarihAraligi.SelectedIndex == 0 && gridView1.DataRowCount>0)
             {
                 DataRow drson = gridView1.GetDataRow(0);
                 DataRow drilk = gridView1.GetDataRow(gridView1.DataRowCount-1);
                 ilktarih.DateTime = Convert.ToDateTime(drilk["GuncellemeTarihi"].ToString());
                 //sontarih.DateTime = Convert.ToDateTime(drson["GuncellemeTarihi"].ToString());
                 sontarih.DateTime = DateTime.Today.AddDays(100);
            }    
        }

        void vAlislarteklifsiparisSfatura()
        {
            string sql = "";

            if (cbTarihAraligi.SelectedIndex == 0)
                sql = sql + @"select top 10 ";
            else
                sql = sql + @"select ";

            sql = sql + @"pkAlislar,convert(bit,'0') as Sec,a.Tarih,ad.Durumu,a.ToplamTutar,
                        a.AcikHesap,a.Aciklama,a.OdemeSekli,0 as Odenen,a.AcikHesap,a.OdenenNakit,a.OdenenKrediKarti,
sum(ade.Adet*((ade.AlisFiyatiKdvHaric-(ade.AlisFiyatiKdvHaric*ade.iskontoyuzdetutar)/100))) AS TutarKdvHaric,
                        sum(ade.iskontotutar) as iskontotutar,
                        sum(ade.AlisFiyati*ade.Adet) as AlisTutar,sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar)) as SatisTutar,
                        sum(ade.Adet*(ade.SatisFiyati-ade.iskontotutar-ade.AlisFiyati)) as Kar,a.GuncellemeTarihi,0 as BonusTutar,
                        FaturaNo,FaturaTarihi 
                        from Alislar a with(nolock)
                        inner join AlisDetay ade with(nolock) on ade.fkAlislar=a.pkAlislar
                        left join AlisDurumu ad with(nolock) on ad.pkAlisDurumu=a.fkSatisDurumu 
                        where a.Siparis=1 and fkSatisDurumu in(1,9,12) and a.fkFirma=@fkFirma";

            if (cbTarihAraligi.SelectedIndex > 0)
                sql = sql + " and a.GuncellemeTarihi between @ilktar and @sontar";

            sql = sql + @" group by pkAlislar,a.Tarih,ad.Durumu,a.ToplamTutar,a.AcikHesap,
                        a.Aciklama,a.OdemeSekli,a.AcikHesap,a.OdenenNakit,a.OdenenKrediKarti,a.GuncellemeTarihi,FaturaNo,FaturaTarihi 
                        order by a.GuncellemeTarihi desc";

            //            if (cbTarihAraligi.SelectedIndex==10)
            //                 sql = @"select pkSatislar,convert(bit,'0') as Sec,s.Tarih,sd.Durumu,s.ToplamTutar,
            //                        s.AcikHesap,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,sum(sde.iskontotutar) as iskontotutar,
            //                        sum(sde.AlisFiyati*sde.Adet) as AlisTutar,sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar)) as SatisTutar,
            //                        sum(sde.Adet*(sde.SatisFiyati-sde.iskontotutar-sde.AlisFiyati)) as Kar,s.GuncellemeTarihi from Satislar s with(nolock)
            //                        inner join SatisDetay sde with(nolock) on sde.fkSatislar=s.pkSatislar
            //                        left join SatisDurumu sd with(nolock) on sd.pkSatisDurumu=s.fkSatisDurumu 
            //                        where AcikHesap>0 and s.Siparis=1 and fkSatisDurumu not in(1,10) and fkFirma=@fkFirma
            //                        and s.GuncellemeTarihi between @ilktar and @sontar
            //                        group by pkSatislar,s.Tarih,sd.Durumu,s.ToplamTutar,s.AcikHesap,
            //                        s.AcikHesapOdenen,s.Aciklama,s.OdemeSekli,s.Odenen,s.AcikHesap,s.OdenenKrediKarti,s.GuncellemeTarihi
            //                        order by s.GuncellemeTarihi desc";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));

            gridControl1.DataSource = DB.GetData(sql, list);

            if (cbTarihAraligi.SelectedIndex == 0 && gridView3.DataRowCount > 0)
            {
                DataRow drson = gridView3.GetDataRow(0);
                DataRow drilk = gridView3.GetDataRow(gridView3.DataRowCount - 1);
                ilktarih.DateTime = Convert.ToDateTime(drilk["GuncellemeTarihi"].ToString());
                //sontarih.DateTime = Convert.ToDateTime(drson["GuncellemeTarihi"].ToString());
                sontarih.DateTime = DateTime.Today.AddDays(100);
            }
        }
        
        void Odemeler()
        {
            string sql = @"select * from (
                        SELECT pkKasaHareket, fkKasalar,AktifHesap,Tarih, Alacak, Borc, fkSatislar, OdemeSekli, fkTedarikciler, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Tedarikçi Tahsilatı' as Hareket
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' and Borc>0 and fkAlislar is null and fkTedarikciler =@fkFirma
                        union all
                        SELECT pkKasaHareket, fkKasalar,AktifHesap,Tarih, Alacak, Borc, fkSatislar, OdemeSekli, fkTedarikciler, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Tedarikçi Ödeme' as Hareket
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' AND Alacak>0 and fkAlislar is null and fkTedarikciler =@fkFirma
                        union all
                        SELECT pkKasaHareket, fkKasalar,AktifHesap,Tarih, Alacak, Borc, fkSatislar, '' as OdemeSekli, fkTedarikciler, 
                        fkPersoneller, fkBankalar,Aciklama,Tutar,'Bakiye Düzelteme' as Hareket
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli='Bakiye Düzeltme' and fkAlislar is null and fkTedarikciler =@fkFirma
                        union all
                        SELECT 0 as pkKasaHareket, 0 as fkKasalar,0 as AktifHesap,Min(Tarih) as Tarih, sum(Alacak) as Alacak, sum(Borc) as Borc,
                        0 as fkSatislar,  'Tümü' as OdemeSekli, Max(fkFirma), 
                        0 as fkPersoneller,0 as fkBankalar,'Alış Ödemeleri Toplamı' as Aciklama,sum(Tutar),'Alış Ödemeleri' as Hareket
                        FROM   KasaHareket with(nolock) WHERE OdemeSekli<>'Bakiye Düzeltme' and fkAlislar is not null and fkTedarikciler =@fkFirma 
                        ) as x
                        order by Tarih desc";

            sql = sql.Replace("@fkFirma", musteriadi.Tag.ToString()); 

            gcOdemeleri.DataSource = DB.GetData(sql);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage==xTabSatislar)
                vAlislar();
            else if (xtraTabControl1.SelectedTabPage == xTabSatisDetay)
                StokListesi2();
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage3)
                StokListesi3();
            else if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                vAlislarteklifsiparisSfatura();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkTedarikci.Text = musteriadi.Tag.ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            Odemeler();
            BakiyeGetir();
        }
        void OdemeYap()
        {
            //if (gridView3.FocusedRowHandle < 0) return;
            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.pkTedarikci.Text = musteriadi.Tag.ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();

            Odemeler();
            BakiyeGetir();
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            OdemeYap();
            Odemeler();
            BakiyeGetir();
        }

        void BakiyeGetir()
        {
            DataTable dt = DB.GetData("select dbo.fon_TedarikciBakiyesi(" + musteriadi.Tag.ToString() + ")");
            if (dt.Rows.Count == 0)
            {
                caSatisBakiye.Value = 0;
            }
            else
            {
                caSatisBakiye.Value = decimal.Parse(dt.Rows[0][0].ToString());
            }
            //bakiye.Value = (ceDevir.Value + satistoplam.Value) - odemetoplam.Value - kalantaksittoplam.Value;
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


            //ilktarih.Enabled = true;
            //sontarih.Enabled = true;
            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// top 10
            {
                //ilktarih.Enabled = false;
                //sontarih.Enabled = false;
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
                sorguTarihAraligi((-ti.Days), 0, 0, 0, 0, 0, false,
                                   0, 0, 0, 0, 0, 0, false);
            }
            else if (cbTarihAraligi.SelectedIndex == 6)// Bu hafta
            {

                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, 0, 0, 0, false,
                                    0, 0, 0, 0, 0, 0, false);

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
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
            else if (cbTarihAraligi.SelectedIndex == 11)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;

                DataTable dt = DB.GetData("select isnull(Max(Tarih),getdate()) as MaxTarih from KasaHareket with(nolock) where fkAlislar is null and fkTedarikciler=" + musteriadi.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Son Ödeme Bulunamadı");
                    return;
                }
                ilktarih.DateTime = Convert.ToDateTime(dt.Rows[0][0].ToString());
                sontarih.DateTime = DateTime.Now;
            }
            simpleButton3_Click(sender, e);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi2.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir2(false, DosyaYol);
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        private void bakiye_EditValueChanged(object sender, EventArgs e)
        {
            if(ceToplamBakiye.EditValue!=null)
            {
                decimal b=0;
                decimal.TryParse(ceToplamBakiye.EditValue.ToString(),out b);
                if (b > 0)
                    ceToplamBakiye.BackColor = System.Drawing.Color.SkyBlue;
                else if (b < 0)
                    ceToplamBakiye.BackColor = System.Drawing.Color.SkyBlue;
                else
                    ceToplamBakiye.BackColor = System.Drawing.Color.SkyBlue;
            }
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (musteriadi.AccessibleDescription == "")
            {
                MessageBox.Show("E-Posta Adresini Kontrol Ediniz");
                return;
            }
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi2.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            //FisYazdir(false, DosyaYol);
            EPostaliFisYazdir(false, DosyaYol);
        }

        private void frmMusteriHareketleri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void hareketiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + dr["pkKasaHareket"].ToString();
            DB.ExecuteSQL(sql);
            Odemeler();
            //gelendurum();
            BakiyeGetir();
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

        private void cbTarihSorgu_CheckedChanged(object sender, EventArgs e)
        {
            pcOdemeTarihAraligi.Visible = !cbTarihSorgu.Checked;
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = musteriadi.Tag.ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
            Odemeler();
            BakiyeGetir();
        }
        private void gridView7_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView7.FocusedRowHandle < 0) return;
            //DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            frmFaturaOzel FaturaOzel = new frmFaturaOzel(musteriadi.Tag.ToString());
            FaturaOzel.ShowDialog();
        }


        private void simpleButton11_Click(object sender, EventArgs e)
        {
            formislemleri.Mesajform("YAPIM AŞAMASINDADIR...", "K", 200);

            return;
            this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(2,"0");
            SatisGoster.ShowDialog();
            this.TopMost = true;
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(3,"0");
            SatisGoster.ShowDialog();
            this.TopMost = true;
        }

        private void satistoplam_EditValueChanged(object sender, EventArgs e)
        {
            ceToplamBakiye.Value = ceAlisToplam.Value;
        }

        private void musteriadi_Click(object sender, EventArgs e)
        {
            DB.pkTedarikciler = int.Parse(musteriadi.Tag.ToString());
            frmTedarikciKarti KurumKarti = new frmTedarikciKarti(musteriadi.Tag.ToString());
            KurumKarti.pkkurum.Text = musteriadi.Tag.ToString();
            KurumKarti.ShowDialog();

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

            DB.ExecuteSQL("delete from KasaHareket where fkAlislar in(select pkAlislar  from Alislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            //DB.ExecuteSQL("delete from KasaHareket where fkFirma=" + musteriadi.Tag.ToString());
            DB.ExecuteSQL("delete from AlisDetay where fkAlislar in(select pkAlislar  from Alislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            DB.ExecuteSQL("delete from Alislar where fkTedarikciler=" + musteriadi.Tag.ToString());

            vAlislar();
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
                        " OdemeSekli='Bakiye Düzeltme') and fkTedarikciler=" + musteriadi.Tag.ToString());

            Odemeler();
            BakiyeGetir();
        }

        private void devirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTedarikciBakiyeDuzeltme DevirBakisiSatisKaydi = new frmTedarikciBakiyeDuzeltme();
            DB.pkTedarikciler = int.Parse(musteriadi.Tag.ToString());
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

        private void caSatisBakiye_EditValueChanged(object sender, EventArgs e)
        {
            ceGenelBakiye.EditValue = caSatisBakiye.EditValue;
        }

        private void fişBazındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir(true, DosyaYol);
        }

        private void hesapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi2.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir2(true, DosyaYol);
        }

        private void hesapListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\TedarikciHesapExtresi3.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("TedarikciHesapExtresi3.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir2(true, DosyaYol);
        }

        private void ilktarih_Enter(object sender, EventArgs e)
        {
            ilktarih.Tag = "1";
        }

        private void sontarih_Enter(object sender, EventArgs e)
        {
            sontarih.Tag = "1";
        }

        private void ilktarih_Leave(object sender, EventArgs e)
        {
            ilktarih.Tag = "0";
        }

        private void sontarih_Leave(object sender, EventArgs e)
        {
            sontarih.Tag = "0";
        }

        private void ilktarih_EditValueChanged(object sender, EventArgs e)
        {
            if (ilktarih.Tag.ToString() == "1")
                cbTarihAraligi.SelectedIndex = 10;
        }

        private void sontarih_EditValueChanged(object sender, EventArgs e)
        {
            if (sontarih.Tag.ToString() == "1")
                cbTarihAraligi.SelectedIndex = 10;
        }

        private void gridView3_DoubleClick_1(object sender, EventArgs e)
        {
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            frmFisAlisGecmis FisNoBilgisi = new frmFisAlisGecmis(false);
            FisNoBilgisi.fisno.EditValue = dr["pkAlislar"].ToString();
            FisNoBilgisi.ShowDialog();
        }
    }
}
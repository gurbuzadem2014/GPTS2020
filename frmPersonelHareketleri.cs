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
    public partial class frmPersonelHareketleri : DevExpress.XtraEditors.XtraForm
    {
        string persone_id = "0";
        public frmPersonelHareketleri(string id)
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 50;
            persone_id = id;
            musteriadi.Tag = id;
        }        
        private void frmCariHareketlerMusteri_Load(object sender, EventArgs e)
        {
            DB.pkTedarikciler = 0;
            cbTarihAraligi.SelectedIndex = 0;
            DataTable dtp = DB.GetData(@"select * from Personeller with(nolock) where pkPersoneller="+ musteriadi.Tag.ToString());
            if (dtp.Rows.Count == 0)
            {
                MessageBox.Show("Personel Bulunamadı");
                return;
            }
            else
            {
                musteriadi.AccessibleName = dtp.Rows[0]["adi"].ToString()+" " + dtp.Rows[0]["soyadi"].ToString();
                musteriadi.Text = dtp.Rows[0]["pkpersoneller"].ToString() + " - " + dtp.Rows[0]["adi"].ToString() + " " + dtp.Rows[0]["soyadi"].ToString();
                musteriadi.AccessibleDescription = dtp.Rows[0]["eposta"].ToString();
            }
            //string sql = @"exec sp_MusteriSatisListesi " + musteriadi.Tag.ToString();
            //gridControl1.DataSource = DB.GetData(sql);

            Avanslar();

            Odemeler();

            BakiyeGetir();
            
            //Taksitler();
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
                rapor.DataSource = gcAvanslar.DataSource;//DB.GetData(sql);
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
                    + musteriadi.Tag.ToString()+ " order by pkKasaHareket DESC");
                dtkasahareketleri.TableName = "kasahareketleri";
                ds.Tables.Add(dtkasahareketleri);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                xrCariHareket rapor = new xrCariHareket();
               
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = "Müşteri Hesap Extresi";
                rapor.Report.Name = "Müşteri Hesap Extresi";
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

        void FisYazdir3(bool Disigner, string RaporDosyasi)
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
                rapor.Name = "MusteriHesapExtresi3";
                rapor.Report.Name = "MusteriHesapExtresi3";

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
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            //FisYazdir(false, DosyaYol);
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
          
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        void Avanslar()
        {
            string sql = @"select * from KasaHareket with(nolock) where Modul=3 and fkPersoneller=@fkPersoneller";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", persone_id));
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gcAvanslar.DataSource =  DB.GetData(sql,list);
        }

        void Maaslar()
        {
            string sql = @"select * from KasaHareket with(nolock) where Modul=5 and fkPersoneller=@fkPersoneller";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", persone_id));
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gridControl2.DataSource = DB.GetData(sql, list);
        }

        void Odemeler()
        {
            string sql = @"select * from KasaHareket with(nolock) where fkPersoneller=@fkPersoneller";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", persone_id));
            //list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            //list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            gcOdemeleri.DataSource = DB.GetData(sql, list);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage==xTabSatislar)
                Avanslar();
            else if (xtraTabControl1.SelectedTabPage == xTabSatisDetay)
                Maaslar();
            else if (xtraTabControl1.SelectedTabPage == xtraTabIzinler)
                PersonelIzinleriListesi();
        }

        void PersonelIzinleriListesi()
        {
            gridControlIzın.DataSource = DB.GetData("select * from Personelizinleri with(nolock) where fkPersoneller=" + persone_id);
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag = "1";
            KasaCikis.pkPersonel.Text = musteriadi.Tag.ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Girişi
            KasaCikis.Tag = "1";
            KasaCikis.tEaciklama.Text = "Personelden Ödeme Girişi";
            KasaCikis.ShowDialog();

            Odemeler();

            BakiyeGetir();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.Tag = "2";
            KasaCikis.pkPersonel.Text = musteriadi.Tag.ToString();
            KasaCikis.cbHareketTipi.SelectedIndex = 3;
            ///Ödeme Çıkışı
            KasaCikis.Tag = "2";
            KasaCikis.tEaciklama.Text = "Personel Avansı";
            KasaCikis.ShowDialog();

            Odemeler();

            BakiyeGetir();
        }

        void BakiyeGetir()
        {
            DataTable dt = DB.GetData("select dbo.fon_PersonellerBakiyesi(" + musteriadi.Tag.ToString() + ")");
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
            T.fkSatislar
            FROM  Taksit T with(nolock) 
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
            else if (cbTarihAraligi.SelectedIndex == 10)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;

                DataTable dt = DB.GetData("select Max(Tarih) as MaxTarih from KasaHareket with(nolock) where fkSatislar is null and fkFirma=" + musteriadi.Tag.ToString());
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Son Ödeme Bulunamadı");
                    return;
                }
                ilktarih.DateTime=Convert.ToDateTime(dt.Rows[0][0].ToString());
                sontarih.DateTime=DateTime.Now;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            //FisYazdir(true, DosyaYol);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi2.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            //FisYazdir2(false, DosyaYol);
        }

        private void hesapExtresiDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi2.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi2.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            //FisYazdir2(true, DosyaYol);
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton3_Click(sender, e);
        }

        private void ePostaGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            ////FisYazdir(false, DosyaYol);
            //EPostaliFisYazdir(false, DosyaYol);
        }

        private void frmMusteriHareketleri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void hareketiSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView2.FocusedRowHandle < 0) return;
            //DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            //string fkCek = dr["fkCek"].ToString();
            //string fkTaksitler = dr["fkTaksitler"].ToString();
            //string pkKasaHareket = dr["pkKasaHareket"].ToString();
            //if (pkKasaHareket == "0") return;

            //string mesaj = "";
            //if (fkCek != "" && fkCek != "0")
            //    mesaj = "Çek Ödemesi olan Tahsilatı, ";
            //if (fkTaksitler != "" && fkTaksitler != "0")
            //    mesaj = " Taksit Ödemesi olan Tahsilatı, ";
            
            //    mesaj = mesaj + " Silmek İstediğinize Eminmisiniz?";

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(mesaj, DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;

            //if (fkCek != "")
            //    DB.ExecuteSQL("update Cekler set fkCekTuru=0 where pkCek=" + fkCek);

            //if (fkTaksitler != "")
            //{
            //    DB.ExecuteSQL("UPDATE Taksitler Set Odenen=0" +
            //        ",OdendigiTarih=null" +
            //        " where pkTaksitler=" + fkTaksitler);
            //}


            //string sql = "DELETE FROM KasaHareket WHERE pkKasaHareket=" + pkKasaHareket;
            //DB.ExecuteSQL(sql);

            //Odemeler();
            ////gelendurum();
            //BakiyeGetir();
            //Taksitler();
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

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\FaturaTopluExresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("FaturaTopluExresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            FisYazdir3(false, DosyaYol);
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
            //TaksitOde();
        }
        private void gridView7_DoubleClick(object sender, EventArgs e)
        {
            //if (gridView7.FocusedRowHandle < 0) return;
            //DataRow dr = gridView7.GetDataRow(gridView7.FocusedRowHandle);
            //DB.PkFirma = int.Parse(musteriadi.Tag.ToString());
            frmFaturaOzel FaturaOzel = new frmFaturaOzel(musteriadi.Tag.ToString());
            FaturaOzel.ShowDialog();
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

      

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmFaturaOzel FaturaOzel = new frmFaturaOzel(musteriadi.Tag.ToString());
            FaturaOzel.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //if (gridView3.FocusedRowHandle < 0) return;

            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //string pkTaksitler = dr["pkTaksitler"].ToString();
            //string taksit_id = dr["taksit_id"].ToString();

            //if (DB.GetData("select * from KasaHareket with(nolock) where fkTaksitler=" + pkTaksitler).Rows.Count > 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce, Taksit Ödemesini Siliniz!", DB.ProjeAdi, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Taksit Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;

            //DB.ExecuteSQL("delete from Taksitler where pkTaksitler=" + pkTaksitler);

            //if(DB.GetData("select count(*) from Taksitler").Rows[0][0].ToString()=="0")
            //   DB.ExecuteSQL("delete from Taksit where taksit_id=" + taksit_id);

            //Taksitler();
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
            frmUcGoster SatisGoster = new frmUcGoster(2,"0");
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
            DB.pkPersoneller = int.Parse(musteriadi.Tag.ToString());
            frmPersonel PersonelKarti = new frmPersonel(musteriadi.Tag.ToString());
            PersonelKarti.ShowDialog();

        }

        private void tümTaksitleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //inputForm sifregir = new inputForm();
            //sifregir.Girilen.PasswordChar = '#';
            //sifregir.ShowDialog();

            //if (sifregir.Girilen.Text != Degerler.OzelSifre) return;

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show("Tüm Taksit Bilgilerini Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;

            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            //DB.ExecuteSQL("delete from Taksitler where taksit_id=" + dr["taksit_id"].ToString());
            //DB.ExecuteSQL("delete from Taksit where taksit_id=" + dr["taksit_id"].ToString());
            //Taksitler();
        }

        private void tümSatışlarıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Degerler.OzelSifre != "")
            //{
            //    inputForm sifregir = new inputForm();
            //    sifregir.Girilen.PasswordChar = '#';
            //    sifregir.ShowDialog();
            //    if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            //}

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(musteriadi.Text + "\nTüm Satışları Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;

            //DB.ExecuteSQL("delete from KasaHareket where fkSatislar in(select pkSatislar from Satislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            ////DB.ExecuteSQL("delete from KasaHareket where fkFirma=" + musteriadi.Tag.ToString());
            //DB.ExecuteSQL("delete from SatisDetay where fkSatislar in(select pkSatislar from Satislar where fkFirma=" + musteriadi.Tag.ToString() + ")");
            //DB.ExecuteSQL("delete from Satislar where fkFirma=" + musteriadi.Tag.ToString());

            //vSatislar();
        }

        private void tümDevirBakiyeleriSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Degerler.OzelSifre != "")
            //{
            //    inputForm sifregir = new inputForm();
            //    sifregir.Girilen.Properties.PasswordChar = '#';
            //    sifregir.ShowDialog();
            //    if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            //}

            //DialogResult secim = DevExpress.XtraEditors.XtraMessageBox.Show(musteriadi.Text + "\nTüm Devir Bakiye Düzeltme Hareketlerini Silmek İstediğinize Eminmisiniz?", DB.ProjeAdi, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (secim == DialogResult.No) return;
            //DB.ExecuteSQL("delete from KasaHareket where   (OdemeSekli='Kasa Bakiye Düzeltme' or " +
            //            " OdemeSekli='Bakiye Düzeltme') and fkFirma=" + musteriadi.Tag.ToString());

            //Odemeler();
            //BakiyeGetir();
        }


        private void gcSatislar_Click(object sender, EventArgs e)
        {

        }

        private void makbuzKesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MakbuzYazdir(false);
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

        private void açıklamaDüzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //frmFisAciklama fFisAciklama = new frmFisAciklama();
            //fFisAciklama.memoozelnot.Text = dr["Aciklama"].ToString();
            //fFisAciklama.ShowDialog();
            ////btnAciklamaGirisi.ToolTip = fFisAciklama.memoozelnot.Text;
            //DB.ExecuteSQL("UPDATE Satislar SET Aciklama='" + fFisAciklama.memoozelnot.Text + "' where pkSatislar=" + dr["pkSatislar"].ToString());

            //gridView1.SetFocusedRowCellValue("Aciklama", fFisAciklama.memoozelnot.Text);

            //fFisAciklama.Dispose();
        }

        private void btnHesapExtre_Click(object sender, EventArgs e)
        {
            //string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriHesapExtresi3.repx";
            //if (!File.Exists(DosyaYol))
            //{
            //    MessageBox.Show("MusteriHesapExtresi3.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
            //    return;
            //}
            //FisYazdir3(false, DosyaYol);
        }

        private void taksitEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmTaksitGirisi TaksitGirisi = new frmTaksitGirisi(musteriadi.Tag.ToString());

            //if (gridView1.FocusedRowHandle >= 0)
            //{
            //    DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //    if (dr["Sec"].ToString() == "True")
            //    {
            //        TaksitGirisi.fkSatislar.Text = dr["pkSatislar"].ToString();
            //        decimal tt = 0;
            //        decimal.TryParse(dr["ToplamTutar"].ToString(), out tt);
            //        TaksitGirisi.ToplamTutar.Value = tt;
            //    }
            //    else
            //        TaksitGirisi.ToplamTutar.Value = ceTaksitBakiyesi.Value;
            //}
            //else
            //    TaksitGirisi.ToplamTutar.Value = ceTaksitBakiyesi.Value;

            //TaksitGirisi.teMusteri.Tag = musteriadi.Tag.ToString();
            //TaksitGirisi.teMusteri.Text = musteriadi.Text;
            
            //TaksitGirisi.ShowDialog();

            //Taksitler();
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
            //int i = gridView3.FocusedRowHandle;
            
            //if (i < 0) return;

            //DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            
            //ftmTaksitKarti taksit = new ftmTaksitKarti(dr["pkTaksitler"].ToString());
            //taksit.Text = dr["SiraNo"].ToString() + ". Taksit Bilgisi";
            //taksit.ShowDialog();

            //Taksitler();

            //gridView3.FocusedRowHandle = i;
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
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmFisSatisGecmis FisNoBilgisi = new frmFisSatisGecmis(false);
            FisNoBilgisi.fisno.Text = dr["fkSatislar"].ToString();
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

        

        private void devirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
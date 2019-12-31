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

namespace GPTS
{
    public partial class frmKasaGunluk : DevExpress.XtraEditors.XtraForm
    {
        public frmKasaGunluk()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        //public static DataTable 

        private DataTable GunSonuDB()
        {
            string sql = @"exec sp_GunSonuRaporu @ilktar,@sontar";
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilktar", ilktarih.DateTime));
            list.Add(new SqlParameter("@sontar", sontarih.DateTime));
            return DB.GetData(sql, list);
        }

        private void ucSatislar_Load(object sender, EventArgs e)
        {
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
        
            deTarih.DateTime = DateTime.Now;

            lUEKullanicilar.Properties.DataSource = DB.GetData("select * from Kullanicilar with(nolock) where eposta is not null");
            lUEKullanicilar.ItemIndex = 0;


            GunlukKasaGetir();
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
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.pkSatislar = int.Parse(dr["pkSatislar"].ToString());
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
            printableLink.Component = gridControl1;
            printableLink.Landscape = true;
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
          //simpleButton1_Click_1(sender,e);
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

        private void btnDevirBakiye_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("fkKasalar", "1"));
            list.Add(new SqlParameter("fkKullanici", DB.fkKullanicilar));
            list.Add(new SqlParameter("Tarih", deTarih.DateTime));
            list.Add(new SqlParameter("KasadakiPara", olmasigereken.Value.ToString().Replace(",", ".")));
            list.Add(new SqlParameter("OlmasiGereken", kasadaki.Value.ToString().Replace(",",".")));
            list.Add(new SqlParameter("Aciklama", txtAciklama.Text));

            DataTable dt = DB.GetData("select * from KasaGunluk with(nolock) where fkKasalar=1 and fkKullanici=" +
                DB.fkKullanicilar + " and Tarih='" + deTarih.DateTime.ToString("yyyy-MM-dd HH:mm")+"'");
            if (dt.Rows.Count == 0)
            {
                DB.ExecuteSQL(@"insert into KasaGunluk (fkKasalar,fkKullanici,Tarih,KasadakiPara,OlmasiGereken,Aciklama) 
            values(@fkKasalar,@fkKullanici,@Tarih,@KasadakiPara,@OlmasiGereken,@Aciklama)", list);
            }
            else
            {
                list.Add(new SqlParameter("pkKasaGunluk", dt.Rows[0]["pkKasaGunluk"]));

                DB.ExecuteSQL(@"update KasaGunluk set KasadakiPara=@KasadakiPara,OlmasiGereken=@OlmasiGereken,Aciklama=@Aciklama
             where pkKasaGunluk=@pkKasaGunluk", list);
            }
            xtraTabControl1.SelectedTabPage = xtraTabPage1;

            deTarih.DateTime = DateTime.Now;
            olmasigereken.Value = 0;
            txtAciklama.Text = "Günlük Kasa Durumu";

            GunlukKasaGetir();
        }

        private void silToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;
            DataRow dr = gridView1.GetDataRow(i);
            string pkKasaGunluk = dr["pkKasaGunluk"].ToString();
            string Tarih = dr["Tarih"].ToString();

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(Tarih + " Tarihli Hareketi Silmek istediğinize Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

            DB.ExecuteSQL("delete from KasaGunluk where pkKasaGunluk=" + pkKasaGunluk);

            GunlukKasaGetir();
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
                rapor.DataSource = gridControl1.DataSource;
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
        void GunlukKasaGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from KasaGunluk with(nolock) where Tarih>getdate()-1");
        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            GunlukKasaGetir();
        }
    }
}

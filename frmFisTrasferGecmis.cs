using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using DevExpress.XtraReports.UI;
using System.IO;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmFisTrasferGecmis : DevExpress.XtraEditors.XtraForm
    {
        bool FisDuzenle = false;
        public frmFisTrasferGecmis(bool _FisDuzenle)
        {
            InitializeComponent();
            FisDuzenle = _FisDuzenle;

            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 40;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 40;
        }

        private void frmFisNoBilgisi_Load(object sender, EventArgs e)
        {
            btnFisDuzenle.Visible = FisDuzenle;

            TransferBilgileri();

            string Dosya = DB.exeDizini + "\\FisTrasferGecmisGrid.xml";

            if (File.Exists(Dosya))
            {
                gridView1.RestoreLayoutFromXml(Dosya);
                gridView1.ActiveFilter.Clear();
            }
            depolar();
            //Yetkiler();
        }
        void depolar()
        {
            repositoryItemLookUpEdit1.DataSource = DB.GetData("select * from Depolar with(nolock)");
            repositoryItemLookUpEdit2.DataSource = DB.GetData("select * from Depolar with(nolock)");
        }
        void Yetkiler()
        {
            string sql = @"SELECT ya.Yetki, p.Aciklama10,ya.Sayi,isnull(p.Aktif,0) as Aktif FROM  YetkiAlanlari ya with(nolock)  
INNER JOIN Parametreler p with(nolock) ON ya.fkParametreler = p.pkParametreler
WHERE ya.fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string aciklama = dtYetkiler.Rows[i]["Aciklama10"].ToString();
                bool yetki = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);
                string sayi = dtYetkiler.Rows[i]["Sayi"].ToString();
                //bool aktif = Convert.ToBoolean(dtYetkiler.Rows[i]["Aktif"]);
            }
        }
        void TransferBilgileri()
        {
            DataTable dtSatislar = DB.GetData("exec hsp_DepoTransfer " + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                //Showmessage("Fiş Bulunamadı", "K");
                return;
            }
            //SatisDurumu.Text = dtSatislar.Rows[0]["SatisDurumu"].ToString();
            //SatisDurumu.Tag = dtSatislar.Rows[0]["fkSatisDurumu"].ToString();
            SatisTarih.Text = dtSatislar.Rows[0]["transfer_tarihi"].ToString();
            KayitTarihi.Text = dtSatislar.Rows[0]["kayit_tarihi"].ToString();
            //KullaniciAdiSoyadi.Tag = dtSatislar.Rows[0]["fkKullanici"].ToString();
            //KullaniciAdiSoyadi.Text = dtSatislar.Rows[0]["KullaniciAdiSoyadi"].ToString();
            
            Aciklama.Text = dtSatislar.Rows[0]["aciklama"].ToString();
            //groupControl1.Tag = dtSatislar.Rows[0]["fkFirma"].ToString();


            //groupControl1.Text = DB.GetData("select OzelKod from Firmalar where pkFirma=" + groupControl1.Tag.ToString()).Rows[0][0].ToString() 
            //+"-" + dtSatislar.Rows[0]["Firmaadi"].ToString();


            if (dtSatislar.Rows[0]["siparis"].ToString() != "True")
            {
                simpleButton3.Enabled = false;
                btnFisDuzenle.Enabled = false;
            }

            gridControl1.DataSource = DB.GetData("exec Hsp_DepoTransferDetay " + fisno.Text + ",0");
        }

        void MevcutDepoMevcutGeriAl()
        {
            int sonuc = 0;

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string fkDepolar = dr["fkDepolar"].ToString();
                string hedef_depo = dr["fkDepolar_alici"].ToString();
                string fkStokKarti = dr["fkStokKarti"].ToString();

                string Adet = dr["Adet"].ToString().Replace(",", ".");

                decimal miktar = 0;
                decimal.TryParse(Adet,out miktar);

                string sql = "";
                //önce kaynak depodan azalacak satış
                sql = "HSP_StokKartiDepo_EkleGuncelle " + fkStokKarti + "," + fkDepolar + "," + miktar*-1 + ",2";
                sonuc = DB.ExecuteSQL(sql);

                //alış gibi hedef depo artacak
                sql = "HSP_StokKartiDepo_EkleGuncelle " + fkStokKarti + "," + hedef_depo + "," + miktar*-1 + ",1";
                sonuc = DB.ExecuteSQL(sql);
                //DB.ExecuteSQL("update StokKarti set Mevcut=dbo.fon_StokMevcut(" + fkStokKarti + ") WHERE pkStokKarti =" + fkStokKarti);
            }
        }


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;
         

            DataTable dtDepoTransfer = DB.GetData("select * from DepoTransfer with(nolock) where pkDepoTransfer=" + fisno.Text);
            if (dtDepoTransfer.Rows.Count == 0)
            {
                formislemleri.Mesajform("Trasfer Fiş Bulunamadı.","K",200);
                return;
            }

            string fkDepolar = dtDepoTransfer.Rows[0]["fkDepolar"].ToString();
            
            string mesaj = "Transfer Fişini Silmek İstediğinize Eminmisiniz.";

            string sec = formislemleri.MesajBox(mesaj, "Trasnfer Fişi Sil", 3, 0);
            if (sec != "1") return;

            MevcutDepoMevcutGeriAl();

            DB.ExecuteSQL("DELETE FROM DepoTransferDetay WHERE fkDepoTransfer=" + fisno.Text);
            DB.ExecuteSQL("DELETE FROM DepoTransfer where pkDepoTransfer=" + fisno.Text);            

            inputForm sifregir1 = new inputForm();
            sifregir1.Text = "Silinme Nedeni";
            sifregir1.GirilenCaption.Text = "Silme Nedenini Giriniz!";
            sifregir1.Girilen.Text = "İptal";
            //sifregir.Girilen.Properties.PasswordChar = '#';
            sifregir1.ShowDialog();

            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            btnFisDuzenle.Tag = "0";
            Close();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            DataTable dtSatislar = DB.GetData("select * from DepoTransfer with(nolock) where pkDepoTransfer=" + fisno.Text);
            if (dtSatislar.Rows.Count == 0)
            {
                formislemleri.Mesajform("Transfer Fiş Bulunamadı.", "K", 200);
                return;
            }

            bool faturasivar = false;
            string mesaj = "Transfer Fişini Düzeltmek İstediğinize Eminmisiniz. \n Not: Stok Adetleri Geri Alınacaktır";
            
            string sec = formislemleri.MesajBox(mesaj, "Transfer Sil", 3, 0);
            if (sec != "1") return;


            DB.ExecuteSQL("UPDATE DepoTransfer SET siparis=0 where pkDepoTransfer=" + fisno.Text);

            //mevcutları geri alma durumu
            MevcutDepoMevcutGeriAl();

            FisDuzenle = true;
            this.btnFisDuzenle.Tag = "1";

            Close();
        }

        void FisYazdir(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                //aynı anda çift ödeme olunca ne olacak
                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli,Tutar from KasaHareket with(nolock) where fkSatislar=" + fisid);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                DataTable Musteri = DB.GetData("select *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  from Firmalar with(nolock) where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }

                xrCariHareket rapor = new xrCariHareket();
                //if (yazdirmaadedi>1)
                //  rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;
                if (Disigner)
                    rapor.ShowDesigner();
                else
                {
                    //if (yazdirmaadedi < 1) yazdirmaadedi = 1;

                    //for (int i = 0; i < yazdirmaadedi; i++)
                        rapor.Print(YaziciAdi);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void FisYazdir1(bool Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
        {
            try
            {
                string fisid = pkSatislar;
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(@"exec sp_SatisDetay " + fisid + ",1");
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);
                DataTable Fis = DB.GetData(@"exec sp_Satislar " + fisid);
                if (Fis.Rows.Count == 0)
                {
                    MessageBox.Show("Satış Bulunamadı");
                    return;
                }
                string fkFirma = Fis.Rows[0]["fkFirma"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select top 1 * from Sirketler");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);
                //Bakiye bilgileri
                DataTable Bakiye = DB.GetData(@"select dbo.fon_MusteriBakiyesi(fkFirma) as Bakiye,ToplamTutar as FisTutar,dbo.fon_MusteriBakiyesi(fkFirma)-ToplamTutar as OncekiBakiye from Satislar
where pkSatislar=" + pkSatislar);
                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);
                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from Firmalar where pkFirma=" + fkFirma);
                Musteri.TableName = "Musteri";
                ds.Tables.Add(Musteri);
                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\" + SatisFisTipi + ".repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("Dosya Bulunamadı");
                    return;
                }
                XtraReport rapor = new XtraReport();
                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                if (Disigner)
                    rapor.ShowDesigner();
                else
                    rapor.Print(YaziciAdi);//.ShowPreview();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string YaziciAdi = "", YaziciDosyasi = "";
            DataTable dtYazicilar =
            DB.GetData("SELECT  YaziciAdi,Dosya  FROM SatisFisiSecimi with(nolock) where Sec=1");
            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                //19.12.2015
                if (SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.Değişim).ToString())
                    SatisDurumu.Tag = ((int)Degerler.SatisDurumlari.Satış).ToString();
                if (SatisDurumu.Tag.ToString() == ((int)Degerler.SatisDurumlari.İade).ToString())
                    SatisDurumu.Tag = ((int)Degerler.SatisDurumlari.Satış).ToString();

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);
                YaziciAyarlari.ShowDialog();
                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            if (YaziciAdi != "")
            {
                FisYazdir(false, fisno.Text, YaziciDosyasi, YaziciAdi);
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (fisno.Text == DB.GetData("select mAX(pkDepoTransfer) from DepoTransfer").Rows[0][0].ToString())
            {
                MessageBox.Show("Son Kayıt");
                return;
            }
            int fno=0;
            int.TryParse(fisno.Text, out fno);
            fno = fno + 1;
            fisno.Text = fno.ToString();
            TransferBilgileri();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (fisno.Text == "1")
            {
                MessageBox.Show("ilk Kayıt");
                return;
            }
            int fno = 0;
            int.TryParse(fisno.Text, out fno);
            fno = fno - 1;
            fisno.Text = fno.ToString();
            TransferBilgileri();
        }

        private void frmFisNoBilgisi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if(e.KeyCode==Keys.F11)
                simpleButton4_Click(sender, e);
        }

        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            Rectangle rect = e.Bounds;
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds);
            Brush brush =
                e.Cache.GetGradientBrush(rect, e.Column.AppearanceHeader.BackColor,
                e.Column.AppearanceHeader.BackColor2, e.Column.AppearanceHeader.GradientMode);
            rect.Inflate(-1, -1);
            // Fill column headers with the specified colors.
            e.Graphics.FillRectangle(brush, rect);
            e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            // Draw the filter and sort buttons.
            foreach (DevExpress.Utils.Drawing.DrawElementInfo info in e.Info.InnerElements)
            {
                try
                {
                    DevExpress.Utils.Drawing.ObjectPainter.DrawObject(e.Cache, info.ElementPainter, info.ElementInfo);
                }
                catch (Exception exp)
                {
                }
            }
            e.Handled = true;
        }


        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void etiketBasımıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            formislemleri.EtiketBas(dr["pkStokKarti"].ToString(),1);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(fisno.Value.ToString());
        }

        private void yazdırDizaynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);
            string pkSatislar = fisno.Value.ToString();
            DB.pkSatislar = int.Parse(pkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();

            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //frmSatisUrunBazindaDetay SatisUrunBazindaDetay = new frmSatisUrunBazindaDetay("0");
            //SatisUrunBazindaDetay.Tag = "2";
            //SatisUrunBazindaDetay.pkStokKarti.Text = dr["fkStokKarti"].ToString();
            //SatisUrunBazindaDetay.ShowDialog();
        }

        private void satışFiyatlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkStokKarti = dr["fkStokKarti"].ToString();
            frmSatisFiyatlari SatisFiyatlari = new frmSatisFiyatlari();
            SatisFiyatlari.Tag = "1";
            SatisFiyatlari.pkStokKarti.Text = fkStokKarti;
            SatisFiyatlari.ShowDialog();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\FisTrasferGecmisGrid.xml";
            gridView1.SaveLayoutToXml(Dosya);
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            string Dosya = DB.exeDizini + "\\FisTrasferGecmisGrid.xml";

            if (File.Exists(Dosya))
            {
                File.Delete(Dosya);
            }

        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = groupControl1.Tag.ToString();
            CariHareketMusteri.Show();
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti(groupControl1.Tag.ToString(), "");
            MusteriKarti.ShowDialog();
        }
    }
}
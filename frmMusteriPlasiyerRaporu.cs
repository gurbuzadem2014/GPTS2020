using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.IO;
using GPTS.islemler;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;

namespace GPTS
{
    public partial class frmMusteriPlasiyerRaporu : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriPlasiyerRaporu()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmKontaklar_Load(object sender, EventArgs e)
        {
            //gridView1.OptionsNavigation.EnterMoveNextColumn = true;

            PersonelGetir();
        }

        void Getir(string fkPerTeslimEden)
        {
            string sql = @"select pkFirma,Firmaadi,OzelKod,Tel,Cep,Yetkili,KaraListe,OdemeGunSayisi,SonOdemeTarihi,SonSatisTarihi,fkPerTeslimEden,Devir
            from Firmalar f with(nolock) where f.Aktif=1 and fkPerTeslimEden=" + fkPerTeslimEden;

            if(fkPerTeslimEden=="0")
                sql = @"select pkFirma,Firmaadi,OzelKod,f.Tel,Cep,Yetkili,KaraListe,OdemeGunSayisi,SonOdemeTarihi,SonSatisTarihi,fkPerTeslimEden,Devir,
adi+' ' +soyadi as Plasiyer  from Firmalar f with(nolock) 
            left join Personeller p with(nolock) on p.pkpersoneller=f.fkPerTeslimEden
             where f.Aktif=1 and fkPerTeslimEden=f.fkPerTeslimEden";
            
            gridControl1.DataSource = DB.GetData(sql);
        }

        void PersonelGetir()
        {
            lUEPersonel.Properties.DataSource = DB.GetData("select 0 as pkpersoneller,'Tümü' as adi union all SELECT pkpersoneller,(adi+' '+Soyadi) as adi FROM personeller where Plasiyer=1 and AyrilisTarihi is null");
            lUEPersonel.Properties.ValueMember = "pkpersoneller";
            lUEPersonel.Properties.DisplayMember = "adi";
            lUEPersonel.EditValue = 0;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\PlasiyerTahsilatRaporu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("PlasiyerTahsilatRaporu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            TahsilatRaporu(false, DosyaYol);
        }

        void SiparisYazdir(bool Disigner, string RaporDosyasi)
        {
            string sql = @"select pkFirma,OzelKod,Firmaadi,Tel,Cep,Yetkili,KaraListe,OdemeGunSayisi,SonOdemeTarihi,SonSatisTarihi,fkPerTeslimEden,Devir
            from Firmalar f with(nolock) where f.Aktif=1 and fkPerTeslimEden=" + lUEPersonel.EditValue.ToString();
            
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(sql);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + lUEPersonel.Text + "' as TarihAraligi");
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

                rapor.Name = "SiparisExtresi";
                rapor.Report.Name = "SiparisExtresi";

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

        void TahsilatRaporu(bool Disigner, string RaporDosyasi)
        {
            string sql = @"select pkFirma,OzelKod,Firmaadi,Tel,Cep,Yetkili,KaraListe,OdemeGunSayisi,SonOdemeTarihi,SonSatisTarihi,fkPerTeslimEden,Devir
            from Firmalar f with(nolock) where f.Aktif=1 and fkPerTeslimEden=" + lUEPersonel.EditValue.ToString();

            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable FisDetay = DB.GetData(sql);
                if (FisDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                FisDetay.TableName = "FisDetay";
                ds.Tables.Add(FisDetay);

                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT '" + lUEPersonel.Text + "' as TarihAraligi");
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

                rapor.Name = "PlasiyerTahsilatRaporu";
                rapor.Report.Name = "PlasiyerTahsilatRaporu";

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

        private void siparisExtresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\SiparisExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("SiparisExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SiparisYazdir(true, DosyaYol);
        }

        void MusteriPlasiyer()
        {
            gcFirmaPersonel.DataSource = DB.GetData("select f.pkFirma,P.pkPersoneller,f.Firmaadi,p.Adi+' '+p.Soyadi as Personel,f.Aktif from Firmalar f with(nolock) left join Personeller P with(nolock) on P.pkPersoneller=f.fkPerTeslimEden");
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if(e.Page==xtraTabPage1)
            //    gridControl3.DataSource = DB.GetData("select * from siparis with(nolock) where kaydedildi=1");
            if (e.Page == xtraTabPage2)
                MusteriPlasiyer();                
        }

        private void ödemeAlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void ödemeYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = pkFirma;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void müşteriKartıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriKarti KurumKarti = new frmMusteriKarti(pkFirma, "");
            KurumKarti.ShowDialog();

            //int secilen = gridView1.FocusedRowHandle;

            //gridrowgetir(secilen);

            //SiparisGetir();
            MusteriPlasiyer();
        }

        private void personelKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView4.FocusedRowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(gridView4.FocusedRowHandle);
            string personel_id = dr["pkPersoneller"].ToString();
            if (personel_id == "")
            {
                formislemleri.Mesajform("Plasiyer Tanımlanmamış", "K", 200);
                return;
            }

            frmPersonel personelkart = new frmPersonel(personel_id);
            personelkart.ShowDialog();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void btnyazdir_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            //if (xtraTabControl1.SelectedTabPageIndex == 0)
                printableLink.Component = gridControl1;
            //else if (xtraTabControl1.SelectedTabPageIndex == 1)
             //   printableLink.Component = gridControl2;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
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

        private void simpleButton5_Click(object sender, EventArgs e)
        {

            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriFiyatlari.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriFiyatlari.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            YazdirFiyat(false, DosyaYol);
        }

        void YazdirFiyat(bool Disigner, string RaporDosyasi)
        {
            //ArrayList list = new ArrayList();
            //list.Add(new SqlParameter("@fkFirma", musteriadi.Tag.ToString()));
            string sql = @" select f.pkFirma,fOzelKod,f.Firmaadi,sf.SatisFiyatiKdvli as SatisFiyati  from Firmalar f with(nolock)
inner join  SatisFiyatlari sf with(nolock) on sf.fkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
where f.fkPerTeslimEden is not null and  sf.fkStokKarti=1";

            sql = @"select f.pkFirma,f.OzelKod,f.Firmaadi,sk.Stokadi,ss.Sira,sf.SatisFiyatiKdvli as SatisFiyati,sf.fkStokKarti from Firmalar f with(nolock)
inner join  SatisFiyatlari sf with(nolock) on sf.fkSatisFiyatlariBaslik=f.fkSatisFiyatlariBaslik
inner join SiparisStoklari ss with(nolock) on ss.fkStokKarti=sf.fkStokKarti
inner join StokKarti sk on sk.pkStokKarti=sf.fkStokKarti
where f.Aktif=1 and f.fkPerTeslimEden is not null 
order by f.pkFirma,ss.Sira";
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable Fiyatlar = DB.GetData(sql);//, list);
                if (Fiyatlar.Rows.Count == 0)
                {
                    MessageBox.Show("Kayıt Yok");
                    return;
                }
                Fiyatlar.TableName = "Fiyatlar";
                ds.Tables.Add(Fiyatlar);

                //Firma Bilgileri
                //DataTable Fis = DB.GetData(@"SELECT *,dbo.fon_MusteriBakiyesi(pkFirma) as Bakiye  FROM Firmalar with(nolock) WHERE pkFirma=" + musteriadi.Tag.ToString());
                //Fis.TableName = "Fis";
                //ds.Tables.Add(Fis);
                //Firma Bilgileri
                DataTable Baslik = DB.GetData("SELECT 'Müşteri Fiyatları' as TarihAraligi");
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

                rapor.Name = "MusteriFiyatlari";
                rapor.Report.Name = "MusteriFiyatlari";

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

        private void musterİFiyatlariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\MusteriFiyatlari.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("MusteriFiyatlari.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            YazdirFiyat(true, DosyaYol);
        }

        private void yeniMüşteriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriKarti musterikarti = new frmMusteriKarti("0", "");
            musterikarti.ShowDialog();

            MusteriPlasiyer();
        }

        private void lUEPersonel_EditValueChanged(object sender, EventArgs e)
        {
            Getir(lUEPersonel.EditValue.ToString());
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkFirma = dr["pkFirma"].ToString();

            frmMusteriKarti KurumKarti = new frmMusteriKarti(pkFirma, "");
            KurumKarti.ShowDialog();

            Getir(lUEPersonel.EditValue.ToString());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\SiparisExtresi.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("SiparisExtresi.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            SiparisYazdir(false, DosyaYol);
        }

        private void müşteriBorçlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DosyaYol = DB.exeDizini + "\\Raporlar\\PlasiyerTahsilatRaporu.repx";
            if (!File.Exists(DosyaYol))
            {
                MessageBox.Show("PlasiyerTahsilatRaporu.repx Rapor Dosyası Bulunamadı Lütfen Destek Merkezini Arayınız");
                return;
            }
            TahsilatRaporu(true, DosyaYol);
        }
    }
}
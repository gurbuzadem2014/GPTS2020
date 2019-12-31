using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using GPTS.Include.Data;
using GPTS.islemler;
using System.IO;
using System.Threading;

namespace GPTS
{
    public partial class frmSatisOdeme : DevExpress.XtraEditors.XtraForm
    {
        short yazdirmaadedi = 1;
        int fkSatisDurumu = 2;
        string fkSatislar = "0";
        int iOdemeSekli = 0;
        bool fisyazdir = false;

        string BonusYuzde = "0";
        string BonusTur = "0";
        bool _iskaydet = true;
        public frmSatisOdeme(string pkSatislar, bool yazdirilsinmi, int OdemeSekli, bool iskaydet)
        {
            InitializeComponent();

            this.Top = 150;
            this.Left = 320;
            
            fkSatislar = pkSatislar;
            iOdemeSekli = OdemeSekli;
            
            if (OdemeSekli == 3)
                fisyazdir = false;
            else
                fisyazdir = yazdirilsinmi;

            _iskaydet = iskaydet;
        }

        void Yetkiler_Gorunur()
        {
            #region Yetkiler
            string sql = @"SELECT   YetkiAlanlari.Yetki, YetkiAlanlari.Sayi, Parametreler.Aciklama10, YetkiAlanlari.BalonGoster
            FROM  YetkiAlanlari INNER JOIN Parametreler ON YetkiAlanlari.fkParametreler = Parametreler.pkParametreler
            WHERE  Parametreler.fkModul = 1  and YetkiAlanlari.fkKullanicilar =" + DB.fkKullanicilar;
            DataTable dtYetkiler = DB.GetData(sql);
            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "OdemeYaz")
                    btyazdir.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "Birimi")
                  //  gcolbirimi.Visible = Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]);

                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "GizliAlan")
                //{
                //if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //else
                //        dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "SayfaYetki")
                //{
                //    if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    {
                //        gridControl5.DataSource = DB.GetData("select * from Parametreler where fkModul=1 order by SiraNo");
                //        dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //    }
                //    else
                //        dockPanel3.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
                //if (dtYetkiler.Rows[i]["Aciklama10"].ToString() == "HizliButon")
                //{
                //    if (Convert.ToBoolean(dtYetkiler.Rows[i]["Yetki"]))
                //    {
                //        timer1.Enabled = true;
                //        //gridControl5.DataSource = DB.GetData("select * from Parametreler where fkModul=1 order by SiraNo");
                //        dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                //    }
                //    else
                //        dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //}
            }
            #endregion

            #region Sirketler
            DataTable dtSirket = DB.GetData("select isnull(BonusYuzde,0) as BonusYuzde,isnull(BonusTur,0) as BonusTur from Sirketler with(nolock)");
            if (dtSirket.Rows.Count > 0)
            {
                BonusYuzde = dtSirket.Rows[0]["BonusYuzde"].ToString();
                BonusTur = dtSirket.Rows[0]["BonusTur"].ToString();

                if (BonusYuzde == "0")
                {
                    ceBonus.Visible = false;
                    lBonus.Visible = false;
                    ceBonusKalan.Visible = false;
                    ceEskiBonus.Visible = false;
                }
            }
            #endregion
        }

        void MusteriBakiye()
        {
            DataTable dt = DB.GetData("exec sp_MusteriBakiyesi " + MusteriAdi.Tag.ToString() + ",0");
            if (dt.Rows.Count == 0)
            {
                lblMusteriBakiyesi.Text = "0,00";
            }
            else
            {
                decimal bakiye = decimal.Parse(dt.Rows[0][0].ToString());
                lblMusteriBakiyesi.Text = bakiye.ToString("##0.00");//dt.Rows[0][0].ToString();
            }
        }

        private void frmSatisOdeme_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;

            DataTable dtSatislar = DB.GetData(@"select s.pkSatislar,s.fkSatisDurumu,s.Aciklama,s.fkFirma,s.Siparis,isnull(s.AcikHesap,0) as AcikHesap,
            isnull(s.BonusTutar,0) as BonusTutar ,isnull(s.NakitOdenen,0) as NakitOdenen,
            isnull(s.Odenen,0) as Odenen,isnull(s.OdenenKrediKarti,0) as OdenenKrediKarti,
            isnull(s.CekTutar,0) as CekTutar,isnull(BonusOdenen,0) as BonusOdenen,
            isnull(s.fkBankalar,0) as fkBankalar from Satislar s with(nolock) 
            where pkSatislar=" + fkSatislar);

            if (dtSatislar.Rows[0]["Siparis"].ToString() == "True")
            {
                MessageBox.Show("Satış Daha Önce Kaydedildi.");
                this.Tag="2";
                Close();
                return;
            }
            lblAciklama.Text = dtSatislar.Rows[0]["Aciklama"].ToString();
            int.TryParse(dtSatislar.Rows[0]["fkSatisDurumu"].ToString(), out fkSatisDurumu);
            
            string sSatisDurumu = "Satış";
            switch (fkSatisDurumu)
            {
                case (int)Degerler.SatisDurumlari.Teklif: sSatisDurumu = "Teklif"; break;
                case (int)Degerler.SatisDurumlari.Satış: sSatisDurumu = "Satış"; break;
                case (int)Degerler.SatisDurumlari.Fatura: sSatisDurumu = "Fatura"; break;
                case (int)Degerler.SatisDurumlari.İade: sSatisDurumu = "İade"; break;
                case (int)Degerler.SatisDurumlari.Değişim: sSatisDurumu = "Değişim"; break;
                case (int)Degerler.SatisDurumlari.İptal: sSatisDurumu = "İptal"; break;
                case (int)Degerler.SatisDurumlari.Sipariş: sSatisDurumu = "Sipariş"; break;
                case (int)Degerler.SatisDurumlari.İrsaliye: sSatisDurumu = "İrsaliye"; break;
                case (int)Degerler.SatisDurumlari.Tevkifat: sSatisDurumu = "Tevkifat"; break;
                default:
                    break;
            }
            this.Text ="Ödeme İşlemleri {" + sSatisDurumu.ToUpper()+"}";

            DataTable dtp = DB.GetData(@"select pkFirma,OzelKod,Firmaadi,Eposta,isnull(Bonus,0) as Bonus,Devir as Bakiye from Firmalar with(nolock) where pkFirma=" + MusteriAdi.Tag.ToString());
            if (dtp.Rows.Count > 0)
            {
                MusteriAdi.Text = dtp.Rows[0]["OzelKod"].ToString() + "-" + dtp.Rows[0]["Firmaadi"].ToString();
                ceBonusKalan.Value = decimal.Parse(dtp.Rows[0]["Bonus"].ToString());

                decimal bakiye = decimal.Parse(dtp.Rows[0]["Bakiye"].ToString());
                lblMusteriBakiyesi.Text = bakiye.ToString("##0.00");
            }

            lUKasa.Properties.DataSource = DB.GetData("select * from Kasalar with(nolock)");
            lUKasa.EditValue = Degerler.fkKasalar;

            lueKKarti.Properties.DataSource = DB.GetData("Select * from Bankalar with(nolock) where Aktif=1");
            //kontrol et

            lueKKarti.EditValue = int.Parse(dtSatislar.Rows[0]["fkBankalar"].ToString());

            CekListesi();
 
            Yetkiler_Gorunur();

            #region Ödemeleri ve açık hesap getir
            
            if (this.Tag.ToString() == "1")
            {
                DataTable dt = DB.GetData("select 'KH' as T,pkKasaHareket as id,fkKasalar,OdemeSekli,fkBankalar,Borc as Odeme,0 as Odenen from KasaHareket with(nolock)" +
                " where fkSatislar=" + fkSatislar.ToString() + " union all" +
                " select 'S' as T,pkSatislar as id,0 as fkKasalar,' Açık Hesap' as OdemeSekli,0 as fkBankalar,AcikHesap as Odeme,AcikHesapOdenen as Odenen from Satislar with(nolock)" +
                " where pkSatislar=" + fkSatislar.ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OdemeSekli"].ToString() == "Nakit")
                    {
                        ceNakit.EditValue = dt.Rows[i]["Odeme"].ToString();
                        lUKasa.EditValue = int.Parse(dt.Rows[i]["fkKasalar"].ToString());
                    }
                    if (dt.Rows[i]["OdemeSekli"].ToString() == "Kredi Kartı")
                    {
                        ceKrediKarti.EditValue = dt.Rows[i]["Odeme"].ToString();
                        lueKKarti.EditValue = int.Parse(dt.Rows[i]["fkBankalar"].ToString());
                    }
                    if (dt.Rows[i]["OdemeSekli"].ToString() == " Açık Hesap")
                    {
                        ceAcikHesap.EditValue = dt.Rows[i]["Odeme"].ToString();
                    }
                }
            }
            #endregion

            OdenecekTutarHesapla();

            if (iOdemeSekli == (int)Degerler.OdemeSekli.Nakit)
            {
                ceNakit.Value = satistutari.Value;
                ceNakit.SelectAll();
                ceNakit.Focus();
            }
            else if (iOdemeSekli == (int)Degerler.OdemeSekli.KrediKarti || iOdemeSekli == (int)Degerler.OdemeSekli.Banka
                || iOdemeSekli == (int)Degerler.OdemeSekli.Sodexo)
            {
                ceKrediKarti.Value = satistutari.Value;
                ceKrediKarti.SelectAll();
                ceKrediKarti.Focus();
            }
            else
            {
                ceNakit.Value = 0;
                ceNakit.SelectAll();
                ceNakit.Focus();
            }


            if (fisyazdir == true)//yazdıra bas
                btyazdir_Click(sender, e);
            else
            {
                //ödeme şekli Diğer değilse hemen işlem yap diğerde(4) parçalı ödeme olduğu için kullanıcıyı bekle
                if(iOdemeSekli!=3)
                   btnKaydet_Click(sender, e);
            }

            ceEskiBonus.Text = dtSatislar.Rows[0]["BonusOdenen"].ToString();
            ceEskiNakit.Text = dtSatislar.Rows[0]["NakitOdenen"].ToString();
            ceEskiKredikarti.Text = dtSatislar.Rows[0]["OdenenKrediKarti"].ToString();
            ceEskiCek.Text = dtSatislar.Rows[0]["CekTutar"].ToString();
            ceEskiAcikHesap.Text = dtSatislar.Rows[0]["AcikHesap"].ToString();

            if (ceEskiNakit.Text == "0,000000" && ceEskiKredikarti.Text == "0,00"
                && ceEskiCek.Text == "0,00"
                && ceEskiAcikHesap.Text == "0,00"
                && ceEskiBonus.Text == "0,000000")

                pEskiOdenenler.Visible = false;
            else
                pEskiOdenenler.Visible = true;
        }

        void OdenecekTutarHesapla()
        {
            decimal eskibakiye = 0;
            decimal.TryParse(lblMusteriBakiyesi.Text, out eskibakiye);

            ceToplamTutar.EditValue = satistutari.Value + eskibakiye;
        }

        private void ceKrediKarti_EditValueChanged(object sender, EventArgs e)
        {
            lueKKarti.Visible=true;
            if (lueKKarti.EditValue==null)
                lueKKarti.ItemIndex = 0;

            BakiyeHesapla();
        }

        private void ceNakit_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ceNakit.EditValue.ToString()) || ceNakit.EditValue.ToString() == "0")
                lUKasa.Visible = false;
            else
                lUKasa.Visible = true;

            BakiyeHesapla();
        }

        private void vYazdir(Int16 dizayner)
        {
            if (YaziciAdi == "")
                MessageBox.Show("Yazıcı Bulunamadı");
            else
                FisYazdir(dizayner, fkSatislar.ToString(), YaziciDosyasi, YaziciAdi);
        }

        void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrintController = new System.Drawing.Printing.StandardPrintController();
        }

        void FisYazdir(Int16 Disigner, string pkSatislar, string SatisFisTipi, string YaziciAdi)
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
                string fkSatisDurumu = Fis.Rows[0]["fkSatisDurumu"].ToString();
                Fis.TableName = "Fis";
                ds.Tables.Add(Fis);

                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";

                ds.Tables.Add(Sirket);

                DataTable Bakiye = DB.GetData(@"select Tutar as OncekiBakiye,Borc, OdemeSekli from KasaHareket with(nolock) where fkSatislar=" + fisid);

                Bakiye.TableName = "Bakiye";
                ds.Tables.Add(Bakiye);

                //Firma bilgileri
                DataTable Musteri = DB.GetData("select * from VM_Musteriler where pkFirma=" + fkFirma);
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
                //error occurred during cancelling print hatası için
                rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                //if (yazdirmaadedi>1)
                //  rapor.PrintingSystem.StartPrint += new DevExpress.XtraPrinting.PrintDocumentEventHandler(PrintingSystem_StartPrint);

                rapor.DataSource = ds;
                rapor.LoadLayout(RaporDosyasi);
                rapor.Name = SatisFisTipi;
                rapor.Report.Name = SatisFisTipi;

                //XtraReport1 report = new XtraReport1();
                //DevExpress.XtraReports.UI.XRPictureBox pictureBox = new DevExpress.XtraReports.UI.XRPictureBox();
                //pictureBox.Image = Image.FromFile("Tulips.jpg");
                //pictureBox.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                //pictureBox.WidthF = rapor.PageWidth - rapor.Margins.Left - rapor.Margins.Right;
                //pictureBox.HeightF = 500;
                //rapor.Bands[DevExpress.XtraReports.UI.BandKind.Detail].Controls.Add(pictureBox);
                //report.ShowPreviewDialog();


                //ön izleme
                if (Disigner == 3)
                {
                    rapor.ShowPreview();
                }
                //dizayn
                else if (Disigner==2)
                    rapor.ShowDesigner();
                else if (Disigner == 1)
                {
                    if (yazdirmaadedi < 1)
                        yazdirmaadedi = 1;
                    else
                        yazdirmaadedi = Degerler.CopyAdetYazdirmaAdedi;

                    //if (Degerler.CopyAdetYazdirmaAdedi != 1)
                    //  yazdirmaadedi = Degerler.CopyAdetYazdirmaAdedi;

                    for (int i = 0; i < yazdirmaadedi; i++)
                    {
                        if (onizle)
                            rapor.ShowPreviewDialog();
                        else
                            rapor.Print(YaziciAdi);
                    }
                    Degerler.CopyAdetYazdirmaAdedi = 1;
                    DB.ExecuteSQL("update satislar set Yazdir=1 where pkSatislar=" + fkSatislar);
                }
                rapor.Dispose();
                
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        void iKaydet()
        {
            #region uyarilar
            if (ceNakit.Value > 0 && lUKasa.EditValue == null)
            {
                MessageBox.Show("Kasa Seçiniz!");
                lUKasa.Focus();
                return;
            }
            if (ceKrediKarti.Value>0 && lueKKarti.EditValue == null)
            {
                MessageBox.Show("K.Kartı Seçiniz!");
                lueKKarti.Focus();
                return;
            }
            if (ceAcikHesap.Value > 0 && MusteriAdi.Tag.ToString() == "1")
            {
                MessageBox.Show("1 Nolu Müşteriye Açık Hesap Olamaz.");
                ceNakit.Focus();
                return;
            }
            #endregion

            //Önceki Bakiyeyi Kaydet
            decimal EskiBakiye = 0;
            decimal.TryParse(lblMusteriBakiyesi.Text.Replace(".", ",") ,out EskiBakiye);
            decimal CekTutar = 0;
            decimal.TryParse(lueCekler.Text, out CekTutar);

            int sonuc= DB.ExecuteSQL("update Satislar Set "+
                " OncekiBakiye=" + EskiBakiye.ToString().Replace(",", ".") +
                ",ToplamTutar=" + satistutari.Value.ToString().Replace(",", ".") +
                ",Odenen=" + ceNakit.Value.ToString().Replace(",", ".") +
                ",NakitOdenen=" + ceNakit.Value.ToString().Replace(",", ".") +
                ",OdenenKrediKarti=" + ceKrediKarti.Value.ToString().Replace(",", ".")+
                ",BonusOdenen=" + ceBonus.Value.ToString().Replace(",", ".") +
                ",CekTutar=" + CekTutar.ToString().Replace(",", ".") +                
                " where pkSatislar=" + fkSatislar.ToString());
            if (sonuc == -1)
            {
                formislemleri.Mesajform("Hata Oluştu Tekrar Deneyiniz", "K", 200);
                this.Tag = 0;
                return;
            }

            #region Nakit Kasa Hareketi Girişi
            if (ceNakit.Value > 0)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma",MusteriAdi.Tag.ToString()));
                list.Add(new SqlParameter("@Borc", ceNakit.Value.ToString().Replace(",", ".").Replace("-", "")));
                list.Add(new SqlParameter("@fkKasalar", lUKasa.EditValue));
                list.Add(new SqlParameter("@Aciklama", fkSatislar +".Fişden Ödenen"));
                list.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list.Add(new SqlParameter("@Tutar", EskiBakiye));
                list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                string sonucn= DB.ExecuteSQL("INSERT INTO KasaHareket (fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,"+
                "OdemeSekli,fkSatislar,Tutar,BilgisayarAdi,fkKullanicilar,aktarildi,fkSube)" +
                " values(@fkKasalar,@fkFirma,getdate(),5,3,@Borc,0,1,@Aciklama,'Nakit',@fkSatislar,@Tutar,@BilgisayarAdi,@fkKullanicilar,@aktarildi,@fkSube)", list);
                if (sonucn != "0")
                {
                    formislemleri.Mesajform("Nakit'de Hata Oluştu", "K", 200);
                    this.Tag = 0;
                    return;
                }
                //triger yapılacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir-" + ceNakit.Value.ToString().Replace(",", ".") + " where pkFirma=" + MusteriAdi.Tag.ToString());
            }
            
            if (ceNakit.Value < 0)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list.Add(new SqlParameter("@Alacak", ceNakit.Value.ToString().Replace(",", ".").Replace("-", "")));
                list.Add(new SqlParameter("@fkKasalar", lUKasa.EditValue));
                list.Add(new SqlParameter("@Aciklama", fkSatislar + ".Fişden Ödenen"));
                list.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list.Add(new SqlParameter("@Tutar", EskiBakiye));
                list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                string sonucn=DB.ExecuteSQL("INSERT INTO KasaHareket (fkKasalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,"+
                "OdemeSekli,fkSatislar,Tutar,BilgisayarAdi,fkKullanicilar,aktarildi,fkSube)" +
                " values(@fkKasalar,@fkFirma,getdate(),5,3,0,@Alacak,1,@Aciklama,'Nakit',@fkSatislar,@Tutar,@BilgisayarAdi,@fkKullanicilar,@aktarildi,@fkSube)", list);

                if (sonucn != "0")
                {
                    formislemleri.Mesajform("Nakit'de Hata Oluştu", "K", 200);
                    this.Tag = 0;
                    return;
                }
                //triger yapılacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir+" + ceNakit.Value.ToString().Replace(",", ".") + " where pkFirma=" + MusteriAdi.Tag.ToString());
            }
            #endregion

            #region Kredi Kartı Kasa Hareketi Ekle
            if (ceKrediKarti.Value>0)
            {
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list2.Add(new SqlParameter("@Borc", ceKrediKarti.Text.Replace(",", ".").Replace("-", "")));
                list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue.ToString()));
                list2.Add(new SqlParameter("@Aciklama", fkSatislar + ".Fişden Ödemesi"));
                list2.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list2.Add(new SqlParameter("@Tutar", EskiBakiye));
                list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list2.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list2.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                if(iOdemeSekli==4)
                    list2.Add(new SqlParameter("@OdemeSekli", "Banka"));
                else if (iOdemeSekli == 5)
                    list2.Add(new SqlParameter("@OdemeSekli", "Sodexo Ticket"));
                else 
                    list2.Add(new SqlParameter("@OdemeSekli", "Kredi Kartı"));

                string sonuck=DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,"+
                    "fkSatislar,Tutar,BilgisayarAdi,fkKullanicilar,aktarildi,fkSube)" +
                    " values(@fkBankalar,@fkFirma,getdate(),8,1,@Borc,0,1,@Aciklama,@OdemeSekli,@fkSatislar,@Tutar,@BilgisayarAdi,@fkKullanicilar,@aktarildi,@fkSube)", list2);
                if (sonuck != "0")
                {
                    formislemleri.Mesajform("Kredi Kartında Hata Oluştu", "K", 200);
                    this.Tag = 0;
                    DB.ExecuteSQL("Delete From KasaHareket where fkSatislar=" + fkSatislar);
                    return;
                }
                //triger yapılacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir+" + dr["Odenen"].ToString().Replace(",", ".") + " where pkFirma=" + DB.PkFirma);
            }
            if (ceKrediKarti.Value < 0)
            {
                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list2.Add(new SqlParameter("@Alacak", ceKrediKarti.Text.Replace(",", ".").Replace("-", "")));
                list2.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue.ToString()));
                list2.Add(new SqlParameter("@Aciklama", fkSatislar + ".Fişden Ödemesi"));
                list2.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list2.Add(new SqlParameter("@Tutar", EskiBakiye));
                list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list2.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list2.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                if (iOdemeSekli == 4)
                    list2.Add(new SqlParameter("@OdemeSekli", "Banka"));
                else if (iOdemeSekli == 5)
                    list2.Add(new SqlParameter("@OdemeSekli", "Sodexo Ticket"));
                else
                    list2.Add(new SqlParameter("@OdemeSekli", "Kredi Kartı"));

                string sonuck=DB.ExecuteSQL("INSERT INTO KasaHareket (fkBankalar,fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,"+
                "fkSatislar,Tutar,BilgisayarAdi,fkKullanicilar,aktarildi,fkSube)" +
                " values(@fkBankalar,@fkFirma,getdate(),8,1,0,@Alacak,1,@Aciklama,@OdemeSekli,@fkSatislar,@Tutar,@BilgisayarAdi,@fkKullanicilar,@aktarildi,@fkSube)", list2);

                if (sonuck != "0")
                {
                    formislemleri.Mesajform("Kredi Kartında Hata Oluştu", "K", 200);
                    this.Tag = 0;
                    DB.ExecuteSQL("Delete From KasaHareket where fkSatislar=" + fkSatislar);
                    return;
                }
                //triger yapılacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + dr["Odenen"].ToString().Replace(",", ".") + " where pkFirma=" + DB.PkFirma);
            }
            #endregion

            #region Bonus Kasa Hareketi Ekle
            if (ceBonus.Value > 0)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list.Add(new SqlParameter("@Borc", ceBonus.Value.ToString().Replace(",", ".")));
                //list.Add(new SqlParameter("@fkKasalar", lUKasa.EditValue));
                list.Add(new SqlParameter("@Aciklama", fkSatislar +".Fişden"));
                list.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list.Add(new SqlParameter("@Tutar", EskiBakiye));
                list.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));
                list.Add(new SqlParameter("@fkSube", Degerler.fkSube));

                DB.ExecuteSQL("INSERT INTO KasaHareket (fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,fkSatislar,"+
                "Tutar,BilgisayarAdi,fkKullanicilar,GelirOlarakisle,GiderOlarakisle,aktarildi,fkSube)" +
                " values(@fkFirma,getdate(),5,3,@Borc,0,0,@Aciklama,'Bonus Ödenen',@fkSatislar,@Tutar,@BilgisayarAdi,@fkKullanicilar,0,1,@aktarildi,@fkSube)", list);
                //triger yapılacak
                //DB.ExecuteSQL("UPDATE Firmalar SET Borc=Borc+" + ceNakit.Value.ToString().Replace(",", ".") + " where pkFirma=" + DB.PkFirma);
                //DB.ExecuteSQL("UPDATE Satislar SET BonusTutar=" + ceBonus.EditValue.ToString().Replace(",", ".") + " where pkSatislar=" + DB.pkSatislar);

                //bonushareket Ekle

                ArrayList list1 = new ArrayList();
                list1.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list1.Add(new SqlParameter("@Bonus", ceBonus.EditValue.ToString().Replace(",", ".")));
                list1.Add(new SqlParameter("@fkSatislar", fkSatislar));
                DB.ExecuteSQL("INSERT INTO BonusKullanilan (fkFirma,Bonus,Tarih,fkSatislar) VALUES(@fkFirma,@Bonus,GETDATE(),@fkSatislar)", list1);

                DB.ExecuteSQL("UPDATE Firmalar SET Bonus=Bonus-" + ceBonus.EditValue.ToString().Replace(",", ".") +
                    " where pkFirma=" + DB.PkFirma);
            }
            #endregion

            #region Açık Hesap Kaydet
            //if (ceAcikHesap.Text != "0" || ceAcikHesap.Text != "" || ceAcikHesap.Text != "0,00")
            //{
                //ArrayList list3 = new ArrayList();
                //list3.Add(new SqlParameter("@AcikHesap", ceAcikHesap.EditValue.ToString().Replace(",",".")));
                //list3.Add(new SqlParameter("@fkSatislar", fkSatislar));
                //if (ceAcikHesap.Value > 0)
               // {
                //aşağıda zaten upda var
                    //DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=@AcikHesap where pkSatislar=@fkSatislar", list3);
               // }
                //else
                  //  DB.ExecuteSQL("UPDATE Satislar SET AcikHesap=0 where pkSatislar=@fkSatislar", list3);

                //DB.ExecuteSQL("UPDATE Firmalar SET Devir=Devir+" + ceAcikHesap.EditValue.ToString().Replace(",", ".")
                  //             + "where pkFirma=" + MusteriAdi.Tag.ToString());
            //}
            #endregion

            #region Çek Kasa Hareketi Ekle
            decimal cektutar = 0;
            decimal.TryParse(lueCekler.Text, out cektutar);

            if (cektutar >0)
            {
                DB.ExecuteSQL("update Cekler set fkCekTuru=1"+
                    ",fkFirma=" + MusteriAdi.Tag.ToString() +
                    " where pkCek=" + lueCekler.EditValue.ToString());

                DB.ExecuteSQL("update Satislar set fkCek=" + lueCekler.EditValue.ToString()+
                    ",CekTutar=" + cektutar.ToString().Replace(",", ".") +
                    " where pkSatislar=" + fkSatislar);

                ArrayList list2 = new ArrayList();
                list2.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                list2.Add(new SqlParameter("@Borc", cektutar.ToString().Replace(",", ".")));
                //list2.Add(new SqlParameter("@fkBankalar", "0"));
                list2.Add(new SqlParameter("@Aciklama", "Çek Ödemesi"));
                list2.Add(new SqlParameter("@fkSatislar", fkSatislar));
                list2.Add(new SqlParameter("@BilgisayarAdi", Degerler.BilgisayarAdi));
                list2.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));
                list2.Add(new SqlParameter("@fkCek", lueCekler.EditValue.ToString()));
                list2.Add(new SqlParameter("@aktarildi", Degerler.AracdaSatis));

                DB.ExecuteSQL("INSERT INTO KasaHareket (fkFirma,Tarih,Modul,Tipi,Borc,Alacak,AktifHesap,Aciklama,OdemeSekli,"+
                    "fkSatislar,BilgisayarAdi,fkKullanicilar,fkCek,aktarildi)" +
                    " values(@fkFirma,getdate(),8,1,@Borc,0,1,@Aciklama,'Çek Girişi',@fkSatislar,@BilgisayarAdi,@fkKullanicilar,@fkCek,@aktarildi)", list2);
            }
            #endregion

            #region Bonus Ekle
            //satıştan bonus eklendi

            //Degerler.SatisDurumlari
            //2-Satış,4-Fatura,5-Değişim,11-G.Fatura
            if (fkSatisDurumu == 2 || fkSatisDurumu == 9 || fkSatisDurumu == 4 || fkSatisDurumu == 5 || fkSatisDurumu == 11)
            {
                if (BonusYuzde != "0")
                {
                    string sqlbonus = "";
                    if (BonusTur == "0") //Kardan bonus ver
                        sqlbonus = @"update Firmalar set Bonus=isnull(Bonus,0)+kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.AlisFiyati)))/100 as kar from 
                        Satislar s with(nolock)
                        inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar 
                        where s.pkSatislar=@pkSatislar
                        group by s.fkFirma) kartablo
                        where Firmalar.pkFirma=kartablo.fkFirma";
                    if (BonusTur == "1") //Satistan bonus ver
                        sqlbonus = @"update Firmalar set Bonus=isnull(Bonus,0)+kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)))/100 as kar from 
                        Satislar  s with(nolock)
                        inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar 
                        where s.pkSatislar=@pkSatislar
                        group by s.fkFirma) kartablo
                        where Firmalar.pkFirma=kartablo.fkFirma";
                    sqlbonus = sqlbonus.Replace("@BonusYuzde", BonusYuzde);
                    sqlbonus = sqlbonus.Replace("@pkSatislar", fkSatislar);

                    DB.ExecuteSQL(sqlbonus);

                    //satışlara kaydet
                    if (BonusTur == "0") //Kardan bonus ver
                        sqlbonus = @"update Satislar set BonusTutar=kartablo.kar  from (
                        select (@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar-sd.AlisFiyati)))/100 as kar from 
                        Satislar s with(nolock)
                        inner join SatisDetay sd with(nolock)on sd.fkSatislar=s.pkSatislar 
                        where s.pkSatislar=@pkSatislar
                        group by s.fkFirma) kartablo
                        where Satislar.pkSatislar=@pkSatislar";
                    if (BonusTur == "1") //Satistan bonus ver
                        sqlbonus = @"update Satislar set BonusTutar=kartablo.kar  from (select s.fkFirma,(@BonusYuzde * sum(sd.Adet*(sd.SatisFiyati-sd.iskontotutar)))/100 as kar from Satislar s
                        inner join SatisDetay sd with(nolock) on sd.fkSatislar=s.pkSatislar 
                        where s.pkSatislar=@pkSatislar
                        group by s.fkFirma) kartablo
                        where Satislar.pkSatislar=@pkSatislar";
                    sqlbonus = sqlbonus.Replace("@BonusYuzde", BonusYuzde);
                    sqlbonus = sqlbonus.Replace("@pkSatislar", fkSatislar);

                    DB.ExecuteSQL(sqlbonus);
                }
            }
            #endregion

            #region Ödeme Şekli
            string ÖdemeSekli = "Nakit";
            if (ceNakit.Value == satistutari.Value &&  iOdemeSekli==0)
                ÖdemeSekli = "Nakit";
            else if (ceAcikHesap.Value == satistutari.Value && iOdemeSekli == 2)
                ÖdemeSekli = "Açık Hesap";
            else if (ceKrediKarti.Value == satistutari.Value &&  iOdemeSekli == 1)
            {
               if (iOdemeSekli == 4)
                   ÖdemeSekli = "Banka";
               else if (iOdemeSekli == 5)
                    ÖdemeSekli = "Sodexo Ticket";
               else
                    ÖdemeSekli = "Kredi Kartı";
            }
            else
                ÖdemeSekli = "Diğer";
            #endregion

            ArrayList list3 = new ArrayList();
            list3.Add(new SqlParameter("@AcikHesap", ceAcikHesap.EditValue.ToString().Replace(",", ".")));
            list3.Add(new SqlParameter("@fkSatislar", fkSatislar));
            list3.Add(new SqlParameter("@OdemeSekli", ÖdemeSekli));

            if (lueKKarti.EditValue != null && ceKrediKarti.Value !=0)
                list3.Add(new SqlParameter("@fkBankalar", lueKKarti.EditValue.ToString()));
            else
                list3.Add(new SqlParameter("@fkBankalar", "0"));

            DB.ExecuteSQL("UPDATE Satislar SET fkBankalar=@fkBankalar, Siparis=1,AcikHesap=@AcikHesap,OdemeSekli=@OdemeSekli where pkSatislar=@fkSatislar", list3);

            #region Satış durumu kaydet
            string sql = "exec sp_SatisDurumuKaydet " + fkSatislar;
            int sonuc3= DB.ExecuteSQL(sql);
            if (sonuc3 != -1)
            {
                DB.logayaz("sp_SatisDurumuKaydet", "Sql Hatası : " + sql);
            }
            #endregion

            this.Tag = 1;
            btnKaydet.Enabled = false;

            #region taksit EKLE

            if (deOdemeTarihi.EditValue != null)
            {
                #region taksit ekle
                ArrayList listt = new ArrayList();
                listt.Add(new SqlParameter("@fkFirma", MusteriAdi.Tag.ToString()));
                listt.Add(new SqlParameter("@aciklama", "Satış Ödeme Taksitdi"));
                listt.Add(new SqlParameter("@kefil", ""));
                listt.Add(new SqlParameter("@mahkeme", ""));
                listt.Add(new SqlParameter("@fkSatislar", fkSatislar));
                listt.Add(new SqlParameter("@fkKullanici", DB.fkKullanicilar));

                sql = @"insert into Taksit(fkFirma,tarih,aciklama,kefil,mahkeme,fkSatislar,fkKullanici)
                    values(@fkFirma,getdate(),@aciklama,@kefil,@mahkeme,@fkSatislar,@fkKullanici) SELECT IDENT_CURRENT('Taksit')";

                string taksit_id = DB.ExecuteScalarSQL(sql, listt);
                //if(taksit_id=="")
                #endregion

                ArrayList listtt = new ArrayList();
                //list.Add(new SqlParameter("@fkFirma", teMusteri.Tag.ToString()));
                listtt.Add(new SqlParameter("@Tarih", deOdemeTarihi.DateTime.ToString("yyyy-MM-dd")));
                listtt.Add(new SqlParameter("@Odenecek", ceAcikHesap.Value.ToString().Replace(",", ".")));
                listtt.Add(new SqlParameter("@Odenen", "0"));
                listtt.Add(new SqlParameter("@SiraNo", "1"));
                listtt.Add(new SqlParameter("@HesabaGecisTarih", DBNull.Value));
                listtt.Add(new SqlParameter("@taksit_id", taksit_id));
                listtt.Add(new SqlParameter("@fkSatislar", fkSatislar));


                DB.ExecuteSQL(" INSERT INTO Taksitler (Tarih,Odenecek,Odenen,SiraNo,HesabaGecisTarih,OdemeSekli,Kaydet,taksit_id,fkevraktipi,fkSatislar)" +
                    " VALUES(@Tarih,@Odenecek,@Odenen,@SiraNo,@HesabaGecisTarih,'Taksit (Senet)',0,@taksit_id,3,@fkSatislar)", listtt);
                 
            }
            #endregion

            if (ceiskonto.Value>0)
               faturaaltiiskontodagit();  
        }

        void faturaaltiiskontodagit()
        {
            DataTable dt = DB.GetData("select pkSatisDetay from SatisDetay sd with(nolock) where fkSatislar=" + fkSatislar);
            
            decimal d = ceiskonto.Value/dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pkSatisDetay = dt.Rows[i]["pkSatisDetay"].ToString();

                DB.ExecuteSQL("update SatisDetay set Faturaiskonto=" + d.ToString().Replace(",",".")+ " where pkSatisDetay=" + pkSatisDetay);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            //if (ceAcikHesap.Value < 0)
            //{
            //    formislemleri.Mesajform("Ödeme Tutarını Kontrol Ediniz.","K",200);
            //    return;
            //}
            iKaydet();

         if (btnKaydet.Enabled==false)   
             Close();
        }

        private void ceNakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceKrediKarti.Focus();
            //nakit
            if (e.KeyValue == 78)
            {
                btnNakit_Click(sender, e);
                btnKaydet.Focus();
            }
            //kredi kartı
            if (e.KeyValue == 75)
            {
                btnOdemeKredi_Click(sender, e);
                btnKaydet.Focus();
            }
        }

        private void ceCek_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }

        private void ceSenet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceAcikHesap.Focus();
        }

        private void ceAcikHesap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }

        private void frmSatisOdeme_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            else if (e.KeyCode == Keys.F9)
                btnKaydet_Click(sender, e);
            else if (e.KeyCode == Keys.F11)
                btyazdir_Click(sender, e);
        }

        private void lUEBanka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }

        private void islemTarihi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnKaydet.Focus();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(2,"0");
            SatisGoster.ShowDialog();
        }

        void SozlesmeYazdir(bool dizayn)
        {
            xrCariHareket rapor = new xrCariHareket();
            rapor.LoadLayout(DB.exeDizini + "\\Raporlar\\Sozlesme.repx");
            //rapor.FindControl("label15", true).Text = DB.PersonellerBaslik;
            try
            {
                string sql = "select * from Taksitler";//"exec personeldurum " + DB.pkPersoneller.ToString();
                rapor.DataSource = DB.GetData(sql);
            }
            catch (Exception ex)
            {

            }
            if (dizayn)
                rapor.ShowPreview();
            else
                rapor.ShowDesigner();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmSayfaAyarlari SayfaAyarlari = new frmSayfaAyarlari("1");//1
            SayfaAyarlari.ShowDialog();
        }

        private void ceKrediKarti_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (ceKrediKarti.Value > 0)
            {
                lueKKarti.ItemIndex = 0;
                lueKKarti.Visible = true;
            }
            else
            {
                lueKKarti.Visible = false;
            }

            if (e.KeyCode == Keys.Enter)
                btnKaydet.Focus();
        }
        string YaziciAdi = "", YaziciDosyasi = "";
        bool onizle = false;
        private void BtnSozlesme_Click(object sender, EventArgs e)
        {
            vYazdir(3);
            //SozlesmeYazdir(false);
        }

        void threadicinYazdir()
        {
            vYazdir(1);
        }

        public bool YaziciDosyasiVarmi()
        {
            string sd=fkSatisDurumu.ToString();

            if (sd == "5") sd = "2";
            if (sd == "9") sd = "2";

            DataTable dtYazicilar =
            DB.GetData("SELECT  *  FROM SatisFisiSecimi with(nolock)" +
            " where Sec=1 and fkSatisDurumu=" + sd +" and (fkKullanicilar=0 or fkKullanicilar=" + DB.fkKullanicilar+")");

            //if (dtYazicilar.Rows.Count == 0)
            //{
            //    dtYazicilar =  DB.GetData("SELECT  YaziciAdi,Dosya,YazdirmaAdedi FROM SatisFisiSecimi with(nolock)"+
            //     " where Sec=1 and fkKullanicilar=0 and fkSatisDurumu=" + sd);
            //}

            if (dtYazicilar.Rows.Count == 0)
            {
                MessageBox.Show("Aktif satış tipi=" + fkSatisDurumu +" Rapor Bulunamadı");
                fisyazdir = false;
                return false;
            }
            if (dtYazicilar.Rows.Count == 1)
            {
                YaziciAdi = dtYazicilar.Rows[0]["YaziciAdi"].ToString();
                YaziciDosyasi = dtYazicilar.Rows[0]["Dosya"].ToString();
                if (dtYazicilar.Rows[0]["onizle"].ToString() == "True")
                    onizle = true;
                short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);
                Degerler.CopyAdetYazdirmaAdedi = yazdirmaadedi;
            }
            else if (dtYazicilar.Rows.Count > 1)
            {
                //short.TryParse(dtYazicilar.Rows[0]["YazdirmaAdedi"].ToString(), out yazdirmaadedi);

                frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,int.Parse(sd));

                YaziciAyarlari.ShowDialog();

                YaziciAyarlari.Tag = 0;
                YaziciDosyasi = YaziciAyarlari.YaziciAdi.Text;
                onizle= YaziciAyarlari.YaziciAdi.UseWaitCursor;

                if (YaziciDosyasi == "")
                {
                    Close();
                    return false;
                   
                }

                if (YaziciAyarlari.YaziciAdi.Tag == null)
                    YaziciAdi = "";
                else
                    YaziciAdi = YaziciAyarlari.YaziciAdi.Tag.ToString();
                YaziciAyarlari.Dispose();
            }
            return true;
        }
        private void btyazdir_Click(object sender, EventArgs e)
        {
            //if (ceAcikHesap.Value < 0)
            //{
            //    formislemleri.Mesajform("Ödeme Tutarını Kontrol Ediniz.", "K", 200);
            //    return;
            //}
            if (YaziciDosyasiVarmi()==false) return;

            if (_iskaydet)
            {
                iKaydet();
            }
            else
                btnKaydet.Enabled = false;

            if (btnKaydet.Enabled == false)
            {
                System.Threading.Thread tred = new System.Threading.Thread(new System.Threading.ThreadStart(threadicinYazdir));
                tred.Start();

                Close();
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ceBonus_EditValueChanged(object sender, EventArgs e)
        {
            BakiyeHesapla();
            //ceAcikHesap.Value = satistutari.Value - ceNakit.Value - ceKrediKarti.Value - ceBonus.Value;
        }

        private void labelControl9_Click(object sender, EventArgs e)
        {
            ceBonus.Value = ceBonusKalan.Value;
        }

        private void ceKrediKarti_Leave(object sender, EventArgs e)
        {
            if (ceKrediKarti.Value == 0)
            {
                lueKKarti.Visible = false;
            }
            else
            {
                lueKKarti.Visible = true;
            }
        }

        void CekListesi()
        {
            string sql = @"select 0 as pkCek,null as Firmaadi,0 as Tutar,'Seçiniz' as Aciklama,null as Vade
                         union all
                         select c.pkCek,f.Firmaadi,c.Tutar,c.Aciklama,c.Vade from Cekler c with(nolock) 
                         left join Firmalar f with(nolock) on f.pkFirma= c.fkFirma  
                         where fkCekTuru=0 order by 1 desc";
            lueCekler.Properties.DataSource = DB.GetData(sql);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmCekGirisi CekGirisi = new frmCekGirisi("0");//MusteriAdi.Tag.ToString());
            CekGirisi.pkCek.Text="0";
            CekGirisi.ceCekTutari.EditValue = ceAcikHesap.EditValue;
            CekGirisi.pkFirma.Text = "0";//MusteriAdi.Tag.ToString();
            CekGirisi.ShowDialog();
            bt_pkCek.Tag = CekGirisi.pkCek.Text;

            CekListesi();

            lueCekler.ItemIndex = 0;
        }

        private void ceBonus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceNakit.Focus();
        }

        private void ceBonus_Leave(object sender, EventArgs e)
        {
            if (ceBonusKalan.Value < ceBonus.Value)
            {
                formislemleri.Mesajform("Kullanılan Bonus Kalan Bonusdan büyük olamaz.", "K", 200);
                ceBonus.Value = ceBonusKalan.Value;
                ceBonus.Focus();
            }
            if (satistutari.Value < ceBonus.Value)
            {
                formislemleri.Mesajform("Kullanılan Bonus Kalan Satış Toplamından büyük olamaz.", "K", 200);
                satistutari.Focus();
            }
        }

        private void satistutari_EditValueChanged(object sender, EventArgs e)
        {
            ceAcikHesap.EditValue = satistutari.Value - ceNakit.Value - ceKrediKarti.Value - ceBonus.Value;

            ceiskonto.EditValue = ceSatisTutariGercek.Value - satistutari.Value;
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            CekListesi();
        }

        private void ceAcikHesap_EditValueChanged(object sender, EventArgs e)
        {
            decimal eskibakiye= 0;
            decimal.TryParse(lblMusteriBakiyesi.Text,out eskibakiye);

            ceKalanBakiye.Value = ceAcikHesap.Value + eskibakiye;

            if (ceAcikHesap.Value > 0)
                ceAcikHesap.BackColor = System.Drawing.Color.Red;
            else if (ceAcikHesap.Value < 0)
                ceAcikHesap.BackColor = System.Drawing.Color.Blue;
            else
                ceAcikHesap.BackColor = System.Drawing.Color.Transparent;

            //if (ceAcikHesap.Value < 0)
            //{
            //    formislemleri.Mesajform("Fazla Ödenen Tahsilatı, Ödeme Al ile Yapınız!", "K");
            //    simpleButton4.Focus();
            //}
            if (ceAcikHesap.Value > 0)
                deOdemeTarihi.Visible = true;
            else
                deOdemeTarihi.Visible = false;
        }

        private void lueCekler_EditValueChanged(object sender, EventArgs e)
        {
            BakiyeHesapla();
        }

        void BakiyeHesapla()
        {
            decimal cektutar = 0;
            decimal.TryParse(lueCekler.Text, out cektutar);
            ceKalanBakiye.Value = ceToplamTutar.Value - ceKrediKarti.Value - ceBonus.Value - ceNakit.Value - cektutar;

            ceAcikHesap.Value = satistutari.Value - ceKrediKarti.Value - ceBonus.Value - ceNakit.Value - cektutar;
            
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(3, "0");
            SatisGoster.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.pkFirma.Text = MusteriAdi.Tag.ToString();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();

            MusteriBakiye();
            OdenecekTutarHesapla();
            BakiyeHesapla();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaCikis = new frmKasaGiris_Cikis();
            KasaCikis.pkFirma.Text = MusteriAdi.Tag.ToString();
            KasaCikis.Tag = "2";
            KasaCikis.ShowDialog();

            MusteriBakiye();
            OdenecekTutarHesapla();
            BakiyeHesapla();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmYaziciAyarlari YaziciAyarlari = new frmYaziciAyarlari(1,0);
            DB.pkSatislar = int.Parse(fkSatislar);
            YaziciAyarlari.Tag = 1;
            YaziciAyarlari.ShowDialog();
        }

        private void sözleşmeYazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SozlesmeYazdir(true);
        }

        private void btnOdemeKredi_Click(object sender, EventArgs e)
        {
            ceNakit.Value = 0;
            ceBonus.Value = 0;
            ceKrediKarti.EditValue = satistutari.EditValue;
            lueCekler.EditValue = 0;
            btnKaydet.Focus();
        }

        private void btnNakit_Click(object sender, EventArgs e)
        {
            ceKrediKarti.Value = 0;
            ceBonus.Value = 0;
            ceNakit.EditValue = satistutari.EditValue;
            lueCekler.EditValue = 0;
            btnKaydet.Focus();
            lueKKarti.Visible = false;
        }

        private void btnAcikHesap_Click(object sender, EventArgs e)
        {
            ceKrediKarti.Value = 0;
            ceBonus.Value = 0;
            ceNakit.EditValue = 0;
            lueCekler.EditValue = 0;
            btnKaydet.Focus();
        }

        private void ceiskonto_EditValueChanged(object sender, EventArgs e)
        {
            ceiskonto.Visible = false;
            ceSatisTutariGercek.Visible = false;
            if(ceiskonto.Value != 0)
            {
              ceiskonto.Visible=true;
              ceSatisTutariGercek.Visible = true;
            }
        }

        private void satistutari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ceNakit.Focus();
        }
    }
}
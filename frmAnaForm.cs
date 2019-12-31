using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Links;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Helpers;
using GPTS.Include.Data;
using System.Xml;
using DevExpress.XtraBars;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using DevExpress.Utils;
using GPTS.islemler;
using System.Configuration;
//using DevExpress.XtraGrid.Columns;

namespace GPTS
{
    public partial class frmAnaForm : DevExpress.XtraEditors.XtraForm
    {
        ucMusteriListesi CariKartlar;
        ucTedarikciListesi Tedarikciler;
        ucStok_Listesi StokListesi;
        //frmSatisRaporlari Satislar;
        //frmStokKontrol StokKontrol;
        //ucAlis Alis;
        ucAlisTrans _AlisTrans;
        public ucAlisTrans AlisTrans
        {
            get
            {
                if (_AlisTrans == null)
                {
                    _AlisTrans = new ucAlisTrans();
                    sbAlis.Visible = true;
                }
                return _AlisTrans;
            }
            set
            {
                _AlisTrans = value;
            }
        }

        ucPersonelListesi _Personeller;
        public ucPersonelListesi Personeller
        {
            get
            {
                if (_Personeller == null)
                {
                    _Personeller = new ucPersonelListesi();
                    sbPersoneller.Visible = true;
                }
                return _Personeller;

            }
            set
            {
                _Personeller = value;
            }
        }

        //ucSiparis Siparis;
        //ucPersonelListesi projeler;

        //public ucSatis _Satis = null;
        //public ucSatis tSatis
        //{
        //    get
        //    {
        //        if (_Satis == null)
        //            _Satis = new ucSatis();
        //        return _Satis;
        //    }
        //    set { _Satis = value; }
        //}
        //ucSatis SatisFaturasi;
        ucSatis _ucSatis;
        public ucSatis Satis
        {
            get
            {
                if (_ucSatis == null)
                {
                    _ucSatis = new ucSatis("0");
                    sbsatis.Visible = true;
                }
                return _ucSatis;
            }
            set
            {
                _ucSatis = value;
            }
        }

        ucSatisCallerId _ucSatisCallerId;
        public ucSatisCallerId SatisCallerId
        {
            get
            {
                if (_ucSatisCallerId == null)
                {
                    _ucSatisCallerId = new ucSatisCallerId();
                    sbCallerIdSatis.Visible = true;
                }
                return _ucSatisCallerId;
            }
            set
            {
                _ucSatisCallerId = value;
            }
        }

        frmCallerID _CallerID;
        public frmCallerID CallerID
        {
            get
            {
                if (_CallerID == null)
                {
                    _CallerID = new frmCallerID();
                    //sbsatis.Visible = true;
                }
                return _CallerID;
            }
            set
            {
                _CallerID = value;
            }
        }

        
        //ucSatisTrans SatisTransFaturasi;
        ucDepoTransfer DepoTransfer;
        //frmCallerID callerid=null;
        //ucRandevuVer Randevular;
        ucRandevuVer _ucRandevuVer;
        public ucRandevuVer RandevuVer
        {
            get
            {
                if (_ucRandevuVer == null)
                {
                    _ucRandevuVer = new ucRandevuVer();
                    sbRandevu.Visible = true;
                }
                return _ucRandevuVer;
            }
            set
            {
                _ucRandevuVer = value;
            }
        }

        ucRandevuVerYeni _ucRandevuVerYeni;
        public ucRandevuVerYeni RandevuVerYeni
        {
            get
            {
                if (_ucRandevuVerYeni == null)
                {
                    _ucRandevuVerYeni = new ucRandevuVerYeni();
                    //sbRandevu.Visible = true;
                }
                return _ucRandevuVerYeni;
            }
            set
            {
                _ucRandevuVerYeni = value;
            }
        }
        ucSatisRandevuSpa _ucRandevuSpa;
        public ucSatisRandevuSpa RandevuSpa
        {
            get
            {
                if (_ucRandevuSpa == null)
                {
                    _ucRandevuSpa = new ucSatisRandevuSpa();
                    sbRandevuSpa.Visible = true;
                }
                return _ucRandevuSpa;
            }
            set
            {
                _ucRandevuSpa = value;
            }
        }

        ucHatirlatmalar Hatirlat;

        ucStokSayimi _ucStokSayimi;
        public ucStokSayimi StokSayimi
        {
            get
            {
                if (_ucStokSayimi == null)
                {
                    _ucStokSayimi = new ucStokSayimi();
                    btnSayim.Visible = true;
                }
                return _ucStokSayimi;
            }
            set
            {
                _ucStokSayimi = value;
            }
        }

        ucAnaEkran _ucAnaEkran;
        public ucAnaEkran AnaEkran
        {
            get
            {
                if (_ucAnaEkran == null)
                {
                    _ucAnaEkran = new ucAnaEkran();
                    btnSayim.Visible = true;
                }
                return _ucAnaEkran;
            }
            set
            {
                _ucAnaEkran = value;
            }
        }
        //ucCekListesi CekListesi = null;
        //ucSenetListesi SenetListesi = null;
        string skin = "Caramel";
        
        public frmAnaForm()
        {
            InitializeComponent();
            SkinHelper.InitSkinGallery(ribbonGalleryBarItem1, true);
            skinyukle(skin);

            ribbonControl1.SelectedPage = c;
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            Degerler.Versiyon = version;
            lcVersiyon.Text = "V." + version;
            lcVersiyon.Tag = version;
            DB.girisbasarili = false;

            frmGiris giris = new frmGiris();
            //if(Degerler.isProf==false)
            //  giris.lcBaslik.Text = "HİTİT ASİST SATIŞ OTOMASYONU";
            giris.ShowDialog();

            if (!DB.girisbasarili) Application.Exit();
        }

        //klavyeDinleyicisi
        /*
    globalKeyboardHook klavyeDinleyicisi = new globalKeyboardHook();
    public void DinlenecekTuslariAyarla()
    {
        // hangi tuşları dinlemek istiyorsak burada ekliyoruz
        // Ben burada F,K ve M harflerine basılınca tetiklenecek şekilde ayarladım
        klavyeDinleyicisi.HookedKeys.Add(Keys.F);
        klavyeDinleyicisi.HookedKeys.Add(Keys.K);
        klavyeDinleyicisi.HookedKeys.Add(Keys.M);

        //basıldığında ilk burası çalışır
        klavyeDinleyicisi.KeyDown += new KeyEventHandler(islem1);
        //basıldıktan sonra ikinci olarak burası çalışır
        klavyeDinleyicisi.KeyUp += new KeyEventHandler(islem2);
    }
    void islem1(object sender, KeyEventArgs e)
    {
        //Yapılmasını istediğiniz kodlar burada yer alacak
        //Burası tuşa basıldığı an çalışır



        //Eğer buraya gelecek olan tuşa basıldığında
        //o tuşun normal işlevi yine çalışsın istiyorsanız
        //e.Handled değeri false olmalı
        //eğer ilgili tuşa basıldığında burada yakalansın
        // ve devamında tuş başka bir işlev gerçekleştirmesin
        //istiyorsanız bu değeri true yapmalısınız
        e.Handled = false;
    }
    void islem2(object sender, KeyEventArgs e)
    {
        //Yapılmasını istediğiniz kodlar burada yer alacak
        // Burası ilgili tuşlara basılıp çekildikten sonra çalışır



        //Eğer buraya gelecek olan tuşa basıldığında
        //o tuşun normal işlevi yine çalışsın istiyorsanız
        //e.Handled değeri false olmalı
        //eğer ilgili tuşa basıldığında burada yakalansın
        // ve devamında tuş başka bir işlev gerçekleştirmesin
        //istiyorsanız bu değeri true yapmalısınız
        e.Handled = true;
    }
    */
        private void frmAnaForm_Load(object sender, EventArgs e)
        {

            //if (ConfigurationSettings.AppSettings["sifre"] == null)
            //    sifre = "vxYhtNcm7YU=";
            //else
            //   sifre = ConfigurationSettings.AppSettings["sifre"].ToString();
            //sifre = "Hitit9999";
            //sifre = CryptoStreamSifreleme.Encrypt("Hitit999", sifre);
            //sifre = CryptoStreamSifreleme.Decrypt("Hitit999", sifre);

            //sifre = DB.VeriTabaniSifre;

            //if (Degerler.isProf == false)
            //    panelControl4.Height=45;
            //pblogo.SizeMode = PictureBoxSizeMode.AutoSize;

           

            //Thread thread1 = new Thread(new ThreadStart(masaustu_akra_plan_resim_yukle));
            //thread1.Start();

            

            if (DB.kul == null) return;

            KullaniciAdi.Text = "Kullanıcı Adı :" + DB.kul.ToString();
            KullaniciAdi.Width = KullaniciAdi.Text.Length * 6;
            ServerAdi.Text = "SQL Server :" + DB.VeriTabaniAdresi;

            //ModullerYetkileri();
            
            //thread ayırdım çünkü aşağıda ilk açılmada açılmaması için
            ModulYetkiler();

            //Thread threadYetkiler = new Thread(new ThreadStart(Yetkiler));
            //threadYetkiler.Start();

            //if (DateTime.Today > Convert.ToDateTime("2014-07-01"))
            //{
            //    MessageBox.Show("Lütfen Hitit Yazılımı Arayınız 0531 464 80 46");
            //    Close();
            //}
            //else
            //    lcVersiyon.Text = "DEMO<" +
            //    (Convert.ToDateTime("2014-07-01") - DateTime.Today).Days.ToString() + " Gün";

            //dEBugun.DateTime = DateTime.Today;
            DB.PkFirma = 1;
            //StringBuilder str = new StringBuilder(128);
            Degerler.BilgisayarAdi=System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            pcadi.Text = Degerler.BilgisayarAdi;
            pcadi.Width = pcadi.Text.Length * 6;
            //SendKeys.Send("^{F1}");//ctrl+F1
            //ribbonMiniToolbar1.Ribbon.Minimized = true;

            SirketBilgileriSayfaAcLisans();

            GunlukHatirlatmalar();

            //kayan_yazi_internetkontrol();

            Thread thread1 = new Thread(new ThreadStart(masaustu_akra_plan_resim_yukle));
            thread1.Start();

            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread thread2 = new Thread(new ThreadStart(kayan_yazi_internetkontrol));
            thread2.Start();

            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread thread3 = new Thread(new ThreadStart(AcikSatislarveAlislar));
            thread3.Start();
            //AcikSatislarveAlislar();
        }

        void AcikSatislarveAlislar()
        {
            string acik_satis = "",acik_alis="";
            DataTable dt= DB.GetData(@"select pkSatislar from Satislar with(nolock)
                        where GuncellemeTarihi < getdate() - 2 and Siparis = 0");

            if (dt.Rows.Count > 0)
                acik_satis = dt.Rows[0][0].ToString() + " Nolu Satış Fişini Tamamlayınız";

            dt = DB.GetData(@"select pkAlislar from Alislar with(nolock)
                            where GuncellemeTarihi < getdate() - 2 and Siparis = 0");

            if (dt.Rows.Count > 0)
                acik_alis = dt.Rows[0][0].ToString() + " Nolu Alış Fişini Tamamlayınız";

            if (acik_satis == "" && acik_alis == "") return;

            if (acik_satis == "") acik_satis = "Açık Alışlar";
            if (acik_alis == "") acik_alis = "Açık Satışlar";

            notifyIcon1.BalloonTipText = acik_satis;
            notifyIcon1.BalloonTipTitle = acik_alis;
            notifyIcon1.Text = "Hitit Prof 2";
            notifyIcon1.ShowBalloonTip(200);
        }

        void masaustu_akra_plan_resim_yukle()
        {
            if (DB.Sektor == "") DB.Sektor = "Genel";

            if (Degerler.isProf== false) DB.Sektor = "Asist";

            string yol = Application.StartupPath.ToString()+"\\masaustu\\" + DB.Sektor + ".fht";
            if (!File.Exists(yol))
                yol = Application.StartupPath.ToString() + "\\masaustu\\Genel.fht";

            if (File.Exists(yol))
            {
                Image img = null;
                try
                {
                    img = Image.FromFile(yol);
                    pblogo.Image = img;
                }
                catch
                {

                }
                //img.Dispose();
            }
        }

        void ModulYetkiler()
        {
            string sql = @"select m.Kod,m.ModulAdi,my.Yetki from Moduller m with(nolock) 
            left join ModullerYetki  my with(nolock) on my.Kod=m.Kod
            where fkKullanicilar=" + DB.fkKullanicilar;

            DataTable dtYetkiler = DB.GetData(sql);

            barButtonItem6.Enabled = false;
            barButtonItem43.Enabled = false;
            barButtonItem27.Enabled = false;

            barButtonItem20.Enabled = false;
            barbtnPersonel.Enabled = false;

            barSubItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//personel
            barSubItem13.Visibility =  DevExpress.XtraBars.BarItemVisibility.Never;//proje
            barButtonItem81.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//proje

            bbiDepoTransfer.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//DepoTransfer
            bbtbTransferRaporlari.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//DepoTransfer
            bbiStokKartiDepo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//DepoTransfer
            //Gün Sonu Raporu 3.3
            barButtonItem50.Enabled = false;
            barButtonItem88.Enabled = false;
            barButtonItem114.Enabled = false;

            barbtnPersonel.Enabled = false;

            //Kasa Girişi
            barButtonItem136.Enabled = false;
            barButtonItem96.Enabled = false;
            //kasa çıkışı
            barButtonItem79.Enabled = false;
            barButtonItem98.Enabled = false;
            //kasa hareket
            barButtonItem28.Enabled = false;
            barButtonItem69.Enabled = false;
            //btnRandevular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            for (int i = 0; i < dtYetkiler.Rows.Count; i++)
            {
                string Kod, Yetki;
                Kod = dtYetkiler.Rows[i]["Kod"].ToString();
                Yetki = dtYetkiler.Rows[i]["Yetki"].ToString();
                //Satış Raporları
                if (Kod == "16")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem6.Enabled = true;
                        barButtonItem43.Enabled = true;
                        barButtonItem91.Enabled = true;
                        barButtonItem27.Enabled = true;
                        bbiTeklifRaporlari.Enabled = true;
                        barBtnEnvanter.Enabled = true;//envanter şimdilik
                    }
                    else
                    {
                        barButtonItem6.Enabled = false;
                        barButtonItem43.Enabled = false;
                        barButtonItem91.Enabled = false;
                        barButtonItem27.Enabled = false;
                        bbiTeklifRaporlari.Enabled = false;
                        barBtnEnvanter.Enabled = false;//envanter şimdilik
                    }


                }
                //satış raporları
                else if (Kod == "1.1")
                {
                    if (Yetki == "False")
                        ribbonPage6.Visible = false;
                    else
                        ribbonPage6.Visible = true;
                    //ribbonControl1.Items[2].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //uygulamalar
                else if (Kod == "24")
                {
                    if (Yetki == "False")
                        ribbonPage7.Visible = false;
                    else
                        ribbonPage7.Visible = true;
                }
                //Alış Faturası
                else if (Kod == "11")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem40.Enabled = true;
                        barButtonItem16.Enabled = true;
                    }
                    else
                    {
                        barButtonItem40.Enabled = false;
                        barButtonItem16.Enabled = false;
                    }
                    //barSubItem10.Enabled = true;

                }
                //Alış Raporları
                else if (Kod == "11.1")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem72.Enabled = true;
                        barButtonItem26.Enabled = true;
                    }
                    else
                    {
                        barButtonItem72.Enabled = false;
                        barButtonItem26.Enabled = false;
                    }
                    //barSubItem10.Enabled = true;

                }
                //Kullanıcı Listesi
                else if (Kod == "17.1")
                {
                    if (Yetki == "True")
                        barbtnPersonel.Enabled = true;
                    else
                        barbtnPersonel.Enabled = false;
                    //barSubItem10.Enabled = true;
                }
                //Kasa
                else if (Kod == "3" && Yetki == "True")
                {
                    if (Yetki == "True")
                        barButtonItem44.Enabled = true;
                    else
                        barButtonItem44.Enabled = false;
                }
                //Kasa Hareketleri
                else if (Kod == "3.1")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem28.Enabled = true;
                        barButtonItem69.Enabled = true;
                    }
                    else
                    {
                        barButtonItem28.Enabled = false;
                        barButtonItem69.Enabled = false;
                    }
                }
                //kasa listesi
                else if (Kod == "3.2")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem151.Enabled = true;
                        barButtonItem20.Enabled = true;
                    }
                    else
                    {
                        barButtonItem151.Enabled = false;
                        barButtonItem20.Enabled = false;
                    }
                }
                //Stok Eksik Listesi
                else if (Kod == "2.5")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem100.Enabled = true;
                    }
                    else
                    {
                        barButtonItem100.Enabled = false;
                    }
                }
                //Banka Listesi
                else if (Kod == "8.1")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem150.Enabled = true;
                    }
                    else
                    {
                        barButtonItem150.Enabled = false;
                    }
                }
                //Senet Listesi
                else if (Kod == "26")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem118.Enabled = true;
                    }
                    else
                    {
                        barButtonItem118.Enabled = false;
                    }
                }
                //Müşteri Aidat Listesi
                else if (Kod == "27")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem160.Enabled = true;
                    }
                    else
                    {
                        barButtonItem160.Enabled = false;
                    }
                }
                //Gün Sonu Raporu
                else if (Kod == "3.3")
                {
                    if (Yetki == "True")
                    {
                        //aa
                        barButtonItem50.Enabled = true;
                        barButtonItem88.Enabled = true;
                        barButtonItem114.Enabled = true;
                    }
                    else
                    {
                        barButtonItem50.Enabled = false;
                        barButtonItem88.Enabled = false;
                        barButtonItem114.Enabled = false;
                    }
                }
                //Gün Sonu Kapanış
                else if (Kod == "3.3.1")
                {
                    if (Yetki == "True")
                    {
                        //aa
                        barButtonItem50.Enabled = true;
                        //barButtonItem88.Enabled = true;
                        barButtonItem131.Enabled = true;
                    }
                    else
                    {
                        barButtonItem50.Enabled = false;
                        //barButtonItem88.Enabled = false;
                        barButtonItem131.Enabled = false;
                    }
                }
                //Kasa Girişi
                else if (Kod == "3.4")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem136.Enabled = true;
                        barButtonItem96.Enabled = true;
                    }
                    else
                    {
                        barButtonItem136.Enabled = false;
                        barButtonItem96.Enabled = false;
                    }

                }
                //Kasa Çıkışı
                else if (Kod == "3.5")
                {
                    if (Yetki == "True")
                    {
                        barButtonItem79.Enabled = true;
                        barButtonItem98.Enabled = true;
                    }
                    else
                    {
                        barButtonItem79.Enabled = false;
                        barButtonItem98.Enabled = false;
                    }

                }
                //Personel Takip
                else if (Kod == "5")
                {
                    if (Yetki == "True")
                    {
                        barSubItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        //barButtonItem28.Enabled = true;
                        barButtonItem81.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                    else
                    {
                        barSubItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        //barButtonItem28.Enabled = true;
                        barButtonItem81.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
                //6-Proje İşlemleri
                else if (Kod == "6" && Yetki == "True")
                {
                    barSubItem13.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                    //barButtonItem28.Enabled = true;
                }
                //bbiDepoTransfer
                else if (Kod == "19" && Yetki == "True")
                {
                    bbiDepoTransfer.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bbtbTransferRaporlari.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//DepoTransfer
                    bbiStokKartiDepo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//DepoTransfer
                    //barButtonItem28.Enabled = true;
                }
                //hatırlatma
                else if (Kod == "13") 
                {
                    if(Yetki == "True")
                        btnRandevular.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        btnRandevular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                    //barButtonItem28.Enabled = true;
                }
                //stok değişikliği toplu
                else if (Kod == "2.1")
                {
                    if (Yetki == "True")
                        barButtonItem124.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        barButtonItem124.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //stok listesi
                else if (Kod == "2.3")
                {
                    if (Yetki == "True")
                        barButtonItem51.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        barButtonItem51.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Stok Satış Fiyatları
                else if (Kod == "2.4")
                {
                    if (Yetki == "True")
                        barButtonItem130.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        barButtonItem130.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Masa Satış
                else if (Kod == "21")
                {
                    if (Yetki == "True")
                        bbtnMasaSatis.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        bbtnMasaSatis.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //atış CallerId 
                else if (Kod == "23")
                {
                    if (Yetki == "True")
                        bBtnSatisCallerId.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        bBtnSatisCallerId.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //atış CallerId Spa
                else if (Kod == "22")
                {
                    if (Yetki == "True")
                        bBtnSatisRandevuSpa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    else
                        bBtnSatisRandevuSpa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
        }

        void SirketBilgileriSayfaAcLisans()
        {
            #region Şirket Ayarlarını Yükle 
            DataTable dt = DB.GetData("select * from Sirketler with(nolock) where pkSirket=1");
            if (dt.Rows.Count > 0)
            {
                sirketadi.Text = dt.Rows[0]["Sirket"].ToString();
                sirketadi.Width = sirketadi.Text.Length * 7;
                DB.Sektor = dt.Rows[0]["Sektor"].ToString();

                DB.TeraziBarkoduBasi1 = int.Parse(dt.Rows[0]["TeraziBarkoduBasi"].ToString());
                DB.TeraziBarkoduBasi2 = int.Parse(dt.Rows[0]["TeraziBarkoduBasi2"].ToString());
                DB.TeraziBarkoduBasi3 = int.Parse(dt.Rows[0]["TeraziBarkoduBasi3"].ToString());

                if(dt.Rows[0]["uruneklendisescal"].ToString()=="True")
                    Degerler.Uruneklendisescal = true;

                if (dt.Rows[0]["satiskopyala"].ToString() == "True")
                    Degerler.Satiskopyala = true;

                Degerler.YedekSaati = dt.Rows[0]["YedekSaati"].ToString();
                Degerler.YedekYolu = dt.Rows[0]["yedekalinacakyer"].ToString();
                Degerler.SirketAdi = dt.Rows[0]["Sirket"].ToString();
                Degerler.SirketVkn = dt.Rows[0]["VergiNo"].ToString();
                Degerler.SirketVDaire = dt.Rows[0]["VergiDairesi"].ToString();
                Degerler.eposta = dt.Rows[0]["eposta"].ToString();

                if (dt.Rows[0]["EnableSsl"].ToString()=="True")
                  Degerler.EnableSsl = true;

                if (dt.Rows[0]["OncekiFiyatHatirla"].ToString() == "True")
                   Degerler.OncekiFiyatHatirla = true;

                if (dt.Rows[0]["DepoKullaniyorum"].ToString() == "True")
                    Degerler.DepoKullaniyorum = true;

                if (dt.Rows[0]["stok_karti_dizayn"].ToString() == "True")
                    Degerler.StokKartiDizayn = true;
                //sms ayarları
                Degerler.smskullaniciadi = dt.Rows[0]["sms_kullaniciadi"].ToString();
                Degerler.smssifre = dt.Rows[0]["sms_sifre"].ToString();
                Degerler.smsbaslik = dt.Rows[0]["sms_baslik"].ToString();
                Degerler.smsKullaniciNo = dt.Rows[0]["KullaniciNo"].ToString();

                Degerler.kdvorani = int.Parse(dt.Rows[0]["kdv_orani"].ToString());
                Degerler.kdvorani_alis = int.Parse(dt.Rows[0]["kdv_orani_alis"].ToString());
                Degerler.stokbirimi = dt.Rows[0]["stok_birimi"].ToString();
                Degerler.OzelSifre = dt.Rows[0]["OzelSifre"].ToString();
                Degerler.odemesekli = dt.Rows[0]["odemesekli"].ToString();
                
                if (Degerler.AracdaSatis == "1")
                {
                    barBtnVeriAktarim.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barButtonItem132.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    barBtnVeriAktarim.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItem132.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                if (dt.Rows[0]["fkKoduDefault"].ToString()!="")
                    Degerler.fkilKoduDefault = int.Parse(dt.Rows[0]["fkKoduDefault"].ToString());
                if (dt.Rows[0]["fkAltGrupDefault"].ToString() != "")
                    Degerler.fkilceAltGrupDefault = int.Parse(dt.Rows[0]["fkAltGrupDefault"].ToString());

                if (dt.Rows[0]["makbuz_yazdir"].ToString() == "True")
                    Degerler.makbuzyazdir = true;

                if (dt.Rows[0]["MusteriZorunluUyari"].ToString() == "True")
                    Degerler.MusteriZorunluUyari = true;
                else
                    Degerler.MusteriZorunluUyari = false;

                if (dt.Rows[0]["tedarikci_OncekiFiyatHatirla"].ToString() == "True")
                    Degerler.OncekiFiyatHatirla_teda = true;
                else
                    Degerler.OncekiFiyatHatirla_teda = false;

                if (dt.Rows[0]["ilktarih_devir_sontarih"].ToString() == "1")
                    Degerler.ilktarih_devir_sontarih = true;
                else
                   Degerler.ilktarih_devir_sontarih = false;
            }

            #endregion

            ServerAdi.Text = "Ana Makina: " + DB.VeriTabaniAdresi;
            ServerAdi.Width = ServerAdi.Text.Length * 6;
            //timer2EpostaGonder.Enabled = true;

            string Callerid = dt.Rows[0]["Callerid"].ToString();
            if (Callerid == "True")// && Degerler.acilista_caller_id==true)
            {
                Degerler.caller_id_sirket = true;
                try
                {
                    CallerID.Show();

                    //if (callerid == null)
                    //    callerid = new frmCallerID();
                    //callerid.Show();
                    //callerid.Hide();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Lütfen Caller İd Cihazını Kontrol Ediniz. Hata :" + exp.Message);
                }
            }
            else
                Degerler.caller_id_sirket = false;


            if (Degerler.iAktifForm == 1) //Satış Faturaları
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                sayfayukle(Satis);
                //SatisEkraniAc();
            }
            else if (Degerler.iAktifForm == 5) //Sipariş
            {
                //ribbonMiniToolbar1.Ribbon.Minimized = true;
                //SiparisEkraniAc();
                frmSiparisHizli HizliSiparis = new frmSiparisHizli();
                HizliSiparis.Show();
            }
            else if (Degerler.iAktifForm == 6) //stok Listesi
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                Stoklar();
            }
            else if (Degerler.iAktifForm == 2) //Müşteri Listesi
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                Musteriler();
            }
            else if (Degerler.iAktifForm == 4 && barButtonItem40.Enabled == true) //Alış Faturaları
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                sayfayukle(AlisTrans);
            }
            else if (Degerler.iAktifForm == 7) //Etiket Ekranı
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                barButtonItem53_ItemClick(null, null);
            }
            else if (Degerler.iAktifForm == 8) //bBtnCallerId Randevu
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                btnRandevular_ItemClick(null, null);
            }
            else if (Degerler.iAktifForm == 9) //Personel Listesi
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                barButtonItem31_ItemClick(null, null);
            }
            else if (Degerler.iAktifForm == 10) //Masa Satış
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                barButtonItem156_ItemClick(null, null);
            }
            else if (Degerler.iAktifForm == 11 && bBtnSatisCallerId.Visibility == BarItemVisibility.Always)
            {
                //CallerId Satış
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                bBtnSatisCallerId_ItemClick(null, null);
            }
            else if (Degerler.iAktifForm == 12 && bBtnSatisRandevuSpa.Visibility== BarItemVisibility.Always)
            {
                //CallerId Satış Spa
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                bBtnCallerIdRandevu_ItemClick(null, null);
            }
            else
            {
                ribbonMiniToolbar1.Ribbon.Minimized = true;
                sayfayukle(AnaEkran);
                //SatisCallerId.VisibleChanged += SatisCaller_VisibleChanged;
            }
            
            //if (gridView2.DataRowCount > 0 && Degerler.acilista_hatirlatma_ekrani == true)
            //{
            //    btnRandevuVer_Click(sender, e);
            //    //frmHatirlatHizmetSenet HatirlatGunluk = new frmHatirlatHizmetSenet();
            //    //HatirlatGunluk.Show();
            //}

            if (Degerler.caller_id_sirket == true)
            {
                //caller id ekranı aç eski
                barButtonItem140_ItemClick(null, null);
            }

            #region Lisans
            if (dt.Rows[0]["BitisTarihi"].ToString() != "")
                Degerler.LisansBitisTarih = Convert.ToDateTime(dt.Rows[0]["BitisTarihi"].ToString());

            TimeSpan ts = DateTime.Today.Subtract(Degerler.LisansBitisTarih);
            if (ts.Days >1)
            {
                webBrowser1.Visible = false;

                //sirketadi.Text = "Lisanslı Değil";
                labelControl3.Text = "Program Lisans Bitiş Tarihi(" + Degerler.LisansBitisTarih.ToString("dd.MM.yyyy") +
                    ") Lütfen Lisans Yenilemek için Hitit Yazılımı Arayınız 0531 464 80 46";
                labelControl3.Visible = true;

                if (ts.Days > 1)
                {
                    //MessageBox.Show("Program Lisanslı Bitiş Tarihi(" + Degerler.LisansBitisTarih.ToString("dd.MM.yyyy") +
                    //    ") Lütfen Lisans Yenilemek için Hitit Yazılımı Arayınız 0531 464 80 46");
                    Process.Start("http://www.hitityazilim.com/iletisim.aspx");
                    ribbonControl1.Enabled = false;
                }
                if (ts.Days > 30)
                {
                    //Process.Start("http://www.hitityazilim.com/iletisim.aspx");
                    //MessageBox.Show("Program Lisansı Bitmiştir. Bitiş Tarihi(" + Degerler.LisansBitisTarih.ToString("dd.MM.yyyy") +
                    //  ") Lütfen Lisans Yenilemek için Hitit Yazılımı Arayınız 0531 464 80 46");

                    Close();
                }
            }
            else if (ts.Days > -10)
            {
                Process.Start("http://www.hitityazilim.com/iletisim.aspx");
                webBrowser1.Visible = false;
                labelControl3.Text = "Yıllık Lisans(" + Degerler.LisansBitisTarih.ToString("dd.MM.yyyy") + ") Bitimine " + ts.Days.ToString() + " Gün Kaldı";
                labelControl3.Visible = true;
            }
            else
            {
                //labelControl3.Text = Degerler.SirketAdi;
                labelControl3.Visible = false;
                webBrowser1.Visible = true;
            }
            #endregion
        }

        void GunlukHatirlatmalar()
        {
            string Sql = @"select pkHatirlatmaAnimsat,h.animsat_zamani,
            isnull(f.Firmaadi,'')+'-'+Aciklama as [Subject],Tarih as StartTime,
            BitisTarihi as EndTime,Konu as [Description],0 as AllDay,fkfirma,Uyar,h.animsat,h.fkSatislar  from HatirlatmaAnimsat h with(nolock)
            left join Firmalar f with(nolock) on pkFirma=h.fkFirma
            where h.fkDurumu<>2 and h.animsat_zamani<getdate()+1 and (fkKullanicilar is null or fkKullanicilar="+DB.fkKullanicilar+")";


            gcHatirlatma.DataSource = DB.GetData(Sql);
            //Hatırlatma Listesi (0)
            dockPanel2.Text = "Hatırlatma Listesi " + gridView2.DataRowCount.ToString();
            dockPanel2.TabText = "Hatırlatma Listesi " + gridView2.DataRowCount.ToString();
            //timer1.Enabled = true;
            //btnHatirlatma.Tag = gridView2.DataRowCount;
        }

        //int tekrarla = 0;
        //string google, facebook, html;
        void kayan_yazi_internetkontrol()
        {
            //tekrarla = 0;
            //google = "google";
            //facebook = "";
            //webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            //return;
            //Thread.Sleep(1000);
            //WebBrowser wbmessages = new WebBrowser();
            //wbmessages.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbmessages_DocumentCompleted);

            if (!DB.InternetVarmi3()) //if (!DB.InternetVarmi3())
            {
                try
                {
                    webBrowser1.Visible = false;
                    labelControl3.Visible = true;
                    labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Kayan Yazı exp " + exp.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    //webBrowser1.Visible = true;
                    //webBrowser1.IsWebBrowserContextMenuEnabled = false;
                    //webBrowser1.AllowNavigation = false;
                    //webBrowser1.AllowWebBrowserDrop = false;
                    //webBrowser1.ContextMenuStrip = contextMenuStrip1;
                    webBrowser1.Dock = DockStyle.Fill;

                    //string url = "http://www.satisprogrami.gen.tr/kayanyazi.html";
                    //try
                    //{
                    //    webBrowser1.Navigate(url);//new Uri(url));
                    //}
                    //catch (System.UriFormatException)
                    //{
                    //    return;
                    //}
                    //webBrowser1.Refresh();
                }
                catch (System.UriFormatException)
                {
                    MessageBox.Show("Kayan Yazı Uri Format Hatası");
                   return;
                }
                 
               //panelControl4.Controls.Add(webBrowser1);
            }
          
           
           
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            //html = ((WebBrowser)sender).Document.Body.InnerHtml;

            //if (google == "google")
            //{
                //if (tekrarla == 0)
                //{
                //    Application.DoEvents();
                //    webBrowser1.Document.GetElementById("q").SetAttribute("value", "hitityazilim");//textBox1.Text);
                //    webBrowser1.Document.GetElementById("btnG").InvokeMember("click");
                //    tekrarla++;
                //}
            //}
            //else if (facebook == "facebook")
            //{
            //    if (tekrarla == 0)
            //    {
            //        Application.DoEvents();
            //        webBrowser1.Document.GetElementById("email").SetAttribute("value", "");//textBox2.Text);
            //        webBrowser1.Document.GetElementById("pass").SetAttribute("value", "");//textBox3.Text);
            //        webBrowser1.Document.Forms[0].InvokeMember("submit");
            //        tekrarla++;
            //    }
            //}
        }

        void ModullerYetkileri()
        {
            DataTable dt = DB.GetData(@"select m.Kod,m.ModulAdi,my.Yetki from Moduller m with(nolock) 
            left join ModullerYetki  my with(nolock) on my.Kod=m.Kod
            where fkKullanicilar=" + DB.fkKullanicilar);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Proje İşlemleri
                if (dt.Rows[i]["Kod"].ToString() == "6")
                {
                    if (dt.Rows[i]["Yetki"].ToString() == "False")
                        barSubItem13.Enabled=false;
                    else
                        barSubItem13.Enabled = true;

                        barSubItem13.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Banka 
                else if (dt.Rows[i]["Kod"].ToString() == "8" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barSubbtnBanka.Enabled = false;
                    barSubbtnBanka.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Personel Takip
                else if (dt.Rows[i]["Kod"].ToString() == "5" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem81.Enabled = false;
                    barButtonItem81.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem12.Enabled = false;
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Alış 
                else if (dt.Rows[i]["Kod"].ToString() == "11")
                {
                    if (dt.Rows[i]["Yetki"].ToString() == "False")
                    {
                        barButtonItem40.Enabled = false;

                        barButtonItem40.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        barSubItem6.Enabled = false;
                        barSubItem6.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        barButtonItem72.Enabled = false;
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
                //Tedarikçi Takip 
                else if (dt.Rows[i]["Kod"].ToString() == "12" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem3.Enabled = false;
                    barButtonItem3.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem8.Enabled = false;
                    barSubItem8.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Hatırlatma 
                else if (dt.Rows[i]["Kod"].ToString() == "13" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem32.Enabled = false;
                    barButtonItem32.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //barSubItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Kampanya  Satış
                else if (dt.Rows[i]["Kod"].ToString() == "14" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem33.Enabled = false;
                    barButtonItem33.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //barSubItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //Müşteri Listesi
                else if (dt.Rows[i]["Kod"].ToString() == "15" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem82.Enabled = false;
                    barButtonItem82.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem2.Enabled = false;
                        //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ribbonPageGroup23.Visible = false;
                }
                //Stok Modülü
                else if (dt.Rows[i]["Kod"].ToString() == "2" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem51.Enabled = false;
                    barButtonItem51.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem7.Enabled = false;
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ribbonPageGroup1.Visible = false;
                }
                //Kasa Modülü
                else if (dt.Rows[i]["Kod"].ToString() == "3" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem136.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    barButtonItem136.Enabled = false;
                    barButtonItem79.Enabled = false;
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem1.Enabled = false;
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    ribbonPageGroup15.Visible = false;
                }
                //Etiket Modülü
                else if (dt.Rows[i]["Kod"].ToString() == "16" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    //barButtonItem136.Enabled = false;
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barSubItem11.Enabled = false;
                    barSubItem11.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //ribbonPageGroup15.Visible = false;
                }
                //Sipariş
                else if (dt.Rows[i]["Kod"].ToString() == "10" && dt.Rows[i]["Yetki"].ToString() == "False")
                {
                    barButtonItem84.Enabled = false;
                    barButtonItem84.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //barSubItem11.Enabled = false;
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //ribbonPageGroup15.Visible = false;
                }
                //Kullanıcılar Modülü
                //else if (dt.Rows[i]["Kod"].ToString() == "17" && dt.Rows[i]["Yetki"].ToString() == "False")
                //{
                    //barButtonItem21.Enabled = false;
                    //barButtonItem21.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //barSubItem10.Enabled = false;
                    //barSubItem10.SuperTip = baloncuk_degistir("Bu Modül Ücretlidir");
                    //.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                   // ribbonPageGroup1.Visible = false;
                //}
            }

            DataTable dtProf = DB.GetData(@"SELECT  pkModuller FROM Moduller with(nolock) where durumu=1 ");
            if (dtProf.Rows.Count==1)
              Degerler.isProf = false;

        }

        private static SuperToolTip baloncuk_degistir(string baslik)
        {
            SuperToolTip sTooltip = new SuperToolTip();
            // Create an object to initialize the SuperToolTip.
            SuperToolTipSetupArgs args = new SuperToolTipSetupArgs();
            args.Title.Text = baslik;
            //args.Contents.Text = aciklama;
            args.Footer.Text = "Tel : 0262 644 51 12 Cep : 0531 464 80 46";
            //args.Contents.Image = resImage;
            sTooltip.Setup(args);

            // Assign the created SuperToolTip to a BarItem.
            //barItem2.SuperTip = sTooltip2;
            return sTooltip;
        }

        //Control IsGetControl(string Name)
        //{
        //    foreach (Control c in panelControl.Controls)
        //        if (c.Name == Name)
        //            return c;
        //    return null;
        //}

        public void sayfayukle3(string acilanformadi)
        {
            XtraUserControl userform= null;
            if (acilanformadi == "ucSatis")
            {
                //user form açık mı?
                bool acikmi = false;
                for (int i = 0; i < panelControl.Controls.Count; i++)
                {
                    if (panelControl.Controls[i].Name == acilanformadi)
                    {
                        acikmi = true;
                        userform = ((XtraUserControl)(panelControl.Controls[i]));
                    }
                }
                if (acikmi == false)
                {
                    userform = Satis;
                    panelControl.Controls.Add(userform);
                    userform.Dock = DockStyle.Fill;
                }
            }
            if (acilanformadi == "ucAlis")
            {
                //user form açık mı?
                bool acikmi = false;
                for (int i = 0; i < panelControl.Controls.Count; i++)
                {
                    if (panelControl.Controls[i].Name == acilanformadi)
                    {
                        acikmi = true;
                        userform = ((XtraUserControl)(panelControl.Controls[i]));
                    }
                }
                if (acikmi == false)
                {
                    userform = AlisTrans;// new ucAlisTrans();
                    panelControl.Controls.Add(userform);
                    userform.Dock = DockStyle.Fill;
                }
            }
            if (acilanformadi == "ucStok_Listesi")
            {
                //user form açık mı?
                bool acikmi = false;
                for (int i = 0; i < panelControl.Controls.Count; i++)
                {
                    if (panelControl.Controls[i].Name == acilanformadi)
                    {
                        acikmi = true;
                        userform = ((XtraUserControl)(panelControl.Controls[i]));
                    }
                }
                if (acikmi == false)
                {
                    userform = new ucStok_Listesi();
                    panelControl.Controls.Add(userform);
                    userform.Dock = DockStyle.Fill;
                }
            }
            if (acilanformadi == "ucMusteriListesi")
            {
                //user form açık mı?
                bool acikmi = false;
                for (int i = 0; i < panelControl.Controls.Count; i++)
                {
                    if (panelControl.Controls[i].Name == acilanformadi)
                    {
                        acikmi = true;
                        userform = ((XtraUserControl)(panelControl.Controls[i]));
                    }
                }
                if (acikmi == false)
                {
                    userform = new ucMusteriListesi();
                    panelControl.Controls.Add(userform);
                    userform.Dock = DockStyle.Fill;
                }
            }
            userform.Show();
            userform.Focus();
            userform.BringToFront();
        }

        public void sayfayukle(XtraUserControl userform)
        {
            //if (userform == null) userform = new XtraUserControl();
            //if (IsGetControl(userform.Name) == null)
            panelControl.Controls.Add(userform);
            userform.Dock = DockStyle.Fill;
            userform.Show();
            userform.Focus();
            userform.BringToFront();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dEBugun.DateTime = dEBugun.DateTime.AddDays(1);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            dEBugun.DateTime = dEBugun.DateTime.AddDays(-1);
        }
      
        private void skinyukle(string yskin)
        {
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            if (File.Exists(exeyol + "\\skin.xml"))
            {
                XmlTextReader xmlDocument = new XmlTextReader(exeyol + "\\skin.xml");
                try
                {
                    while (xmlDocument.Read())
                    {
                        if (xmlDocument.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlDocument.Name)
                            {
                                case "skin": yskin = Convert.ToString(xmlDocument.ReadString());
                                    break;
                            }
                        }
                    }
                    xmlDocument.Close();
                }
                catch (Exception ex)
                {
                    // DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Money Twins";
                    defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";

                    //MessageBox.Show("Xml Baglanti Hatasi : " + ex.Message);
                }
            }
            defaultLookAndFeel1.LookAndFeel.SkinName = yskin;
        }

        private void yeniMüşteriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.PkFirma = 0;
            frmMusteriKarti KurumKarti = new frmMusteriKarti("0", "");
            KurumKarti.ShowDialog();
        }

        private void programdanÇıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCikis Cikis = new frmCikis();
            Cikis.ShowDialog();
            //Application.Exit();
        }

        private void stokListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StokListesi = new ucStok_Listesi();
            sayfayukle(StokListesi);
        }

        private void yeniStokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
        }

        private void stokFiyatGrubuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGrupTanimlari GrupTanimlari = new frmGrupTanimlari();
            GrupTanimlari.ShowDialog();
        }

        private void stokFiyatGrubuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmGrupTanimlari GrupTanimlari = new frmGrupTanimlari();
            GrupTanimlari.ShowDialog();
        }

        private void depoTanımlarıToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDepoKarti DepoKarti = new frmDepoKarti();
            DepoKarti.ShowDialog();
        }

        private void yeniKasaHareketiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.ShowDialog();
        }

        private void tahsilatGirişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKasaTanimlari KasaBankaTanimlamaKarti = new frmKasaTanimlari("0");
            KasaBankaTanimlamaKarti.ShowDialog();
        }

        private void kasaTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKasaTransferKarti KasaTransferKarti = new frmKasaTransferKarti();
            KasaTransferKarti.ShowDialog();
        }

        private void stokGrupKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStokGrupKarti StokGrupKarti = new frmStokGrupKarti();
            StokGrupKarti.ShowDialog();
        }

        private void taksitÖdemeleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucTaksitOdemeleri TaksitOdemeleri = new ucTaksitOdemeleri();
            sayfayukle(TaksitOdemeleri);
        }

        //void SatisEkraniAc()
        //{
        //    //if (SatisFaturasi == null || SatisFaturasi.IsDisposed)
        //    //{
        //    //    SatisFaturasi = new ucSatis();
        //    //    sbsatis.Visible = true;
        //    //}
        //    sayfayukle(ucSatis);

        //    //sayfayukle(tSatis);

        //    DB.pkSatislar = 0;
        //}

        
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sayfayukle(Satis);
            Satis.VisibleChanged += Satis_VisibleChanged;
            //SatisEkraniAc();
        }

        private void Satis_VisibleChanged(object sender, EventArgs e)
        {
            Satis = null;
            sbsatis.Visible = false;
        }

        void Taksitler()
        {
            frmTaksitOdemeleri TaksitOdemeleri = new frmTaksitOdemeleri();
            TaksitOdemeleri.Show();
        }

        private void barButtonItem82_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Musteriler();
        }

        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            switch (panelControl.Controls[0].Name)
            {
                case "ucCariKartlar":
                    printableLink.Component = CariKartlar.gridControl1;
                    break;
                //case "ucSatislar":
                //    printableLink.Component = SatisFaturasi.gcSatisDetay;
                //    break;
                case "ucStok_Listesi":
                    printableLink.Component = StokListesi.gridControl1;
                    break;
                //case "ucStokKontrol":
                //    printableLink.Component = StokKontrol.gridControl1;
                //    break;
            }
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            yazdir();
        }

        void gunluksatisler()
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@ilkTarih", dEBugun.DateTime.ToString("yyyy-MM-dd 00:00:00")));
            list.Add(new SqlParameter("@sonTarih", dEBugun.DateTime.ToString("yyyy-MM-dd 23:59:59")));
            gridControl1.DataSource = DB.GetData(@"SELECT Satislar.pkSatislar, Satislar.Tarih, Satislar.fkFirma, Satislar.GelisNo, Satislar.Siparis,Firmalar.Firmaadi,SUM(sd.SatisFiyati) as Tutar
FROM Satislar 
INNER JOIN Firmalar ON Satislar.fkFirma = Firmalar.PkFirma
left join SatisDetay sd on sd.fkSatislar=Satislar.pkSatislar
where Satislar.Tarih between @ilkTarih and @sonTarih
group by Satislar.pkSatislar, Satislar.Tarih, Satislar.fkFirma, Satislar.GelisNo, Satislar.Siparis,Firmalar.Firmaadi", list);
        }
        private void dockPanel1_TabIndexChanged(object sender, EventArgs e)
        {
            gunluksatisler();
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            dEBugun.DateTime = dEBugun.DateTime.AddDays(1);
            gunluksatisler();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            dEBugun.DateTime = dEBugun.DateTime.AddDays(-1);
            gunluksatisler();
        }
        #region Print

        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }

        #endregion
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }
        //public void ShowRibbonPreviewDialog(IReport report)
        //{
        //    InitPrintTool(new ReportPrintTool(report));
        //}
        public virtual void InitPrintTool(PrintTool tool)
        {

            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
            //PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            ////if (panelControl1.Controls[0].Name == "ucCariKartlar")
            ////    printableLink.Component = CariKartlar.gridControl1;
            ////if (panelControl1.Controls[0].Name == "ucCariHareket")
            ////    printableLink.Component = CariHareket.gCPerHareketleri;
            ////if (panelControl1.Controls[0].Name == "ucIstenAyrılanlar")
            ////    printableLink.Component = IstenAyrılanlar.gridControl2;
            ////if (panelControl1.Controls[0].Name == "ucProjePersonelDurum")
            //printableLink.Component = CariKartlar.gridControl1;
            //printableLink.Landscape = true;
            //printableLink.CreateDocument(Printing);
            //ShowRibbonPreviewDialog(printableLink);
        }


        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("Mtp2012Guncelle.exe");
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            System.Diagnostics.Process.Start(exeDiz);
            Application.Exit();
        }

        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataTable dt = DB.GetData("exec sp_backup");
                if (dt.Rows[0][0].ToString() == "1")
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alındı.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exp)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu." + exp.Message.ToString(), "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void frmAnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DB.direkcik == true)
            {
                DB.ExecuteSQL("update Kullanicilar set AcikMi=0 where pkKullanicilar=" + DB.fkKullanicilar);
                return;
            }
            //frmCikis Cikis = new frmCikis();
            //Cikis.ShowDialog();
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Programdan Çıkmak İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                e.Cancel = true;

            DB.ExecuteSQL("update Kullanicilar set CikisZamani=getdate(),AcikMi=0 where pkKullanicilar=" + DB.fkKullanicilar);
  
        }

        private void barButtonItem71_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
            //PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            //printableLink.Component = PersonelListesi.gridControl2;
            //printableLink.Landscape = true;
            //printableLink.CreateDocument(Printing);
            //ShowRibbonPreviewDialog(printableLink);
        }
        void Stoklar()
        {
            if (StokListesi == null || StokListesi.IsDisposed)
            {
                StokListesi = new ucStok_Listesi();
                sbStoklar.Visible = true;
            }
            sayfayukle(StokListesi);
            sbStoklar.Visible = true;
        }

        void Musteriler()
        {
            DB.PkFirma = 0;
            if (CariKartlar == null || CariKartlar.IsDisposed)
            {
                CariKartlar = new ucMusteriListesi();
                sbMusteriler.Visible = true;
            }
            sayfayukle(CariKartlar);
            sbMusteriler.Visible = true;
        }

       
        

        private void barButtonItem70_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAktar Aktar = new frmAktar();
            Aktar.ShowDialog();
        }
        private void barButtonItem67_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
            //PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            //printableLink.Component = StokListesi.gridControl1;
            //printableLink.Landscape = true;
            //printableLink.CreateDocument(Printing);
            //ShowRibbonPreviewDialog(printableLink);
        }

        private void barButtonItem45_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
        }

        private void barButtonItem62_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
        }

        private void barButtonItem75_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem36_ItemClick(sender, e);
        }
        private void barButtonItem64_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKasaTanimlari KasaBankaTanimlamaKarti = new frmKasaTanimlari("0");
            KasaBankaTanimlamaKarti.ShowDialog();
        }
  
        private void barButtonItem53_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            System.Diagnostics.Process.Start(exeDiz+"\\Mtp2012Guncelle.exe");
            System.Diagnostics.Process.Start(exeDiz);
            DB.direkcik = true;
            Application.Exit();
        }
        private void skinkaydet(string kskin)
        {
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            if (File.Exists(exeyol + "\\skin.xml"))
            {
                File.Delete(exeyol + "\\skin.xml");
                Application.DoEvents();
            }
            //else
            // {
            XmlTextWriter xmlDocument = new XmlTextWriter(exeyol + "\\skin.xml", System.Text.UTF8Encoding.UTF8);
            //herhangi bi hata olusup olusmadigini anlamak için try-catch blogu kullandim           
            try
            {
                //xmle yazma islemini baslattim 
                xmlDocument.WriteStartDocument();
                //channel ve item elementlerini olusturup verileri kayidini gerçeklestirdim.
                xmlDocument.WriteStartElement("CHANNEL");
                xmlDocument.WriteStartElement("ITEM");
                xmlDocument.WriteElementString("skin", kskin);
                //channelve item taglarini kapattik               
                xmlDocument.WriteEndElement();
                xmlDocument.WriteEndElement();
                //dökümani sonlandirdim                
                xmlDocument.WriteEndDocument();
                //ve son olarak xml ile baglantiyi kestim. bu kisim çok önemli eger bu kodu unutursak
                //bu xml dosyasina okumak yada yazmak istersek baglamanayiz.               
                xmlDocument.Close();
                // MessageBox.Show("Kayit Tamamlandi.");
                // Constants.ayarlarini();
            }
            catch (Exception ex)
            {
                defaultLookAndFeel1.LookAndFeel.SkinName = "Money Twins";
                //  DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Money Twins";
                //eger xml olusturulurken bir hata olusursa hatanin sebebini ögrendim               
                // MessageBox.Show("Hata" + ex.Message);
            }
            // }
        }
        private void ribbonGalleryBarItem1_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            skinkaydet(e.Item.Caption);
        }

        private void ribbonControl1_SelectedPageChanging(object sender, DevExpress.XtraBars.Ribbon.RibbonPageChangingEventArgs e)
        {
            //switch (e.Page.Name)
            //    {
            //         case "ribbonPage2":
            //            Musteriler();
            //            break;
            //         case "ribbonPage3":
            //            //vSatislar();
            //            break;
            //        case "ribbonPage6":
            //            Stoklar();
            //            break;
            //        case "ribbonPage8":
            //           // Taksitler();
            //            break;
            //    }
        }

        private void barButtonItem114_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAyarlar KurumDonem = new frmAyarlar(1);
            KurumDonem.ShowDialog();              
        }

        private void barButtonItem79_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "2";
            KasaGirisi.ShowDialog();
        }

        private void barButtonItem136_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            DB.PkFirma = 0;
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            navBarItem4_LinkClickedExtracted();
        }

        private void barButtonItem139_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmEpostaGonder Yenilikler = new frmEpostaGonder();
            Yenilikler.pFaturaTarihi.Visible = false;
            Yenilikler.Text = "Yenilikler";
            Yenilikler.ShowDialog();
            //ucKasaHareketInstantFeedback userform = new ucKasaHareketInstantFeedback();
            //panelControl.Controls.Add(userform);
            //userform.Dock = DockStyle.Fill;
            //userform.Show();
            //userform.BringToFront();

            //string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            //if(File.Exists(exeDiz + "\\Guncelle.exe"))
            //{
            //  System.Diagnostics.Process.Start(exeDiz + "\\Guncelle.exe");
            //  System.Diagnostics.Process.Start(exeDiz);
            //  DB.direkcik = true;
            //  Application.Exit();
            //}
        }

        private void ribbonControl1_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show(((BarButtonItem)sender).Tag.ToString());
        }
        private void barButtonItem140_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
                CallerID.Show();
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show("Caller Id Cihazı Bulunamadı" + exp.Message);
            //    //throw;
            //}
            

            //if (callerid == null)
            //callerid = new frmCallerID();
            //if(callerid.IsDisposed)
            //    callerid = new frmCallerID();
            //callerid.Show();
        }

        private static void navBarItem4_LinkClickedExtracted()
        {
            frmMusteriKarti KurumKarti = new frmMusteriKarti("0", "");
            DB.PkFirma = 0;
            KurumKarti.ShowDialog();
        }

        private void barbtnWeb_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWebAyarlari WebAyarlari = new frmWebAyarlari();
            WebAyarlari.ShowDialog();
        }

        private void barbtnAktarim_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAktarim Aktarim = new frmAktarim();
            Aktarim.ShowDialog();
        }

        private void barButtonItem117_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Hatirlat == null || Hatirlat.IsDisposed)
            {
                Hatirlat = new ucHatirlatmalar();
                //sbsatis.Visible = true;
            }
            sayfayukle(Hatirlat);

        }

        private void barButtonItem142_ItemClick(object sender, ItemClickEventArgs e)
        {
            Musteriler();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
           Stoklar();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Tedarikciler == null || Tedarikciler.IsDisposed)
                Tedarikciler = new ucTedarikciListesi();
            sayfayukle(Tedarikciler);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokSatisGrafigi StokSatisGrafigi = new frmStokSatisGrafigi("0");
            StokSatisGrafigi.ShowDialog();
        }
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            sayfayukle(AlisTrans);
            AlisTrans.VisibleChanged += AlisTrans_VisibleChanged;
                //UcstokListesi_VisibleChanged;
            //if (Alist == null || Alist.IsDisposed)
            //{
            //    Alist = new ucAlisTrans();
            //    sbAlis.Visible = true;
            //}
            //sayfayukle(Alist);
            //DB.pkAlislar = 0;


            //return;

            //if (Alis == null || Alis.IsDisposed)
            //{
            //    Alis = new ucAlis();
            //    sbAlis.Visible = true;
            //}
            //sayfayukle(Alis);
            //DB.pkAlislar = 0;
        }

        private void AlisTrans_VisibleChanged(object sender, EventArgs e)
        {
            AlisTrans = null;
            //throw new NotImplementedException();
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucKasalar Kasalar = new ucKasalar();
            sayfayukle(Kasalar);
        }
        void KullaniciDegistir()
        {
            for (int i = 0; i < panelControl.Controls.Count; i++)
            {
                if (panelControl.Controls[i].Name == "ucSatis" || panelControl.Controls[i].Name == "ucAlis")
                {
                    _ucSatis = null;
                    _AlisTrans = null;
                    panelControl.Controls[i].Dispose();
                }
            }
            frmGiris Giris = new frmGiris();
            Giris.ShowDialog();
            KullaniciAdi.Text = "Kullanıcı Adı :" + DB.kul.ToString();
            ServerAdi.Text = "SQL Server :" + DB.VeriTabaniAdresi.ToString();

            ModulYetkiler();
            SirketBilgileriSayfaAcLisans();
            tHatirlat.Interval = 6000;
        }
        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            //btnSayim.PerformClick();
            sayfayukle(StokSayimi);
            btnSayim.VisibleChanged += Sayim_VisibleChanged;
        }

        private void barbtnBankaListesi_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucBankaListesi BankaListesi = new ucBankaListesi();
            sayfayukle(BankaListesi);
        }

        private void barButtonItem48_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBankaHareketleri KasaHareketleri = new frmBankaHareketleri();
            KasaHareketleri.ShowDialog();
        }

        private void barButtonItem47_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaTransferKarti KasaTransferKarti = new frmKasaTransferKarti();
            KasaTransferKarti.ShowDialog();
        }

        private void barButtonItem18_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmGunlukKur GunlukKur = new frmGunlukKur(false);
            GunlukKur.ShowDialog();
        }

        private void barButtonItem109_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAyarlar KurumDonem = new frmAyarlar(1);
            KurumDonem.ShowDialog();
        }

        private void barbtnPersonel_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKullanicilar kul = new frmKullanicilar();
            kul.Show();
        }

        private void psatis_Click(object sender, EventArgs e)
        {
            sayfayukle(Satis);
            Satis.VisibleChanged += Satis_VisibleChanged1;
            //sbsatis.Visible = true;
            //sayfayukle3("ucSatis");
        }

        private void Satis_VisibleChanged1(object sender, EventArgs e)
        {
            Satis = null;
            sbsatis.Visible = false;
            //throw new NotImplementedException();
        }

        private void RandevuVer_VisibleChanged(object sender, EventArgs e)
        {
            RandevuVer = null;
            RandevuVer.Visible = false;
            //throw new NotImplementedException();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < panelControl.Controls.Count; i++)
            {
                if (panelControl.Controls[i].Name == "ucSatis")
                {
                    panelControl.Controls[i].Focus();
                    //SendKeys.Send("{ENTER}");
                    //userform = ((XtraUserControl)(panelControl.Controls[i]));
                }
            }
            //panelControl.Focus();
            //pblogo.Focus();
            //SendKeys.Send("{TAB}");
            //this.GetNextControl(ActiveControl, true).Focus();
            //this.GetNextControl(ActiveControl, true).Focus();
            this.WindowState = FormWindowState.Minimized;
        }

        private void barButtonItem119_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DataTable dt = DB.GetData("exec sp_backup");
                if (dt.Rows[0][0].ToString() == "1")
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alındı.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exp)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu." + exp.Message.ToString(), "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKullaniciRaporlari KullaniciRaporlari = new frmKullaniciRaporlari();
            KullaniciRaporlari.ShowDialog();
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    Saat.Text = DateTime.Now.ToString("HH:mm") + DateTime.Now.ToString("dd.MM.yyyy");
        //}

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Musteriler();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSatisRaporlari Satislar = new frmSatisRaporlari();
            Satislar.Show();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            sayfayukle3("ucStok_Listesi");
        }

        private void barButtonItem28_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaHareketleri KasaHareketleri = new frmKasaHareketleri();
            KasaHareketleri.ShowDialog();
        }

        private void barButtonItem29_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAyarlar isyeri = new frmAyarlar(1);
            isyeri.ShowDialog();
        }
        private void barButtonItem32_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Hatirlat == null || Hatirlat.IsDisposed)
            {
                Hatirlat = new ucHatirlatmalar();
                //sbsatis.Visible = true;
            }
            sayfayukle(Hatirlat);
        }

        private void barButtonItem33_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKampanya Kampanyalar = new frmKampanya();
            Kampanyalar.Show();
            //sayfayukle(Kampanyalar);
        }

        private void barButtonItem44_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageBox.Show("Yapım Aşamasındadır!");
        }

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = 0;
            StokKarti.ShowDialog();
        }

        private void barButtonItem39_ItemClick(object sender, ItemClickEventArgs e)
        {
            DB.PkFirma = 0;
            frmMusteriKarti MusteriKarti = new frmMusteriKarti("0", "");
            MusteriKarti.ShowDialog();
        }

        private void barButtonItem41_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTedarikciKarti MusteriKarti = new frmTedarikciKarti("0");
            DB.PkFirma = 0;
            MusteriKarti.ShowDialog();
        }
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            sayfayukle(AlisTrans);
            //sayfayukle3("ucAlis");
        }

        private void barButtonItem53_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmEtiketBas EtiketBas = new frmEtiketBas();
            EtiketBas.Show();
        }
        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAlisRaporlari Satislar = new frmAlisRaporlari();
            Satislar.Show();
        }

        private void barButtonItem43_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSatisRaporlari Satislar = new frmSatisRaporlari();
            Satislar.Show();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAlisRaporlari Alislar = new frmAlisRaporlari();
            Alislar.Show();
        }
        private void barButtonItem31_ItemClick(object sender, ItemClickEventArgs e)
        {
            sayfayukle(Personeller);
            Personeller.VisibleChanged += Personeller_VisibleChanged1;
            //AlisTrans.VisibleChanged += AlisTrans_VisibleChanged;

            //    if (projeler == null || projeler.IsDisposed)
                    //projeler = new ucPersonelListesi();
            //    sayfayukle(projeler);
            //GPTS.frmProjeKarti projekarti = new GPTS.frmProjeKarti();
            //projekarti.ShowDialog();
        }

        private void Personeller_VisibleChanged1(object sender, EventArgs e)
        {
            Personeller = null;
            sbPersoneller.Visible = false;
            //throw new NotImplementedException();
        }

        private void barButtonItem57_ItemClick(object sender, ItemClickEventArgs e)
        {
            //ucProjeListesi projeler = new ucProjeListesi();
            //sayfayukle(projeler);
        }

        private void ribbonControl1_ItemClick(object sender, ItemClickEventArgs e)
        {
            string s = e.Item.Name.ToString();
            int a = s.ToString().IndexOf("Sub");
            if(a==-1)
                ribbonMiniToolbar1.Ribbon.Minimized = true;
        }

        private void barButtonItem69_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaHareketleri KasaHareketleri = new frmKasaHareketleri();
            KasaHareketleri.Show();
        }

        private void barButtonItem70_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmKasaTanimlari b = new frmKasaTanimlari("0");
            b.ShowDialog();
        }

        private void barButtonItem78_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.Tag = "0";
            KasaGirisi.ShowDialog();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DataTable dtxml = new DataTable("VersionFile");
            //    dtxml.Columns.Add("Version", typeof(string));
            //    dtxml.ReadXml("http://www.dorukdekorasyon.com/2012Versiyon.xml");
            //    string xmls = "";
            //    if (dtxml.Rows.Count > 0)
            //        xmls = dtxml.Rows[0][0].ToString();
            //    if (xmls != "" && xmls != lcVersiyon.Tag.ToString())
            //    {
            //        DialogResult secim;
            //        secim = DevExpress.XtraEditors.XtraMessageBox.Show("Yeni Versiyon Yüklenecek.\n Program Kapatılıp Dosyalar İndirilecekdir. \n Lütfen indirilen Dosyaları, açılan klasörün içine sürükleyiniz!", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //        if (secim == DialogResult.No) return;
            //        Process.Start("http://www.hitityazilim.com/guncelleme/Hitit2012.rar");
            //        Process.Start(Application.StartupPath);
            //        Application.Exit();
            //    }
            //    else
            //        DevExpress.XtraEditors.XtraMessageBox.Show("Son Versiyonu Kullanmaktasınız", "Hitit 2012", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception exp)
            //{
                DialogResult secim;
                secim = DevExpress.XtraEditors.XtraMessageBox.Show("Yeni Versiyon Yüklenecek.\n Program Kapatılıp Dosyalar İndirilecekdir. \n Lütfen indirilen Dosyaları, açılan klasörün içine sürükleyiniz!", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No) return;


                Process.Start("http://www.hitityazilim.com/guncelleme/Hitit2012.rar");
                Process.Start(Application.StartupPath);
                Application.Exit();
            //}
        }

        private void barButtonItem80_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmFiyatGor FiyatGor = new frmFiyatGor();
            FiyatGor.ShowDialog();
        }
        void SiparisEkraniAc()
        {
            //if (Siparis == null || Siparis.IsDisposed)
            //    Siparis = new ucSiparis();
            //sayfayukle(Siparis);
        }
        private void barButtonItem83_ItemClick(object sender, ItemClickEventArgs e)
        {
            SiparisEkraniAc();
        }

        private void Saat_Click(object sender, EventArgs e)
        {
            frmDuyurular duyurular = new frmDuyurular();
            duyurular.Show();
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.FileName = "C:\\windows\\system32\\rundll32.exe";
            //psi.Arguments = "shell32.dll,Control_RunDLL timedate.cpl";
            //Process.Start(psi);
            //Process.Start("calc");
        }

        private void Baslik_Click(object sender, EventArgs e)
        {
            //kayan_yazi();
            //webBrowser1.Refresh();
        }

        private void barButtonItem85_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (callerid == null)
            //    callerid = new frmCallerID();
            //callerid.Show();
            CallerID.Show();
        }

        private void barButtonItem88_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaTransfer kasatransfer = new frmKasaTransfer();
            kasatransfer.Show();
        }

        private void barButtonItem89_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void sbSatisRaporlari_Click(object sender, EventArgs e)
        {
            sayfayukle(Personeller);
            //Personeller.VisibleChanged += Personeller_VisibleChanged;
            //frmSatisRaporlari Satislar = new frmSatisRaporlari();
            //Satislar.Show();
        }

        //private void Personeller_VisibleChanged(object sender, EventArgs e)
        //{
        //    Personeller = null;
        //    sbPersoneller.Visible = false;
        //    //throw new NotImplementedException();
        //}

        private void barButtonItem91_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriKarlikRaporu istatistikRaporlari = new frmMusteriKarlikRaporu();
            istatistikRaporlari.Show();
        }

        private void barButtonItem94_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(3, "0");
            SatisGoster.ShowDialog();
        }

        private void barButtonItem95_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit(); //Close();
        }

        private void barButtonItem99_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUcGoster SatisGoster = new frmUcGoster(2, "0");
            SatisGoster.ShowDialog();            
        }

        private void barButtonItem100_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmEksik_Listesi Eksik_Listesi = new frmEksik_Listesi();
            Eksik_Listesi.Show();
        }

        private void barButtonItem103_ItemClick(object sender, ItemClickEventArgs e)
        {
             System.Diagnostics.Process.Start("http://www.hitityazilim.com/iletisim.aspx");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hitityazilim.com/SayfaSablon.aspx?id=2021");
        }

        private void barButtonItem102_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("iexplore.exe", "http://www.hitityazilim.com/kayanyazi.html");
            webBrowser1.Refresh();
            //kayan_yazi();
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {
            //YedekAl();
            Process.Start("http://www.hitityazilim.com");
        }

        private void frmAnaForm_MinimumSizeChanged(object sender, EventArgs e)
        {
            //if (SatisFaturasi == null || SatisFaturasi.IsDisposed)
            //{
            //    SatisFaturasi = new ucSatis();
            //   sbsatis.Visible = true;
            //}
            //sayfayukle(SatisFaturasi);
            //sayfayukle(tSatis);
            DB.pkSatislar = 0;
        }

        private void hideContainerRight_Click(object sender, EventArgs e)
        {
            GunlukHatirlatmalar();
            //gcHatirlatma.DataSource = DB.GetData("select * from Hatirlatma where Tarih>getdate()-1  and Tarih<getdate()+1");
        }

        private void tmrYedekAl_Tick(object sender, EventArgs e)
        {
            //if(btnHatirlatma.Tag.ToString() !="0")
            //{
            //    if (btnHatirlatma.BorderStyle == DevExpress.XtraEditors.Controls.BorderStyles.Flat)
            //    {
            //        btnHatirlatma.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            //    }
            //    else
            //        btnHatirlatma.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            //}

            if(this.Tag.ToString() == "0")
               this.Text = Degerler.fkSube + "-HİTİT PROF SATIŞ PROGRAMI " + DateTime.Now.ToString("HH:mm:ss");

            if (!Degerler.giris_yapildi) return;

            if (DateTime.Now.ToString("HH:mm:ss") == Degerler.YedekSaati)  
            {
                if (Degerler.AnaBilgisayar == false) return;

                this.Text = "HİTİT PROF SATIŞ PROGRAMI Yedek Alınıyor... " + DateTime.Now.ToString("HH:mm:ss");
                this.Tag = "1";

                Thread thread1 = new Thread(new ThreadStart(YedekAl));
                thread1.Start();
            }
            else if ((DateTime.Now.ToString("HH:mm:ss") == Degerler.GunlukKurSaati) && Degerler.isKurlariAl)
            {
                //kurları güncelle
                Thread thread1 = new Thread(new ThreadStart(kurlarial));
                thread1.Start();
            }

            tHatirlat.Interval = Degerler.AnimsatmaSaniyeInterval;

            if (btnUyar.Text != "0")
            {
                if (btnUyar.ForeColor == System.Drawing.Color.Red)
                {
                    //btnUyar.Visible = false;
                    btnUyar.ForeColor = System.Drawing.Color.Blue;
                    dockPanel2.ForeColor = System.Drawing.Color.Blue;
                    btnUyar.Font = btnSimge.Font;
                }
                else
                {
                    //btnUyar.Visible = true;
                    btnUyar.ForeColor = System.Drawing.Color.Red;
                    dockPanel2.ForeColor = System.Drawing.Color.Red;
                    btnUyar.Font = btnHakkimizda.Font;
                }
            }
            else
            {
                //btnUyar.Visible = true;
                btnUyar.ForeColor = System.Drawing.Color.Black;
                dockPanel2.ForeColor = System.Drawing.Color.Black;
            }
            //Degerler.sessiotimeout = Degerler.sessiotimeout + 1;
            //if (Degerler.sessiotimeoutsuresi < Degerler.sessiotimeout)
            //{
            //    KullaniciDegistir();
            //    Degerler.sessiotimeout = 0;
            //    Degerler.sessiotimeoutsuresi = 0;
            //}

            //pcadi.Text = Degerler.sessiotimeout.ToString();
        }

       private void kurlarial()
       {
            frmGunlukKur kurlarigetir = new frmGunlukKur(true);
            kurlarigetir.Show();
            kurlarigetir.Close();
       }

        private void YedekAl()
        {
            Digerislemler.YedekAl();
            this.Tag = "0";
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(DateTime.Now,DateTime.Now,0);
            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
            DB.pkHatirlatmaAnimsat=int.Parse(pkHatirlatmaAnimsat);
            Hatirlat.ShowDialog();
            //DB.ExecuteSQL("update Hatirlatma set Uyar=1 where pkHatirlatma=" + pkHatirlatma);
            GunlukHatirlatmalar();

        }

        private void barButtonItem108_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmFaturaOzel FaturaOzel = new frmFaturaOzel("1");
            FaturaOzel.ShowDialog();
        }

        private void hititYazılımaBildirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hitityazilim.com/iletisim.aspx");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            //this.TopMost = false;
            frmUcGoster SatisGoster = new frmUcGoster(0, "0");
            SatisGoster.Show();
            //this.TopMost = true;

            //try
            //{

            //   // if (File.Exists(@"K:\hititprof2\bin\Debug\index.html"))
            //    Process.Start(DB.exeDizini + "\\yardim.htm");
            //}
            //catch
            //{
            //    Process.Start("http://www.hitityazilim.com/yardim.htm");
            //}
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmCikis Hak = new frmCikis();
            Hak.Text = "Hakkımızda";
            Hak.Tag = "1";
            Hak.ShowDialog();
        }

        private void btnEposta_Click(object sender, EventArgs e)
        {
            frmDestek Destek = new frmDestek();
            //frmEpostaGonder EpostaGonder = new frmEpostaGonder();
            //Destek.pFaturaTarihi.Visible = true;
            //Destek.meMesaj.Text = "";
            Destek.Show();
        }

        private void yeniHatırlatmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHatirlatmaUzat Hatirlat = new frmHatirlatmaUzat(DateTime.Now, DateTime.Now,0);
            DB.pkHatirlatmaAnimsat = 0;
            Hatirlat.ShowDialog();

            GunlukHatirlatmalar();
        }
        string konu = "",epostagonder="";

        void vHatirlatmaSesli()
        {
            DataTable dt = (DataTable)gcHatirlatma.DataSource;
            
            dockPanel2.Text = "Hatirlatma Listesi (" + dt.Rows.Count.ToString() + ")";

            if (dt.Rows.Count == 0)
            {
                //timer1.Enabled = false;
                return;
            }
            DateTime tarih;
            DateTime.TryParse(dt.Rows[0]["Tarih"].ToString(), out tarih);
            if (tarih < DateTime.Now)
            {
                //timer1.Enabled = false;

                konu = dt.Rows[0]["Konu"].ToString();
                epostagonder = dt.Rows[0]["EpostaGonder"].ToString();
                //MessageBox.Show(konu);

                string pkHatirlatma = dt.Rows[0]["pkHatirlatma"].ToString();

                DB.ExecuteSQL("update Hatirlatma set EpostaGonder=0 where pkHatirlatma=" + pkHatirlatma);

                GunlukHatirlatmalar();

                if (epostagonder == "True")
                {
                    Thread thread1 = new Thread(new ThreadStart(hatirlatma_epostasi_gonder));
                    thread1.Start();
                }

            }
        }
        void hatirlatma_epostasi_gonder()
        {
            //ses çal
            string sessonuc = formislemleri.UyariSesiCal();
            if (sessonuc != "OK")
            {
                MessageBox.Show("Ses Dosyasını Kontrol Ediniz : \n" + sessonuc);
            }

            if (Degerler.eposta.Length>5)
               DB.epostagonder(Degerler.eposta, konu, "", "Hatırlatma");
            konu = "";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //tHatirlat.Interval = 1000;
            tHatirlat.Interval = Degerler.AnimsatmaSaniyeInterval;
            if (Hatirlat == null || Hatirlat.IsDisposed)
            {
                Hatirlat = new ucHatirlatmalar();
                //sbsatis.Visible = true;
            }
            sayfayukle(Hatirlat);
           
            //frmHatirlatmaUyar uyar = new frmHatirlatmaUyar();
            //uyar.Show();
            //if (Hatirlat == null || Hatirlat.IsDisposed)
            //{
            //    Hatirlat = new ucHatirlatmalar();
            //    //sbAlis.Visible = true;
            //}
            //sayfayukle(Hatirlat);
            //tmrYedekAl.Enabled = true;
            //System.Diagnostics.Process.Start("chrome.exe", "https://chrome.google.com/webstore/detail/chrome-remote-desktop/gbchcmhmhahfdphkhkmpfmihenigjmpp");
        }

        private void KullaniciAdi_Click(object sender, EventArgs e)
        {
            KullaniciDegistir();
        }

        private void barButtonItem99_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmMusteriKarlikRaporu istatistikRaporlari = new frmMusteriKarlikRaporu();
            istatistikRaporlari.Tag = 1;
            istatistikRaporlari.Show();
        }

        private void barButtonItem113_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem112_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBaBsFormlari BaBsFormlari = new frmBaBsFormlari();
            BaBsFormlari.Show();
        }

        private void barBtnEnvanter_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmEnvanter Envanter = new frmEnvanter();
            Envanter.Show();
        }

        private void barButISmsGonder_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSmsGonder SmsGonder = new frmSmsGonder();
            SmsGonder.Show();
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageBox.Show("Analiz Ediliyor... \n\r(Yapım Aşamasında))");
        }


        private void barButtonItem110_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusterilereGenelBakis MusterilereGenelBakis = new frmMusterilereGenelBakis();
            MusterilereGenelBakis.Show();
        }

        private void barButtonItem111_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWebAyarlari WebAyarlari = new frmWebAyarlari();
            WebAyarlari.Show();
        }

        private void barButtonItem115_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokGirisCikislari StokGirisCikislari = new frmStokGirisCikislari();
            StokGirisCikislari.Show();
        }

        private void barButtonItem120_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmFaturaRaporlari SanalFaturalar = new frmFaturaRaporlari();
            SanalFaturalar.Show();
        }

        private void barButtonItem123_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmGenelDurumEkrani frmGenelDurum = new frmGenelDurumEkrani();
            frmGenelDurum.Show();

            //frmKasaGiderTablosu KasaGiderTablosu = new frmKasaGiderTablosu();
            //KasaGiderTablosu.Show();
        }

        private void barButtonItem124_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokTopluDegisiklik StokTopluDegisiklik = new frmStokTopluDegisiklik();
            StokTopluDegisiklik.Show();
        }

        private void ServerAdi_Click(object sender, EventArgs e)
        {
            frmSeo fs = new frmSeo();
            fs.Show();
        }

        private void barButtonItem125_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTedarikcilereGenelBakis TedarikcilereGenelBakis = new frmTedarikcilereGenelBakis();
            TedarikcilereGenelBakis.Show();
        }

        private void barButtonItem128_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTransferRaporu VezneRaporu = new frmTransferRaporu();
            VezneRaporu.Show();
        }

        private void bbtnisletme_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmisletmeHareketleri isletmeHareketleri = new frmisletmeHareketleri();
            isletmeHareketleri.Show();
        }

        private void barButtonItem129_ItemClick(object sender, ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void barButtonItem130_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokSatisFiyatlari StokSatisFiyatlari = new frmStokSatisFiyatlari();
            StokSatisFiyatlari.Show();
        }

        private void hatırlatmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                string v = gridView2.GetSelectedRows().GetValue(i).ToString();

                DataRow dr = gridView2.GetDataRow(int.Parse(v));
                string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

                //if (dr["animsat"].ToString() == "True")
                    DB.ExecuteSQL("update HatirlatmaAnimsat set fkDurumu=2 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
                //else
                //  DB.ExecuteSQL("update HatirlatmaAnimsat  set fkDurumu=1 where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

                yenihatirlatmaekle(pkHatirlatmaAnimsat);
            }

            GunlukHatirlatmalar();

        }

        private bool yenihatirlatmaekle(string pkHatirlatmaAnimsat)
        {
            DataTable dt =
            DB.GetData("select * from HatirlatmaAnimsat with(nolock) where pkHatirlatmaAnimsat="+
                pkHatirlatmaAnimsat);

            if(dt.Rows.Count>0)
            {
                string gunsonra = dt.Rows[0]["gun_sonra"].ToString();

                if (gunsonra == "") gunsonra = "0";

                if (gunsonra != "0")
                {
                    DB.ExecuteSQL("UPDATE HatirlatmaAnimsat SET " +
                   "fkDurumu=1,arandi=0,animsat=1,animsat_zamani=DATEADD(day,gun_sonra,animsat_zamani)," +
                   "Tarih=DATEADD(day,gun_sonra,Tarih)," +
                   "BitisTarihi=DATEADD(day,gun_sonra,BitisTarihi) where pkHatirlatmaAnimsat=" +
                    pkHatirlatmaAnimsat);
                }
            }

            return true;
        }

        private void barButtonItem132_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAktarimGonderAl Aktarim = new frmAktarimGonderAl();
            Aktarim.Show();

            //frmAktarimWeb AktarimWeb = new frmAktarimWeb();
            //AktarimWeb.Show();
        }

        private void bbtniSiparisHizli_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSiparisHizli siparis = new frmSiparisHizli();
            siparis.Show();
        }

        private void barBtngunlukKurlar_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmGunlukKur GunlukKur = new frmGunlukKur(false);
            GunlukKur.Show();
        }

        private void barButtonItem133_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLogs log = new frmLogs();
            log.Show();
        }

        private void barButtonItem134_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKullaniciRaporlari KullaniciRaporlari = new frmKullaniciRaporlari();
            KullaniciRaporlari.Show();
        }

        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmAyarlar isyeri = new frmAyarlar(1);
            isyeri.ShowDialog();
        }

        private void barButtonItem30_ItemClick(object sender, ItemClickEventArgs e)
        {


            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '*';
            sifregir.ShowDialog();
            if (sifregir.Girilen.Text == "9999*")
            {
                frmAktarim Aktar = new frmAktarim();
                Aktar.Show();
            }
            else
            {
                frmMesajBox mesaj = new frmMesajBox(200);
                mesaj.label1.Text = "Hatalı Şifre";
                mesaj.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmYedekAl YedekAl = new frmYedekAl();
            YedekAl.Show();
        }

        private void barButtonItem135_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAktar DisveriAl = new frmAktar();
            DisveriAl.ShowDialog();
        }
        
        private void barButtonItem137_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriPlasiyerRaporu MusteriPlasiyerRaporu = new frmMusteriPlasiyerRaporu();
            MusteriPlasiyerRaporu.Show();
        }

        private void barButtonItem138_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriStokSatisFiyatlari MusteriStokSatisFiyatlari = new frmMusteriStokSatisFiyatlari();
            MusteriStokSatisFiyatlari.Show();
        }

        private void barBtnVeriGonder_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAktarimWeb AktarimWeb = new frmAktarimWeb();
            AktarimWeb.Show();
        }

        private void barBtnVeriAktarim_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAktarimGonderAlKontrol Aktarim = new frmAktarimGonderAlKontrol();
            Aktarim.Show();
        }

        private void barButtonItem144_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAcikSatislarveAlislar AcikSatislarveAlislar = new frmAcikSatislarveAlislar();
            AcikSatislarveAlislar.Show();
        }

        private void barButtonItem145_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmHataliKayitlar HataliKayitlar = new frmHataliKayitlar();
            HataliKayitlar.Show();
        }

        private void bbtnDevir_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmDevirAktar devir = new frmDevirAktar();
            devir.Show();
        }

        private void sbtbDuyurular_Click(object sender, EventArgs e)
        {
            frmDuyurular duyurular = new frmDuyurular();
            duyurular.Show();
        }

        private void barButtonItem146_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmGenelDurumEkrani GenelDurumEkrani = new frmGenelDurumEkrani();
            GenelDurumEkrani.Show();
        }

        private void frmAnaForm_MouseMove(object sender, MouseEventArgs e)
        {
            //Degerler.sessiotimeout=0;
        }

        private void frmAnaForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //keypreviev=true; ise çalışıyor
            //Degerler.sessiotimeout = 0;
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            frmCikis Cikis = new frmCikis();
            Cikis.ShowDialog();
        }

        private void bbiTeklifRaporlari_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmTeklifRaporlari TeklifRaporlari = new frmTeklifRaporlari();
            TeklifRaporlari.Show();
        }

        private void bbiBankaHareketleri_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            frmBankaHareketleri bankahareketleri = new frmBankaHareketleri();
            bankahareketleri.Show();
        }

        private void bbiStokKartiDepo_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokDepoMevcut depostokdurumu = new frmStokDepoMevcut();
            depostokdurumu.Show();
        }

        private void barButtonItem148_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokDepoMevcut StokKartiDepo = new frmStokDepoMevcut();
            StokKartiDepo.Show();
        }

        private void barButtonItem149_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void bbiDepoTransfer_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DepoTransfer == null || DepoTransfer.IsDisposed)
            {
                DepoTransfer = new ucDepoTransfer();
                //sbsatis.Visible = true;
            }
            sayfayukle(DepoTransfer);

            //frmDepoTransfer DepoTransfer = new frmDepoTransfer();
            //DepoTransfer.Show();
        }

        private void bbiDepoTalepleri_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokDepoTalepleri StokDepoTalepleri = new frmStokDepoTalepleri();
            StokDepoTalepleri.Show();
        }

        private void barButtonItem153_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmVardiyaCizelgesi VardiyaCizelgesi = new frmVardiyaCizelgesi();
            VardiyaCizelgesi.Show();
        }

       
        private void müşteriHareketleriToolStripMenuItem1_Click(object sender, EventArgs e)
        {
              if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string fkfirma = dr["fkfirma"].ToString();

            if (fkfirma == "") return;
            frmMusteriHareketleri MusteriHareketleri = new frmMusteriHareketleri();
            MusteriHareketleri.musteriadi.Tag = fkfirma; 
            MusteriHareketleri.Show();
        }

        private void barButtonItem154_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmHatirlatHizmetSenet s = new frmHatirlatHizmetSenet();
            s.Show();
        }

        private void bbtbTransferRaporlari_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTransferRaporlari TransferRaporlari = new frmTransferRaporlari();
            TransferRaporlari.Show();
        }

        private void bbtnMusteriKontaklari_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriKontaklari mk = new frmMusteriKontaklari();
            mk.Show();
        }

        private void frmAnaForm_Shown(object sender, EventArgs e)
        {
            if (Degerler.giris_yapildi)
            {
                //formislemleri.Mesajform("Açıldı", "S", 100);
                tmrYedekAl.Enabled = true;
                tHatirlat.Enabled = true;
            }
        }

        private void bbiRandevular_ItemClick(object sender, ItemClickEventArgs e)
        {
            sayfayukle(RandevuVer);
            RandevuVer.VisibleChanged += RandevuVer_VisibleChanged;
            //if (Randevular == null || Randevular.IsDisposed)
            //{
            //    Randevular = new ucRandevuVer();
            //    //sbAlis.Visible = true;
            //}
            //sayfayukle(Randevular);
        }

        private void btnRandevular_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (Randevular == null || Randevular.IsDisposed)
            //{
            //    Randevular = new ucRandevuVer();
            //    sbRandevu.Visible = true;
            //}
            sayfayukle(RandevuVer);
            RandevuVer.VisibleChanged += RandevuVer_VisibleChanged;
        }

        private void tHatirlat_Tick(object sender, EventArgs e)
        {
            tHatirlat.Interval = 60000;
            Degerler.AnimsatmaSaniyeInterval = 60000;
            //Degerler.isHatirlatmaUyar
           if (Degerler.isHatirlatmaUyar)// && Degerler.dakikam != DateTime.Now.ToString("mm"))// && btnUyar.Visible)
            {
                //Degerler.dakikam = DateTime.Now.ToString("mm");
                //Degerler.isHatirlatmaUyar = true;
                //Degerler.isHatirlatmaAcikmi = true;
                //eğer Hatırmatmaları uyar açıksa ekran göster

                HatirlatmaUyar();

                //Degerler.isHatirlatmaAcikmi = false;
                //Thread thread1 = new Thread(new ThreadStart(HatirlatmaUyar));
                //thread1.Start();
            }
        }

        private void HatirlatmaUyar()
        {
            //DataTable dt = DB.GetData(@"
            //declare @mintar datetime2
            //declare @maxtar datetime2
            //set @mintar = cast (GETDATE() as DATE) 
            //set @maxtar = DATEADD(ns, -100, DATEADD(s, 86400, @mintar))
            //Select * from HatirlatmaAnimsat with(nolock) where fkDurumu=1 and animsat_zamani>=@mintar and animsat_zamani<=@maxtar
            //order by animsat_zamani");
            DataTable dt = DB.GetData(@"Select * from HatirlatmaAnimsat with(nolock) "+
" where fkDurumu=1 and animsat_zamani<getdate() and (fkKullanicilar is null or fkKullanicilar="+DB.fkKullanicilar+")"+
" order by animsat_zamani");
            int c = dt.Rows.Count;
            btnUyar.Text = c.ToString();
            //btnUyar.Tag = c.ToString();

            if (c == 0) return;

            //string uz = dt.Rows[0]["uyari_zamani"].ToString();
            //string uz = dt.Rows[0]["uyari_zamani"].ToString();
            //DateTime uzamani=Convert.ToDateTime(uz);

            //if (Degerler.isHatirlatmaUyar)
            //{
            //    Digerislemler.AnimsaticiSesCal();
            //    frmHatirlatmaUyar hu = new frmHatirlatmaUyar();
            //    Degerler.isHatirlatmaUyar = false;//açıkken tekrar uyarma
            //    hu.Show();
            //}
            //if (Degerler.isHatirlatmaUyar)
            {
                notifyIcon1.BalloonTipText = dt.Rows[0]["konu"].ToString();
                notifyIcon1.BalloonTipTitle = btnUyar.Text + " tane hatırlatmanız var!";
                notifyIcon1.Text = "Hitit Prof 2";
                notifyIcon1.ShowBalloonTip(100);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Hatirlat == null || Hatirlat.IsDisposed)
            {
                Hatirlat = new ucHatirlatmalar();
                //sbsatis.Visible = true;
            }
            sayfayukle(Hatirlat);

            //frmHatirlatGunluk hg = new frmHatirlatGunluk();
            //hg.Show();
        }

        private void dakikaSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(5, Degerler.ZamanDilimi.Dakika);
        }

        void sonraAnimsat(int dk,Degerler.ZamanDilimi zd)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();
            if(Degerler.ZamanDilimi.Dakika== zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(minute,"+ dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            else if (Degerler.ZamanDilimi.Saat == zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(hour," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);
            else if (Degerler.ZamanDilimi.Gun == zd)
                DB.ExecuteSQL("update HatirlatmaAnimsat set animsat_zamani=DATEADD(day," + dk + ",getdate()) where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            GunlukHatirlatmalar();
        }

        private void dakikaSonraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sonraAnimsat(10, Degerler.ZamanDilimi.Dakika);
        }

        private void dakikaSonraToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            sonraAnimsat(15, Degerler.ZamanDilimi.Dakika);
        }

        private void yarımSaatSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(30, Degerler.ZamanDilimi.Dakika);
        }

        private void saatSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(1, Degerler.ZamanDilimi.Saat);
        }

        private void saatSonraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sonraAnimsat(2, Degerler.ZamanDilimi.Saat);
        }

        private void günSonraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonraAnimsat(1, Degerler.ZamanDilimi.Gun);
        }

        private void fişBilgisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string fkSatislar = dr["fkSatislar"].ToString();
            if (fkSatislar == "") return;

            frmFisSatisGecmis fis = new frmFisSatisGecmis(false);
            fis.fisno.Text = fkSatislar;
            fis.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmHatirlatmaUyar uyar = new frmHatirlatmaUyar();
            uyar.Show();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            frmRandevuVer RandevuVer = new frmRandevuVer();
            RandevuVer.Show();
        }

        private void kapatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnKapat_Click(sender, e);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            //MessageBox.Show("aç");
            this.Activate();
            this.Show();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            if (Hatirlat == null || Hatirlat.IsDisposed)
            {
                Hatirlat = new ucHatirlatmalar();
                //sbsatis.Visible = true;
            }
            sayfayukle(Hatirlat);
        }

        private void diğerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dtpSaat
        }

        private void barButtonItem155_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriGelirGider MusteriGelirGider = new frmMusteriGelirGider();
            MusteriGelirGider.Show();
        }

        private void barButtonItem147_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem113_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmRaporGoster rg = new frmRaporGoster();
            rg.ShowDialog();
        }

        private void hakkımızdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hitityazilim.com/iletisim.aspx");
        }

        private void barButtonItem156_ItemClick(object sender, ItemClickEventArgs e)
        {
           // if (Randevular == null || Randevular.IsDisposed)
            //{
                ucSatisMasa  masa = new ucSatisMasa();
                //sbAlis.Visible = true;
            //}
            sayfayukle(masa);
        }

        private void bBtnOzelRaporlar_ItemClick(object sender, ItemClickEventArgs e)
        {
          
        }

        private void özelRaporlarıGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread tRaporgonder = new Thread(new ThreadStart(raporgonder));
            tRaporgonder.Start();
        }
        void raporgondercsv()
        {
            string mail = formislemleri.inputbox("E-Posta Adresi Giriniz", "Gönderilecek E-Posta", "@", false);
            if (mail.Length < 5) return;

            DataTable dt = DB.GetData("select * from OzelRaporlar with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sql = dt.Rows[i]["rapor_sql"].ToString();

                DataTable dt2 = DB.GetData(sql);
                string dosyaadi = Application.StartupPath + "\\OzelRapor" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
                Digerislemler.CreateCSVFile(dt2, dosyaadi);

                DB.epostagonder(mail, "Özel Rapor Mesaj", dosyaadi, "OzelRapor Konu");

                Process.Start(dosyaadi);

                formislemleri.Mesajform("E-Posta Gönderildi.", "S", 100);
            }
        }
        void raporgonder()
        {
            DataTable dtMail = DB.GetData("select Sirket,eposta from Sirketler with(nolock)");
            if (dtMail.Rows.Count == 0)
            {
                formislemleri.Mesajform("e-posta adresini kontrol ediniz!", "K", 150);
                return;
            }
            string mail = dtMail.Rows[0]["eposta"].ToString();
            string sirket = dtMail.Rows[0]["Sirket"].ToString();
            //string mail = formislemleri.inputbox("E-Posta Adresi Giriniz", "Gönderilecek E-Posta", "gurbuzadem@gmail.com", false);
            if (mail.Length < 5)
            {
                formislemleri.Mesajform("e-posta adresini hatalı!", "K", 150);
                return;
            }
            frmRaporlarOzel raporozel = new frmRaporlarOzel();

            DataTable dt = DB.GetData("select * from OzelRaporlar with(nolock)");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                raporozel.gcSql.DataSource = null;
                string sql = dt.Rows[i]["rapor_sql"].ToString();
                string rapor_adi = dt.Rows[i]["rapor_adi"].ToString();
                DataTable dt2 = DB.GetData(sql);
                raporozel.gcSql.DataSource = dt2;
                //DevExpress.XtraGrid.GridControl grid = new DevExpress.XtraGrid.GridControl();
                //DevExpress.XtraGrid.Views.Grid.GridView gridview = new DevExpress.XtraGrid.Views.Grid.GridView();
                //for (int j = 0; j < dt2.Rows.Count - 1; j++)
                //{
                //    gridview.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn() { FieldName = dt2.Columns[j].ToString() });
                //    //gridview.Columns.AddField(dt2.Columns[j].ToString());
                //}
                //gridview.Columns.Clear();
                //grid.Dock = DockStyle.Fill;
                //gridview.BeginDataUpdate();

                //grid.ViewCollection.Add(gridview);
                //grid.MainView = gridview;
                //grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridview });
                //grid.Name = "gcGrid";
                //gridview.GridControl = grid;
                //gridview.Name = "gvGridView";
                //gridview.OptionsView.ShowAutoFilterRow = true;
                //GridColumn column = gridview.Columns.Add();
                //column.Caption = "Name";
                //column.FieldName = "Name";
                //column.Visible = true;
                //this.Controls.Add(grid);
                //grid.ForceInitialize();
                //grid.MainView.PopulateColumns();
                //grid.ShowPrintPreview();
                //this.Controls.Remove(grid);
                //gc.ExportToXls();
                //gc.RefreshDataSource();

                //grid.DefaultView.PopulateColumns();
                //
                //grid.DataSource = dt2;
                //raporozel.gridView1.PopulateColumns();
                //grid.DefaultView.PopulateColumns();
                //gridview.EndDataUpdate();

                //raporozel.Show();



                string dosyaadi = Application.StartupPath + "\\OzelRapor" + DateTime.Now.ToString("yyMMddHHmmss") + ".Xls";
                //gridview.ExportToXls(dosyaadi);
                raporozel.gridView1.ExportToXls(dosyaadi);
                //Process.Start(dosyaadi);

                DB.epostagonder(mail, rapor_adi + " Raporu", dosyaadi, sirket);

                //Process.Start(dosyaadi);

                //formislemleri.Mesajform("E-Posta Gönderildi.", "S", 100);

                //Yazdirma_islemleri yi = new Yazdirma_islemleri();
                //yi.GridYazdir(gc, "A4");
                raporozel.gridView1.BeginDataUpdate();
                raporozel.gcSql.DataSource = null;
                raporozel.gridView1.Columns.Clear();
                for (int c = 0; c < raporozel.gridView1.Columns.Count; c++)
                {
                    raporozel.gridView1.Columns.RemoveAt(c);
                }
             }
      
            raporozel.Dispose();
            formislemleri.Mesajform("Raporlar Gönderildi-" + mail, "S", 100);
            //MessageBox.Show("Raporlar Gönderildi.");
        }

        private void barButtonItem68_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaTransfer trasn = new frmKasaTransfer();
            trasn.ShowDialog();
        }

        private void barButtonItem157_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmRaporlarOzel raporozel = new frmRaporlarOzel();
            raporozel.Show();
        }

        private void barButtonItem21_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            KullaniciDegistir();
        }

        private void barButtonItem114_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmGunSonuRaporlari gunsonu = new frmGunSonuRaporlari();
            gunsonu.Show();
        }

        private void barButtonItem50_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmGunSonuRaporlari gunsonu = new frmGunSonuRaporlari();
            gunsonu.Show();
        }

        private void bbiMusteriKarlikRaporu_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmMusteriKarliRaporu mrv = new frmMusteriKarliRaporu();
            mrv.Show();
        }

        private void barButtonItem141_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucProjeListesi projelistesi = new ucProjeListesi();
            //projelistesi.Show();
            panelControl.Controls.Add(projelistesi);
            projelistesi.Dock = DockStyle.Fill;
            projelistesi.Show();
            projelistesi.Focus();
            projelistesi.BringToFront();
            projelistesi.VisibleChanged += Projelistesi_VisibleChanged;
        }

        private void Projelistesi_VisibleChanged(object sender, EventArgs e)
        {
            //if(((UserControl)sender).Visible = false)
            ((UserControl)sender).Parent= null;
            //(sender)this.Visible = false;
            //throw new NotImplementedException();
        }

        private void barButtonItem158_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucDepoIstek _DepoIstek = new ucDepoIstek();
            //panelControl.Controls.Add(_DepoIstek);
            //_DepoIstek.Dock = DockStyle.Fill;
            _DepoIstek.Show();
            //_DepoIstek.Focus();
            //_DepoIstek.BringToFront();
            //_DepoIstek.VisibleChanged += Projelistesi_VisibleChanged;
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            string s = formislemleri.MesajBox("Hatırlatma Silinsin mi?", "Randevu Sil", 1, 1);

            if (s == "0") return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            string pkHatirlatmaAnimsat = dr["pkHatirlatmaAnimsat"].ToString();

            DB.ExecuteSQL("delete from  HatirlatmaAnimsat where pkHatirlatmaAnimsat=" + pkHatirlatmaAnimsat);

            GunlukHatirlatmalar(); 
        }

        private void btnSayim_Click(object sender, EventArgs e)
        {
            sayfayukle(StokSayimi);
            btnSayim.VisibleChanged += Sayim_VisibleChanged;
            //sbsatis.Visible = true;
            //sayfayukle3("ucSatis");
        }

        private void Sayim_VisibleChanged(object sender, EventArgs e)
        {
            StokSayimi = null;
            btnSayim.Visible = false;
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void bBtnSatisCallerId_ItemClick(object sender, ItemClickEventArgs e)
        {
            sayfayukle(SatisCallerId);
            SatisCallerId.VisibleChanged += SatisCaller_VisibleChanged;
        }

        private void SatisCaller_VisibleChanged(object sender, EventArgs e)
        {
           SatisCallerId = null;
           sbCallerIdSatis.Visible = false;
            //throw new NotImplementedException();
        }
        private void sbCallerIdSatis_Click(object sender, EventArgs e)
        {
            sayfayukle(SatisCallerId);
            SatisCallerId.VisibleChanged += Satis_VisibleChanged1;
        }

        private void barButtonItem159_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmStokDepoKalanMevcut StokDepoKalanMevcut = new frmStokDepoKalanMevcut();
            StokDepoKalanMevcut.Show();
        }

        private void sbRandevu_Click(object sender, EventArgs e)
        {
            sayfayukle(RandevuVer);
            RandevuVer.VisibleChanged += RandevuVer_VisibleChanged;
        }

        private void bBtnCallerIdRandevu_ItemClick(object sender, ItemClickEventArgs e)
        {
            sayfayukle(RandevuSpa);
            RandevuSpa.VisibleChanged += SatisRandevuSpa_VisibleChanged;
            
            //ucSatisRandevuSpa SatisRandevuSpa = new ucSatisRandevuSpa();
            //SatisRandevuSpa.VisibleChanged += SatisRandevuSpa_VisibleChanged;
            //panelControl.Controls.Add(SatisRandevuSpa);
            //SatisRandevuSpa.Dock = DockStyle.Fill;
            //SatisRandevuSpa.Show();
            //sbRandevuSpa.Visible = true;
            //SatisRandevuSpa.Focus();
            //SatisRandevuSpa.BringToFront();
        }
        void SatisRandevuSpa_VisibleChanged(object sender, EventArgs e)
        {
            RandevuSpa = null;
            sbRandevuSpa.Visible = false;
        }

        private void barButtonItem160_ItemClick(object sender, ItemClickEventArgs e)
        {
            DB.PkFirma = 0;
            //if (CariKartlar == null || CariKartlar.IsDisposed)
            //{
                ucMusteriAidatListesi MusteriAidatListesi = new ucMusteriAidatListesi();
                //sbMusteriler.Visible = true;
            //}
            sayfayukle(MusteriAidatListesi);
            //sbMusteriler.Visible = true;
        }

        private void barButtonItem161_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmDepoKarti depolar = new frmDepoKarti();
            depolar.ShowDialog();
        }

        private void frmAnaForm_VisibleChanged(object sender, EventArgs e)
        {
            panelControl.Focus();
        }

        private void frmAnaForm_Activated(object sender, EventArgs e)
        {
            panelControl.Focus();
            panelControl.Controls[0].Focus();
           // panelControl.Controls[0].Visible = true;
        }

        private void frmAnaForm_Deactivate(object sender, EventArgs e)
        {
            panelControl.Focus();
        }

        private void frmAnaForm_Leave(object sender, EventArgs e)
        {
            panelControl.Focus();
        }

        private void frmAnaForm_ParentChanged(object sender, EventArgs e)
        {
            panelControl.Focus();
        }

        private void bbtRandevu_ItemClick(object sender, ItemClickEventArgs e)
        {
            

            frmTestComponet tc = new frmTestComponet();
            tc.ShowDialog();

            //sayfayukle(RandevuVerYeni);
            //RandevuVerYeni.VisibleChanged += RandevuVerYeni_VisibleChanged;
        }
        private void RandevuVerYeni_VisibleChanged(object sender, EventArgs e)
        {
            RandevuVerYeni = null;
            RandevuVerYeni.Visible = false;
            //throw new NotImplementedException();
        }

        private void barButtonItem162_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucSatisMasalar masa = new ucSatisMasalar();
            //sbAlis.Visible = true;
            //}
            sayfayukle(masa);
        }

        private void barButtonItem131_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmKasaSayimSonucu kasakapanis = new frmKasaSayimSonucu();
            kasakapanis.ShowDialog();
        }

        private void btnRandevuVer_Click(object sender, EventArgs e)
        {
            frmRandevuVer RandevuVer = new frmRandevuVer();
            //RandevuVer.Show();

            panelControl.Controls.Add(RandevuVer);
            RandevuVer.Dock = DockStyle.Fill;
            RandevuVer.Show();
            RandevuVer.Focus();
            RandevuVer.BringToFront();
        }
      }
    }
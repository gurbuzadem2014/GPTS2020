using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using GPTS.Include.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Reflection;
using System.Diagnostics; //api işlemleri kullanmak için
using System.ServiceProcess;
using System.Threading;
using GPTS.islemler;
using System.Configuration;
using Microsoft.Win32;
using System.Linq;
using System.Globalization;

namespace GPTS
{
    public partial class frmGiris : XtraForm
    {
        int sifre_hatali_say = 0;
        bool yetkili = false; 
        string iniDosya = "";
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string sekme, string anahtar, string deger, string inidosya);//ini dosyası yazma apisi
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string sekme, string anahtar, string def, StringBuilder retVal, int size, string inidosya); //ini dosyasından okuma apisi

        public void iniYaz(string sekme, string anahtar, string deger)
        {
            WritePrivateProfileString(sekme, anahtar, deger, iniDosya);
        }

        //FormScreenKeyboard formKbd = new FormScreenKeyboard();
        public frmGiris()
        {
            InitializeComponent();
        }
        
        private void frmGiris_Load(object sender, EventArgs e)
        {
            //3kvYs/OSFFf9i4jh+sZEVQ==
            //string sifre= islemler.CryptoStreamSifreleme.md5Sifrele("TEKsql@3653");
            
            cbDil.SelectedIndex = 0;
            //sanal klavye
            //formKbd.Left = this.Left + this.Width;
            //formKbd.Top = this.Top;
            //formKbd.Show();

            //data_katman.DB db = new data_katman.DB();
            //data_katman.StokKarti sk = new data_katman.StokKarti();
            //sk.Stokadi = "stokadi";

            //data_katman.MTP2012Entities ent = new data_katman.MTP2012Entities();

            //business_katman.stok.StokRepository stoklar = new business_katman.stok.StokRepository();
            //DataTable dt  = stoklar.TumStoklar();

            BaglantiAyarlariGetir();
            //DB.ExecuteSQL("begin transaction tran_kasa");
            //DB.ExecuteSQL("update KasaHareket set fkKasalar=2 where pkKasaHareket= 12590");
            //DB.ExecuteSQL("ROLLBACK transaction tran_kasa");
            //DB.ExecuteSQL("COMMIT TRANSACTION tran_kasa");

            GrisiVerKulSube();

            #region lisans kontrol ve ilk kurulum
            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            //string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string Callerid = dtSirket.Rows[0]["Callerid"].ToString();
            string oto_guncelle = dtSirket.Rows[0]["oto_guncelle"].ToString();
            
            if (Callerid == "True")
            {
                Degerler.caller_id_sirket = true;
            }
            else
                Degerler.caller_id_sirket = false;

            tsslVol.Visible = toolStripStatusLabel5.Visible= false;

            //tsslVol.Text= dtSirket.Rows[0]["versiyon"].ToString();

            if (eposta == "")
            {
                frmYeniKullanici yeni = new frmYeniKullanici();
                yeni.ShowDialog();

                //SqlYuklumu();
            }
            //FileLogaYaz("Giriş Yapıldı");
            //DBLogaYaz("Giriş Yapıldı");
            #endregion

            //Thread thread2 = new Thread(new ThreadStart(versiyon_kontrol));
            //thread2.Start();

            if(oto_guncelle=="True")
               versiyon_kontrol();
            //#if DEBUG
            //txtSifre.Text = "9999*";
            //#endif
        }

        private void SqlYuklumu()
        {
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);

            //http://www.serefakyuz.com/2011/08/sharpta-sql-veritabann-da-eren-program-setup-oluturma.html

            //http://www.csharpnedir.com/articles/read/?id=1032

            //https://social.technet.microsoft.com/Forums/tr-TR/9c835e31-5e89-46b7-b4e7-8820e0a1e7c9/windows-uygulamas-ile-birlikte-bir-veritaban-kurmak?forum=sqltr

            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names
            //Eğer bu bilgisayarda SQL SERVER veya SQLSERVEREXPRESS sürümü yüklendi ise yukarıda regedit bölümünde yüklü SQL SERVER instance'leri yer alacaktır.           
            string[] yuklusqller = (string[])Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Microsoft SQL Server").GetValue("InstalledInstances");
            //Eğer kullanıcının bilgisayarında SQLExpress yüklü değilse
            //var yukluozellikle(from s in yuklusqller where s.Contains("SQLEXPR_x64_ENU") select s).FirstOrDefault();
            //if (yukluozellikler == null)
            //{
                DialogResult sonuc = MessageBox.Show("Programı kullanabilmek için SQLEXPRESS gereklidir. Bu Programı Yüklemek istiyor musunuz?", "UYARI", MessageBoxButtons.YesNo);
                if (sonuc == DialogResult.Yes)
                {
                    string path = exeyol+ "\\sqlkur\\SqlLocalDB.msi";
                        //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SQLEXPR_x64_ENU.exe";
                    Process pro = new Process();
                    pro.StartInfo.FileName = path;
                    //Aşağıdaki parametreleri SQLEXPRESS setup dosyama göndererek kurulumu başlatırsam kullanıcıya kurulum yeri v.s gibi bilgileri sormayacak ve doğrudan kuruluma geçecektir.
                    //pro.StartInfo.Arguments = "/qb INSTANCENAME=\"SQLEXPRESS\" INSTALLSQLDIR=\"C:\\Program Files\\Microsoft SQL Server\" INSTALLSQLSHAREDDIR=\"C:\\Program Files\\Microsoft SQL Server\" INSTALLSQLDATADIR=\"C:\\Program Files\\Microsoft SQL Server\" ADDLOCAL=\"All\" SQLAUTOSTART=1 SQLBROWSERAUTOSTART=0 SQLBROWSERACCOUNT=\"NT AUTHORITY\\SYSTEM\" SQLACCOUNT=\"NT AUTHORITY\\SYSTEM\" SECURITYMODE=SQL SAPWD=\"\" SQLCOLLATION=\"SQL_Latin1_General_Cp1_CS_AS\" DISABLENETWORKPROTOCOLS=0 ERRORREPORTING=1 ENABLERANU=0";
                    //Process için pencere oluştur
                    pro.StartInfo.CreateNoWindow = true;
                    //Process arka planda çalışsın.
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.Start();
                    //İSQL kurulumu bitene kadar bekle
                    pro.WaitForExit();

                    //string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
                    System.Diagnostics.Process.Start(exeyol + "\\HTTKurulum.exe");

                veritabanirestore();
                kullaniciolustur();
            }
                else
                {
                    formislemleri.Mesajform(yuklusqller + " Zaten Yüklü","K",120);
                    //SQLEXPRESS'i kurmak istemiyorsa programı sonlandır
                    this.Close();
                }
           // }
            //else
              //  formislemleri.Mesajform(yuklusqller + " Zaten Yüklü", "K", 120);
        }

        void veritabanirestore()
        {
            string uygulama_dizin = Path.GetDirectoryName(Application.ExecutablePath);

            string sqlklasor = uygulama_dizin + "\\sqlkur";
            string sqldataklasor = uygulama_dizin + "\\data";

            if (!Directory.Exists(sqlklasor))
                Directory.CreateDirectory(sqlklasor);


            if (!Directory.Exists(sqldataklasor))
                Directory.CreateDirectory(sqldataklasor);

            if (!File.Exists(sqlklasor + "\\db.bak"))
            {
                MessageBox.Show("Yedek Dosyası Bulunamadı");
                return;
            }

            if (File.Exists(sqldataklasor + "\\MTP2012.mdf"))
            {

                MessageBox.Show("Veritabanı Dosyası Bulunmaktadır");
                return;
            }



            // string s = formislemleri.MesajBox("c:\\data\\db.bak dosyası veritabanı oluşturulacaktır! ", "Yedek " + cbVeriTabaniAdi.Text + " Restore", 2, 1);
            //if (s == "0") return;

            string sql = "RESTORE DATABASE MTP2012";// +cbVeriTabaniAdi.Text;
            sql = sql + " FROM DISK = '" + sqlklasor + "\\db.bak'";
            sql = sql + " with move 'akincilar' to '" + sqldataklasor + "\\MTP2012.mdf',";
            sql = sql + " move 'akincilar_log' to '" + sqldataklasor + "\\MTP2012.ldf',";
            sql = sql + " replace";
            DB.GetData(sql);
            //if (sonuc == -1)
            //{
            //    this.Text = "Veritabanı Oluşturuldu";

            //    MessageBox.Show("Kurulum Tamamlandı. \r Masaüstünde Hitit Prof Kısayolunu Aç Yapabilirsiniz \r Şifre =1");

            //    //string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            //    //System.Diagnostics.Process.Start(exedizini + "\\httkur\\HititKurulum.msi");
            //}
            //else
            //{
            //    //MessageBox.Show(DB.hata);
            //    this.Text = "Veritabanı Oluşturulamadı";
            //}
        }

        void kullaniciolustur()
        {
            //sifele.sifremi_sifrele ss = new sifele.sifremi_sifrele();
            //string sifre  =ss.SifremiSifrele("ewrsdvmlkdnvkfd");
            string sifre = "hitit9999";


            string sql = @"CREATE LOGIN [hitityazilim] WITH PASSWORD='@sifre',
            DEFAULT_DATABASE=[MTP2012], 
            DEFAULT_LANGUAGE=[us_english], 
            CHECK_EXPIRATION=OFF, CHECK_POLICY=ON";
            sql = sql.Replace("@sifre", sifre);
            //ddb = new DB();
            //if (!ddb.DBOpen())
           // {
            //    MessageBox.Show("Bağlantı sağlanamadı!");
           // }

            DB.GetData(sql);
            //MessageBox.Show(DB.hata);
            //DataTable dt = ddb.GetData("select getdate()");


            //string sonuc = "";//formislemleri.inputbox("Şifre Giriniz", "Şifre", "", true);
            //if (sonuc == "hitit9999")
            //{
            //    sql = sql.Replace("@sifre", sonuc);
            //    int s = 0;// DB.ExecuteSQLSa(sql);
            //    if (s != 0)
            //    {
            //        //DB.ExecuteSQLSa("EXEC sys.sp_addsrvrolemember @loginame = N'hitityazilim', @rolename = N'sysadmin'");

            //        //DB.ExecuteSQLSa("ALTER LOGIN [hitityazilim] DISABLE");

            //        //formislemleri.Mesajform("Kullanıcı Oluşturuldu", "S", 150);
            //    }
            //    //else
            //    //  formislemleri.Mesajform("Hatalı Şifre", "K", 150);
            //}
        }

        void FileLogaYaz(string mesaj)
        {
            //Logger.MyLogger ml = new Logger.MyLogger();
            //ml.AddLog(mesaj,"File","c:\\Data");
        }
        void DBLogaYaz(string mesaj)
        {
            //Logger.MyLogger ml = new Logger.MyLogger();
            //ml.AddLog(mesaj, "Sql","");

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //button1_Click(sender, e);//şifrele geçici sonra kaldır. 19.05.2017

            if (ipadres.Visible == true)
            {
                ipadres.Visible = false;
                teVeriTabaniKul.Visible = false;
                teVeritabaniSifre.Visible = false;            
            }

            BaglantiAyarlariKaydet();

            if (lueAdi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Kullanıcı Adı Giriniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtSifre.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Şifre Giriniz!",Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataTable dt = null;
            if (txtSifre.Text == "9999*")
            {
                dt = DB.GetData("select top 1 * from Kullanicilar with(nolock)");
            }
            else
            {
                //string str = @"select k.fkSube from Kullanicilar k with(nolock)
                //inner join Depolar d with(nolock) on d.pkDepolar = k.fkDepolar
                //where k.pkKullanicilar = 1";
                dt = DB.GetData("select * from Kullanicilar with(nolock) where KullaniciAdi='" + lueAdi.Text + "' and Sifre='" + txtSifre.Text + "'");
            }

            if (dt.Rows.Count > 0)
            {
                sifre_hatali_say = 0;
                yetkili = true;
                DB.kul = lueAdi.Text;
                DB.fkKullanicilar = dt.Rows[0]["pkKullanicilar"].ToString();
                DB.Yetkilikodu = dt.Rows[0]["Yetkilikodu"].ToString();

                int iAktifForm = 0;
                int.TryParse(dt.Rows[0]["AktifForm"].ToString(),out iAktifForm);
                Degerler.iAktifForm = iAktifForm;

                ipadres.Visible = false;
                
                if (dt.Rows[0]["AcikMi"].ToString() == "True")
                {
                   // islemler.formislemleri.Mesajform("Kullanıcı Aktif Görünmektedir","K");
                    //XtraMessageBox.Show("Kullanıcı Aktif Görünmektedir", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                   // yetkili = false;
                   // return;
                }
                DB.ExecuteSQL("update Kullanicilar set CikisZamani=null,GirisZamani=getdate(),AcikMi=1 where pkKullanicilar=" + DB.fkKullanicilar);

                int fkSatisDurumu=2;
                int.TryParse(dt.Rows[0]["fkSatisDurumu"].ToString(), out fkSatisDurumu);
                Degerler.fkSatisDurumu=fkSatisDurumu;

                if (dt.Rows[0]["AnaBilgisayar"].ToString() == "True")
                   Degerler.AnaBilgisayar = true;

                if (dt.Rows[0]["acilista_hatirlatma_ekrani"].ToString() == "True")
                    Degerler.acilista_hatirlatma_ekrani = true;

                if (dt.Rows[0]["acilista_caller_id"].ToString() == "True")
                    Degerler.caller_id_kullanici = true;

                if (dt.Rows[0]["hatirlatma_uyar"].ToString() == "True")
                    Degerler.isHatirlatmaUyar = true;


                string fkDepolar = "1";
                if (dt.Rows[0]["fkDepolar"].ToString() != "")
                    fkDepolar = dt.Rows[0]["fkDepolar"].ToString();
                Degerler.fkDepolar = int.Parse(fkDepolar);

                string fkKullaniciGruplari = "1";
                if (dt.Rows[0]["fkKullaniciGruplari"].ToString() != "")
                    fkKullaniciGruplari = dt.Rows[0]["fkKullaniciGruplari"].ToString();
                Degerler.fkKullaniciGruplari = fkKullaniciGruplari;

                int donem = 1;

                if (lueDonemler.EditValue!=null)
                    int.TryParse(lueDonemler.EditValue.ToString(), out donem);

                Degerler.fkDonemler = donem;

                int max_firma = 20;
                if (dt.Rows[0]["select_top_firma"].ToString() != "")
                    int.TryParse(dt.Rows[0]["select_top_firma"].ToString(),out max_firma);
                Degerler.select_top_firma = max_firma;

                int max_tedarikci = 20;
                if (dt.Rows[0]["select_top_firma"].ToString() != "")
                    int.TryParse(dt.Rows[0]["select_top_tedarikci"].ToString(), out max_tedarikci);
                Degerler.select_top_tedarikci = max_tedarikci;

                Degerler.giris_yapildi = true;

                string fkKasalar = "1";
                if (dt.Rows[0]["fkKasalar"].ToString() != "")
                    fkKasalar = dt.Rows[0]["fkKasalar"].ToString();
                Degerler.fkKasalar = int.Parse(fkKasalar);

                int fkSatisFiyatlariBaslik = 1;
                if (dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString() != "")
                    fkSatisFiyatlariBaslik = int.Parse(dt.Rows[0]["fkSatisFiyatlariBaslik"].ToString());
                Degerler.fkSatisFiyatlariBaslik = fkSatisFiyatlariBaslik;


                DB.girisbasarili = true;
                Close();
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Kullanıcı Bilgileriniz Hatalı. \n Lütfen Kontrol ediniz!", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if(sifre_hatali_say>3)
                {
                    Application.Exit();
                }
                sifre_hatali_say++;
                yetkili = false;
                return;
            }

            if(cbAracdaSatis.Checked)
              Degerler.AracdaSatis = "0";
            else
              Degerler.AracdaSatis = "1";

            button2_Click(sender, e);//giriş bilgileri kaydet

            Thread  t = new Thread(new ThreadStart(YetkiKontroUzakBitisTarihi));
            t.Start();
            
        }

        private void frmGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            DB.direkcik = true;

            if (yetkili == true)
                e.Cancel = false;
            else
                Application.Exit();
        }

        private void sifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                simpleButton1_Click(sender, e);
        }
        
        void GrisiVerKulSube()
        {
            //BaglantiAyarlariKaydet();
            //BaglantiAyarlariGetir();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;
            this.Text = "   Hitit Prof 2012  " + version;
            toolStripStatusLabel3.Text = version;

            Subeler();

            Kullanicilar();

            
            //Donemler();

            //lueDonemler.EditValue = 1;
        }

        void Subeler()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
            //lueSubeler.EditValue = 1;
        }

        void Kullanicilar()
        {
            DataTable dt = DB.GetData("SELECT pkKullanicilar,KullaniciAdi FROM Kullanicilar with(nolock) where durumu=1");
            lueAdi.Properties.DataSource = dt;
            if (dt.Rows.Count == 0)
            {
                WebConfigdenOku();
            }
            else
            {
                //lueAdi.ItemIndex = 03.10.2017

                if(lueAdi.Tag==null)
                    WebConfigdenOku();
                else
                    lueAdi.Text = lueAdi.Tag.ToString();
            }
        }

        void Donemler()
        {
            lueDonemler.Properties.DataSource = DB.GetData("select * from Donemler with(nolock) order by varsayilan desc");
            lueDonemler.ItemIndex = 0;
            //lueDonemler.EditValue = Degerler.fkDonemler;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmSifreDegis SifreDegis = new frmSifreDegis();
            DB.kullaniciadi = lueAdi.Text;
            SifreDegis.ShowDialog();
        }

        private void YetkiKontroUzakBitisTarihi()
        {

            DataTable dtSirket =   DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }
            
            string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string TelefonNo = dtSirket.Rows[0]["TelefonNo"].ToString();
            string kul = "";
            try
            {
                wshitityazilim.WebService ws = new wshitityazilim.WebService();
                //ws.Url = "http://hitityazilim.com/WebService.asmx";
                //DB.InternetVarmi22
                if (ws!=null)
                    kul = ws.HelloWorld(eposta);
            }
            catch (Exception exp)
            {
                DB.logayaz("internet bağlantısı yok", "lisans bilgisi bulunamadı = "+ eposta+
                    "/r hata:"+exp.Message);
                //throw;
            }
            
            //if (kul == "Kayıt Bulunamadı")
            //    MessageBox.Show("Kayıt Bulunamadı");
            //else
            //    MessageBox.Show(kul);

            //SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=185.106.211.51\\MSSQLSERVER2016;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityaz_hitityazilim;Password=Hitit9999");
            //string sql = "select KullaniciAdi,versiyon,Kiralik,WebAdresi,convert(nvarchar,BitisTarihi,112) as BitisTarihi from Kullanicilar with(nolock) where Eposta='" + eposta+"'";
            ////sql = sql.Replace("@KullaniciAdi", username);
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            //bool baglanti_basarili = false;
            //DataTable dt = new DataTable();
            //try
            //{
            //    adp.Fill(dt);
            //    baglanti_basarili = true;
            //    DB.uzaksqlbaglandimi = true;
            //}
            //catch (Exception exp)
            //{
            //    DB.logayaz("Bağlantı Hatası: Hitit Yazılımı Arayınız hata: ", exp.Message);
            //    baglanti_basarili = false;
            //    DB.uzaksqlbaglandimi = false;
            //}
            //finally
            //{
            //    con.Dispose();
            //    adp.Dispose();
            //}

            if (kul == "Kayıt Bulunamadı")
            {
                XtraMessageBox.Show("Lisans Bilgileriniz Hatalı. \r Lütfen Hitit Yazılımı arayınız.", "Lisanas Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DB.kayitli = 0;

                Application.Exit();
                return;
            }
            else
            //if (dt.Rows.Count > 0)
            {
                //string KullaniciAdi = dt.Rows[0]["KullaniciAdi"].ToString();
                //string vol = dt.Rows[0]["versiyon"].ToString();
                //DB.kayitli = int.Parse(dt.Rows[0]["Aktif"].ToString());
                string BitisTarihi = kul;
                                        
                if (BitisTarihi != "")
                    DB.ExecuteSQL("update Sirketler set BitisTarihi='" + BitisTarihi + "'");

                //if (Convert.ToDateTime(BitisTarihi) > DateTime.Today)
                //{
                //    XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                //if (Kiralik == "True")
                //{
                  
                //}
                //else
                //{
                //    DB.ExecuteSQL("update Sirketler set BitisTarihi=getdate()+365");
                //}
                //if (BitisTarihi != "")
                //  Degerler.LisansBitisTarih = Convert.ToDateTime(BitisTarihi);

                //DB.ExecuteSQL("INSERT INTO Logs(LogTipi,sonuc,Tarih,LogAciklama) values(1,1,getdate(),'Giriş Başarılı')");

                //DB.epostagonder("gurbuzadem@gmail.com", "Sirket: " + Sirket, "", "E-Posta: " + eposta + "- Tel:" + TelefonNo);

                if (BitisTarihi != "")
                {
                    Thread t = new Thread(new ThreadStart(GirisYapildiEpostaGonder));
                    t.Start();
                }
            }

            //epostagonder(Sirket, eposta, TelefonNo);
            //return;
        }

        void GirisYapildiEpostaGonder()// string Sirket,string eposta,string TelefonNo)
        {
            DataTable dtSirket = DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string TelefonNo = dtSirket.Rows[0]["TelefonNo"].ToString();

            DB.epostagonder("hitityazilim@gmail.com", "Sirket: " + Sirket +
                " E-Posta: " + eposta + "- Tel:" + TelefonNo + " Bilgisayar Adı:" + Degerler.BilgisayarAdi + " Versiyon: " + Degerler.Versiyon, "", "Programa Giriş Yapıldı");
        }

        void BaglantiAyarlariGetir()
        {
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            TextReader ini = File.OpenText(exeyol + "\\baglanti.ini");
            try
            {
                lueAdi.Tag = ini.ReadLine();
                ipadres.Text = ini.ReadLine();
                //DB.uzakipadresi  = ini.ReadLine();
                DB.VeriTabaniAdresi = ipadres.Text;

                DB.VeriTabaniAdi = ini.ReadLine();
                cbVeriTabaniAdi.Text = DB.VeriTabaniAdi;
                
                DB.VeriTabaniKul = ini.ReadLine();
                teVeriTabaniKul.Text = DB.VeriTabaniKul;

                DB.VeriTabaniSifre = ini.ReadLine();
                teVeritabaniSifre.Text = DB.VeriTabaniSifre;
                //E ise aktarilacak =0 yap değilse  aktarılacak = 1
                Degerler.AracdaSatis = ini.ReadLine();

                if (DB.VeriTabaniAdi == null)
                    DB.VeriTabaniAdi = "MTP2012";

                if (DB.VeriTabaniKul == null)
                    DB.VeriTabaniKul = "hitityazilim";

                if (DB.VeriTabaniSifre == null)
                    DB.VeriTabaniSifre = "WXEn2PMYSlwx5OCe8BLWDw==";

                if(Degerler.AracdaSatis ==null)
                    Degerler.AracdaSatis ="1";

                if (Degerler.AracdaSatis == "1")
                    cbAracdaSatis.Checked = false;
                else
                    cbAracdaSatis.Checked = true;

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                ipadres.Text = "localhost";
                lueAdi.Text = "Admin";
                DB.uzakipadresi = "localhost";
            }
            ini.Close();
            //MessageBox.Show(DB.VeriTabaniAdi);
            if (ipadres.Text == "185.130.56.98" && DB.VeriTabaniAdi == "MTP2012")
            {
                txtSifre.Text = "1";
                lDemo.Visible = true;
                Degerler.ip_adres = ipadres.Text;
            }
            else
                lDemo.Visible = false;
        }

        private void WebConfigdenOku()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    MessageBox.Show("Ayarlar Bulunamadı");
                    return;
                    //Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        if (key == "sonkullanici")
                        {
                            DB.VeriTabaniAdresi = appSettings[key];
                            lueAdi.Text = DB.VeriTabaniAdresi;
                        }
                        else if (key == "serveradi")
                        {
                            DB.VeriTabaniAdresi = appSettings[key];
                            ipadres.Text = DB.VeriTabaniAdresi;
                        }
                        else if (key == "veritabani")
                        {
                            DB.VeriTabaniAdi = appSettings[key];
                            cbVeriTabaniAdi.Text = DB.VeriTabaniAdi;
                        }
                        else if (key == "kullaniciadi")
                        {
                            DB.VeriTabaniKul = appSettings[key];
                            teVeriTabaniKul.Text = DB.VeriTabaniKul;
                        }
                        else if (key == "sifre")
                        {
                            DB.VeriTabaniSifre = appSettings[key];
                            teVeritabaniSifre.Text = DB.VeriTabaniSifre;
                        }

                        //Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException hata)
            {
                MessageBox.Show("web config ayarları okunamadı"+ hata.Message);
                //Console.WriteLine("Error reading app settings");
            }
        }

        void BaglantiAyarlariKaydet()
        {
            string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            if (File.Exists(exeyol + "\\baglanti.ini"))
            {
                String[] sitesDizi = new String[6];
                
                inioku.Items.Add(lueAdi.Text);
                sitesDizi[0] = lueAdi.Text;

                inioku.Items.Add(ipadres.Text);
                sitesDizi[1] = ipadres.Text;
                DB.VeriTabaniAdresi = ipadres.Text;

                inioku.Items.Add(cbVeriTabaniAdi.Text);
                sitesDizi[2] = cbVeriTabaniAdi.Text;
                DB.VeriTabaniAdi= cbVeriTabaniAdi.Text;

                if (DB.VeriTabaniAdi == "") DB.VeriTabaniAdi = "MTP2012";

                inioku.Items.Add(teVeriTabaniKul.Text);
                sitesDizi[3] = teVeriTabaniKul.Text;
                DB.VeriTabaniKul = teVeriTabaniKul.Text;

                inioku.Items.Add(teVeritabaniSifre.Text);
                sitesDizi[4] = teVeritabaniSifre.Text;
                DB.VeriTabaniSifre = teVeritabaniSifre.Text;

                if (cbAracdaSatis.Checked)
                {
                    inioku.Items.Add("0");
                    sitesDizi[5] = "0";
                    Degerler.AracdaSatis = "0";
                }
                else
                {
                    inioku.Items.Add("1");
                    sitesDizi[5] = "1";
                    Degerler.AracdaSatis = "1";
                }
                
                

                File.WriteAllLines(exeyol + "\\baglanti.ini", sitesDizi);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ipadres.Visible = true;
            cbVeriTabaniAdi.Visible = true;
            teVeriTabaniKul.Visible = true;
            teVeritabaniSifre.Visible = true;
            cbAracdaSatis.Enabled = true;
            cbVeriTabaniAdi.Enabled = true;
            btnDonem.Visible = true;
            BaglantiAyarlariGetir();

            try
            {
                DataTable dtdb = DB.GetData("sp_databases");
                for (int i = 0; i < dtdb.Rows.Count; i++)
                {
                    if (i == 0) cbVeriTabaniAdi.Properties.Items.Clear();
                    cbVeriTabaniAdi.Properties.Items.Add(dtdb.Rows[i]["DATABASE_NAME"].ToString());
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message;
            }
        }

        private void adi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                txtSifre.Focus();
        }
        
        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath;// +@"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath +@"\";
                SourcePath = "\\\\" + SourcePath;
                //SourcePath = SourcePath.Substring(1, SourcePath.Length);
                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                ret = false;
            }
            return ret;
        }
        
        private void labelControl3_Click(object sender, EventArgs e)
        {
            frmYeniKullanici yeni = new frmYeniKullanici();
            yeni.ShowDialog();

            //DB.ExecuteSQL("alter table MusteriZiyaretGunleri add Durumu varchar(30)");

            //CopyDirectory(ipadres.Text + "\\Hitit", Path.GetDirectoryName(Application.ExecutablePath), true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GrisiVerKulSube();
        }

        private void ipadres_Leave(object sender, EventArgs e)
        {
            try
            {
                String[] sitesDizi = new String[inioku.Items.Count];
                sitesDizi[0] = ipadres.Text;
                sitesDizi[1] = inioku.Items[1].ToString();
                sitesDizi[2] = inioku.Items[2].ToString();
                string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
                File.WriteAllLines(exeyol + "\\baglanti.ini", sitesDizi);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void güvenlikDuvarınıAçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("OpenSqlServerPort.bat");
            //FileInfo test = new FileInfo(".\\Resources\\OpenSqlServerPort.bat").CopyTo("c:\\OpenSqlServerPort.bat", true);
            //Process.Start("c:\\OpenSqlServerPort.bat");
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        private void mSSQLServisiBaşlatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartService("MSSQLSERVER",1000);

            return;

            //string myServiceName = "MSSQL$SQLEXPRESS";
            //string myServiceName = "MSSQLSERVER";
            //string status;
            //ServiceController mySC = new ServiceController(myServiceName);

            // try
            //{
            //    status = mySC.Status.ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Servis Durumu:\n Servis Başlatılamadı Hata Nedeni: "+  ex.Message);
            //    return;
            //}                     
          

            //if (mySC.Status.Equals(ServiceControllerStatus.Stopped) | mySC.Status.Equals(ServiceControllerStatus.StopPending))
            //{
            //    try
            //    {
            //        TimeSpan timeout = TimeSpan.FromMilliseconds(1000);
            //        mySC.Start();
            //        mySC.WaitForStatus(ServiceControllerStatus.Running, timeout);                   
            //    }
            //    catch (Exception ex)
            //    {
            //      MessageBox.Show("Servis Başlatılamadı Hata Nedeni: "+  ex.Message);  
            //    }

            //}

            //if (mySC.Status.Equals(ServiceControllerStatus.Running))
            //{
            //        MessageBox.Show("Zaten MSSQL Çalışıyor");
            //}
        }

        private void sQLServerManager10ToolStripMenuItem_Click(object sender, EventArgs e)
        {

             Process.Start(@"C:\Windows\system32\SQLServerManager10.msc");
        }

        private void hAKKIMIZDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCikis Hak = new frmCikis();
            Hak.Text = "Hakkımızda";
            Hak.Tag = "1";
            Hak.ShowDialog();
        }

        private void cbVeriTabaniAdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB.VeriTabaniAdi = cbVeriTabaniAdi.Text;
        }

        private void lbVeriTabanıAdi_Click(object sender, EventArgs e)
        {
            SqlYuklumu();
        }

        private void veritabanıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string belgelerklasor = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string exeyol = Application.StartupPath;
            //string exeyol = Path.GetDirectoryName(Application.ExecutablePath);
            if (!File.Exists(exeyol + "\\sqlkur\\db.bak"))
            {
                formislemleri.Mesajform(exeyol + "\\sqlkur\\db.bak \r Yedek Dosyası Bulunamadı","K",250);
                return;
            }

            if (File.Exists(exeyol + "\\sqlkur\\MTP2012.mdf"))
            {
                formislemleri.Mesajform(exeyol + "\\sqlkur\\MTP2012.mdf \r Dosyası Bulunmaktadır", "K",250);
                //return;
            }
            

            string s = formislemleri.MesajBox("Yedek Dosyası Geri Yüklenecektir Eminmisiniz? ", "Yedek Geri Yükle " + cbVeriTabaniAdi.Text + " Restore", 2, 1);
            if (s == "0") return;

            string sql = "RESTORE DATABASE MTP2012";// +cbVeriTabaniAdi.Text;
            sql = sql + " FROM DISK = '"+ exeyol + "\\sqlkur\\db.bak'";
            sql = sql + " with move 'akincilar' to '"+ exeyol + "\\sqlkur\\MTP2012.mdf',";
            sql = sql + " move 'akincilar_log' to '"+ exeyol + "\\sqlkur\\MTP2012.ldf',";
            sql = sql + " replace";
            int sonuc = DB.ExecuteSQLSa(sql);
            if (sonuc == -1)
            {
                formislemleri.Mesajform("Veritabanı Oluşturuldu", "S", 250);
            }
            else
            {
                formislemleri.Mesajform("Veritabanı Oluşturulamadı", "K", 250);
            }

            pictureBox1_Click(sender, e);

        }

        private void kullanıcıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = @"CREATE LOGIN [hitityazilim] WITH PASSWORD='@sifre',
 DEFAULT_DATABASE=[MTP2012], 
 DEFAULT_LANGUAGE=[us_english], 
 CHECK_EXPIRATION=OFF, CHECK_POLICY=ON";
            //string sonuc = formislemleri.inputbox("Şifre Giriniz", "Şifre", "", true);
            string sonuc = "hitit9999";
            if (sonuc == "hitit9999")
            {
                sql = sql.Replace("@sifre", sonuc);

                int s = DB.ExecuteSQLSa(sql);
                if (s != 0)
                {
                    DB.ExecuteSQLSa("EXEC sys.sp_addsrvrolemember @loginame = N'hitityazilim', @rolename = N'sysadmin'");

                    //DB.ExecuteSQLSa("ALTER LOGIN [hitityazilim] DISABLE");

                    formislemleri.Mesajform("Kullanıcı Oluşturuldu", "S", 150);
                }
                else
                    formislemleri.Mesajform("Hatalı Şifre", "K", 150);
                

            }
        }

        private void labelControl8_Click(object sender, EventArgs e)
        {
            frmDonemler donemler = new frmDonemler();
            donemler.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "1";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "2";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "3";
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "4";
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "5";
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "6";
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "7";
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "8";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "9";
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "0";
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            txtSifre.Text = txtSifre.Text + "*";
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (txtSifre.Text.Length == 0) return;
            txtSifre.Text = txtSifre.Text.Substring(0, txtSifre.Text.Length-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EkleveyaGuncelleAppSettings("sonkullanici", lueAdi.Text);
            EkleveyaGuncelleAppSettings("serveradi", ipadres.Text);
            EkleveyaGuncelleAppSettings("veritabani", cbVeriTabaniAdi.Text);
            EkleveyaGuncelleAppSettings("kullaniciadi", teVeriTabaniKul.Text);
            EkleveyaGuncelleAppSettings("sifre", teVeritabaniSifre.Text);
            EkleveyaGuncelleAppSettings("AracdaSatis", cbAracdaSatis.Checked.ToString());
        }
        static void EkleveyaGuncelleAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void localBağlantıTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = DB.GetDataSa("select GetDate()");
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Bağlandı");
            }
            else
                MessageBox.Show("Bağlantı Hatalı");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (!formislemleri.SifreIste()) return;

            inputForm sifregir = new inputForm();
            sifregir.Girilen.PasswordChar = '#';
            sifregir.ShowDialog();

            
            if (sifregir.Girilen.Text != "9999*")
               return;
            
            frmAyarlar ayarlar = new frmAyarlar(2);
            ayarlar.ShowDialog();
            //SqlYuklumu();
        }

        private void localServisBaşlatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string exeyol = Path.GetDirectoryName(Application.ExecutablePath);

            Process.Start("sqllocaldb.exe", "start");
            Process.Start("sqllocaldb.exe", "start v11.0");
        }

        private void txtSifre_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void lueAdi_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void lueAdi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtSifre.Focus();
        }

        private void txtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                simpleButton1_Click(sender, e);
        }

        private void lueAdi_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" + lueAdi.EditValue.ToString());
            if(dt.Rows.Count>0)
            {
                int.TryParse(dt.Rows[0]["fkSube"].ToString(),out Degerler.fkSube);
                //lueSubeler.EditValue = Degerler.fkSube;
            }
            if (Degerler.fkSube == 0) Degerler.fkSube = 1;
            lueSubeler.EditValue = Degerler.fkSube;
        }

        private void cbDil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDil.SelectedIndex == 0)
            {
                DB.Dil = "Azerice";
                //Settings.Default.Save();
                //Application.Restart();
            }
            if (cbDil.SelectedIndex == 1)
            {
                DB.Dil = "İngilizce";
                //Settings.Default.Save();
                //Application.Restart();
            }
            else if (cbDil.SelectedIndex == 2)
            {
                DB.Dil = "Türkçe";
                //Settings.Default.Save();
                //Application.Restart();
            }
            DilYukle();
        }

        void DilYukle()
        {
            if (DB.Dil == "İngilizce")
            {
                Localization.Culture = new CultureInfo("en-US");
            }
            //else if (DB.Dil == "Türkçe")
            //{
            //    Localization.Culture = new CultureInfo("tr-TR");
            //}
            else
            {
                //Azerice
                Localization.Culture = new CultureInfo("");
            }

            lcBaslik.Text = Localization.GBaslik;

            lKullanniciAdi.Text = Localization.KullaniciAdi;
            lSifre.Text = Localization.Sifre;
            lsube.Text = Localization.Sube;
            lDil.Text = Localization.lDil;

            btnGiris.Text = Localization.btnGiris;
        }

        void versiyon_kontrol()
        {
            cVersiyonKontrol volkontrol = new cVersiyonKontrol();
            string result = volkontrol.versiyonkontrol(DB.vol);
            tsslGuncelExe.Text = "Güncelleme =" + result;
            //güncelleme için web küçük client büyük olmalı web(6)>client(7) OK ise güncelle
            if (result == "OK")
            {
                // ask to user would you like download this update/s
                DialogResult dialog = MessageBox.Show("Uygulamayı güncellemek istiyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    //"exekontrol.exe"; çalıştır
                    //DownloadUpdate();
                    uygulamaac();
                }
                //if (tsslVol.Text != DB.vol)
                //    YeniVersiyonicinScript1Calistir();
                //uygulamaac();
                // ask to user would you like download this update/s
                //if (MessageBox.Show("Uygulamayı güncellemek istiyor musunuz?");//, "Bir güncelleme var!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //{
                //DownloadUpdate();
                //}
            }
            else if (result == "NO")
            {
                //exe güncellendi fakat script güncellenmedi ise
                if (tsslVol.Text != DB.vol)
                    YeniVersiyonicinScript1Calistir();
                // say no update
                //MessageBox.Show("Uygulamanız güncel!", "Bir güncelleme var!", MessageBoxButton.OK, MessageBoxImage.Information);

                //DialogResult dialog = MessageBox.Show("Uygulamayı güncellemek istiyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (dialog == DialogResult.Yes)
                //{
                //    //"exekontrol.exe"; çalıştır
                //    //DownloadUpdate();
                //    uygulamaac();
                //}
            }
            //else if (result == "RETURN")
            //{
               //web(6)<client(7)
            //}

        }

        void YeniVersiyonicinScript1Calistir()
        {
            DB.ExecuteSQL(@"if((select count(*) as [Kolon Adları]
            from INFORMATION_SCHEMA.COLUMNS
            where TABLE_NAME = 'Sirketler' and COLUMN_NAME = 'versiyon') = 0)
            alter table Sirketler add versiyon int");

            DB.ExecuteSQL_Sonuc_Sifir("alter table Sirketler add oto_guncelle bit");
            DB.ExecuteSQL("update Sirketler set oto_guncelle=1 where oto_guncelle is null");
            DB.ExecuteSQL("update Sirketler set versiyon = "+DB.vol);

            //DB.ExecuteSQL("alter table Sirketler add kdv_orani_alis int");
        }

        void uygulamaac()
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string onceki_yedek_sil = exeDiz + "/_GPTS.exe";

            if (File.Exists(onceki_yedek_sil))
                File.Delete(onceki_yedek_sil);

            try
            {
                System.Diagnostics.Process.Start("exekontrol.exe");
                System.Diagnostics.Process.Start(exeDiz);

                Application.Exit();
                //Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu : " + exp.Message);
                //Close();
            }
        }

        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
        {

        }

        private void güncellemeİndirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.hitityazilim.com/guncelleme/Hitit2012.rar");
            Process.Start(Application.StartupPath);
        }

        private void lSifre_Click(object sender, EventArgs e)
        {
            //txtSifre.Text= islemler.CryptoStreamSifreleme.Decrypt2("Hitit999", "hitit9999");
            //string s= islemler.CryptoStreamSifreleme.md5SifreyiCoz(teVeritabaniSifre.Text);
            //DB.logayaz(s, "");
        }

        //     public static void StartSQLService()
        //{
        //    //Declare and create an instance of the ManagedComputer object that represents the WMI Provider services.
        //    ManagedComputer mc = default(ManagedComputer);
        //    mc = new ManagedComputer();

        //    //Iterate through each service registered with the WMI Provider.
        //    Service svc = default(Service);
        //    foreach ( svc in mc.Services) {
        //        Console.WriteLine(svc.Name);
        //    }

        //    //Reference the Microsoft SQL Server service.
        //    svc = mc.Services("MSSQLSERVER");

        //    //Stop the service if it is running and report on the status continuously until it has stopped.
        //    if (svc.ServiceState == ServiceState.Running) {
        //        svc.Stop();

        //        Console.WriteLine(string.Format("{0} service state is {1}", svc.Name, svc.ServiceState));
        //        while (!(string.Format("{0}", svc.ServiceState) == "Stopped")) {
        //            Console.WriteLine(string.Format("{0}", svc.ServiceState));
        //            svc.Refresh();
        //        }
        //        Console.WriteLine(string.Format("{0} service state is {1}", svc.Name, svc.ServiceState));
        //        //Start the service and report on the status continuously until it has started.
        //        svc.Start();
        //        while (!(string.Format("{0}", svc.ServiceState) == "Running")) {
        //            Console.WriteLine(string.Format("{0}", svc.ServiceState));
        //            svc.Refresh();
        //        }
        //        Console.WriteLine(string.Format("{0} service state is {1}", svc.Name, svc.ServiceState));

        //    } else {
        //        Console.WriteLine("SQL Server service is not running.");
        //    }
    }
}
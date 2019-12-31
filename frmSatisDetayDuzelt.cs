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

namespace GPTS
{
    public partial class frmSatisDetayDuzelt : DevExpress.XtraEditors.XtraForm
    {
        //int i = 0;
        bool yetkili = false;
        int _fkSatisDetay = 0;
        public frmSatisDetayDuzelt(int fkSatisDetay)
        {
            InitializeComponent();
            _fkSatisDetay = fkSatisDetay;
        }

        private void frmGiris_Load(object sender, EventArgs e)
        {
            DataTable dt =
            DB.GetData(@"select pkSatisDetay,Adet,sd.SatisFiyati,sk.Stokadi from SatisDetay sd with(nolock) 
inner join StokKarti sk with(nolock) on sk.pkStokKarti = sd.fkStokKarti
where pkSatisDetay = " + _fkSatisDetay);
            if (dt.Rows.Count > 0)
            {
                decimal sfiyat = 0;
                decimal.TryParse(dt.Rows[0]["SatisFiyati"].ToString(), out sfiyat);
                SatisFiyati.Value = sfiyat;
                
                seAdet.Text =
                dt.Rows[0]["Adet"].ToString();

                labelControl1.Text = dt.Rows[0]["Stokadi"].ToString();

                SatisFiyati.Focus();

            }
        }

        private void frmGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
           
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
            
            Kullanicilar();
        }

   

        void Kullanicilar()
        {
            DataTable dt = DB.GetData("SELECT pkKullanicilar,KullaniciAdi FROM Kullanicilar with(nolock) where durumu=1");
            //lueAdi.Properties.DataSource = dt;
            if (dt.Rows.Count == 0)
            {
                
            }
            else
            {
                
            }
        }

        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmSifreDegis SifreDegis = new frmSifreDegis();
            //DB.kullaniciadi = lueAdi.Text;
            SifreDegis.ShowDialog();
        }

        private void YetkiKontrol_uzak()
        {

            DataTable dtSirket =   DB.GetData("select * from Sirketler with(nolock)");
            if (dtSirket.Rows.Count == 0)
            {
                return;
            }

            string Sirket = dtSirket.Rows[0]["Sirket"].ToString();
            string eposta = dtSirket.Rows[0]["eposta"].ToString();
            string TelefonNo = dtSirket.Rows[0]["TelefonNo"].ToString();



            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=sql2012.isimtescil.net;Initial Catalog=hitityazilim_db9999;Persist Security Info=True;User ID=hitityazilim_adem;Password=Hitit9999");
            string sql = "select * from Kullanicilar with(nolock) where Eposta='"+eposta+"'";
            //sql = sql.Replace("@KullaniciAdi", username);
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            bool baglanti_basarili = false;
            DataTable dt = new DataTable();
            try
            {
                adp.Fill(dt);
                baglanti_basarili = true;
            }
            catch (Exception exp)
            {
                DB.logayaz("Bağlantı Hatası: Hitit Yazılımı Arayınız hata: ", exp.Message);
                //return;
                baglanti_basarili = false;
            }
            finally
            {
                con.Dispose();
                adp.Dispose();
            }

            if (dt.Rows.Count == 0 && baglanti_basarili)
            {
                XtraMessageBox.Show("Lisans Bilgileriniz Hatalı. \r Lütfen Hitit Yazılımı arayınız.", "Lisanas Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DB.kayitli = 0;
                DB.ExecuteSQL("update Sirketler set BitisTarihi=getdate()-100");

                Application.Exit();
                return;
            }
            if (dt.Rows.Count > 0)
            {
                string KullaniciAdi = dt.Rows[0]["KullaniciAdi"].ToString();
                string vol = dt.Rows[0]["versiyon"].ToString();
                //DB.kayitli = int.Parse(dt.Rows[0]["Aktif"].ToString());
                string BitisTarihi = dt.Rows[0]["BitisTarihi"].ToString();
                string Kiralik = dt.Rows[0]["Kiralik"].ToString();
                string WebAdresi = dt.Rows[0]["WebAdresi"].ToString();
                //DB.kayitli = 1;// dt.Rows.Count;

                //if (Convert.ToDateTime(BitisTarihi) > DateTime.Today)
                //{
                //XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                if (Kiralik == "True")
                {
                    if (BitisTarihi != "")
                        DB.ExecuteSQL("update Sirketler set BitisTarihi='" + Convert.ToDateTime(BitisTarihi).ToString("yyyy-MM-dd") + "'");
                }
                else
                {
                    DB.ExecuteSQL("update Sirketler set BitisTarihi=getdate()+365");
                }
                if (BitisTarihi != "")
                    Degerler.LisansBitisTarih = Convert.ToDateTime(BitisTarihi);

                //DB.ExecuteSQL("INSERT INTO Logs(LogTipi,sonuc,Tarih,LogAciklama) values(1,1,getdate(),'Giriş Başarılı')");

                //DB.epostagonder("gurbuzadem@gmail.com", "Sirket: " + Sirket, "", "E-Posta: " + eposta + "- Tel:" + TelefonNo);


                Thread t = new Thread(new ThreadStart(GirisYapildiEpostaGonder));
                t.Start();
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
    
        private void adi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                SatisFiyati.Focus();
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
           
        }

        private void lbVeriTabanıAdi_Click(object sender, EventArgs e)
        {
         
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
            

           
            
        }

        private void kullanıcıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = @"CREATE LOGIN [hitityazilim] WITH PASSWORD='@sifre',
 DEFAULT_DATABASE=[MTP2012], 
 DEFAULT_LANGUAGE=[us_english], 
 CHECK_EXPIRATION=OFF, CHECK_POLICY=ON";
            //string sonuc = formislemleri.inputbox("Şifre Giriniz", "Şifre", "", true);
            string sonuc = "Hitit9999";
            if (sonuc == "Hitit9999")
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

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string v = SatisFiyati.Text + "1";
            SatisFiyati.Value = decimal.Parse(v);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "2";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "3";
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "4";
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "5";
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "6";
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "7";
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "8";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "9";
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + "0";
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            SatisFiyati.Text = SatisFiyati.Text + ",";
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (SatisFiyati.Text.Length == 0) return;
            SatisFiyati.Text = SatisFiyati.Text.Substring(0, SatisFiyati.Text.Length-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
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
                SatisFiyati.Focus();
        }

        private void txtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                simpleButton1_Click(sender, e);
        }

        private void lueAdi_EditValueChanged(object sender, EventArgs e)
        {
            //DataTable dt = DB.GetData("select * from Kullanicilar with(nolock) where pkKullanicilar=" + lueAdi.EditValue.ToString());
            //if(dt.Rows.Count>0)
            //{
            //    //int.TryParse(dt.Rows[0]["fkSube"].ToString(),out Degerler.fkSube);
            //    //lueSubeler.EditValue = Degerler.fkSube;
            //}
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("update SatisDetay set SatisFiyati=" + SatisFiyati.Text.Replace(",", ".") +
                ",Adet=" + seAdet.Text.Replace(",",".")+
                " where pkSatisDetay=" + _fkSatisDetay);

            DB.ExecuteSQL("update SatisDetay set SatisFiyatiKdvHaric=SatisFiyati-((SatisFiyati*KdvOrani)/(100+KdvOrani))" +
                ",iskontoyuzdetutar=0,iskontotutar=0" +
                " where pkSatisDetay=" + _fkSatisDetay);


            Close();
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void SatisFiyati_EditValueChanged(object sender, EventArgs e)
        {
            ceTutar.Value = SatisFiyati.Value * seAdet.Value;
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            SatisFiyati.Value = 0;
            SatisFiyati.Focus();
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
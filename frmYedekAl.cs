using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using System.Threading;
using GPTS.Include.Data;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using GPTS.islemler;
using System.Net;

namespace GPTS
{
    public partial class frmYedekAl : DevExpress.XtraEditors.XtraForm
    {
        public static string yedekalinacakyer;
        public static bool otomatik;
        public frmYedekAl()
        {
            InitializeComponent();
        }
        private void frmYedekAl_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            DataTable dt = DB.GetData("select * from Sirketler with(nolock) where pkSirket=1");
            klasoryol.Text = dt.Rows[0]["yedekalinacakyer"].ToString();
            dateEdit2.EditValue = dt.Rows[0]["YedekSaati"].ToString();
            dt.Dispose();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(klasoryol.Text))
            //{
            //    formislemleri.Mesajform("Klasör Bulunamadı. Lütfen Yedek Alınacak Yeri Seçiniz!", "K", 200);
            //    return;
            //}
            pictureBox1.Visible = true;
            labelControl1.Visible = true;

            yedekalinacakyer = klasoryol.Text;
            simpleButton3.Enabled = false;

            Thread thread1 = new Thread(new ThreadStart(YedekAl));
            thread1.Start();

            //Thread thread2 = new Thread(new ThreadStart(MusterileriExcelAt));
            //thread2.Start();

            //Thread thread3 = new Thread(new ThreadStart(TedarikcileriExcelAt));
            //thread3.Start();
        }

        private void YedekAl()
        {

            DateTime  Tarih = DateTime.Now;
            string Yer = Tarih.ToString("yyyyMMddHHmm") + ".bak";
            try
            {
                labelControl1.Text = "Veritabanı Shrink Yapılıyor...";
                if (DB.VeriTabaniAdi == "") DB.VeriTabaniAdi = "MTP2012";
                DB.ExecuteSQL("DBCC SHRINKDATABASE ("+ DB.VeriTabaniAdi+", 10);");
            }
            catch (Exception)
            {
                //a 
            }

            Application.DoEvents();
            labelControl1.Text = "Yedek Alınıyor Lütfen Bekleyiniz...";

            try
            {
                string YerSil = Tarih.AddDays(-10).ToString("yyyyMMddHHmm") + ".bak";

                DataTable dt = DB.GetData("select top 1  Sirket from Sirketler with(nolock)");
                string Sirket = dt.Rows[0]["Sirket"].ToString();
                Sirket = Sirket.Replace("*", "");
                Yer = yedekalinacakyer + "\\" + Sirket + Yer;
                //Sirket + Dosya

                string sql = "BEGIN TRY BACKUP DATABASE ["+ DB.VeriTabaniAdi+"] TO  DISK = '@yer'"+
                @" WITH NOFORMAT, NOINIT,   
                NAME = N'Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 
                select ('1') as sonuc
                END TRY
                BEGIN CATCH;
                 SELECT ERROR_MESSAGE() AS sonuc;
                END CATCH";
                sql = sql.Replace("@yer", Yer);

                DataTable dt1 = DB.GetData(sql);
                if (dt1.Rows.Count == 0)
                {
                    MessageBox.Show("Yedek Alınamadı. MSSQL Hatası Oluştu");
                    return;
                }
                string sonuc = dt1.Rows[0][0].ToString();

                if (sonuc == "1")
                {
                    if (File.Exists(yedekalinacakyer + "\\" + Sirket + YerSil))
                        File.Delete(yedekalinacakyer + "\\" + Sirket + YerSil);

                    labelControl1.Text= "Veritabanı yedeği alındı.";
                    btnKlasorAc.Enabled = true;
                    pictureBox1.Visible = false;

                    //ArrayList list = new ArrayList();
                    //list.Add(new SqlParameter("@yedekalinacakyer", yedekalinacakyer));
                    //list.Add(new SqlParameter("@YedekSaati", dateEdit2.DateTime.ToString("HH:mm:ss")));
                    //Degerler.YedekSaati = dateEdit2.DateTime.ToString("HH:mm:ss");

                    //DB.ExecuteSQL("UPDATE Sirketler SET yedekalinacakyer=@yedekalinacakyer,YedekSaati=@YedekSaati", list);
                    DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) VALUES(3,convert(varchar(20),getdate(),120)+' Yedek Alındı',getdate(),1)");
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Yedek Alınırken Hata Oluştur.\nHata Mesajı: " + sonuc, Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc,HataAciklama,fkKullanicilar) " +
                        " VALUES(3,'Yedek Alınırken Hata Oluştur',getdate(),0,'"+sonuc+"',"+DB.fkKullanicilar+")");

                    return;
                }
            }
            catch (Exception exp)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu." + exp.Message.ToString(), "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBox1.Visible = false;
                return;
            }
            
            //sıkıştır
            bool bsonuc = Digerislemler.Ziple(Yer);

            //sıkıştırdıktan sonra bak dosyasını sil
            if (File.Exists(Yer))
                File.Delete(Yer);
            //DB.epostagonder("musteri@hitityazilim.com", sirketadi.Text + " Yedek Aldı", "", ServerAdi.Text);
        }

        public void MusterileriExcelAt()
        {   
            if (!Directory.Exists(Degerler.YedekYolu))
            {
                formislemleri.Mesajform(Degerler.YedekYolu + " Dosya Yolunu Kontrol Ediniz", "K", 200);
                return;
            }
            string dosya = Degerler.YedekYolu + "\\" + "Musteriler" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            
            if (File.Exists(dosya)) File.Delete(dosya);

            gCDynamik.DataSource = DB.GetData("select pkFirma,OzelKod,Firmaadi,FaturaUnvani,Tel,Fax,Adres,webadresi,Eposta,Tel2,Aktif,Yetkili,VergiDairesi,VergiNo,Unvani,Cep,Cep2,Devir from Firmalar with(nolock) where Aktif=1 order by Devir desc");
            gCDynamik.ExportToXls(dosya);
        }

        public void TedarikcileriExcelAt()
        {

            if (!Directory.Exists(Degerler.YedekYolu))
            {
                formislemleri.Mesajform(Degerler.YedekYolu + " Dosya Yolunu Kontrol Ediniz", "K", 200);
                return;
            }
            string dosya = Degerler.YedekYolu + "\\" + "Tedarikciler" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";

            if (File.Exists(dosya)) File.Delete(dosya);

            gcTedarikciler.DataSource = DB.GetData("select pkTedarikciler,OzelKod,Firmaadi,Tel,Fax,Adres,webadresi,Eposta,Tel2,Aktif,Yetkili,VergiDairesi,VergiNo,Unvani,Cep,Cep2,Devir from Tedarikciler with(nolock) where Aktif=1 order by Devir desc");
            gcTedarikciler.ExportToXls(dosya);
        } 

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                klasoryol.Text = folderBrowserDialog1.SelectedPath;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmYedekAl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(klasoryol.Text))
                Process.Start(klasoryol.Text);
            else
                MessageBox.Show("Yedek Yolu Hatalı");
        }

        private void labelControl6_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosyaac= new OpenFileDialog();
            dosyaac.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            dosyaac.Title = "Hitit Zip Dosyası Seçiniz";

            if (dosyaac.ShowDialog() == DialogResult.OK)
            {
                //SaveFileDialog dosyakaydet = new SaveFileDialog();
                //dosyakaydet.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                //dosyakaydet.Title = "Kaydedilecek Yer Seçiniz";
                //dosyakaydet.FileName = dosyaac.FileName.Substring(0,dosyaac.FileName.Length-4);

                //if (dosyakaydet.ShowDialog() == DialogResult.OK)
                    Digerislemler.ZipAc(dosyaac.FileName);

                //Process.Start(dosyakaydet.InitialDirectory);
                //OpenFileDialog dosyaac2 = new OpenFileDialog();
                //dosyaac2.ShowDialog();
                //MessageBox.Show(dosyaac.FileName + " Geri Yüklendi.");
            }
        }

        void DosyaGonder_ftp()
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(DosyaGonder_ftp));
            //t.Start();

            //string yol = Application.StartupPath.ToString();
            openFileDialog1.InitialDirectory = klasoryol.Text;
            if (System.Windows.Forms.DialogResult.Cancel == openFileDialog1.ShowDialog()) return;


            DataTable dt = DB.GetData("select top 1  Sirket from Sirketler with(nolock)");
            string Sirket = dt.Rows[0]["Sirket"].ToString();

            FileInfo fileInf = new FileInfo(openFileDialog1.FileName);//openFileDialog1.SafeFileName)
            ///images/merpa_logo.jpg
            string uri = "ftp://ftp.hitityazilim.com/httpdocs/guncelleme/" + openFileDialog1.SafeFileName;// Sirket.Replace("**","").Replace(" ","") + ".zip";//Burada uplaod ediceğiniz dizini tam oalrak belirtmelisiniz.
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential("hitityazilim", "Hitit9999");
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;

            if (fileInf.Length > 20*1024*1000)
            {
                MessageBox.Show("Dosya Boyutu 20 MB'dan Büyük Olamaz!");
                return;
            }

            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();

            try
            {
                contentLen = fs.Read(buff, 0, buffLength);
       
                Stream strm = reqFTP.GetRequestStream();
                frmYukleniyor yukleniyor = new frmYukleniyor();
                yukleniyor.labelControl1.Text = "Dosya Gönderiliyor...";
                yukleniyor.TopMost = true;
                yukleniyor.Show();
              
                while (contentLen != 0)
                {
                    yukleniyor.Show();
                    Application.DoEvents();
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                yukleniyor.Close();
                strm.Close();
                fs.Close();
                MessageBox.Show("Dosya Webe Gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 1)
            {
                DataTable dt = DB.GetData("select yedek_yeri_yol from Kullanicilar with(nolock) where pkKullanicilar=" + DB.fkKullanicilar);

                if (dt.Rows.Count > 0)
                    klasoryol.Text = dt.Rows[0]["yedek_yeri_yol"].ToString();
            }
            else
            {
                DataTable dt = DB.GetData("select * from Sirketler with(nolock) where pkSirket=1");
                klasoryol.Text = dt.Rows[0]["yedekalinacakyer"].ToString();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Ionic.Zip;
using System.Media;
using System.Windows.Forms;
using System.IO;
using GPTS.Include.Data;
using System.Data;

namespace GPTS.islemler
{
    class Digerislemler
    {
        public static bool Ziple(string ziplenecekdosya)
        {
            //string dosya = "GPTS.exe";
            //if (!Path.GetExtension(dosya).Equals(".zip"))
            //  dosya = dosya + ".zip";

            try
            {

            using (ZipFile zip = new ZipFile())
            {
                //zip.AddDirectory(Server.MapPath("~/upload/testklasoru"));
                zip.Password = "hitit9999";
                zip.AddFile(ziplenecekdosya);
                zip.Save(ziplenecekdosya + ".zip");

                //hlDosya.Text = dosya;
                //ViewState["Dosya"] =
                //  hlDosya.NavigateUrl = "~/upload/" + dosya;

                //txtFolder.Text = "~/upload/" + Path.GetFileNameWithoutExtension(dosya);
                //pnlDosyaAc.Visible = true;

                //KlasorleriGoster();
            }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Zip Hatası " + exp.Message);
                return false;
                //throw;
            }
            return true;
        }

        public static bool ZipAc(string zipliDosya)
        {
            //string klasor = txtFolder.Text;
            //string zipDosya = "GPTS.zip";

            try
            {
                using (ZipFile zip = new ZipFile(zipliDosya))
                {
                    zip.Password = "hitit9999";
                    zip.ExtractAll(DB.exeDizini + "\\zipliDosya.bak", ExtractExistingFileAction.OverwriteSilently);
//@"C:\Users\agurbuz\Documents\Visual Studio 2010\Projects\gzip\gzip\bin\Debug", ExtractExistingFileAction.OverwriteSilently);
                    // KlasorleriGoster();
                }
            }
            catch (Exception exp)
            {
                return false;
                //MessageBox.Show(exp.Message);
            }
            return true;
        }

        public static bool UrunEkleSesCal()
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\Sesler\\urunekle.wav";
            //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
            if (File.Exists(dosya))
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = dosya;// "chord.wav";
                player.Play();
            }
            return true;
        }

        public static bool UrunYokSesCal()
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\Sesler\\Yok.wav";
            //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
            if (File.Exists(dosya) && Degerler.stokkartisescal)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = dosya;// "chord.wav";
                player.Play();
            }
            return true;
        }
        public static bool AnimsaticiSesCal()
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\Sesler\\animsat.wav";
            //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
            if (File.Exists(dosya) && Degerler.stokkartisescal)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = dosya;// "chord.wav";
                player.Play();
            }
            return true;
        }
        public static bool YedekAl()
        {
            //try
            //{
            //    DB.ExecuteSQL("DBCC SHRINKDATABASE (MTP2012, 10);");
            //}
            //catch (Exception)
            //{
            //    //a
            //}

            // try
            // {
            //this.Text = "HİTİT PROF SATIŞ PROGRAMI " + DateTime.Now.ToString("HH:mm:ss");
            string Yer = DateTime.Now.ToString("yyyyMMddHHmm") + ".bak";
            string YerSil = DateTime.Now.AddDays(-10).ToString("yyyyMMddHHmm") + ".bak";
            Yer = Degerler.YedekYolu + "\\" + Degerler.SirketAdi.ToString().Replace("*", "") + Yer;
            YerSil = Degerler.YedekYolu + "\\" + Degerler.SirketAdi.ToString().Replace("*", "") + YerSil;
            string sql = @"BEGIN TRY
    BACKUP DATABASE [MTP2012] TO  DISK = '@yer'  
    WITH NOFORMAT, NOINIT,   
    NAME = N'Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 
    select ('1') as sonuc
END TRY
BEGIN CATCH;
select ('0') as sonuc
END CATCH";
            sql = sql.Replace("@yer", Yer);
            DataTable dt1 = DB.GetData(sql);
            if (dt1.Rows[0][0].ToString() == "1")
            {
                //labelControl1.Text = "Veritabanı yedeği alındı.";
                DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) VALUES(3,convert(varchar(20),getdate(),120)+' Yedek Alındı',getdate(),1)");

                if (File.Exists(YerSil))
                    File.Delete(YerSil);
                //Baslik.Text = "Yedek Alındı";
                //Baslik.Visible = true;
                //btnKlasorAc.Enabled = true;
                //pictureBox1.Visible = false;
                //DB.ExecuteSQL("UPDATE Sirketler SET yedekalinacakyer='" + yedekalinacakyer + "'");
                //Process.Start(DB.GetData("select top 1 yedekalinacakyer from Sirketler").Rows[0][0].ToString());
            }
            else
            {
                //Baslik.Text = "Yedek Alınamadı";
                DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) VALUES(3,convert(varchar(20),getdate(),120)+' Yedek Alınamadı.Dosya Yolunu kontrol ediniz',getdate(),1)");
                //DevExpress.XtraEditors.XtraMessageBox.Show("Program veritabanı yedek alınırken Hata Oluştu.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //}
            //catch (Exception exp)
            //  {
            //     DB.ExecuteSQL("INSERT INTO Logs (LogTipi,LogAciklama,Tarih,Sonuc) VALUES(3,convert(varchar(20),getdate(),120)+' Yedek Alınamadı',getdate(),1)");
            //    Baslik.Text = "Yedek Alınamadı";
            //    return;
            //}
            //DB.epostagonder("musteri@hitityazilim.com", sirketadi.Text + " Yedek Aldı", "", ServerAdi.Text);
            return true;
        }

        public static bool BarkodEan13(string _barkod)
        {
            //string _barkod = Barkod.Text;
            if (_barkod.Length < 13) return false;

            int ciftTopla = 0, tekTopla = 0;
            for (int i = 0; i < _barkod.Length - 1; i++)
            {
                if (i % 2 == 1)
                    ciftTopla = ciftTopla + int.Parse(_barkod.Substring(i, 1));
                else
                    tekTopla = tekTopla + int.Parse(_barkod.Substring(i, 1));
            }

            int carpuc = ciftTopla * 3;
            int tekcapuc = tekTopla + carpuc;
            string yuzdencikar = (100 - tekcapuc).ToString();

            if (yuzdencikar == _barkod.Substring(_barkod.Length - 1, 1))
                return true;//MessageBox.Show("Doğru");
            else
                return false;//MessageBox.Show("Yanlış");
        }

        public static void CreateCSVFile(DataTable dt, string strFilePath)
        {
            try
            {
                // Create the CSV file to which grid data will be exported.
                StreamWriter sw = new StreamWriter(strFilePath, false);
                // First we will write the headers.
                //DataTable dt = m_dsProducts.Tables[0];
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);

                // Now write all the rows.

                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString());
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }

                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

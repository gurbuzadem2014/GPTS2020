using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Collections;
using GPTS.Include.Data;
using System.Threading;
using System.ServiceModel.Channels;
using System.ServiceModel;
using GPTS.islemler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using Ionic.Zip;
using System.Xml.Serialization;
using Ionic.Zlib;
using System.Security.Cryptography;

namespace GPTS
{
    public partial class frmWebAyarlari : DevExpress.XtraEditors.XtraForm
    {
        string pkStokGrup = "0";
        public frmWebAyarlari()
        {
            InitializeComponent();
        }
        void KullaniciKontrol(string KullaniciAdi)
        {
            try
            {
                SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
                string sql = "select * from kullanicilar where KullaniciAdi='" + KullaniciAdi + "'";// where username='@username' and password='@password'";
                //sql = sql.Replace("@username", username);
                //sql = sql.Replace("@password", password);
                SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                adp.SelectCommand.CommandTimeout = 60;

                DataTable dt = new DataTable();
                try
                {
                    adp.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        DB.webpkKullanicilar = 0;
                        XtraMessageBox.Show("Lisans için Lütfen 0262 644 51 12 arayınız.", "Hitit Yazılım", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    DB.webpkKullanicilar = int.Parse(dt.Rows[0]["pkKullanicilar"].ToString());
                    //Degerler.userid = dt.Rows[0]["sirano"].ToString();
                    //Degerler.username = dt.Rows[0]["username"].ToString()
                    //Degerler.password = dt.Rows[0]["password"].ToString();
                    //Degerler.smsadet = dt.Rows[0]["smsadet"].ToString();
                    //Degerler.smskullanilan = dt.Rows[0]["kullanilan"].ToString();
                    //Degerler.yetkili = dt.Rows[0]["yetkili"].ToString();
                    //Degerler.merkezadi = dt.Rows[0]["hastaneadi"].ToString();
                    //Degerler.ilce = dt.Rows[0]["ilce"].ToString();
                    //Degerler.mumessil = dt.Rows[0]["mumessil"].ToString();
                    //Degerler.sehir = dt.Rows[0]["sehir"].ToString();
                    //Degerler.vol = dt.Rows[0]["versiyon"].ToString();
                    //Degerler.aktif = dt.Rows[0]["aktif"].ToString();
                    //Degerler.smsuser = dt.Rows[0]["smsuser"].ToString();
                    //Degerler.smspass = dt.Rows[0]["smspass"].ToString();
                    //Degerler.smsbayi = dt.Rows[0]["smsbayikodu"].ToString();
                    //Degerler.smssender = dt.Rows[0]["smssender"].ToString();
                }
                catch (Exception exp)
                {
                    return;
                }
                finally
                {
                    con.Dispose();
                    adp.Dispose();
                }
                if (dt.Rows.Count == 0)
                {
                    //Degerler.kayitli = 0;
                    return;
                }
                //else
                //Degerler.kayitli = dt.Rows.Count;
                //
            }
            catch (Exception exp)
            {
                XtraMessageBox.Show("Lisans için Lütfen Yazılım firmasını arayınız.", "Termo Takip", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
                // Application.Exit();
            }
            finally
            {
                //con.Dispose();
                //adp.Dispose();
            }
            return;
        }
        void StokGruplari()
        {
            gridControl1.DataSource = DB.GetData("Select * from StokGruplari order by SortID");
        }
        void urunler()
        {
            string sql = @"SELECT WebStokKarti.pkWebStokKarti, WebStokKarti.fkStokKarti, WebStokKarti.Gonderildi,
 WebStokKarti.fkKullanicilar, WebStokKarti.fkMenukategori, sk.Stokadi, sk.SatisFiyati,
 sg.StokGrup
FROM   WebStokKarti 
LEFT OUTER JOIN StokKarti sk ON WebStokKarti.fkStokKarti = sk.pkStokKarti
left join StokGruplari sg on sg.pkStokGrup=sk.fkStokGrup";
            if (pkStokGrup != "0")
                sql += " where sg.pkStokGrup=" + pkStokGrup;
            //WHERE WebStokKarti.fkKullanicilar = 1";
            DataTable dtWebStokKarti = DB.GetData(sql);
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("pkWebStokKarti", typeof(Int32)));
            dt.Columns.Add(new DataColumn("fkStokKarti", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Stokadi", typeof(string)));
            dt.Columns.Add(new DataColumn("SatisFiyati", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Resim", typeof(Image)));
            for (int i = 0; i < dtWebStokKarti.Rows.Count; i++)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr["pkWebStokKarti"] = dtWebStokKarti.Rows[i]["pkWebStokKarti"].ToString();
                dr["fkStokKarti"] = dtWebStokKarti.Rows[i]["fkStokKarti"].ToString();
                dr["Stokadi"] = dtWebStokKarti.Rows[i]["Stokadi"].ToString();
                if (dtWebStokKarti.Rows[i]["SatisFiyati"].ToString() == "")
                    dr["SatisFiyati"] = 0;
                else
                    dr["SatisFiyati"] = dtWebStokKarti.Rows[i]["SatisFiyati"].ToString();

                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                if (File.Exists(exeDiz + "\\StokKartiResim\\" + dtWebStokKarti.Rows[i]["fkStokKarti"].ToString() + ".jpg"))
                {
                    FileStream bitmapFile = new FileStream(exeDiz + "\\StokKartiResim\\" + dtWebStokKarti.Rows[i]["fkStokKarti"].ToString() + ".jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Image loaded = new Bitmap(bitmapFile);
                    dr["Resim"] = loaded;
                }
                else
                {
                    //FileStream bitmapFile = new FileStream(exeDiz + "\\StokKartiResim\\.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    Image loaded = new Bitmap(Properties.Resources.bos);
                    dr["Resim"] = loaded;
                }
                dt.Rows.Add(dr);
            }

            gridControl2.DataSource = dt;
        }
        private void frmWebAyarlari_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = DateTime.Today.AddDays(-100);

            StokGruplari();
            urunler();

            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            if (File.Exists(exeDiz + "\\logo.jpg"))
            {
                FileStream bitmapFile = new FileStream(exeDiz + "\\logo.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Image loaded = new Bitmap(bitmapFile);
                pblogo.Image = loaded;
            }
            /*
            KullaniciKontrol("gizembebe");
            if (DB.webpkKullanicilar == 0) return;
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
            SqlDataAdapter adp = new SqlDataAdapter("select * from Menukategori where fkKullanicilar=" + DB.webpkKullanicilar.ToString(), con);
            adp.SelectCommand.CommandTimeout = 60;

            DataTable dt = new DataTable();
            try
            {
                adp.Fill(dt);
                gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                // throw e;
            }
            finally
            {
                con.Dispose();
                adp.Dispose();
            }
             */
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                sql += "UPDATE Menukategori SET Menu_Adi='" + dr["Menu_Adi"].ToString() + "' WHERE Kod=" + dr["Kod"].ToString();
            }
            //KullaniciKontrol("gizembebe");
            if (DB.webpkKullanicilar == 0) return;
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            SqlCommand cmd = new SqlCommand(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                // adp.Fill(dt);
                //gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                // throw e;
            }
            finally
            {
                con.Dispose();
                //adp.Dispose();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string sql = "";
            // for (int i = 0; i < gridView1.DataRowCount; i++)
            // {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            sql = "DELETE FROM  Menukategori WHERE Kod=" + dr["Kod"].ToString();
            // }
            //KullaniciKontrol("gizembebe");
            if (DB.webpkKullanicilar == 0) return;
            SqlConnection con = new System.Data.SqlClient.SqlConnection("Data Source=mssql02.turhost.com;Initial Catalog=hitityazilim;Persist Security Info=True;User ID=hitityazilim;Password=hitit9999");
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            SqlCommand cmd = new SqlCommand(sql, con);
            //adp.SelectCommand.CommandTimeout = 60;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                gridView1.DeleteSelectedRows();
                // adp.Fill(dt);
                //gridControl1.DataSource = dt;
            }
            catch (Exception exp)
            {
                // throw e;
            }
            finally
            {
                con.Dispose();
                //adp.Dispose();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Ofd = new OpenFileDialog();
            Ofd.ShowDialog();

            FileStream bitmapFile = new FileStream(Ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Image loaded = new Bitmap(bitmapFile);
            pblogo.Image = loaded;
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\logo.jpg";
            loaded.Save(dosya);
            Ofd.Dispose();
        }

        private void layoutView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = cardView1.GetDataRow(cardView1.FocusedRowHandle);
            if (dr["fkStokKarti"].ToString() == "0")
            {
                frmStokAra StokAra = new frmStokAra("");
                StokAra.ShowDialog();
                string id = DB.pkStokKarti.ToString();
                return;
            }
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //pkStokGrup = "0";
            urunler();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkStokGrup = dr["pkStokGrup"].ToString();
            urunler();
        }

        private void anaSayfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pkStokGrup = "0";
            urunler();
        }
        private void Upload(string FtpServer, string Username, string Password, string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            ///images/merpa_logo.jpg
            string uri = "ftp://" + FtpServer + "/httpdocs/Merpa/images/" + fileInf.Name;//Burada uplaod ediceğiniz dizini tam oalrak belirtmelisiniz.
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(Username, Password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();

            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                MessageBox.Show("Dosya Webe Gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }

        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string dosya = exeDiz + "\\logo.jpg";
            //DB.ExecuteSQL("UPDATE StokKarti SET WebdeGoster=1 where pkStokKarti=" + tEpkStokKarti.Text);
            if (File.Exists(dosya))
                Upload("bendeyapi.com", "gurbuzadem", "tekteksql41", dosya);
            else
            {
                MessageBox.Show("Dosya Webe Gönderilemedi. Dosya Yolunu Kontrol Ediniz.");
                //DB.ExecuteSQL("UPDATE StokKarti SET WebdeGoster=0 where pkStokKarti=" + tEpkStokKarti.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(UrunleriWebeGonder));
            thread1.Start();
        }

        private void UrunleriWebeGonder()
        {
            //MessageBox.Show("Kullanıcı Bulunamadı");
            //return;

            DataTable dt = DB.GetData("select top 10 * from StokKarti with(nolock) where Aktif=1 and Webde is null");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@StokKod", dt.Rows[i]["StokKod"].ToString()));
                list.Add(new SqlParameter("@Barcode", dt.Rows[i]["Barcode"].ToString()));
                list.Add(new SqlParameter("@Stokadi", dt.Rows[i]["Stokadi"].ToString()));
                list.Add(new SqlParameter("@Stoktipi", dt.Rows[i]["Stoktipi"].ToString()));
                list.Add(new SqlParameter("@KdvOrani", dt.Rows[i]["KdvOrani"].ToString()));
                list.Add(new SqlParameter("@Mevcut", dt.Rows[i]["Mevcut"].ToString()));
                list.Add(new SqlParameter("@SatisFiyatiN", dt.Rows[i]["SatisFiyati"].ToString().Replace(",", ".")));

                string sql = @"insert into StokKarti (StokKod,Barcode,Stokadi,Stoktipi,KdvOrani,Mevcut,Tarih,YeniUrunler,SatisFiyatiN,fkMenukategoriKod)
                    values(@StokKod,@Barcode,@Stokadi,@Stoktipi,@KdvOrani,@Mevcut,getdate(),1,@SatisFiyatiN,0)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                if (sonuc == "0")
                    DB.ExecuteSQL("update StokKarti set Webde=1 where pkStokKarti=" + dt.Rows[i]["pkStokKarti"].ToString());
            }
        }

        private void FirmalariWebeGonder()
        {
            //MessageBox.Show("Kullanıcı Bulunamadı");
            //return;

            DataTable dt = DB.GetData("select top 10 * from Firmalar with(nolock) where Aktif=1 and Webde is null");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Firmaadi", dt.Rows[i]["Firmaadi"].ToString()));
                list.Add(new SqlParameter("@Yetkili", dt.Rows[i]["Yetkili"].ToString()));
                //list.Add(new SqlParameter("@fkFirmaGruplari", dt.Rows[i]["fkFirmaGruplari"].ToString()));
                list.Add(new SqlParameter("@Unvani", dt.Rows[i]["Unvani"].ToString()));
                list.Add(new SqlParameter("@Tel", dt.Rows[i]["Tel"].ToString()));
                list.Add(new SqlParameter("@Cep", dt.Rows[i]["Cep"].ToString()));
                list.Add(new SqlParameter("@OzelKod", dt.Rows[i]["OzelKod"].ToString()));
                list.Add(new SqlParameter("@Devir", dt.Rows[i]["Devir"].ToString().Replace(",", ".")));

                string sql = @"insert into Firmalar (Firmaadi,Yetkili,Unvani,OzelKod,Tel,Cep,Devir)
                    values(@Firmaadi,@Yetkili,@Unvani,@OzelKod,@Tel,@Cep,@Devir)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                if (sonuc == "0")
                    DB.ExecuteSQL("update Firmalar set Webde=1 where pkFirma=" + dt.Rows[i]["pkFirma"].ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fileInf = new FileInfo("Hitit2012.rar");
                ///images/merpa_logo.jpg
                string uri = "ftp://ftp.hitityazilim.com/httpdocs/guncelleme/Hitit2012.rar";//Burada uplaod ediceğiniz dizini tam oalrak belirtmelisiniz.
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential("hitityazilim", "Hitit9999");
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();

                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                frmYukleniyor yukleniyor = new frmYukleniyor();
                yukleniyor.TopMost = true;
                yukleniyor.Show();

                while (contentLen != 0)
                {
                    yukleniyor.Show();
                    Application.DoEvents();
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);

                    yukleniyor.Text = buffLength.ToString();
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

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(SatislariWebeGonder));
            thread1.Start();
        }
        private void SatislariWebeGonder()
        {
            //-- null ise insert 0 ise güncelle 1 ise gönderildi
            DataTable dt = DB.GetData("select top 100 * from Satislar with(nolock)  where GonderildiWS is null ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString())));
                list.Add(new SqlParameter("@fkFirma", dt.Rows[i]["fkFirma"].ToString()));
                list.Add(new SqlParameter("@fkSatisDurumu", dt.Rows[i]["fkSatisDurumu"].ToString()));
                list.Add(new SqlParameter("@Aciklama", dt.Rows[i]["Aciklama"].ToString()));
                list.Add(new SqlParameter("@ToplamTutar", dt.Rows[i]["ToplamTutar"].ToString().Replace(",", ".")));

                string sql = @"insert into Satislar (Tarih,fkFirma,fkSatisDurumu,Aciklama,ToplamTutar)
                    values(@Tarih,@fkFirma,@fkSatisDurumu,@Aciklama,@ToplamTutar)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);

                if (sonuc == "0")
                    DB.ExecuteSQL("update Satislar set GonderildiWS=1 where pkSatislar=" + dt.Rows[i]["pkSatislar"].ToString());
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(SatisDetayWebeGonder));
            thread1.Start();
        }
        private void SatisDetayWebeGonder()
        {
            //-- null ise insert 0 ise güncelle 1 ise gönderildi
            DataTable dt = DB.GetData("select top 100 * from SatisDetay with(nolock)  where GonderildiWS is null ");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@fkSatislar", dt.Rows[i]["fkSatislar"].ToString()));
                list.Add(new SqlParameter("@Tarih", Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString())));
                list.Add(new SqlParameter("@fkStokKarti", dt.Rows[i]["fkStokKarti"].ToString()));
                list.Add(new SqlParameter("@Adet", dt.Rows[i]["Adet"].ToString()));
                list.Add(new SqlParameter("@SatisFiyati", dt.Rows[i]["SatisFiyati"].ToString().Replace(",", ".")));

                string sql = @"insert into SatisDetay (Tarih,fkSatislar,fkStokKarti,Adet,SatisFiyati)
                    values(@Tarih,@fkSatislar,@fkStokKarti,@Adet,@SatisFiyati)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                if (sonuc == "0")
                    DB.ExecuteSQL("update SatisDetay set GonderildiWS=1 where pkSatisDetay=" + dt.Rows[i]["pkSatisDetay"].ToString());
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(HatirlatmalariWebeGonder));
            thread1.Start();
        }
        private void HatirlatmalariWebeGonder()
        {
            //MessageBox.Show("Kullanıcı Bulunamadı");
            //return;

            DataTable dt = DB.GetData("select top 100 * from Hatirlatma with(nolock) where isnull(Uyar,0)=0");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@Konu", dt.Rows[i]["Konu"].ToString()));
                list.Add(new SqlParameter("@Tarih", dt.Rows[i]["Tarih"].ToString()));
                list.Add(new SqlParameter("@Aciklama", dt.Rows[i]["Aciklama"].ToString()));
                list.Add(new SqlParameter("@fkCek", dt.Rows[i]["fkCek"].ToString()));
                list.Add(new SqlParameter("@SmsGonder", dt.Rows[i]["SmsGonder"].ToString()));
                list.Add(new SqlParameter("@EpostaGonder", dt.Rows[i]["EpostaGonder"].ToString()));
                list.Add(new SqlParameter("@Kategori", dt.Rows[i]["Kategori"].ToString()));

                string sql = @"insert into Hatirlatma (Konu,Tarih,Aciklama,fkCek,SmsGonder,EpostaGonder,Kategori)
                    values(@Konu,@Tarih,@Aciklama,@fkCek,@SmsGonder,@EpostaGonder,@Kategori)";

                string sonuc = DBWeb.ExecuteSQL_Web(sql, list);
                //  if (sonuc == "0")
                //DB.ExecuteSQL("update StokKarti set MiadKontrol=1 where pkStokKarti=" + dt.Rows[i]["pkStokKarti"].ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(FirmalariWebeGonder));
            thread1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            skrs3servis.WSSKRSSistemlerClient sonuc = GetSKRS3Client("900092", "789nn789");
            //skrs3servis.wsskrsSistemlerResponse sistemler = new skrs3servis.wsskrsSistemlerResponse();
            GPTS.skrs3servis.kodDegerleriResponse mahalleler = new skrs3servis.kodDegerleriResponse();
            try
            {
                //sistemler = sonuc.Sistemler();

                mahalleler = sonuc.SistemKodlari("8462635e-5253-4e7b-8010-6020fd1501df", dateEdit1.DateTime);
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }

            int gelenmah = mahalleler.kodDegerleri.Length;
            for (int i = 0; i < gelenmah; i++)
            {
                string koy_kodu = mahalleler.kodDegerleri[i][0].kolonIcerigi.Value;
                string mahkodu_adi = mahalleler.kodDegerleri[i][1].kolonIcerigi.Value;
                string mahale_kodu = mahalleler.kodDegerleri[i][2].kolonIcerigi.Value;
                string mah_aktif = mahalleler.kodDegerleri[i][3].kolonIcerigi.Value;
                //string mah_tanim = mahalleler.kodDegerleri[i][4].kolonIcerigi.Value;
                string mah_tipi = mahalleler.kodDegerleri[i][5].kolonIcerigi.Value;
                //string mah_yetkili_iadre_kodu = mahalleler.kodDegerleri[i][6].kolonIcerigi.Value;
                string mah_olus_tarih = mahalleler.kodDegerleri[i][7].kolonIcerigi.Value;
                string mah_gun_tarih = mahalleler.kodDegerleri[i][8].kolonIcerigi.Value;
                if (DB.GetData("select count(*) from TMahalle with(nolock) where mahalle_kodu='" + mahale_kodu + "'").Rows[0][0].ToString() == "0")
                {
                    DB.ExecuteSQL("insert into TMahalle (mahalle_adi,mahalle_kodu,aktif,koy_kodu,u_kullanici_id,u_tarih)" +
                        " values('" + mahkodu_adi + "','" + mahale_kodu + "',1,'" + koy_kodu + "',-1,getdate())");
                }

            }

            //900092
            //789nn789            

            //SKRS.WSSKRSSistemlerService servis = new SKRS.WSSKRSSistemlerService();
            ////servis.ClientCredentials.UserName.UserName = "900092";
            //servis.Credentials = new System.Net.NetworkCredential("900092", "789nn789");
            //ClientCertificates.Add(.Password = "789nn789";
            //servis.Open();

            //UsernameToken userToken = new UsernameToken(SAGLIKNET_KULLANICI_ADI, SAGLIKNET_KULLANICI_SIFRE, PasswordOption.SendPlainText);
            //NabizSrvc.SetClientCredential(userToken);
            //NabizSrvc.RequestSoapContext.Security.Timestamp.TtlInSeconds = 1000000;
            //NabizSrvc.RequestSoapContext.Security.Tokens.Add(userToken);
            //NabizSrvc.RequestSoapContext.Security.MustUnderstand = false;


            //skrs2.responseSistemler response = new skrs2.responseSistemler();
            //try
            //{
            //    SKRS.wsskrsSistemlerResponse res = new SKRS.wsskrsSistemlerResponse();

            //    res = servis.Sistemler();

            //    SKRS.kodDegerleriResponse gelenveri = new SKRS.kodDegerleriResponse();
            //    gelenveri = servis.SistemKodlari("5bc508fa-782a-4d75-831f-34948e350e72", DateTime.Today.AddYears(-2), true);
            //}
            //catch (Exception exp)
            //{

            //    //Possible SOAP version mismatch: Envelope namespace http://ws.esaglik.surat.com.tr/ was unexpected. Expecting http://schemas.xmlsoap.org/soap/envelope/.
            //}
        }


        public static skrs3servis.WSSKRSSistemlerClient GetSKRS3Client(string userName, string password)
        {
            string url = "https://yws.sagliknet.saglik.gov.tr/ESaglikYardimciServislerApp-WSHariciWebServisleri/WSSKRSSistemlerPort"; //gerÃ§ek

            CustomBinding bind = new CustomBinding();
            TransportSecurityBindingElement tsbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            tsbe.EnableUnsecuredResponse = true;
            tsbe.SetKeyDerivation(true);
            tsbe.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
            tsbe.IncludeTimestamp = false;
            tsbe.AllowInsecureTransport = true;
            tsbe.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.CombinedEntropy;
            tsbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
            bind.Elements.Add(tsbe);


            TextMessageEncodingBindingElement tmebe = new TextMessageEncodingBindingElement();
            tmebe.MaxReadPoolSize = 64;
            tmebe.MaxWritePoolSize = 16;
            tmebe.MessageVersion = MessageVersion.Soap11WSAddressing10;
            tmebe.WriteEncoding = System.Text.Encoding.UTF8;
            tmebe.ReaderQuotas.MaxDepth = 32;
            tmebe.ReaderQuotas.MaxStringContentLength = 8192;
            tmebe.ReaderQuotas.MaxArrayLength = 16384;
            tmebe.ReaderQuotas.MaxBytesPerRead = 4096;
            tmebe.ReaderQuotas.MaxNameTableCharCount = 16384;
            bind.Elements.Add(tmebe);

            HttpsTransportBindingElement httpstbe = new HttpsTransportBindingElement();
            httpstbe.MaxReceivedMessageSize = 2147483647;//4194304;
            httpstbe.MaxBufferSize = 2147483647;
            httpstbe.AllowCookies = true;
            //httpstbe.DecompressionEnabled = true;

            bind.Elements.Add(httpstbe);
            EndpointAddress endpointAddress = new EndpointAddress(url);

            skrs3servis.WSSKRSSistemlerClient client = new skrs3servis.WSSKRSSistemlerClient(bind, endpointAddress);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;

            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);
            return client;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            gridControl3.DataSource = DB.GetData("SELECT * FROM ILLER");
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(e.RowHandle);

            gridControl4.DataSource = DB.GetData("SELECT * FROM Ilceler where il_kodu=" + dr["KODU"].ToString());
        }

        private void gridView4_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView4.GetDataRow(e.RowHandle);

            gridControl7.DataSource = DB.GetData("SELECT * FROM TMahalle where koy_kodu=" + dr["koy_kodu"].ToString());


        }

        private void gridView3_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(e.RowHandle);

            gridControl6.DataSource = DB.GetData("SELECT * FROM TBucak where ilce_kodu=" + dr["ilce_kodu"].ToString());
        }

        private void gridView5_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            DataRow dr = gridView5.GetDataRow(e.RowHandle);

            gridControl5.DataSource = DB.GetData("SELECT * FROM TKoy where bucak_kodu=" + dr["bucak_kodu"].ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            skrs3servis.WSSKRSSistemlerClient sonuc = GetSKRS3Client("900092", "789nn789");
            //skrs3servis.wsskrsSistemlerResponse sistemler = new skrs3servis.wsskrsSistemlerResponse();
            GPTS.skrs3servis.kodDegerleriResponse bucaklar = new skrs3servis.kodDegerleriResponse();
            try
            {
                //sistemler = sonuc.Sistemler();

                bucaklar = sonuc.SistemKodlari("822af824-4163-46f8-b028-3741259b8471", dateEdit2.DateTime);
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }

            if (bucaklar.kodDegerleri == null)
            {
                formislemleri.Mesajform("Yeni Kayıt Yok", "K", 100);
                return;
            }
            int gelenmah = bucaklar.kodDegerleri.Length;
            for (int i = 0; i < gelenmah; i++)
            {
                string koy_kodu = bucaklar.kodDegerleri[i][0].kolonIcerigi.Value;
                string bucak_adi = bucaklar.kodDegerleri[i][1].kolonIcerigi.Value;
                string bucak_kodu = bucaklar.kodDegerleri[i][2].kolonIcerigi.Value;
                string mah_aktif = bucaklar.kodDegerleri[i][3].kolonIcerigi.Value;
                //string mah_tanim = mahalleler.kodDegerleri[i][4].kolonIcerigi.Value;
                //string mah_tipi = bucaklar.kodDegerleri[i][5].kolonIcerigi.Value;
                //string mah_yetkili_iadre_kodu = mahalleler.kodDegerleri[i][6].kolonIcerigi.Value;
                string mah_olus_tarih = bucaklar.kodDegerleri[i][4].kolonIcerigi.Value;
                string mah_gun_tarih = bucaklar.kodDegerleri[i][5].kolonIcerigi.Value;
                if (DB.GetData("select count(*) from TBucak with(nolock) where bucak_kodu='" + bucak_kodu + "'").Rows[0][0].ToString() == "0")
                {
                    DB.ExecuteSQL("insert into TBucak (bucak_adi,bucak_kodu,aktif,ilce_kodu,u_kullanici_id,u_tarih)" +
                        " values('" + bucak_adi + "','" + bucak_kodu + "',1,'" + koy_kodu + "',-1,getdate())");
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            skrs3servis.WSSKRSSistemlerClient sonuc = GetSKRS3Client("900092", "789nn789");
            //skrs3servis.wsskrsSistemlerResponse sistemler = new skrs3servis.wsskrsSistemlerResponse();
            GPTS.skrs3servis.kodDegerleriResponse koyler = new skrs3servis.kodDegerleriResponse();
            try
            {
                //sistemler = sonuc.Sistemler();
                koyler = sonuc.SistemKodlari("186585bf-70b6-4db4-805e-22177714d12e", dateEdit3.DateTime);
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }

            if (koyler.kodDegerleri == null)
            {
                formislemleri.Mesajform("Yeni Kayıt Yok", "K", 100);
                return;
            }

            int gelenmah = koyler.kodDegerleri.Length;
            for (int i = 0; i < gelenmah; i++)
            {
                string bucak_kodu = koyler.kodDegerleri[i][0].kolonIcerigi.Value;
                string koy_adi = koyler.kodDegerleri[i][1].kolonIcerigi.Value;
                string koy_kodu = koyler.kodDegerleri[i][2].kolonIcerigi.Value;
                string mah_aktif = koyler.kodDegerleri[i][3].kolonIcerigi.Value;
                //string mah_tanim = mahalleler.kodDegerleri[i][4].kolonIcerigi.Value;
                string sirano = koyler.kodDegerleri[i][4].kolonIcerigi.Value;
                //string mah_yetkili_iadre_kodu = mahalleler.kodDegerleri[i][6].kolonIcerigi.Value;
                string mah_olus_tarih = koyler.kodDegerleri[i][5].kolonIcerigi.Value;
                string mah_gun_tarih = koyler.kodDegerleri[i][6].kolonIcerigi.Value;
                if (DB.GetData("select count(*) from TKoy with(nolock) where koy_kodu='" + koy_kodu + "'").Rows[0][0].ToString() == "0")
                {
                    DB.ExecuteSQL("insert into TKoy (koy_adi,koy_kodu,aktif,bucak_kodu,u_kullanici_id,u_tarih)" +
                        " values('" + koy_adi + "','" + koy_kodu + "',1,'" + bucak_kodu + "',-1,getdate())");
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            yemeksepetiws.AuthHeader aut = new yemeksepetiws.AuthHeader();
            aut.UserName = "admin";
            aut.Password = "admin";

            yemeksepetiws.IntegrationSoapClient iws = new yemeksepetiws.IntegrationSoapClient();
            string message = iws.GetAllMessages(aut);
            MessageBox.Show(message);

            //IntegrationWebService.Integration iws = new IntegrationTest.IntegrationWebService.Integration();
            //iws.AuthHeaderValue = new IntegrationTest.IntegrationWebService.AuthHeader();

            //iws.AuthHeaderValue.UserName = “Your UserName”;

            //iws.AuthHeaderValue.Password = “Your Password”;


        }

        private void button12_Click(object sender, EventArgs e)
        {
            webapidengetir();
        }

        void webapidengetir()
        {

            string apiurl = "http://95.130.169.42/api/stoklar";//1
            //apiurl = DB.webapiurl + "/api/Tetkikistek";///1";
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(apiurl);
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
                {
                    var reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();
                    var arr = JsonConvert.DeserializeObject<JArray>(s);
                    gridControl8.DataSource = arr;
                }
                else
                {
                    MessageBox.Show(string.Format("Status code == {0}", webResponse.StatusCode));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnn11_Click(object sender, EventArgs e)
        {
            /*
                        //string apiurl = "https://api.n11.com/ws/ProductService.wsdl";
                        Api.CategoryRequest cat = new Api.CategoryRequest();

                        Api.ProductServicePortClient servis = new Api.ProductServicePortClient();
                        //Api.Authentication;

                        Api.get istek = new Api.GetProductListRequest();
                        istek.auth = new Api.Authentication();
                        istek.auth.appKey = "79ee55a9-fdae-461a-bee6-d438a4320000";
                        istek.auth.appSecret = "ItRQgbqn7WrCqSa9";
                        try
                        {
                            Api.GetProductListResponse cevap = servis.GetProductList(istek);
                        }
                        catch (Exception)
                        {

                            throw;
                        }


                        //string apiurl = "https://api.n11.com/ws/ProductService.wsdl";
                        Api.ProductServicePortClient servis = new Api.ProductServicePortClient();
                        //Api.Authentication;

                        Api.GetProductListRequest istek = new Api.GetProductListRequest();
                        istek.auth = new Api.Authentication();
                        istek.auth.appKey = "79ee55a9-fdae-461a-bee6-d438a4320000";
                        istek.auth.appSecret = "ItRQgbqn7WrCqSa9";
                        try
                        {
                            Api.GetProductListResponse cevap = servis.GetProductList(istek);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        */
            //com.n11.api.ProductServicePortService servis = new com.n11.api.ProductServicePortService();

            //var authentication = new Api.ProductService.Authentication();
            //authentication.appKey = “”; //api anahtarınız
            //authentication.appSecret = “”;//api şifeniz

            //var productImageList = new List<Api.ProductService.ProductImage>();
        }

        private void btnEArsiv_Click(object sender, EventArgs e)
        {
            EArsivDiyaLogo.PostBoxServiceEndpoint servis = new EArsivDiyaLogo.PostBoxServiceEndpoint();
            EArsivDiyaLogo.LoginType lt = new EArsivDiyaLogo.LoginType();
            lt.userName = "9460069536";
            lt.passWord = "123abc";
            string sessionID = "";//Guid.NewGuid().ToString();
            bool LoginResult = false, LoginResultSpecified = false;
            servis.Login(lt, out LoginResult, out LoginResultSpecified, out sessionID);

            //9f1b19f7-1bd9-49df-89d2-187823ff7fd3
            if (sessionID == "")
            {
                MessageBox.Show("id alınamadı");
                return;
            }
            else
            {
                MessageBox.Show("id =" + sessionID);
                //return;
            }
            //mükellef mi sorgula
            string uuid = "";
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            EArsivDiyaLogo.DocumentType document = new EArsivDiyaLogo.DocumentType();
            string dosyaYolxml = exeDiz + "\\eArsiv\\f60bfb19-4ac7-4c62-b704-37a51a3d1cfc.xml";
            string dosyaYolzip = exeDiz + "\\eArsiv\\f60bfb19-4ac7-4c62-b704-37a51a3d1cfc.zip";
            document.fileName = dosyaYolxml;

            int _id = 0;
            bool isRefid = false;
            string[] paramList = new string[3];
            paramList[0] = "DOCUMENTTYPE=EARCHIVE";
            paramList[1] = "ALIAS=urn:mail:gurbuzadem@gmail.com";
            //if(iptal=="")
            //   "DOCUMENTTYPE = CANCELEARCHIVEINVOICE"
            paramList[2] = "SIGNED=0";
            //EArsivDiyaLogo.ElementType element = new EArsivDiyaLogo.ElementType();
            byte[] file = File.ReadAllBytes(dosyaYolzip);
            //MemoryStream memory = new MemoryStream(file);
            //BinaryReader reader = new BinaryReader(memory);
            
            document.binaryData = new EArsivDiyaLogo.base64BinaryData();
            document.binaryData.Value = file;//reader.ReadByte();//zip formatında olmalı 

            var md5 = MD5.Create();
            var stream = File.OpenRead(dosyaYolxml);
            var hash = md5.ComputeHash(stream);

            document.hash = hash.ToString();


            EArsivDiyaLogo.ResultType resultType =
            servis.SendDocument(sessionID, paramList, document, out _id, out isRefid);
            if (resultType.resultCode == -1)
            {

            }
            if (resultType.resultMsg != "")
            {
                MessageBox.Show(resultType.resultMsg);
            }

            if (resultType.outputList != null)
            {
                var list = resultType.outputList;
            }
            //SendEArchiveEmail(guid, "", uuid, "gurbuzadem@gmail.com");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //    using (EArsivDiyaLogo.PostBoxServiceEndpoint svc = new EArsivDiyaLogo.PostBoxServiceEndpoint())
            //    {
            //        EArsivDiyaLogo.LoginType login = new EArsivDiyaLogo.LoginType();
            //        login.userName = "test";
            //        login.passWord = "test";

            //        string sessionId;
            //        if (svc.Login(login, out sessionId))

            //        { string[] paramList = new string[3];
            //            paramList[0] = "DOCUMENTTYPE=RECEIPTADVICE";
            //            EArsivDiyaLogo.ElementType element = new EArsivDiyaLogo.ElementType();
            //            EArsivDiyaLogo.ResultType result = svc.GetDocument(sessionId, paramList, out element);

            //            if (result.resultCode == 1)
            //            { File.WriteAllBytes(@"c:\x\" + element.fileName, element.binaryData.Value);
            //                svc.GetDocumentDone(sessionId, element.fileName, paramList);
            //                Console.Write("Belge alındı " + result.resultMsg);
            //            }
            //            else
            //                Console.Write("Gönderilemedi " + result.resultMsg);
            //        }
            //    }
            //}

        }
        private void button14_Click(object sender, EventArgs e)
        {
            #region Variables

            string ublVersionID = "2.1";
            const string customizationID = "TR1.2";
            const string profileID = "EARSIVFATURA";
            const string idPrefix = "DIS";
            string schemeValue = Convert.ToString(DateTime.Now.Year) + String.Format("{0:#000000000}", 3);
            UBLTR.ID faturaZimmetNo = new UBLTR.ID { Value = idPrefix + schemeValue };

            const bool copyIndicator = false;
            string uUID = Guid.NewGuid().ToString();
            DateTime issueDate = Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", DateTime.Now));
            DateTime issueTime = Convert.ToDateTime(String.Format("{0:hh:mm:ss}", DateTime.Now));
            const string invoiceTypeCode = "SATIS";
            const string tutarYaziPrefix = "Yalnız ";
            const string tutarYazi = "YüzOnSekiz TL";
            const string listAgencyName = "United Nations Economic Commission for Europe";
            const string listID = "ISO 4217 Alpha";
            const string listName = "Currency";
            const ushort listVersionID = 2001;
            const string documentCurrencyCodeValue = "TRY";
            const int lineCountNumeric = 2;
            string despatchDocumentReferenceValue = String.Format("{0:#0000000000000000}", 17);
            const string gonderimSekliID = "gonderimSekli";
            const string gonderimSekliDT = "ELEKTRONIK";
            const string duzenlemeTarihiID = "duzenlemeTarihi";
            const string eINVOICEID = "EINVOICE";
            const string eINVOICEDT = "3";
            const string odemeSekliID = "internetSatisBilgi/odemeSekli";
            const string odemeSekliDT = "KREDIKARTI";
            const string webAdresiID = "internetSatisBilgi/webAdresi";
            const string webAdresiDT = "WWW.XX.COM";
            const string odemeTarihiID = "internetSatisBilgi/odemeTarihi";
            const string faturaZimmetNoDT = "XSLT";
            const string characterSetCode = "UTF-8";
            const string encodingCode = "Base64";
            string fileName = Convert.ToString(faturaZimmetNo) + ".xslt";
            const string mimeCode = "application/xml";
            string tutarYaziDT = "TOTAL_NET_STR";
            const string signatureScheme = "VKN_TCKN";

            const string supplierIdentificationScheme = "VKN";
            const string vkn = "2211223322";
            const string supplierIdentificationTicaret = "TICARETSICILNO";
            const string supplierIdentificationMersisNo = "fqefeqqfe";
            const string supplierName = "A FİRMA SAN. TİC. AŞ.";
            const string supplierStreetName = "Değirmenyolu";
            const string supplierBuildingNumber = "";
            const string supplierCitySubdivisionName = "Ataşehir";
            const string supplierCityName = "İstanbul";
            const string supplierPostalZone = "";
            const string supplierRegion = "Ataşehir";
            const string countryIdentificationCode = "TR";
            const string country = "TÜRKİYE";
            const string supplierTaxScheme = "SINDIRGI VERGİ DAİRE";
            string digitalSignatureURI = "#Signature_" + Convert.ToString(faturaZimmetNo);
            const string supplierTelephone = "02626798099";
            const string supplierFax = "";
            const string supplierMail = "irem.dehmen@logo.com.tr";

            const string customerIDScheme = "VKN";
            const string customerTCKN = "1234567890";
            //const string customerIdentificationTicaret = "TICARETSICILNO";
            //const string customerIdentificationMersisNo = "fqefeqqfe";
            const string customerPartyName = "EARŞİV CARİ";
            const string customerStreetName = "xxxx";
            const string customerBuildingNumber = "";
            const string customerCitySubdivisionName = "Beşiktaş";
            const string customerCityName = "İstanbul";
            const string customerPostalZone = "";
            const string customerRegion = "";
            const string customerCountryIdentificationCode = "TR";
            const string customerCountry = "";
            const string customerTaxScheme = "";
            const string customerTelephone = "";
            const string customerFax = "";
            const string customerMail = "";

            const string taxCurrencyID = "TRY";
            double taxCurrencyValue = 18.0;
            int taxableAmount = 100;
            double percent = 18.0;
            const string taxSchemeName = "KDV";
            const string taxTypeCode = "0015";
            double lineExtensionAmount = 100.00;
            double taxExclusiveAmount = 100.00;
            double taxInclusiveAmount = 118.00;
            double allowanceTotalAmount = 0.00;
            double payableAmount = 118.00;

            string prefixCAC = "cac";
            string prefixCBC = "cbc";
            string namespaceCAC = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            string namespaceCBC = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            #endregion

            #region fatura xml
            XmlDocument doc = new XmlDocument();

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement root = doc.CreateElement("invoice");
            XmlElement element = null;
            XmlElement elementChild = null;
            XmlElement elementChildChild = null;

            XmlAttribute schemaLoc = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            schemaLoc.Value = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 UBL-Invoice-2.1.xsd";
            root.SetAttributeNode(schemaLoc);
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("xmlns:udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
            root.SetAttribute("xmlns:ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
            root.SetAttribute("xmlns:qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
            root.SetAttribute("xmlns:ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            root.SetAttribute("xmlns:ccts", "urn:un:unece:uncefact:documentation:2");
            root.SetAttribute("xmlns:cbc", namespaceCBC);
            root.SetAttribute("xmlns:cac", namespaceCAC);
            root.SetAttribute("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            doc.AppendChild(root);

            element = doc.CreateElement(prefixCBC, "UBLVersionID", namespaceCBC);
            element.InnerText = ublVersionID;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "CustomizationID", namespaceCBC);
            element.InnerText = customizationID;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "ProfileID", namespaceCBC);
            element.InnerText = profileID;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            element.InnerText = idPrefix + schemeValue;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "CopyIndicator", namespaceCBC);
            element.InnerText = copyIndicator.ToString();
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "UUID", namespaceCBC);
            element.InnerText = uUID;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
            element.InnerText = issueDate.ToString();
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "IssueTime", namespaceCBC);
            element.InnerText = issueTime.ToString();
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "InvoiceTypeCode", namespaceCBC);
            element.InnerText = invoiceTypeCode;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "Note", namespaceCBC);
            element.InnerText = tutarYaziPrefix + tutarYazi;
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "DocumentCurrencyCode", namespaceCBC);
            element.InnerText = documentCurrencyCodeValue.ToString();
            element.SetAttribute("listVersionID", listVersionID.ToString());
            element.SetAttribute("listName", listName);
            element.SetAttribute("listID", listID);
            element.SetAttribute("listAgencyName", listAgencyName);
            root.AppendChild(element);

            element = doc.CreateElement(prefixCBC, "LineCountNumeric", namespaceCBC);
            element.InnerText = lineCountNumeric.ToString();
            root.AppendChild(element);

            element = doc.CreateElement(prefixCAC, "AdditionalDocumentReference", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = gonderimSekliID;
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
            elementChild.InnerText = issueDate.ToString();
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "DocumentType", namespaceCBC);
            elementChild.InnerText = gonderimSekliDT;
            element.AppendChild(elementChild);
            root.AppendChild(element);

            element = doc.CreateElement(prefixCAC, "AdditionalDocumentReference", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = duzenlemeTarihiID;
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
            elementChild.InnerText = issueDate.ToString();
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "DocumentType", namespaceCBC);
            elementChild.InnerText = issueTime.ToString();
            element.AppendChild(elementChild);
            root.AppendChild(element);

            element = doc.CreateElement(prefixCAC, "AdditionalDocumentReference", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = eINVOICEID;
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
            elementChild.InnerText = issueDate.ToString();
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "DocumentType", namespaceCBC);
            elementChild.InnerText = eINVOICEDT;
            element.AppendChild(elementChild);
            root.AppendChild(element);

            element = doc.CreateElement(prefixCAC, "AdditionalDocumentReference", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = idPrefix + schemeValue;
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
            elementChild.InnerText = issueDate.ToString();
            element.AppendChild(elementChild);
            elementChild = doc.CreateElement(prefixCAC, "Attachment", namespaceCAC);
            elementChildChild = doc.CreateElement(prefixCBC, "EmbeddedDocumentBinaryObject", namespaceCBC);
            elementChildChild.InnerText = "foajfoşjeıfheuaaodkeaojıfhıaelfıaejfojdfoae";
            elementChildChild.SetAttribute("mimeCode", mimeCode);
            elementChildChild.SetAttribute("fileName", fileName);
            elementChildChild.SetAttribute("encodingCode", encodingCode);
            elementChildChild.SetAttribute("characterSetCode", characterSetCode);
            elementChild.AppendChild(elementChildChild);
            element.AppendChild(elementChild);
            root.AppendChild(element);

            // AccountingSupplierParty
            element = doc.CreateElement(prefixCAC, "AccountingSupplierParty", namespaceCAC);
            XmlElement elementParty = doc.CreateElement(prefixCAC, "Party", namespaceCAC);
            XmlElement elementWebSite = doc.CreateElement(prefixCBC, "WebsiteURI", namespaceCBC);
            elementWebSite.InnerText = "http://www.yeditepedishastanesi.com/";
            elementParty.AppendChild(elementWebSite);

            XmlElement elementPartyIdentification1 = doc.CreateElement(prefixCAC, "PartyIdentification", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = vkn;
            elementChild.SetAttribute("schemeID", supplierIdentificationScheme);
            elementPartyIdentification1.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyIdentification1);

            XmlElement elementPartyIdentification2 = doc.CreateElement(prefixCAC, "PartyIdentification", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = vkn;
            elementChild.SetAttribute("schemeID", supplierIdentificationTicaret);
            elementPartyIdentification2.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyIdentification2);

            XmlElement elementPartyIdentification3 = doc.CreateElement(prefixCAC, "PartyIdentification", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = supplierIdentificationMersisNo;
            elementChild.SetAttribute("schemeID", "MERSISNO");
            elementPartyIdentification3.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyIdentification3);

            XmlElement elementPartyName = doc.CreateElement(prefixCAC, "PartyName", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementChild.InnerText = "T.C. Yeditepe Diş Hastanesi";
            elementPartyName.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyName);

            XmlElement elementPostalAddress = doc.CreateElement(prefixCAC, "PostalAddress", namespaceCAC);
            XmlElement elementStreetName = doc.CreateElement(prefixCBC, "StreetName", namespaceCBC);
            elementStreetName.InnerText = "Bağdat Cad.";
            elementPostalAddress.AppendChild(elementStreetName);
            XmlElement elementBuildingNumber = doc.CreateElement(prefixCBC, "BuildingNumber", namespaceCBC);
            elementBuildingNumber.InnerText = "828";
            elementPostalAddress.AppendChild(elementBuildingNumber);
            XmlElement elementCitySubdivisionName = doc.CreateElement(prefixCBC, "CitySubdivisionName", namespaceCBC);
            elementCitySubdivisionName.InnerText = "Kadıköy";
            elementPostalAddress.AppendChild(elementCitySubdivisionName);
            XmlElement elementCityName = doc.CreateElement(prefixCBC, "CityName", namespaceCBC);
            elementCityName.InnerText = "İstanbul";
            elementPostalAddress.AppendChild(elementCityName);
            XmlElement elementPostalZone = doc.CreateElement(prefixCBC, "PostalZone", namespaceCBC);
            elementPostalZone.InnerText = "37000";
            elementPostalAddress.AppendChild(elementPostalZone);
            XmlElement elementCountry = doc.CreateElement(prefixCAC, "Country", namespaceCAC);
            XmlElement elementIdentificationCode = doc.CreateElement(prefixCBC, "IdentificationCode", namespaceCBC);
            elementIdentificationCode.InnerText = "TR";
            elementCountry.AppendChild(elementIdentificationCode);
            XmlElement elementCountryName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementCountryName.InnerText = "TÜRKİYE";
            elementCountry.AppendChild(elementCountryName);
            elementPostalAddress.AppendChild(elementCountry);
            elementParty.AppendChild(elementPostalAddress);

            XmlElement elementPartyTaxScheme = doc.CreateElement(prefixCAC, "PartyTaxScheme", namespaceCAC);
            XmlElement elementTaxScheme = doc.CreateElement(prefixCAC, "TaxScheme", namespaceCAC);
            XmlElement elementPartyTaxName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementPartyTaxName.InnerText = "Büyük Mükellefler";
            elementTaxScheme.AppendChild(elementPartyTaxName);
            elementPartyTaxScheme.AppendChild(elementTaxScheme);
            elementParty.AppendChild(elementPartyTaxScheme);

            XmlElement elementContact = doc.CreateElement(prefixCAC, "Contact", namespaceCAC);
            XmlElement elementTelephone = doc.CreateElement(prefixCBC, "Telephone", namespaceCBC);
            elementTelephone.InnerText = "02164680800";
            elementContact.AppendChild(elementTelephone);
            XmlElement elementTelefax = doc.CreateElement(prefixCBC, "Telefax", namespaceCBC);
            elementTelefax.InnerText = "05325552255";
            elementContact.AppendChild(elementTelefax);
            XmlElement elementElectronicMail = doc.CreateElement(prefixCBC, "ElectronicMail", namespaceCBC);
            elementElectronicMail.InnerText = "e@yeditepe.edu.tr";
            elementContact.AppendChild(elementElectronicMail);
            elementParty.AppendChild(elementContact);
            element.AppendChild(elementParty);
            root.AppendChild(element);

            // AccountingCustomerParty
            element = doc.CreateElement(prefixCAC, "AccountingCustomerParty", namespaceCAC);
            elementParty = doc.CreateElement(prefixCAC, "Party", namespaceCAC);
            elementWebSite = doc.CreateElement(prefixCBC, "WebsiteURI", namespaceCBC);
            //elementWebSite.InnerText = "http://www.yeditepedishastanesi.com/";
            elementParty.AppendChild(elementWebSite);

            elementPartyIdentification1 = doc.CreateElement(prefixCAC, "PartyIdentification", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
            elementChild.InnerText = customerTCKN;
            elementChild.SetAttribute("schemeID", "TCKN");
            elementPartyIdentification1.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyIdentification1);

            elementPartyName = doc.CreateElement(prefixCAC, "PartyName", namespaceCAC);
            elementChild = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            //elementChild.InnerText = "T.C. Yeditepe Diş Hastanesi";
            elementPartyName.AppendChild(elementChild);
            elementParty.AppendChild(elementPartyName);

            elementPostalAddress = doc.CreateElement(prefixCAC, "PostalAddress", namespaceCAC);
            elementStreetName = doc.CreateElement(prefixCBC, "StreetName", namespaceCBC);
            elementStreetName.InnerText = "";
            elementPostalAddress.AppendChild(elementStreetName);
            elementBuildingNumber = doc.CreateElement(prefixCBC, "BuildingNumber", namespaceCBC);
            elementBuildingNumber.InnerText = "";
            elementPostalAddress.AppendChild(elementBuildingNumber);
            elementCitySubdivisionName = doc.CreateElement(prefixCBC, "CitySubdivisionName", namespaceCBC);
            elementCitySubdivisionName.InnerText = "MALTEPE";
            elementPostalAddress.AppendChild(elementCitySubdivisionName);
            elementCityName = doc.CreateElement(prefixCBC, "CityName", namespaceCBC);
            elementCityName.InnerText = "İSTANBUL";
            elementPostalAddress.AppendChild(elementCityName);
            elementPostalZone = doc.CreateElement(prefixCBC, "PostalZone", namespaceCBC);
            elementPostalZone.InnerText = "37600";
            elementPostalAddress.AppendChild(elementPostalZone);
            elementCountry = doc.CreateElement(prefixCAC, "Country", namespaceCAC);
            elementIdentificationCode = doc.CreateElement(prefixCBC, "IdentificationCode", namespaceCBC);
            elementIdentificationCode.InnerText = "TR";
            elementCountry.AppendChild(elementIdentificationCode);
            elementCountryName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementCountryName.InnerText = "TÜRKİYE";
            elementCountry.AppendChild(elementCountryName);
            elementPostalAddress.AppendChild(elementCountry);
            elementParty.AppendChild(elementPostalAddress);

            elementPartyTaxScheme = doc.CreateElement(prefixCAC, "PartyTaxScheme", namespaceCAC);
            elementTaxScheme = doc.CreateElement(prefixCAC, "TaxScheme", namespaceCAC);
            elementPartyTaxName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementPartyTaxName.InnerText = "";
            elementTaxScheme.AppendChild(elementPartyTaxName);
            elementPartyTaxScheme.AppendChild(elementTaxScheme);
            elementParty.AppendChild(elementPartyTaxScheme);

            elementContact = doc.CreateElement(prefixCAC, "Contact", namespaceCAC);
            elementTelephone = doc.CreateElement(prefixCBC, "Telephone", namespaceCBC);
            elementTelephone.InnerText = "";
            elementContact.AppendChild(elementTelephone);
            elementTelefax = doc.CreateElement(prefixCBC, "Telefax", namespaceCBC);
            elementTelefax.InnerText = "";
            elementContact.AppendChild(elementTelefax);
            elementElectronicMail = doc.CreateElement(prefixCBC, "ElectronicMail", namespaceCBC);
            //elementElectronicMail.InnerText = "";
            elementContact.AppendChild(elementElectronicMail);
            elementParty.AppendChild(elementContact);

            XmlElement elementPerson = doc.CreateElement(prefixCAC, "Person", namespaceCAC);
            XmlElement elementFirstName = doc.CreateElement(prefixCBC, "FirstName", namespaceCBC);
            elementFirstName.InnerText = "Ergin";
            elementPerson.AppendChild(elementFirstName);
            XmlElement elementFamilyName = doc.CreateElement(prefixCBC, "FamilyName", namespaceCBC);
            elementFamilyName.InnerText = "Güven";
            elementPerson.AppendChild(elementFamilyName);
            elementParty.AppendChild(elementPerson);
            element.AppendChild(elementParty);
            root.AppendChild(element);

            // TaxTotal
            XmlElement elementTaxTotal = doc.CreateElement(prefixCAC, "TaxTotal", namespaceCAC);
            XmlElement elementTaxAmount = doc.CreateElement(prefixCBC, "TaxAmount", namespaceCBC);
            elementTaxAmount.InnerText = taxCurrencyValue.ToString();
            elementTaxAmount.SetAttribute("currencyID", taxCurrencyID);
            elementTaxTotal.AppendChild(elementTaxAmount);
            XmlElement elementTaxSubtotal = doc.CreateElement(prefixCAC, "TaxSubtotal", namespaceCAC);
            XmlElement elementTaxableAmount = doc.CreateElement(prefixCBC, "TaxableAmount", namespaceCBC);
            elementTaxableAmount.InnerText = taxableAmount.ToString();
            elementTaxableAmount.SetAttribute("currencyID", taxCurrencyID);
            elementTaxSubtotal.AppendChild(elementTaxableAmount);
            XmlElement elementSubTotalTaxAmount = doc.CreateElement(prefixCBC, "TaxAmount", namespaceCBC);
            elementSubTotalTaxAmount.InnerText = taxCurrencyValue.ToString();
            elementSubTotalTaxAmount.SetAttribute("currencyID", taxCurrencyID);
            elementTaxSubtotal.AppendChild(elementSubTotalTaxAmount);
            XmlElement elementPercent = doc.CreateElement(prefixCBC, "Percent", namespaceCBC);
            elementPercent.InnerText = percent.ToString();
            elementTaxSubtotal.AppendChild(elementPercent);
            XmlElement elementTaxCategory = doc.CreateElement(prefixCAC, "TaxCategory", namespaceCAC);
            XmlElement elementTaxTotalTaxScheme = doc.CreateElement(prefixCAC, "TaxScheme", namespaceCAC);
            XmlElement elementTaxTotalName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
            elementTaxTotalName.InnerText = "KDV GERCEK";
            elementTaxTotalTaxScheme.AppendChild(elementTaxTotalName);
            XmlElement elementTaxTotalTaxTypeCode = doc.CreateElement(prefixCBC, "TaxTypeCode", namespaceCBC);
            elementTaxTotalTaxTypeCode.InnerText = taxTypeCode;
            elementTaxTotalTaxScheme.AppendChild(elementTaxTotalTaxTypeCode);
            elementTaxCategory.AppendChild(elementTaxTotalTaxScheme);
            elementTaxSubtotal.AppendChild(elementTaxCategory);
            elementTaxTotal.AppendChild(elementTaxSubtotal);
            root.AppendChild(elementTaxTotal);

            // LegalMonetaryTotal
            XmlElement elementLegalMonetaryTotal = doc.CreateElement(prefixCAC, "LegalMonetaryTotal", namespaceCAC);
            XmlElement elementLineExtensionAmount = doc.CreateElement(prefixCBC, "LineExtensionAmount", namespaceCBC);
            elementLineExtensionAmount.InnerText = lineExtensionAmount.ToString();
            elementLineExtensionAmount.SetAttribute("currencyID", taxCurrencyID);
            elementLegalMonetaryTotal.AppendChild(elementLineExtensionAmount);
            XmlElement elementTaxExclusiveAmount = doc.CreateElement(prefixCBC, "TaxExclusiveAmount", namespaceCBC);
            elementTaxExclusiveAmount.InnerText = taxExclusiveAmount.ToString();
            elementTaxExclusiveAmount.SetAttribute("currencyID", taxCurrencyID);
            elementLegalMonetaryTotal.AppendChild(elementTaxExclusiveAmount);
            XmlElement elementTaxInclusiveAmount = doc.CreateElement(prefixCBC, "TaxInclusiveAmount", namespaceCBC);
            elementTaxInclusiveAmount.InnerText = taxInclusiveAmount.ToString();
            elementTaxInclusiveAmount.SetAttribute("currencyID", taxCurrencyID);
            elementLegalMonetaryTotal.AppendChild(elementTaxInclusiveAmount);
            XmlElement elementAllowanceTotalAmount = doc.CreateElement(prefixCBC, "AllowanceTotalAmount", namespaceCBC);
            elementAllowanceTotalAmount.InnerText = allowanceTotalAmount.ToString();
            elementAllowanceTotalAmount.SetAttribute("currencyID", taxCurrencyID);
            elementLegalMonetaryTotal.AppendChild(elementAllowanceTotalAmount);
            XmlElement elementPayableAmount = doc.CreateElement(prefixCBC, "PayableAmount", namespaceCBC);
            elementPayableAmount.InnerText = payableAmount.ToString();
            elementPayableAmount.SetAttribute("currencyID", taxCurrencyID);
            elementLegalMonetaryTotal.AppendChild(elementPayableAmount);
            root.AppendChild(elementLegalMonetaryTotal);

            int length = 2;
            for (int i = 1; i <= length; i++)
            {
                // InvoiceLine
                XmlElement elementInvoiceLine = doc.CreateElement(prefixCAC, "InvoiceLine", namespaceCAC);
                XmlElement elementInvoiceLineID = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
                elementInvoiceLineID.InnerText = i.ToString();
                elementInvoiceLine.AppendChild(elementInvoiceLineID);
                XmlElement elementInvoiceLineNote = doc.CreateElement(prefixCBC, "Note", namespaceCBC);
                elementInvoiceLineNote.InnerText = "";
                elementInvoiceLine.AppendChild(elementInvoiceLineNote);
                XmlElement elementInvoicedQuantity = doc.CreateElement(prefixCBC, "InvoicedQuantity", namespaceCBC);
                elementInvoicedQuantity.InnerText = "5";
                elementInvoicedQuantity.SetAttribute("unitCode", "NIU");
                elementInvoiceLine.AppendChild(elementInvoicedQuantity);
                XmlElement elementInvoiceLineLineExtensionAmount = doc.CreateElement(prefixCBC, "LineExtensionAmount", namespaceCBC);
                elementInvoiceLineLineExtensionAmount.InnerText = "50";
                elementInvoiceLineLineExtensionAmount.SetAttribute("currencyID", taxCurrencyID);
                elementInvoiceLine.AppendChild(elementInvoiceLineLineExtensionAmount);

                // InvoiceLine DespatchLineReference
                XmlElement elementDespatchLineReference = doc.CreateElement(prefixCAC, "DespatchLineReference", namespaceCAC);
                XmlElement elementLineID = doc.CreateElement(prefixCBC, "LineID", namespaceCBC);
                elementLineID.InnerText = i.ToString();
                elementDespatchLineReference.AppendChild(elementLineID);
                XmlElement elementDocumentReference = doc.CreateElement(prefixCAC, "DocumentReference", namespaceCAC);
                XmlElement elementDocumentReferenceID = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
                elementDocumentReferenceID.InnerText = i.ToString();
                elementDocumentReference.AppendChild(elementDocumentReferenceID);
                XmlElement elementDocumentReferenceIssueDate = doc.CreateElement(prefixCBC, "IssueDate", namespaceCBC);
                elementDocumentReferenceIssueDate.InnerText = issueDate.ToString();
                elementDocumentReference.AppendChild(elementDocumentReferenceIssueDate);
                elementDespatchLineReference.AppendChild(elementDocumentReference);
                elementInvoiceLine.AppendChild(elementDespatchLineReference);

                // InvoiceLine TaxTotal
                XmlElement elementInvoiceLineTaxTotal = doc.CreateElement(prefixCAC, "TaxTotal", namespaceCAC);
                XmlElement elementInvoiceLineTaxAmount = doc.CreateElement(prefixCBC, "TaxAmount", namespaceCBC);
                elementInvoiceLineTaxAmount.InnerText = taxCurrencyValue.ToString();
                elementInvoiceLineTaxAmount.SetAttribute("currencyID", taxCurrencyID);
                elementInvoiceLineTaxTotal.AppendChild(elementInvoiceLineTaxAmount);
                XmlElement elementInvoiceLineTaxSubtotal = doc.CreateElement(prefixCAC, "TaxSubtotal", namespaceCAC);
                XmlElement elementInvoiceLineTaxableAmount = doc.CreateElement(prefixCBC, "TaxableAmount", namespaceCBC);
                elementInvoiceLineTaxableAmount.InnerText = taxableAmount.ToString();
                elementInvoiceLineTaxableAmount.SetAttribute("currencyID", taxCurrencyID);
                elementInvoiceLineTaxSubtotal.AppendChild(elementInvoiceLineTaxableAmount);
                XmlElement elementInvoiceLineSubTotalTaxAmount = doc.CreateElement(prefixCBC, "TaxAmount", namespaceCBC);
                elementInvoiceLineSubTotalTaxAmount.InnerText = taxCurrencyValue.ToString();
                elementInvoiceLineSubTotalTaxAmount.SetAttribute("currencyID", taxCurrencyID);
                elementInvoiceLineTaxSubtotal.AppendChild(elementInvoiceLineSubTotalTaxAmount);
                XmlElement elementInvoiceLinePercent = doc.CreateElement(prefixCBC, "Percent", namespaceCBC);
                elementInvoiceLinePercent.InnerText = percent.ToString();
                elementInvoiceLineTaxSubtotal.AppendChild(elementInvoiceLinePercent);
                XmlElement elementInvoiceLineTaxCategory = doc.CreateElement(prefixCAC, "TaxCategory", namespaceCAC);
                XmlElement elementInvoiceLineTaxTotalTaxScheme = doc.CreateElement(prefixCAC, "TaxScheme", namespaceCAC);
                XmlElement elementInvoiceLineTaxTotalName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
                elementInvoiceLineTaxTotalName.InnerText = "KDV GERCEK";
                elementInvoiceLineTaxTotalTaxScheme.AppendChild(elementInvoiceLineTaxTotalName);
                XmlElement elementInvoiceLineTaxTotalTaxTypeCode = doc.CreateElement(prefixCBC, "TaxTypeCode", namespaceCBC);
                elementInvoiceLineTaxTotalTaxTypeCode.InnerText = taxTypeCode;
                elementInvoiceLineTaxTotalTaxScheme.AppendChild(elementTaxTotalTaxTypeCode);
                elementInvoiceLineTaxCategory.AppendChild(elementInvoiceLineTaxTotalTaxScheme);
                elementInvoiceLineTaxSubtotal.AppendChild(elementInvoiceLineTaxCategory);
                elementInvoiceLineTaxTotal.AppendChild(elementInvoiceLineTaxSubtotal);
                elementInvoiceLine.AppendChild(elementInvoiceLineTaxTotal);

                // InvoiceLine Item
                XmlElement elementInvoiceLineItem = doc.CreateElement(prefixCAC, "Item", namespaceCAC);
                XmlElement elementInvoiceLineItemDescription = doc.CreateElement(prefixCBC, "Description", namespaceCBC);
                elementInvoiceLineItemDescription.InnerText = "KALEM_" + i.ToString();
                elementInvoiceLineItem.AppendChild(elementInvoiceLineItemDescription);
                XmlElement elementInvoiceLineItemName = doc.CreateElement(prefixCBC, "Name", namespaceCBC);
                elementInvoiceLineItemName.InnerText = "TM-000" + i.ToString();
                elementInvoiceLineItem.AppendChild(elementInvoiceLineItemName);
                XmlElement elementItemSellersItemIdentification = doc.CreateElement(prefixCAC, "SellersItemIdentification", namespaceCAC);
                XmlElement elementItemSellersItemIdentificationID = doc.CreateElement(prefixCBC, "ID", namespaceCBC);
                elementItemSellersItemIdentificationID.InnerText = "TM-000" + i.ToString();
                elementItemSellersItemIdentification.AppendChild(elementItemSellersItemIdentificationID);
                elementInvoiceLineItem.AppendChild(elementItemSellersItemIdentification);
                elementInvoiceLine.AppendChild(elementInvoiceLineItem);

                // InvoiceLine Price
                XmlElement elementInvoiceLinePrice = doc.CreateElement(prefixCAC, "Price", namespaceCAC);
                XmlElement elementPricePriceAmount = doc.CreateElement(prefixCBC, "PriceAmount", namespaceCBC);
                elementPricePriceAmount.InnerText = taxCurrencyValue.ToString();
                elementPricePriceAmount.SetAttribute("currencyID", taxCurrencyID);
                elementInvoiceLinePrice.AppendChild(elementPricePriceAmount);
                elementInvoiceLine.AppendChild(elementInvoiceLinePrice);

                root.AppendChild(elementInvoiceLine);
            }

            #endregion

            //string uUID = Guid.NewGuid().ToString();
            string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
            string xmldosyayol = exeDiz + @"\" + uUID + ".xml";// Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + uUID + ".xml";
            doc.Save(xmldosyayol);

            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            byte[] bytes = ms.ToArray();

            //var saveToFilename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//" + uUID + ".zip";

            // ZIP the uploaded contents and save the ZIP to disk!
            using (var zip = new ZipFile())
            {
                //zip.AddFile(path);
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.AddEntry(uUID + ".xml", bytes);
                //zip.AddFile(path, "");
                zip.Save(xmldosyayol.Replace(".xml", ".zip"));
            }

            //string startPath = @"c:\example\start";
            //string zipPath = @"c:\example\result.zip";
            //string extractPath = @"c:\example\extract";

            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);

            ////byte[] zip = Zip(doc.OuterXml);

            ////using (FileStream originalFileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            ////{
            ////    if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
            ////    {
            ////        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
            ////        {
            ////            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionLevel.Fastest, true))
            ////            {
            ////                originalFileStream.CopyTo(compressionStream);
            ////            }
            ////            Console.WriteLine(string.Format("file compressed to {0} bytes", compressedFileStream.Length));
            ////        }
            ////    }
            ////}


        }


        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static class SerializerHelper<T>
        {
            private static XmlSerializer _Serializer;
            private static XmlWriterSettings _XmlSettings;

            private static XmlSerializerNamespaces _Namespaces;

            static SerializerHelper()
            {
                _Namespaces = new XmlSerializerNamespaces();
                //_Namespaces.Add(string.Empty, string.Empty);

                _XmlSettings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8
                };

                _Serializer = new XmlSerializer(typeof(T));
            }

            public static string ObjectToXml(T obj)
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, _XmlSettings))
                    {
                        _Serializer.Serialize(xmlWriter, obj, _Namespaces);
                    }
                    return stringWriter.ToString();
                }
            }
        }
    }
}
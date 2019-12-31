using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.Collections;
using System.Xml;
using System.Data.SqlClient;
using GPTS.Include.Data;
using System.IO;
using GPTS.islemler;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Threading;

namespace GPTS
{
    public partial class frmSmsGonder : DevExpress.XtraEditors.XtraForm
    {
       
        public frmSmsGonder()
        {
            InitializeComponent();
            panelControl3.Height = 0;
        }

        private void frmSmsGonder_Load(object sender, EventArgs e)
        {
            tbKullaniciAdi.Text = Degerler.smskullaniciadi;
            string coz = DB.DecodeFrom64(Degerler.smssifre);
            tbSmsSifre.Text = coz;
            tbSmsBaslik.Text = Degerler.smsbaslik;
            kno.Text = Degerler.smsKullaniciNo;

            SmsSablonlari();

            Gonderilecekler();
            cbTarihAraligi.SelectedIndex = 5;

            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Thread thread1 = new Thread(new ThreadStart(Kalan_Sms));
            thread1.Start();
            //Kalan_Sms();
        }

        #region sms_mobil
        public static string smsgonder(string mesaj, string ceptel, string baslik, string pkSms)
        {
            string coz = DB.DecodeFrom64(Degerler.smssifre);

            mesaj = mesaj.Replace("ş", "s");
            mesaj = mesaj.Replace("ü", "u");
            mesaj = mesaj.Replace("ğ", "g");
            mesaj = mesaj.Replace("ö", "o");
            mesaj = mesaj.Replace("ı", "i");
            mesaj = mesaj.Replace("ç", "c");
            mesaj = mesaj.Replace("â", "a");

            mesaj = mesaj.Replace("Ş", "S");
            mesaj = mesaj.Replace("Ü", "U");
            mesaj = mesaj.Replace("Ğ", "G");
            mesaj = mesaj.Replace("Ö", "O");
            mesaj = mesaj.Replace("I", "i");
            mesaj = mesaj.Replace("Ç", "C");
            mesaj = mesaj.Replace("İ", "i");

            try
            {
                string xml = "<BILGI>" +
                "<KULLANICI_ADI>" + Degerler.smskullaniciadi + "</KULLANICI_ADI>" +
                "<SIFRE>" + coz + "</SIFRE>" +
                "<GONDERIM_TARIH>" + DateTime.Now.ToString("yyyy-MM-dd") + "</GONDERIM_TARIH>" +//2014-02-10 10:52
                "<BASLIK>" + baslik + "</BASLIK>" +
                "</BILGI>" +
                "<ISLEM>";
                if (ceptel != "")
                {
                    xml = xml + "<YOLLA>" +
                    " <MESAJ>" + mesaj + "</MESAJ>" +
                    " <NO>" + ceptel + "</NO>" +//
                    "</YOLLA>";
                }

                xml = xml + "</ISLEM>";

                StringBuilder sb = new StringBuilder();

                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.mobilsms.com.tr/services/api.php?islem=sms");
                request.Timeout = 300000;
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "text/xml";
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string sorgu_sonucu = "0";
                if (response.StatusCode.ToString() == "OK")
                {
                    string durum, islemno;
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string sonuc = sr.ReadToEnd();
                    string[] ayirlandeger = sonuc.Split(' ');
                    durum = ayirlandeger[0];
                    islemno = ayirlandeger[4];
                    sr.Close();
                    if (islemno == "kalan")
                    {
                        DB.ExecuteSQL("update Sms set Mesaj='" + mesaj + "',DurumMesaji='Bakiye Yetersiz',TakipNo='" + islemno + "' where pkSms=" + pkSms);
                        return "STOP";
                    }
                    DB.ExecuteSQL("update Sms set Mesaj='" + mesaj + "',TakipNo='" + islemno + "' where pkSms=" + pkSms);

                    if (ceptel != "")
                        DB.logayaz(ceptel + " Gönderildi.", "");

                    sorgu_sonucu = sms_sorgula(islemno, pkSms);

                    //return response.StatusCode.ToString();
                }
                return sorgu_sonucu;// response.StatusCode.ToString();
            }
            catch (Exception exp)
            {
                DB.logayaz("Hata smsozelgonder: " + exp.Message.ToString(), "");
                return "ERR";
            }
            return "ERR";
        }

        public static string sms_sorgula(string TakipNo, string pkSms)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://www.mobilsms.com.tr/services/api.php?islem=sms&islemno=" + TakipNo);

                request.Timeout = 300000;
                request.Method = "GET";

                request.ContentType = "text/xml";
                String strXML = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    strXML = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    //XmlTextReader reader = new XmlTextReader(dataStream);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXML);

                    XmlNodeList xnList = doc.SelectNodes("SONUCLAR");
                    foreach (XmlNode xn in xnList)
                    {
                        //(1=TAMAMLANDI, 2=BEKLEMEDE, 3=IPTAL)
                        string durumu = xn["DURUM"].InnerText;

                        if (durumu == "1") durumu = "3";
                        else if (durumu == "3") durumu = "0";
                        else if (durumu == "2") durumu = "0";
                        DB.ExecuteSQL("update Sms set Durumu=" + durumu + " where pkSms=" + pkSms);
                    }
                    foreach (XmlNode xn2 in xnList)
                    {
                        string basarili = xn2["BASARILI_SAYISI"].InnerText;
                        DB.ExecuteSQL("update Sms set DurumMesaji=" + basarili + " where pkSms=" + pkSms);
                    }

                    //foreach (XmlNode xn3 in xnList)
                    //{
                    //    string firstName = xn3["BASARISIZ_SAYISI"].InnerText;
                    //}
                    //foreach (XmlNode xn4 in xnList)
                    //{
                    //    string firstName = xn4["BASARISIZLAR"].InnerText;
                    //}

                    return response.StatusCode.ToString();
                }

                //               string xml = "<SONUCLAR>" +
                //"<DURUM>1</DURUM>" +// 
                //"<BASARILI_SAYISI>XXX</BASARILI_SAYISI>" +
                //"<BASARISIZLAR>5000000000,5000000001,5000000002</BASARISIZLAR>" +
                //"</SONUCLAR>";
            }
            catch (Exception exp)
            {
                DB.logayaz("Hata smsozelgonder: " + exp.Message.ToString(), "");
                return "ERR";
            }
            return "OK";

        }

        #endregion

        #region sms_oztek

        public static string smsgonder_oztek(string mesaj, string ceptel, string baslik, string pkSms, string kullaniciadi, string sifre, string kullanicino)
        {
            string coz = DB.DecodeFrom64(Degerler.smssifre);

            try
            {
                //xml içerisinde aşağıdaki gibi değerleri gönderebilirsiniz..
            //<zaman>2014-04-17 08:30:00</zaman>//sms gitmeye başlama zamanı 
            //<zamanasimi>2014-04-17 10:30:00</zamanasimi>//son gönderim deneme zamanı 
            string tur = "Normal";

            //if (turkce.Checked == true) 
                tur = "Turkce";

                string sms1N = "data=<sms><kno>" + kullanicino + "</kno><kulad>" + kullaniciadi + "</kulad><sifre>" + coz + "</sifre>" +
                "<gonderen>" + baslik + "</gonderen>" +
                "<mesaj>" + mesaj + "</mesaj>" +
                "<numaralar>" + ceptel+ "</numaralar>" +
                "<tur>" + tur + "</tur></sms>";

                string sonuc = "";
                using (WebClient wUpload = new WebClient())
                {
                    wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    Byte[] bPostArray = Encoding.UTF8.GetBytes(sms1N);
                    Byte[] bResponse = wUpload.UploadData("http://www.ozteksms.com/panel/smsgonder1Npost.php", "POST", bPostArray);
                    Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                    string sWebPage = new string(sReturnChars);
                    sonuc = sWebPage;
                }
                
                //string sonuc = XmlPost("http://www.ozteksms.com/panel/smsgonder1Npost.php", sms1N);
                //1:7297493:Gonderildi:1:0.039:110   //başarılı
                //2:Kullanici bulunamadi //hatalı
                string _id1 = "", _id2 = "", _id3 = "", TakipNo="";
                if (sonuc.Length > 0)
                {
                    _id1 = sonuc.Split(':')[0].ToString();//1.başarılı

                    ArrayList list = new ArrayList();
                    if (_id1 == "1")
                    {
                        TakipNo = sonuc.Split(':')[1].ToString();//sorgu id
                        _id2 = sonuc.Split(':')[2].ToString();//gönderildi.
                        //_id3 = sonuc.Split(':')[3].ToString();//1 ?
                        //_id3 = sonuc.Split(':')[4].ToString();//giden kuruş
                        //_id3 = sonuc.Split(':')[5].ToString();//kalan tl

                        list.Add(new SqlParameter("@TakipNo", TakipNo));
                        list.Add(new SqlParameter("@DurumMesaji", sonuc));
                        list.Add(new SqlParameter("@GondermeZamani", DateTime.Now));
                    }
                    else
                    {
                        list.Add(new SqlParameter("@TakipNo", TakipNo));
                        list.Add(new SqlParameter("@DurumMesaji", sonuc));
                        list.Add(new SqlParameter("@GondermeZamani", DBNull.Value));
                    }
                    if (_id1 == "1") _id1 = "3";
                    if (_id1 == "2") _id1 = "0";

                    list.Add(new SqlParameter("@Durumu", _id1));
                    list.Add(new SqlParameter("@pkSms", pkSms));
                    DB.ExecuteSQL("UPDATE Sms SET Durumu=@Durumu,TakipNo=@TakipNo,DurumMesaji=@DurumMesaji,GondermeZamani=GetDate() WHERE pkSms=@pkSms", list);
                }
            }
            catch (Exception)
            {
                return "ERR";
                //throw;
            }
            //try
            //{
            //    string xml = "<BILGI>" +
            //    "<KULLANICI_ADI>" + Degerler.smskullaniciadi + "</KULLANICI_ADI>" +
            //    "<SIFRE>" + coz + "</SIFRE>" +
            //    "<GONDERIM_TARIH>" + DateTime.Now.ToString("yyyy-MM-dd") + "</GONDERIM_TARIH>" +//2014-02-10 10:52
            //    "<BASLIK>" + baslik + "</BASLIK>" +
            //    "</BILGI>" +
            //    "<ISLEM>";
            //    if (ceptel != "")
            //    {
            //        xml = xml + "<YOLLA>" +
            //        " <MESAJ>" + mesaj + "</MESAJ>" +
            //        " <NO>" + ceptel + "</NO>" +//
            //        "</YOLLA>";
            //    }

            //    xml = xml + "</ISLEM>";

            //    StringBuilder sb = new StringBuilder();

            //    byte[] bytes = Encoding.UTF8.GetBytes(xml);
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.mobilsms.com.tr/services/api.php?islem=sms");
            //    request.Timeout = 300000;
            //    request.Method = "POST";
            //    request.ContentLength = bytes.Length;
            //    request.ContentType = "text/xml";
            //    using (Stream requestStream = request.GetRequestStream())
            //    {
            //        requestStream.Write(bytes, 0, bytes.Length);
            //    }

            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    string sorgu_sonucu = "0";
            //    if (response.StatusCode.ToString() == "OK")
            //    {
            //        string durum, islemno;
            //        StreamReader sr = new StreamReader(response.GetResponseStream());
            //        string sonuc = sr.ReadToEnd();
            //        string[] ayirlandeger = sonuc.Split(' ');
            //        durum = ayirlandeger[0];
            //        islemno = ayirlandeger[4];
            //        sr.Close();
            //        if (islemno == "kalan")
            //        {
            //            DB.ExecuteSQL("update Sms set Mesaj='" + mesaj + "',DurumMesaji='Bakiye Yetersiz',TakipNo='" + islemno + "' where pkSms=" + pkSms);
            //            return "STOP";
            //        }
            //        DB.ExecuteSQL("update Sms set Mesaj='" + mesaj + "',TakipNo='" + islemno + "' where pkSms=" + pkSms);

            //        if (ceptel != "")
            //            DB.logayaz(ceptel + " Gönderildi.", "");

            //        sorgu_sonucu = sms_sorgula(islemno, pkSms);

            //        //return response.StatusCode.ToString();
            //    }
            //    return sorgu_sonucu;// response.StatusCode.ToString();
            //}
            //catch (Exception exp)
            //{
            //    DB.logayaz("Hata smsozelgonder: " + exp.Message.ToString(), "");
            //    return "ERR";
            //}
            return "ERR";
        }

        public static string sms_sorgula_oztek(string kul_ad, string sifre)
        {
            //http://www.ozteksms.com/panel/kullanicibilgi.php?kul_ad=905314648046&sifre=338B9E
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://www.ozteksms.com/panel/kullanicibilgi.php?kul_ad=" + kul_ad + "&sifre=" + sifre);

                request.Timeout = 300000;
                request.Method = "GET";

                request.ContentType = "text/xml";
                String strXML = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream,Encoding.Default);
                    
                    strXML = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    //XmlTextReader reader = new XmlTextReader(dataStream);
                   // XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(strXML);

                    //XmlNodeList xnList = doc.SelectNodes("SONUCLAR");
                    //foreach (XmlNode xn in xnList)
                    //{
                    //    //(1=TAMAMLANDI, 2=BEKLEMEDE, 3=IPTAL)
                    //    string durumu = xn["DURUM"].InnerText;

                    //    if (durumu == "1") durumu = "3";
                    //    else if (durumu == "3") durumu = "0";
                    //    else if (durumu == "2") durumu = "0";
                    //    //DB.ExecuteSQL("update Sms set Durumu=" + durumu + " where pkSms=" + pkSms);
                    //}
                    //foreach (XmlNode xn2 in xnList)
                    //{
                    //    string basarili = xn2["BASARILI_SAYISI"].InnerText;
                    //    //DB.ExecuteSQL("update Sms set DurumMesaji=" + basarili + " where pkSms=" + pkSms);
                    //}
                    return strXML;
                    //return response.StatusCode.ToString();
                }

                //               string xml = "<SONUCLAR>" +
                //"<DURUM>1</DURUM>" +// 
                //"<BASARILI_SAYISI>XXX</BASARILI_SAYISI>" +
                //"<BASARISIZLAR>5000000000,5000000001,5000000002</BASARISIZLAR>" +
                //"</SONUCLAR>";
            }
            catch (Exception exp)
            {
                DB.logayaz("Hata smsozelgonder: " + exp.Message.ToString(), "");
                return "ERR";
            }
        }

        public static string sms_baslikvemusterino_oztek(string kul_ad, string sifre)
        {
            //http://www.ozteksms.com/panel/kullanicibilgi.php?kul_ad=905314648046&sifre=338B9E
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://www.ozteksms.com/panel/kullanicibilgi.php?kul_ad=" + kul_ad + "&sifre=" + sifre);

                request.Timeout = 300000;
                request.Method = "GET";

                request.ContentType = "text/xml";
                String strXML = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream, Encoding.Default);

                    strXML = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    //XmlTextReader reader = new XmlTextReader(dataStream);
                    // XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(strXML);

                    //XmlNodeList xnList = doc.SelectNodes("SONUCLAR");
                    //foreach (XmlNode xn in xnList)
                    //{
                    //    //(1=TAMAMLANDI, 2=BEKLEMEDE, 3=IPTAL)
                    //    string durumu = xn["DURUM"].InnerText;

                    //    if (durumu == "1") durumu = "3";
                    //    else if (durumu == "3") durumu = "0";
                    //    else if (durumu == "2") durumu = "0";
                    //    //DB.ExecuteSQL("update Sms set Durumu=" + durumu + " where pkSms=" + pkSms);
                    //}
                    //foreach (XmlNode xn2 in xnList)
                    //{
                    //    string basarili = xn2["BASARILI_SAYISI"].InnerText;
                    //    //DB.ExecuteSQL("update Sms set DurumMesaji=" + basarili + " where pkSms=" + pkSms);
                    //}
                    return strXML;
                    //return response.StatusCode.ToString();
                }

                //               string xml = "<SONUCLAR>" +
                //"<DURUM>1</DURUM>" +// 
                //"<BASARILI_SAYISI>XXX</BASARILI_SAYISI>" +
                //"<BASARISIZLAR>5000000000,5000000001,5000000002</BASARISIZLAR>" +
                //"</SONUCLAR>";
            }
            catch (Exception exp)
            {
                DB.logayaz("Hata smsozelgonder: " + exp.Message.ToString(), "");
                return "ERR";
            }
            return "OK";

        }
        #endregion
        
        void Gonderilecekler()
        {
            string sql = @"SELECT  Firmalar.pkFirma, CONVERT(bit, '0') AS Sec, Sms.pkSms, Sms.Mesaj, Sms.Tarih, Firmalar.Tel, 
                         isnull(CariAdi,Firmalar.Firmaadi) as Firmaadi,Firmalar.Yetkili, Firmalar.KaraListe, Firmalar.OzelKod, 
                         Firmalar.Adres,Firmalar.Devir,Sms.CepTel, FirmaGruplari.GrupAdi,Sms.DurumMesaji,Sms.TakipNo 
                         FROM  Sms with(nolock)
                         LEFT JOIN  Firmalar with(nolock) ON Sms.fkFirma = Firmalar.pkFirma 
                         LEFT JOIN FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari where Durumu=0";
            gridControl2.DataSource = DB.GetData(sql);
        }

        void OnayBekleyenler()
        {
            string sql = @"SELECT Firmalar.pkFirma, CONVERT(bit, '1') AS Sec, Sms.pkSms, Sms.Mesaj, Sms.Tarih, Firmalar.Tel, Firmalar.Firmaadi, Firmalar.KaraListe, Firmalar.OzelKod, 
                      Firmalar.Adres, Sms.CepTel, FirmaGruplari.GrupAdi,Sms.TakipNo,Sms.DurumMesaji
                        FROM Sms with(nolock) 
                        LEFT JOIN  Firmalar ON Sms.fkFirma = Firmalar.pkFirma 
                        LEFT JOIN  FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari where Durumu=1";
            gridControl1.DataSource = DB.GetData(sql);
        }
        
        void Gonderilenler2()
        {
            string sql = @"SELECT   Firmalar.pkFirma, CONVERT(bit, '1') AS Sec, Sms.pkSms, Sms.Mesaj, Sms.Tarih, Firmalar.Tel, isnull(CariAdi,Firmalar.Firmaadi) as Firmaadi, 
                Firmalar.Cep, Firmalar.KaraListe, Firmalar.OzelKod, Firmalar.Devir,
                Firmalar.Adres, Sms.CepTel, FirmaGruplari.GrupAdi,Sms.TakipNo,Sms.DurumMesaji,Sms.GondermeZamani,Sms.Durumu
                FROM  Sms with(nolock) 
                LEFT JOIN  Firmalar with(nolock) ON Sms.fkFirma = Firmalar.pkFirma 
                LEFT JOIN  FirmaGruplari with(nolock) ON Firmalar.fkFirmaGruplari = FirmaGruplari.pkFirmaGruplari 
                where Durumu=3 ";
            sql += " and Sms.Tarih>'" + ilktarih.DateTime.ToString("yyyy-MM-dd") + "' and Sms.Tarih<'"+sontarih.DateTime.ToString("yyyy-MM-dd 23:59")+"'";
            sql += " order by Sms.Tarih desc";
            gridControl3.DataSource = DB.GetData(sql);
        }
        
        void SmsSablonlari()
        {
            lookUpEdit1.Properties.DataSource = DB.GetData("select * from SmsSablon with(nolock)");
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (memoEdit1.Text == "")
            {
                MessageBox.Show("Mesaj Boş Olamaz");
                return;
            }

            if (gridView2.DataRowCount == 0)
            {
                MessageBox.Show("Müşteri Ekleyiniz");
                return;
            }

            if (Degerler.OzelSifre != "")
            {
                inputForm giris = new inputForm();
                giris.Girilen.PasswordChar = '#';
                giris.ShowDialog();

                if (giris.Girilen.Text != Degerler.OzelSifre) return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Liste Gönderilecektir. Eminmisiniz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                 DataRow dr =gridView2.GetDataRow(i);

                //if (dr["Sec"].ToString() != "True") continue;

                string Cep = dr["CepTel"].ToString();
                string musteriadi = dr["Firmaadi"].ToString();
                double b = 0;
                double.TryParse(dr["Devir"].ToString(), out b);

                Cep = Cep.Replace(" ","");

                string mesaj = memoEdit1.Text;

                mesaj = mesaj.Replace("ş", "s");
                mesaj = mesaj.Replace("ü", "u");
                mesaj = mesaj.Replace("ğ", "g");
                mesaj = mesaj.Replace("ö", "o");
                mesaj = mesaj.Replace("ı", "i");
                mesaj = mesaj.Replace("ç", "c");
                mesaj = mesaj.Replace("â", "a");

                mesaj = mesaj.Replace("Ş", "S");
                mesaj = mesaj.Replace("Ü", "U");
                mesaj = mesaj.Replace("Ğ", "G");
                mesaj = mesaj.Replace("Ö", "O");
                mesaj = mesaj.Replace("I", "i");
                mesaj = mesaj.Replace("Ç", "C");
                mesaj = mesaj.Replace("İ", "i");

                mesaj = mesaj.Replace("{isim}", musteriadi);
                mesaj = mesaj.Replace("{Tutar}", b.ToString("##0.00"));

                smsgonder_oztek(mesaj, Cep, Degerler.smsbaslik, dr["pkSms"].ToString(), Degerler.smskullaniciadi, Degerler.smssifre, Degerler.smsKullaniciNo);
            }
            memoEdit1.Text = "";
            Gonderilecekler();

            xtraTabControl1.SelectedTabPageIndex = 2;
            //button2_Click(sender, e);//onayla
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string pkFirma = "0";
            frmMusteriAraSiparis MusteriAra = new frmMusteriAraSiparis();
            MusteriAra.ShowDialog();
            if (MusteriAra.Tag.ToString() == "0") return;
            for (int i = 0; i < MusteriAra.gridView1.DataRowCount; i++)
            {
                DataRow dr = MusteriAra.gridView1.GetDataRow(i);
                
                if (dr["Sec"].ToString() != "True") continue;

                pkFirma = dr["pkFirma"].ToString();
                if (DB.GetData("select * from Sms with(nolock) where Durumu=0 and fkFirma=" + pkFirma).Rows.Count == 0)
                {
                    //string musteriadi = dr["Firmaadi"].ToString();

                    string cep = dr["Cep"].ToString().Replace(" ", "");
                    string cep2 = dr["Cep2"].ToString().Replace(" ", "");
                    string sms_no = cep;

                    

                    if (cep.Length < 10)
                    {
                        sms_no = cep2;
                        //continue;//05444394179 --11 veya 10
                    }
                    if (sms_no.Length < 10)
                    {
                        continue;//05444394179 --11 veya 10
                    }

                    ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", pkFirma));
                    list.Add(new SqlParameter("@CepTel", sms_no));

                    string mesaj = memoEdit1.Text;

                    //mesaj = mesaj.Replace("{isim}", musteriadi);
                    
                    
                    mesaj = mesaj.Replace("ş", "s");
                    mesaj = mesaj.Replace("ü", "u");
                    mesaj = mesaj.Replace("ğ", "g");
                    mesaj = mesaj.Replace("ö", "o");
                    mesaj = mesaj.Replace("ı", "i");
                    mesaj = mesaj.Replace("ç", "c");
                    mesaj = mesaj.Replace("â", "a");

                    mesaj = mesaj.Replace("Ş", "S");
                    mesaj = mesaj.Replace("Ü", "U");
                    mesaj = mesaj.Replace("Ğ", "G");
                    mesaj = mesaj.Replace("Ö", "O");
                    mesaj = mesaj.Replace("I", "i");
                    mesaj = mesaj.Replace("Ç", "C");
                    mesaj = mesaj.Replace("İ", "i");

                    

                    list.Add(new SqlParameter("@Mesaj", mesaj));
                    DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CepTel,0,@Mesaj,GetDate())", list);
                }
            }
            Gonderilecekler();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage3)
                Gonderilecekler();
            if (e.Page == xtraTabPage1)
                OnayBekleyenler();
            if (e.Page == xtraTabPage4)
                Gonderilenler2();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilen Müşteri(ler) Çıkartılsın mı?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("DELETE FROM Sms where pkSms=" + dr["pkSms"].ToString());
                }
            }
            Gonderilecekler();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();
            Gonderilecekler();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //AsistanWS.AsistanWS s = new AsistanWS.AsistanWS();
            //kontür bilgisi
            //System.Xml.XmlNode sonuc = s.CheckUserAccount(textBox1.Text, textBox2.Text, textBox3.Text);
            //string str = sonuc.InnerText;
            //string[] dizi = str.Split('.');
            //string gmesaj = "", id = "0";
            //for (int d = 0; d < dizi.Length; d++)
            //{
            //    gmesaj = dizi[0];
            //    id = dizi[1];
            //}
            //textBox4.Text = id;
            //MessageBox.Show(sonuc.InnerText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;
            
            if (gridView1.FocusedRowHandle < 0) return;
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(gridView1.DataRowCount.ToString() +
                " Sms Gönderilecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
            //AsistanWS.AsistanWS s = new AsistanWS.AsistanWS();
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    string TakipNo = dr["TakipNo"].ToString();
                     //System.Xml.XmlNode sonuc = s.ConfirmSmsTransaction(textBox1.Text, textBox2.Text, textBox3.Text, TakipNo);

                     //if (sonuc.InnerText == "27Alınan parametrelerden biri veya birkaçı hatalı.")
                     //{
                     //    MessageBox.Show(sonuc.InnerText);
                     //    return;
                     //}
                     //MessageBox.Show(sonuc.InnerText);
                     DB.ExecuteSQL("UPDATE Sms SET Durumu=3,Mesaj='Onaylandı',DurumMesaji='Onaylandı' where pkSms=" + dr["pkSms"].ToString());
                }
            }
            OnayBekleyenler();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Seçilenler Silinecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;


            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                if (dr["Sec"].ToString() == "True")
                {
                    DB.ExecuteSQL("DELETE FROM Sms where pkSms=" + dr["pkSms"].ToString());
                }
            }
            OnayBekleyenler();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            //sms
            list.Add(new SqlParameter("@sms_kullaniciadi", tbKullaniciAdi.Text));
            string sifrele = DB.EncodeTo64(tbSmsSifre.Text);
            list.Add(new SqlParameter("@sms_sifre", sifrele));
            list.Add(new SqlParameter("@sms_baslik", tbSmsBaslik.Text));
            list.Add(new SqlParameter("@KullaniciNo", kno.Text));

            string sql = @"UPDATE Sirketler set sms_kullaniciadi=@sms_kullaniciadi,sms_sifre=@sms_sifre,sms_baslik=@sms_baslik,KullaniciNo=@KullaniciNo";
            string s = DB.ExecuteSQL(sql,list);

            if (s == "0")
            {
                Degerler.smskullaniciadi = tbKullaniciAdi.Text;
                Degerler.smssifre = sifrele;
                Degerler.smsbaslik=tbSmsBaslik.Text;
                Degerler.smsKullaniciNo = kno.Text;
            }
        }

        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            speicindekimiktar.Value = 160 - memoEdit1.EditValue.ToString().Length;
        }

        private void müşteriKayıtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            if (dr["pkFirma"] == System.DBNull.Value) return;
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());

            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");

            KurumKarti.ShowDialog();

            Gonderilecekler();
        }

        private void tbSmsBaslik_EditValueChanged(object sender, EventArgs e)
        {
            labelControl1.Text = tbSmsBaslik.Text.Length.ToString() + "Max(11)";
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (memoEdit1.Text == "")
            {
                formislemleri.Mesajform("Mesaj Boş Olamaz", "K", 200);
                memoEdit1.Focus();
                return;
            }
            if (txtCariAdi.Text == "")
            {
                formislemleri.Mesajform("Adı Soyadı Boş Olamaz", "K", 200);
                txtCariAdi.Focus();
                return;
            }
            if (txtCepTel.Text == "" || txtCepTel.Text.Length<10)
            {
                formislemleri.Mesajform("Cep Telefon Numarasını kontrol ediniz", "K", 200);
                txtCepTel.Focus();
                return;
            }
            if (DB.GetData("select CepTel from Sms where Durumu=0 AND CepTel='" + txtCepTel.Text + "'").Rows.Count > 0)
            {
                formislemleri.Mesajform("Daha Önce Eklendi", "K", 200);
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkFirma", "0"));
            list.Add(new SqlParameter("@CariAdi", txtCariAdi.Text));
            list.Add(new SqlParameter("@CepTel", txtCepTel.Text));

            string mesaj =memoEdit1.Text;
            mesaj = mesaj.Replace("ş", "s");
            mesaj = mesaj.Replace("ü", "u");
            mesaj = mesaj.Replace("ğ", "g");
            mesaj = mesaj.Replace("ö", "o");
            mesaj = mesaj.Replace("ı", "i");
            mesaj = mesaj.Replace("ç", "c");
            mesaj = mesaj.Replace("â", "a");

            mesaj = mesaj.Replace("Ş", "S");
            mesaj = mesaj.Replace("Ü", "U");
            mesaj = mesaj.Replace("Ğ", "G");
            mesaj = mesaj.Replace("Ö", "O");
            mesaj = mesaj.Replace("I", "i");
            mesaj = mesaj.Replace("Ç", "C");
            mesaj = mesaj.Replace("İ", "i");

            list.Add(new SqlParameter("@Mesaj", mesaj));
            DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CariAdi,CepTel,Durumu,Mesaj,Tarih) values(@fkFirma,@CariAdi,@CepTel,0,@Mesaj,GetDate())", list);
            
            txtCariAdi.Text = "";
            txtCepTel.Text = "";
            txtCariAdi.Focus();
            
            Gonderilecekler();

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string TakipNo = dr["TakipNo"].ToString();
            string pkSms= dr["pkSms"].ToString();
            string sonuc = sms_sorgula(TakipNo, pkSms);

            formislemleri.Mesajform(sonuc, "M", 200);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    httsmsgonderws.sms_gonder_ws servis = new httsmsgonderws.sms_gonder_ws();
                

            //    httsmsgonderws.AuthenticationHeader guvenlik = new httsmsgonderws.AuthenticationHeader();
            //    guvenlik.KullaniciAdi = "admin";
            //    guvenlik.Sifre = "123456";
            //    servis.AuthenticationHeaderValue = guvenlik;
            //    string sonuc =servis.smsgonder(txtCariAdi.Text, txtCepTel.Text,memoEdit1.Text);
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void tümünüSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.ExecuteSQL("delete from Sms where Durumu=0");
            Gonderilecekler();
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            memoEdit1.Text = lookUpEdit1.GetColumnValue("sablon").ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string mesaj = memoEdit1.Text;
            string coz = DB.DecodeFrom64(Degerler.smssifre);

            mesaj = mesaj.Replace("ş", "s");
            mesaj = mesaj.Replace("ü", "u");
            mesaj = mesaj.Replace("ğ", "g");
            mesaj = mesaj.Replace("ö", "o");
            mesaj = mesaj.Replace("ı", "i");
            mesaj = mesaj.Replace("ç", "c");
            mesaj = mesaj.Replace("â", "a");

            mesaj = mesaj.Replace("Ş", "S");
            mesaj = mesaj.Replace("Ü", "U");
            mesaj = mesaj.Replace("Ğ", "G");
            mesaj = mesaj.Replace("Ö", "O");
            mesaj = mesaj.Replace("I", "i");
            mesaj = mesaj.Replace("Ç", "C");
            mesaj = mesaj.Replace("İ", "i");

            //xml içerisinde aşağıdaki gibi değerleri gönderebilirsiniz..
            //<zaman>2014-04-17 08:30:00</zaman>//sms gitmeye başlama zamanı 
            //<zamanasimi>2014-04-17 10:30:00</zamanasimi>//son gönderim deneme zamanı 
            string tur = "Normal";

            //if (turkce.Checked == true) 
                tur = "Turkce";

                string sms1N = "data=<sms><kno>" + kno.Text + "</kno><kulad>" + tbKullaniciAdi.Text + "</kulad><sifre>" + tbSmsSifre.Text + "</sifre>" +
                "<gonderen>" + tbSmsBaslik.Text + "</gonderen>" +
                "<mesaj>" + mesaj + "</mesaj>" +
                "<numaralar>" + txtCepTel.Text + "</numaralar>" +
                "<tur>" + tur + "</tur></sms>";

                string sonuc = XmlPost("http://www.ozteksms.com/panel/smsgonder1Npost.php", sms1N);
                //1:7297493:Gonderildi:1:0.039:110
                string _id1 = "", _id2 = "", _id3 = "", TakipNo="";
                if (sonuc.Length > 0)
                {
                    _id1 = sonuc.Split(':')[0].ToString();//1.başarılı
                    TakipNo = sonuc.Split(':')[1].ToString();//sorgu id
                    _id2 = sonuc.Split(':')[2].ToString();//gönderildi.
                    _id3 = sonuc.Split(':')[3].ToString();//1 ?
                    _id3 = sonuc.Split(':')[4].ToString();//giden kuruş
                    _id3 = sonuc.Split(':')[5].ToString();//kalan tl

                        ArrayList list = new ArrayList();
                    list.Add(new SqlParameter("@fkFirma", "0"));
                    list.Add(new SqlParameter("@CepTel", txtCepTel.Text));
                    list.Add(new SqlParameter("@Mesaj", mesaj));
                    list.Add(new SqlParameter("@TakipNo", TakipNo));
                    list.Add(new SqlParameter("@DurumMesaji", sonuc));

                    DB.ExecuteSQL("INSERT INTO Sms (fkFirma,CepTel,Durumu,Mesaj,Tarih,TakipNo,DurumMesaji,GondermeZamani)" +
                        " values(@fkFirma,@CepTel,0,@Mesaj,GetDate(),@TakipNo,@DurumMesaji,getdate())", list);
                }
                MessageBox.Show(sonuc);
        }

        public string XmlPost(string PostAddress, string xmlData)
        {
            using (WebClient wUpload = new WebClient())
            {
                wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
        }

        private string XmlPostSorgu(string PostAddress, string xmlData)
        {
            using (WebClient wUpload = new WebClient())
            {
                wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string smsRapor = "data=<smsrapor><kulad>"
                + tbKullaniciAdi.Text + "</kulad><sifre>"
                + tbSmsSifre.Text + "</sifre>" +
            "<ozelkod>" + textBox4.Text + "</ozelkod></smsrapor>";

            string sonuc = XmlPost("http://www.ozteksms.com/panel/smstakippost.php", smsRapor);
            MessageBox.Show(sonuc);
        }

        void Kalan_Sms()
        {
            try
            {
                string gelenXML = sms_sorgula_oztek(tbKullaniciAdi.Text, tbSmsSifre.Text);
                if (gelenXML == "ERR")
                {
                    lbKalan.Text = "İnternet Bağlantınızı Kontrol Ediniz";
                    return;
                }
                gelenXML = gelenXML.Replace("<br>", "|").Replace("=", ":");
                string[] s = gelenXML.Split('|');
                listBox1.Items.Clear();

                foreach (string item in s)
                {
                    listBox1.Items.Add(item);
                }
                if (s.Length==1)
                    lbKalan.Text = s[0].ToString();
                else
                    lbKalan.Text = s[9].ToString();
            }
            catch (Exception exp)
            {
                lbKalan.Text = "Hata : "+exp.Message;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Kalan_Sms();
            /*
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(gelenXML);

            string sonucKodu = ""; string sonucMesaji = "";

            XmlNodeList xnList = xd.GetElementsByTagName("Cari Kodu");
            if (xnList.Count > 0)
                sonucKodu = xnList[0].Attributes["value"].InnerText;

            XmlNodeList xnList1 = xd.GetElementsByTagName("Cari Adı");
            if (xnList1.Count > 0)
                sonucKodu = xnList1[0].Attributes["value"].InnerText;
            

            XmlNodeList xnList2 = xd.GetElementsByTagName("SMS Birim Fiyatı");
            if (xnList2.Count > 0)
                sonucMesaji = xnList2[0].Attributes["value"].InnerText;

            XmlNodeList xnList3 = xd.GetElementsByTagName("Kalan Bakiye");
            if (xnList3.Count > 0)
                sonucMesaji = xnList3[0].Attributes["value"].InnerText;
            */
           // return;

           // string smsRapor="?kul_ad=" + tbKullaniciAdi.Text + "&sifre="+tbSmsSifre.Text;

           ////http://www.ozteksms.com/panel/kullanicibilgi.php?kul_ad=905314648046&sifre=338B9E

           // //cevap.Text = XmlPost("http://www.ozteksms.com/panel/smstakippost.php", smsRapor);
           // string sonuc = XmlPostSorgu("http://www.ozteksms.com/panel/smstakippost.php", smsRapor);

           // MessageBox.Show(sonuc);
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void btnwww_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.ozteksms.com");
        }

        private void btnSablonlar_Click(object sender, EventArgs e)
        {
            frmSmsSablon SmsSablon = new frmSmsSablon();
            SmsSablon.ShowDialog();

            SmsSablonlari();
        }

        private void cbTumu_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTumu.Checked == true)
            {
                panelControl3.Height = 50;
                txtCariAdi.Focus();
                //panelControl5.Visible = true;
            }
            else
                panelControl3.Height = 0;
                //panelControl5.Visible = false;
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void seçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;

            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            DB.ExecuteSQL("DELETE FROM Sms where pkSms=" + dr["pkSms"].ToString());
            gridView2.DeleteSelectedRows();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                gridView2.SetRowCellValue(i,"Sec", checkEdit1.Checked);

                //DataRow dr = gridView2.GetDataRow(i);
                //if (dr["Sec"].ToString() == "True")
                //{
                //    DB.ExecuteSQL("DELETE FROM Sms where pkSms=" + dr["pkSms"].ToString());
                //}
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında...");
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //müşteri no ve başlık alma
            string s = sms_baslikvemusterino_oztek(tbKullaniciAdi.Text, tbSmsSifre.Text);
            kno.Text = s;
        }

        private void labelControl40_Click(object sender, EventArgs e)
        {
            if (Degerler.OzelSifre != "")
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();
                if (sifregir.Girilen.Text != Degerler.OzelSifre) return;
            }
            MessageBox.Show(tbSmsSifre.Text);
        }

        private void excelGönderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridView2.ExportToXls(Application.StartupPath + "SmsGonder.Xls");
            Process.Start(Application.StartupPath + "SmsGonder.Xls"); 
        }

        private void excelGönderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridView3.ExportToXls(Application.StartupPath + "SmsGonderilen.Xls");
            Process.Start(Application.StartupPath + "SmsGonderilen.Xls");
        }

        private void excelGönderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            gridView1.ExportToXls(Application.StartupPath + "SmsOnayBekleyen.Xls");
            Process.Start(Application.StartupPath + "SmsOnayBekleyen.Xls");
        }

        private void cbTarihAraligi_SelectedIndexChanged(object sender, EventArgs e)
        {
            ilktarih.Properties.EditMask = "D";
            sontarih.Properties.EditMask = "D";
            ilktarih.Properties.DisplayFormat.FormatString = "D";
            ilktarih.Properties.EditFormat.FormatString = "D";
            sontarih.Properties.DisplayFormat.FormatString = "D";
            sontarih.Properties.EditFormat.FormatString = "D";


            DateTime haftabasi = getStartOfWeek(false);
            if (cbTarihAraligi.SelectedIndex == 0)// son 30 gün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-30, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            if (cbTarihAraligi.SelectedIndex == 1)// dün
            {
                ilktarih.Properties.DisplayFormat.FormatString = "f";
                sontarih.Properties.EditFormat.FormatString = "f";
                ilktarih.Properties.EditFormat.FormatString = "f";
                sontarih.Properties.DisplayFormat.FormatString = "f";
                ilktarih.Properties.EditMask = "f";
                sontarih.Properties.EditMask = "f";

                sorguTarihAraligi(-1, 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  -1, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 2)// geçen Hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days - 7), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                   (-ti.Days - 1), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);


            }
            else if (cbTarihAraligi.SelectedIndex == 3)// Bu hafta
            {
                TimeSpan ti = DateTime.Now - haftabasi;
                sorguTarihAraligi((-ti.Days), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                    0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 4)// Bu ay
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), 0, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                 0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);
            }
            else if (cbTarihAraligi.SelectedIndex == 5)// önceki ay
            {

                sorguTarihAraligi((-DateTime.Now.Day), -1, 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                 (-DateTime.Now.Day), 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);

            }
            else if (cbTarihAraligi.SelectedIndex == 6)// bu yıl
            {
                sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, (-DateTime.Now.Hour), (-DateTime.Now.Minute), (-DateTime.Now.Second), false,
                                  0, 0, 0, (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second), false);




            }
            //else if (cbTarihAraligi.SelectedIndex ==6) // Bu Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false,
            //                      0, 0, 0, 0, 0, 0, false);

            //}
            //else if (cbTarihAraligi.SelectedIndex == 7) // Geçen Yıl
            //{

            //    sorguTarihAraligi((-DateTime.Now.Day + 1), (-DateTime.Now.Month + 1), -1, 0, 0, 0, false,
            //                    (-DateTime.Now.Day), (-DateTime.Now.Month + 1), 0, 0, 0, 0, false);

            //}
            else if (cbTarihAraligi.SelectedIndex == 6)
            {
                sontarih.Enabled = true;
                ilktarih.Enabled = true;
            }
        }

        private DateTime getStartOfWeek(bool useSunday)
        {
            DateTime now = DateTime.Now;
            int dayOfWeek = (int)now.DayOfWeek;

            if (!useSunday)
                dayOfWeek--;

            if (dayOfWeek < 0)
            {// day of week is Sunday and we want to use Monday as the start of the week
                // Sunday is now the seventh day of the week
                dayOfWeek = 6;
            }

            return now.AddDays(-1 * (double)dayOfWeek);
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

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Gonderilenler2();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;

            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);
            string fkFirma = dr["pkFirma"].ToString();

            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = fkFirma;
            CariHareketMusteri.Show();
            
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            FisYazdir(false);
        }

        void FisYazdir(bool Disigner)
        {
            //string sql = "exec sp_MusterilereGenelBakis";
            try
            {
                System.Data.DataSet ds = new DataSet("Test");
                DataTable dtliste = (DataTable)gridControl3.DataSource;
                if (gridView3.Columns["Firmaadi"].FilterInfo.Value != null)
                {
                    string aranan = gridView3.Columns["Firmaadi"].FilterInfo.Value.ToString();
                    dtliste = dtliste.Select("Firmaadi like'%" + aranan + "%'").CopyToDataTable();
                }
                dtliste.TableName = "dtliste";
                ds.Tables.Add(dtliste);
                //şirket bilgileri
                DataTable Sirket = DB.GetData(@"select * from Sirketler with(nolock)");
                Sirket.TableName = "Sirket";
                ds.Tables.Add(Sirket);

                string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

                string RaporDosyasi = exedizini + "\\Raporlar\\MusteriSmsListesi.repx";
                if (!File.Exists(RaporDosyasi))
                {
                    MessageBox.Show("MusterismsListesi.repx Dosya Bulunamadı");
                    return;
                }
                DevExpress.XtraReports.UI.XtraReport rapor = new DevExpress.XtraReports.UI.XtraReport();


                rapor.LoadLayout(RaporDosyasi);

                rapor.Name = "MusteriSmsListesi";

                rapor.Report.Name = "MusteriSmsListesi";

                rapor.DataSource = ds;

                if (Disigner)
                    rapor.ShowDesignerDialog();
                else
                    rapor.ShowPreviewDialog();
                //ds.Dispose();
                ds.Tables.Remove(dtliste);
                ds.Tables.Remove(Sirket);


                dtliste.Dispose();
                ds.Dispose();
                rapor.Dispose();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Hata Oluştu " + exp.Message);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            FisYazdir(true);
        }
        //private string XmlPost(string PostAddress, string xmlData)
        //{
        //    using (WebClient wUpload = new WebClient())
        //    {
        //        wUpload.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        //        Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
        //        Byte[] bResponse = wUpload.UploadData(PostAddress, "POST", bPostArray);
        //        Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
        //        string sWebPage = new string(sReturnChars);
        //        return sWebPage;
        //    }
        //}
    }
}
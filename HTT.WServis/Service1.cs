using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;

namespace HTT.WServis
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer1 = new System.Timers.Timer();
        double timer1Interval = 1000;
        string timerInterval= "300000";//5dk
        public Service1()
        {
            InitializeComponent();

            //ProcessQueue();  //test etmek için 
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
        }

        protected override void OnStart(string[] args)
        {
            logayaz("Servis Başlatıldı.");
            timer1.Interval = timer1Interval;
            timer1.Start();
        }

        protected override void OnStop()
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                ProcessQueue();
            }
            catch (Exception ex)
            {
                logayaz("1.timer " + ex.Message);
            }
            timer1Interval = 300000;//5 dk bir 
            double.TryParse(timerInterval, out timer1Interval);
            timer1.Interval = timer1Interval;//5dk  10.12.2013 14:08:50 5dk(300.000) sonra 10.12.2013 14:13:51
            timer1.Start();
        }

        private void ProcessQueue()
        {
            //"00:00"
            //string ENabizGonderSaatBas = ConfigurationSettings.AppSettings["ENabizGonderSaatBas"].ToString();
            //"23:59"
            //string ENabizGonderSaatBitis = ConfigurationSettings.AppSettings["ENabizGonderSaatBitis"].ToString();

            string sonuc = "";
            //DateTime now = DateTime.Now;
            //DateTime zaman = new DateTime(2018, 9, 28, now.Hour, now.Minute, now.Second);

            //her 10 dakikada bir skrsden sorgular
            //servis 10 dk bir çalıştırılması sağlandı.
            //if (int.Parse(now.ToString("mm")) % 10 == 0)
            //{
            //    Thread thread1 = new Thread(new ThreadStart(tolayafet));
            //    thread1.Start();
            //}

            //if ((zaman > Convert.ToDateTime(zaman.ToString("yyyy-MM-dd " + ENabizGonderSaatBas)) &&
            //    (zaman < Convert.ToDateTime(zaman.ToString("yyyy-MM-dd 23:59:59")))) ||
            //    (zaman > Convert.ToDateTime(zaman.ToString("yyyy-MM-dd 00:00:00"))) &&
            //    (zaman < Convert.ToDateTime(zaman.ToString("yyyy-MM-dd " + ENabizGonderSaatBitis))))
            //{
            try
            {
                //sonuc = TeleTipGonderim();
                //HatirlatmaSorgulaGonder();
                logayaz(sonuc);
            }
            catch (Exception exp)
            {
                logayaz(sonuc + "exp:" + exp.Message);
            }
            //}
        }

        //private void HatirlatmaSorgulaGonder()
        //{
        //    try
        //    {
        //        NoteManager noteManager = new NoteManager();
        //        string mesaj = "";
        //        Note note = new Note();
        //        //HTT.Entities eklemince hata veriyor
        //        var liste = noteManager.List(x => x.ModifiedOn > DateTime.Now);
        //        if (liste.Count > 0)
        //        {
        //            mesaj = liste[0].Text;
        //            //logayaz(mesaj);
        //            epostagonder("gurbuzadem@gmail.com", mesaj, "", "Hatırlatma");
        //            logayaz("Hatırlatma Gönderildi");

        //            note = liste[0];
        //            note.ModifiedOn = DateTime.Now;
        //            noteManager.Update(note);
        //        }
        //        else
        //            logayaz("Hatırlatma Yok Gönderildi");
        //    }
        //    catch (Exception exp)
        //    {

        //        logayaz("Sorgularken Hata Oluştu: "+exp.Message);
        //        logayaz("DB : " + exp.InnerException);
        //        //throw;
        //    }
            

        //    //return "Hatırlatma Gönderildi";
        //}


        public static string epostagonder(string kime, string mesaj, string dosya, string Subject)
        {
            if (mesaj == "") return "Lütfen Mesaj Giriniz";

            string sirket = "Hayat Defteri",
                Host = "mail.hitityazilim.com",
                GonderenEposta = "destek@hitityazilim.com",
                GonderenEpostaSifre = "TEKsql@3653", Port = "587", ccEposta = "", bccEposta = "";

            //DataTable dt = DB.GetData("select top 1 * from Sirketler with(nolock)");
            //if (dt.Rows.Count > 0)
            //{
            //    sirket = dt.Rows[0]["Sirket"].ToString();
            //    Host = dt.Rows[0]["Host"].ToString();
            //    GonderenEposta = dt.Rows[0]["GonderenEposta"].ToString();
            //    //string epostasifresi = islemler.CryptoStreamSifreleme.Decrypt("Hitit999", dt.Rows[0]["GonderenEpostaSifre"].ToString());
            //    string epostasifresi = islemler.CryptoStreamSifreleme.md5SifreyiCoz(dt.Rows[0]["GonderenEpostaSifre"].ToString());
            //    GonderenEpostaSifre = epostasifresi;//dt.Rows[0]["GonderenEpostaSifre"].ToString();
            //    Port = dt.Rows[0]["Port"].ToString();

            //    ccEposta = dt.Rows[0]["ccEposta"].ToString();
            //    bccEposta = dt.Rows[0]["bccEposta"].ToString();
            //}

            try
            {
                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Port = int.Parse(Port);   //Smtp Portu (Sunucuya Göre Değişir)25
                smtpclient.Host = Host;//"smtp.gmail.com";  ;Smtp Hostu (Gmail smtp adresi bu şekilde)
                smtpclient.EnableSsl = false;//Degerler.EnableSsl;   //Sunucunun SSL kullanıp kullanmadıgı
                smtpclient.Credentials = new NetworkCredential(GonderenEposta, GonderenEpostaSifre);

                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("gurbuzadem@gmail.com", "www.hitityazilim.com"); //Gidecek Mail Adresi ve Görünüm Adınız
                mail.From = new MailAddress(GonderenEposta, Host.Replace("mail", "www")); //Gidecek Mail Adresi ve Görünüm Adınız

                if (ccEposta.Length > 5)
                    mail.CC.Add(ccEposta);//bilgi
                if (bccEposta.Length > 5)
                    mail.Bcc.Add(bccEposta);//gizli

                mail.To.Add(kime); //Kime Göndereceğiniz
                                   //mail.To.Add("info@hitityazilim.com");

                if (Subject == "") Subject = mesaj.Substring(0, 5) + "...";
                mail.Subject = Subject;// "Kayıt İşlemi";    //Emailin Konusu
                mail.Body = sirket + "<br>" + mesaj;
                mail.IsBodyHtml = true;           //Mesajınızın Gövdesinde HTML destegininin olup olmadıgı
                if (dosya != "")
                {
                    Attachment data = new Attachment(dosya, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(dosya);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(dosya);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(dosya);
                    // Add the file attachment to this e-mail message.
                    mail.Attachments.Add(data);
                }
                smtpclient.Send(mail);
                mail.Dispose();
                //smtpclient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                return "OK";
            }
            catch (Exception exp)
            {
                logayaz("e-posta : " + exp.Message);
                return "Hata Oluştu Lütfen E-Posta Bilgileri Kontrol Ediniz!" + exp.Message.ToString();
                //showmessage("Hata Oluştu Lütfen E-Posta Bilgileri Kontrol Ediniz!" + exp.Message.ToString());
            }
        }

        public static bool logayaz(string logmetin)
        {
            try
            {
                string HDLogDosyaYolu = @"C:\\hayatdefteriservis";

                if (!Directory.Exists(HDLogDosyaYolu))
                    Directory.CreateDirectory(HDLogDosyaYolu);


                string dosyaadi = HDLogDosyaYolu + "\\WindowsServis" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                string silinecek_dosyaadi = HDLogDosyaYolu + "\\WindowsServis" + DateTime.Now.AddDays(-7).ToString("yyyyMMdd") + ".log";

                StreamWriter sw = new StreamWriter(dosyaadi, true);
                sw.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " - " + logmetin);
                sw.Close();

                if (File.Exists(silinecek_dosyaadi))
                    File.Delete(silinecek_dosyaadi);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

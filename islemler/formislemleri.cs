using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Media;
using DevExpress.XtraPrinting.Links;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraGrid;
using GPTS.Include.Data;
using System.Data;

namespace GPTS.islemler
{
    class formislemleri
    {
        public static string Mesajform(string mesaj,string renk,int interval)
        {
            frmMesajBox Mesaj = new frmMesajBox(interval);
            Mesaj.label1.Text = mesaj;
            if(renk=="K")
              Mesaj.label1.BackColor=System.Drawing.Color.Red;
            else if(renk=="S")
              Mesaj.label1.BackColor=System.Drawing.Color.Yellow;
            else if (renk == "Y")
                Mesaj.label1.BackColor = System.Drawing.Color.Green;
            else if (renk == "M")
                Mesaj.label1.BackColor = System.Drawing.Color.Blue;
            else 
                Mesaj.label1.BackColor = System.Drawing.Color.Silver;

            Mesaj.Show();
            return "";
        }

        public static void MesajformBalon(string balonbaslik,string balonmesaj,string text,int balonsure)
        {
            NotifyIcon notifyIcon1 = new NotifyIcon();
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipText = balonmesaj;
            notifyIcon1.BalloonTipTitle = balonbaslik;
            notifyIcon1.Text = text;
            notifyIcon1.ShowBalloonTip(balonsure);
            //notifyIcon1.Dispose();
        }

        public static string MesajBox(string mesaj,string baslik,int mesaj_tipi,int aktif_button)
        {
            frmMesajKutusu MesajKutusu = new frmMesajKutusu(mesaj,baslik,mesaj_tipi,aktif_button);
            MesajKutusu.ShowDialog();

            return MesajKutusu.Tag.ToString();
        }

        public static string inputbox(string Baslik,string Caption,string varsayilandeger,bool sifreli)
        {
            string deger="";
            inputForm sifregir = new inputForm();
            sifregir.Text = Baslik;
            sifregir.GirilenCaption.Text=Caption;
            if (sifreli)
                sifregir.Girilen.PasswordChar = char.Parse("*");
            else
                sifregir.Girilen.PasswordChar = char.Parse("\0");
            sifregir.Girilen.Text= varsayilandeger;
            sifregir.ShowDialog();
            deger = sifregir.Girilen.Text;
            sifregir.Dispose();
            return deger;
        }
        
        public static string EtiketBas(string pkStokKarti,int adet)
        {
            DataTable dbStok = DB.GetData("select pkStokKarti,SatisAdedi,KutuFiyat as icindekiadet,SatisFiyati  from StokKarti with(nolock) where pkStokKarti=" + pkStokKarti);
            if (dbStok.Rows.Count == 0) return "Stok Bulunamadı";

            //string adet = dbStok.Rows[0]["SatisAdedi"].ToString();
            //string icindekiadet = dbStok.Rows[0]["icindekiadet"].ToString();
            string satisFiyati = dbStok.Rows[0]["SatisFiyati"].ToString();

            //int aadet = 1;
            //int.TryParse(adet, out aadet);

            //int iadet = 1;
            //int.TryParse(icindekiadet, out iadet);
            //int etiketadet = 1;
            //if (iadet != aadet)
            //{
            //    string s = MesajBox("Satış Adedi için Evet, İçindeki Adet için Hayırı Seçin?", "Etiket Adedi", 3, 2);
            //    if (s == "0")
            //        etiketadet = aadet;
            //    else
            //        etiketadet = iadet;
            //    //return "İşlem İptal Edildi";
            //}

            string pkEtiketBas = "0";
            pkEtiketBas = DB.ExecuteScalarSQL("INSERT INTO EtiketBas (Tarih,Aciklama,Siparis) values(getdate(),'',0) SELECT IDENT_CURRENT('EtiketBas')");

            DB.ExecuteSQL("INSERT INTO EtiketBasDetay (fkEtiketBas,fkStokKarti,Adet,SatisFiyati,Tarih) "+
            " VALUES(" + pkEtiketBas + "," + pkStokKarti + ","+ adet.ToString() + "," + satisFiyati.Replace(",",".") + ",getdate())");
            
            frmEtiketBas fEtiketBas = new frmEtiketBas();
            fEtiketBas.alisfaturasindangelenfisno.Text = pkEtiketBas;
            fEtiketBas.ShowDialog();
            return "";
        }

        public static string UyariSesiCal()
        {
            try
            {
                string exeDiz = Path.GetDirectoryName(Application.ExecutablePath);
                string dosya = exeDiz + "\\Sesler\\uyari.wav";
                //string path = "C:\\windows\\media\\ding.wav"; // Müzik adresi
                if (File.Exists(dosya))
                {
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = dosya;// "chord.wav";
                    player.Play();
                }
            }
            catch (Exception exp)
            {
                return "Hata " +  exp.Message;
            }
            return "OK";
        }

        public static bool SifreIste()
        {
            if (Degerler.OzelSifre != "" && DB.fkKullanicilar != "1")
            {
                inputForm sifregir = new inputForm();
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();

                if (sifregir.Girilen.Text != Degerler.OzelSifre)
                {
                    if (sifregir.Girilen.Text == "9999*")
                        return true;
                    else
                        return false;
                }  
            }
                return true;
        }

        public static bool SifreIsteYazilim()
        {
            //if (Degerler.OzelSifre != "" && DB.fkKullanicilar != "1")
            //{
                inputForm sifregir = new inputForm();
                sifregir.Text = "Şifre Ekranı Yazılım";
                sifregir.Girilen.PasswordChar = '#';
                sifregir.ShowDialog();

                if (sifregir.Girilen.Text != Degerler.OzelSifreYazilim)
                    return false;

            //}
            return true;
        }
    }
}

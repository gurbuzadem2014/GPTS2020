using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

public class Fonksiyonlar
{

   

    public void LogYazTxt(string islemAdi, string stackTrace, string hataMesaji)
    {
        //if (hataMesaji.Length > 200) hataMesaji = hataMesaji.Substring(0, 200);

        string log = "Tarih-Saat: " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
        log += "İşlem Adı: " + islemAdi + Environment.NewLine;
        log += "Stack Trace: " + stackTrace + Environment.NewLine;
        log += "Hata Mesajı: " + hataMesaji + Environment.NewLine;
        log += "________________________________________________________________________________" + Environment.NewLine;
        try
        {
            TextWriter tw = new StreamWriter(this.DosyaAdi(), true);
            tw.WriteLine(log);
            tw.Close();
        }
        catch (Exception ex)
        {
            string hata = ex.Message;
        }
    }

    private string DosyaAdi()
    {
        string logYolu = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "\\Log\\";
        string dosyaAdi = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
        return logYolu + dosyaAdi + ".txt";
    }

    public static string Replace(string deger, List<string> eski, List<string> yeni)
    {
        if (eski.Count == yeni.Count)
        {
            for (int i = 0; i < eski.Count; i++)
                deger = deger.Replace(eski[i], yeni[i]);
        }

        return deger;
    }

    public static string TirnakTemizle(string metin)
    {
        return Replace(metin, new List<string>() { "'", ";", "--" }, new List<string>() { "\"", ",", "" });
    }

    public static string IPAdresBul()
    {
        string ipAdres = "";

        try
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            IPAddress[] ipAdresleri = ipEntry.AddressList;

            if (ipAdresleri.Length > 0)
                ipAdres = ipAdresleri[0].ToString();
        }
        catch (Exception)
        {
        }

        return ipAdres;
    }

}


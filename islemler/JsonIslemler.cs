using GPTS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GPTS.islemler
{
    public static class JsonIslemler
    {
        //Tekli Yazma
        public static bool JsonYaz(Bilgiler bilgi)
        {
            string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bool sonuc = false;
            try
            {
                using (FileStream fs = File.Open(masaustuYolu + "\\deneme.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, bilgi);
                }
                sonuc = true;
            }
            catch { }
            return sonuc;
        }

        //ÇokluYazma
        public static bool JsonYaz(List<Bilgiler> bilgiler)
        {
            bool sonuc = false;
            string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                using (FileStream fs = File.Open(masaustuYolu + "\\deneme.json", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, bilgiler);
                }
                sonuc = true;
            }
            catch { }
            return sonuc;
        }

        //Tekli Okuma
        public static Bilgiler JsonOku(string masaustuYolu)
        {
            //string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Bilgiler sonuc = new Bilgiler();
            try
            {
                using (StreamReader r = new StreamReader(masaustuYolu + "\\deneme.json"))
                {
                    string json = r.ReadToEnd();
                    sonuc = JsonConvert.DeserializeObject<Bilgiler>(json);
                }
            }
            catch (Exception exp)
            {
                formislemleri.Mesajform("Json Hata:" + exp.Message, "K", 200);
            }
            return sonuc;
        }
        //Çoklu Okuma
        public static List<Bilgiler> JsonOkuList()
        {
            string masaustuYolu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            List<Bilgiler> sonuc = new List<Bilgiler>();
            try
            {
                using (StreamReader r = new StreamReader(masaustuYolu + "\\deneme.json"))
                {
                    string json = r.ReadToEnd();
                    sonuc = JsonConvert.DeserializeObject<List<Bilgiler>>(json);
                }
            }
            catch { }
            return sonuc;
        }

    }
}

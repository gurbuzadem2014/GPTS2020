using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GPTS.islemler
{
    //public static string 
    class DBislemleri
    {
        public static string MusteriPasifYap(string pkFirma)
        {
            //if (DB.GetData("select fkFirma from Satislar where fkFirma=" + pkFirma).Rows.Count == 0)
            //{
            //    return "Satış Yapıldığı için Müşteri Silinemez!";
            //}
            int sonuc=  
            DB.ExecuteSQL("update Firmalar set Aktif=0 where pkFirma="+ pkFirma );
            if(sonuc==1)
              return "İşlem Başarılı.";
            else
              return "Hata Oluştu.";
        }

        public static string TedarikciyiPasifYap(string pkTedarikciler)
        {
            //if (DB.GetData("select fkFirma from Satislar where fkFirma=" + pkFirma).Rows.Count == 0)
            //{
            //    return "Satış Yapıldığı için Müşteri Silinemez!";
            //}
            int sonuc =
            DB.ExecuteSQL("update Tedarikciler set Aktif=0 where pkTedarikciler=" + pkTedarikciler);
            if (sonuc == 1)
                return "İşlem Başarılı.";
            else
                return "Hata Oluştu.";
        }

        public static DataTable ilgetir()
        {
            DataTable dt = DB.GetData(@"SELECT [ADI] ,[KODU] ,[GRUP],[ALTGRUP] FROM IL_ILCE_MAH with(nolock)
                                         WHERE GRUP=1  ORDER BY KODU");
            return dt;
        }

        public static DataTable ilcelerigetir(string ilid)
        {
            DataTable dt = DB.GetData(@"SELECT [ADI] ,[KODU] ,[GRUP],[ALTGRUP] FROM IL_ILCE_MAH with(nolock)
                                         WHERE ALTGRUP=" + ilid + "  ORDER BY KODU");
            return dt;
        }

        public static string YeniSatisEkle()
        {
            string yenifisno = "0";
            return yenifisno;
        }

        public static string YeniHatirlatmaEkle(string konu,DateTime bastar,string aciklama,DateTime bittar,string fkfirma,string fisno,string fkCek,string pkTaksitler)
        {
            if (bastar < DateTime.Now)
                bastar = DateTime.Now;

            if (bastar.ToString("HH") == "00")
            {
                bastar = bastar.AddMinutes(12);
                bittar = bittar.AddMinutes(13);
            }
                string sql = "HSP_HatirlatmaAnimsatEkle '"+konu+"','"+bastar.ToString("yyyy-MM-dd HH:mm")
                + "','"+aciklama+"',1,'" + bittar.ToString("yyyy-MM-dd HH:mm") + "',1,1,"+ fkfirma+","+ fisno+","+ fkCek+","+ pkTaksitler;

            //string yenifisno = "0";
            int sonuc = DB.ExecuteSQL_Sonuc_Sifir(sql);

            return sonuc.ToString();
        }

        public static bool StokHareketEkle()
        {
            return true;
        }
    }
   
    
}

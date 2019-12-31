using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GPTS.islemler
{
    class Kullanici_islemleri
    {
        public static DataTable KullanicilariGetir(int fkKullanicilar)
        {
            string sql = @"select * from Kullanicilar with(nolock) where pkKullanicilar=" + fkKullanicilar;

            return DB.GetData(sql);
        }
    }
}

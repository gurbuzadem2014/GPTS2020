using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GPTS.islemler
{
    class yetkiler
    {
        public static DataTable ModulYetkileri(int fkKullanicilar)
        {
            string sql = @"select m.Kod,m.ModulAdi,my.Yetki from Moduller m with(nolock) 
            left join ModullerYetki  my with(nolock) on my.Kod=m.Kod
            where fkKullanicilar=" + fkKullanicilar;

            return DB.GetData(sql);
        }

        public static bool ModulYetkileri(int fkKullanicilar,string Kod)
        {
            string sql = @"select m.ModulAdi,my.Yetki from Moduller m with(nolock) 
            left join ModullerYetki  my with(nolock) on my.Kod=m.Kod
            where fkKullanicilar=" + fkKullanicilar + " and m.Kod='" + Kod+"'";

            DataTable dt = DB.GetData(sql);

            if (dt.Rows.Count == 0)
                return false;
            else 
            {
                if (dt.Rows[0]["Yetki"].ToString() == "0")
                    return false;
                else
                    return true;
                }

            
        }
    }
}

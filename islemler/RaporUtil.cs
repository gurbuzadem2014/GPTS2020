using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace GPTS.islemler
{
    public class RaporUtil
    {
        public static MemoryStream GetMemStr(string fkSatisFisiSecimi, string modul_kod)
        {
            MemoryStream ms = null;
            DataTable dt =
            DB.GetData("select * from SatisFisiSecimi with(nolock) where pkSatisFisiSecimi=" + fkSatisFisiSecimi);// +
            //" or modul_kod='" + modul_kod + "'");

            if (dt.Rows.Count == 0) return null;

            try
            {
                byte[] resim = (byte[])dt.Rows[0]["rapor_dosya"];
                ms = new MemoryStream(resim, 0, resim.Length);
                ms.Write(resim, 0, resim.Length);
            }
            catch (Exception exp)
            {
                DB.logayaz(exp.Message, "Lab_Raporlar");
                //MessageBox.Show(exp.Message);
            }

            return ms;
        }
    }
}

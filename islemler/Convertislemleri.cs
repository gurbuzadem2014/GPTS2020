using System;
using System.Collections.Generic;
using System.Text;

namespace GPTS.islemler
{
    public class Convertislemleri
    {
        #region "Kod Oluştur"
        public static string TurkceKodOlustur(string Text)
        {
            string strReturn = Text.Trim();
            try
            {
                strReturn = strReturn.Replace("ğ", "g");
                strReturn = strReturn.Replace("Ğ", "G");
                strReturn = strReturn.Replace("ü", "u");
                strReturn = strReturn.Replace("Ü", "U");
                strReturn = strReturn.Replace("ş", "s");
                strReturn = strReturn.Replace("Ş", "S");
                strReturn = strReturn.Replace("ı", "i");
                strReturn = strReturn.Replace("İ", "I");
                strReturn = strReturn.Replace("ö", "o");
                strReturn = strReturn.Replace("Ö", "O");
                strReturn = strReturn.Replace("ç", "c");
                strReturn = strReturn.Replace("Ç", "C");
                strReturn = strReturn.Replace("-", "+");
                strReturn = strReturn.Replace(" ", "+");
                strReturn = strReturn.Trim();
                // strReturn = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9+]").Replace(strReturn, "");
                strReturn = strReturn.Trim();
                strReturn = strReturn.Replace("+", "-");
                return strReturn;
            }
            catch (Exception ex)
            {
                //throw ex;
                return ex.Message;
            }
            return strReturn;
        }

        #endregion
    }

    
}

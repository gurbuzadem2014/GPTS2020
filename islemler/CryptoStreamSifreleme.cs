using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GPTS.islemler
{
    class CryptoStreamSifreleme
    {
        public static string Encrypt2(string key, string originalString)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(key);

            if (String.IsNullOrEmpty(originalString)) return string.Empty;

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);

            writer.Write(originalString);

            writer.Flush();

            cryptoStream.FlushFinalBlock();

            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

        }

        public static string Decrypt2(string key, string cryptedString)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(key);

            if (String.IsNullOrEmpty(cryptedString)) return string.Empty;

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();

            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));

            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        private static string hash = "www.hitityazilim.com";
        public static string  md5Sifrele(string txtsifresiz)
        {
            string sifrele = "";
            byte[] data = UTF8Encoding.UTF8.GetBytes(txtsifresiz);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    sifrele = Convert.ToBase64String(results, 0, results.Length);
                }
            }
            return sifrele;
        }
        public static string md5SifreyiCoz(string txtsifreli)
        {
            if (txtsifreli == "") return "";

            string txtcozulmus = "";

            try
            {
                byte[] data = Convert.FromBase64String(txtsifreli);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        txtcozulmus = UTF8Encoding.UTF8.GetString(results);
                    }
                }
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
           

            return txtcozulmus;
        }
    }
}

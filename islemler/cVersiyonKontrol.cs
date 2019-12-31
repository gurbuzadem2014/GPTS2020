using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GPTS.islemler
{
    public class cVersiyonKontrol
    {
        /// <summary>
        /// Update Check Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string versiyonkontrol(string version)
        {
            //char[] ayrac = { '.' };
            //string[] parcalar = version.Split(ayrac);
            //string vol = "";
            //int i = 0;
            //foreach (string item in parcalar)
            //{
            //    if (i == 0)
            //        vol = vol + item+".";
            //    if (i == 1)
            //        vol = vol + item;
            //    i++;
            //}
            //version = vol;
            // cerate ne web client for web request
            string result = "OK";
            MyWebClient webclient = new MyWebClient();
            try
            {
                // open and read web page
                Stream stream = webclient.OpenRead("http://www.hitityazilim.com/guncelleme/check.php?version=" + version);
                // stream content
                StreamReader reader = new StreamReader(stream);
                // get result
                result = reader.ReadToEnd();

                result = result.Trim();
            }
            catch (Exception)
            {
                result = "RETURN";
                //throw;
            }

            return result;
        }
    }
}

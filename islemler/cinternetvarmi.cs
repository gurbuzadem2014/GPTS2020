using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPTS.islemler
{
    
    
    public class cinternetvarmi
    {
        public static bool ivarmi = false;
        public cinternetvarmi()
        {
            //    ivarmi = false;
            //    CallMethod();
            //Asyivarmi();

        }
        public bool Asyivarmi()
        {
            try
            {
                Uri url = new Uri("http://www.google.com");

                var client = new WebClient();
                client.DownloadStringCompleted += (sender, e) =>
                {
                    string pageSourceCode = e.Result;
                    //do something with results 
                };

                client.DownloadStringAsync(url);

                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
           
        }
        //static async void CallMethod()
        //{
        //    await InternetVarmi2();
        //    //InternetVarmi2();
        //}

        public bool InternetVarmi2()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingDurumu = ping.Send("www.google.com");//IPAddress.Parse("64.15.112.45"));

                if (pingDurumu.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    ivarmi = true;
                    return true;
                    //Console.WriteLine("İnternet bağlantısı var");
                }
                else
                {
                    ivarmi = false;
                    return false;
                    //Console.WriteLine("İnternet bağlantısı yok");
                }
                //Console.ReadKey();
                //return true;
            }
            catch
            {
                ivarmi = false;
                return false;
            }
        }

    }
}

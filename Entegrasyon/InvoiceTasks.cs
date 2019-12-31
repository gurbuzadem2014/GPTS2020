using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GPTS.EFaturaBasic;
using GPTS.Properties;
//using GPTS.Properties;

namespace GPTS.Entegrasyon
{
    public class InvoiceTasks
    {
        public static InvoiceTasks Instance = new InvoiceTasks();

        private InvoiceTasks()
        {

        }

        public BasicIntegrationClient CreateClient()
        {
            //var username = "Uyumsoft";
            //var password = "Uyumsoft";
            var serviceuri = Include.Data.Degerler.e_fatura_url;
                //"http://efatura-test.uyumsoft.com.tr/Services/BasicIntegration";


            //if (Settings.Default.UseTestEnvironment)
            //{
            //    username = Settings1.Default.WebServiceTestUsername;
            //    password = Settings1.Default.WebServiceTestPassword;
            //    serviceuri = Settings1.Default.WebServiceTestUri;
            //}
            //if (!Settings1.Default.UseTestEnvironment)
            //{
            //    username = Settings1.Default.WebServiceLiveUsername;
            //    password = Settings1.Default.WebServiceLivePassword;
            //    serviceuri = Settings1.Default.WebServiceLiveUri;
            //}

            var client = new BasicIntegrationClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceuri);
            UserInformation userInfo = new UserInformation();
            client.ClientCredentials.UserName.UserName = userInfo.Username;//username;
            client.ClientCredentials.UserName.Password = userInfo.Password;// password;
            return client;
        }

        public UserInformation GetUserInfo()
        {
            var username = Include.Data.Degerler.e_fatura_kul;
            var password = Include.Data.Degerler.e_fatura_sifre;

            UserInformation userInfo = new UserInformation();

            //DataTable dt = DB.GetData("select * from sirketler with(nolock)");
            //if (dt.Rows.Count > 0)
            //{
            //    username = dt.Rows[0]["e_fatura_kul"].ToString();
            //    password = dt.Rows[0]["e_fatura_sifre"].ToString();
            //}

            //if (Settings1.Default.UseTestEnvironment)
            //{
                userInfo.Username = username;//Settings1.Default.WebServiceTestUsername;
                userInfo.Password = password;//Settings1.Default.WebServiceTestPassword;

            //}
            //if (!Settings1.Default.UseTestEnvironment)
            //{
            //    userInfo.Username = Settings1.Default.WebServiceLiveUsername;
            //    userInfo.Password = Settings1.Default.WebServiceLivePassword;
            //}
            return userInfo;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GPTS.islemler
{
    public class MyWebClient : WebClient
    {
        //public class MyWebClient
        //{

        //}
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 2000;//20 * 60 * 1000;
            return w;
        }
    }
}

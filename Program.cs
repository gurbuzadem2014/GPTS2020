using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GPTS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.CurrentCulture =
            new System.Globalization.CultureInfo("tr-TR");

            // The following line provides localization for the application's user interface. 
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");

            Application.Run(new frmAnaForm());
            //Application.Run(new frmUyumSoftEfatura());
        }
    }
}
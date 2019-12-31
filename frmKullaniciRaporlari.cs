using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GPTS
{
    public partial class frmKullaniciRaporlari : DevExpress.XtraEditors.XtraForm
    {
        public frmKullaniciRaporlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }

        private void frmKullaniciRaporlari_Load(object sender, EventArgs e)
        {
            string sql = @"SELECT YA.pkYetkiAlanlari, YA.fkKullanicilar, M.ModulAdi, P.Aciklama10 AS YetkiKodu, P.Aciklama50 AS YetkiAdi, YA.Yetki AS YetkiDurumu, YA.Sayi AS Deger, 
                      dbo.Kullanicilar.KullaniciAdi
FROM         dbo.YetkiAlanlari AS YA WITH (nolock) LEFT OUTER JOIN
                      dbo.Kullanicilar ON YA.fkKullanicilar = dbo.Kullanicilar.pkKullanicilar LEFT OUTER JOIN
                      dbo.Parametreler AS P WITH (nolock) ON P.pkParametreler = YA.fkParametreler LEFT OUTER JOIN
                      dbo.Moduller AS M WITH (nolock) ON M.pkModuller = P.fkModul";
            gridControl5.DataSource=  DB.GetData(sql);

            gridView6.ExpandAllGroups();
        }
    }
}
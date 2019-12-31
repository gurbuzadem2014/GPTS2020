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
    public partial class frmUcGoster : DevExpress.XtraEditors.XtraForm
    {
        int form_id = 0;
        string _fisno = "0";
        public frmUcGoster(int Formid,string fisno)
        {
            InitializeComponent();
            form_id = Formid;
            _fisno = fisno;
        }

        private void frmUcGoster_Load(object sender, EventArgs e)
        {
            if (form_id == 0)
            {
                try
                {
                    WebBrowser webBrowser1 = new WebBrowser();
                    webBrowser1.Visible = true;
                    //webBrowser1.IsWebBrowserContextMenuEnabled = false;
                    //webBrowser1.AllowNavigation = false;
                    //webBrowser1.AllowWebBrowserDrop = false;
                    //webBrowser1.ContextMenuStrip = contextMenuStrip1;
                    webBrowser1.Dock = DockStyle.Fill;
                    // if (File.Exists(@"K:\hititprof2\bin\Debug\index.html"))
                    webBrowser1.Navigate(DB.exeDizini + "\\yardim.htm");
                    //Process.Start(DB.exeDizini + "\\yardim.htm");
                    this.Controls.Add(webBrowser1);
                }
                catch
                {
                    //WebBrowser webBrowser1 = new WebBrowser();
                    //webBrowser1.Visible = true;
                    ////webBrowser1.IsWebBrowserContextMenuEnabled = false;
                    ////webBrowser1.AllowNavigation = false;
                    ////webBrowser1.AllowWebBrowserDrop = false;
                    ////webBrowser1.ContextMenuStrip = contextMenuStrip1;
                    //webBrowser1.Dock = DockStyle.Fill;
                    //// if (File.Exists(@"K:\hititprof2\bin\Debug\index.html"))
                    //webBrowser1.Navigate("http://www.hitityazilim.com/yardim.htm");

                    System.Diagnostics.Process.Start("http://www.hitityazilim.com/yardim.htm");

                    //this.Controls.Add(webBrowser1);
                }
                return;
            }

            if ((int)GPTS.Include.Data.FormAdlari.SatisFormu == form_id)
            {
                ucSatis Satis = new ucSatis(_fisno);
                Satis.Dock = DockStyle.Fill;
                this.Controls.Add(Satis);
                Satis.Dock = DockStyle.Fill;
                Satis.BringToFront();
            }
            else if ((int)GPTS.Include.Data.FormAdlari.SenetFormu == form_id)
            {
                ucSenetListesi Satis = new ucSenetListesi();
                Satis.Tag = musteriadi.Text;
                Satis.Dock = DockStyle.Fill;
                this.Controls.Add(Satis);
                Satis.Dock = DockStyle.Fill;
                Satis.BringToFront();
            }
            else if ((int)GPTS.Include.Data.FormAdlari.CekFormu == form_id)
            {
                ucCekListesi Satis = new ucCekListesi();
                Satis.Dock = DockStyle.Fill;
                this.Controls.Add(Satis);
                Satis.Dock = DockStyle.Fill;
                Satis.BringToFront();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
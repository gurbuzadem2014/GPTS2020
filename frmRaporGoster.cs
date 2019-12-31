using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GPTS.islemler;
using System.IO;

namespace GPTS
{
    public partial class frmRaporGoster : Form
    {
        public xrCariHareket _xrCariHareket = null;
        public xrCariHareket txrCariHareket
        {
            get
            {
                if (_xrCariHareket == null)
                    _xrCariHareket = new xrCariHareket();
                return _xrCariHareket;
            }
            set { _xrCariHareket = value; }
        }

        public frmRaporGoster()
        {
            InitializeComponent();
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            string RaporDosyasi = exedizini + "\\Raporlar\\PersonelKarti.repx";
            //xrCariHareket xr = new xrCariHareket();

            // XtraReport1 xr = new XtraReport1();
            txrCariHareket.LoadLayout(RaporDosyasi);
            //documentViewer1.DocumentSource = xr;
            printControl1.PrintingSystem = txrCariHareket.PrintingSystem;
            txrCariHareket.CreateDocument();// false);
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
          
            
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }
       
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);

            string RaporDosyasi = exedizini + "\\Raporlar\\PersonelKarti.repx";
            //xrCariHareket xr = new xrCariHareket();
            txrCariHareket.LoadLayout(RaporDosyasi);
            printControl1.PrintingSystem = txrCariHareket.PrintingSystem;
            txrCariHareket.ShowDesignerDialog();
            //reportDesigner1.OpenReport(xr);
        }

       
    }
}

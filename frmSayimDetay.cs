using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.LookAndFeel;
using DevExpress.XtraPrinting.Links;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmSayimDetay : DevExpress.XtraEditors.XtraForm
    {
        public frmSayimDetay()
        {
            InitializeComponent();
        }

        private void frmSayimDetay_Load(object sender, EventArgs e)
        {
            string sql ="";
           // if (cbSayilanlar.Checked)        
                sql= @"SELECT ssd.pkStokSayimDetay, ss.pkStokSayim, ssd.MevcutMiktar, ssd.SayimSonuMiktari, sk.Barcode, 
                      sk.Stokadi, sag.StokAltGrup, sg.StokGrup, sk.Mevcut, 
                      sk.AlisFiyati, s.pkSayim,s.fkDepolar,sk.pkStokKarti,sk.Mevcut
                      FROM Sayim s with(nolock)
                      INNER JOIN StokSayim ss with(nolock) ON s.pkSayim = ss.fkSayim 
                      INNER JOIN StokSayimDetay ssd with(nolock) ON ss.pkStokSayim = ssd.fkStokSayim 
                      INNER JOIN StokKarti sk with(nolock) ON ssd.fkStokKarti = sk.pkStokKarti 
                      LEFT JOIN StokGruplari sg with(nolock) ON sk.fkStokGrup = sg.pkStokGrup 
                      LEFT JOIN StokAltGruplari sag ON sk.fkStokAltGruplari = sag.pkStokAltGruplari
                      WHERE ss.pkStokSayim =" + this.Tag.ToString();
            gridControl1.DataSource = DB.GetData(sql);
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void ShowRibbonPreviewDialog(LinkBase link)
        {
            InitPrintTool(new LinkPrintTool(link));
        }
        public virtual void InitPrintTool(PrintTool tool)
        {
            tool.ShowRibbonPreviewDialog(UserLookAndFeel.Default);
        }
        PrintingSystem printingSystem = null;
        PrintingSystem Printing
        {
            get
            {
                if (printingSystem == null) printingSystem = new PrintingSystem();
                return printingSystem;
            }
        }
        void yazdir()
        {
            PrintableComponentLink printableLink = new PrintableComponentLink(new PrintingSystem());
            printableLink.Component = gridControl1;
            printableLink.Landscape = true;
            printableLink.CreateDocument(Printing);
            ShowRibbonPreviewDialog(printableLink);
            printableLink.Dispose();
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            yazdir();
        }

        private void cbSayilanlar_CheckedChanged(object sender, EventArgs e)
        {
            frmSayimDetay_Load(sender, e);
        }

        private void depoMevcutlarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //string pkSayim= dr["pkSayim"].ToString();
            string fkStokKarti = dr["pkStokKarti"].ToString();

            frmStokKartiDepoMevcut StokKartiDepoMevcut = new frmStokKartiDepoMevcut(int.Parse(fkStokKarti));
            StokKartiDepoMevcut.ShowDialog();
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;
           
            DataRow dr = gridView1.GetDataRow(i);
            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void depoMevcutlarıGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Depo Mevcutları ve Stok kartları sayimsonuMiktari olarak güncellensin mi?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No)
            {
                return;
            }

            DataRow dr = gridView1.GetDataRow(i);


            DB.ExecuteSQL(@"update stokkartidepo set MevcutAdet = stoksayimdetay.sayimsonuMiktari,
            stokkartidepo.ToplamGiren = MevcutAdet from stoksayimdetay
            where stokkartidepo.fkDepolar = "+ dr["fkDepolar"].ToString() + " and fkStokSayim = "+ dr["pkStokSayim"].ToString()
            + " and stokkartidepo.fkStokKarti = stoksayimdetay.fkStokKarti");

            //stok kartınıda güncelle
            DB.ExecuteSQL(@"update StokKarti set Mevcut = MevcutAdet from
                  (select fkStokKarti, sum(MevcutAdet) as MevcutAdet from StokKartiDepo
                  group by fkStokKarti) ds
                 where StokKarti.pkStokKarti = ds.fkStokKarti");
        }
    }
}
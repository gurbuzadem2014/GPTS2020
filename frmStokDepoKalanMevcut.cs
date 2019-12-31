using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmStokDepoKalanMevcut : DevExpress.XtraEditors.XtraForm
    {
        public frmStokDepoKalanMevcut()
        {
            InitializeComponent();
        }

        void StokKartiDepo()
        {

            string sql = @"Select sk.pkStokKarti,sk.Stokadi,sk.Barcode,sk.Mevcut as ToplamMevcut,deposube.sube_adi,deposube.DepoAdi,skd.MevcutAdet as DepoMevcut from StokKarti sk 
left join StokKartiDepo skd on skd.fkStokKarti=sk.pkStokKarti
left join (
select d.pkDepolar,DepoAdi,S.pkSube,S.sube_adi from Depolar d 
left join Subeler s  on s.pkSube=d.fkSube) deposube on deposube.pkDepolar=skd.fkDepolar";


                pivotGridControl2.DataSource = DB.GetData(sql);
            
        }

        private void frmStokKartiDepo_Load(object sender, EventArgs e)
        {
            gridControl3.DataSource = DB.GetData("HSP_DepoMevcutlari");

            //StokKartiDepo();
        }


        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView1.DataRowCount == 0) return;

            //int i = gridView1.FocusedRowHandle;
            //if (i < 0)
            //{
            //    i = gridView1.DataRowCount;
            //    i--;
            //}
            //DataRow dr = gridView1.GetDataRow(i);

            //frmStokKarti StokKarti = new frmStokKarti();

            //DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            //StokKarti.ShowDialog();
        }

        private void devirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (gridView1.FocusedRowHandle < 0) return;
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            //StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            //StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            //StokHareketleri.ShowDialog();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında  StokKartiDepo");
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                //yesilisikyeni();
                return;
            }
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\SokDepoMevcut.Xls";
            pivotGridControl2.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\SokDepoMevcutListe.Xls";
            gridControl3.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\SokDepoMevcutPivod.Xls";
            pivotGridControl2.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridView3.DataRowCount == 0) return;

            int i = gridView3.FocusedRowHandle;

            //if (i < 0)
            //{
            //    i = gridView3.DataRowCount;
            //    i--;
            //}

            DataRow dr = gridView3.GetDataRow(i);

            //if (gridView1.DataRowCount == 0) return;

            //int i = gridView1.FocusedRowHandle;
            //if (i < 0)
            //{
            //    i = gridView1.DataRowCount;
            //    i--;
            //}
            //DataRow dr = gridView1.GetDataRow(i);

            frmStokKarti StokKarti = new frmStokKarti();

            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridView3.FocusedRowHandle < 0) return;
            DataRow dr = gridView3.GetDataRow(gridView3.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["pkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }
    }
}
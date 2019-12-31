using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace GPTS
{
    public partial class frmKodver : DevExpress.XtraEditors.XtraForm
    {
        public frmKodver()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            gridControl2.DataSource = DB.GetData("select *,'Kod ver' as Kodversec from Kodver");
        }
        private void frmKodver_Load(object sender, EventArgs e)
        {
            gridyukle();
        }
        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            BtnKaydet.Enabled = true;
            BtnKaydet.Text = "Düzenle";
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            pkKodver.Text = dr["pkKodver"].ToString();
            sRakam.EditValue = dr["Rakam"].ToString();


            sbtnSil.Visible = true;
            sbtnSil.Enabled = true;
            GridHitInfo ghi = gridView2.CalcHitInfo(e.Location);
            if (ghi.Column == null) return;
            if (ghi.Column.Name == "gridColumn5")
            {
                btnKodver();
                Close();
            }
            
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            frmStokKoduverKarti StokKoduverKarti = new frmStokKoduverKarti();
            StokKoduverKarti.pkKodver.Text = pkKodver.Text;
            StokKoduverKarti.ShowDialog();
            gridyukle();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmStokKoduverKarti StokKoduverKarti = new frmStokKoduverKarti();
            StokKoduverKarti.ShowDialog();
            gridyukle();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            kodal.Text = "";
            Close();
        }

        void btnKodver()
        {
            
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            pkKodver.Text = dr["pkKodver"].ToString();
            //tkod.Text = ;
            //sRakam.EditValue = dr["Rakam"].ToString();
            //tAciklama.Text = dr["Aciklama"].ToString();
            kodal.Text = dr["Kodu"].ToString() + dr["Rakam"].ToString();
        }
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            btnKodver();
            Close();
        }

        private void aktifSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.ExecuteSQL("Update Kodver Set AktifSec=0");
            DB.ExecuteSQL("Update Kodver Set AktifSec=1 where pkKodver="+  dr["pkKodver"].ToString());
            gridyukle();
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Aciklama"].ToString() + "\n Kartı Silmek İstediğinize Eminmisiniz.", "Hitit 2012", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

                DB.ExecuteSQL("DELETE FROM Kodver where pkKodver=" + dr["pkKodver"].ToString());
            gridyukle();
        }

        private void frmKodver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void repositoryItemHyperLinkEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
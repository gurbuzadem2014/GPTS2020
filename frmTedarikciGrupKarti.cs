using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmTedarikciGrupKarti : Form
    {
        public frmTedarikciGrupKarti()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            gridControl1.DataSource = DB.GetData("Select * from TedarikcilerGruplari");
        }
        private void frmStokGrupKarti_Load(object sender, EventArgs e)
        {
            gridyukle();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(e.RowHandle);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkTedarikcilerGruplari = dr["pkTedarikcilerGruplari"].ToString();
            if (DB.GetData("select pkTedarikciler from Tedarikciler where fkFirmaGruplari=" + pkTedarikcilerGruplari).Rows.Count > 0)
            {
                MessageBox.Show("Bu Gruba ait Tedarikçi Grubu Bulunmaktadır.Lütfen Önce Tedarikçi Grubunu değiştiriniz.");
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["GrupAdi"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("DELETE FROM TedarikcilerGruplari WHERE pkTedarikcilerGruplari=" + dr["pkTedarikcilerGruplari"].ToString());
            gridView1.DeleteSelectedRows();
            //gridyukle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kAYDETToolStripMenuItem_Click(sender,e);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            yeniToolStripMenuItem_Click(sender, e);
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            gridColumn2.OptionsColumn.AllowEdit = true;
        }

        void kaydet()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE TedarikcilerGruplari SET GrupAdi='" + dr["GrupAdi"].ToString() + "' WHERE pkTedarikcilerGruplari=" + dr["pkTedarikcilerGruplari"].ToString());
            }
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //önce kaydet
            kaydet();
            frmStokGrubuYeniKarti StokGrubuYeniKarti = new frmStokGrubuYeniKarti();
            StokGrubuYeniKarti.Tag = "3";
            StokGrubuYeniKarti.ShowDialog();
            gridyukle();
        }

        private void kAYDETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kaydet();
            Close();
        }

        private void frmStokGrupKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}

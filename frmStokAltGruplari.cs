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
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmStokAltGruplari : DevExpress.XtraEditors.XtraForm
    {
        public frmStokAltGruplari()
        {
            InitializeComponent();
        }
        void StokGruplariGetir()
        {
            lueStokGruplari.Properties.DataSource = DB.GetData("select * from StokGruplari where Aktif=1");
            lueStokGruplari.EditValue = int.Parse(lueStokGruplari.Tag.ToString());
        }
        void StokAltGruplariGetir()
        {
            gridControl2.DataSource = DB.GetData("select * from StokAltGruplari where fkStokGruplari=" + lueStokGruplari.EditValue.ToString());
        }
        private void frmKodver_Load(object sender, EventArgs e)
        {
            StokGruplariGetir();
        }
        void kaydet()
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("UPDATE StokAltGruplari SET StokAltGrup='" + dr["StokAltGrup"].ToString() + "' WHERE pkStokAltGruplari=" + dr["pkStokAltGruplari"].ToString());
            }
            gridColumn2.OptionsColumn.AllowEdit = false;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kaydet();
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        //void btnKodver()
        //{
        //    kodal.Text = tkod.Text + sRakam.Text;
        //    DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
        //    pkKodver.Text = dr["pkKodver"].ToString();
        //    //tkod.Text = dr["Kodu"].ToString();
        //    //sRakam.EditValue = dr["Rakam"].ToString();
        //    //tAciklama.Text = dr["Aciklama"].ToString();
        //}
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            gridColumn2.OptionsColumn.AllowEdit = true;
        }

        private void aktifSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.ExecuteSQL("Update Kodver Set AktifSec=0");
            DB.ExecuteSQL("Update Kodver Set AktifSec=1 where pkKodver="+  dr["pkKodver"].ToString());
            StokAltGruplariGetir();
        }

        private void lueStokGruplari_EditValueChanged(object sender, EventArgs e)
        {
            StokAltGruplariGetir();
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string fkStokAltGruplari = dr["pkStokAltGruplari"].ToString();
            if (DB.GetData("select pkStokKarti from StokKarti where fkStokAltGruplari=" + fkStokAltGruplari).Rows.Count > 0)
            {
                MessageBox.Show("Bu Alt Gruba ait Stok bulunmaktadır.Lütfen Önce Stok Alt Grubunu değiştiriniz.");
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["StokAltGrup"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("Delete From StokAltGruplari where pkStokAltGruplari=" + fkStokAltGruplari);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Ürün Bilgileri Silindi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridView2.DeleteSelectedRows();
            //StokAltGruplariGetir();
        }
        private void btnYeni_Click(object sender, EventArgs e)
        {
            kaydet();
            frmStokGrubuYeniKarti StokGrubuYeniKarti = new frmStokGrubuYeniKarti();
            StokGrubuYeniKarti.Tag = "2";
            StokGrubuYeniKarti.grupid.Tag = lueStokGruplari.EditValue.ToString();
            StokGrubuYeniKarti.grupid.Text = lueStokGruplari.Text;
            StokGrubuYeniKarti.ShowDialog();
            StokAltGruplariGetir();
        }
    }
}
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
    public partial class frmMusteriAltGruplari : DevExpress.XtraEditors.XtraForm
    {
        public frmMusteriAltGruplari()
        {
            InitializeComponent();
        }
        void StokGruplariGetir()
        {
            lueStokGruplari.Properties.DataSource = DB.GetData("select * from FirmaGruplari where Aktif=1");
            lueStokGruplari.EditValue = int.Parse(lueStokGruplari.Tag.ToString());
        }
        void StokAltGruplariGetir()
        {
            gridControl2.DataSource = DB.GetData("select * from FirmaAltGruplari where fkFirmaGruplari=" + lueStokGruplari.EditValue.ToString());
        }
        private void frmKodver_Load(object sender, EventArgs e)
        {
            StokGruplariGetir();
            StokAltGruplariGetir();
        }
        void kaydet()
        {
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                DataRow dr = gridView2.GetDataRow(i);
                DB.ExecuteSQL("UPDATE FirmaAltGruplari SET FirmaAltGrupAdi='" + dr["FirmaAltGrupAdi"].ToString() + "' WHERE pkFirmaAltGruplari=" + dr["pkFirmaAltGruplari"].ToString());
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
            //DB.ExecuteSQL("UPDATE FirmaAltGruplari SET FirmaAltGrupAdi='" + dr["FirmaAltGrupAdi"].ToString() + "' WHERE pkFirmaAltGruplari=" + dr["pkFirmaAltGruplari"].ToString());


            string pkFirmaAltGruplari = dr["pkFirmaAltGruplari"].ToString();
            if (DB.GetData("select pkFirma from Firmalar where fkFirmaAltGruplari=" + pkFirmaAltGruplari).Rows.Count > 0)
            {
                MessageBox.Show("Bu Alt Gruba ait Müşteri bulunmaktadır.Lütfen Önce Müşteri Alt Grubunu değiştiriniz.");
                return;
            }
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["FirmaAltGrupAdi"].ToString() + " Kartı Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("Delete From FirmaAltGruplari where pkFirmaAltGruplari=" + pkFirmaAltGruplari);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Ürün Bilgileri Silindi.", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridView2.DeleteSelectedRows();
            //StokAltGruplariGetir();
        }
        private void btnYeni_Click(object sender, EventArgs e)
        {
            //kaydet();
            frmMusteriGrupKarti StokGrubuYeniKarti = new frmMusteriGrupKarti();
            StokGrubuYeniKarti.Tag = "2";
            StokGrubuYeniKarti.grupid.Tag = lueStokGruplari.EditValue.ToString();
            StokGrubuYeniKarti.grupid.Text = lueStokGruplari.Text;
            StokGrubuYeniKarti.ShowDialog();
            StokAltGruplariGetir();
        }
    }
}
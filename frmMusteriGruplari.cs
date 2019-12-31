using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmMusteriGruplari : Form
    {
        public frmMusteriGruplari()
        {
            InitializeComponent();
        }
        void gruplarigetir()
        {
            gridControl1.DataSource = DB.GetData("select * from FirmaGruplari");
        }
        private void frmCariGrupKarti_Load(object sender, EventArgs e)
        {
            gruplarigetir();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
          
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            BtnKaydet.Tag = dr["GrupAdi"].ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            frmMusteriGrupKarti StokKoduverKarti = new frmMusteriGrupKarti();
            //StokKoduverKarti.pkFirmaGruplari.Text = "0";
            StokKoduverKarti.ShowDialog();
            gruplarigetir();
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;
            string pkFirmaGruplari = "0";
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            pkFirmaGruplari= dr["pkFirmaGruplari"].ToString();
            DataTable dt =
            DB.GetData("select COUNT(*) as co from Firmalar where fkFirmaGruplari=" + pkFirmaGruplari);

            frmMesajBox mesaj = new frmMesajBox(200);
            if (dt.Rows[0][0].ToString() == "0")
            {
                DB.ExecuteSQL("DELETE FROM FirmaGruplari WHERE pkFirmaGruplari=" + pkFirmaGruplari);
                mesaj.label1.Text = "Grup Silindi";
                gruplarigetir();
            }
            else
            {
                mesaj.label1.Text = "Müşteri Grubu Kullanıldığı için Silemezsiniz";
            }
            mesaj.Show();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            gridColumn2.OptionsColumn.AllowEdit = true;
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
        void kaydet()
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                DB.ExecuteSQL("UPDATE FirmaGruplari SET GrupAdi='" + dr["GrupAdi"].ToString() +
                    "',FirmaTuru='" + dr["FirmaTuru"].ToString() +
                    "' WHERE pkFirmaGruplari=" + dr["pkFirmaGruplari"].ToString());
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kaydet();
            Close();
        }

        private void frmMusteriGruplari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}

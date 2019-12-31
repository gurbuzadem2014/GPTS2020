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
    public partial class frmHizliButtonDuzenle : DevExpress.XtraEditors.XtraForm
    {
        string pkStokKartionceki = "";
        public frmHizliButtonDuzenle()
        {
            InitializeComponent();
        }
        private void barkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton3_Click(sender, e);
            }
            if (e.KeyCode == Keys.F8 || e.KeyCode == Keys.Space)
            {
                simpleButton1_Click(sender, e);
            }
        }

        private void frmHizliButtonDuzenle_Load(object sender, EventArgs e)
        {
            if (oncekibarkod.Text != "")
            {
                DataTable dtonceki = DB.GetData("select pkStokKarti,StokAdi from StokKarti where Barcode='" + oncekibarkod.Text + "'");
                pkStokKartionceki = dtonceki.Rows[0]["pkStokKarti"].ToString();
                stokadi.Text = dtonceki.Rows[0]["StokAdi"].ToString();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DataTable dtSimdiki = DB.GetData("select * from StokKarti where Barcode='" + barkod.Text + "'");
            if (dtSimdiki.Rows.Count == 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Stok Bulunamadı", Degerler.mesajbaslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string pkStokKartiSimdiki = dtSimdiki.Rows[0]["pkStokKarti"].ToString();
            string StokadiSimdiki = dtSimdiki.Rows[0]["Stokadi"].ToString();
            DataTable dtHizliStokSatis = DB.GetData("select * from HizliStokSatis where fkStokKarti=" + pkStokKartiSimdiki);
            if (dtHizliStokSatis.Rows.Count > 0)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bu Stok Zaten Hızlı Butonlar Listesinde Bulunmaktadır", "Hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           // if (dtSimdiki.Rows[0]["HizliSatisAdi"].ToString() == "")
            DB.ExecuteSQL("UPDATE StokKarti Set HizliSatisAdi=Stokadi where pkStokKarti=" + pkStokKartiSimdiki);
            DB.ExecuteSQL("UPDATE HizliStokSatis Set fkStokKarti=" + pkStokKartiSimdiki + " where pkHizliStokSatis=" + oncekibarkod.Tag);
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            if (StokAra.gridView1.FocusedRowHandle >= 0)
            {
                DataRow dr = StokAra.gridView1.GetDataRow(StokAra.gridView1.FocusedRowHandle);
                int pkUrunid = 0;
                int.TryParse(dr["pkStokKarti"].ToString(), out pkUrunid);
                if (pkUrunid == 0) return;
                barkod.Text = dr["Barcode"].ToString();
                barkod.ToolTip = dr["Stokadi"].ToString();
                //barkod.Tag = dr["pkStokadi"].ToString();
                labelControl1.Text = dr["Stokadi"].ToString();
            }
            StokAra.Dispose();
        }

        private void frmHizliButtonDuzenle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            else if (e.KeyCode == Keys.F9)
                simpleButton3_Click(sender,  e);
            else if (e.KeyCode == Keys.F8)
                simpleButton1_Click(sender,  e);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
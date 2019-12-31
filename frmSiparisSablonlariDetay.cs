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
    public partial class frmSiparisSablonlariDetay : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisSablonlariDetay()
        {
            InitializeComponent();
        }

        private void frmSiparisSablonlariDetay_Load(object sender, EventArgs e)
        {
            lueMusteri.Properties.DataSource = DB.GetData("select pkFirma,Firmaadi,OzelKod from Firmalar where Aktif=1");
            lueMusteri.EditValue = int.Parse(pkFirma.Text);
            //gridControl1.DataSource = DB.GetData("select * from SiparisSablonlari where fkFirma=" + lueMusteri.EditValue.ToString());
        }
        void SiparisSablonlari()
        {
               gridControl1.DataSource = DB.GetData("select * from SiparisSablonlari where fkFirma=" + lueMusteri.EditValue.ToString());
        }
        private void lueMusteri_EditValueChanged(object sender, EventArgs e)
        {
            SiparisSablonlari();
            gridControl2.DataSource = null;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            gridControl2.DataSource = DB.GetData(@"SELECT     SiparisSablonDetay.pkSiparisSablonDetay, SiparisSablonDetay.fkSiparisSablonlari, SiparisSablonDetay.fkStokKarti, SiparisSablonDetay.Adet, StokKarti.Barcode, 
                      StokKarti.Stokadi, StokKarti.AlisFiyati, StokKarti.SatisFiyati
FROM         SiparisSablonDetay INNER JOIN
                      StokKarti ON SiparisSablonDetay.fkStokKarti = StokKarti.pkStokKarti
WHERE SiparisSablonDetay.fkSiparisSablonlari =" + dr["pkSiparisSablonlari"].ToString());
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnyazdir_Click_1(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSiparisSablonlari=dr["pkSiparisSablonlari"].ToString();
            DB.ExecuteSQL("DELETE FROM SiparisSablonDetay WHERE fkSiparisSablonlari=" + pkSiparisSablonlari);
            DB.ExecuteSQL("DELETE FROM SiparisSablonlari WHERE pkSiparisSablonlari=" + pkSiparisSablonlari);
            DB.ExecuteSQL("UPDATE MusteriZiyaretGunleri SET fkSiparisSablonlari=NULL WHERE fkSiparisSablonlari=" + pkSiparisSablonlari);
            SiparisSablonlari();
            gridControl2.DataSource = null;
        }
    }
}
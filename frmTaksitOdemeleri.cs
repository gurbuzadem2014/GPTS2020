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
    public partial class frmTaksitOdemeleri : DevExpress.XtraEditors.XtraForm
    {
        public frmTaksitOdemeleri()
        {
            InitializeComponent();
        }
        void TaksitOdemeleri()
        {
            string sql = @"SELECT Taksitler.pkTaksitler, Taksitler.fkFirma, Taksitler.Tarih, Taksitler.Odenecek, Taksitler.Odenen, ISNULL(Taksitler.Odenecek, 0) - ISNULL(Taksitler.Odenen, 0) AS Kalan, 
                      Taksitler.SiraNo, Firmalar.Firmaadi
FROM         Taksitler INNER JOIN
                      Firmalar ON Taksitler.fkFirma = Firmalar.PkFirma";
            gridControl2.DataSource = DB.GetData(sql);
        }
        private void frmTaksitOdemeleri_Load(object sender, EventArgs e)
        {
            TaksitOdemeleri();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["fkFirma"].ToString());
            DB.pkKasaHareket = 0;
            frmKasaGiris_Cikis KasaGirisi = new frmKasaGiris_Cikis();
            KasaGirisi.Tag = "1";
            KasaGirisi.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
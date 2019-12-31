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

namespace GPTS
{
    public partial class frmGrupTanimlari : DevExpress.XtraEditors.XtraForm
    {
        public frmGrupTanimlari()
        {
            InitializeComponent();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@StokFiyatGrup", tEGrupAdi.Text));
            DB.ExecuteSQL("INSERT INTO StokFiyatGruplari (StokFiyatGrup) VALUES(@StokFiyatGrup)",list);
            StokFiyatGruplari();
        }

        void StokFiyatGruplari()
        {
            gridControl1.DataSource = DB.GetData("select * from StokFiyatGruplari");
        }
        private void frmGrupTanimlari_Load(object sender, EventArgs e)
        {
            StokFiyatGruplari();
        }
    }
}
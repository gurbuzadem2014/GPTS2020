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
    public partial class frmStokDepoTalepleri : DevExpress.XtraEditors.XtraForm
    {
        public frmStokDepoTalepleri()
        {
            InitializeComponent();
        }
        void getir()
        {
            gridControl1.DataSource = DB.GetData("select * from StokKarti with(nolock)");
        }
        private void frmAracTakip_Load(object sender, EventArgs e)
        {
            getir();
        }

        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmDestek d = new frmDestek();
            d.ShowDialog();
        }
    }
}
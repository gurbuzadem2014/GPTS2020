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
    public partial class frmSatisFiyatlariBaslik : DevExpress.XtraEditors.XtraForm
    {
        public frmSatisFiyatlariBaslik()
        {
            InitializeComponent();
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Stok Satiş Fiyatları Silinecektir Eminmisiniz!");
            DB.ExecuteSQL("delete from SatisFiyatlariBaslik");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSatisFiyatlariBaslik_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource =
                DB.GetData("select * from SatisFiyatlariBaslik with(nolock)");
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

        }
    }
}
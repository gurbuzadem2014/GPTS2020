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
    public partial class frmSiparisKaydetGuncelle : DevExpress.XtraEditors.XtraForm
    {
        public frmSiparisKaydetGuncelle()
        {
            InitializeComponent();
        }
        private void frmSiparisKaydetGuncelle_Load(object sender, EventArgs e)
        {
            lueSablonGrup.Properties.DataSource = DB.GetData("select * from SablonGrup");
            lueSablonGrup.EditValue = 1;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Tag = "1";
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            Close();
        }

       
    }
}
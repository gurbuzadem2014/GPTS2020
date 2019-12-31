using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmOdaDegis : Form
    {
        public frmOdaDegis()
        {
            InitializeComponent();
        }

        void OdalarGetir()
        {
            lueOdalar.Properties.DataSource = DB.GetData(@"select 0 as pkOda,0 as fkKat,'Tüm Odalar' as oda_adi
            union all
            select pkOda,fkKat,oda_adi from Odalar with(nolock) where aktif = 1");
            lueOdalar.EditValue = 0;
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            OdalarGetir();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            this.Tag = "1";
            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmEpostaGonder : DevExpress.XtraEditors.XtraForm
    {
        public frmEpostaGonder()
        {
            InitializeComponent();
        }
        

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //DB.epostagonder("gurbuzadem@gmail.com", meMesaj.Text, "", "Yazılım");
            DB.epostagonder("destek@hitityazilim.com", meMesaj.Text, "", "Yazılım");

            formislemleri.Mesajform("E-Posta Gönderildi.", "S", 200);
            meMesaj.Text = "";
        }

        private void meMesaj_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
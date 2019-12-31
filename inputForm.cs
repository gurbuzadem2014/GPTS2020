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
    public partial class inputForm : DevExpress.XtraEditors.XtraForm
    {
        public inputForm()
        {
            InitializeComponent();
        }

        private void sifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape) Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            this.Tag = "1";
            Close();
        }

        private void inputForm_Load(object sender, EventArgs e)
        {
            iskontooranlari();
        }

        void iskontooranlari()
        {
            lueKarOranlari.Properties.DataSource = DB.GetData("select * from iskontoOranlari");
        }
        private void lueKarOranlari_EditValueChanged(object sender, EventArgs e)
        {
            //if(lueKarOranlari.ItemIndex==0)  return;
            Girilen.Text = lueKarOranlari.Text;
            Girilen.Focus();
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            if (DB.GetData("select * from iskontoOranlari where iskonto_orani=" + Girilen.Text).Rows.Count == 0)
                DB.ExecuteSQL("insert into iskontoOranlari (iskonto_orani) values(" + Girilen.Text + ")");
            else
                MessageBox.Show("Zaten var");
        }
    }
}
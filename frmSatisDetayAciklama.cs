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
    public partial class frmSatisDetayAciklama : DevExpress.XtraEditors.XtraForm
    {
        private string SatisDetay_id = "0";
        private bool kayit_var_yok = false;

        public frmSatisDetayAciklama(string fkSatisDetay)
        {
            InitializeComponent();
            SatisDetay_id = fkSatisDetay;
            this.Width = 400;
        }

        private void frmFisAciklama_Load(object sender, EventArgs e)
        {
            DataTable dtStok = DB.GetData(@"select Stokadi from SatisDetay sd with(nolock) 
            INNER JOIN StokKarti  sk with(nolock) on sk.pkStokKarti=sd.fkStokKarti where pkSatisDetay=" + SatisDetay_id);
            if (dtStok.Rows.Count > 0)
            {
                this.Text = this.Text+" " + dtStok.Rows[0]["Stokadi"].ToString();
            }

            DataTable dt =
            DB.GetData("select aciklama_detay from SatisDetayAciklama with(nolock) where fkSatisDetay=" + SatisDetay_id);
            if (dt.Rows.Count > 0)
            {
                kayit_var_yok = true;
                memoozelnot.Text = dt.Rows[0]["aciklama_detay"].ToString();
            }
        }

        private void frmFisAciklama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                BtnKaydet_Click(sender, e);
                Close();
            }

            //if (e.KeyCode == Keys.Enter )
            //{
            //    //this.Tag = "1";
            //    BtnKaydet_Click(sender, e);
            //    //Close();
            //}
            if (e.KeyCode == Keys.Escape)
            {
                this.Tag = "0";
                Close();
            }
            if (e.KeyCode == Keys.F5)
            {
              DialogResult secim;
              secim = DevExpress.XtraEditors.XtraMessageBox.Show("Açıklama Bilgisi Temizlenecek Onaylıyormusunuz?", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (secim == DialogResult.No) return;
                memoozelnot.Text = "";
            }
        }

        private void memoozelnot_KeyPress(object sender, KeyPressEventArgs e)
        {
            //labelControl2.Text = memoozelnot.Text.Length.ToString();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (kayit_var_yok)
            {
                DB.ExecuteSQL("update SatisDetayAciklama set  aciklama_detay='" + memoozelnot .Text+ "' where fkSatisDetay=" + SatisDetay_id);
            }
            else
            {
                DB.ExecuteSQL("insert into SatisDetayAciklama (fkSatisDetay,aciklama_detay) values("+SatisDetay_id+",'" + memoozelnot.Text + "')");
            }
            //if (this.Tag.ToString() == "Alis")
            //{
            //    DB.ExecuteSQL("update Alislar set  Aciklama='" + memoozelnot.Text + "' where pkAlislar=" + memoozelnot.Tag.ToString());
            //}
            //this.Tag = "1";//alış tutar kaydet
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            //memoozelnot.EditValue = memoozelnot.OldEditValue;

            this.Tag = "0";//alış tutar kaydetme
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            memoozelnot.EditValue = null;
            memoozelnot.Focus();
        }
    }
}
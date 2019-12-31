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
    public partial class frmFisAciklama : DevExpress.XtraEditors.XtraForm
    {
        public frmFisAciklama()
        {
            InitializeComponent();
            
        }

        private void frmFisAciklama_Load(object sender, EventArgs e)
        {
            if (this.Tag.ToString() == "Satis")
            {
                this.Height = 250;
            }
            else
                this.Height = 460;

            this.Width = 400;

            //ceTutari.SendKey(Keys.F4);

            if (this.Tag.ToString() == "Satis")
            {
                DataTable dt =
                DB.GetData("select Aciklama from Satislar with(nolock) where pksatislar=" + memoozelnot.Tag.ToString());
                if (dt.Rows.Count > 0)
                    memoozelnot.Text = dt.Rows[0]["Aciklama"].ToString();
            }
            if (this.Tag.ToString() == "Alis")
            {
                DataTable dt =
                DB.GetData("select Aciklama from Alislar with(nolock) where pkAlislar=" + memoozelnot.Tag.ToString());
                if (dt.Rows.Count > 0)
                    memoozelnot.Text = dt.Rows[0]["Aciklama"].ToString();
            }
            if (pcTutarHesapla.Visible == true)
            {
                pcTutarHesapla.Left = 0;
                this.Width = pcTutarHesapla.Width;
                pcTutarHesapla.Focus();
                ceTutari.Focus();
                ceTutari.SelectAll();
                ceTutari.Focus();
                //SendKeys.Send("{F4}");
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
            labelControl2.Text = memoozelnot.Text.Length.ToString();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
           
            if (this.Tag.ToString() == "Satis")
            {
                DB.ExecuteSQL("update Satislar set  Aciklama='" + memoozelnot .Text+ "' where pksatislar=" + memoozelnot.Tag.ToString());
            }
            if (this.Tag.ToString() == "Alis")
            {
                DB.ExecuteSQL("update Alislar set  Aciklama='" + memoozelnot.Text + "' where pkAlislar=" + memoozelnot.Tag.ToString());
            }
            this.Tag = "1";//alış tutar kaydet
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

        private void ceTutari_EditValueChanged(object sender, EventArgs e)
        {
            if (seMiktar.Value == 0) return;

            calcEdit1.Value = ceTutari.Value/seMiktar.Value;
        }

        private void ceTutari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet_Click(sender,e);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "1";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "2";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "3";
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "4";
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "5";
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "6";
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "7";
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "8";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "9";
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + ".";
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            ceTutari.Text = ceTutari.Text + "0";
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            ceTutari.Text = "";
        }
    }
}
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
    public partial class frmTarihSec : DevExpress.XtraEditors.XtraForm
    {
        public frmTarihSec()
        {
            InitializeComponent();
            this.Width = 600;
        }

        private void frmFisAciklama_Load(object sender, EventArgs e)
        {
            dateNavigator1.DateTime=DateTime.Today;
            //dtpSaat.Value = DateTime.Now;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //if (this.Tag.ToString() == "Satis")
            //{
            //    DB.ExecuteSQL("update Satislar set  Aciklama='" + memoozelnot .Text+ "' where pksatislar=" + memoozelnot.Tag.ToString());
            //}
            //if (this.Tag.ToString() == "Alis")
            //{
            //    DB.ExecuteSQL("update Alislar set  Aciklama='" + memoozelnot.Text + "' where pkAlislar=" + memoozelnot.Tag.ToString());
            //}
            this.Tag = "1";
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Now.AddDays(1);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime=DateTime.Now.AddDays(7);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            dateNavigator1.DateTime = DateTime.Now.AddDays(30);
        }

        private void dateNavigator1_DoubleClick(object sender, EventArgs e)
        {
            this.Tag = "1";
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            dtpSaat.Value = DateTime.Now;
        }

        private void dtpSaat_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                BtnKaydet_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                simpleButton21_Click(sender, e);
            }
            
        }

        private void frmTarihSec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
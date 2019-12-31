using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmAyrilmaNedeni : Form
    {
        public frmAyrilmaNedeni()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            deAyrilisTarihi.DateTime = DateTime.Today;
            AyrilmalarListesi();
        }
        void AyrilmalarListesi()
        {
            gridControl2.DataSource = DB.GetData("select * from PersonelAyrilma with(nolock) where fkPersoneller="+ pkpersoneller.Text);
        }
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (deAyrilisTarihi.EditValue == null)
            {
                MessageBox.Show("Ayrılma Tarihini Giriniz");
                return;
            }
            //string an = "";
            //an = AyrilmaNedeni.ayrilmanedeni.Text;
            //if (AyrilmaNedeni.TopMost == true) return;
            if (DevExpress.XtraEditors.XtraMessageBox.Show("İşten Çıkarıldığına Eminmisiniz?", "Personel Takip", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                System.Windows.Forms.DialogResult.Yes)
            {
                DB.ExecuteSQL("UPDATE Personeller SET AyrilisTarihi='" + Convert.ToDateTime(deAyrilisTarihi.EditValue).ToString("yyyy-MM-dd") + "',AyrilisNedeni='" +
                    ayrilmanedeni.Text+"' WHERE pkpersoneller=" + pkpersoneller.Text);
                DevExpress.XtraEditors.XtraMessageBox.Show("İşlem Tamamlandı.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
                deAyrilisTarihi.EditValue = null;

            #region ayrilma log 
            string sql = "insert into PersonelAyrilma (fkPersoneller,ayrilma_tarihi,ayrilma_nedeni,fkKullanicilar,kayit_tarihi)" +
                " values(@fkPersoneller,@ayrilma_tarihi,@ayrilma_nedeni,@fkKullanicilar,getdate())";

            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkPersoneller", pkpersoneller.Text));
            list.Add(new SqlParameter("@ayrilma_tarihi", deAyrilisTarihi.DateTime.ToString("yyyy-MM-dd")));
            list.Add(new SqlParameter("@ayrilma_nedeni", ayrilmanedeni.Text));
            list.Add(new SqlParameter("@fkKullanicilar", DB.fkKullanicilar));

            DB.ExecuteSQL(sql, list);
            AyrilmalarListesi();
            #endregion
        }

        private void pkpersoneller_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = DB.GetData("select * from Personeller with(nolock) where pkPersoneller=" + pkpersoneller.Text);

            label1.Text = pkpersoneller.Text + "-" + dt.Rows[0]["adi"].ToString() + " " + dt.Rows[0]["soyadi"].ToString();
            deAyrilisTarihi.EditValue = dt.Rows[0]["AyrilisTarihi"].ToString();
            ayrilmanedeni.Text = dt.Rows[0]["AyrilisNedeni"].ToString();
        }
    }
}

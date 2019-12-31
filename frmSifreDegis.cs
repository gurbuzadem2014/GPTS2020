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
    public partial class frmSifreDegis : DevExpress.XtraEditors.XtraForm
    {
        public frmSifreDegis()
        {
            InitializeComponent();
        }

        private void frmSifreDegis_Load(object sender, EventArgs e)
        {
            adi.Text = DB.kullaniciadi;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (adi.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Kullanıcı Adı Giriniz!", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (sifre.Text == "")
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Şifre Giriniz!", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            if (yenisifre.Text != yenisifreonay.Text)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Yeni Şifreniz Onaylanmadı.", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataTable dt = DB.GetData("select * from Kullanicilar where KullaniciAdi='" + adi.Text + "' and Sifre='" + sifre.Text + "'");
            if (dt.Rows.Count > 0)
            {
                //DB.kul = adi.Text;
                //DB.fkKullanicilar = dt.Rows[0]["pkKullanicilar"].ToString();
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("Sifre", yenisifre.Text));
                list.Add(new SqlParameter("KullaniciAdi", adi.Text));
                try
                {
                    DB.ExecuteSQL("update Kullanicilar set Sifre=@Sifre where KullaniciAdi=@KullaniciAdi", list);
                }
                catch (Exception exp)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Hata Oluştu Lütfen Sistem Yöneticinize Başvurun!", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DevExpress.XtraEditors.XtraMessageBox.Show("Şifreniz Değiştirildi.", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Information);    
                Close();
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Bilgiler Hatalı. \n Lütfen Kontrol ediniz!", "Personel Takip Progrmı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
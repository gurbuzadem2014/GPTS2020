using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Collections;

namespace GPTS
{
    public partial class frmGorusmeDetayBilgileri : DevExpress.XtraEditors.XtraForm
    {
        public frmGorusmeDetayBilgileri()
        {
            InitializeComponent();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            if (GorRanTarihi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Görüşme Randevu Tarihi Boş Olamaz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                GorRanTarihi.Focus();
                return;
            }
            if (GorYapKisi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Görüşme Yapılan Kişi Boş Olmaz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                GorYapKisi.Focus();
                return;
            }
            if (GorTarihi.EditValue == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Görüşme Tarihi Boş Olamaz!", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                GorTarihi.Focus();
                return;
            }
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@fkProjeler", DB.pkProjeler));
            list.Add(new SqlParameter("@GorusmeRandevuTarihi", GorRanTarihi.DateTime));
            if (GorTarihi.EditValue == null)
                list.Add(new SqlParameter("@GorusmeTarihi", DBNull.Value));
            else
                list.Add(new SqlParameter("@GorusmeTarihi", GorTarihi.DateTime));
            list.Add(new SqlParameter("@fkPersoneller", 1));
            list.Add(new SqlParameter("@GorusmeYapilacakKisi", GorYapKisi.Text));
            list.Add(new SqlParameter("@GorusmeYapilanKisininGorevi", GorYapKisiGorev.Text));
            list.Add(new SqlParameter("@GorusmeNotu", GorusmeNotu.Text));
            list.Add(new SqlParameter("@GorusmeSonucu", GorusmeSonucu.Text));
            string pkProjeler;
            if (pkProjeGorusme.Text == "0")
            {
                pkProjeGorusme.Text = DB.ExecuteScalarSQL("INSERT INTO ProjeGorusme (fkProjeler,GorusmeRandevuTarihi,GorusmeTarihi,fkPersoneller,GorusmeYapilacakKisi,GorusmeYapilanKisininGorevi,GorusmeNotu,GorusmeSonucu)" +
                 " values(@fkProjeler,@GorusmeRandevuTarihi,@GorusmeTarihi,@fkPersoneller,@GorusmeYapilacakKisi,@GorusmeYapilanKisininGorevi,@GorusmeNotu,@GorusmeSonucu) SELECT IDENT_CURRENT('ProjeGorusme')", list);
                //DB.pkProjeler = int.Parse(pkProjeler);
                DevExpress.XtraEditors.XtraMessageBox.Show("Görüşme Bilgileri Kaydedildi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                list.Add(new SqlParameter("@pkProjeGorusme", pkProjeGorusme.Text));
                DB.ExecuteSQL(@"UPDATE ProjeGorusme SET GorusmeRandevuTarihi=@GorusmeRandevuTarihi,
                GorusmeTarihi=@GorusmeTarihi,GorusmeYapilacakKisi=@GorusmeYapilacakKisi,GorusmeYapilanKisininGorevi=@GorusmeYapilanKisininGorevi,GorusmeNotu=@GorusmeNotu,
                GorusmeSonucu=@GorusmeSonucu where pkProjeGorusme=@pkProjeGorusme", list);
                DevExpress.XtraEditors.XtraMessageBox.Show("Görüşme Bilgileri Güncellendi.", "Personel Takip Sistemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void pkProjeGorusme_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
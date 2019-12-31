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
    public partial class frmStokKoduverKarti : DevExpress.XtraEditors.XtraForm
    {
        public frmStokKoduverKarti()
        {
            InitializeComponent();
        }

        private void frmStokKoduverKarti_Load(object sender, EventArgs e)
        {
            //pkKodver.Text = "";
            //pKart.Visible = true;
            tkod.Text = "";
            sRakam.EditValue = 0;
            tAciklama.Text = "";
            txtUreticiKodu.Text = "0000";
            txtUlkeKodu.Text = "869";

            BtnKaydet.Enabled = true;

            bilgileri();
        }
        void bilgileri()
        {
            DataTable dt = DB.GetData("select * from Kodver where pkKodver=" + pkKodver.Text);
            if (dt.Rows.Count > 0)
            {
                tkod.Text = dt.Rows[0]["Kodu"].ToString();
                sRakam.Text = dt.Rows[0]["Rakam"].ToString();
                seOtoBarkod.Text = dt.Rows[0]["stok_barkodu"].ToString();
                tAciklama.Text = dt.Rows[0]["Aciklama"].ToString();
                txtUreticiKodu.Text = dt.Rows[0]["uretici_barkodu"].ToString();
                txtUlkeKodu.Text = dt.Rows[0]["ulke_kodu"].ToString();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@Kodu", tkod.Text));
            list.Add(new SqlParameter("@Rakam", sRakam.Value));
            list.Add(new SqlParameter("@stok_barkodu", seOtoBarkod.Value));
            list.Add(new SqlParameter("@Aciklama", tAciklama.Text));
            list.Add(new SqlParameter("@uretici_barkodu", txtUreticiKodu.Text));
            list.Add(new SqlParameter("@ulke_kodu", txtUlkeKodu.Text));

            if (pkKodver.Text == "")
                DB.ExecuteSQL("INSERT INTO Kodver (Kodu,Rakam,stok_barkodu,Aciklama,uretici_barkodu,ulke_kodu) " +
                    "VALUES(@Kodu,@Rakam,@stok_barkodu,@Aciklama,@uretici_barkodu,@ulke_kodu)", list);
            else
            {
                list.Add(new SqlParameter("@pkKodver", pkKodver.Text));
                DB.ExecuteSQL(@"UPDATE Kodver SET Kodu=@Kodu,Rakam=@Rakam,
                stok_barkodu=@stok_barkodu,Aciklama=@Aciklama,
                uretici_barkodu=@uretici_barkodu,ulke_kodu=@ulke_kodu
                WHERE pkKodver=@pkKodver", list);
            }
            Close();
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmStokKoduverKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void tAciklama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tkod.Focus();
        }

        private void tkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sRakam.Focus();
        }

        private void sRakam_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                cbVarsayilan.Focus();
        }

        private void cbVarsayilan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnKaydet.Focus();
        }

        private void cbVarsayilan_CheckedChanged(object sender, EventArgs e)
        {
            DB.ExecuteSQL("UPDATE Kodver SET AktifSec=0");
            DB.ExecuteSQL("UPDATE Kodver SET AktifSec=1 WHERE pkKodver=" + pkKodver.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
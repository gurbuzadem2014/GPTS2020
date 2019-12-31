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
    public partial class frmMusteriAdresleri : DevExpress.XtraEditors.XtraForm
    {
        int _fkFirma;
        public frmMusteriAdresleri(int fkFirma)
        {
            InitializeComponent();
            _fkFirma = fkFirma;
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            DepolariGetir();
            AdresTipleri();
        }

        void DepolariGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from FirmaAdres with(nolock) where fkfirma=" + _fkFirma);
        }

        void AdresTipleri()
        {
            lueAdresTipleri.Properties.DataSource = DB.GetData("select pkSabitDegerDetay, kodu, deger, aciklama from AdresTipleri with(nolock)");
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //string pkDepolar = "0";
            string sonuc = "0", ak = "1";

            if (txtAdres.Tag.ToString() == "0")
            {
                if (!cbAktif.Checked)
                    ak = "0";

                sonuc = DB.ExecuteScalarSQL("INSERT INTO FirmaAdres (fkFirma,adres,adres_turu_id,Aktif) " +
                    " values("+ _fkFirma + ",'" + txtAdres.Text + "',0,"+ak+ ") select IDENT_CURRENT('FirmaAdres')");

                txtAdres.Tag = sonuc;
            }
            else
            {
                if (!cbAktif.Checked)
                    ak = "0";
                DB.ExecuteSQL("UPDATE FirmaAdres SET aktif=" + ak + ",adres='" + txtAdres.Text + "' WHERE pkFirmaAdres=" + txtAdres.Tag.ToString());
            }
            //pkDepolar= DB.GetData("select MAX(pkDepolar) from Depolar").Rows[0][0].ToString();
            //string sql = "INSERT INTO DepoHareketleri" +
            //    " SELECT " + pkDepolar + ", pkStokKarti,0,0,0 FROM StokKarti";
            // DB.ExecuteSQL(sql);

            temizle();
            DepolariGetir();
        }

        void AdresSil()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Adresi Silmek istediğinize Edilsin mi?", "hitit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkFirmaAdres = dr["pkFirmaAdres"].ToString();

            //if (DB.GetData("select * from DepoHareketleri where Mevcut>0 and fkDepolar=" + fkDepolar).Rows.Count > 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce Depo Transfer Kullanarak Silmek istediğiniz Depodaki Stokları Aktarınız!", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //select * from StokKartiDepo where fkDepolar=1 and fkStokKarti=1
            //DB.ExecuteSQL("DELETE FROM StokKartiDepo WHERE fkDepolar=" + fkDepolar);
            //DB.ExecuteSQL("DELETE FROM DepoHareketleri WHERE fkDepolar=" + fkDepolar);
            DB.ExecuteSQL("DELETE FROM FirmaAdres WHERE pkFirmaAdres=" + fkFirmaAdres);

            formislemleri.Mesajform("Adres Silindi.", "S", 200);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Depo Silindi.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string pkOda = dr["pkFirmaAdres"].ToString();

            //if (DB.GetData("select * from Firmalar with(nolock) where fkFirmaAdres = " + pkOda).Rows.Count>0)
            //{
            //    formislemleri.Mesajform("Hareket Gördüğü için silemezsiniz", "K", 150);
            //    return;
            //}
            AdresSil();
            DepolariGetir();
        }
        void temizle()
        {
            txtAdres.Text = "";
            txtAdres.Tag = "0";
            txtAdres.Focus();

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            txtAdres.Text = dr["adres"].ToString();
            txtAdres.Tag = dr["pkFirmaAdres"].ToString();

            if (dr["aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;
        }
    }
}

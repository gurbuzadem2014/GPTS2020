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
    public partial class frmDepoKarti : Form
    {
        public frmDepoKarti()
        {
            InitializeComponent();
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            SubeleriGetir();

            DepolariGetir();
        }

        void SubeleriGetir()
        {
            lueSubeler.Properties.DataSource = DB.GetData("select * from Subeler with(nolock)");
        }

        void DepolariGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from Depolar with(nolock)");
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //string pkDepolar = "0";
            string sonuc = "0";
            string aktif = "0";
            if (cbAktif.Checked) aktif = "1";

            if (DepoAdi.Tag.ToString() == "0")
            {
                sonuc = DB.ExecuteScalarSQL("INSERT INTO Depolar (DepoAdi,Aktif,fkSube) values('" +
                DepoAdi.Text + "',"+ aktif + "," + lueSubeler.EditValue.ToString() + ") select IDENT_CURRENT('Depolar')");
                DepoAdi.Tag = sonuc;
            }
            else
                DB.ExecuteSQL("UPDATE Depolar SET DepoAdi='" + DepoAdi.Text + "' " +
                    ",fkSube=" + lueSubeler.EditValue.ToString()+
                    ",Aktif="+ aktif +" WHERE pkDepolar=" + DepoAdi.Tag.ToString());
                //pkDepolar= DB.GetData("select MAX(pkDepolar) from Depolar").Rows[0][0].ToString();
            //string sql = "INSERT INTO DepoHareketleri" +
            //    " SELECT " + pkDepolar + ", pkStokKarti,0,0,0 FROM StokKarti";
           // DB.ExecuteSQL(sql);

            DepolariGetir();
        }

        void DepoSil()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Depo Kartını Pasif Yapmak istediğinize Edilsin mi?", "hitit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkDepolar = dr["pkDepolar"].ToString();

            //DB.ExecuteSQL("DELETE FROM StokKartiDepo WHERE fkDepolar=" + fkDepolar);
            //DB.ExecuteSQL("DELETE FROM DepoHareketleri WHERE fkDepolar=" + fkDepolar);
            //DB.ExecuteSQL("DELETE FROM Depolar WHERE pkDepolar=" + fkDepolar);
            DB.ExecuteSQL("update Depolar set aktif=0 WHERE pkDepolar=" + fkDepolar);

            formislemleri.Mesajform("Depo Pasif Yapıdı.", "S", 200);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Depo Silindi.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DepoSil();
            DepolariGetir();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DepoAdi.Text = "";
            DepoAdi.Tag = "0";
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DepoAdi.Text = dr["DepoAdi"].ToString();
            txtAciklama.Text = dr["aciklama"].ToString();
            DepoAdi.Tag = dr["pkDepolar"].ToString();


            int sube = 1;
            int.TryParse(dr["fkSube"].ToString(), out sube);

            lueSubeler.EditValue = sube;

            if (dr["aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;
        }

        private void lueSubeler_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl14_Click(object sender, EventArgs e)
        {
            frmSubeler subeler = new frmSubeler();
            subeler.ShowDialog();
        }
    }
}

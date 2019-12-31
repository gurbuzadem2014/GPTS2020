using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GPTS.islemler;
using System.Data.SqlClient;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmSubeler : DevExpress.XtraEditors.XtraForm
    {
        public frmSubeler()
        {
            InitializeComponent();
        }
        void Subeler()
        {
            gridControl1.DataSource = DB.GetData("select * from Subeler with(nolock)");
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            txtSubeAdi.Tag = "0";
            Subeler();
            DepolarSube(txtSubeAdi.Tag.ToString());
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list.Add(new SqlParameter("@sube_adi", txtSubeAdi.Text));
            list.Add(new SqlParameter("@aciklama", txtAciklama.Text));
            list.Add(new SqlParameter("@yetkili", txtYetkili.Text));
            

            if (cbAktif.Checked)
                list.Add(new SqlParameter("@aktif", "1"));
            else
                list.Add(new SqlParameter("@aktif", "0"));

            if (txtSubeAdi.Tag.ToString() == "0")
            {
                DB.ExecuteSQL("insert into Subeler (sube_adi,aciklama,yetkili,aktif) " +
                    "values(@sube_adi,@aciklama,@yetkili,@aktif)", list);
            }
            else
            {
                list.Add(new SqlParameter("@pkSube", txtSubeAdi.Tag.ToString()));
                DB.ExecuteSQL("update Subeler set sube_adi=@sube_adi,aciklama=@aciklama," +
                    "yetkili=@yetkili,aktif=@aktif where pkSube=@pkSube", list);
            }

            temizle();

            Subeler();
        }

        void SubeSil()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Şubeyi Pasif Yapmak İstediğinize Edilsin mi?", "Şube", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string pkSube = dr["pkSube"].ToString();
            DB.ExecuteSQL("update Subeler set aktif=0 WHERE pkSube=" + pkSube);
            //DB.ExecuteSQL("DELETE FROM Subeler WHERE pkSube=" + pkSube);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SubeSil();
            Subeler();
        }

        void temizle()
        {
            txtSubeAdi.Text = "";
            txtSubeAdi.Tag = "0";
            txtAciklama.Text = "";
            txtSubeAdi.Focus();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            txtSubeAdi.Text = dr["sube_adi"].ToString();
            txtSubeAdi.Tag = dr["pkSube"].ToString();
            txtAciklama.Text = dr["aciklama"].ToString();
            txtYetkili.Text = dr["yetkili"].ToString();
            

            if (dr["aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;

            DepolarSube(txtSubeAdi.Tag.ToString());
        }

        void DepolarSube(string subeid)
        {
            gridControl2.DataSource = DB.GetData("select * from Depolar with(nolock) where fkSube="+subeid);
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmDepoKarti depo = new frmDepoKarti();
            depo.ShowDialog();
        }

        private void frmSubeler_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}

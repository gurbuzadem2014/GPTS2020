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
    public partial class frmOdalar : DevExpress.XtraEditors.XtraForm
    {
        public frmOdalar()
        {
            InitializeComponent();
        }
        void DepolariGetir()
        {
            gridControl1.DataSource = DB.GetData("select * from Odalar with(nolock)");
        }

        private void frmDepoKarti_Load(object sender, EventArgs e)
        {
            DepolariGetir();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //string pkDepolar = "0";
            string sonuc = "0", ak = "1";

            if (DepoAdi.Tag.ToString() == "0")
            {
                if (!cbAktif.Checked)
                    ak = "0";
                sonuc = DB.ExecuteScalarSQL("INSERT INTO Odalar (oda_adi,Aktif) values('" + DepoAdi.Text + "',"+ak+") select IDENT_CURRENT('Odalar')");
                DepoAdi.Tag = sonuc;
            }
            else
            {
                if (!cbAktif.Checked)
                    ak = "0";
                DB.ExecuteSQL("UPDATE Odalar SET aktif=" + ak + ",oda_adi='" + DepoAdi.Text + "' WHERE pkOda=" + DepoAdi.Tag.ToString());
            }
            //pkDepolar= DB.GetData("select MAX(pkDepolar) from Depolar").Rows[0][0].ToString();
            //string sql = "INSERT INTO DepoHareketleri" +
            //    " SELECT " + pkDepolar + ", pkStokKarti,0,0,0 FROM StokKarti";
            // DB.ExecuteSQL(sql);

            temizle();
            DepolariGetir();
        }

        void DepoSil()
        {
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Oda Kartını Silmek istediğinize Edilsin mi?", "hitit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (secim == DialogResult.No) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string fkDepolar = dr["pkOda"].ToString();

            //if (DB.GetData("select * from DepoHareketleri where Mevcut>0 and fkDepolar=" + fkDepolar).Rows.Count > 0)
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("Lütfen Önce Depo Transfer Kullanarak Silmek istediğiniz Depodaki Stokları Aktarınız!", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //select * from StokKartiDepo where fkDepolar=1 and fkStokKarti=1
            //DB.ExecuteSQL("DELETE FROM StokKartiDepo WHERE fkDepolar=" + fkDepolar);
            //DB.ExecuteSQL("DELETE FROM DepoHareketleri WHERE fkDepolar=" + fkDepolar);
            DB.ExecuteSQL("DELETE FROM Odalar WHERE pkOda=" + fkDepolar);

            formislemleri.Mesajform("Oda Silindi.", "S", 200);
            //DevExpress.XtraEditors.XtraMessageBox.Show("Depo Silindi.", "hitit", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string pkOda = dr["pkOda"].ToString();

            if (DB.GetData("select * from Hatirlatma with(nolock) where fkOda = "+ pkOda).Rows.Count>0)
            {
                formislemleri.Mesajform("Hareket Gördüğü için silemezsiniz", "K", 150);
                return;
            }
            DepoSil();
            DepolariGetir();
        }
        void temizle()
        {
            DepoAdi.Text = "";
            DepoAdi.Tag = "0";
            DepoAdi.Focus();

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DepoAdi.Text = dr["oda_adi"].ToString();
            DepoAdi.Tag = dr["pkOda"].ToString();

            if (dr["aktif"].ToString() == "True")
                cbAktif.Checked = true;
            else
                cbAktif.Checked = false;
        }
    }
}

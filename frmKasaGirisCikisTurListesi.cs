using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmKasaGirisCikisTurListesi : Form
    {
        public frmKasaGirisCikisTurListesi()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            //DataTable dt = DB.GetData("select top 1 WebKullanir from dbo.Sirketler");
            string gc = "1";
            if (this.Tag.ToString() == "2") gc = "0";

            gridControl1.DataSource = DB.GetData(@"Select pkKasaGirisCikisTurleri,kgct.Aciklama,kgct.Aktif,kgct.GiderOlarakisle,kgct.GelirOlarakisle,g.GrupAdi from KasaGirisCikisTurleri kgct with(nolock) 
left join KasaGirisCikisGruplari g with(nolock) on g.pkKasaGirisCikisGruplari=kgct.fkKasaGirisCikisGruplari
where kgct.GirisCikis=" + gc);
        }
        private void frmStokGrupKarti_Load(object sender, EventArgs e)
        {
            if (this.Tag.ToString() == ((int)Degerler.GelirGider.Gelir).ToString())
            {
                gcGider.Visible = false;
                gcGelir.Visible = true;
            }
            else
            {
                gcGider.Visible = true;
                gcGelir.Visible = false;
            }

            gridyukle();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkKasaGirisCikisTurleri = dr["pkKasaGirisCikisTurleri"].ToString();

            if (DB.GetData("select fkKasaGirisCikisTurleri from KasaHareket where fkKasaGirisCikisTurleri=" + fkKasaGirisCikisTurleri).Rows.Count > 0)
            {
                MessageBox.Show("Bu Gruba ait Kasa Hareketi Bulunmaktadır.");
                return;
            }

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["Aciklama"].ToString() + " Grubunu Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("DELETE FROM KasaGirisCikisTurleri WHERE pkKasaGirisCikisTurleri=" + dr["pkKasaGirisCikisTurleri"].ToString());
            gridView1.DeleteSelectedRows();
        }


        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            yeniToolStripMenuItem_Click(sender, e);
        }

        private void sbtnSil_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            //if (gridView1.FocusedRowHandle < 0) return;

            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string fkKasaGirisCikisTurleri = dr["pkKasaGirisCikisTurleri"].ToString();

            frmKasaGirisCikisTurleri fKasaGirisCikisTurleri = new frmKasaGirisCikisTurleri("0");
            fKasaGirisCikisTurleri.Tag = this.Tag;
            fKasaGirisCikisTurleri.ShowDialog();
            gridyukle();
        }


        private void frmStokGrupKarti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkKasaGirisCikisTurleri = dr["pkKasaGirisCikisTurleri"].ToString();

            frmKasaGirisCikisTurleri fKasaGirisCikisTurleri = new frmKasaGirisCikisTurleri(fkKasaGirisCikisTurleri);
            fKasaGirisCikisTurleri.Tag = this.Tag;
            fKasaGirisCikisTurleri.ShowDialog();

            gridyukle();
        }
    }
}

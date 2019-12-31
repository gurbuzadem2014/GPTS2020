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
    public partial class frmKasaGirisCikisGrupListesi : Form
    {
        public frmKasaGirisCikisGrupListesi()
        {
            InitializeComponent();
        }
        void gridyukle()
        {
            //DataTable dt = DB.GetData("select top 1 WebKullanir from dbo.Sirketler");
            
            if (this.Tag.ToString() == "2") this.Tag = 0;

            string sql = "Select * from KasaGirisCikisGruplari with(nolock)";

            if (this.Tag.ToString() == "1")
                sql = sql + " where isnull(Gelir,1)=1";
            else
                sql = sql + " where isnull(Gider,1)=1";

            gridControl1.DataSource = DB.GetData(sql);
        }
        private void frmStokGrupKarti_Load(object sender, EventArgs e)
        {
            //0 gider
            //if (this.Tag.ToString() == "2")
            //{
            //    gcGider.Visible = true;
            //    gcGelir.Visible = false;
            //}
            //else
            //{
            //    gcGider.Visible = false;
            //    gcGelir.Visible = true;
            //}

            gridyukle();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //string fkKasaGirisCikisGruplari = dr["pkfkKasaGirisCikisGruplari"].ToString();

            //if (DB.GetData("select KasaGirisCikisGruplari from KasaHareket where fkKasaGirisCikisGruplari=" + fkKasaGirisCikisGruplari).Rows.Count > 0)
            //{
            //    MessageBox.Show("Bu Gruba ait Kasa Hareketi Bulunmaktadır.");
            //    return;
            //}

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["GrupAdi"].ToString() + " Grubunu Silmek İstediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No)
                return;

            DB.ExecuteSQL("DELETE FROM KasaGirisCikisGruplari WHERE pkKasaGirisCikisGruplari=" + dr["pkKasaGirisCikisGruplari"].ToString());
            gridView1.DeleteSelectedRows();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            kAYDETToolStripMenuItem_Click(sender,e);
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

            //string fkKasaGirisCikisGruplari = dr["pkKasaGirisCikisGruplari"].ToString();

            frmKasaGirisCikisGruplari KasaGirisCikisGruplari = new frmKasaGirisCikisGruplari("0");
            KasaGirisCikisGruplari.Tag = this.Tag;
            KasaGirisCikisGruplari.ShowDialog();
            gridyukle();
        }

        private void kAYDETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

            string fkKasaGirisCikisGruplari = dr["pkKasaGirisCikisGruplari"].ToString();

            frmKasaGirisCikisGruplari KasaGirisCikisGruplari = new frmKasaGirisCikisGruplari(fkKasaGirisCikisGruplari);
            KasaGirisCikisGruplari.Tag = this.Tag;
            KasaGirisCikisGruplari.ShowDialog();

            gridyukle();
        }

        private void gelirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkKasaGirisCikisGruplari = dr["pkKasaGirisCikisGruplari"].ToString();

            string sql = "update KasaGirisCikisGruplari set Gelir=1,Gider=0 where pkKasaGirisCikisGruplari=" +
                fkKasaGirisCikisGruplari;

            DB.ExecuteSQL(sql); 

            gridyukle();
        }

        private void giderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkKasaGirisCikisGruplari = dr["pkKasaGirisCikisGruplari"].ToString();

            string sql = "update KasaGirisCikisGruplari set Gelir=0,Gider=1 where pkKasaGirisCikisGruplari=" +
                fkKasaGirisCikisGruplari;
            
            DB.ExecuteSQL(sql);

            gridyukle();
        }
    }
}

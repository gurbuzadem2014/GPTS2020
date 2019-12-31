using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;
using GPTS.islemler;

namespace GPTS
{
    public partial class frmStokKartiDepoMevcut : DevExpress.XtraEditors.XtraForm
    {
        int _stok_id = 0;
        public frmStokKartiDepoMevcut(int stok_id)
        {
            InitializeComponent();
            _stok_id = stok_id;
            txtStokKarti_id.Text = _stok_id.ToString();
        }

        void Getir()
        {
            gridControl1.DataSource = DB.GetData(@"select * from StokKartiDepo skd with(nolock)
            left join Depolar d with(nolock) on d.pkDepolar=skd.fkDepolar
            where skd.fkStokKarti=" + txtStokKarti_id.Text);
        }
        private void frmStokKartiDepo_Load(object sender, EventArgs e)
        {
            //Getir();
        }


        private void stokKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;

            int i = gridView1.FocusedRowHandle;
            if (i < 0)
            {
                i = gridView1.DataRowCount;
                i--;
            }
            DataRow dr = gridView1.GetDataRow(i);

            frmStokKarti StokKarti = new frmStokKarti();

            DB.pkStokKarti = int.Parse(dr["fkStokKarti"].ToString());
            StokKarti.ShowDialog();

            StokBilgileri();
        }

        private void devirBakiyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;

            int i = gridView1.FocusedRowHandle;
            if (i < 0)
            {
                i = gridView1.DataRowCount;
                i--;
            }

            DataRow dr = gridView1.GetDataRow(i);
            //DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());

            frmStokDepoDevir StokDevir = new frmStokDepoDevir();
            StokDevir.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokDevir.ShowDialog();

            Getir();
        }

        private void txtStokKarti_id_TextChanged(object sender, EventArgs e)
        {
            Getir();
            StokBilgileri();
        }

        void StokBilgileri()
        {
            DataTable dt = DB.GetData(@"select * from StokKarti sk with(nolock)
            where sk.pkStokKarti=" + txtStokKarti_id.Text);
            if (dt.Rows.Count > 0)
            {
                AraAdindan.Text = dt.Rows[0]["Stokadi"].ToString();
                AraBarkodtan.Text = dt.Rows[0]["Barcode"].ToString();
            }
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            frmStokAra StokAra = new frmStokAra("");
            StokAra.Tag = "0";
            StokAra.ShowDialog();
            lueStoklar.Tag = "0";
            //Urunler();

            if (StokAra.TopMost == false)
            {
                for (int i = 0; i < StokAra.gridView1.SelectedRowsCount; i++)
                {
                    string v = StokAra.gridView1.GetSelectedRows().GetValue(i).ToString();

                    DataRow dr = StokAra.gridView1.GetDataRow(int.Parse(v));
                    string pkStokKarti = dr["pkStokKarti"].ToString();
                    //int sk = 0;
                    //int.TryParse(pkStokKarti, out sk);
                    //lueStoklar.EditValue = sk;
                    txtStokKarti_id.Text = pkStokKarti;
                }
            }
            StokAra.Dispose();
        }

        private void AraBarkodtan_KeyDown(object sender, KeyEventArgs e)
        {
            if (AraBarkodtan.Text != "" && e.KeyCode == Keys.Enter)
            {
                DataTable dt =
                DB.GetData("select * from StokKarti with(nolock) where Barcode='" + AraBarkodtan.Text +"'");

                if (dt.Rows.Count == 0)
                {
                    formislemleri.Mesajform("Ürün Bulunamadı", "K", 200);
                    txtStokKarti_id.Text = "0";
                    AraAdindan.Text = "";
                }
                else
                {
                    txtStokKarti_id.Text = dt.Rows[0]["pkStokKarti"].ToString();
                    AraAdindan.Text = dt.Rows[0]["Stokadi"].ToString();
                }
            }
        }
    }
}
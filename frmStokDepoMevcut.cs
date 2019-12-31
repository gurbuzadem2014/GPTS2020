using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GPTS.Include.Data;

namespace GPTS
{
    public partial class frmStokDepoMevcut : DevExpress.XtraEditors.XtraForm
    {
        public frmStokDepoMevcut()
        {
            InitializeComponent();
        }

        void Depolar()
        {
            lueDepolar.Properties.DataSource = DB.GetData("select * from Depolar with(nolock)");
            lueDepolar.EditValue = Degerler.fkDepolar;
        }

        void StokKartiDepo()
        {
            if (lueDepolar.EditValue != null)
            {
                string sql = @"select pkStokKartiDepo,sk.Barcode,sk.Stokadi,sk.Mevcut,skd.fkStokKarti,skd.MevcutAdet,skd.KritikAdet,skd.MinimumAdet,
                skd.ToplamGiren,skd.ToplamCikan,sk.AlisFiyati,sk.SatisFiyati from StokKartiDepo skd with(nolock)
                inner join StokKarti sk with(nolock) on skd.fkStokKarti=sk.pkStokKarti
                where skd.fkDepolar=" + lueDepolar.EditValue.ToString();

                if (cbGoster.Checked)
                   sql = sql + " and skd.MevcutAdet>0";

                gridControl1.DataSource = DB.GetData(sql);
            }
        }

        private void frmStokKartiDepo_Load(object sender, EventArgs e)
        {
            Depolar();
        }

        private void lueDepolar_EditValueChanged(object sender, EventArgs e)
        {
            StokKartiDepo();
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {
            frmDepoKarti depolar = new frmDepoKarti();
            depolar.Show();
        }

        private void cbGoster_CheckedChanged(object sender, EventArgs e)
        {
            StokKartiDepo();
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
        }

        private void stokHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokHareketleri StokHareketleri = new frmStokHareketleri();
            StokHareketleri.fkStokKarti.Text = dr["Stokadi"].ToString();
            StokHareketleri.fkStokKarti.Tag = dr["fkStokKarti"].ToString();
            StokHareketleri.ShowDialog();
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = gridView1.FocusedRowHandle;
            if (i < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            string fkStokKartiDepo = dr["pkStokKartiDepo"].ToString();           
            string fkStokKarti = dr["fkStokKarti"].ToString();
            string fkDepolar = lueDepolar.EditValue.ToString();

            frmStokkartiDepoDuzen StokkartiDepoDuzen = new frmStokkartiDepoDuzen(fkStokKartiDepo, fkDepolar,fkStokKarti);
            StokkartiDepoDuzen.Text = "Stok Depo Düzen "+ lueDepolar.Text+"-" + dr["Stokadi"].ToString();
            StokkartiDepoDuzen.ShowDialog();

            StokKartiDepo();

            gridView1.FocusedRowHandle=i;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                //yesilisikyeni();
                return;
            }
            else
                MusteriSonSatislari(e.RowHandle);
        }

        void MusteriSonSatislari(int RowHandle)
        {
            DataRow dr = gridView1.GetDataRow(RowHandle);

            string sql = "";

            sql = "select pkStokKartiDepo,DepoAdi,skd.fkDepolar,skd.MevcutAdet,skd.KritikAdet from StokKartiDepo skd with(nolock) left join Depolar d with(nolock)  on d.pkDepolar=skd.fkDepolar where fkStokKarti=" + dr["fkStokKarti"].ToString();

            gridControl2.DataSource = DB.GetData(sql);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string dosyaadi = Application.StartupPath + "\\StokDepoMevcutListe"+ lueDepolar.Text+ ".Xls";
            gridControl1.ExportToXls(dosyaadi);
            System.Diagnostics.Process.Start(dosyaadi);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmPersonelAra : Form
    {
        public frmPersonelAra()
        {
            InitializeComponent();
        }
        private void frmMusteriAra_Load(object sender, EventArgs e)
        {
            musteriara();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
             simpleButton2_Click(sender, e);
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            //DB.FirmaAdi = dr["Firmaadi"].ToString();
            //Close();
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(sender, e);
            }
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            DB.FirmaAdi = dr["Firmaadi"].ToString();
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            fkFirma.AccessibleDescription = dr["Firmaadi"].ToString();
            fkFirma.Tag = dr["pkFirma"].ToString();
            DB.ExecuteSQL("update Firmalar set tiklamaadedi=tiklamaadedi+1 where pkFirma=" + dr["pkFirma"].ToString());
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //fkFirma.Tag = "1";
            //fkFirma.AccessibleDescription = "Peşin Müşteri";
            Close();
        }

        private void AraAdindan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            AraBarkodtan.Text = "";
            if (e.KeyCode == Keys.Left && AraAdindan.Text == "")
            {
                AraBarkodtan.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
                gridView1.FocusedRowHandle = 1;
            }
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton2_Click(sender, e);
                //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //DB.FirmaAdi = dr["Firmaadi"].ToString();
                //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
                //Close();
            }
        }

        private void AraAdindan_KeyUp(object sender, KeyEventArgs e)
        {
            musteriara();
        }

        private void frmMusteriAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            else if (e.KeyCode == Keys.F7)
                btnYeni_Click(sender, e);
            else if (e.KeyCode == Keys.F9)
                simpleButton2_Click(sender, e);
        }

        private void AraBarkodtan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
            AraAdindan.Text = "";
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DB.FirmaAdi = dr["Firmaadi"].ToString();
                DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
                Close();
            }
            if (e.KeyCode == Keys.Right)
                AraAdindan.Focus();
        }
        void musteriara()
        {
            string sql = "exec musteriara_sp '" + AraBarkodtan.Text + "','" + AraAdindan.Text + "','1'";
            sql = "SELECT * FROM Personeller";
            gridControl1.DataSource = DB.GetData(sql);
        }
        private void AraBarkodtan_KeyUp(object sender, KeyEventArgs e)
        {
            musteriara();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //fkFirma.AccessibleDescription=dr["Firmaadi"].ToString();
            //fkFirma.Tag = dr["pkFirma"].ToString();
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmMusteriKarti MusteriKarti = new frmMusteriKarti("0", "");
            DB.PkFirma = 0;
            MusteriKarti.ShowDialog();
            musteriara();
        }
    }
}

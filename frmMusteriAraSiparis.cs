using GPTS.islemler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmMusteriAraSiparis : Form
    {
        public frmMusteriAraSiparis()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        private void frmMusteriAra_Load(object sender, EventArgs e)
        {
            //DataTable dt = DB.GetData("select top 1 * from Sirketler");
            //if (dt.Rows.Count>0)
            //{
            //    if (dt.Rows[0]["BonusYuzde"].ToString()=="0")
            //    gcBonus.Visible = false;
            //}
            musteriara();

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimYukle(gridView1, "MusteriAraSiparisGrid");
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
            if (gridView1.FocusedRowHandle < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            DB.FirmaAdi = dr["Firmaadi"].ToString();
            //DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            fkFirma.AccessibleDescription = dr["Firmaadi"].ToString();
            fkFirma.Tag = dr["pkFirma"].ToString();

            DB.ExecuteSQL("update Firmalar set tiklamaadedi=tiklamaadedi+1 where pkFirma=" + dr["pkFirma"].ToString());
            this.Tag = "1";
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Tag = "0";
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
                simpleButton2_Click(sender, e);//tamam
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
                simpleButton2_Click(sender, e);//tamam
            }
            if (e.KeyCode == Keys.Right)
                AraAdindan.Focus();
        }
        void musteriara()
        {
            string sql = "";
            if (AraBarkodtan.Text == "")
                sql = @"SELECT convert(bit,'0') as Sec,f.pkFirma, f.Firmaadi, fg.GrupAdi,
                 f.OzelKod,f.Cep,f.Cep2,f.Adres,f.Devir FROM Firmalar f with(nolock)
                LEFT JOIN FirmaGruplari fg with(nolock) ON f.fkFirmaGruplari = fg.pkFirmaGruplari 
                 where pkFirma>1 and f.Aktif=1 and f.Firmaadi like '%" +
                     AraAdindan.Text + "%'";
            else
                sql = @"SELECT convert(bit,'0') as Sec,f.pkFirma, f.Firmaadi, fg.GrupAdi,
                 f.OzelKod,f.Cep,f.Cep2,f.Adres,f.Devir FROM Firmalar f with(nolock)
                LEFT JOIN FirmaGruplari fg with(nolock) ON f.fkFirmaGruplari = fg.pkFirmaGruplari 
                 where f.pkFirma>1 and f.Aktif=1 and f.OzelKod ='" + AraBarkodtan.Text + "'";

            //if (!checkEdit1.Checked)
            //    sql = sql + " order by tiklamaadedi desc";
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

        private void AraAdindan_Leave(object sender, EventArgs e)
        {
            AraAdindan.BackColor=System.Drawing.Color.White;
        }

        private void AraAdindan_Enter(object sender, EventArgs e)
        {
            AraAdindan.BackColor = System.Drawing.Color.GreenYellow;
        }

        private void AraBarkodtan_Leave(object sender, EventArgs e)
        {
            AraAdindan.BackColor = System.Drawing.Color.White;
        }

        private void AraBarkodtan_Enter(object sender, EventArgs e)
        {
            AraAdindan.BackColor = System.Drawing.Color.GreenYellow;
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            musteriara();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                gridView1.SetRowCellValue(i, "Sec", checkEdit2.Checked);
            }
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = gridView1.GetDataRow(e.RowHandle);
                if (dr["Sec"].ToString() == "True")
                {
                    e.Appearance.BackColor = Color.SlateBlue;

                    e.Appearance.ForeColor = Color.White;
                }
            } 
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView1.DataRowCount < 1000)
            {
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    gridView1.SetRowCellValue(i, "Sec", false);
                }
            }
            if (gridView1.SelectedRowsCount == 1)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dr["Sec"].ToString() == "True")
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", false);
                else
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Sec", true);
                return;
            }
            for (int i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                int si = gridView1.GetSelectedRows()[i];
                DataRow dr = gridView1.GetDataRow(si);
                gridView1.SetRowCellValue(si, "Sec", true);
            }
        }

        private void müşteriKartıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriKarti KurumKarti = new frmMusteriKarti(dr["pkFirma"].ToString(), "");
            KurumKarti.ShowDialog();

            musteriara();
        }

        private void müşteriHareketleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //DB.PkFirma = int.Parse(dr["pkFirma"].ToString());
            frmMusteriHareketleri CariHareketMusteri = new frmMusteriHareketleri();
            CariHareketMusteri.musteriadi.Tag = dr["pkFirma"].ToString();
            CariHareketMusteri.Show();
        }

        private void sütunSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            gridView1.ShowCustomization();
            gridView1.OptionsBehavior.AutoPopulateColumns = true;
            gridView1.OptionsCustomization.AllowColumnMoving = true;
            gridView1.OptionsCustomization.AllowColumnResizing = true;
            gridView1.OptionsCustomization.AllowQuickHideColumns = true;
            gridView1.OptionsCustomization.AllowRowSizing = true;
        }

        private void varsayılanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimSil("MusteriAraSiparisGrid");
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!formislemleri.SifreIste()) return;

            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridTasarimKaydet(gridView1, "MusteriAraSiparisGrid");
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "A4");
        }

        private void AraAdindan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

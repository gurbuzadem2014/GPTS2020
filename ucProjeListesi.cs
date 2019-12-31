using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;

namespace GPTS
{
    public partial class ucProjeListesi : DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable dt;
        public ucProjeListesi()
        {
            InitializeComponent();
            projelistesigetir();
        }

        private void projelistesigetir()
        {
          gcProjeler.DataSource = DB.GetData(@"select * from Projeler");
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show(dr["pkProjeler"].ToString() + " Nolu Proje Silinsin mi?", "Personel Takip Sistemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;
            DB.ExecuteSQL("delete from Projeler where pkProjeler=" + dr["pkProjeler"].ToString());
            DB.ExecuteSQL(@"update Personeller set fkgrup=0 from ProjePersonel 
            where ProjePersonel.fkPersonel=Personeller.pkpersoneller and ProjePersonel.fkProjeler="+ dr["pkProjeler"].ToString());
            DB.ExecuteSQL("delete from ProjePersonel where fkProjeler=" + dr["pkProjeler"].ToString());
            DevExpress.XtraEditors.XtraMessageBox.Show("Proje Silindi.", "Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gridView1.DeleteSelectedRows();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmProjeKarti ProjeKarti = new frmProjeKarti();
            DB.pkProjeler = 0;
            ProjeKarti.ShowDialog(); 
            projelistesigetir();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow row = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.pkProjeler = int.Parse(row["pkProjeler"].ToString());
            frmProjeKarti ProjeKarti = new frmProjeKarti();
            ProjeKarti.ShowDialog();
            projelistesigetir();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            projelistesigetir();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            //this.Dispose();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            frmProjeSatis ProjeSatis = new frmProjeSatis();
            //ProjeSatis.pkProjeler.Text=
            ProjeSatis.ShowDialog();
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow row = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.pkProjeler = int.Parse(row["pkProjeler"].ToString());
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (gridView2.FocusedRowHandle < 0) return;
            DataRow row = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            DB.pkProjeler = int.Parse(row["pkProjeler"].ToString());
            frmProjeKarti ProjeKarti = new frmProjeKarti();
            ProjeKarti.Tag = "1";
            ProjeKarti.ShowDialog();
            projelistesigetir();
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == xtraTabPage2)
                gridControl1.DataSource = DB.GetData(@"SELECT ProjeGorev.pkProjeGorev, ProjeGorev.fkProjeler, 
ProjeGorev.gorev_adi, 
ProjeGorev.gorev_aciklama, ProjeGorev.oncelik, ProjeGorev.bas_tarih, 
                      ProjeGorev.bitis_tarih, ProjeGorev.mail_yolla, 
                      Projeler.Aciklama as proje_aciklama, 
                      Personeller.adi+' '+ Personeller.soyadi as fkPersoneller_gorev_giren
FROM         ProjeGorev 
LEFT JOIN  Projeler ON ProjeGorev.fkProjeler = Projeler.pkProjeler 
LEFT JOIN Personeller ON ProjeGorev.fkPersoneller_gorev_giren = Personeller.pkpersoneller");
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmProjeGorevGirisi GorevGirisi = new frmProjeGorevGirisi();
            GorevGirisi.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using System.IO;
using GPTS.islemler;
using System.Collections;
using System.Data.SqlClient;

namespace GPTS
{
    public partial class frmStokSatisFiyatlari : DevExpress.XtraEditors.XtraForm
    {
        public frmStokSatisFiyatlari()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 50;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
        }
        private void frmEnvanter_Load(object sender, EventArgs e)
        {
            gridyukle();
        }
        void gridyukle()
        {
            string sql = @"
select 
sk.pkStokKarti,
sk.Stokadi,
sk.Barcode,
sk.SatisFiyati,
sk.AlisFiyati,
N.SatisFiyatiKdvli as SatisFiyatiN,
K.SatisFiyatiKdvli as SatisFiyatiK,
N.Aktif as AktifN,
K.Aktif as AktifK

from StokKarti sk with(nolock)
left join SatisFiyatlariN N on  N.Aktif=1 and N.fkStokKarti=sk.pkStokKarti
left join SatisFiyatlariK K on K.Aktif=1 and K.fkStokKarti=sk.pkStokKarti
 where 1=1";

            if (cbFarkli.Checked)
                sql = sql + " and N.SatisFiyatiKdvli<>K.SatisFiyatiKdvli";

            gridControl1.DataSource = DB.GetData(sql);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int secilen = gridView1.FocusedRowHandle;
            if (secilen < 0) return;

            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            frmStokKarti StokKarti = new frmStokKarti();
            DB.pkStokKarti = int.Parse(dr["pkStokKarti"].ToString());
            StokKarti.ShowDialog();

            gridyukle();

            gridView1.FocusedRowHandle = secilen;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string exedizini = Path.GetDirectoryName(Application.ExecutablePath);
            string guid = Guid.NewGuid().ToString();
            gridControl1.ExportToXls(exedizini + "/" + guid + ".xls");
            Process.Start(exedizini + "/" + guid + ".xls");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //formislemleri.Mesajform("OK", "S");
            Yazdirma_islemleri yi = new Yazdirma_islemleri();
            yi.GridYazdir(gridControl1, "A4");
        }

        private void frmEnvanter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void gridView1_EndSorting(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount > 0)
                gridView1.FocusedRowHandle = 0;
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if(e.Page==xtraTabPage1)
            //    gridyukle();
        }

        private void cbFarkli_CheckedChanged(object sender, EventArgs e)
        {
            gridyukle();
        }       
    }
}
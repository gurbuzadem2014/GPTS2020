﻿using System;
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
    public partial class frmMasaGruplari : DevExpress.XtraEditors.XtraForm
    {
        public frmMasaGruplari()
        {
            InitializeComponent();
        }
        void yukle()
        {
            gridControl1.DataSource =
            DB.GetData("select * from MasaGruplari with(nolock)");
        }
        private void frmHizliGruplar_Load(object sender, EventArgs e)
        {
            yukle();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
            
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                DataRow dr = gridView1.GetDataRow(i);
                string sira = dr["sira_no"].ToString();
                if (sira == "") sira = "1";
                string sql = "UPDATE MasaGruplari SET masa_grup_adi='" +
                    dr["masa_grup_adi"].ToString() + 
                    "',sira_no=" + sira;

                if (dr["aktif"].ToString() == "True")
                    sql = sql + ",aktif=1";
                else
                    sql = sql + ",aktif=0";
                    sql=sql+ " WHERE pkMasaGruplari=" + dr["pkMasaGruplari"].ToString();
                DB.ExecuteSQL(sql);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            inputForm sifregir = new inputForm();
            sifregir.Text = "Yeni Grup Adı";
            sifregir.GirilenCaption.Text = "Yeni Grup Adı Girniz";
            //sifregir.Girilen.Properties.PasswordChar = '*';
            sifregir.ShowDialog();
            if (sifregir.Girilen.Text == "") return;

            string yeniid=DB.ExecuteScalarSQL("insert into MasaGruplari (masa_grup_adi,aktif,sira_no)" +
                " values('" + sifregir.Girilen.Text + "',1,1) SELECT IDENT_CURRENT('MasaGruplari')");

            //if (yeniid != "")
            //{
            //    string maxpkHizliGruplar = DB.GetData("SELECT Min(pkHizliGruplar) from HizliGruplar with(nolock)").Rows[0][0].ToString();
            //    DB.ExecuteSQL("insert into HizliStokSatis" +
            //        " SELECT 0 ," + yeniid + " FROM HizliStokSatis  where fkHizliGruplar=" + maxpkHizliGruplar);
            //}
            yukle();
        }

        private void sİLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridView1.FocusedRowHandle<0) return;

            if (gridView1.DataRowCount == 1) return;

            DialogResult secim;
            secim = DevExpress.XtraEditors.XtraMessageBox.Show("Grubunu Silmek istediğinize Eminmisiniz.", Degerler.mesajbaslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.No) return;

                 DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            
            //if (dr["pkHizliGruplar"].ToString() == "1") return;

                 //DB.ExecuteSQL("delete from HizliStokSatis where fkHizliGruplar=" + dr["pkHizliGruplar"].ToString());
                 DB.ExecuteSQL("delete from MasaGruplari where pkMasaGruplari=" + dr["pkMasaGruplari"].ToString());

                 yukle();
        }
    }
}